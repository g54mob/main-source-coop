using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.AudioServiceModule.Scripts;
using Features.ChestScreamerModule.Scripts.Presets;
using Features.ChestScreamerModule.Scripts.Systems;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.HingeModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.ChestScreamerModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class ChestScreamerController : NetworkBehaviour, IChestScreamerLidGate, IHingeInitializable
	{
		[Header("References")]
		[SerializeField]
		private Rigidbody _lidRigidbody;

		[SerializeField]
		private HingeJoint _hingeJoint;

		[SerializeField]
		private SimplePointGrabable _lidGrabable;

		[SerializeField]
		private Transform _screamerOrigin;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[Header("Eligibility")]
		[SerializeField]
		private bool _isScreamAvailable = true;

		[Header("Lid Occupant Guard")]
		[SerializeField]
		private Vector3 _topCheckHalfExtents = new Vector3(0.35f, 0.2f, 0.35f);

		[SerializeField]
		private float _topCheckUpOffset = 0.2f;

		[SerializeField]
		private LayerMask _lidOccupantCheckMask = -1;

		private ChestScreamerConfiguration _configuration;

		private IChestScreamerGameplay _gameplay;

		private readonly Collider[] _topCheckHits = new Collider[16];

		private Quaternion _closedWorldRotation;

		private float _closedHingeAngle;

		private float _grabHoldSeconds;

		private bool _rollCompleted;

		private float _grabPhysicsRestoreAtTime = -1f;

		private const int LidHoldCaptureDelayTicks = 2;

		private bool _isLidHeldOpen;

		private int _lidHoldCaptureDelayTicksRemaining;

		private Quaternion _heldOpenRotation;

		private bool _localVisualRestored;

		[WeaverGenerated]
		[DefaultForProperty("IsScreamerChest", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsScreamerChest;

		[WeaverGenerated]
		[DefaultForProperty("HasTriggered", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _HasTriggered;

		[WeaverGenerated]
		[DefaultForProperty("_screamerType", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private ChestScreamerType __screamerType;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsScreamerChest
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestScreamerController.IsScreamerChest. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestScreamerController.IsScreamerChest. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe bool HasTriggered
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestScreamerController.HasTriggered. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestScreamerController.HasTriggered. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe ChestScreamerType _screamerType
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestScreamerController._screamerType. Networked properties can only be accessed when Spawned() has been called.");
				}
				return (ChestScreamerType)Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ChestScreamerController._screamerType. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = (int)value;
			}
		}

		public bool ShouldSuppressHingeAngleBreak
		{
			get
			{
				if (IsScreamerChest)
				{
					return !HasTriggered;
				}
				return false;
			}
		}

		public bool ShouldSuppressHingeDespawn
		{
			get
			{
				if (IsScreamerChest)
				{
					return HasTriggered;
				}
				return false;
			}
		}

		public bool ShouldSuppressStandUpOpenImpulse
		{
			get
			{
				if (HasTriggered)
				{
					return _gameplay.IsBlockedGrabbleAfterScreamer(_screamerType);
				}
				return false;
			}
		}

		[Inject]
		public void InjectDependencies(IChestScreamerGameplay gameplay, ChestScreamerConfiguration configuration)
		{
			_gameplay = gameplay;
			_configuration = configuration;
		}

		public void DebugApplyScreamerChance()
		{
			if (base.Object.HasStateAuthority && !HasTriggered)
			{
				_rollCompleted = false;
				_grabHoldSeconds = 0f;
				TryCompleteRoll();
			}
		}

		public void InitializeHingeJoint(HingeJoint joint)
		{
			_hingeJoint = joint;
			CacheClosedState();
		}

		public override void Spawned()
		{
			base.Spawned();
			CacheClosedState();
			TryCompleteRoll();
			TryRestoreTriggeredState();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_localVisualRestored = false;
		}

		public override void FixedUpdateNetwork()
		{
			if (!_rollCompleted && base.Object.HasStateAuthority)
			{
				TryCompleteRoll();
			}
			if (HasTriggered)
			{
				HoldLidOpenPosition();
				TryRestoreLidGrabPhysics();
			}
			else if (IsScreamerChest && base.Object.HasStateAuthority && _gameplay.HasPreset(_screamerType) && !IsSomethingOnLidTop())
			{
				if (IsLidGrabbed())
				{
					_grabHoldSeconds += base.Runner.DeltaTime;
				}
				else
				{
					_grabHoldSeconds = 0f;
				}
				if (TryGetOpenDelta(out var delta) && _gameplay.CanTrigger(_screamerType, delta, _grabHoldSeconds, IsLidGrabbed()))
				{
					TriggerScreamer(GetTriggeringPlayerId());
				}
			}
		}

		private void TryCompleteRoll()
		{
			if (_rollCompleted || !base.Object.HasStateAuthority)
			{
				return;
			}
			if (!_isScreamAvailable)
			{
				IsScreamerChest = false;
				_rollCompleted = true;
			}
			else if (!IsSomethingOnLidTop())
			{
				IsScreamerChest = UnityEngine.Random.value < _configuration.ProcChance;
				if (IsScreamerChest)
				{
					_screamerType = GetRandomScreamerTypeByWeight();
				}
				_rollCompleted = true;
			}
		}

		private bool IsLidGrabbed()
		{
			if (_lidGrabable.GrabbedByPlayersCount <= 0)
			{
				return _lidGrabable.GrabbedBySomethingCount > 0;
			}
			return true;
		}

		private bool IsSomethingOnLidTop()
		{
			int num = Physics.OverlapBoxNonAlloc(_lidRigidbody.worldCenterOfMass + Vector3.up * _topCheckUpOffset, _topCheckHalfExtents, _topCheckHits, _lidRigidbody.rotation, _lidOccupantCheckMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Collider collider = _topCheckHits[i];
				if (!(collider == null) && !IsOwnLidCollider(collider))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsOwnLidCollider(Collider collider)
		{
			if (!(collider.attachedRigidbody == _lidRigidbody))
			{
				return collider.GetComponentInParent<SimplePointGrabable>() == _lidGrabable;
			}
			return true;
		}

		private bool TryGetOpenDelta(out float delta)
		{
			if (_hingeJoint != null && TryGetHingeDelta(out delta))
			{
				return true;
			}
			delta = Quaternion.Angle(_closedWorldRotation, base.transform.rotation);
			return true;
		}

		private bool TryGetHingeDelta(out float delta)
		{
			float angle = _hingeJoint.angle;
			if (float.IsNaN(angle) || float.IsNaN(_closedHingeAngle))
			{
				delta = 0f;
				return false;
			}
			delta = Mathf.Abs(angle - _closedHingeAngle);
			return true;
		}

		private int GetTriggeringPlayerId()
		{
			if (_lidGrabable.GrabbedByPlayers.Count > 0)
			{
				return _lidGrabable.GrabbedByPlayers[0];
			}
			List<PhysGrabber> list = _lidGrabable.GrabObject?.Grabbers;
			if (list != null && list.Count > 0)
			{
				return list[0].PlayerId;
			}
			return 0;
		}

		private void TriggerScreamer(int triggeringPlayerId)
		{
			HasTriggered = true;
			TriggerScreamerRpc(triggeringPlayerId);
		}

		private void TryRestoreTriggeredState()
		{
			if (HasTriggered && !_localVisualRestored && _gameplay.HasPreset(_screamerType))
			{
				_localVisualRestored = true;
				if (_gameplay.IsBlockedGrabbleAfterScreamer(_screamerType))
				{
					SetLidGrabPhysicsBlocked(blocked: true);
				}
				Vector3 position = _screamerOrigin.position;
				Quaternion rotation = _screamerOrigin.rotation;
				_gameplay.RestorePersistedScreamerVisual(_screamerType, position, rotation);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 3871187737u)]
		private void TriggerScreamerRpc([RpcPayload(4)] int triggeringPlayerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3871187737u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.ChestScreamerModule.Scripts.ChestScreamerController::TriggerScreamerRpc(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(triggeringPlayerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_localVisualRestored = true;
			SetLidGrabPhysicsBlocked(blocked: true);
			_grabPhysicsRestoreAtTime = base.Runner.SimulationTime + _gameplay.GetUnblockGrabTimer(_screamerType);
			Vector3 position = _screamerOrigin.position;
			Quaternion rotation = _screamerOrigin.rotation;
			_gameplay.PlayFeedback(_screamerType, position, rotation, _soundSourceBehaviour.ID);
			if (base.Object.HasStateAuthority)
			{
				ReleaseLidGrabs();
				ApplyLidOpenImpulse(GetLidOpenImpulseReferencePosition(triggeringPlayerId));
				_isLidHeldOpen = true;
				_lidHoldCaptureDelayTicksRemaining = 2;
			}
		}

		private void ReleaseLidGrabs()
		{
			List<int> list = new List<int>(_lidGrabable.GrabbedByPlayers);
			for (int i = 0; i < list.Count; i++)
			{
				_lidGrabable.UnGrabbedByPlayer(list[i]);
			}
		}

		private void ApplyLidOpenImpulse(Vector3 referencePosition)
		{
			if (_gameplay.HasPreset(_screamerType))
			{
				Transform nearestHandle = _lidGrabable.GetNearestHandle(referencePosition);
				if (nearestHandle == null)
				{
					nearestHandle = _lidRigidbody.transform;
				}
				float num = Mathf.Max(0.0001f, _lidRigidbody.mass);
				Vector3 vector = Vector3.ProjectOnPlane(referencePosition - _lidRigidbody.worldCenterOfMass, Vector3.up);
				if (vector.sqrMagnitude < 0.0001f)
				{
					vector = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
				}
				vector.Normalize();
				float openImpulseForwardBias = _gameplay.GetOpenImpulseForwardBias(_screamerType);
				Vector3 force = (Vector3.up + vector * openImpulseForwardBias).normalized * (num * _gameplay.GetOpenImpulsePerMass(_screamerType));
				_lidRigidbody.AddForceAtPosition(force, nearestHandle.position, ForceMode.Impulse);
			}
		}

		private Vector3 GetLidOpenImpulseReferencePosition(int triggeringPlayerId)
		{
			if (base.Runner != null && triggeringPlayerId > 0)
			{
				foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
				{
					if (activePlayer.PlayerId == triggeringPlayerId)
					{
						NetworkObject playerObject = base.Runner.GetPlayerObject(activePlayer);
						if (playerObject != null)
						{
							return playerObject.transform.position;
						}
					}
				}
			}
			return base.transform.position;
		}

		private void HoldLidOpenPosition()
		{
			if (!_isLidHeldOpen || !base.Object.HasStateAuthority || (_grabPhysicsRestoreAtTime >= 0f && base.Runner.SimulationTime >= _grabPhysicsRestoreAtTime))
			{
				return;
			}
			if (!_lidRigidbody.isKinematic)
			{
				if (_lidHoldCaptureDelayTicksRemaining > 0)
				{
					_lidHoldCaptureDelayTicksRemaining--;
					return;
				}
				_heldOpenRotation = GetHoldOpenRotation();
				_lidRigidbody.isKinematic = true;
			}
			_lidRigidbody.MoveRotation(_heldOpenRotation);
		}

		private Quaternion GetHoldOpenRotation()
		{
			float holdOpenAngleDegrees = _gameplay.GetHoldOpenAngleDegrees(_screamerType);
			if (_hingeJoint != null)
			{
				Vector3 normalized = base.transform.TransformDirection(_hingeJoint.axis).normalized;
				float num = _hingeJoint.angle - _closedHingeAngle;
				return Quaternion.AngleAxis((Mathf.Approximately(num, 0f) ? 1f : Mathf.Sign(num)) * holdOpenAngleDegrees, normalized) * _closedWorldRotation;
			}
			return Quaternion.AngleAxis(holdOpenAngleDegrees, base.transform.up) * _closedWorldRotation;
		}

		private void TryRestoreLidGrabPhysics()
		{
			if (!(_grabPhysicsRestoreAtTime < 0f) && !(base.Runner.SimulationTime < _grabPhysicsRestoreAtTime))
			{
				ReleaseLidOpenHold();
				if (!_gameplay.IsBlockedGrabbleAfterScreamer(_screamerType))
				{
					SetLidGrabPhysicsBlocked(blocked: false);
				}
				_grabPhysicsRestoreAtTime = -1f;
			}
		}

		private void ReleaseLidOpenHold()
		{
			if (_isLidHeldOpen)
			{
				_isLidHeldOpen = false;
				_lidHoldCaptureDelayTicksRemaining = 0;
				if (base.Object.HasStateAuthority)
				{
					_lidRigidbody.isKinematic = false;
					_lidRigidbody.WakeUp();
				}
			}
		}

		private void SetLidGrabPhysicsBlocked(bool blocked)
		{
			_lidGrabable.GrabObject.GrabbingPhysicsBlocked = blocked;
		}

		private void CacheClosedState()
		{
			_closedWorldRotation = base.transform.rotation;
			_closedHingeAngle = ((_hingeJoint != null && !float.IsNaN(_hingeJoint.angle)) ? _hingeJoint.angle : 0f);
		}

		private ChestScreamerType GetRandomScreamerTypeByWeight()
		{
			float num = 0f;
			ChestScreamerPreset[] presets = _configuration.Presets;
			foreach (ChestScreamerPreset chestScreamerPreset in presets)
			{
				num += Mathf.Max(0f, chestScreamerPreset.Weight);
			}
			if (num <= 0f)
			{
				return ChestScreamerType.None;
			}
			float num2 = UnityEngine.Random.Range(0f, num);
			presets = _configuration.Presets;
			foreach (ChestScreamerPreset chestScreamerPreset2 in presets)
			{
				float num3 = Mathf.Max(0f, chestScreamerPreset2.Weight);
				if (num2 < num3)
				{
					return chestScreamerPreset2.Type;
				}
				num2 -= num3;
			}
			return _configuration.Presets[^1].Type;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsScreamerChest = _IsScreamerChest;
			HasTriggered = _HasTriggered;
			_screamerType = __screamerType;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsScreamerChest = IsScreamerChest;
			_HasTriggered = HasTriggered;
			__screamerType = _screamerType;
		}

		[NetworkRpcWeavedInvoker(3871187737u)]
		[Preserve]
		[WeaverGenerated]
		protected static void TriggerScreamerRpc_0040Invoker3871187737([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((ChestScreamerController)context.TargetBehaviour).TriggerScreamerRpc(value);
		}
	}
}
