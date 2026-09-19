using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.CustomSynchronizersModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.RagdollModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class RagdollEntity : NetworkBehaviour, IRagdollEntity, IAfterSpawned, IPublicFacingInterface
	{
		private const string BoneTagName = "Bone";

		private const string NonPhysicsBoneTagName = "NonPhysicsBone";

		[SerializeField]
		private Transform _ragdollExitBoneTransform;

		[SerializeField]
		private Transform _ragdollRoot;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private Animator _referenceAnimator;

		[SerializeField]
		private Transform _animationRoot;

		[SerializeField]
		private float _ragdollYOffset = 1f;

		[SerializeField]
		private float _ragdollExitGroundThreshold = 0.05f;

		[SerializeField]
		private float _ragdollExitRaycastDistance = 5f;

		[SerializeField]
		private LayerMask _ragdollExitGroundMask = -1;

		[SerializeField]
		private float _ragdollBlendOutDuration = 0.35f;

		[SerializeField]
		private float _animatorCrossFadeDuration = 0.25f;

		[SerializeField]
		private PhysicsSynchronizer _networkTransform;

		[SerializeField]
		private bool _isSimulatedOnStart;

		[SerializeField]
		private List<Collider> _excludeColliders;

		[SerializeField]
		private List<Rigidbody> _excludeRIgidibodies;

		[SerializeField]
		protected List<PhysicsSynchronizer> _physicsSynchronizer;

		[SerializeField]
		private Vector3 _teleportPosition;

		private int _prevTeleportKey;

		protected Vector3 EntityRagdollPosition = Vector3.zero;

		private TagHandle _boneTag;

		private TagHandle _nonPhysicsBoneTag;

		private Rigidbody[] _ragdollRbs;

		private List<INetworkTRSPTeleport> _ragdollSynchronizers = new List<INetworkTRSPTeleport>();

		private readonly HashSet<RagdollSimulationReasonEnum> _simulationReasons = new HashSet<RagdollSimulationReasonEnum>();

		[WeaverGenerated]
		[DefaultForProperty("SimulationReasonsMask", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _SimulationReasonsMask;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsRagdollSimulated", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsRagdollSimulated;

		private int _pendingMaskClear;

		private readonly Dictionary<RagdollBoneData, Transform> _jointsAnimationBonesMap = new Dictionary<RagdollBoneData, Transform>();

		private Dictionary<ConfigurableJoint, Quaternion> _initialJointLocalRotations;

		private Dictionary<RagdollBoneData, (Vector3 localPosition, Quaternion localRotation)> _initialBonePoses;

		private Dictionary<Transform, Vector3> _initialAnimationBoneLocalPositions;

		private bool _isSimulated;

		private bool _disableRagdollRequested;

		private int _lastAppliedSimulationReasonsMask = -1;

		private Coroutine _poseResetRoutine;

		private Coroutine _disableRagdollCoroutine;

		private bool _isBlendOutInProgress;

		[field: SerializeField]
		public bool IsActiveRagdoll { get; set; } = true;

		protected Collider[] RagdollColliders { get; private set; }

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe int SimulationReasonsMask
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RagdollEntity.SimulationReasonsMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RagdollEntity.SimulationReasonsMask. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe bool IsRagdollSimulated
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RagdollEntity.IsRagdollSimulated. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RagdollEntity.IsRagdollSimulated. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		public NetworkObject NetworkObject => base.Object;

		public HashSet<RagdollSimulationReasonEnum> SimulationReasons => _simulationReasons;

		public bool IsSimulated
		{
			get
			{
				return _isSimulated;
			}
			private set
			{
				if (_isSimulated != value)
				{
					_isSimulated = value;
					if (_isSimulated)
					{
						this.OnSimulationStarted?.Invoke(this);
					}
					else
					{
						this.OnSimulationStopped?.Invoke(this);
					}
				}
			}
		}

		public bool IsBlendingOut => _isBlendOutInProgress;

		public IReadOnlyList<Rigidbody> Bones => _ragdollRbs;

		public RagdollPhysData RootPhysData => new RagdollPhysData
		{
			RigidBody = ((_ragdollRbs.Length == 0) ? null : _ragdollRbs[0]),
			Collider = ((RagdollColliders.Length == 0) ? null : RagdollColliders[0]),
			TransformSynchronizer = ((_ragdollSynchronizers.Count == 0) ? null : _ragdollSynchronizers[0])
		};

		public bool IsInitialized { get; private set; }

		public event Action<RagdollSimulationReasonEnum> OnRagdollSimulationReasonRemoved;

		public event Action<Vector3, Vector3> OnTeleported;

		public event Action<IRagdollEntity> OnSimulationStarted;

		public event Action<IRagdollEntity> OnSimulationStopped;

		public event Action<RagdollSimulationReasonEnum> OnRagdollSimulationReasonAdded;

		public void Teleport(Vector3 position, Quaternion? rotation = null)
		{
			if (!base.HasStateAuthority || PlayerSpawnLock.ShouldBlockPositionOverride())
			{
				return;
			}
			Vector3 position2 = RootPhysData.RigidBody.position;
			Quaternion? quaternion = (rotation.HasValue ? new Quaternion?(rotation.Value * Quaternion.Inverse(RootPhysData.RigidBody.rotation)) : ((Quaternion?)null));
			if (IsSimulated)
			{
				ResetJointTargets();
				for (int i = 0; i < _ragdollRbs.Length; i++)
				{
					Vector3 vector = _ragdollRbs[i].position - position2;
					Vector3 value = position + (quaternion.HasValue ? (quaternion.Value * vector) : vector);
					Quaternion? rotation2 = (quaternion.HasValue ? new Quaternion?(quaternion.Value * _ragdollRbs[i].rotation) : ((Quaternion?)null));
					_ragdollSynchronizers[i].Teleport(value, rotation2);
				}
				Rigidbody[] ragdollRbs = _ragdollRbs;
				foreach (Rigidbody obj in ragdollRbs)
				{
					obj.linearVelocity = Vector3.zero;
					obj.angularVelocity = Vector3.zero;
				}
				SetRagdollReferencePosition();
			}
			else
			{
				_networkTransform.Teleport(position, rotation);
			}
			this.OnTeleported?.Invoke(position, position2);
		}

		public void SetInterpolation(RigidbodyInterpolation interpolation)
		{
			SetInterpolationRpc(interpolation);
		}

		public void SetPose(Transform poseBlueprint)
		{
			ApplyPoseImmediate(poseBlueprint.position, poseBlueprint.rotation);
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 447713943u)]
		private void SetInterpolationRpc([RpcPayload(4)] RigidbodyInterpolation interpolation)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(447713943u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.RagdollEntity::SetInterpolationRpc(UnityEngine.RigidbodyInterpolation)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(interpolation, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			SetInterpolationInternal(interpolation);
		}

		private void SetInterpolationInternal(RigidbodyInterpolation interpolation)
		{
			Rigidbody[] ragdollRbs = _ragdollRbs;
			for (int i = 0; i < ragdollRbs.Length; i++)
			{
				ragdollRbs[i].interpolation = interpolation;
			}
		}

		public void ApplyWholeBodyBounce(Vector3 bounceVelocity)
		{
			if (_ragdollRbs == null || !IsSimulated)
			{
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			Rigidbody[] ragdollRbs = _ragdollRbs;
			foreach (Rigidbody rigidbody in ragdollRbs)
			{
				if (!(rigidbody == null))
				{
					if (rigidbody.isKinematic)
					{
						rigidbody.MovePosition(rigidbody.position + bounceVelocity * fixedDeltaTime);
						continue;
					}
					rigidbody.WakeUp();
					rigidbody.linearVelocity = bounceVelocity;
				}
			}
			Physics.SyncTransforms();
		}

		public void AfterSpawned()
		{
			_boneTag = TagHandle.GetExistingTag("Bone");
			_nonPhysicsBoneTag = TagHandle.GetExistingTag("NonPhysicsBone");
			List<Rigidbody> list = _ragdollRoot.GetComponentsInChildren<Rigidbody>().ToList();
			foreach (Rigidbody excludeRIgidibody in _excludeRIgidibodies)
			{
				if (list.Contains(excludeRIgidibody))
				{
					list.Remove(excludeRIgidibody);
				}
			}
			_ragdollRbs = list.ToArray();
			List<Collider> list2 = _ragdollRoot.GetComponentsInChildren<Collider>().ToList();
			foreach (Collider excludeCollider in _excludeColliders)
			{
				if (list2.Contains(excludeCollider))
				{
					list2.Remove(excludeCollider);
				}
			}
			Rigidbody[] ragdollRbs = _ragdollRbs;
			foreach (Rigidbody rigidbody in ragdollRbs)
			{
				_ragdollSynchronizers.Add(rigidbody.GetComponent<INetworkTRSPTeleport>());
				rigidbody.maxDepenetrationVelocity = 1f;
			}
			RagdollColliders = list2.ToArray();
			MapJointsToAnimationBones();
			InitializeJointsLocalRotation();
			CaptureInitialBonePoses();
			SwitchSelfCollisions(collisionsEnabled: false);
			EntityRagdollPosition = base.transform.position;
			OnInitialized();
			EnsurePhysicsSynchronizersCached();
			if (_isSimulatedOnStart)
			{
				TryPushSimulationReason(RagdollSimulationReasonEnum.Default);
				AddReasonToNetworkMask(RagdollSimulationReasonEnum.Default);
				EnableRagdoll();
				SetInterpolationInternal(RigidbodyInterpolation.Interpolate);
			}
			else
			{
				SyncSimulationReasonsFromNetworkMask();
				if (_simulationReasons.Count == 0)
				{
					DisableRagdoll();
				}
			}
			_lastAppliedSimulationReasonsMask = SimulationReasonsMask;
			_pendingMaskClear = 0;
			IsInitialized = true;
		}

		public void AddSimulationReason(RagdollSimulationReasonEnum reason)
		{
			if (reason != RagdollSimulationReasonEnum.None)
			{
				AddSimulationReasonRpc(reason);
			}
		}

		public void AddSimulationReason(RagdollSimulationReasonEnum reason, Transform poseBlueprint)
		{
			if (reason != RagdollSimulationReasonEnum.None)
			{
				AddSimulationReasonWithPoseRpc(reason, poseBlueprint.position, poseBlueprint.rotation);
			}
		}

		public void RemoveSimulationReason(RagdollSimulationReasonEnum reason)
		{
			if (reason != RagdollSimulationReasonEnum.None)
			{
				RemoveSimulationReasonRpc(reason);
			}
		}

		public void RemoveAllSimulationReasons()
		{
			RemoveAllSimulationReasonsRpc();
		}

		public void ForceRecoverInPlace()
		{
			if (!IsInitialized)
			{
				return;
			}
			foreach (RagdollSimulationReasonEnum simulationReason in _simulationReasons)
			{
				this.OnRagdollSimulationReasonRemoved?.Invoke(simulationReason);
			}
			if (!base.HasStateAuthority)
			{
				_pendingMaskClear |= LocalReasonsMask();
			}
			_simulationReasons.Clear();
			ClearNetworkMask();
			bool isBlendOutInProgress = _isBlendOutInProgress;
			CancelDisableRagdollBlendCoroutine();
			_disableRagdollRequested = false;
			if (!IsSimulated && !isBlendOutInProgress)
			{
				return;
			}
			OnRagdollBlendOutStarted();
			DisableRagdollImmediate();
			OnRagdollBlendOutFinished();
			EnsurePhysicsSynchronizersCached();
			foreach (PhysicsSynchronizer item in _physicsSynchronizer)
			{
				item.EnableSynchronization();
			}
		}

		public bool HasSimulationReason(RagdollSimulationReasonEnum reason)
		{
			return _simulationReasons.Contains(reason);
		}

		public void ApplyPose(Transform poseBlueprint)
		{
			ApplyPoseRpc(poseBlueprint.position, poseBlueprint.rotation);
		}

		public void SwitchSelfCollisions(bool collisionsEnabled)
		{
			for (int i = 0; i < RagdollColliders.Length; i++)
			{
				for (int j = i + 1; j < RagdollColliders.Length; j++)
				{
					Physics.IgnoreCollision(RagdollColliders[i], RagdollColliders[j], !collisionsEnabled);
				}
			}
		}

		[ContextMenu("ResetPose")]
		public void ResetPose()
		{
			ResetPoseRpc();
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3204686439u)]
		private void ResetPoseRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3204686439u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.RagdollEntity::ResetPoseRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (_poseResetRoutine != null)
			{
				StopCoroutine(_poseResetRoutine);
			}
			_poseResetRoutine = StartCoroutine(ResetPoseCoroutine());
		}

		private IEnumerator ResetPoseCoroutine()
		{
			SwitchRagdollRigidbodies(active: false);
			foreach (var (ragdollBoneData2, tuple2) in _initialBonePoses)
			{
				ragdollBoneData2.Bone.localPosition = tuple2.Item1;
				ragdollBoneData2.Bone.localRotation = tuple2.Item2;
			}
			yield return new WaitForFixedUpdate();
			Rigidbody[] ragdollRbs = _ragdollRbs;
			foreach (Rigidbody obj in ragdollRbs)
			{
				obj.linearVelocity = Vector3.zero;
				obj.angularVelocity = Vector3.zero;
			}
			ResetJointTargets();
			SwitchRagdollRigidbodies(active: true);
		}

		private void Update()
		{
			SetRagdollReferencePosition();
			if (IsInitialized && _lastAppliedSimulationReasonsMask != SimulationReasonsMask)
			{
				SyncSimulationReasonsFromNetworkMask();
			}
			if (!IsSimulated || _isBlendOutInProgress || !IsActiveRagdoll)
			{
				return;
			}
			foreach (var (ragdollBoneData2, transform2) in _jointsAnimationBonesMap)
			{
				if (ragdollBoneData2.HasJoint && base.HasStateAuthority)
				{
					ragdollBoneData2.Joint.SetTargetRotationLocal(transform2.localRotation, _initialJointLocalRotations[ragdollBoneData2.Joint]);
				}
				if (!ragdollBoneData2.IsPhysical)
				{
					if (ragdollBoneData2.IsRotationOnlySync)
					{
						CopyLocalRotation(transform2, ragdollBoneData2.Bone);
					}
					else
					{
						CopyLocalTransform(transform2, ragdollBoneData2.Bone);
					}
				}
			}
		}

		private void SetRagdollReferencePosition()
		{
			if (IsInitialized)
			{
				RagdollPhysData rootPhysData = RootPhysData;
				if (rootPhysData.RigidBody != null)
				{
					EntityRagdollPosition = new Vector3(rootPhysData.RigidBody.position.x, rootPhysData.RigidBody.position.y + _ragdollYOffset, rootPhysData.RigidBody.position.z);
				}
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _disableRagdollRequested)
			{
				_disableRagdollRequested = false;
				if (_simulationReasons.Count == 0)
				{
					DisableRagdoll();
				}
			}
			if (base.HasStateAuthority && IsSimulated && SimulationReasonsMask == 0 && _simulationReasons.Count > 0)
			{
				SyncNetworkMaskFromLocalReasons();
			}
			if (base.HasStateAuthority && _pendingMaskClear != 0)
			{
				SimulationReasonsMask &= ~_pendingMaskClear;
				_pendingMaskClear = 0;
			}
			if (base.HasStateAuthority && !IsSimulated && !_isBlendOutInProgress && _simulationReasons.Count > 0)
			{
				EnableRagdoll();
			}
			if (base.HasStateAuthority && IsSimulated && !_isBlendOutInProgress && SimulationReasonsMask == 0 && _simulationReasons.Count == 0)
			{
				_disableRagdollRequested = true;
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3920136252u)]
		private void AddSimulationReasonRpc([RpcPayload(4)] RagdollSimulationReasonEnum reason)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3920136252u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.RagdollEntity::AddSimulationReasonRpc(Features.RagdollModule.Scripts.RagdollSimulationReasonEnum)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(reason, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			_pendingMaskClear &= ~ReasonToMask(reason);
			if (TryPushSimulationReason(reason))
			{
				AddReasonToNetworkMask(reason);
				_disableRagdollRequested = false;
				if (base.HasStateAuthority && (!IsSimulated || _isBlendOutInProgress))
				{
					EnableRagdoll();
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 36603363u)]
		private void AddSimulationReasonWithPoseRpc([RpcPayload(4)] RagdollSimulationReasonEnum reason, [RpcPayload(12)] Vector3Compressed posePosition, [RpcPayload(16)] QuaternionCompressed poseRotation)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(16);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(36603363u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.RagdollEntity::AddSimulationReasonWithPoseRpc(Features.RagdollModule.Scripts.RagdollSimulationReasonEnum,Fusion.Vector3Compressed,Fusion.QuaternionCompressed)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(reason, 4);
						writer.Write(posePosition, 12);
						writer.Write(poseRotation, 16);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyPoseImmediate(posePosition, ((Quaternion)poseRotation).normalized);
			_pendingMaskClear &= ~ReasonToMask(reason);
			if (TryPushSimulationReason(reason))
			{
				AddReasonToNetworkMask(reason);
				_disableRagdollRequested = false;
				if (base.HasStateAuthority && (!IsSimulated || _isBlendOutInProgress))
				{
					EnableRagdoll();
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1384075437u)]
		private void RemoveSimulationReasonRpc([RpcPayload(4)] RagdollSimulationReasonEnum reason)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1384075437u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.RagdollEntity::RemoveSimulationReasonRpc(Features.RagdollModule.Scripts.RagdollSimulationReasonEnum)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(reason, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (TryPopSimulationReason(reason))
			{
				RemoveReasonFromNetworkMask(reason);
				if (!base.HasStateAuthority)
				{
					_pendingMaskClear |= ReasonToMask(reason);
				}
				if (_simulationReasons.Count == 0 && IsSimulated)
				{
					_disableRagdollRequested = true;
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 3865049292u)]
		private void RemoveAllSimulationReasonsRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3865049292u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.RagdollEntity::RemoveAllSimulationReasonsRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			foreach (RagdollSimulationReasonEnum simulationReason in _simulationReasons)
			{
				this.OnRagdollSimulationReasonRemoved?.Invoke(simulationReason);
			}
			if (!base.HasStateAuthority)
			{
				_pendingMaskClear |= LocalReasonsMask();
			}
			_simulationReasons.Clear();
			ClearNetworkMask();
			if (IsSimulated)
			{
				_disableRagdollRequested = true;
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 1006259398u)]
		private void ApplyPoseRpc([RpcPayload(12)] Vector3Compressed posePosition, [RpcPayload(16)] QuaternionCompressed poseRotation)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(16);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1006259398u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.RagdollModule.Scripts.RagdollEntity::ApplyPoseRpc(Fusion.Vector3Compressed,Fusion.QuaternionCompressed)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(posePosition, 12);
						writer.Write(poseRotation, 16);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyPoseImmediate(posePosition, ((Quaternion)poseRotation).normalized);
		}

		public override void Render()
		{
			if (IsInitialized && !base.HasStateAuthority && !_isBlendOutInProgress && IsRagdollSimulated != _isSimulated)
			{
				if (IsRagdollSimulated)
				{
					EnableRagdoll();
				}
				else
				{
					DisableRagdoll();
				}
			}
		}

		protected virtual void EnableRagdoll()
		{
			CancelDisableRagdollBlendCoroutine();
			EnsurePhysicsSynchronizersCached();
			foreach (PhysicsSynchronizer item in _physicsSynchronizer)
			{
				item.EnableSynchronization();
			}
			SwitchRagdollAnimator(active: false);
			SwitchRagdollRigidbodies(active: true);
			StabilizeBodiesOnEnable();
			SwitchRagdollColliders(active: true);
			IsSimulated = true;
			if (base.HasStateAuthority)
			{
				IsRagdollSimulated = true;
			}
		}

		private void StabilizeBodiesOnEnable()
		{
			Rigidbody[] ragdollRbs = _ragdollRbs;
			foreach (Rigidbody rigidbody in ragdollRbs)
			{
				if (!(rigidbody == null) && !rigidbody.isKinematic)
				{
					rigidbody.maxDepenetrationVelocity = 1f;
					rigidbody.linearVelocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
				}
			}
		}

		protected virtual void DisableRagdoll()
		{
			CancelDisableRagdollBlendCoroutine();
			if (!IsSimulated)
			{
				DisableRagdollImmediate();
			}
			else if (_ragdollBlendOutDuration <= 0f)
			{
				DisableRagdollImmediate();
			}
			else
			{
				_disableRagdollCoroutine = StartCoroutine(DisableRagdollBlendOutCoroutine());
			}
		}

		protected virtual void OnRagdollBlendOutStarted()
		{
		}

		protected virtual void OnRagdollBlendOutFinished()
		{
		}

		private IEnumerator DisableRagdollBlendOutCoroutine()
		{
			_isBlendOutInProgress = true;
			OnRagdollBlendOutStarted();
			SwitchRagdollRigidbodies(active: false);
			SwitchRagdollColliders(active: false);
			ZeroRigidbodyVelocities();
			SnapEntityToRagdoll();
			EnsurePhysicsSynchronizersCached();
			foreach (PhysicsSynchronizer item in _physicsSynchronizer)
			{
				item.DisableSynchronization();
			}
			Dictionary<Transform, (Vector3 localPosition, Quaternion localRotation)> startPoses = CaptureAnimationBonesFromRagdoll();
			float elapsed = 0f;
			while (elapsed < _ragdollBlendOutDuration)
			{
				yield return null;
				elapsed += Time.deltaTime;
				float blendT = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / _ragdollBlendOutDuration));
				BlendBonesTowardAnimator(startPoses, blendT);
			}
			if (_simulationReasons.Count > 0)
			{
				_isBlendOutInProgress = false;
				_disableRagdollCoroutine = null;
				EnableRagdoll();
			}
			else
			{
				DisableRagdollImmediate();
				OnRagdollBlendOutFinished();
				_isBlendOutInProgress = false;
				_disableRagdollCoroutine = null;
			}
		}

		private void DisableRagdollImmediate(bool rebindAnimator = true)
		{
			SwitchRagdollRigidbodies(active: false);
			SwitchRagdollColliders(active: false);
			SnapEntityToRagdoll();
			RestoreNonPhysicalAnimationBoneLocalPositions();
			SwitchRagdollAnimator(active: true, rebindAnimator);
			SynchronizeAnimators(_animatorCrossFadeDuration);
			IsSimulated = false;
			if (base.HasStateAuthority)
			{
				IsRagdollSimulated = false;
			}
			EnsurePhysicsSynchronizersCached();
			foreach (PhysicsSynchronizer item in _physicsSynchronizer)
			{
				item.DisableSynchronization();
			}
		}

		private void CancelDisableRagdollBlendCoroutine()
		{
			if (_disableRagdollCoroutine != null)
			{
				StopCoroutine(_disableRagdollCoroutine);
				_disableRagdollCoroutine = null;
				_isBlendOutInProgress = false;
				RestoreNonPhysicalAnimationBoneLocalPositions();
			}
		}

		private void EnsurePhysicsSynchronizersCached()
		{
			if (_physicsSynchronizer == null || _physicsSynchronizer.Count <= 0)
			{
				_physicsSynchronizer = GetComponentsInChildren<PhysicsSynchronizer>().ToList();
			}
		}

		private void SyncSimulationReasonsFromNetworkMask()
		{
			_lastAppliedSimulationReasonsMask = SimulationReasonsMask;
			HashSet<RagdollSimulationReasonEnum> hashSet = new HashSet<RagdollSimulationReasonEnum>();
			foreach (RagdollSimulationReasonEnum value in Enum.GetValues(typeof(RagdollSimulationReasonEnum)))
			{
				if (value != RagdollSimulationReasonEnum.None && (SimulationReasonsMask & ReasonToMask(value)) != 0)
				{
					hashSet.Add(value);
				}
			}
			if (hashSet.SetEquals(_simulationReasons))
			{
				return;
			}
			bool flag = hashSet.Count > 0;
			_simulationReasons.Clear();
			foreach (RagdollSimulationReasonEnum item in hashSet)
			{
				_simulationReasons.Add(item);
			}
			if (flag && !IsSimulated)
			{
				_disableRagdollRequested = false;
				EnableRagdoll();
			}
			else if (!flag && IsSimulated)
			{
				_disableRagdollRequested = false;
				DisableRagdoll();
			}
		}

		private void AddReasonToNetworkMask(RagdollSimulationReasonEnum reason)
		{
			if (base.HasStateAuthority && reason != RagdollSimulationReasonEnum.None)
			{
				SimulationReasonsMask |= ReasonToMask(reason);
			}
		}

		private void RemoveReasonFromNetworkMask(RagdollSimulationReasonEnum reason)
		{
			if (base.HasStateAuthority && reason != RagdollSimulationReasonEnum.None)
			{
				SimulationReasonsMask &= ~ReasonToMask(reason);
			}
		}

		private void ClearNetworkMask()
		{
			if (base.HasStateAuthority)
			{
				SimulationReasonsMask = 0;
			}
		}

		private int ReasonToMask(RagdollSimulationReasonEnum reason)
		{
			return 1 << (int)reason;
		}

		private int LocalReasonsMask()
		{
			int num = 0;
			foreach (RagdollSimulationReasonEnum simulationReason in _simulationReasons)
			{
				num |= ReasonToMask(simulationReason);
			}
			return num;
		}

		private void SyncNetworkMaskFromLocalReasons()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			int num = 0;
			foreach (RagdollSimulationReasonEnum simulationReason in _simulationReasons)
			{
				num |= ReasonToMask(simulationReason);
			}
			SimulationReasonsMask = num;
		}

		protected virtual void SnapEntityToRagdoll()
		{
			ResolveEntityRagdollPosition();
			Transform obj = RootPhysData.RigidBody.transform;
			Vector3 position = obj.position;
			_networkTransform.Teleport(EntityRagdollPosition);
			obj.position = position;
		}

		private void ResolveEntityRagdollPosition()
		{
			if (TryGetStandUpCapsule(out var capsule))
			{
				Vector3 position = _ragdollExitBoneTransform.position;
				float y = position.y + _ragdollYOffset;
				RaycastHit hitInfo2;
				if (Physics.Raycast(position, Vector3.down, out var hitInfo, _ragdollExitRaycastDistance, _ragdollExitGroundMask, QueryTriggerInteraction.Ignore))
				{
					y = hitInfo.point.y + GetCapsuleBottomOffset(capsule) + _ragdollExitGroundThreshold;
				}
				else if (Physics.Raycast(position + Vector3.up * _ragdollExitRaycastDistance, Vector3.down, out hitInfo2, _ragdollExitRaycastDistance, _ragdollExitGroundMask, QueryTriggerInteraction.Ignore))
				{
					y = hitInfo2.point.y + GetCapsuleBottomOffset(capsule) + _ragdollExitGroundThreshold;
				}
				EntityRagdollPosition = new Vector3(position.x, y, position.z);
			}
		}

		protected virtual bool TryGetStandUpCapsule(out CapsuleCollider capsule)
		{
			capsule = null;
			return false;
		}

		public Vector3 GetReconnectStandUpPosition()
		{
			Vector3 position = RootPhysData.RigidBody.position;
			return new Vector3(position.x, position.y + _ragdollYOffset, position.z);
		}

		private static float GetCapsuleBottomOffset(CapsuleCollider capsule)
		{
			Vector3 lossyScale = capsule.transform.lossyScale;
			var (num, num2) = capsule.direction switch
			{
				0 => (Mathf.Abs(lossyScale.x), capsule.center.x), 
				1 => (Mathf.Abs(lossyScale.y), capsule.center.y), 
				_ => (Mathf.Abs(lossyScale.z), capsule.center.z), 
			};
			return (capsule.height * 0.5f - num2) * num;
		}

		protected virtual void OnInitialized()
		{
		}

		private void SwitchRagdollRigidbodies(bool active)
		{
			Rigidbody[] ragdollRbs = _ragdollRbs;
			foreach (Rigidbody rigidbody in ragdollRbs)
			{
				if (!(rigidbody == null))
				{
					rigidbody.isKinematic = !active;
					rigidbody.collisionDetectionMode = (active ? CollisionDetectionMode.ContinuousSpeculative : CollisionDetectionMode.Discrete);
				}
			}
		}

		private void SwitchRagdollColliders(bool active)
		{
			Collider[] ragdollColliders = RagdollColliders;
			for (int i = 0; i < ragdollColliders.Length; i++)
			{
				ragdollColliders[i].enabled = active;
			}
		}

		private void SwitchRagdollAnimator(bool active, bool rebind = true)
		{
			if (!(_animator == null))
			{
				_animator.enabled = active;
				if (active && rebind)
				{
					_animator.Rebind();
				}
			}
		}

		private void MapJointsToAnimationBones()
		{
			JointsTraverseUtil(_ragdollRoot, _animationRoot);
		}

		private void JointsTraverseUtil(Transform ragdollRoot, Transform animationRoot)
		{
			MapTaggedChildren(ragdollRoot, animationRoot, _boneTag, isRotationOnlySync: false);
			MapTaggedChildren(ragdollRoot, animationRoot, _nonPhysicsBoneTag, isRotationOnlySync: true);
		}

		private void MapTaggedChildren(Transform ragdollRoot, Transform animationRoot, TagHandle tag, bool isRotationOnlySync)
		{
			List<Transform> childrenWithTag = GetChildrenWithTag(ragdollRoot, tag);
			List<Transform> childrenWithTag2 = GetChildrenWithTag(animationRoot, tag);
			for (int i = 0; i < childrenWithTag.Count; i++)
			{
				Transform transform = childrenWithTag[i];
				Transform transform2 = childrenWithTag2[i];
				ConfigurableJoint component;
				bool hasJoint = transform.TryGetComponent<ConfigurableJoint>(out component);
				Rigidbody component2;
				RagdollBoneData key = new RagdollBoneData
				{
					Bone = transform,
					Joint = component,
					HasJoint = hasJoint,
					IsPhysical = transform.TryGetComponent<Rigidbody>(out component2),
					IsRotationOnlySync = isRotationOnlySync
				};
				_jointsAnimationBonesMap[key] = transform2;
				JointsTraverseUtil(transform, transform2);
			}
		}

		private List<Transform> GetChildrenWithTag(Transform parent, TagHandle tagHandle)
		{
			List<Transform> list = new List<Transform>(parent.childCount);
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (child.CompareTag(tagHandle))
				{
					list.Add(child);
				}
			}
			return list;
		}

		private void InitializeJointsLocalRotation()
		{
			_initialJointLocalRotations = new Dictionary<ConfigurableJoint, Quaternion>(_jointsAnimationBonesMap.Count((KeyValuePair<RagdollBoneData, Transform> x) => x.Key.HasJoint));
			foreach (RagdollBoneData key in _jointsAnimationBonesMap.Keys)
			{
				if (key.HasJoint)
				{
					_initialJointLocalRotations[key.Joint] = key.Joint.transform.localRotation;
				}
			}
		}

		private void CopyLocalTransform(Transform src, Transform dst)
		{
			dst.localPosition = src.localPosition;
			dst.localRotation = src.localRotation;
			dst.localScale = src.localScale;
		}

		private void CopyLocalRotation(Transform src, Transform dst)
		{
			dst.localRotation = src.localRotation;
		}

		private void SynchronizeAnimators(float crossFadeDuration = 0f)
		{
			if (_animator == null || _referenceAnimator == null)
			{
				return;
			}
			AnimatorControllerParameter[] parameters = _referenceAnimator.parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				switch (animatorControllerParameter.type)
				{
				case AnimatorControllerParameterType.Float:
					_animator.SetFloat(animatorControllerParameter.name, _referenceAnimator.GetFloat(animatorControllerParameter.name));
					break;
				case AnimatorControllerParameterType.Int:
					_animator.SetInteger(animatorControllerParameter.name, _referenceAnimator.GetInteger(animatorControllerParameter.name));
					break;
				case AnimatorControllerParameterType.Bool:
					_animator.SetBool(animatorControllerParameter.name, _referenceAnimator.GetBool(animatorControllerParameter.name));
					break;
				case AnimatorControllerParameterType.Trigger:
					if (_referenceAnimator.GetBool(animatorControllerParameter.name))
					{
						_animator.SetTrigger(animatorControllerParameter.name);
					}
					break;
				}
			}
			int num = 0;
			if (_referenceAnimator.IsInTransition(num))
			{
				AnimatorStateInfo nextAnimatorStateInfo = _referenceAnimator.GetNextAnimatorStateInfo(num);
				float normalizedTime = _referenceAnimator.GetAnimatorTransitionInfo(num).normalizedTime;
				_animator.CrossFade(nextAnimatorStateInfo.fullPathHash, crossFadeDuration, num, 0f);
				_animator.Update(0f);
				_animator.Play(nextAnimatorStateInfo.fullPathHash, num, normalizedTime);
			}
			else
			{
				AnimatorStateInfo currentAnimatorStateInfo = _referenceAnimator.GetCurrentAnimatorStateInfo(num);
				if (crossFadeDuration > 0f)
				{
					_animator.CrossFade(currentAnimatorStateInfo.fullPathHash, crossFadeDuration, num, currentAnimatorStateInfo.normalizedTime);
				}
				else
				{
					_animator.Play(currentAnimatorStateInfo.fullPathHash, num, currentAnimatorStateInfo.normalizedTime);
				}
			}
			_animator.Update(0f);
		}

		private void ZeroRigidbodyVelocities()
		{
			Rigidbody[] ragdollRbs = _ragdollRbs;
			foreach (Rigidbody rigidbody in ragdollRbs)
			{
				if (!(rigidbody == null))
				{
					rigidbody.linearVelocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
				}
			}
		}

		private Dictionary<Transform, (Vector3 localPosition, Quaternion localRotation)> CaptureAnimationBonesFromRagdoll()
		{
			Dictionary<Transform, (Vector3, Quaternion)> dictionary = new Dictionary<Transform, (Vector3, Quaternion)>(_jointsAnimationBonesMap.Count);
			foreach (KeyValuePair<RagdollBoneData, Transform> item in _jointsAnimationBonesMap)
			{
				item.Deconstruct(out var key, out var value);
				RagdollBoneData ragdollBoneData = key;
				Transform transform = value;
				Transform bone = ragdollBoneData.Bone;
				if (ragdollBoneData.IsRotationOnlySync)
				{
					transform.localRotation = bone.localRotation;
				}
				else
				{
					transform.SetPositionAndRotation(bone.position, bone.rotation);
				}
				dictionary[transform] = (transform.localPosition, transform.localRotation);
			}
			Physics.SyncTransforms();
			return dictionary;
		}

		private void BlendBonesTowardAnimator(Dictionary<Transform, (Vector3 localPosition, Quaternion localRotation)> startPoses, float blendT)
		{
			foreach (var (ragdollBoneData2, transform2) in _jointsAnimationBonesMap)
			{
				if (startPoses.TryGetValue(transform2, out (Vector3, Quaternion) value))
				{
					Quaternion localRotation = transform2.localRotation;
					transform2.localRotation = Quaternion.Slerp(value.Item2, localRotation, blendT);
					Transform bone = ragdollBoneData2.Bone;
					if (ragdollBoneData2.IsRotationOnlySync)
					{
						bone.localRotation = transform2.localRotation;
						continue;
					}
					Vector3 localPosition = transform2.localPosition;
					transform2.localPosition = Vector3.Lerp(value.Item1, localPosition, blendT);
					bone.SetPositionAndRotation(transform2.position, transform2.rotation);
				}
			}
		}

		private void RestoreNonPhysicalAnimationBoneLocalPositions()
		{
			if (_initialAnimationBoneLocalPositions == null)
			{
				return;
			}
			foreach (var (ragdollBoneData2, transform2) in _jointsAnimationBonesMap)
			{
				if ((!ragdollBoneData2.IsPhysical || ragdollBoneData2.IsRotationOnlySync) && _initialAnimationBoneLocalPositions.TryGetValue(transform2, out var value))
				{
					transform2.localPosition = value;
				}
			}
		}

		private void ResetJointTargets()
		{
			foreach (RagdollBoneData item in _jointsAnimationBonesMap.Keys.Where((RagdollBoneData bone) => bone.HasJoint))
			{
				item.Joint.targetRotation = Quaternion.identity;
				item.Joint.targetAngularVelocity = Vector3.zero;
				item.Joint.targetVelocity = Vector3.zero;
				item.Joint.targetPosition = Vector3.zero;
			}
		}

		private void ApplyPoseImmediate(Vector3 posePosition, Quaternion poseRotation)
		{
			Rigidbody rigidBody = RootPhysData.RigidBody;
			SwitchRagdollAnimator(active: false);
			Vector3 position = rigidBody.position;
			Quaternion quaternion = Quaternion.Inverse(rigidBody.rotation);
			for (int i = 0; i < _ragdollRbs.Length; i++)
			{
				Rigidbody rigidbody = _ragdollRbs[i];
				Vector3 vector = quaternion * (rigidbody.position - position);
				Quaternion quaternion2 = quaternion * rigidbody.rotation;
				Vector3 vector2 = posePosition + poseRotation * vector;
				Quaternion quaternion3 = poseRotation * quaternion2;
				rigidbody.position = vector2;
				rigidbody.rotation = quaternion3;
				rigidbody.linearVelocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.PublishTransform();
				if (base.HasStateAuthority && i < _ragdollSynchronizers.Count && _ragdollSynchronizers[i] != null)
				{
					_ragdollSynchronizers[i].Teleport(vector2, quaternion3);
				}
			}
			ResetJointTargets();
			SetRagdollReferencePosition();
			Physics.SyncTransforms();
		}

		private void CaptureInitialBonePoses()
		{
			_initialBonePoses = new Dictionary<RagdollBoneData, (Vector3, Quaternion)>(_jointsAnimationBonesMap.Count);
			_initialAnimationBoneLocalPositions = new Dictionary<Transform, Vector3>(_jointsAnimationBonesMap.Count);
			foreach (var (ragdollBoneData2, transform2) in _jointsAnimationBonesMap)
			{
				_initialBonePoses[ragdollBoneData2] = (ragdollBoneData2.Bone.localPosition, ragdollBoneData2.Bone.localRotation);
				_initialAnimationBoneLocalPositions[transform2] = transform2.localPosition;
			}
		}

		private bool TryPushSimulationReason(RagdollSimulationReasonEnum reason)
		{
			if (!_simulationReasons.Add(reason))
			{
				return false;
			}
			this.OnRagdollSimulationReasonAdded?.Invoke(reason);
			return true;
		}

		private bool TryPopSimulationReason(RagdollSimulationReasonEnum reason)
		{
			if (!_simulationReasons.Remove(reason))
			{
				return false;
			}
			this.OnRagdollSimulationReasonRemoved?.Invoke(reason);
			return true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SimulationReasonsMask = _SimulationReasonsMask;
			IsRagdollSimulated = _IsRagdollSimulated;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SimulationReasonsMask = SimulationReasonsMask;
			_IsRagdollSimulated = IsRagdollSimulated;
		}

		[NetworkRpcWeavedInvoker(447713943u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetInterpolationRpc_0040Invoker447713943([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out RigidbodyInterpolation value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RagdollEntity)context.TargetBehaviour).SetInterpolationRpc(value);
		}

		[NetworkRpcWeavedInvoker(3204686439u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ResetPoseRpc_0040Invoker3204686439([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RagdollEntity)context.TargetBehaviour).ResetPoseRpc();
		}

		[NetworkRpcWeavedInvoker(3920136252u)]
		[Preserve]
		[WeaverGenerated]
		protected static void AddSimulationReasonRpc_0040Invoker3920136252([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out RagdollSimulationReasonEnum value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RagdollEntity)context.TargetBehaviour).AddSimulationReasonRpc(value);
		}

		[NetworkRpcWeavedInvoker(36603363u)]
		[Preserve]
		[WeaverGenerated]
		protected static void AddSimulationReasonWithPoseRpc_0040Invoker36603363([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out RagdollSimulationReasonEnum value, 4);
			payloadReader.Read(out Vector3Compressed value2, 12);
			payloadReader.Read(out QuaternionCompressed value3, 16);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RagdollEntity)context.TargetBehaviour).AddSimulationReasonWithPoseRpc(value, value2, value3);
		}

		[NetworkRpcWeavedInvoker(1384075437u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RemoveSimulationReasonRpc_0040Invoker1384075437([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out RagdollSimulationReasonEnum value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RagdollEntity)context.TargetBehaviour).RemoveSimulationReasonRpc(value);
		}

		[NetworkRpcWeavedInvoker(3865049292u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RemoveAllSimulationReasonsRpc_0040Invoker3865049292([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RagdollEntity)context.TargetBehaviour).RemoveAllSimulationReasonsRpc();
		}

		[NetworkRpcWeavedInvoker(1006259398u)]
		[Preserve]
		[WeaverGenerated]
		protected static void ApplyPoseRpc_0040Invoker1006259398([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out Vector3Compressed value, 12);
			payloadReader.Read(out QuaternionCompressed value2, 16);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((RagdollEntity)context.TargetBehaviour).ApplyPoseRpc(value, value2);
		}
	}
}
