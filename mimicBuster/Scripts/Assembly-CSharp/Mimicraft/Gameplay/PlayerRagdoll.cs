using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerRagdoll : NetworkBehaviour
	{
		[Tooltip("The humanoid model whose bones go limp. Resolved from PlayerCameraRig when left empty, so the reference only has to exist in one place on the prefab.")]
		[SerializeField]
		private GameObject characterModel;

		[Tooltip("The humanoid's Animator. Switched off while the bones are loose - an animator writing poses and physics writing poses is a fight the animator always wins.")]
		[SerializeField]
		private Animator animator;

		[Tooltip("The hips (or whichever bone the ragdoll settles around). Used to bring the player's capsule to where the body actually came to rest before it stands up. Optional - without it a player stands up where they fell over instead of where they landed.")]
		[SerializeField]
		private Transform hips;

		[Tooltip("Yüzüstü düşen bir bedenin kalkış trigger'ı.")]
		[SerializeField]
		private string standUpFrontTrigger = "StandUpFront";

		[Tooltip("Yüzüstü kalkış STATE'inin adı. Sadece klibin bittiğini anlamak için kullanılır; tutmazsa aşağıdaki zaman aşımı devreye girer.")]
		[SerializeField]
		private string standUpFrontState = "StandUpFront";

		[Tooltip("Sırtüstü düşen bir bedenin kalkış trigger'ı.")]
		[SerializeField]
		private string standUpBackTrigger = "StandUpBack";

		[Tooltip("Sırtüstü kalkış STATE'inin adı.")]
		[SerializeField]
		private string standUpBackState = "StandUpBack";

		[Tooltip("How long the body stays loose before it starts getting up.")]
		[SerializeField]
		private float limpSeconds = 3f;

		[Tooltip("Ceiling on the get-up. Reached only when the animator has no such state, or its clip loops - without it a mistyped name would leave a player frozen on the floor forever.")]
		[SerializeField]
		private float standUpTimeoutSeconds = 4f;

		[Tooltip("What the body is dropped onto when looking for the ground under the hips.")]
		[SerializeField]
		private LayerMask groundLayers = -1;

		private readonly NetworkVariable<RagdollState> state = new NetworkVariable<RagdollState>(RagdollState.None);

		private PlayerMovement movement;

		private CharacterController characterController;

		private PlayerAnimator playerAnimator;

		private readonly List<Rigidbody> bones = new List<Rigidbody>();

		private readonly List<Collider> boneColliders = new List<Collider>();

		private bool bonesCached;

		private double serverStateDeadline;

		private float standUpStartedAt;

		private bool warnedAboutTrigger;

		private Vector3 pendingImpulse;

		private int pendingNudgeBone = -1;

		private Vector3 pendingNudgePoint;

		private Vector3 pendingNudge;

		private const float MaxNudgesPerFrame = 3f;

		private int queuedNudgeBone = -1;

		private Vector3 queuedNudgePoint;

		private Vector3 queuedNudge;

		private bool nudgeLoggedThisLimp;

		public const float ShoveImmunitySeconds = 3f;

		private bool serverShoved;

		private double serverShoveImmuneUntil;

		private const float VelocitySmoothing = 20f;

		private Vector3 rootVelocity;

		private Vector3 velocitySamplePosition;

		private bool hasVelocitySample;

		private RagdollState previousApplied;

		private bool detachedFromCapsule;

		private string activeStandUpState;

		private Vector3[] bonePosePositions;

		private Quaternion[] bonePoseRotations;

		private bool bonePosesCaptured;

		private static readonly Vector3[] FacingCandidates = new Vector3[6]
		{
			Vector3.forward,
			Vector3.back,
			Vector3.up,
			Vector3.down,
			Vector3.right,
			Vector3.left
		};

		private Vector3 hipsFacingAxis = Vector3.forward;

		private bool facingAxisCalibrated;

		public RagdollState State => state.Value;

		public bool IsLimp
		{
			get
			{
				if (state.Value != RagdollState.Limp)
				{
					return state.Value == RagdollState.Dead;
				}
				return true;
			}
		}

		public bool HasHumanoid
		{
			get
			{
				if (characterModel != null)
				{
					return characterModel.activeInHierarchy;
				}
				return false;
			}
		}

		public bool CanRagdoll
		{
			get
			{
				if (HasHumanoid)
				{
					return Bones.Count > 0;
				}
				return false;
			}
		}

		private List<Rigidbody> Bones
		{
			get
			{
				CacheBones();
				return bones;
			}
		}

		public IReadOnlyList<Rigidbody> BoneBodies => Bones;

		public bool ServerShoveImmune => Time.timeAsDouble < serverShoveImmuneUntil;

		public Vector3 BodyPosition
		{
			get
			{
				if (!IsLimp || !(hips != null))
				{
					return base.transform.position;
				}
				return hips.position;
			}
		}

		public override void OnNetworkSpawn()
		{
			movement = GetComponent<PlayerMovement>();
			characterController = GetComponent<CharacterController>();
			playerAnimator = GetComponentInChildren<PlayerAnimator>(includeInactive: true);
			if (characterModel == null)
			{
				PlayerCameraRig component = GetComponent<PlayerCameraRig>();
				characterModel = ((component != null) ? component.CharacterModel : null);
			}
			if (animator == null && characterModel != null)
			{
				animator = characterModel.GetComponentInChildren<Animator>(includeInactive: true);
			}
			NetworkVariable<RagdollState> networkVariable = state;
			networkVariable.OnValueChanged = (NetworkVariable<RagdollState>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<RagdollState>.OnValueChangedDelegate(OnStateChanged));
			CacheBones();
			Apply(state.Value);
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<RagdollState> networkVariable = state;
			networkVariable.OnValueChanged = (NetworkVariable<RagdollState>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<RagdollState>.OnValueChangedDelegate(OnStateChanged));
		}

		private void CacheBones()
		{
			if (bonesCached || characterModel == null)
			{
				return;
			}
			bonesCached = true;
			characterModel.GetComponentsInChildren(includeInactive: true, bones);
			foreach (Rigidbody bone in bones)
			{
				Collider component = bone.GetComponent<Collider>();
				if (component != null)
				{
					boneColliders.Add(component);
				}
			}
			SetBonesSimulated(simulated: false);
		}

		public bool ServerBeginDeathRagdoll()
		{
			if (!base.IsServer || !CanRagdoll || state.Value == RagdollState.Dead)
			{
				return false;
			}
			state.Value = RagdollState.Dead;
			serverStateDeadline = 0.0;
			return true;
		}

		public bool ServerBeginRecoverableRagdoll()
		{
			if (!base.IsServer || state.Value != RagdollState.None || !CanRagdoll)
			{
				return false;
			}
			state.Value = RagdollState.Limp;
			serverStateDeadline = Time.timeAsDouble + (double)limpSeconds;
			return true;
		}

		public bool ServerBeginKnockdown(Vector3 impulse)
		{
			if (!ServerBeginRecoverableRagdoll())
			{
				return false;
			}
			KnockdownImpulseClientRpc(impulse);
			return true;
		}

		[ClientRpc]
		private void KnockdownImpulseClientRpc(Vector3 impulse)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(3848969826u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in impulse);
				__endSendClientRpc(ref bufferWriter, 3848969826u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				pendingImpulse = impulse;
				if (IsLimp)
				{
					ApplyPendingImpulse();
				}
			}
		}

		public void ServerNudge(Rigidbody bone, Vector3 point, Vector3 velocityChange)
		{
			if (!base.IsServer || !IsLimp || bone == null)
			{
				return;
			}
			int num = Bones.IndexOf(bone);
			if (num >= 0)
			{
				if (Debug.isDebugBuild && !nudgeLoggedThisLimp)
				{
					nudgeLoggedThisLimp = true;
					Debug.Log("[PlayerRagdoll] " + base.name + ": yerdeki bedene atis itmesi gonderildi - kemik '" + bone.name + "', " + $"durum {state.Value}, hiz {velocityChange.magnitude:0.00} m/sn.", this);
				}
				if (pendingNudgeBone < 0)
				{
					pendingNudgeBone = num;
					pendingNudgePoint = point;
				}
				pendingNudge = Vector3.ClampMagnitude(pendingNudge + velocityChange, velocityChange.magnitude * 3f);
			}
		}

		private void LateUpdate()
		{
			if (base.IsServer && pendingNudgeBone >= 0)
			{
				NudgeClientRpc(pendingNudgeBone, pendingNudgePoint, pendingNudge);
				pendingNudgeBone = -1;
				pendingNudge = Vector3.zero;
			}
		}

		[ClientRpc]
		private void NudgeClientRpc(int boneIndex, Vector3 point, Vector3 velocityChange)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(770797621u, clientRpcParams, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, boneIndex);
				bufferWriter.WriteValueSafe(in point);
				bufferWriter.WriteValueSafe(in velocityChange);
				__endSendClientRpc(ref bufferWriter, 770797621u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsClient && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (boneIndex >= 0 && boneIndex < Bones.Count)
			{
				if (!IsLimp)
				{
					queuedNudgeBone = boneIndex;
					queuedNudgePoint = point;
					queuedNudge = velocityChange;
				}
				else
				{
					ApplyNudge(boneIndex, point, velocityChange);
				}
			}
		}

		private void ApplyNudge(int boneIndex, Vector3 point, Vector3 velocityChange)
		{
			Rigidbody rigidbody = Bones[boneIndex];
			if (!(rigidbody == null) && !rigidbody.isKinematic)
			{
				rigidbody.AddForceAtPosition(velocityChange * rigidbody.mass, point, ForceMode.Impulse);
			}
		}

		private void ApplyQueuedNudge()
		{
			if (queuedNudgeBone < 0 || queuedNudgeBone >= bones.Count)
			{
				queuedNudgeBone = -1;
				return;
			}
			ApplyNudge(queuedNudgeBone, queuedNudgePoint, queuedNudge);
			queuedNudgeBone = -1;
		}

		public void ServerNudgeNearest(Vector3 point, Vector3 velocityChange)
		{
			if (!base.IsServer || !IsLimp)
			{
				return;
			}
			Rigidbody rigidbody = null;
			float num = float.MaxValue;
			foreach (Rigidbody bone in Bones)
			{
				if (!(bone == null))
				{
					float sqrMagnitude = (bone.worldCenterOfMass - point).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						rigidbody = bone;
					}
				}
			}
			if (rigidbody != null)
			{
				ServerNudge(rigidbody, point, velocityChange);
			}
		}

		private void ApplyPendingImpulse()
		{
			if (pendingImpulse.sqrMagnitude < 0.0001f || bones.Count == 0)
			{
				return;
			}
			float y = base.transform.position.y;
			float num = ((characterController != null) ? Mathf.Max(characterController.height, 0.1f) : 1.8f);
			foreach (Rigidbody bone in bones)
			{
				if (!(bone == null) && !bone.isKinematic)
				{
					float t = Mathf.Clamp01((bone.worldCenterOfMass.y - y) / num);
					bone.AddForce(pendingImpulse * Mathf.Lerp(0.35f, 1f, t), ForceMode.VelocityChange);
				}
			}
			pendingImpulse = Vector3.zero;
		}

		public bool ServerBeginShove(Vector3 impulse)
		{
			if (!base.IsServer || ServerShoveImmune || !ServerBeginKnockdown(impulse))
			{
				return false;
			}
			serverShoved = true;
			return true;
		}

		private void ServerFinishStandUp()
		{
			state.Value = RagdollState.None;
			serverStateDeadline = 0.0;
			if (serverShoved)
			{
				serverShoved = false;
				serverShoveImmuneUntil = Time.timeAsDouble + 3.0;
			}
		}

		public void ServerClearRagdoll()
		{
			if (base.IsServer && state.Value != RagdollState.None)
			{
				state.Value = RagdollState.None;
				serverShoved = false;
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void RequestRagdollServerRpc(RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(504955165u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 504955165u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (rpcParams.Receive.SenderClientId == base.OwnerClientId)
				{
					ServerBeginRecoverableRagdoll();
				}
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void ReportStandUpFinishedServerRpc(RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(2903118628u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 2903118628u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (rpcParams.Receive.SenderClientId == base.OwnerClientId && state.Value == RagdollState.StandingUp)
				{
					ServerFinishStandUp();
				}
			}
		}

		private void Update()
		{
			TrackRootVelocity();
			if (base.IsServer)
			{
				TickServerTimers();
			}
			if (base.IsOwner)
			{
				TickOwner();
			}
		}

		private void TrackRootVelocity()
		{
			Vector3 position = base.transform.position;
			if (!hasVelocitySample)
			{
				hasVelocitySample = true;
				velocitySamplePosition = position;
				return;
			}
			if (Time.deltaTime > 0f)
			{
				Vector3 b = (position - velocitySamplePosition) / Time.deltaTime;
				rootVelocity = Vector3.Lerp(rootVelocity, b, 1f - Mathf.Exp(-20f * Time.deltaTime));
			}
			velocitySamplePosition = position;
		}

		private void TickServerTimers()
		{
			if (!(serverStateDeadline <= 0.0) && !(Time.timeAsDouble < serverStateDeadline))
			{
				switch (state.Value)
				{
				case RagdollState.Limp:
					state.Value = RagdollState.StandingUp;
					serverStateDeadline = Time.timeAsDouble + (double)standUpTimeoutSeconds;
					break;
				case RagdollState.StandingUp:
					ServerFinishStandUp();
					break;
				default:
					serverStateDeadline = 0.0;
					break;
				}
			}
		}

		private void TickOwner()
		{
			if (state.Value == RagdollState.StandingUp)
			{
				if (HasStandUpFinished())
				{
					ReportStandUpFinishedServerRpc();
				}
			}
			else if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame && !GameMenuState.InputCaptured && state.Value == RagdollState.None && CanRagdoll && (!(movement != null) || (movement.InputEnabled && !movement.SpectatorFrozen)))
			{
				RequestRagdollServerRpc();
			}
		}

		private bool HasStandUpFinished()
		{
			if (animator == null || !animator.enabled || animator.runtimeAnimatorController == null)
			{
				return true;
			}
			if (animator.IsInTransition(0))
			{
				return false;
			}
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (string.IsNullOrEmpty(activeStandUpState) || !currentAnimatorStateInfo.IsName(activeStandUpState))
			{
				if (Time.time - standUpStartedAt > 0.5f)
				{
					return !WarnAboutMissingState();
				}
				return false;
			}
			return currentAnimatorStateInfo.normalizedTime >= 1f;
		}

		private bool WarnAboutMissingState()
		{
			if (!warnedAboutTrigger)
			{
				warnedAboutTrigger = true;
				Debug.LogWarning("[PlayerRagdoll] Animator '" + activeStandUpState + "' state'ine gecmedi - o state ve onu acan trigger controller'da var mi? Zaman asimiyla ayaga kalkilacak.");
			}
			return false;
		}

		private void OnStateChanged(RagdollState previous, RagdollState current)
		{
			Apply(current);
		}

		private void Apply(RagdollState current)
		{
			if (current == RagdollState.Limp || current == RagdollState.Dead)
			{
				EnterLimp();
			}
			else if (previousApplied == RagdollState.Limp || previousApplied == RagdollState.Dead)
			{
				ExitLimp(current == RagdollState.StandingUp);
			}
			if (movement != null)
			{
				movement.RagdollFrozen = current != RagdollState.None;
			}
			if (current == RagdollState.None && playerAnimator != null)
			{
				playerAnimator.SetRigsSuspended(suspended: false);
			}
			previousApplied = current;
		}

		public bool TryGetLimpPivotLocal(out Vector3 pivotLocal)
		{
			pivotLocal = default(Vector3);
			if (!IsLimp || hips == null)
			{
				return false;
			}
			pivotLocal = base.transform.InverseTransformPoint(hips.position);
			return true;
		}

		private void EnterLimp()
		{
			CacheBones();
			if (bones.Count != 0)
			{
				CalibrateFacingAxis();
				if (playerAnimator != null)
				{
					playerAnimator.SetRigsSuspended(suspended: true);
				}
				if (animator != null)
				{
					animator.enabled = false;
				}
				SnapshotBonePoses();
				SetBonesSimulated(simulated: true);
				DetachBonesFromOwnCapsule();
				LaunchBonesWithCurrentMotion();
				ApplyPendingImpulse();
				ApplyQueuedNudge();
				nudgeLoggedThisLimp = false;
			}
		}

		private void LaunchBonesWithCurrentMotion()
		{
			if (rootVelocity.sqrMagnitude < 0.01f)
			{
				return;
			}
			foreach (Rigidbody bone in bones)
			{
				if (bone != null && !bone.isKinematic)
				{
					bone.linearVelocity = rootVelocity;
				}
			}
		}

		private void DetachBonesFromOwnCapsule()
		{
			if (detachedFromCapsule || characterController == null || !characterController.enabled)
			{
				return;
			}
			detachedFromCapsule = true;
			foreach (Collider boneCollider in boneColliders)
			{
				if (boneCollider != null && boneCollider.enabled)
				{
					Physics.IgnoreCollision(boneCollider, characterController, ignore: true);
				}
			}
		}

		private void ExitLimp(bool playStandUp)
		{
			if (bones.Count == 0)
			{
				return;
			}
			bool num = hips != null;
			Vector3 restPosition = (num ? hips.position : Vector3.zero);
			bool flag = IsLyingFaceDown();
			SetBonesSimulated(simulated: false);
			RestoreBonePoses();
			pendingImpulse = Vector3.zero;
			queuedNudgeBone = -1;
			if (animator != null)
			{
				animator.enabled = true;
			}
			if (num && playStandUp)
			{
				MoveCapsuleUnder(restPosition);
			}
			if (playStandUp && !(animator == null))
			{
				string text = (flag ? standUpFrontTrigger : standUpBackTrigger);
				activeStandUpState = (flag ? standUpFrontState : standUpBackState);
				standUpStartedAt = Time.time;
				if (!string.IsNullOrEmpty(text) && HasTrigger(text))
				{
					animator.SetTrigger(text);
				}
			}
		}

		private void SnapshotBonePoses()
		{
			if (bonePosePositions == null || bonePosePositions.Length != bones.Count)
			{
				bonePosePositions = new Vector3[bones.Count];
				bonePoseRotations = new Quaternion[bones.Count];
			}
			for (int i = 0; i < bones.Count; i++)
			{
				if (!(bones[i] == null))
				{
					Transform transform = bones[i].transform;
					bonePosePositions[i] = transform.localPosition;
					bonePoseRotations[i] = transform.localRotation;
				}
			}
			bonePosesCaptured = true;
		}

		private void RestoreBonePoses()
		{
			if (!bonePosesCaptured || bonePosePositions == null || bonePosePositions.Length != bones.Count)
			{
				return;
			}
			for (int i = 0; i < bones.Count; i++)
			{
				if (!(bones[i] == null))
				{
					Transform obj = bones[i].transform;
					obj.localPosition = bonePosePositions[i];
					obj.localRotation = bonePoseRotations[i];
				}
			}
		}

		private bool IsLyingFaceDown()
		{
			if (hips == null)
			{
				return true;
			}
			return Vector3.Dot(hips.TransformDirection(hipsFacingAxis), Vector3.up) < 0f;
		}

		private void CalibrateFacingAxis()
		{
			if (facingAxisCalibrated || hips == null)
			{
				return;
			}
			facingAxisCalibrated = true;
			Vector3 forward = base.transform.forward;
			float num = float.MinValue;
			Vector3[] facingCandidates = FacingCandidates;
			foreach (Vector3 direction in facingCandidates)
			{
				float num2 = Vector3.Dot(hips.TransformDirection(direction), forward);
				if (!(num2 <= num))
				{
					num = num2;
					hipsFacingAxis = direction;
				}
			}
		}

		private void MoveCapsuleUnder(Vector3 restPosition)
		{
			if (!base.IsOwner)
			{
				return;
			}
			Vector3 position = restPosition;
			if (Physics.Raycast(restPosition + Vector3.up, Vector3.down, out var hitInfo, 4f, groundLayers, QueryTriggerInteraction.Ignore))
			{
				position = hitInfo.point;
			}
			int num;
			if (characterController != null)
			{
				num = (characterController.enabled ? 1 : 0);
				if (num != 0)
				{
					characterController.enabled = false;
				}
			}
			else
			{
				num = 0;
			}
			base.transform.position = position;
			if (num != 0)
			{
				characterController.enabled = true;
			}
			if (movement != null)
			{
				movement.NotifyRepositioned();
				movement.ResolveOverlaps();
			}
		}

		private void SetBonesSimulated(bool simulated)
		{
			foreach (Rigidbody bone in bones)
			{
				if (!(bone == null))
				{
					bone.isKinematic = !simulated;
					bone.detectCollisions = simulated;
					if (simulated)
					{
						bone.WakeUp();
					}
				}
			}
			foreach (Collider boneCollider in boneColliders)
			{
				if (boneCollider != null)
				{
					boneCollider.enabled = simulated;
				}
			}
		}

		private bool HasTrigger(string name)
		{
			AnimatorControllerParameter[] parameters = animator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Trigger && animatorControllerParameter.name == name)
				{
					return true;
				}
			}
			WarnAboutMissingState();
			return false;
		}

		protected override void __initializeVariables()
		{
			if (state == null)
			{
				throw new Exception("PlayerRagdoll.state cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			state.Initialize(this);
			__nameNetworkVariable(state, "state");
			NetworkVariableFields.Add(state);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(3848969826u, __rpc_handler_3848969826, "KnockdownImpulseClientRpc", RpcInvokePermission.Server);
			__registerRpc(770797621u, __rpc_handler_770797621, "NudgeClientRpc", RpcInvokePermission.Server);
			__registerRpc(504955165u, __rpc_handler_504955165, "RequestRagdollServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(2903118628u, __rpc_handler_2903118628, "ReportStandUpFinishedServerRpc", RpcInvokePermission.Everyone);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_3848969826(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerRagdoll)target).KnockdownImpulseClientRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_770797621(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out int value);
				reader.ReadValueSafe(out Vector3 value2);
				reader.ReadValueSafe(out Vector3 value3);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerRagdoll)target).NudgeClientRpc(value, value2, value3);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_504955165(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerRagdoll)target).RequestRagdollServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2903118628(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((PlayerRagdoll)target).ReportStandUpFinishedServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "PlayerRagdoll";
		}
	}
}
