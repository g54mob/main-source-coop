#define TRACE
#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Fusion
{
	[DisallowMultipleComponent]
	[HelpURL("https://doc.photonengine.com/fusion/v2/manual/sync-components/network-transform")]
	[NetworkBehaviourWeaved(21)]
	public sealed class NetworkTransform : NetworkTRSP, INetworkTRSPTeleport, IBeforeAllTicks, IPublicFacingInterface, IAfterAllTicks, IBeforeCopyPreviousState, IAfterClientPredictionReset
	{
		[Flags]
		public enum NetworkTransformFlags
		{
			None = 0,
			DisableSharedModeInterpolation = 1
		}

		public delegate void CustomForecastDelegate(NetworkTransform networkTransform, float physicsDt, AbstractPhysicsBody physicsBody, float ticksToExtrapolate, ref KinematicSnapshot localSnapshot, ref KinematicSnapshot remoteSnapshot, out KinematicSnapshot extrapolatedSnapshot);

		public delegate void ApplyCorrectionDelegate(NetworkTransform networkTransform, float physicsDt, float physicsFps, AbstractPhysicsBody physicsBody, float timeSinceLastOnCollisionEnter, ref KinematicSnapshot localSnapshot, ref KinematicSnapshot remoteSnapshot, ref KinematicSnapshot extrapolatedSnapshot);

		public delegate float CustomErrorDetectionDelegate(NetworkTransform networkTransform, float physicsDt, AbstractPhysicsBody physicsBody, Vector3 previousLocalPosition, Vector3 previousExtrapolatedPosition, ref KinematicSnapshot localSnapshot, ref KinematicSnapshot extrapolatedSnapshot);

		[SerializeField]
		[InlineHelp]
		public bool SyncScale = false;

		[SerializeField]
		[InlineHelp]
		public bool SyncParent = false;

		private Tick _initial;

		private Transform _transform;

		private bool _simulation;

		private bool _aoiEnabled;

		private bool _aoiAutoUpdateOriginal;

		[SerializeField]
		[InlineHelp]
		private bool _autoAOIOverride = true;

		[SerializeField]
		[InlineHelp]
		public NetworkTransformFlags ConfigFlags = NetworkTransformFlags.None;

		private bool _render;

		private Vector3 _renderPosition;

		private Vector3 _renderScale;

		private Quaternion _renderRotation;

		private Transform _renderParent;

		public CustomForecastDelegate CustomForecast;

		public ApplyCorrectionDelegate CustomApplyCorrection;

		public CustomErrorDetectionDelegate CustomErrorDetection;

		private AbstractPhysicsBody _physicsBody;

		private int _previousTeleportKey;

		[HideInInspector]
		public PhysicsSettings PhysicsSettings = new PhysicsSettings();

		private NetworkTRSPData _remoteTRSPData;

		private NetworkPhysicsData _remotePhysicsData;

		private AbstractPhysicsBody.BodyInterpolation _previousInterpolation;

		[NonSerialized]
		private Vector3 _physicsBodyLinearVelocity;

		[NonSerialized]
		private Vector3 _remotePosition;

		[NonSerialized]
		private Quaternion _remoteRotation = Quaternion.identity;

		[NonSerialized]
		private Quaternion _extrapolatedRotation = Quaternion.identity;

		private bool _hasRemoteData = false;

		private Vector3 _extrapolatedPosition;

		private Vector3 _previousExtrapolatedPosition;

		private Vector3 _position;

		private Vector3 _previousPosition;

		private float _timeSinceLastImpactfulCollision;

		private float _accruedErrorTime;

		private float _accruedRemoteSleepIgnoreTime;

		private bool _isColliding = false;

		private bool _sleepNextUpdate = false;

		private bool _firstExecution = true;

		private float _physicsFps;

		[NonSerialized]
		public NetworkTransformTrace CurrentTrace = null;

		public bool AutoUpdateAreaOfInterestOverride
		{
			get
			{
				return _autoAOIOverride;
			}
			set
			{
				_autoAOIOverride = (_aoiAutoUpdateOriginal = value);
			}
		}

		public NetworkPhysicsData PhysicsData => base.StateBufferIsValid ? ReinterpretState<NetworkPhysicsData>(14) : default(NetworkPhysicsData);

		private ref NetworkPhysicsData PhysicsState
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref ReinterpretState<NetworkPhysicsData>(14);
			}
		}

		private ref NetworkPhysicsData RemotePhysicsData
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (base.Object.IsInSimulation && _hasRemoteData)
				{
					return ref _remotePhysicsData;
				}
				return ref PhysicsState;
			}
		}

		private ref NetworkTRSPData RemoteTRSPData
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (base.Object.IsInSimulation && _hasRemoteData)
				{
					return ref _remoteTRSPData;
				}
				return ref base.State;
			}
		}

		public bool HasForecastEnabled
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return PhysicsSettings.ForecastEnabled && (base.Runner?.Config.PhysicsForecast ?? false);
			}
		}

		public Vector3 PositionError
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private set;
		}

		public Vector3 PreviousPositionError
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private set;
		}

		public bool HasPhysicsBody
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _physicsBody?.Valid ?? false;
			}
		}

		public AbstractPhysicsBody PhysicsBody
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return HasPhysicsBody ? _physicsBody : null;
			}
		}

		private void Awake()
		{
			_aoiAutoUpdateOriginal = _autoAOIOverride;
			TryGetComponent<Transform>(out _transform);
			PhysicsInit();
		}

		private void CopyToEngine(bool allowUpdateTransform)
		{
			if (SyncParent && base.IsMainTRSP)
			{
				NetworkTRSP.SetParentTransform(this, _transform, base.State.Parent);
			}
			if (SyncScale)
			{
				_transform.localScale = base.State.Scale;
			}
			bool flag = PhysicsOverrideCopyToEngine();
			if (!flag && allowUpdateTransform)
			{
				ref NetworkTRSPData state = ref base.State;
				_transform.localPosition = state.Position;
				_transform.localRotation = state.Rotation;
			}
		}

		private void CopyToBuffer(bool onSpawn)
		{
			Transform transform = _transform;
			if (!PhysicsOverrideCopyToBuffer(onSpawn))
			{
				ref NetworkTRSPData state = ref base.State;
				state.Position = transform.localPosition;
				state.Rotation = transform.localRotation;
			}
			if (SyncScale)
			{
				base.State.Scale = transform.localScale;
			}
			if (!base.IsMainTRSP)
			{
				return;
			}
			Transform parent = transform.parent;
			bool flag = parent;
			if (_aoiEnabled && _autoAOIOverride)
			{
				if (flag)
				{
					NetworkTRSP.ResolveAOIOverride(this, parent);
				}
				else
				{
					SetAreaOfInterestOverride(null);
				}
			}
			if (!SyncParent)
			{
				return;
			}
			if (flag)
			{
				if (parent.TryGetComponent<NetworkBehaviour>(out var component))
				{
					base.State.Parent = component;
				}
				else
				{
					base.State.Parent = NetworkTRSPData.NonNetworkedParent;
				}
			}
			else
			{
				base.State.Parent = default(NetworkBehaviourId);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool CanInterpolateCheckModeAndDisabled(SimulationModes simulationMode, bool disableSharedModeInterpolation, Topologies topology, bool hasStateAuthority, GameMode gameMode)
		{
			if (simulationMode == SimulationModes.Server || (disableSharedModeInterpolation && ((topology == Topologies.Shared && hasStateAuthority) || gameMode == GameMode.Single)))
			{
				return false;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool CanInterpolateCheckForecastAndBody(bool hasForecastEnabled, bool hasPhysicsBody, bool isKinematic, bool hasStateAuthority)
		{
			if (!hasForecastEnabled)
			{
				if (hasPhysicsBody && !isKinematic && hasStateAuthority)
				{
					return false;
				}
				return true;
			}
			if (hasPhysicsBody && !isKinematic)
			{
				return false;
			}
			return true;
		}

		private bool CanInterpolate()
		{
			SimulationModes mode = base.Runner.Mode;
			bool disableSharedModeInterpolation = ConfigFlags.Has(NetworkTransformFlags.DisableSharedModeInterpolation);
			Topologies topology = base.Runner.Topology;
			bool hasStateAuthority = base.HasStateAuthority;
			GameMode gameMode = base.Runner.GameMode;
			if (!CanInterpolateCheckModeAndDisabled(mode, disableSharedModeInterpolation, topology, hasStateAuthority, gameMode))
			{
				return false;
			}
			bool hasForecastEnabled = HasForecastEnabled;
			bool hasPhysicsBody = HasPhysicsBody;
			bool isKinematic = (hasForecastEnabled ? PhysicsState.Flags.Has(NetworkRigidbodyFlags.IsKinematic) : (!hasPhysicsBody || _physicsBody.Kinematic));
			return CanInterpolateCheckForecastAndBody(hasForecastEnabled, hasPhysicsBody, isKinematic, base.HasStateAuthority);
		}

		void IBeforeAllTicks.BeforeAllTicks(bool resimulation, int tickCount)
		{
			if (!CanInterpolate())
			{
				if (resimulation && HasPhysicsBody && PhysicsBody.Kinematic != PhysicsState.Flags.Has(NetworkRigidbodyFlags.IsKinematic))
				{
					CopyToEngine(allowUpdateTransform: true);
				}
				return;
			}
			if (resimulation)
			{
				CopyToEngine(allowUpdateTransform: true);
				return;
			}
			if (_render && _simulation)
			{
				if (base.IsMainTRSP && SyncParent && base.transform.parent == _renderParent)
				{
					NetworkTRSP.SetParentTransform(this, _transform, base.State.Parent);
				}
				if (_transform.localPosition == _renderPosition && _transform.localRotation == _renderRotation)
				{
					Assert.Check(!HasPhysicsBody || !HasForecastEnabled || !PhysicsState.Flags.HasNot(NetworkRigidbodyFlags.IsKinematic), "!(HasPhysicsBody && HasForecastEnabled && PhysicsState.Flags.HasNot(NetworkRigidbodyFlags.IsKinematic))");
					ref NetworkTRSPData state = ref base.State;
					_transform.localPosition = state.Position;
					_transform.localRotation = state.Rotation;
				}
				if (SyncScale && _transform.localScale == _renderScale)
				{
					_transform.localScale = base.State.Scale;
				}
			}
			_render = false;
			_simulation = false;
		}

		void IAfterAllTicks.AfterAllTicks(bool resimulation, int tickCount)
		{
			CopyToBuffer(onSpawn: false);
			_simulation = true;
		}

		void IBeforeCopyPreviousState.BeforeCopyPreviousState()
		{
			CopyToBuffer(onSpawn: false);
		}

		public void Teleport(Vector3? position = null, Quaternion? rotation = null)
		{
			if (!PhysicsOverrideTeleport(position, rotation))
			{
				NetworkTRSP.Teleport(this, _transform, position, rotation);
			}
		}

		public override void SetAreaOfInterestOverride(NetworkObject obj)
		{
			base.SetAreaOfInterestOverride(obj);
			if ((bool)obj)
			{
				_autoAOIOverride = false;
			}
			else
			{
				_autoAOIOverride = _aoiAutoUpdateOriginal;
			}
		}

		protected internal override void OnEnable()
		{
			base.OnEnable();
			if (base.StateBufferIsValid && base.HasStateAuthority)
			{
				CopyToBuffer(onSpawn: true);
			}
		}

		public override void Spawned()
		{
			if (!_transform)
			{
				Awake();
			}
			_aoiEnabled = base.Runner.Config.Simulation.AreaOfInterestEnabled;
			_initial = default(Tick);
			if (base.Object.HasStateAuthority && !base.Object.Meta.HasSnapshots)
			{
				CopyToBuffer(onSpawn: true);
			}
			else
			{
				CopyToEngine(allowUpdateTransform: true);
			}
			PhysicsSpawned();
		}

		public override void Render()
		{
			if (!CanInterpolate())
			{
				return;
			}
			if (!base.HasStateAuthority)
			{
				PhysicsOverrideCopyToEngine();
			}
			if (NetworkTRSP.Render(this, _transform, SyncScale, SyncParent, local: true, HasForecastEnabled && HasPhysicsBody, ref _initial))
			{
				_render = true;
				_renderPosition = _transform.localPosition;
				_renderRotation = _transform.localRotation;
				if (SyncScale)
				{
					_renderScale = _transform.localScale;
				}
				_renderParent = _transform.parent;
			}
		}

		private void OnDrawGizmos()
		{
			PhysicsDrawGizmos();
		}

		public bool TriggerLocalOverridesExtrapolated(bool force = true)
		{
			if (base.HasStateAuthority)
			{
				return false;
			}
			if (!force && _timeSinceLastImpactfulCollision < PhysicsSettings.ImpactCorrectionTimeComplete)
			{
				return false;
			}
			_timeSinceLastImpactfulCollision = 0f;
			return true;
		}

		public void PhysicsInit()
		{
			if (!HasPhysicsBody)
			{
				Rigidbody2D component2;
				if (TryGetComponent<Rigidbody>(out var component))
				{
					_physicsBody = new PhysicsBody3D(component);
				}
				else if (TryGetComponent<Rigidbody2D>(out component2))
				{
					_physicsBody = new PhysicsBody2D(component2);
				}
				else
				{
					_physicsBody = null;
				}
			}
		}

		private void PhysicsSpawned()
		{
			if (!HasPhysicsBody)
			{
				return;
			}
			if (!HasForecastEnabled)
			{
				if (PhysicsSettings.SetForecastDisabledProxyRigidbodiesToKinematic)
				{
					NetworkRunner runner = base.Runner;
					if ((object)runner != null && !runner.Config.DoNotSetForecastDisabledProxyRigidbodiesToKinematic && !base.Object.IsInSimulation)
					{
						_physicsBody.Kinematic = true;
					}
				}
				return;
			}
			_physicsFps = 1f / Time.fixedDeltaTime;
			_firstExecution = true;
			if (!PhysicsState.Flags.Has(NetworkRigidbodyFlags.IsKinematic))
			{
				CopyToRemoteSnapshot(out var remoteSnapshot);
				_physicsBody.Position = remoteSnapshot.WorldPosition;
				_physicsBody.Rotation = remoteSnapshot.WorldRotation;
				_physicsBody.LinearVelocity = remoteSnapshot.LinearVelocity;
				_physicsBody.AngularVelocity = remoteSnapshot.AngularVelocity;
				_transform.position = _physicsBody.Position;
				_transform.rotation = _physicsBody.Rotation;
			}
		}

		private bool PhysicsOverrideCopyToBuffer(bool onSpawn)
		{
			if (!HasPhysicsBody || !HasForecastEnabled)
			{
				return false;
			}
			ref NetworkPhysicsData physicsState = ref PhysicsState;
			NetworkRigidbodyFlags flags = _physicsBody.Flags;
			bool flag = flags.Has(NetworkRigidbodyFlags.IsKinematic);
			if (PhysicsSettings.SetPhysicsInterpolationToDisabledWhenKinematic && (flag != physicsState.Flags.Has(NetworkRigidbodyFlags.IsKinematic) || onSpawn))
			{
				if (flag)
				{
					_previousInterpolation = _physicsBody.Interpolation;
					_physicsBody.Interpolation = AbstractPhysicsBody.BodyInterpolation.None;
				}
				else if (!onSpawn)
				{
					_physicsBody.Interpolation = _previousInterpolation;
				}
			}
			physicsState.FlagsAndConstraints = (flags: flags, constraints: _physicsBody.EncodedConstraints);
			if (flag)
			{
				return false;
			}
			physicsState.LinearVelocity = _physicsBody.LinearVelocity;
			physicsState.AngularVelocity = _physicsBody.AngularVelocity;
			ref NetworkTRSPData state = ref base.State;
			if (onSpawn)
			{
				state.Position = _transform.position;
				state.Rotation = _transform.rotation;
				_physicsBody.Position = state.Position;
				_physicsBody.Rotation = state.Rotation;
			}
			else
			{
				state.Position = _physicsBody.Position;
				state.Rotation = _physicsBody.Rotation;
			}
			return true;
		}

		private bool PhysicsOverrideCopyToEngine()
		{
			if (!HasPhysicsBody || !HasForecastEnabled)
			{
				return false;
			}
			var (flag, encodedConstraints) = PhysicsState.FlagsAndConstraints;
			_physicsBody.EncodedConstraints = encodedConstraints;
			bool flag2 = flag.Has(NetworkRigidbodyFlags.IsKinematic);
			if (flag2 != _physicsBody.Kinematic)
			{
				_physicsBody.Kinematic = flag2;
				if (PhysicsSettings.SetPhysicsInterpolationToDisabledWhenKinematic)
				{
					if (flag2)
					{
						_previousInterpolation = _physicsBody.Interpolation;
						_physicsBody.Interpolation = AbstractPhysicsBody.BodyInterpolation.None;
					}
					else
					{
						_physicsBody.Interpolation = _previousInterpolation;
					}
				}
			}
			if (flag2)
			{
				return false;
			}
			return true;
		}

		private bool PhysicsOverrideTeleport(Vector3? position = null, Quaternion? rotation = null)
		{
			if (!HasPhysicsBody || !HasForecastEnabled || PhysicsState.Flags.Has(NetworkRigidbodyFlags.IsKinematic))
			{
				return false;
			}
			ref NetworkTRSPData state = ref base.State;
			if (position.HasValue)
			{
				_transform.position = position.Value;
				state.Position = _transform.position;
				_physicsBody.Position = position.Value;
			}
			if (rotation.HasValue)
			{
				_transform.rotation = rotation.Value;
				state.Rotation = _transform.rotation;
				_physicsBody.Rotation = rotation.Value;
			}
			base.State.TeleportKey++;
			return true;
		}

		internal (bool shouldCorrect, bool immediateMove) DetectError(float physicsDt, ref KinematicSnapshot localSnapshot, ref KinematicSnapshot extrapolatedSnapshot)
		{
			float num = Vector3.Distance(localSnapshot.WorldPosition, extrapolatedSnapshot.WorldPosition);
			float num2 = Quaternion.Angle(localSnapshot.WorldRotation, extrapolatedSnapshot.WorldRotation);
			if (!(num >= PhysicsSettings.MinLinearDetectedError) && !(num2 >= PhysicsSettings.MinAngularDetectedError))
			{
				_accruedErrorTime = 0f;
				return (shouldCorrect: false, immediateMove: false);
			}
			if ((PhysicsSettings.MaxLinearError > 0f && num > PhysicsSettings.MaxLinearError) || (PhysicsSettings.MaxAngularError > 0f && num2 > PhysicsSettings.MaxAngularError))
			{
				return (shouldCorrect: true, immediateMove: true);
			}
			float num3 = 0f;
			num3 = ((CustomErrorDetection == null) ? NetworkTransformHelpers.DefaultStalledErrorCorrectionDetection(this, physicsDt, _physicsBody, _previousPosition, _previousExtrapolatedPosition, ref localSnapshot, ref extrapolatedSnapshot) : CustomErrorDetection(this, physicsDt, _physicsBody, _previousPosition, _previousExtrapolatedPosition, ref localSnapshot, ref extrapolatedSnapshot));
			_accruedErrorTime = Mathf.Max(_accruedErrorTime + num3 * physicsDt, 0f);
			RecordStallScoreAndProgress(num3, _accruedErrorTime);
			if (_accruedErrorTime > PhysicsSettings.MaxErrorTotalTime)
			{
				_accruedErrorTime = 0f;
				return (shouldCorrect: true, immediateMove: true);
			}
			bool flag = RemotePhysicsData.Flags.Has(NetworkRigidbodyFlags.IsSleeping);
			if (flag && flag != _physicsBody.Sleeping)
			{
				_accruedRemoteSleepIgnoreTime += physicsDt;
				if (_accruedRemoteSleepIgnoreTime >= PhysicsSettings.MaxRemoteSleepIgnoreTime)
				{
					_accruedRemoteSleepIgnoreTime = 0f;
					_sleepNextUpdate = true;
					return (shouldCorrect: true, immediateMove: true);
				}
			}
			else
			{
				_accruedRemoteSleepIgnoreTime = 0f;
			}
			return (shouldCorrect: true, immediateMove: false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float ComputeTicksToExtrapolate()
		{
			NetworkRunner runner = base.Runner;
			float num = (int)runner.Tick - (int)base.Object.LastReceiveTick;
			if (num < 0f)
			{
				InternalLogStreams.LogTraceForecast?.Log(this, $"ticksToExtrapolate: {num} < 0, runner.Tick: {runner.Tick}, LastReceiveTick: {base.Object.LastReceiveTick}, Runner.LatestServerTick: {runner.LatestServerTick} (no longer used)");
			}
			num += runner.LocalAlpha;
			float max = PhysicsSettings.MaxExtrapolationTime / runner.DeltaTime;
			return Mathf.Clamp(num, 0f, max);
		}

		private void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			_timeSinceLastImpactfulCollision += fixedDeltaTime;
			if (!base.Object || !base.Runner || base.Object.HasStateAuthority || !HasPhysicsBody || !HasForecastEnabled || PhysicsState.Flags.Has(NetworkRigidbodyFlags.IsKinematic))
			{
				return;
			}
			if (_firstExecution)
			{
				_firstExecution = false;
			}
			else
			{
				if (RemotePhysicsData.Flags.Has(NetworkRigidbodyFlags.IsKinematic))
				{
					return;
				}
				CopyToEngine(allowUpdateTransform: false);
				_physicsBody.WasColliding = _isColliding;
				_isColliding = false;
				_previousExtrapolatedPosition = _extrapolatedPosition;
				CopyToRemoteSnapshot(out var remoteSnapshot);
				_remotePosition = remoteSnapshot.WorldPosition;
				_remoteRotation = remoteSnapshot.WorldRotation;
				CopyToLocalSnapshot(out var localSnapshot);
				_position = localSnapshot.WorldPosition;
				KinematicSnapshot result = default(KinematicSnapshot);
				float ticksToExtrapolate = ComputeTicksToExtrapolate();
				if (CustomForecast != null)
				{
					CustomForecast(this, fixedDeltaTime, _physicsBody, ticksToExtrapolate, ref localSnapshot, ref remoteSnapshot, out result);
				}
				else
				{
					NetworkTransformHelpers.ComputeExtrapolatedSnapshot(this, fixedDeltaTime, _physicsBody, ticksToExtrapolate, ref localSnapshot, ref remoteSnapshot, out result);
				}
				_extrapolatedPosition = result.WorldPosition;
				_extrapolatedRotation = result.WorldRotation;
				if (_sleepNextUpdate)
				{
					_sleepNextUpdate = false;
					_physicsBody.Sleeping = true;
					return;
				}
				var (flag, flag2) = DetectError(fixedDeltaTime, ref localSnapshot, ref result);
				if (!flag)
				{
					return;
				}
				int teleportKey = RemoteTRSPData.TeleportKey;
				bool flag3 = _previousTeleportKey != teleportKey;
				if (flag2 || flag3)
				{
					ref NetworkTRSPData remoteTRSPData = ref RemoteTRSPData;
					ref NetworkPhysicsData remotePhysicsData = ref RemotePhysicsData;
					_physicsBody.Position = remoteTRSPData.Position;
					_physicsBody.Rotation = remoteTRSPData.Rotation;
					_physicsBody.LinearVelocity = remotePhysicsData.LinearVelocity;
					_physicsBody.AngularVelocity = remotePhysicsData.AngularVelocity;
					_previousTeleportKey = teleportKey;
				}
				else
				{
					PositionError = result.WorldPosition - localSnapshot.WorldPosition;
					if (CustomApplyCorrection != null)
					{
						CustomApplyCorrection(this, fixedDeltaTime, _physicsFps, _physicsBody, _timeSinceLastImpactfulCollision, ref localSnapshot, ref remoteSnapshot, ref result);
					}
					else
					{
						NetworkTransformHelpers.ApplyCorrection(this, fixedDeltaTime, _physicsFps, _physicsBody, _timeSinceLastImpactfulCollision, ref localSnapshot, ref remoteSnapshot, ref result);
					}
					PreviousPositionError = PositionError;
					_physicsBodyLinearVelocity = _physicsBody.LinearVelocity;
				}
				_previousPosition = localSnapshot.WorldPosition;
			}
		}

		private void OnCollisionEnter(Collision other)
		{
			if (base.HasStateAuthority)
			{
				return;
			}
			Vector3 collisionNormal;
			if (other.contactCount == 1)
			{
				collisionNormal = other.GetContact(0).normal;
			}
			else
			{
				Vector3 zero = Vector3.zero;
				for (int i = 0; i < other.contactCount; i++)
				{
					zero += other.GetContact(i).normal;
				}
				collisionNormal = zero.normalized;
			}
			CheckIfImpactfulAndSwitchToLocalPhysics(other.relativeVelocity, collisionNormal);
		}

		private void OnCollisionStay(Collision other)
		{
			_isColliding = true;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (base.HasStateAuthority)
			{
				return;
			}
			Vector2 vector;
			if (other.contactCount == 1)
			{
				vector = other.GetContact(0).normal;
			}
			else
			{
				Vector2 zero = Vector2.zero;
				for (int i = 0; i < other.contactCount; i++)
				{
					zero += other.GetContact(i).normal;
				}
				vector = zero.normalized;
			}
			CheckIfImpactfulAndSwitchToLocalPhysics(other.relativeVelocity, vector);
		}

		private void OnCollisionStay2D(Collision2D other)
		{
			_isColliding = true;
		}

		private void CheckIfImpactfulAndSwitchToLocalPhysics(Vector3 relativeVelocity, Vector3 collisionNormal)
		{
			float num = Math.Abs(Vector3.Dot(relativeVelocity.normalized, collisionNormal));
			RecordCollisionEnter(relativeVelocity.magnitude, num);
			if (num >= PhysicsSettings.MinImpactfulCollisionAlignment)
			{
				_timeSinceLastImpactfulCollision = 0f;
			}
		}

		private void CopyToRemoteSnapshot(out KinematicSnapshot remoteSnapshot)
		{
			ref NetworkTRSPData remoteTRSPData = ref RemoteTRSPData;
			ref NetworkPhysicsData remotePhysicsData = ref RemotePhysicsData;
			remoteSnapshot.WorldPosition = remoteTRSPData.Position;
			remoteSnapshot.WorldRotation = remoteTRSPData.Rotation;
			remoteSnapshot.LinearVelocity = remotePhysicsData.LinearVelocity;
			remoteSnapshot.AngularVelocity = remotePhysicsData.AngularVelocity;
			remoteSnapshot.IsSleeping = remotePhysicsData.Flags.Has(NetworkRigidbodyFlags.IsSleeping);
		}

		private void CopyToLocalSnapshot(out KinematicSnapshot localSnapshot)
		{
			localSnapshot.WorldPosition = _physicsBody.Position;
			localSnapshot.WorldRotation = _physicsBody.Rotation;
			localSnapshot.LinearVelocity = _physicsBody.LinearVelocity;
			localSnapshot.AngularVelocity = _physicsBody.AngularVelocity;
			localSnapshot.IsSleeping = _physicsBody.Sleeping;
		}

		void IAfterClientPredictionReset.AfterClientPredictionReset()
		{
			if (HasPhysicsBody)
			{
				_remoteTRSPData = base.Data;
				_remotePhysicsData = PhysicsData;
				_hasRemoteData = true;
			}
		}

		private void PhysicsDrawGizmos()
		{
			if (PhysicsSettings.DebugPhysics && (bool)base.Object && HasPhysicsBody && HasForecastEnabled && !base.Object.HasStateAuthority && !PhysicsState.Flags.Has(NetworkRigidbodyFlags.IsKinematic))
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(_position, _physicsBodyLinearVelocity);
				Gizmos.color = Color.green;
				Gizmos.matrix = (Gizmos.matrix = Matrix4x4.TRS(_remotePosition, _remoteRotation, new Vector3(1.01f, 1.01f, 1.01f)));
				Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 1f, 1f));
				Gizmos.color = Color.red;
				Gizmos.matrix = (Gizmos.matrix = Matrix4x4.TRS(_extrapolatedPosition, _extrapolatedRotation, new Vector3(1.01f, 1.01f, 1.01f)));
				Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 1f, 1f));
			}
		}

		public bool IsTraceEnabled()
		{
			return CurrentTrace != null;
		}

		[Conditional("DEBUG")]
		public void SetTraceEnabled(bool enabled)
		{
			if (enabled && CurrentTrace == null)
			{
				CurrentTrace = new NetworkTransformTrace();
			}
			else if (!enabled && CurrentTrace != null)
			{
				CurrentTrace = null;
			}
		}

		[Conditional("DEBUG")]
		internal void RecordForecastData(Vector3 previousVelocity, Vector3 newVelocity, Vector3 desiredVelocity, float lerpAlpha)
		{
			CurrentTrace?.ForecastBuffer.Write(new NetworkTransformTrace.ForecastData
			{
				PreviousVelocity = previousVelocity,
				NewVelocity = newVelocity,
				DesiredVelocity = desiredVelocity,
				LerpAlpha = lerpAlpha
			});
		}

		[Conditional("DEBUG")]
		internal void RecordStallHeuristicData(float errorSimilarity, float correctionProgress)
		{
			CurrentTrace?.StallHeuristicBuffer.Write(new NetworkTransformTrace.StallHeuristicData
			{
				ErrorSimilarity = errorSimilarity,
				CorrectionProgress = correctionProgress
			});
		}

		[Conditional("DEBUG")]
		internal void RecordStallScoreAndProgress(float stallScore, float stallProgress)
		{
			CurrentTrace?.StallBuffer.Write(new NetworkTransformTrace.StallData
			{
				StallProgress = stallProgress,
				StallScore = stallScore
			});
		}

		[Conditional("DEBUG")]
		internal void RecordCollisionEnter(float relativeVelocity, float impactAlignment)
		{
			CurrentTrace?.CollisionEnterBuffer.Write(new NetworkTransformTrace.CollisionEnterData
			{
				RelativeVelocity = relativeVelocity,
				ImpactAlignment = impactAlignment
			});
		}
	}
}
