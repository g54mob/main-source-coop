using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using Mirror;
using Mirror.RemoteCalls;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.Effects;
using NWH.VehiclePhysics2.Powertrain;
using NWH.VehiclePhysics2.Sound;
using NWH.VehiclePhysics2.Sound.SoundComponents;
using NWH.WheelController3D;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Player;
using NomadDrive.Features.Vehicle.WheelSystem;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.Vehicle.Networking
{
	[DefaultExecutionOrder(110)]
	[RequireComponent(typeof(NetworkIdentity))]
	[RequireComponent(typeof(VehicleController))]
	public class NetworkedNWHVehicle : NetworkBehaviour, IConnectionOwnedCleanup, IFloatingOriginShiftable
	{
		private sealed class RemoteWheelVisual
		{
			public readonly WheelLocation Location;

			public Transform Rotating;

			public Transform NonRotating;

			public float AxleAngle;

			public float SmoothedSteer;

			public float SmoothedSusp;

			public RemoteWheelVisual(WheelLocation location)
			{
				Location = location;
			}
		}

		private VehicleController _vehicleController;

		private Rigidbody _rigidbody;

		private bool _vehicleInitialized;

		[Inject]
		private IPlayerService _playerService;

		private VehicleManager _vehicleManager;

		private bool _localRiderDrivesTransform;

		private static readonly HashSet<NetworkedNWHVehicle> AllVehicles;

		[Header("Snapshot Interpolation")]
		[SerializeField]
		private float bufferTimeMultiplier = 2f;

		[SerializeField]
		private float catchupSpeed = 0.02f;

		[SerializeField]
		private float slowdownSpeed = 0.04f;

		[SerializeField]
		private float catchupNegativeThreshold = -1f;

		[SerializeField]
		private float catchupPositiveThreshold = 1f;

		[Header("At-Rest Detection")]
		[SerializeField]
		private float restVelocityThreshold = 0.05f;

		[SerializeField]
		private float restAngularThreshold = 0.05f;

		[Header("Teleport")]
		[SerializeField]
		private float teleportDistance = 15f;

		[Header("Sync")]
		[SerializeField]
		private float snapshotSendInterval = 0.05f;

		[Header("Owner Smoothness (optional)")]
		[SerializeField]
		private bool useOwnerInterpolation;

		[Header("Remote Physics")]
		[SerializeField]
		private bool disableRemoteWheelSimulation = true;

		[SerializeField]
		private float wheelSteerSmoothing = 18f;

		[SerializeField]
		private float wheelSuspensionSmoothing = 25f;

		[SerializeField]
		private float wheelSteerVisualMultiplier = 1f;

		[SerializeField]
		[FormerlySerializedAs("sleepRemoteWheelSimulation")]
		private bool sleepParkedWheelSimulation = true;

		private const int SnapshotBufferLimit = 30;

		private readonly SortedList<double, VehicleSnapshot> _snapshotBuffer = new SortedList<double, VehicleSnapshot>();

		private readonly List<double> _snapshotKeyScratch = new List<double>();

		private double _localTimeline;

		private double _localTimescale = 1.0;

		private ExponentialMovingAverage _driftEma;

		private ExponentialMovingAverage _deliveryTimeEma;

		private bool _ownerIsAtRest;

		private bool _sentFinalRestSnapshot;

		private CollisionDetectionMode _drivingCcd = CollisionDetectionMode.Continuous;

		private CollisionDetectionMode _currentCcd = CollisionDetectionMode.Continuous;

		private bool _remoteIsAtRest;

		private Vector3 _restPosition;

		private Quaternion _restRotation = Quaternion.identity;

		private bool _hasReceivedFirstSnapshot;

		private float _syncTimer;

		[SyncVar(hook = "OnDriverDisplayHookChanged")]
		private DriverDisplayInputs _driverDisplay;

		private DriverDisplayInputs _lastSentDisplay;

		private float _displaySyncTimer;

		private float _displayHeartbeatTimer;

		private const float DisplaySyncInterval = 1f / 15f;

		private const float DisplayHeartbeatInterval = 0.5f;

		private const float SteeringEpsilonDeg = 0.5f;

		private const float PedalEpsilon = 0.02f;

		[SyncVar(hook = "OnWheelVisualStateHookChanged")]
		private WheelVisualState _wheelVisualState;

		private WheelsManager _wheelsManager;

		private WheelVisualState _lastSentWheelVisual;

		private WheelVisualState _appliedWheelVisual;

		private float _wheelSyncTimer;

		private float _wheelHeartbeatTimer;

		private readonly RemoteWheelVisual[] _remoteWheels = new RemoteWheelVisual[4]
		{
			new RemoteWheelVisual(WheelLocation.FrontLeft),
			new RemoteWheelVisual(WheelLocation.FrontRight),
			new RemoteWheelVisual(WheelLocation.RearLeft),
			new RemoteWheelVisual(WheelLocation.RearRight)
		};

		private bool _wheelTransformsCached;

		private bool _hasReceivedWheelSync;

		private bool _snapWheelVisual;

		private const float WheelSyncInterval = 1f / 15f;

		private const float WheelHeartbeatInterval = 0.5f;

		private const float WheelAngVelEpsilon = 0.05f;

		private const float WheelSteerEpsilonDeg = 0.3f;

		private const float WheelSuspEpsilon = 0.003f;

		[SyncVar(hook = "OnEngineAudioHookChanged")]
		private EngineAudioState _engineAudio;

		private EngineAudioState _lastSentEngineAudio;

		private float _engineAudioSyncTimer;

		private float _engineAudioHeartbeatTimer;

		private bool _remoteEngineApplied;

		private bool _remoteEngineRunning;

		private float _remoteEngineAngVel;

		private const float EngineAudioSyncInterval = 1f / 15f;

		private const float EngineAudioHeartbeatInterval = 0.5f;

		private const float EngineAngVelEpsilon = 5f;

		[Header("Debug")]
		[SerializeField]
		private bool _isRemote;

		[SerializeField]
		[SyncVar(hook = "OnHasDriverChanged")]
		private bool _hasDriver;

		[SerializeField]
		private bool _parkedFrozen;

		[SerializeField]
		private bool _logWheelSync;

		private float _wheelSyncLogTimer;

		[SerializeField]
		private bool logRestoreCollisions;

		private float _restoreCollisionLogTimer;

		[SerializeField]
		private bool logPhantomThrottle;

		private float _phantomThrottleLogTimer;

		[SerializeField]
		private bool logRestoreDriveDiag;

		private bool _wasRestoredForDiag;

		private bool _diagPrimed;

		private float _driveDiagTimer;

		private Vector3 _lastDiagPos;

		private Quaternion _lastDiagRot;

		private float _maxStepPosDelta;

		private float _maxStepAngDelta;

		private bool _visualFxOriginalsCached;

		private float _origSkidmarkIntensity;

		private float _origParticleEmissionCoeff;

		private const float DriverlessLinearDecel = 20f;

		private const float DriverlessAngularDecel = 8f;

		public Action<DriverDisplayInputs, DriverDisplayInputs> _Mirror_SyncVarHookDelegate__driverDisplay;

		public Action<WheelVisualState, WheelVisualState> _Mirror_SyncVarHookDelegate__wheelVisualState;

		public Action<EngineAudioState, EngineAudioState> _Mirror_SyncVarHookDelegate__engineAudio;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__hasDriver;

		public float RemoteSteeringAngleDeg { get; private set; }

		public float RemoteThrottle => _driverDisplay.Throttle;

		public float RemoteBrakes => _driverDisplay.Brakes;

		private float BufferTime => snapshotSendInterval * bufferTimeMultiplier;

		public bool HasDriver => _hasDriver;

		public bool IsControlling
		{
			get
			{
				if (base.isOwned)
				{
					return !_isRemote;
				}
				return false;
			}
		}

		public bool IsParkedFrozen => _parkedFrozen;

		public DriverDisplayInputs Network_driverDisplay
		{
			get
			{
				return _driverDisplay;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _driverDisplay, 1uL, _Mirror_SyncVarHookDelegate__driverDisplay);
			}
		}

		public WheelVisualState Network_wheelVisualState
		{
			get
			{
				return _wheelVisualState;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _wheelVisualState, 2uL, _Mirror_SyncVarHookDelegate__wheelVisualState);
			}
		}

		public EngineAudioState Network_engineAudio
		{
			get
			{
				return _engineAudio;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _engineAudio, 4uL, _Mirror_SyncVarHookDelegate__engineAudio);
			}
		}

		public bool Network_hasDriver
		{
			get
			{
				return _hasDriver;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _hasDriver, 8uL, _Mirror_SyncVarHookDelegate__hasDriver);
			}
		}

		private void Awake()
		{
			_vehicleController = GetComponent<VehicleController>();
			_rigidbody = GetComponent<Rigidbody>();
			if (_rigidbody != null)
			{
				_rigidbody.solverIterations = 12;
				_rigidbody.solverVelocityIterations = 6;
				_drivingCcd = _rigidbody.collisionDetectionMode;
				_currentCcd = _rigidbody.collisionDetectionMode;
			}
			_wheelsManager = GetComponent<WheelsManager>();
			_vehicleManager = GetComponentInParent<VehicleManager>();
			_driftEma = new ExponentialMovingAverage(10);
			_deliveryTimeEma = new ExponentialMovingAverage(10);
			_vehicleController.onVehicleInitialized.AddListener(OnVehicleInitialized);
			FloatingOriginManager.RegisterShiftable(this);
		}

		private void OnDestroy()
		{
			AllVehicles.Remove(this);
			FloatingOriginManager.UnregisterShiftable(this);
			if (_vehicleController != null)
			{
				_vehicleController.onVehicleInitialized.RemoveListener(OnVehicleInitialized);
				_vehicleController.onMultiplayerStatusChanged.RemoveListener(OnMultiplayerStatusChanged);
			}
		}

		private void OnVehicleInitialized()
		{
			_vehicleInitialized = true;
			_vehicleController.onMultiplayerStatusChanged.AddListener(OnMultiplayerStatusChanged);
			UpdateRemoteStatus();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			AllVehicles.Add(this);
			if (_vehicleInitialized)
			{
				UpdateRemoteStatus();
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			AllVehicles.Remove(this);
		}

		public static NetworkedNWHVehicle FindNearest(Vector3 origin, float radius, bool requireNoDriver)
		{
			NetworkedNWHVehicle result = null;
			float num = radius * radius;
			foreach (NetworkedNWHVehicle allVehicle in AllVehicles)
			{
				if (!(allVehicle == null) && (!requireNoDriver || !allVehicle.HasDriver))
				{
					float sqrMagnitude = (origin - allVehicle.transform.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = allVehicle;
					}
				}
			}
			return result;
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (NetworkServer.localConnection != null)
			{
				base.netIdentity.AssignClientAuthority(NetworkServer.localConnection);
			}
			if (_vehicleInitialized)
			{
				UpdateRemoteStatus();
			}
			VehicleFreezeManager.EnsureExists();
			VehicleFreezeManager.Instance?.ServerRegister(this);
			VehicleFreezeManager.Instance?.ServerEvaluateNow(this);
		}

		public override void OnStopServer()
		{
			base.OnStopServer();
			VehicleFreezeManager.Instance?.ServerUnregister(base.netId);
		}

		public override void OnStartAuthority()
		{
			base.OnStartAuthority();
			_snapshotBuffer.Clear();
			_ownerIsAtRest = false;
			_sentFinalRestSnapshot = false;
			_syncTimer = 0f;
			_displaySyncTimer = 0f;
			_displayHeartbeatTimer = 0f;
			_wheelSyncTimer = 0f;
			_wheelHeartbeatTimer = 0f;
			_lastSentDisplay = default(DriverDisplayInputs);
			_lastSentWheelVisual = default(WheelVisualState);
			ResetRemoteWheelVisualState();
			ResetRemoteEngineAudioState();
			RemoteSteeringAngleDeg = 0f;
			UpdateRemoteStatus();
		}

		public override void OnStopAuthority()
		{
			base.OnStopAuthority();
			_hasReceivedFirstSnapshot = false;
			_snapshotBuffer.Clear();
			_remoteIsAtRest = false;
			_localTimeline = 0.0;
			_localTimescale = 1.0;
			_driftEma = new ExponentialMovingAverage(10);
			_deliveryTimeEma = new ExponentialMovingAverage(10);
			ResetRemoteWheelVisualState();
			ResetRemoteEngineAudioState();
			UpdateRemoteStatus();
		}

		private void UpdateRemoteStatus()
		{
			if (_vehicleInitialized)
			{
				bool flag = (_isRemote = !base.isOwned);
				_vehicleController.MultiplayerIsRemote = flag;
				bool isKinematic = flag || _parkedFrozen;
				if (_rigidbody != null)
				{
					_rigidbody.isKinematic = isKinematic;
					ApplyRemoteInterpolationMode();
					ApplyCollisionDetectionMode();
				}
				ApplyWheelSimulationGate();
				ApplyRemoteSurfaceSoundGate(flag);
				ApplyRemoteVisualFxGate(flag);
				ApplyInputAuthority();
			}
		}

		private void ApplyCollisionDetectionMode()
		{
			if (!(_rigidbody == null))
			{
				CollisionDetectionMode collisionDetectionMode = ((!_rigidbody.isKinematic && !_ownerIsAtRest) ? _drivingCcd : CollisionDetectionMode.Discrete);
				if (_currentCcd != collisionDetectionMode)
				{
					_currentCcd = collisionDetectionMode;
					_rigidbody.collisionDetectionMode = collisionDetectionMode;
				}
			}
		}

		private void ApplyRemoteSurfaceSoundGate(bool shouldBeRemote)
		{
			SoundManager soundManager = _vehicleController.soundManager;
			if (soundManager != null)
			{
				SetSourceMuted(soundManager.wheelSkidComponent?.source, shouldBeRemote);
				SetSourceMuted(soundManager.wheelTireNoiseComponent?.source, shouldBeRemote);
				SetSourceMuted(soundManager.suspensionBumpComponent?.source, shouldBeRemote);
			}
		}

		private static void SetSourceMuted(AudioSource source, bool muted)
		{
			if (source != null)
			{
				source.mute = muted;
			}
		}

		private void ApplyRemoteVisualFxGate(bool shouldBeRemote)
		{
			EffectManager effectManager = ((_vehicleController != null) ? _vehicleController.effectsManager : null);
			if (effectManager != null)
			{
				if (!_visualFxOriginalsCached)
				{
					_origSkidmarkIntensity = effectManager.skidmarkManager.globalSkidmarkIntensity;
					_origParticleEmissionCoeff = effectManager.surfaceParticleManager.emissionRateCoeff;
					_visualFxOriginalsCached = true;
				}
				effectManager.skidmarkManager.globalSkidmarkIntensity = (shouldBeRemote ? 0f : _origSkidmarkIntensity);
				effectManager.surfaceParticleManager.emissionRateCoeff = (shouldBeRemote ? 0f : _origParticleEmissionCoeff);
			}
		}

		private void OnMultiplayerStatusChanged(bool isRemote)
		{
			ApplyInputAuthority();
		}

		private void ApplyInputAuthority()
		{
			if (_vehicleInitialized && !(_vehicleController == null))
			{
				bool flag = IsControlling && HasDriver;
				_vehicleController.input.autoSetInput = flag;
				if (!flag && base.isOwned)
				{
					NeutralizeLocalDriveInput();
				}
			}
		}

		private void NeutralizeLocalDriveInput()
		{
			if (!(_vehicleController == null))
			{
				_vehicleController.input.states.throttle = 0f;
				_vehicleController.input.states.brakes = 0f;
			}
		}

		private void ServerHoldDriverlessVehicle()
		{
			if (!(_vehicleController == null))
			{
				_vehicleController.input.autoSetInput = false;
				_vehicleController.input.states.throttle = 0f;
				_vehicleController.input.states.brakes = 0f;
				if (!(_rigidbody == null) && !_rigidbody.isKinematic)
				{
					Vector3 linearVelocity = _rigidbody.linearVelocity;
					_rigidbody.linearVelocity = ((linearVelocity.magnitude <= restVelocityThreshold) ? Vector3.zero : Vector3.MoveTowards(linearVelocity, Vector3.zero, 20f * Time.fixedDeltaTime));
					Vector3 angularVelocity = _rigidbody.angularVelocity;
					_rigidbody.angularVelocity = ((angularVelocity.magnitude <= restAngularThreshold) ? Vector3.zero : Vector3.MoveTowards(angularVelocity, Vector3.zero, 8f * Time.fixedDeltaTime));
				}
			}
		}

		private void OnHasDriverChanged(bool oldValue, bool newValue)
		{
			ApplyInputAuthority();
		}

		private void FixedUpdate()
		{
			if (!_vehicleInitialized)
			{
				return;
			}
			if (!base.isOwned)
			{
				ReassertRemoteEngineRpm();
				return;
			}
			if (logRestoreDriveDiag)
			{
				LogDriveDiagIfNeeded();
			}
			if (base.isServer && !_hasDriver && !_parkedFrozen)
			{
				ServerHoldDriverlessVehicle();
			}
			SendDriverDisplayIfChanged();
			SendWheelVisualStateIfChanged();
			SendEngineAudioIfChanged();
			_syncTimer += Time.fixedDeltaTime;
			if (!(_syncTimer < snapshotSendInterval))
			{
				_syncTimer = 0f;
				float sqrMagnitude = _rigidbody.linearVelocity.sqrMagnitude;
				float sqrMagnitude2 = _rigidbody.angularVelocity.sqrMagnitude;
				if (sqrMagnitude > restVelocityThreshold * restVelocityThreshold || sqrMagnitude2 > restAngularThreshold * restAngularThreshold)
				{
					_ownerIsAtRest = false;
					_sentFinalRestSnapshot = false;
					ApplyCollisionDetectionMode();
					CmdSendSnapshot(NetworkTime.time, base.transform.position, base.transform.rotation, _rigidbody.linearVelocity, _rigidbody.angularVelocity, atRest: false, CurrentOriginShift());
					CmdSyncMultiplayerState(_vehicleController.GetMultiplayerState());
				}
				else if (!_sentFinalRestSnapshot)
				{
					CmdSendSnapshot(NetworkTime.time, base.transform.position, base.transform.rotation, Vector3.zero, Vector3.zero, atRest: true, CurrentOriginShift());
					CmdSyncMultiplayerState(_vehicleController.GetMultiplayerState());
					_sentFinalRestSnapshot = true;
					_ownerIsAtRest = true;
					ApplyCollisionDetectionMode();
				}
			}
		}

		private void ApplyRemoteInterpolationMode()
		{
			if (!(_rigidbody == null))
			{
				if (!_rigidbody.isKinematic)
				{
					_rigidbody.interpolation = (useOwnerInterpolation ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None);
				}
				else
				{
					_rigidbody.interpolation = ((!_localRiderDrivesTransform) ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None);
				}
			}
		}

		private void UpdateLocalRiderTransformMode()
		{
			bool flag = IsLocalPlayerRidingThisVehicle();
			if (flag != _localRiderDrivesTransform)
			{
				_localRiderDrivesTransform = flag;
				ApplyRemoteInterpolationMode();
			}
		}

		private bool IsLocalPlayerRidingThisVehicle()
		{
			if (_playerService == null || _vehicleManager == null)
			{
				return false;
			}
			NomadDrive.Features.Player.Player localPlayer = _playerService.LocalPlayer;
			if (localPlayer == null)
			{
				return false;
			}
			if (!localPlayer.IsMountedOnVehicle(_vehicleManager))
			{
				return localPlayer.IsSurfaceRidingVehicle(base.netId);
			}
			return true;
		}

		private void ApplyRemotePose(Vector3 position, Quaternion rotation, bool teleport)
		{
			if (_localRiderDrivesTransform)
			{
				_rigidbody.position = position;
				_rigidbody.rotation = rotation;
				base.transform.SetPositionAndRotation(position, rotation);
			}
			else if (teleport)
			{
				_rigidbody.position = position;
				_rigidbody.rotation = rotation;
			}
			else
			{
				_rigidbody.MovePosition(position);
				_rigidbody.MoveRotation(rotation);
			}
		}

		private void Update()
		{
			EnsureNotParentedUnderContentRoot();
			if (!_vehicleInitialized)
			{
				return;
			}
			if (!base.isOwned)
			{
				ReapplyRemoteDriverInputs();
				ApplyRemoteSurfaceSoundGate(shouldBeRemote: true);
				ReapplyRemoteEngineAudio();
			}
			if (base.isOwned)
			{
				LogPhantomThrottleIfNeeded();
				if (!useOwnerInterpolation)
				{
					base.transform.SetPositionAndRotation(_rigidbody.position, _rigidbody.rotation);
				}
				return;
			}
			UpdateLocalRiderTransformMode();
			if (!_hasReceivedFirstSnapshot)
			{
				return;
			}
			if (_remoteIsAtRest)
			{
				float num = Vector3.Distance(_rigidbody.position, _restPosition);
				if (num > 0.001f)
				{
					Vector3 position = Vector3.MoveTowards(_rigidbody.position, _restPosition, Mathf.Max(num * 20f, 0.5f) * Time.deltaTime);
					Quaternion rotation = Quaternion.RotateTowards(_rigidbody.rotation, _restRotation, 1000f * Time.deltaTime);
					ApplyRemotePose(position, rotation, teleport: false);
				}
				else
				{
					ApplyRemotePose(_restPosition, _restRotation, teleport: false);
				}
			}
			else if (_snapshotBuffer.Count != 0)
			{
				SnapshotInterpolation.Step(_snapshotBuffer, Time.deltaTime, ref _localTimeline, _localTimescale, out var fromSnapshot, out var toSnapshot, out var t);
				VehicleSnapshot vehicleSnapshot = VehicleSnapshot.Interpolate(fromSnapshot, toSnapshot, t);
				bool teleport = Vector3.Distance(_rigidbody.position, vehicleSnapshot.position) > teleportDistance;
				ApplyRemotePose(vehicleSnapshot.position, vehicleSnapshot.rotation, teleport);
			}
		}

		[Command]
		private void CmdSendSnapshot(double timestamp, Vector3 pos, Quaternion rot, Vector3 vel, Vector3 angVel, bool atRest, Vector3 senderShift)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteDouble(timestamp);
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot);
			writer.WriteVector3(vel);
			writer.WriteVector3(angVel);
			writer.WriteBool(atRest);
			writer.WriteVector3(senderShift);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSendSnapshot(System.Double,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3,UnityEngine.Vector3,System.Boolean,UnityEngine.Vector3)", 568761801, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcReceiveSnapshot(double timestamp, Vector3 pos, Quaternion rot, Vector3 vel, Vector3 angVel, bool atRest, Vector3 senderShift)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteDouble(timestamp);
			writer.WriteVector3(pos);
			writer.WriteQuaternion(rot);
			writer.WriteVector3(vel);
			writer.WriteVector3(angVel);
			writer.WriteBool(atRest);
			writer.WriteVector3(senderShift);
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcReceiveSnapshot(System.Double,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3,UnityEngine.Vector3,System.Boolean,UnityEngine.Vector3)", -625777889, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnSnapshotReceived(double timestamp, Vector3 pos, Quaternion rot, Vector3 vel, Vector3 angVel, bool atRest, Vector3 senderShift)
		{
			if (base.isOwned)
			{
				return;
			}
			pos += CurrentOriginShift() - senderShift;
			if (atRest)
			{
				_remoteIsAtRest = true;
				_restPosition = pos;
				_restRotation = rot;
				if (!_hasReceivedFirstSnapshot)
				{
					_hasReceivedFirstSnapshot = true;
					ApplyRemotePose(pos, rot, teleport: true);
				}
			}
			else
			{
				_remoteIsAtRest = false;
				VehicleSnapshot snapshot = new VehicleSnapshot(timestamp, NetworkTime.localTime, pos, rot, vel, angVel);
				if (!_hasReceivedFirstSnapshot)
				{
					_hasReceivedFirstSnapshot = true;
					base.transform.SetPositionAndRotation(pos, rot);
				}
				SnapshotInterpolation.InsertAndAdjust(_snapshotBuffer, 30, snapshot, ref _localTimeline, ref _localTimescale, snapshotSendInterval, BufferTime, catchupSpeed, slowdownSpeed, ref _driftEma, catchupNegativeThreshold, catchupPositiveThreshold, ref _deliveryTimeEma);
			}
		}

		public override void OnSerialize(NetworkWriter writer, bool initialState)
		{
			base.OnSerialize(writer, initialState);
			if (initialState)
			{
				writer.WriteVector3(base.transform.position);
				writer.WriteQuaternion(base.transform.rotation);
				writer.WriteVector3(CurrentOriginShift());
				writer.WriteBool(_ownerIsAtRest);
			}
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			base.OnDeserialize(reader, initialState);
			if (!initialState)
			{
				return;
			}
			Vector3 vector = reader.ReadVector3();
			Quaternion quaternion = reader.ReadQuaternion();
			Vector3 vector2 = reader.ReadVector3();
			reader.ReadBool();
			if (!base.isOwned)
			{
				vector += CurrentOriginShift() - vector2;
				base.transform.SetPositionAndRotation(vector, quaternion);
				if (_rigidbody != null)
				{
					_rigidbody.position = vector;
					_rigidbody.rotation = quaternion;
				}
				_remoteIsAtRest = true;
				_restPosition = vector;
				_restRotation = quaternion;
				_hasReceivedFirstSnapshot = true;
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdSyncMultiplayerState(VehicleController.MultiplayerState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSyncMultiplayerState(NWH.VehiclePhysics2.VehicleController/MultiplayerState)", -1490085749, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcSyncMultiplayerState(VehicleController.MultiplayerState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(writer, state);
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcSyncMultiplayerState(NWH.VehiclePhysics2.VehicleController/MultiplayerState)", 793393344, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void SendDriverDisplayIfChanged()
		{
			_displaySyncTimer += Time.fixedDeltaTime;
			if (!(_displaySyncTimer < 1f / 15f))
			{
				_displaySyncTimer = 0f;
				_displayHeartbeatTimer += 1f / 15f;
				DriverDisplayInputs driverDisplayInputs = new DriverDisplayInputs
				{
					SteeringAngleDeg = _vehicleController.steering.angle,
					Throttle = _vehicleController.input.Throttle,
					Brakes = _vehicleController.input.Brakes,
					Horn = _vehicleController.input.Horn
				};
				bool num = Mathf.Abs(driverDisplayInputs.SteeringAngleDeg - _lastSentDisplay.SteeringAngleDeg) > 0.5f;
				bool flag = Mathf.Abs(driverDisplayInputs.Throttle - _lastSentDisplay.Throttle) > 0.02f;
				bool flag2 = Mathf.Abs(driverDisplayInputs.Brakes - _lastSentDisplay.Brakes) > 0.02f;
				bool flag3 = driverDisplayInputs.Horn != _lastSentDisplay.Horn;
				bool flag4 = _displayHeartbeatTimer >= 0.5f;
				if (num || flag || flag2 || flag3 || flag4)
				{
					_lastSentDisplay = driverDisplayInputs;
					_displayHeartbeatTimer = 0f;
					CmdSyncDriverDisplay(driverDisplayInputs);
				}
			}
		}

		[Command(channel = 1)]
		private void CmdSyncDriverDisplay(DriverDisplayInputs state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSyncDriverDisplay(NomadDrive.Features.Vehicle.Networking.DriverDisplayInputs)", -40655847, writer, 1);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(channel = 1, includeOwner = false)]
		private void RpcReceiveDriverDisplay(DriverDisplayInputs state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(writer, state);
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcReceiveDriverDisplay(NomadDrive.Features.Vehicle.Networking.DriverDisplayInputs)", 1831120960, writer, 1, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnDriverDisplayHookChanged(DriverDisplayInputs oldValue, DriverDisplayInputs newValue)
		{
			if (_vehicleInitialized)
			{
				ApplyDriverDisplay(newValue);
			}
		}

		private void ApplyDriverDisplay(DriverDisplayInputs state)
		{
			if (!base.isOwned)
			{
				RemoteSteeringAngleDeg = state.SteeringAngleDeg;
				Network_driverDisplay = state;
				ReapplyRemoteDriverInputs();
			}
		}

		private void ReapplyRemoteDriverInputs()
		{
			if (_vehicleInitialized && !base.isOwned)
			{
				_vehicleController.input.states.throttle = _driverDisplay.Throttle;
				_vehicleController.input.states.brakes = _driverDisplay.Brakes;
				_vehicleController.input.states.horn = _driverDisplay.Horn;
			}
		}

		private void SendWheelVisualStateIfChanged()
		{
			if (_wheelsManager == null)
			{
				return;
			}
			_wheelSyncTimer += Time.fixedDeltaTime;
			if (!(_wheelSyncTimer < 1f / 15f))
			{
				_wheelSyncTimer = 0f;
				_wheelHeartbeatTimer += 1f / 15f;
				WheelVisualState a = SampleWheelVisualState();
				bool num = WheelVisualChanged(in a, in _lastSentWheelVisual);
				bool flag = _wheelHeartbeatTimer >= 0.5f;
				if (num || flag)
				{
					_lastSentWheelVisual = a;
					_wheelHeartbeatTimer = 0f;
					CmdSyncWheelVisualState(a);
					_ = _logWheelSync;
				}
			}
		}

		private static bool WheelVisualChanged(in WheelVisualState a, in WheelVisualState b)
		{
			if (!(Mathf.Abs(a.AngVelFL - b.AngVelFL) > 0.05f) && !(Mathf.Abs(a.AngVelFR - b.AngVelFR) > 0.05f) && !(Mathf.Abs(a.AngVelRL - b.AngVelRL) > 0.05f) && !(Mathf.Abs(a.AngVelRR - b.AngVelRR) > 0.05f) && !(Mathf.Abs(a.SteerFL - b.SteerFL) > 0.3f) && !(Mathf.Abs(a.SteerFR - b.SteerFR) > 0.3f) && !(Mathf.Abs(a.SteerRL - b.SteerRL) > 0.3f) && !(Mathf.Abs(a.SteerRR - b.SteerRR) > 0.3f) && !(Mathf.Abs(a.SuspFL - b.SuspFL) > 0.003f) && !(Mathf.Abs(a.SuspFR - b.SuspFR) > 0.003f) && !(Mathf.Abs(a.SuspRL - b.SuspRL) > 0.003f))
			{
				return Mathf.Abs(a.SuspRR - b.SuspRR) > 0.003f;
			}
			return true;
		}

		private WheelVisualState SampleWheelVisualState()
		{
			GetWheelSample(WheelLocation.FrontLeft, out var angVel, out var steer, out var susp);
			GetWheelSample(WheelLocation.FrontRight, out var angVel2, out var steer2, out var susp2);
			GetWheelSample(WheelLocation.RearLeft, out var angVel3, out var steer3, out var susp3);
			GetWheelSample(WheelLocation.RearRight, out var angVel4, out var steer4, out var susp4);
			return new WheelVisualState
			{
				AngVelFL = angVel,
				AngVelFR = angVel2,
				AngVelRL = angVel3,
				AngVelRR = angVel4,
				SteerFL = steer,
				SteerFR = steer2,
				SteerRL = steer3,
				SteerRR = steer4,
				SuspFL = susp,
				SuspFR = susp2,
				SuspRL = susp3,
				SuspRR = susp4
			};
		}

		private void GetWheelSample(WheelLocation location, out float angVel, out float steer, out float susp)
		{
			angVel = 0f;
			steer = 0f;
			susp = 0f;
			if (!_wheelsManager.HasWheelLocation(location))
			{
				return;
			}
			WheelController wheelController = _wheelsManager.GetWheelController(location);
			if (!(wheelController == null) && wheelController.wheel != null)
			{
				angVel = wheelController.wheel.angularVelocity;
				steer = wheelController.SteerAngle;
				if (wheelController.wheel.rotatingContainer != null)
				{
					susp = wheelController.wheel.rotatingContainer.localPosition.y;
				}
			}
		}

		[Command(channel = 1)]
		private void CmdSyncWheelVisualState(WheelVisualState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSyncWheelVisualState(NomadDrive.Features.Vehicle.Networking.WheelVisualState)", 371653334, writer, 1);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(channel = 1, includeOwner = false)]
		private void RpcReceiveWheelVisualState(WheelVisualState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(writer, state);
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcReceiveWheelVisualState(NomadDrive.Features.Vehicle.Networking.WheelVisualState)", -1704253573, writer, 1, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnWheelVisualStateHookChanged(WheelVisualState oldValue, WheelVisualState newValue)
		{
			if (_vehicleInitialized)
			{
				ApplyWheelVisualState(newValue);
			}
		}

		private void ApplyWheelVisualState(WheelVisualState state)
		{
			if (!base.isOwned)
			{
				bool num = !_hasReceivedWheelSync;
				_appliedWheelVisual = state;
				_hasReceivedWheelSync = true;
				if (num)
				{
					_snapWheelVisual = true;
					ApplyWheelSimulationGate();
					_ = _logWheelSync;
				}
			}
		}

		private void LateUpdate()
		{
			if (!_vehicleInitialized)
			{
				return;
			}
			_vehicleManager?.DriveAttachedParts(base.transform);
			if (base.isOwned || !_hasReceivedWheelSync || _wheelsManager == null)
			{
				return;
			}
			if (!_wheelTransformsCached)
			{
				TryCacheWheelTransforms();
			}
			float deltaTime = Time.deltaTime;
			bool snapWheelVisual = _snapWheelVisual;
			_snapWheelVisual = false;
			float t = (snapWheelVisual ? 1f : (1f - Mathf.Exp((0f - wheelSteerSmoothing) * deltaTime)));
			float t2 = (snapWheelVisual ? 1f : (1f - Mathf.Exp((0f - wheelSuspensionSmoothing) * deltaTime)));
			for (int i = 0; i < _remoteWheels.Length; i++)
			{
				RemoteWheelVisual remoteWheelVisual = _remoteWheels[i];
				if (!(remoteWheelVisual.Rotating == null) && IsWheelAttached(i))
				{
					GetSyncedWheel(in _appliedWheelVisual, i, out var angVel, out var steer, out var susp);
					remoteWheelVisual.AxleAngle = Mathf.Repeat(remoteWheelVisual.AxleAngle + angVel * 57.29578f * deltaTime, 360f);
					remoteWheelVisual.SmoothedSteer = Mathf.Lerp(remoteWheelVisual.SmoothedSteer, steer * wheelSteerVisualMultiplier, t);
					remoteWheelVisual.SmoothedSusp = Mathf.Lerp(remoteWheelVisual.SmoothedSusp, susp, t2);
					Vector3 localPosition = remoteWheelVisual.Rotating.localPosition;
					remoteWheelVisual.Rotating.localPosition = new Vector3(localPosition.x, remoteWheelVisual.SmoothedSusp, localPosition.z);
					remoteWheelVisual.Rotating.localRotation = Quaternion.Euler(remoteWheelVisual.AxleAngle, remoteWheelVisual.SmoothedSteer, 0f);
					if (remoteWheelVisual.NonRotating != null)
					{
						Vector3 localPosition2 = remoteWheelVisual.NonRotating.localPosition;
						remoteWheelVisual.NonRotating.localPosition = new Vector3(localPosition2.x, remoteWheelVisual.SmoothedSusp, localPosition2.z);
					}
				}
			}
			if (_logWheelSync)
			{
				_wheelSyncLogTimer += deltaTime;
				if (_wheelSyncLogTimer >= 1f)
				{
					_wheelSyncLogTimer = 0f;
				}
			}
		}

		private static void GetSyncedWheel(in WheelVisualState s, int index, out float angVel, out float steer, out float susp)
		{
			switch (index)
			{
			case 0:
				angVel = s.AngVelFL;
				steer = s.SteerFL;
				susp = s.SuspFL;
				break;
			case 1:
				angVel = s.AngVelFR;
				steer = s.SteerFR;
				susp = s.SuspFR;
				break;
			case 2:
				angVel = s.AngVelRL;
				steer = s.SteerRL;
				susp = s.SuspRL;
				break;
			default:
				angVel = s.AngVelRR;
				steer = s.SteerRR;
				susp = s.SuspRR;
				break;
			}
		}

		private bool IsWheelAttached(int index)
		{
			return index switch
			{
				0 => _wheelsManager.IsFrontLeftWheelAttached, 
				1 => _wheelsManager.IsFrontRightWheelAttached, 
				2 => _wheelsManager.IsRearLeftWheelAttached, 
				_ => _wheelsManager.IsRearRightWheelAttached, 
			};
		}

		private void TryCacheWheelTransforms()
		{
			bool wheelTransformsCached = false;
			for (int i = 0; i < _remoteWheels.Length; i++)
			{
				RemoteWheelVisual remoteWheelVisual = _remoteWheels[i];
				remoteWheelVisual.Rotating = SafeGetRotating(remoteWheelVisual.Location);
				remoteWheelVisual.NonRotating = SafeGetNonRotating(remoteWheelVisual.Location);
				if (remoteWheelVisual.Rotating != null)
				{
					wheelTransformsCached = true;
				}
			}
			_wheelTransformsCached = wheelTransformsCached;
			_ = _logWheelSync;
		}

		private Transform SafeGetRotating(WheelLocation location)
		{
			if (!_wheelsManager.HasWheelLocation(location))
			{
				return null;
			}
			WheelSlot wheelSlot = _wheelsManager.GetWheelSlot(location);
			if (wheelSlot != null && wheelSlot.RotatingTransform != null)
			{
				return wheelSlot.RotatingTransform;
			}
			WheelController wheelController = _wheelsManager.GetWheelController(location);
			if (wheelController != null && wheelController.wheel != null && wheelController.wheel.rotatingContainer != null)
			{
				return wheelController.wheel.rotatingContainer;
			}
			return null;
		}

		private Transform SafeGetNonRotating(WheelLocation location)
		{
			if (!_wheelsManager.HasWheelLocation(location))
			{
				return null;
			}
			WheelController wheelController = _wheelsManager.GetWheelController(location);
			if (!(wheelController != null) || wheelController.wheel == null)
			{
				return null;
			}
			return wheelController.wheel.nonRotatingContainer;
		}

		private void ApplyWheelSimulationGate()
		{
			if (!(_wheelsManager == null))
			{
				bool flag = !base.isOwned;
				bool flag2 = (disableRemoteWheelSimulation && flag && _hasReceivedWheelSync) || (sleepParkedWheelSimulation && _parkedFrozen);
				_wheelsManager.SetWheelSimulation(!flag2);
			}
		}

		private void ResetRemoteWheelVisualState()
		{
			_hasReceivedWheelSync = false;
			_snapWheelVisual = true;
			RemoteWheelVisual[] remoteWheels = _remoteWheels;
			foreach (RemoteWheelVisual obj in remoteWheels)
			{
				obj.AxleAngle = 0f;
				obj.SmoothedSteer = 0f;
				obj.SmoothedSusp = 0f;
			}
		}

		private void SendEngineAudioIfChanged()
		{
			EngineComponent engineComponent = _vehicleController.powertrain?.engine;
			if (engineComponent == null)
			{
				return;
			}
			_engineAudioSyncTimer += Time.fixedDeltaTime;
			if (!(_engineAudioSyncTimer < 1f / 15f))
			{
				_engineAudioSyncTimer = 0f;
				_engineAudioHeartbeatTimer += 1f / 15f;
				EngineAudioState engineAudioState = new EngineAudioState
				{
					IsRunning = engineComponent.IsRunning,
					AngularVelocity = engineComponent.outputAngularVelocity
				};
				bool num = engineAudioState.IsRunning != _lastSentEngineAudio.IsRunning;
				bool flag = Mathf.Abs(engineAudioState.AngularVelocity - _lastSentEngineAudio.AngularVelocity) > 5f;
				bool flag2 = _engineAudioHeartbeatTimer >= 0.5f;
				if (num || flag || flag2)
				{
					_lastSentEngineAudio = engineAudioState;
					_engineAudioHeartbeatTimer = 0f;
					CmdSyncEngineAudio(engineAudioState);
				}
			}
		}

		[Command(channel = 1)]
		private void CmdSyncEngineAudio(EngineAudioState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSyncEngineAudio(NomadDrive.Features.Vehicle.Networking.EngineAudioState)", -395102805, writer, 1);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(channel = 1, includeOwner = false)]
		private void RpcReceiveEngineAudio(EngineAudioState state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(writer, state);
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcReceiveEngineAudio(NomadDrive.Features.Vehicle.Networking.EngineAudioState)", 1450933960, writer, 1, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnEngineAudioHookChanged(EngineAudioState oldValue, EngineAudioState newValue)
		{
			if (_vehicleInitialized)
			{
				ApplyEngineAudio(newValue);
			}
		}

		private void ApplyEngineAudio(EngineAudioState state)
		{
			if (base.isOwned)
			{
				return;
			}
			bool flag = !_remoteEngineApplied;
			bool flag2 = _remoteEngineApplied && _remoteEngineRunning;
			bool isRunning = state.IsRunning;
			if (!flag && !isRunning && flag2)
			{
				PlayEngineStopShutdown();
			}
			_remoteEngineApplied = true;
			_remoteEngineRunning = isRunning;
			_remoteEngineAngVel = state.AngularVelocity;
			ApplyRemoteEngineState();
			if (isRunning && !flag2)
			{
				if (!flag)
				{
					PlayEngineStartCrank();
				}
				PlayEngineLoops();
			}
			else if (!isRunning && flag2)
			{
				StopEngineLoops();
			}
		}

		private void ReassertRemoteEngineRpm()
		{
			if (_remoteEngineApplied && _remoteEngineRunning)
			{
				EngineComponent engineComponent = _vehicleController.powertrain?.engine;
				if (engineComponent != null)
				{
					engineComponent.ignition = true;
					engineComponent.outputAngularVelocity = _remoteEngineAngVel;
				}
			}
		}

		private void ReapplyRemoteEngineAudio()
		{
			if (_remoteEngineApplied)
			{
				ApplyRemoteEngineState();
				if (_remoteEngineRunning)
				{
					PlayEngineLoops();
				}
			}
		}

		private void ApplyRemoteEngineState()
		{
			EngineComponent engineComponent = _vehicleController.powertrain?.engine;
			if (engineComponent != null)
			{
				if (_remoteEngineRunning)
				{
					engineComponent.ignition = true;
					engineComponent.outputAngularVelocity = _remoteEngineAngVel;
				}
				else
				{
					engineComponent.ignition = false;
					_vehicleController.input.states.throttle = 0f;
				}
			}
		}

		private void PlayEngineLoops()
		{
			SoundManager soundManager = _vehicleController.soundManager;
			if (soundManager != null)
			{
				PlaySource(soundManager.engineRunningComponent);
				PlaySource(soundManager.engineFanComponent);
			}
		}

		private void StopEngineLoops()
		{
			SoundManager soundManager = _vehicleController.soundManager;
			if (soundManager != null)
			{
				StopSource(soundManager.engineRunningComponent);
				StopSource(soundManager.engineFanComponent);
			}
		}

		private void PlayEngineStartCrank()
		{
			EngineStartStopComponent engineStartStopComponent = _vehicleController.soundManager?.engineStartStopComponent;
			if (engineStartStopComponent != null && engineStartStopComponent.source != null)
			{
				engineStartStopComponent.PlayStarting();
			}
		}

		private void PlayEngineStopShutdown()
		{
			EngineStartStopComponent engineStartStopComponent = _vehicleController.soundManager?.engineStartStopComponent;
			if (engineStartStopComponent != null && engineStartStopComponent.source != null)
			{
				engineStartStopComponent.PlayStopping();
			}
		}

		private static void PlaySource(SoundComponent component)
		{
			if (component != null && component.source != null)
			{
				component.Play();
			}
		}

		private static void StopSource(SoundComponent component)
		{
			if (component != null && component.source != null)
			{
				component.Stop();
			}
		}

		private void ResetRemoteEngineAudioState()
		{
			_remoteEngineApplied = false;
			_remoteEngineRunning = false;
			_remoteEngineAngVel = 0f;
			_lastSentEngineAudio = default(EngineAudioState);
			_engineAudioSyncTimer = 0f;
			_engineAudioHeartbeatTimer = 0f;
		}

		[Server]
		public void OnDriverEntered(NetworkConnectionToClient driverConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::OnDriverEntered(Mirror.NetworkConnectionToClient)' called when server was not active");
				return;
			}
			if (driverConnection == null)
			{
				EvilLogger.LogError("[NetworkedNWHVehicle] Driver connection is null!", "OnDriverEntered", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Networking\\NetworkedNWHVehicle.cs", 1755);
				return;
			}
			if (_parkedFrozen)
			{
				ServerSetParkedFrozen(frozen: false);
			}
			BroadcastNeutralDriverState();
			if (base.netIdentity.connectionToClient != null)
			{
				base.netIdentity.RemoveClientAuthority();
			}
			base.netIdentity.AssignClientAuthority(driverConnection);
			Network_hasDriver = true;
		}

		[Server]
		public void OnDriverExited()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::OnDriverExited()' called when server was not active");
				return;
			}
			BroadcastNeutralDriverState();
			if (base.netIdentity.connectionToClient != null)
			{
				base.netIdentity.RemoveClientAuthority();
			}
			if (NetworkServer.localConnection != null)
			{
				base.netIdentity.AssignClientAuthority(NetworkServer.localConnection);
			}
			Network_hasDriver = false;
			ServerHoldDriverlessVehicle();
			if (_rigidbody != null)
			{
				_ = _rigidbody.linearVelocity.magnitude;
				_ = restVelocityThreshold;
			}
			VehicleFreezeManager.Instance?.ServerEvaluateNow(this);
		}

		public void OnOwnerDisconnecting(NetworkConnectionToClient conn)
		{
			if (base.netIdentity.connectionToClient == conn)
			{
				OnDriverExited();
			}
		}

		[Server]
		private void BroadcastNeutralDriverState()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::BroadcastNeutralDriverState()' called when server was not active");
			}
			else if (_vehicleInitialized)
			{
				Network_driverDisplay = default(DriverDisplayInputs);
				WheelVisualState wheelVisualState = _wheelVisualState;
				wheelVisualState.AngVelFL = (wheelVisualState.AngVelFR = (wheelVisualState.AngVelRL = (wheelVisualState.AngVelRR = 0f)));
				Network_wheelVisualState = wheelVisualState;
				RpcReceiveDriverDisplay(default(DriverDisplayInputs));
				RpcReceiveWheelVisualState(wheelVisualState);
			}
		}

		public void ServerSetParkedFrozen(bool frozen)
		{
			if (base.isServer && _parkedFrozen != frozen)
			{
				_parkedFrozen = frozen;
				if (frozen && _rigidbody != null && !_rigidbody.isKinematic)
				{
					_rigidbody.linearVelocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
				}
				UpdateRemoteStatus();
			}
		}

		public void ServerReassertPhysicsStateAfterRestore()
		{
			if (base.isServer)
			{
				_wheelsManager?.ResyncAfterSettle();
				NeutralizeLocalDriveInput();
			}
		}

		public void RequestRescueReposition(Vector3 safePos, Quaternion safeRot)
		{
			CmdRescueReposition(safePos, safeRot, CurrentOriginShift());
		}

		[Command(requiresAuthority = false)]
		private void CmdRescueReposition(Vector3 safePos, Quaternion safeRot, Vector3 senderShift)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(safePos);
			writer.WriteQuaternion(safeRot);
			writer.WriteVector3(senderShift);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdRescueReposition(UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3)", 1478542697, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcRefreshAttachedParts()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcRefreshAttachedParts()", -1731541667, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void OnCollisionEnter(UnityEngine.Collision collision)
		{
			LogRestoreCollision("Enter", collision);
		}

		private void OnCollisionStay(UnityEngine.Collision collision)
		{
			if (logRestoreCollisions)
			{
				_restoreCollisionLogTimer += Time.deltaTime;
				if (!(_restoreCollisionLogTimer < 0.5f))
				{
					_restoreCollisionLogTimer = 0f;
					LogRestoreCollision("Stay", collision);
				}
			}
		}

		private void LogRestoreCollision(string phase, UnityEngine.Collision collision)
		{
			if (logRestoreCollisions && collision != null && !(collision.collider == null))
			{
				_ = collision.collider;
				if (collision.contactCount > 0)
				{
					_ = collision.GetContact(0).separation;
				}
				if (!(_rigidbody != null))
				{
					_ = Vector3.zero;
				}
				else
				{
					_ = _rigidbody.linearVelocity;
				}
			}
		}

		private void LogPhantomThrottleIfNeeded()
		{
			if (!logPhantomThrottle || _vehicleController == null || HasDriver)
			{
				return;
			}
			float throttle = _vehicleController.input.Throttle;
			float num = ((_rigidbody != null) ? _rigidbody.linearVelocity.magnitude : 0f);
			if (!(throttle <= 0.05f) || !(num <= 0.5f))
			{
				_phantomThrottleLogTimer += Time.deltaTime;
				if (!(_phantomThrottleLogTimer < 0.5f))
				{
					_phantomThrottleLogTimer = 0f;
					_ = _vehicleController.input.autoSetInput;
				}
			}
		}

		public void MarkRestoredForDiag()
		{
			_wasRestoredForDiag = true;
		}

		private void LogDriveDiagIfNeeded()
		{
			if (_rigidbody == null)
			{
				return;
			}
			Vector3 position = _rigidbody.position;
			Quaternion rotation = _rigidbody.rotation;
			if (!_diagPrimed)
			{
				_lastDiagPos = position;
				_lastDiagRot = rotation;
				_diagPrimed = true;
			}
			float magnitude = (position - _lastDiagPos).magnitude;
			float num = Quaternion.Angle(rotation, _lastDiagRot);
			if (magnitude > _maxStepPosDelta)
			{
				_maxStepPosDelta = magnitude;
			}
			if (num > _maxStepAngDelta)
			{
				_maxStepAngDelta = num;
			}
			_lastDiagPos = position;
			_lastDiagRot = rotation;
			_driveDiagTimer += Time.fixedDeltaTime;
			if (_driveDiagTimer < 0.25f)
			{
				return;
			}
			_driveDiagTimer = 0f;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>(includeInactive: true);
			foreach (Collider collider in componentsInChildren)
			{
				if (!(collider == null))
				{
					num2++;
					if (collider.enabled && !collider.isTrigger)
					{
						num3++;
					}
					if (collider is MeshCollider && collider.gameObject.name == "Collider")
					{
						num4++;
					}
				}
			}
			int num5 = 0;
			int num6 = 0;
			Rigidbody[] componentsInChildren2 = GetComponentsInChildren<Rigidbody>(includeInactive: true);
			foreach (Rigidbody rigidbody in componentsInChildren2)
			{
				if (!(rigidbody == null) && !(rigidbody == _rigidbody))
				{
					num5++;
					if (!rigidbody.isKinematic)
					{
						num6++;
					}
				}
			}
			if (!(FloatingOriginManager.Instance != null))
			{
				_ = Vector3.zero;
			}
			else
			{
				_ = FloatingOriginManager.Instance.TotalShift;
			}
			_ = base.transform.position;
			_maxStepPosDelta = 0f;
			_maxStepAngDelta = 0f;
			_wheelsManager?.LogSuspensionDiag("drive");
		}

		private static Vector3 CurrentOriginShift()
		{
			if (!(FloatingOriginManager.Instance != null))
			{
				return Vector3.zero;
			}
			return FloatingOriginManager.Instance.TotalShift;
		}

		private void EnsureNotParentedUnderContentRoot()
		{
			if (!(base.transform.parent == null))
			{
				FloatingOriginManager instance = FloatingOriginManager.Instance;
				if (instance != null && instance.WorldContentRoot != null && base.transform.IsChildOf(instance.WorldContentRoot))
				{
					base.transform.SetParent(null, worldPositionStays: true);
				}
			}
		}

		public void OnOriginShift(Vector3 delta)
		{
			if (_rigidbody != null)
			{
				_rigidbody.position += delta;
			}
			base.transform.position += delta;
			_restPosition += delta;
			if (_snapshotBuffer.Count > 0)
			{
				_snapshotKeyScratch.Clear();
				_snapshotKeyScratch.AddRange(_snapshotBuffer.Keys);
				foreach (double item in _snapshotKeyScratch)
				{
					VehicleSnapshot value = _snapshotBuffer[item];
					value.position += delta;
					_snapshotBuffer[item] = value;
				}
			}
			GetComponent<VehicleManager>()?.RefreshAttachedPartsInteraction();
		}

		public NetworkedNWHVehicle()
		{
			_Mirror_SyncVarHookDelegate__driverDisplay = OnDriverDisplayHookChanged;
			_Mirror_SyncVarHookDelegate__wheelVisualState = OnWheelVisualStateHookChanged;
			_Mirror_SyncVarHookDelegate__engineAudio = OnEngineAudioHookChanged;
			_Mirror_SyncVarHookDelegate__hasDriver = OnHasDriverChanged;
		}

		static NetworkedNWHVehicle()
		{
			AllVehicles = new HashSet<NetworkedNWHVehicle>();
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSendSnapshot(System.Double,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3,UnityEngine.Vector3,System.Boolean,UnityEngine.Vector3)", InvokeUserCode_CmdSendSnapshot__Double__Vector3__Quaternion__Vector3__Vector3__Boolean__Vector3, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSyncMultiplayerState(NWH.VehiclePhysics2.VehicleController/MultiplayerState)", InvokeUserCode_CmdSyncMultiplayerState__MultiplayerState, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSyncDriverDisplay(NomadDrive.Features.Vehicle.Networking.DriverDisplayInputs)", InvokeUserCode_CmdSyncDriverDisplay__DriverDisplayInputs, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSyncWheelVisualState(NomadDrive.Features.Vehicle.Networking.WheelVisualState)", InvokeUserCode_CmdSyncWheelVisualState__WheelVisualState, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdSyncEngineAudio(NomadDrive.Features.Vehicle.Networking.EngineAudioState)", InvokeUserCode_CmdSyncEngineAudio__EngineAudioState, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::CmdRescueReposition(UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3)", InvokeUserCode_CmdRescueReposition__Vector3__Quaternion__Vector3, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcReceiveSnapshot(System.Double,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Vector3,UnityEngine.Vector3,System.Boolean,UnityEngine.Vector3)", InvokeUserCode_RpcReceiveSnapshot__Double__Vector3__Quaternion__Vector3__Vector3__Boolean__Vector3);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcSyncMultiplayerState(NWH.VehiclePhysics2.VehicleController/MultiplayerState)", InvokeUserCode_RpcSyncMultiplayerState__MultiplayerState);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcReceiveDriverDisplay(NomadDrive.Features.Vehicle.Networking.DriverDisplayInputs)", InvokeUserCode_RpcReceiveDriverDisplay__DriverDisplayInputs);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcReceiveWheelVisualState(NomadDrive.Features.Vehicle.Networking.WheelVisualState)", InvokeUserCode_RpcReceiveWheelVisualState__WheelVisualState);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcReceiveEngineAudio(NomadDrive.Features.Vehicle.Networking.EngineAudioState)", InvokeUserCode_RpcReceiveEngineAudio__EngineAudioState);
			RemoteProcedureCalls.RegisterRpc(typeof(NetworkedNWHVehicle), "System.Void NomadDrive.Features.Vehicle.Networking.NetworkedNWHVehicle::RpcRefreshAttachedParts()", InvokeUserCode_RpcRefreshAttachedParts);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSendSnapshot__Double__Vector3__Quaternion__Vector3__Vector3__Boolean__Vector3(double timestamp, Vector3 pos, Quaternion rot, Vector3 vel, Vector3 angVel, bool atRest, Vector3 senderShift)
		{
			OnSnapshotReceived(timestamp, pos, rot, vel, angVel, atRest, senderShift);
			RpcReceiveSnapshot(timestamp, pos, rot, vel, angVel, atRest, senderShift);
		}

		protected static void InvokeUserCode_CmdSendSnapshot__Double__Vector3__Quaternion__Vector3__Vector3__Boolean__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendSnapshot called on client.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_CmdSendSnapshot__Double__Vector3__Quaternion__Vector3__Vector3__Boolean__Vector3(reader.ReadDouble(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadBool(), reader.ReadVector3());
			}
		}

		protected void UserCode_RpcReceiveSnapshot__Double__Vector3__Quaternion__Vector3__Vector3__Boolean__Vector3(double timestamp, Vector3 pos, Quaternion rot, Vector3 vel, Vector3 angVel, bool atRest, Vector3 senderShift)
		{
			if (!base.isServer)
			{
				OnSnapshotReceived(timestamp, pos, rot, vel, angVel, atRest, senderShift);
			}
		}

		protected static void InvokeUserCode_RpcReceiveSnapshot__Double__Vector3__Quaternion__Vector3__Vector3__Boolean__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveSnapshot called on server.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_RpcReceiveSnapshot__Double__Vector3__Quaternion__Vector3__Vector3__Boolean__Vector3(reader.ReadDouble(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadBool(), reader.ReadVector3());
			}
		}

		protected void UserCode_CmdSyncMultiplayerState__MultiplayerState(VehicleController.MultiplayerState state)
		{
			RpcSyncMultiplayerState(state);
		}

		protected static void InvokeUserCode_CmdSyncMultiplayerState__MultiplayerState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncMultiplayerState called on client.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_CmdSyncMultiplayerState__MultiplayerState(GeneratedNetworkCode._Read_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(reader));
			}
		}

		protected void UserCode_RpcSyncMultiplayerState__MultiplayerState(VehicleController.MultiplayerState state)
		{
			if (_vehicleInitialized && !base.isOwned)
			{
				_vehicleController.SetMultiplayerState(state);
			}
		}

		protected static void InvokeUserCode_RpcSyncMultiplayerState__MultiplayerState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSyncMultiplayerState called on server.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_RpcSyncMultiplayerState__MultiplayerState(GeneratedNetworkCode._Read_NWH_002EVehiclePhysics2_002EVehicleController_002FMultiplayerState(reader));
			}
		}

		protected void UserCode_CmdSyncDriverDisplay__DriverDisplayInputs(DriverDisplayInputs state)
		{
			Network_driverDisplay = state;
			ApplyDriverDisplay(state);
			RpcReceiveDriverDisplay(state);
		}

		protected static void InvokeUserCode_CmdSyncDriverDisplay__DriverDisplayInputs(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncDriverDisplay called on client.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_CmdSyncDriverDisplay__DriverDisplayInputs(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(reader));
			}
		}

		protected void UserCode_RpcReceiveDriverDisplay__DriverDisplayInputs(DriverDisplayInputs state)
		{
			if (!base.isServer)
			{
				ApplyDriverDisplay(state);
			}
		}

		protected static void InvokeUserCode_RpcReceiveDriverDisplay__DriverDisplayInputs(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveDriverDisplay called on server.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_RpcReceiveDriverDisplay__DriverDisplayInputs(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(reader));
			}
		}

		protected void UserCode_CmdSyncWheelVisualState__WheelVisualState(WheelVisualState state)
		{
			Network_wheelVisualState = state;
			ApplyWheelVisualState(state);
			RpcReceiveWheelVisualState(state);
		}

		protected static void InvokeUserCode_CmdSyncWheelVisualState__WheelVisualState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncWheelVisualState called on client.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_CmdSyncWheelVisualState__WheelVisualState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(reader));
			}
		}

		protected void UserCode_RpcReceiveWheelVisualState__WheelVisualState(WheelVisualState state)
		{
			if (!base.isServer)
			{
				ApplyWheelVisualState(state);
			}
		}

		protected static void InvokeUserCode_RpcReceiveWheelVisualState__WheelVisualState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveWheelVisualState called on server.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_RpcReceiveWheelVisualState__WheelVisualState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(reader));
			}
		}

		protected void UserCode_CmdSyncEngineAudio__EngineAudioState(EngineAudioState state)
		{
			Network_engineAudio = state;
			ApplyEngineAudio(state);
			RpcReceiveEngineAudio(state);
		}

		protected static void InvokeUserCode_CmdSyncEngineAudio__EngineAudioState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSyncEngineAudio called on client.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_CmdSyncEngineAudio__EngineAudioState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(reader));
			}
		}

		protected void UserCode_RpcReceiveEngineAudio__EngineAudioState(EngineAudioState state)
		{
			if (!base.isServer)
			{
				ApplyEngineAudio(state);
			}
		}

		protected static void InvokeUserCode_RpcReceiveEngineAudio__EngineAudioState(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveEngineAudio called on server.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_RpcReceiveEngineAudio__EngineAudioState(GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(reader));
			}
		}

		protected void UserCode_CmdRescueReposition__Vector3__Quaternion__Vector3(Vector3 safePos, Quaternion safeRot, Vector3 senderShift)
		{
			if (!HasDriver && !(_rigidbody == null))
			{
				safePos += CurrentOriginShift() - senderShift;
				ServerSetParkedFrozen(frozen: true);
				base.transform.SetPositionAndRotation(safePos, safeRot);
				_rigidbody.position = safePos;
				_rigidbody.rotation = safeRot;
				ServerSetParkedFrozen(frozen: false);
				ServerReassertPhysicsStateAfterRestore();
				VehicleFreezeManager.Instance?.ServerEvaluateNow(this);
				RpcRefreshAttachedParts();
			}
		}

		protected static void InvokeUserCode_CmdRescueReposition__Vector3__Quaternion__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRescueReposition called on client.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_CmdRescueReposition__Vector3__Quaternion__Vector3(reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadVector3());
			}
		}

		protected void UserCode_RpcRefreshAttachedParts()
		{
			GetComponent<VehicleManager>()?.RefreshAttachedPartsInteraction();
		}

		protected static void InvokeUserCode_RpcRefreshAttachedParts(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcRefreshAttachedParts called on server.");
			}
			else
			{
				((NetworkedNWHVehicle)obj).UserCode_RpcRefreshAttachedParts();
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(writer, _driverDisplay);
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(writer, _wheelVisualState);
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(writer, _engineAudio);
				writer.WriteBool(_hasDriver);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(writer, _driverDisplay);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(writer, _wheelVisualState);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(writer, _engineAudio);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteBool(_hasDriver);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _driverDisplay, _Mirror_SyncVarHookDelegate__driverDisplay, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(reader));
				GeneratedSyncVarDeserialize(ref _wheelVisualState, _Mirror_SyncVarHookDelegate__wheelVisualState, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(reader));
				GeneratedSyncVarDeserialize(ref _engineAudio, _Mirror_SyncVarHookDelegate__engineAudio, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(reader));
				GeneratedSyncVarDeserialize(ref _hasDriver, _Mirror_SyncVarHookDelegate__hasDriver, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _driverDisplay, _Mirror_SyncVarHookDelegate__driverDisplay, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EDriverDisplayInputs(reader));
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _wheelVisualState, _Mirror_SyncVarHookDelegate__wheelVisualState, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EWheelVisualState(reader));
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _engineAudio, _Mirror_SyncVarHookDelegate__engineAudio, GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002EVehicle_002ENetworking_002EEngineAudioState(reader));
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _hasDriver, _Mirror_SyncVarHookDelegate__hasDriver, reader.ReadBool());
			}
		}
	}
}
