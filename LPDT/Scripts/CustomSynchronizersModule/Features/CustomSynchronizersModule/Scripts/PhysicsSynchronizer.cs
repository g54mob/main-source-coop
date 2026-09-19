using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.CustomSynchronizersModule.Scripts
{
	[NetworkBehaviourWeaved(16)]
	public class PhysicsSynchronizer : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface, INetworkTRSPTeleport
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _positionInterpolationSpeed = 10f;

		[SerializeField]
		private float _rotationInterpolationSpeed = 20f;

		[SerializeField]
		private float _maxPositionError = 10f;

		[SerializeField]
		private float _maxRotationError = 45f;

		[SerializeField]
		private bool _isPositionSynchronizationEnabled = true;

		[SerializeField]
		private bool _isRotationSynchronizationEnabled = true;

		[SerializeField]
		private bool _isLinearVelocitySynchronizationEnabled = true;

		[SerializeField]
		private bool _isAngularVelocitySynchronizationEnabled = true;

		[SerializeField]
		private bool _usePositionSendThreshold;

		[SerializeField]
		private float _sendThreshold = 0.02f;

		[SerializeField]
		private bool _useRotationSendThreshold;

		[SerializeField]
		private float _rotationSendThreshold = 1f;

		[SerializeField]
		private bool _useLinearVelocitySendThreshold;

		[SerializeField]
		private float _linearVelocitySendThreshold = 0.02f;

		[SerializeField]
		private bool _useAngularVelocitySendThreshold;

		[SerializeField]
		private float _angularVelocitySendThreshold = 0.02f;

		[Tooltip("If enabled, proxy Spawned skips Apply until PositionWriteCounter > SyncEnableBaseline (avoids default origin snap). Off by default — enable per prefab (e.g. Snake).")]
		[SerializeField]
		private bool _gateProxySpawnedUntilValidPose;

		[Tooltip("If enabled, DISABLING synchronization also retires the stored pose (SyncEnableBaseline = PositionWriteCounter), so a later authority handover cannot Move the body onto a pose from before the disable. Off by default — enable per prefab whose sync is toggled at runtime (e.g. ArmEnd*).")]
		[SerializeField]
		private bool _retirePoseOnSynchronizationDisabled;

		[Tooltip("Proxy side: follow the networked pose with Rigidbody.MovePosition/MoveRotation instead of assigning .position/.rotation, so Unity's own Rigidbody interpolation draws the frames between fixed steps. Needs the Rigidbody's Interpolate setting and a kinematic proxy. Off by default — enable per prefab on bodies that are WATCHED closely (the player character).")]
		[SerializeField]
		private bool _useRigidbodyInterpolationOnProxy;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Position", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3Compressed _Position;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Rotation", 3, 4)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private QuaternionCompressed _Rotation;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("LinearVelocity", 7, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3Compressed _LinearVelocity;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("AngularVelocity", 10, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3Compressed _AngularVelocity;

		[WeaverGenerated]
		[DefaultForProperty("TeleportationKey", 13, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _TeleportationKey;

		[WeaverGenerated]
		[DefaultForProperty("PositionWriteCounter", 14, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _PositionWriteCounter;

		[WeaverGenerated]
		[DefaultForProperty("SyncEnableBaseline", 15, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _SyncEnableBaseline;

		private bool _needToBumpWriteCounter;

		private bool _isInitialized;

		private Action _enqueuedStateAuthorityAction;

		private bool _isSynchronizationActive = true;

		private Vector3 _lastSentPosition;

		private Quaternion _lastSentRotation;

		private Vector3 _lastSentLinearVelocity;

		private Vector3 _lastSentAngularVelocity;

		private bool _forcePositionSync = true;

		private bool _forceRotationSync = true;

		private bool _forceLinearVelocitySync = true;

		private bool _forceAngularVelocitySync = true;

		private int _lastTeleportationKey = -1;

		private Vector3 _lastObservedNetworkPosition;

		private int _staleNetworkPositionSteps;

		private const int PROXY_REST_STALE_STEPS = 25;

		private const float PROXY_REST_MAX_VELOCITY_SQR = 0.09f;

		private const float PROXY_REST_POSITION_EPSILON = 0.01f;

		private const float PROXY_REST_ROTATION_EPSILON = 2f;

		[Networked(AllowPrediction = true)]
		[NetworkedWeaved(0, 3)]
		public unsafe Vector3Compressed Position
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.Position. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3Compressed*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.Position. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3Compressed*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked(AllowPrediction = true)]
		[NetworkedWeaved(3, 4)]
		public unsafe QuaternionCompressed Rotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.Rotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(QuaternionCompressed*)(Ptr + 3);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.Rotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(QuaternionCompressed*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(7, 3)]
		public unsafe Vector3Compressed LinearVelocity
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.LinearVelocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3Compressed*)(Ptr + 7);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.LinearVelocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3Compressed*)(Ptr + 7) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(10, 3)]
		public unsafe Vector3Compressed AngularVelocity
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.AngularVelocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3Compressed*)(Ptr + 10);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.AngularVelocity. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3Compressed*)(Ptr + 10) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(13, 1)]
		private unsafe int TeleportationKey
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.TeleportationKey. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[13];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.TeleportationKey. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[13] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(14, 1)]
		private unsafe int PositionWriteCounter
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.PositionWriteCounter. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[14];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.PositionWriteCounter. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[14] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(15, 1)]
		private unsafe int SyncEnableBaseline
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.SyncEnableBaseline. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[15];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PhysicsSynchronizer.SyncEnableBaseline. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[15] = value;
			}
		}

		public bool HasValidNetworkPose => PositionWriteCounter > SyncEnableBaseline;

		public Rigidbody Rigidbody => _rigidbody;

		public bool HasValidSyncedPose
		{
			get
			{
				if (!_isInitialized)
				{
					return false;
				}
				if (base.HasStateAuthority)
				{
					return true;
				}
				return PositionWriteCounter > SyncEnableBaseline;
			}
		}

		public override void Spawned()
		{
			base.Spawned();
			_isInitialized = true;
			if (base.HasStateAuthority)
			{
				_lastSentPosition = _rigidbody.position;
				_lastSentRotation = _rigidbody.rotation;
				_lastSentLinearVelocity = _rigidbody.linearVelocity;
				_lastSentAngularVelocity = _rigidbody.angularVelocity;
				_forcePositionSync = true;
				_forceRotationSync = true;
				_forceLinearVelocitySync = true;
				_forceAngularVelocitySync = true;
				_needToBumpWriteCounter = true;
				bool num = TryWritePosition();
				TryWriteRotation();
				if (num && _needToBumpWriteCounter)
				{
					PositionWriteCounter++;
					_needToBumpWriteCounter = false;
				}
			}
			else if (!ShouldBlockProxyInterpolation() && (!_gateProxySpawnedUntilValidPose || PositionWriteCounter > SyncEnableBaseline))
			{
				ApplyPhysicsPosition(applyImmediate: true);
				ApplyPhysicsRotation(applyImmediate: true);
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.HasStateAuthority || !_isInitialized || !_isSynchronizationActive)
			{
				return;
			}
			bool flag = _forcePositionSync || _forceRotationSync;
			if (_rigidbody.IsSleeping() && !flag)
			{
				return;
			}
			bool num = TryWritePosition();
			TryWriteRotation();
			if (_isLinearVelocitySynchronizationEnabled)
			{
				Vector3 linearVelocity = _rigidbody.linearVelocity;
				if (!_useLinearVelocitySendThreshold || _forceLinearVelocitySync || Vector3.Distance(linearVelocity, _lastSentLinearVelocity) >= _linearVelocitySendThreshold)
				{
					LinearVelocity = linearVelocity;
					_lastSentLinearVelocity = linearVelocity;
					_forceLinearVelocitySync = false;
				}
			}
			if (_isAngularVelocitySynchronizationEnabled)
			{
				Vector3 angularVelocity = _rigidbody.angularVelocity;
				if (!_useAngularVelocitySendThreshold || _forceAngularVelocitySync || Vector3.Distance(angularVelocity, _lastSentAngularVelocity) >= _angularVelocitySendThreshold)
				{
					AngularVelocity = angularVelocity;
					_lastSentAngularVelocity = angularVelocity;
					_forceAngularVelocitySync = false;
				}
			}
			if (num && _needToBumpWriteCounter)
			{
				PositionWriteCounter++;
				_needToBumpWriteCounter = false;
			}
		}

		private bool TryWritePosition()
		{
			if (!_isPositionSynchronizationEnabled)
			{
				return false;
			}
			Vector3 position = _rigidbody.position;
			if (_usePositionSendThreshold && !_forcePositionSync && !(Vector3.Distance(position, _lastSentPosition) >= _sendThreshold))
			{
				return false;
			}
			Position = position;
			_lastSentPosition = position;
			_forcePositionSync = false;
			return true;
		}

		private void TryWriteRotation()
		{
			if (_isRotationSynchronizationEnabled)
			{
				Quaternion rotation = _rigidbody.rotation;
				if (!_useRotationSendThreshold || _forceRotationSync || Quaternion.Angle(rotation, _lastSentRotation) >= _rotationSendThreshold)
				{
					Rotation = rotation;
					_lastSentRotation = rotation;
					_forceRotationSync = false;
				}
			}
		}

		private void FixedUpdate()
		{
			if (_enqueuedStateAuthorityAction != null)
			{
				_enqueuedStateAuthorityAction();
				_enqueuedStateAuthorityAction = null;
			}
			if (_isInitialized && _isSynchronizationActive && !(base.Object == null) && base.Object.IsValid && !base.HasStateAuthority && PositionWriteCounter > SyncEnableBaseline && !ShouldBlockProxyInterpolation() && !TryRestProxyAtNetworkPose())
			{
				ApplyPhysicsPosition();
				ApplyPhysicsRotation();
			}
		}

		private bool TryRestProxyAtNetworkPose()
		{
			if (!_isPositionSynchronizationEnabled)
			{
				return false;
			}
			Vector3 vector = Position;
			if ((vector - _lastObservedNetworkPosition).sqrMagnitude > 1E-06f)
			{
				_lastObservedNetworkPosition = vector;
				_staleNetworkPositionSteps = 0;
				return false;
			}
			if (_staleNetworkPositionSteps < 25)
			{
				_staleNetworkPositionSteps++;
				return false;
			}
			if (((Vector3)LinearVelocity).sqrMagnitude > 0.09f || ((Vector3)AngularVelocity).sqrMagnitude > 0.09f)
			{
				return false;
			}
			if (_rigidbody.isKinematic)
			{
				return true;
			}
			Quaternion quaternion = (_isRotationSynchronizationEnabled ? ((Quaternion)Rotation).normalized : _rigidbody.rotation);
			if (!(Vector3.Distance(_rigidbody.position, vector) <= 0.01f) || !(Quaternion.Angle(_rigidbody.rotation, quaternion) <= 2f))
			{
				_rigidbody.position = vector;
				_rigidbody.rotation = quaternion;
				if (!_rigidbody.isKinematic)
				{
					_rigidbody.linearVelocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
				}
			}
			if (!_rigidbody.IsSleeping())
			{
				_rigidbody.Sleep();
			}
			return true;
		}

		public void StateAuthorityChanged()
		{
			if (!_isInitialized || base.Object == null || !base.Object.IsValid || PositionWriteCounter <= SyncEnableBaseline)
			{
				return;
			}
			Vector3 snapshotPosition = Position;
			Vector3 snapshotLinearVelocity = LinearVelocity;
			Quaternion snapshotRotation = ((Quaternion)Rotation).normalized;
			Vector3 snapshotAngularVelocity = AngularVelocity;
			if (!base.HasStateAuthority)
			{
				float deltaTime = base.Runner.DeltaTime;
				Vector3 vector = snapshotPosition + snapshotLinearVelocity * deltaTime;
				Quaternion quaternion = Quaternion.AngleAxis(snapshotAngularVelocity.magnitude * 57.29578f * deltaTime, snapshotAngularVelocity.normalized) * snapshotRotation;
				Position = vector;
				Rotation = quaternion;
				return;
			}
			_forcePositionSync = true;
			_lastSentPosition = snapshotPosition;
			_forceRotationSync = true;
			_lastSentRotation = snapshotRotation;
			_forceLinearVelocitySync = true;
			_lastSentLinearVelocity = snapshotLinearVelocity;
			_forceAngularVelocitySync = true;
			_lastSentAngularVelocity = snapshotAngularVelocity;
			_enqueuedStateAuthorityAction = delegate
			{
				if (!_rigidbody.isKinematic)
				{
					_rigidbody.linearVelocity = snapshotLinearVelocity;
					_rigidbody.angularVelocity = snapshotAngularVelocity;
				}
				_rigidbody.Move(snapshotPosition, snapshotRotation);
			};
		}

		public override void Render()
		{
			base.Render();
			if (!(base.Object == null) && base.Object.IsValid)
			{
				if (_lastTeleportationKey != TeleportationKey && !base.HasStateAuthority && !ShouldBlockProxyInterpolation())
				{
					ApplyPhysicsPosition(applyImmediate: true);
					ApplyPhysicsRotation(applyImmediate: true);
				}
				_lastTeleportationKey = TeleportationKey;
			}
		}

		public void EnableSynchronization()
		{
			SetSynchronizationActiveRpc(active: true);
		}

		public void DisableSynchronization()
		{
			SetSynchronizationActiveRpc(active: false);
		}

		public void SnapProxyToNetworkedPoseImmediate()
		{
			if (_isInitialized && !base.HasStateAuthority && !(base.Object == null) && base.Object.IsValid && HasValidSyncedPose && !ShouldBlockProxyInterpolation())
			{
				ApplyPhysicsPosition(applyImmediate: true);
				ApplyPhysicsRotation(applyImmediate: true);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.All, Key = 2441995450u)]
		private void SetSynchronizationActiveRpc([RpcPayload(4)] bool active)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(active);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(2441995450u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.CustomSynchronizersModule.Scripts.PhysicsSynchronizer::SetSynchronizationActiveRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(active);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (active && !_isSynchronizationActive)
			{
				_forcePositionSync = true;
				_forceRotationSync = true;
				_forceLinearVelocitySync = true;
				_forceAngularVelocitySync = true;
				if (base.HasStateAuthority)
				{
					SyncEnableBaseline = PositionWriteCounter;
				}
			}
			if (active && base.HasStateAuthority)
			{
				_needToBumpWriteCounter = true;
			}
			if (!active && _retirePoseOnSynchronizationDisabled && base.HasStateAuthority)
			{
				SyncEnableBaseline = PositionWriteCounter;
			}
			_isSynchronizationActive = active;
		}

		private bool ShouldBlockProxyInterpolation()
		{
			if (!base.HasStateAuthority && base.Object != null && base.Object.IsValid && base.Object.HasInputAuthority)
			{
				return PlayerSpawnLock.ShouldBlockPositionOverride();
			}
			return false;
		}

		private void ApplyPhysicsPosition(bool applyImmediate = false)
		{
			if (ShouldBlockProxyInterpolation())
			{
				return;
			}
			int num;
			Vector3 vector;
			if (!applyImmediate)
			{
				num = ((Vector3.Distance(_rigidbody.position, Position) > _maxPositionError) ? 1 : 0);
				if (num == 0)
				{
					vector = Vector3.Lerp(_rigidbody.position, Position, Time.fixedDeltaTime * _positionInterpolationSpeed);
					goto IL_0069;
				}
			}
			else
			{
				num = 1;
			}
			vector = Position;
			goto IL_0069;
			IL_0069:
			Vector3 position = vector;
			if (num == 0 && ShouldFollowProxyWithMove())
			{
				_rigidbody.MovePosition(position);
			}
			else
			{
				_rigidbody.position = position;
			}
		}

		private void ApplyPhysicsRotation(bool applyImmediate = false)
		{
			if (ShouldBlockProxyInterpolation())
			{
				return;
			}
			Quaternion normalized = ((Quaternion)Rotation).normalized;
			int num;
			Quaternion quaternion;
			if (!applyImmediate)
			{
				num = ((Quaternion.Angle(_rigidbody.rotation, normalized) > _maxRotationError) ? 1 : 0);
				if (num == 0)
				{
					quaternion = Quaternion.Lerp(_rigidbody.rotation, normalized, Time.fixedDeltaTime * _rotationInterpolationSpeed);
					goto IL_005f;
				}
			}
			else
			{
				num = 1;
			}
			quaternion = normalized;
			goto IL_005f;
			IL_005f:
			Quaternion quaternion2 = quaternion;
			if (num == 0 && ShouldFollowProxyWithMove())
			{
				_rigidbody.MoveRotation(quaternion2);
			}
			else
			{
				_rigidbody.rotation = quaternion2;
			}
		}

		private bool ShouldFollowProxyWithMove()
		{
			if (_useRigidbodyInterpolationOnProxy && _rigidbody.isKinematic)
			{
				return _rigidbody.interpolation != RigidbodyInterpolation.None;
			}
			return false;
		}

		public void Teleport(Vector3? position = null, Quaternion? rotation = null)
		{
			if (position.HasValue)
			{
				_rigidbody.position = position.Value;
				_rigidbody.linearVelocity = Vector3.zero;
				_forcePositionSync = true;
				_forceLinearVelocitySync = true;
			}
			if (rotation.HasValue)
			{
				_rigidbody.rotation = rotation.Value;
				_rigidbody.angularVelocity = Vector3.zero;
				_forceRotationSync = true;
				_forceAngularVelocitySync = true;
			}
			_rigidbody.PublishTransform();
			if (base.HasStateAuthority)
			{
				TeleportationKey++;
			}
		}

		public void ForcePoseResync()
		{
			_forcePositionSync = true;
			_forceRotationSync = true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Position = _Position;
			Rotation = _Rotation;
			LinearVelocity = _LinearVelocity;
			AngularVelocity = _AngularVelocity;
			TeleportationKey = _TeleportationKey;
			PositionWriteCounter = _PositionWriteCounter;
			SyncEnableBaseline = _SyncEnableBaseline;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Position = Position;
			_Rotation = Rotation;
			_LinearVelocity = LinearVelocity;
			_AngularVelocity = AngularVelocity;
			_TeleportationKey = TeleportationKey;
			_PositionWriteCounter = PositionWriteCounter;
			_SyncEnableBaseline = SyncEnableBaseline;
		}

		[NetworkRpcWeavedInvoker(2441995450u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetSynchronizationActiveRpc_0040Invoker2441995450([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((PhysicsSynchronizer)context.TargetBehaviour).SetSynchronizationActiveRpc(value);
		}
	}
}
