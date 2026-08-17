using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using ECM2;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	[DefaultExecutionOrder(115)]
	public class PlayerVehicleSurfaceAttachment : NetworkBehaviour, IPlayerComponent, IFloatingOriginShiftable
	{
		[Header("References")]
		[SerializeField]
		private Character character;

		[SerializeField]
		private Player player;

		[SerializeField]
		private NetworkTransformUnreliable worldNetworkTransform;

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

		[SerializeField]
		private float snapshotSendInterval = 0.05f;

		[SerializeField]
		private float positionSensitivity = 0.005f;

		[SerializeField]
		private float rotationSensitivityDeg = 0.5f;

		[SerializeField]
		private float teleportDistance = 8f;

		[Header("Owner Tracking")]
		[Tooltip("Apply vehicle yaw delta to the player's rotation. With this off the player keeps independent FPS look while standing on a turning vehicle (typical FPS behaviour). With it on, the player turns with the vehicle.")]
		[SerializeField]
		private bool applyVehicleRotationToPlayer = true;

		[Tooltip("Stand-up hand-off: after OwnerForceAttach (standing up onto a moving vehicle), keep the forced attachment for this long so the auto-detector doesn't drop it before ECM2's movingPlatform detection catches up to the teleported player.")]
		[SerializeField]
		private float forceAttachGraceSeconds = 0.5f;

		[Tooltip("Ride interpolation is forced None only while the vehicle is actually moving; this is how long None is held after the last detected motion so brief stops don't thrash the interpolation mode. While parked the player's normal interpolation is restored so interior walking stays render-smooth instead of dropping to the 50 Hz FixedUpdate cadence.")]
		[SerializeField]
		private float rideMovingHoldSeconds = 0.25f;

		[Tooltip("Per-frame vehicle translation (metres) below which the vehicle counts as parked: the ride skips SetPosition and restores interpolation. Neutralises sub-mm render-interpolation micro-drift on a stationary vehicle. Raise if an idling vehicle's micro-jitter keeps the ride 'moving'.")]
		[SerializeField]
		private float rideMoveEpsilon = 0.0005f;

		[Tooltip("Per-frame vehicle rotation (degrees) below which the vehicle counts as parked (pairs with rideMoveEpsilon).")]
		[SerializeField]
		private float rideRotEpsilonDeg = 0.02f;

		[Header("Debug")]
		[SerializeField]
		private uint debugAttachedNetId;

		[SerializeField]
		private bool debugIsAttached;

		private const int SnapshotBufferLimit = 30;

		[Inject]
		private INetworkManager _networkManager;

		[SyncVar(hook = "OnAttachedVehicleNetIdChanged")]
		private uint _attachedVehicleNetId;

		private Transform _attachedVehicleTransform;

		private Rigidbody _attachedVehicleRigidbody;

		private CharacterMovement _characterMovement;

		private Vector3 _lastVehiclePosition;

		private Quaternion _lastVehicleRotation = Quaternion.identity;

		private bool _hasLastVehicleTransform;

		private VehicleSurfaceSnapshot _lastSentSnapshot;

		private bool _hasSentSnapshot;

		private float _sendTimer;

		private float _ownerForceAttachGraceTimer;

		private float _lastVehicleMotionTime = -1f / 0f;

		private RigidbodyInterpolation _originalInterpolation;

		private bool _interpolationOverridden;

		private bool _worldNtDisabledForRide;

		private readonly SortedList<double, VehicleSurfaceSnapshot> _snapshotBuffer = new SortedList<double, VehicleSurfaceSnapshot>();

		private double _localTimeline;

		private double _localTimescale = 1.0;

		private ExponentialMovingAverage _driftEma;

		private ExponentialMovingAverage _deliveryTimeEma;

		private bool _hasReceivedFirstSnapshot;

		private bool _isActive;

		private bool _enforcedFlagsDisabled;

		public Action<uint, uint> _Mirror_SyncVarHookDelegate__attachedVehicleNetId;

		public int SetupPriority => 35;

		private float BufferTime => snapshotSendInterval * bufferTimeMultiplier;

		public bool IsAttached
		{
			get
			{
				if (_attachedVehicleNetId != 0)
				{
					return _attachedVehicleTransform != null;
				}
				return false;
			}
		}

		public uint AttachedVehicleNetId => _attachedVehicleNetId;

		public uint Network_attachedVehicleNetId
		{
			get
			{
				return _attachedVehicleNetId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _attachedVehicleNetId, 1uL, _Mirror_SyncVarHookDelegate__attachedVehicleNetId);
			}
		}

		public void SetupForPlayer(bool isLocalPlayer)
		{
			_isActive = true;
			base.enabled = true;
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			if (character == null)
			{
				character = GetComponent<Character>();
			}
			if (player == null)
			{
				player = GetComponent<Player>();
			}
			if (worldNetworkTransform == null)
			{
				worldNetworkTransform = GetComponent<NetworkTransformUnreliable>();
			}
		}

		private void Awake()
		{
			if (character == null)
			{
				character = GetComponent<Character>();
			}
			if (player == null)
			{
				player = GetComponent<Player>();
			}
			if (worldNetworkTransform == null)
			{
				worldNetworkTransform = ResolveRootNetworkTransform();
			}
			if (character != null)
			{
				_characterMovement = character.characterMovement;
			}
			EnforcePlatformImpartFlagsDisabled();
			_driftEma = new ExponentialMovingAverage(10);
			_deliveryTimeEma = new ExponentialMovingAverage(10);
		}

		private NetworkTransformUnreliable ResolveRootNetworkTransform()
		{
			NetworkTransformUnreliable[] components = GetComponents<NetworkTransformUnreliable>();
			NetworkTransformUnreliable[] array = components;
			foreach (NetworkTransformUnreliable networkTransformUnreliable in array)
			{
				if (!(networkTransformUnreliable == null) && (networkTransformUnreliable.target == null || networkTransformUnreliable.target == base.transform))
				{
					return networkTransformUnreliable;
				}
			}
			if (components.Length == 0)
			{
				return null;
			}
			return components[0];
		}

		private void OnEnable()
		{
			FloatingOriginManager.RegisterShiftable(this);
		}

		private void OnDisable()
		{
			FloatingOriginManager.UnregisterShiftable(this);
			ApplyRidingInterpolation(riding: false);
			ReconcileWorldNetworkTransform(riding: false);
		}

		public void OnOriginShift(Vector3 delta)
		{
			if (_hasLastVehicleTransform)
			{
				_lastVehiclePosition += delta;
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (_attachedVehicleNetId != 0)
			{
				AttachLocallyAsync(_attachedVehicleNetId).Forget();
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			if (_attachedVehicleNetId != 0)
			{
				DetachLocally(applyVelocity: false);
			}
		}

		private void Update()
		{
			if (!_isActive)
			{
				return;
			}
			ReconcileWorldNetworkTransform(_attachedVehicleTransform != null);
			if (base.isOwned)
			{
				UpdateOwnerDetection();
				if (_attachedVehicleTransform != null)
				{
					ApplyOwnerVehicleDelta();
				}
				ApplyRidingInterpolation(_attachedVehicleTransform != null && RideNeedsNoInterpolation());
			}
			else if (_attachedVehicleNetId != 0)
			{
				UpdateRemoteInterpolation();
			}
		}

		private void LateUpdate()
		{
			if (_isActive && base.isOwned && !(_attachedVehicleTransform == null))
			{
				TrySendSnapshot();
			}
		}

		private void UpdateOwnerDetection()
		{
			if (player != null && player.IsPlayerSitting())
			{
				_ownerForceAttachGraceTimer = 0f;
				if (_attachedVehicleNetId != 0)
				{
					CmdRequestDetach();
				}
				return;
			}
			if (_ownerForceAttachGraceTimer > 0f)
			{
				_ownerForceAttachGraceTimer -= Time.deltaTime;
				if (_attachedVehicleNetId != 0 && DetectGroundVehicleNetId() == _attachedVehicleNetId)
				{
					_ownerForceAttachGraceTimer = 0f;
				}
				return;
			}
			uint num = DetectGroundVehicleNetId();
			if (num != _attachedVehicleNetId)
			{
				if (num == 0)
				{
					CmdRequestDetach();
				}
				else
				{
					CmdRequestAttach(num);
				}
			}
		}

		private uint DetectGroundVehicleNetId()
		{
			if (_characterMovement == null)
			{
				return 0u;
			}
			Rigidbody platform = _characterMovement.movingPlatform.platform;
			if (platform == null)
			{
				return 0u;
			}
			if (!platform.TryGetComponent<NetworkedNWHVehicle>(out var component))
			{
				return 0u;
			}
			if (component.netIdentity == null)
			{
				return 0u;
			}
			return component.netIdentity.netId;
		}

		private void ApplyOwnerVehicleDelta()
		{
			Transform attachedVehicleTransform = _attachedVehicleTransform;
			Vector3 position = attachedVehicleTransform.position;
			Quaternion rotation = attachedVehicleTransform.rotation;
			if (!_hasLastVehicleTransform)
			{
				_lastVehiclePosition = position;
				_lastVehicleRotation = rotation;
				_hasLastVehicleTransform = true;
				return;
			}
			float sqrMagnitude = (position - _lastVehiclePosition).sqrMagnitude;
			float num = Quaternion.Angle(rotation, _lastVehicleRotation);
			if (sqrMagnitude <= rideMoveEpsilon * rideMoveEpsilon && num <= rideRotEpsilonDeg)
			{
				return;
			}
			_lastVehicleMotionTime = Time.time;
			Vector3 vector = ((_characterMovement != null) ? _characterMovement.position : base.transform.position);
			Quaternion quaternion = rotation * Quaternion.Inverse(_lastVehicleRotation);
			Vector3 vector2 = vector - _lastVehiclePosition;
			Vector3 vector3 = quaternion * vector2;
			Vector3 vector4 = position + vector3;
			_lastVehiclePosition = position;
			_lastVehicleRotation = rotation;
			if ((vector4 - vector).sqrMagnitude > 0f)
			{
				if (_characterMovement != null)
				{
					_characterMovement.SetPosition(vector4);
				}
				else
				{
					base.transform.position = vector4;
				}
			}
			if (applyVehicleRotationToPlayer && _characterMovement != null)
			{
				Quaternion quaternion2 = ExtractYawDelta(quaternion);
				if (Quaternion.Angle(quaternion2, Quaternion.identity) > 0.001f)
				{
					_characterMovement.rotation = quaternion2 * _characterMovement.rotation;
				}
			}
		}

		private static Quaternion ExtractYawDelta(Quaternion fullDelta)
		{
			Vector3 vector = fullDelta * Vector3.forward;
			vector.y = 0f;
			if (vector.sqrMagnitude < 1E-06f)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		private void TrySendSnapshot()
		{
			_sendTimer += Time.deltaTime;
			if (_sendTimer < snapshotSendInterval)
			{
				return;
			}
			_sendTimer = 0f;
			Transform attachedVehicleTransform = _attachedVehicleTransform;
			Vector3 vector = attachedVehicleTransform.InverseTransformPoint(base.transform.position);
			Quaternion quaternion = Quaternion.Inverse(attachedVehicleTransform.rotation) * base.transform.rotation;
			if (_hasSentSnapshot)
			{
				bool num = (vector - _lastSentSnapshot.localPosition).sqrMagnitude > positionSensitivity * positionSensitivity;
				bool flag = Quaternion.Angle(quaternion, _lastSentSnapshot.localRotation) > rotationSensitivityDeg;
				if (!num && !flag)
				{
					return;
				}
			}
			_lastSentSnapshot = new VehicleSurfaceSnapshot(NetworkTime.localTime, NetworkTime.localTime, vector, quaternion);
			_hasSentSnapshot = true;
			CmdSendSnapshot(NetworkTime.localTime, vector, quaternion);
		}

		[Command(channel = 1)]
		private void CmdSendSnapshot(double timestamp, Vector3 localPos, Quaternion localRot)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteDouble(timestamp);
			writer.WriteVector3(localPos);
			writer.WriteQuaternion(localRot);
			SendCommandInternal("System.Void NomadDrive.Features.Player.PlayerVehicleSurfaceAttachment::CmdSendSnapshot(System.Double,UnityEngine.Vector3,UnityEngine.Quaternion)", -426417097, writer, 1);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false, channel = 1)]
		private void RpcReceiveSnapshot(double timestamp, Vector3 localPos, Quaternion localRot)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteDouble(timestamp);
			writer.WriteVector3(localPos);
			writer.WriteQuaternion(localRot);
			SendRPCInternal("System.Void NomadDrive.Features.Player.PlayerVehicleSurfaceAttachment::RpcReceiveSnapshot(System.Double,UnityEngine.Vector3,UnityEngine.Quaternion)", -813506715, writer, 1, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnSnapshotReceived(double timestamp, Vector3 localPos, Quaternion localRot)
		{
			if (base.isOwned || _attachedVehicleNetId == 0)
			{
				return;
			}
			VehicleSurfaceSnapshot snapshot = new VehicleSurfaceSnapshot(timestamp, NetworkTime.localTime, localPos, localRot);
			if (!_hasReceivedFirstSnapshot)
			{
				_hasReceivedFirstSnapshot = true;
				if (_attachedVehicleTransform != null)
				{
					base.transform.position = _attachedVehicleTransform.TransformPoint(localPos);
					base.transform.rotation = _attachedVehicleTransform.rotation * localRot;
				}
			}
			SnapshotInterpolation.InsertAndAdjust(_snapshotBuffer, 30, snapshot, ref _localTimeline, ref _localTimescale, snapshotSendInterval, BufferTime, catchupSpeed, slowdownSpeed, ref _driftEma, catchupNegativeThreshold, catchupPositiveThreshold, ref _deliveryTimeEma);
		}

		private void UpdateRemoteInterpolation()
		{
			if (_attachedVehicleNetId != 0 && !(_attachedVehicleTransform == null) && _hasReceivedFirstSnapshot && _snapshotBuffer.Count != 0)
			{
				SnapshotInterpolation.Step(_snapshotBuffer, Time.deltaTime, ref _localTimeline, _localTimescale, out var fromSnapshot, out var toSnapshot, out var t);
				VehicleSurfaceSnapshot vehicleSurfaceSnapshot = VehicleSurfaceSnapshot.Interpolate(fromSnapshot, toSnapshot, t);
				Vector3 vector = _attachedVehicleTransform.TransformPoint(vehicleSurfaceSnapshot.localPosition);
				Quaternion rotation = _attachedVehicleTransform.rotation * vehicleSurfaceSnapshot.localRotation;
				if (Vector3.Distance(base.transform.position, vector) > teleportDistance)
				{
					base.transform.SetPositionAndRotation(vector, rotation);
				}
				else
				{
					base.transform.SetPositionAndRotation(vector, rotation);
				}
			}
		}

		[Command]
		private void CmdRequestAttach(uint vehicleNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(vehicleNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Player.PlayerVehicleSurfaceAttachment::CmdRequestAttach(System.UInt32)", 1076650173, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdRequestDetach()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Player.PlayerVehicleSurfaceAttachment::CmdRequestDetach()", -246056173, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		private void OnAttachedVehicleNetIdChanged(uint oldId, uint newId)
		{
			if (newId == 0)
			{
				DetachLocally(applyVelocity: true);
			}
			else
			{
				AttachLocallyAsync(newId).Forget();
			}
		}

		private async UniTaskVoid AttachLocallyAsync(uint vehicleNetId)
		{
			if (_networkManager == null)
			{
				EvilLogger.LogError("[PlayerVehicleSurfaceAttachment] INetworkManager is not injected.", "AttachLocallyAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\PlayerVehicleSurfaceAttachment.cs", 587);
				return;
			}
			var (flag, gameObject) = await _networkManager.TryGetNetworkObjectByIdWithBackoffAsync(vehicleNetId, 30, 100, 2000, 1.5f, this.GetCancellationTokenOnDestroy());
			if (flag && !(gameObject == null) && _attachedVehicleNetId == vehicleNetId)
			{
				SetupLocalAttachment(gameObject.transform, gameObject.GetComponent<Rigidbody>());
			}
		}

		public void OwnerForceAttach(NetworkedNWHVehicle vehicle)
		{
			if (base.isOwned && !(vehicle == null) && !(vehicle.netIdentity == null))
			{
				SetupLocalAttachment(vehicle.transform, vehicle.GetComponent<Rigidbody>());
				_ownerForceAttachGraceTimer = forceAttachGraceSeconds;
				_lastVehicleMotionTime = Time.time;
				if (_attachedVehicleNetId != vehicle.netIdentity.netId)
				{
					CmdRequestAttach(vehicle.netIdentity.netId);
				}
			}
		}

		private void SetupLocalAttachment(Transform vehicleTransform, Rigidbody vehicleRigidbody)
		{
			_attachedVehicleTransform = vehicleTransform;
			_attachedVehicleRigidbody = vehicleRigidbody;
			_hasLastVehicleTransform = false;
			_snapshotBuffer.Clear();
			_hasReceivedFirstSnapshot = false;
			_hasSentSnapshot = false;
			_localTimeline = 0.0;
			_localTimescale = 1.0;
		}

		private void DetachLocally(bool applyVelocity)
		{
			if (applyVelocity && base.isOwned && _attachedVehicleRigidbody != null && character != null)
			{
				Vector3 pointVelocity = _attachedVehicleRigidbody.GetPointVelocity(base.transform.position);
				character.LaunchCharacter(pointVelocity, overrideVerticalVelocity: true, overrideLateralVelocity: true);
			}
			_attachedVehicleTransform = null;
			_attachedVehicleRigidbody = null;
			_hasLastVehicleTransform = false;
			_snapshotBuffer.Clear();
			_hasReceivedFirstSnapshot = false;
			_hasSentSnapshot = false;
		}

		private bool RideNeedsNoInterpolation()
		{
			return Time.time - _lastVehicleMotionTime < rideMovingHoldSeconds;
		}

		private void ApplyRidingInterpolation(bool riding)
		{
			if (base.isOwned && !(_characterMovement == null) && _interpolationOverridden != riding)
			{
				if (riding)
				{
					_originalInterpolation = _characterMovement.interpolation;
					_characterMovement.interpolation = RigidbodyInterpolation.None;
				}
				else
				{
					_characterMovement.interpolation = _originalInterpolation;
				}
				_interpolationOverridden = riding;
			}
		}

		private void ReconcileWorldNetworkTransform(bool riding)
		{
			if (!(worldNetworkTransform == null) && _worldNtDisabledForRide != riding)
			{
				worldNetworkTransform.enabled = !riding;
				_worldNtDisabledForRide = riding;
			}
		}

		private void EnforcePlatformImpartFlagsDisabled()
		{
			if (!_enforcedFlagsDisabled && !(_characterMovement == null))
			{
				_characterMovement.impartPlatformMovement = false;
				_characterMovement.impartPlatformRotation = false;
				_characterMovement.impartPlatformVelocity = false;
				_enforcedFlagsDisabled = true;
			}
		}

		public PlayerVehicleSurfaceAttachment()
		{
			_Mirror_SyncVarHookDelegate__attachedVehicleNetId = OnAttachedVehicleNetIdChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdSendSnapshot__Double__Vector3__Quaternion(double timestamp, Vector3 localPos, Quaternion localRot)
		{
			OnSnapshotReceived(timestamp, localPos, localRot);
			RpcReceiveSnapshot(timestamp, localPos, localRot);
		}

		protected static void InvokeUserCode_CmdSendSnapshot__Double__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSendSnapshot called on client.");
			}
			else
			{
				((PlayerVehicleSurfaceAttachment)obj).UserCode_CmdSendSnapshot__Double__Vector3__Quaternion(reader.ReadDouble(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_RpcReceiveSnapshot__Double__Vector3__Quaternion(double timestamp, Vector3 localPos, Quaternion localRot)
		{
			if (!base.isServer)
			{
				OnSnapshotReceived(timestamp, localPos, localRot);
			}
		}

		protected static void InvokeUserCode_RpcReceiveSnapshot__Double__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReceiveSnapshot called on server.");
			}
			else
			{
				((PlayerVehicleSurfaceAttachment)obj).UserCode_RpcReceiveSnapshot__Double__Vector3__Quaternion(reader.ReadDouble(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		protected void UserCode_CmdRequestAttach__UInt32(uint vehicleNetId)
		{
			if (vehicleNetId != 0)
			{
				Network_attachedVehicleNetId = vehicleNetId;
			}
		}

		protected static void InvokeUserCode_CmdRequestAttach__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestAttach called on client.");
			}
			else
			{
				((PlayerVehicleSurfaceAttachment)obj).UserCode_CmdRequestAttach__UInt32(reader.ReadVarUInt());
			}
		}

		protected void UserCode_CmdRequestDetach()
		{
			Network_attachedVehicleNetId = 0u;
		}

		protected static void InvokeUserCode_CmdRequestDetach(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestDetach called on client.");
			}
			else
			{
				((PlayerVehicleSurfaceAttachment)obj).UserCode_CmdRequestDetach();
			}
		}

		static PlayerVehicleSurfaceAttachment()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerVehicleSurfaceAttachment), "System.Void NomadDrive.Features.Player.PlayerVehicleSurfaceAttachment::CmdSendSnapshot(System.Double,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_CmdSendSnapshot__Double__Vector3__Quaternion, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerVehicleSurfaceAttachment), "System.Void NomadDrive.Features.Player.PlayerVehicleSurfaceAttachment::CmdRequestAttach(System.UInt32)", InvokeUserCode_CmdRequestAttach__UInt32, requiresAuthority: true);
			RemoteProcedureCalls.RegisterCommand(typeof(PlayerVehicleSurfaceAttachment), "System.Void NomadDrive.Features.Player.PlayerVehicleSurfaceAttachment::CmdRequestDetach()", InvokeUserCode_CmdRequestDetach, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(PlayerVehicleSurfaceAttachment), "System.Void NomadDrive.Features.Player.PlayerVehicleSurfaceAttachment::RpcReceiveSnapshot(System.Double,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcReceiveSnapshot__Double__Vector3__Quaternion);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarUInt(_attachedVehicleNetId);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarUInt(_attachedVehicleNetId);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _attachedVehicleNetId, _Mirror_SyncVarHookDelegate__attachedVehicleNetId, reader.ReadVarUInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _attachedVehicleNetId, _Mirror_SyncVarHookDelegate__attachedVehicleNetId, reader.ReadVarUInt());
			}
		}
	}
}
