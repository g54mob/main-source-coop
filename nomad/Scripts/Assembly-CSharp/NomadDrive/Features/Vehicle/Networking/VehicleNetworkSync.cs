using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using EvilCore.Networking.Parenting;
using Mirror;
using Mirror.RemoteCalls;
using NWH.VehiclePhysics2;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Features.Vehicle.Collision;
using NomadDrive.Features.Vehicle.Interactables;
using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Networking
{
	public class VehicleNetworkSync : NetworkBehaviour, INetworkSaveable
	{
		public readonly SyncDictionary<byte, uint> SlotAttachments = new SyncDictionary<byte, uint>();

		public readonly SyncDictionary<byte, byte> InteractableStates = new SyncDictionary<byte, byte>();

		[SyncVar(hook = "OnIsBrakingChanged")]
		private bool _isBraking;

		[SyncVar]
		private float _engineRpm;

		[SyncVar]
		private float _vehicleSpeedKmh;

		[SyncVar]
		private float _fuelAmount;

		[SyncVar]
		private string _gearName = string.Empty;

		[SyncVar(hook = "OnOdometerKmChanged")]
		private int _odometerKm;

		private const float TelemetrySendInterval = 0.1f;

		private float _telemetryTimer;

		private const float OdometerSpeedDeadZone = 0.3f;

		private double _odometerMetersLocal;

		private int _lastSentKm;

		private const float BrakePressThreshold = 0.1f;

		private const float BrakeReleaseThreshold = 0.05f;

		private bool _brakeEventsWired;

		private VehicleManager _vehicleManager;

		private readonly Dictionary<byte, VehicleSlot> _registeredSlots = new Dictionary<byte, VehicleSlot>();

		private readonly Dictionary<byte, VehicleInteractable> _registeredInteractables = new Dictionary<byte, VehicleInteractable>();

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__isBraking;

		public Action<int, int> _Mirror_SyncVarHookDelegate__odometerKm;

		public bool IsBraking => _isBraking;

		public float EngineRpm => _engineRpm;

		public float VehicleSpeedKmh => _vehicleSpeedKmh;

		public float FuelAmount => _fuelAmount;

		public string GearName => _gearName;

		public int OdometerKm => _odometerKm;

		public string ContributorKey => "vehicle";

		public bool Network_isBraking
		{
			get
			{
				return _isBraking;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isBraking, 1uL, _Mirror_SyncVarHookDelegate__isBraking);
			}
		}

		public float Network_engineRpm
		{
			get
			{
				return _engineRpm;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _engineRpm, 2uL, null);
			}
		}

		public float Network_vehicleSpeedKmh
		{
			get
			{
				return _vehicleSpeedKmh;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _vehicleSpeedKmh, 4uL, null);
			}
		}

		public float Network_fuelAmount
		{
			get
			{
				return _fuelAmount;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _fuelAmount, 8uL, null);
			}
		}

		public string Network_gearName
		{
			get
			{
				return _gearName;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _gearName, 16uL, null);
			}
		}

		public int Network_odometerKm
		{
			get
			{
				return _odometerKm;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _odometerKm, 32uL, _Mirror_SyncVarHookDelegate__odometerKm);
			}
		}

		private void Awake()
		{
			_vehicleManager = GetComponentInParent<VehicleManager>();
		}

		public void RegisterSlot(VehicleSlotId slotId, VehicleSlot slot)
		{
			byte b = (byte)slotId;
			_registeredSlots[b] = slot;
			if (base.isServer && !SlotAttachments.ContainsKey(b))
			{
				SlotAttachments[b] = 0u;
			}
		}

		public void RegisterInteractable(VehicleInteractableId id, byte instanceIndex, VehicleInteractable interactable, bool syncState = true)
		{
			byte b = VehicleInteractableKey.Compose(id, instanceIndex);
			if (_registeredInteractables.TryGetValue(b, out var value) && value != null && value != interactable)
			{
				EvilLogger.LogError($"[VehicleNetworkSync] Duplicate interactable key for {id} instance {instanceIndex} " + "('" + value.name + "' vs '" + interactable.name + "'). Assign a unique instance index per interactable.", "RegisterInteractable", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Networking\\VehicleNetworkSync.cs", 98);
			}
			_registeredInteractables[b] = interactable;
			if (syncState && base.isServer && !InteractableStates.ContainsKey(b))
			{
				InteractableStates[b] = 0;
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdAttachToSlot(byte slotId, uint objectNetId, NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, slotId);
			writer.WriteVarUInt(objectNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdAttachToSlot(System.Byte,System.UInt32,Mirror.NetworkConnectionToClient)", -1390649170, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetAttachRejected(NetworkConnectionToClient target, byte slotId, uint objectNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, slotId);
			writer.WriteVarUInt(objectNetId);
			SendTargetRPCInternal(target, "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::TargetAttachRejected(Mirror.NetworkConnectionToClient,System.Byte,System.UInt32)", 1508084568, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		public void CmdDetachFromSlot(byte slotId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, slotId);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdDetachFromSlot(System.Byte)", -214797324, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerAttachToSlot(byte slotId, uint objectNetId)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::ServerAttachToSlot(System.Byte,System.UInt32)' called when server was not active");
			}
			else
			{
				SlotAttachments[slotId] = objectNetId;
			}
		}

		[Server]
		public void ServerDetachFromSlot(byte slotId)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::ServerDetachFromSlot(System.Byte)' called when server was not active");
			}
			else
			{
				SlotAttachments[slotId] = 0u;
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdSetInteractableState(byte interactableId, byte state)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, interactableId);
			NetworkWriterExtensions.WriteByte(writer, state);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdSetInteractableState(System.Byte,System.Byte)", 314483757, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerSetInteractableState(byte interactableId, byte state)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::ServerSetInteractableState(System.Byte,System.Byte)' called when server was not active");
			}
			else
			{
				InteractableStates[interactableId] = state;
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SyncDictionary<byte, uint> slotAttachments = SlotAttachments;
			slotAttachments.OnChange = (Action<SyncIDictionary<byte, uint>.Operation, byte, uint>)Delegate.Combine(slotAttachments.OnChange, new Action<SyncIDictionary<byte, uint>.Operation, byte, uint>(OnSlotAttachmentChanged));
			SyncDictionary<byte, byte> interactableStates = InteractableStates;
			interactableStates.OnChange = (Action<SyncIDictionary<byte, byte>.Operation, byte, byte>)Delegate.Combine(interactableStates.OnChange, new Action<SyncIDictionary<byte, byte>.Operation, byte, byte>(OnInteractableStateChanged));
			WireBrakeEvents();
			ApplyAllStatesForLateJoiner();
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			SyncDictionary<byte, uint> slotAttachments = SlotAttachments;
			slotAttachments.OnChange = (Action<SyncIDictionary<byte, uint>.Operation, byte, uint>)Delegate.Remove(slotAttachments.OnChange, new Action<SyncIDictionary<byte, uint>.Operation, byte, uint>(OnSlotAttachmentChanged));
			SyncDictionary<byte, byte> interactableStates = InteractableStates;
			interactableStates.OnChange = (Action<SyncIDictionary<byte, byte>.Operation, byte, byte>)Delegate.Remove(interactableStates.OnChange, new Action<SyncIDictionary<byte, byte>.Operation, byte, byte>(OnInteractableStateChanged));
			UnwireBrakeEvents();
		}

		private void WireBrakeEvents()
		{
			if (!_brakeEventsWired && _vehicleManager?.EventBus != null)
			{
				_vehicleManager.EventBus.OnFrontSeatsVacated += OnFrontSeatsVacatedHandler;
				_brakeEventsWired = true;
			}
		}

		private void UnwireBrakeEvents()
		{
			if (_brakeEventsWired)
			{
				if (_vehicleManager?.EventBus != null)
				{
					_vehicleManager.EventBus.OnFrontSeatsVacated -= OnFrontSeatsVacatedHandler;
				}
				_brakeEventsWired = false;
			}
		}

		private void OnFrontSeatsVacatedHandler()
		{
			if (base.isOwned && _isBraking)
			{
				CmdSetBraking(value: false);
			}
		}

		[Command(requiresAuthority = false)]
		public void CmdSetBraking(bool value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteBool(value);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdSetBraking(System.Boolean)", -512363793, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdPushDashboardTelemetry(float rpm, float speedKmh, float fuel, string gearName)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(rpm);
			writer.WriteFloat(speedKmh);
			writer.WriteFloat(fuel);
			writer.WriteString(gearName);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdPushDashboardTelemetry(System.Single,System.Single,System.Single,System.String)", -134103910, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void Update()
		{
			if (!base.isOwned)
			{
				return;
			}
			VehicleController vehicleController = _vehicleManager?.VehicleController;
			float num = ((vehicleController != null) ? vehicleController.input.Brakes : 0f);
			bool flag = (_isBraking ? (num > 0.05f) : (num >= 0.1f));
			if (flag != _isBraking)
			{
				CmdSetBraking(flag);
			}
			_telemetryTimer += Time.deltaTime;
			if (_telemetryTimer < 0.1f)
			{
				return;
			}
			_telemetryTimer = 0f;
			if (vehicleController == null)
			{
				return;
			}
			float outputRPM = vehicleController.powertrain.engine.OutputRPM;
			float num2 = vehicleController.Speed * 3.6f;
			string text = vehicleController.powertrain.transmission.GearName ?? string.Empty;
			float valueOrDefault = (_vehicleManager.FuelModuleWrapper?.module?.amount).GetValueOrDefault();
			if (base.isServer)
			{
				Network_engineRpm = outputRPM;
				Network_vehicleSpeedKmh = num2;
				Network_fuelAmount = valueOrDefault;
				Network_gearName = text;
			}
			else
			{
				CmdPushDashboardTelemetry(outputRPM, num2, valueOrDefault, text);
			}
			float num3 = Mathf.Abs(vehicleController.Speed);
			if (num3 >= 0.3f)
			{
				_odometerMetersLocal += num3 * 0.1f;
			}
			int num4 = (int)(_odometerMetersLocal / 1000.0);
			if (num4 > 999999)
			{
				num4 = 999999;
			}
			if (num4 > _odometerKm)
			{
				if (base.isServer)
				{
					Network_odometerKm = num4;
				}
				else if (num4 > _lastSentKm)
				{
					CmdReportOdometerKm(num4);
					_lastSentKm = num4;
				}
			}
		}

		private void OnIsBrakingChanged(bool oldValue, bool newValue)
		{
			if (_vehicleManager?.EventBus != null)
			{
				if (newValue)
				{
					_vehicleManager.EventBus.FireBrakePressed();
				}
				else
				{
					_vehicleManager.EventBus.FireBrakeReleased();
				}
			}
		}

		private void ApplyAllStatesForLateJoiner()
		{
			ApplyAllStatesAsync().Forget();
		}

		private async UniTaskVoid ApplyAllStatesAsync()
		{
			await UniTask.DelayFrame(2, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			foreach (KeyValuePair<byte, uint> slotAttachment in SlotAttachments)
			{
				NotifySlot(slotAttachment.Key, slotAttachment.Value);
			}
			foreach (KeyValuePair<byte, byte> interactableState in InteractableStates)
			{
				NotifyInteractable(interactableState.Key, interactableState.Value, skipAnimation: true);
			}
			for (int i = 0; i < 14; i++)
			{
				_vehicleManager?.RefreshAttachedPartsInteraction();
				await UniTask.Delay(TimeSpan.FromSeconds(0.75), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			}
		}

		private void OnSlotAttachmentChanged(SyncIDictionary<byte, uint>.Operation op, byte key, uint _)
		{
			if ((uint)op == 1u || (uint)op == 0u)
			{
				NotifySlot(key, SlotAttachments[key]);
			}
		}

		private void OnInteractableStateChanged(SyncIDictionary<byte, byte>.Operation op, byte key, byte _)
		{
			if ((uint)op == 1u || (uint)op == 0u)
			{
				NotifyInteractable(key, InteractableStates[key], skipAnimation: false);
			}
		}

		private void NotifySlot(byte slotId, uint netId)
		{
			if (!_registeredSlots.TryGetValue(slotId, out var value))
			{
				return;
			}
			try
			{
				value.OnNetworkAttachmentChanged(netId);
			}
			catch (Exception arg)
			{
				EvilLogger.LogError($"[VehicleNetworkSync] Slot {slotId} OnNetworkAttachmentChanged threw: {arg}", "NotifySlot", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Networking\\VehicleNetworkSync.cs", 381);
			}
		}

		private void NotifyInteractable(byte id, byte newState, bool skipAnimation)
		{
			if (!_registeredInteractables.TryGetValue(id, out var value))
			{
				return;
			}
			try
			{
				value.ApplyStateFromNetwork(newState, skipAnimation);
			}
			catch (Exception arg)
			{
				EvilLogger.LogError($"[VehicleNetworkSync] Interactable {id} ApplyStateFromNetwork threw: {arg}", "NotifyInteractable", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Networking\\VehicleNetworkSync.cs", 397);
			}
		}

		public bool TryGetInteractable(byte id, out VehicleInteractable interactable)
		{
			return _registeredInteractables.TryGetValue(id, out interactable);
		}

		public uint GetSlotAttachment(VehicleSlotId slotId)
		{
			byte key = (byte)slotId;
			if (!SlotAttachments.TryGetValue(key, out var value))
			{
				return 0u;
			}
			return value;
		}

		public byte GetInteractableState(VehicleInteractableId id)
		{
			byte key = (byte)id;
			if (!InteractableStates.TryGetValue(key, out var value))
			{
				return 0;
			}
			return value;
		}

		public bool IsSlotOccupied(VehicleSlotId slotId)
		{
			return GetSlotAttachment(slotId) != 0;
		}

		public override void OnStartAuthority()
		{
			base.OnStartAuthority();
			SeedOdometerAccumulator(_odometerKm);
			_lastSentKm = _odometerKm;
		}

		private void OnOdometerKmChanged(int oldValue, int newValue)
		{
			SeedOdometerAccumulator(newValue);
			if (newValue > _lastSentKm)
			{
				_lastSentKm = newValue;
			}
		}

		private void SeedOdometerAccumulator(int km)
		{
			double num = (double)km * 1000.0;
			if (num > _odometerMetersLocal)
			{
				_odometerMetersLocal = num;
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdReportOdometerKm(int km)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(km);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdReportOdometerKm(System.Int32)", -1177044128, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		public void ServerSetOdometerKm(int km)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::ServerSetOdometerKm(System.Int32)' called when server was not active");
				return;
			}
			Network_odometerKm = Mathf.Clamp(km, 0, 999999);
			SeedOdometerAccumulator(_odometerKm);
		}

		public bool TryGetSlot(byte slotId, out VehicleSlot slot)
		{
			return _registeredSlots.TryGetValue(slotId, out slot);
		}

		[Command(requiresAuthority = false)]
		public void CmdReportCollision(Vector3 impactPoint, Vector3 impactNormal, float force, Vector3 relativeVelocity, float relativeSpeed, byte severity, uint collidedObjectNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(impactPoint);
			writer.WriteVector3(impactNormal);
			writer.WriteFloat(force);
			writer.WriteVector3(relativeVelocity);
			writer.WriteFloat(relativeSpeed);
			NetworkWriterExtensions.WriteByte(writer, severity);
			writer.WriteVarUInt(collidedObjectNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdReportCollision(UnityEngine.Vector3,UnityEngine.Vector3,System.Single,UnityEngine.Vector3,System.Single,System.Byte,System.UInt32)", 574247190, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcBroadcastCollision(Vector3 impactPoint, Vector3 impactNormal, float force, Vector3 relativeVelocity, float relativeSpeed, byte severity, uint collidedObjectNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(impactPoint);
			writer.WriteVector3(impactNormal);
			writer.WriteFloat(force);
			writer.WriteVector3(relativeVelocity);
			writer.WriteFloat(relativeSpeed);
			NetworkWriterExtensions.WriteByte(writer, severity);
			writer.WriteVarUInt(collidedObjectNetId);
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::RpcBroadcastCollision(UnityEngine.Vector3,UnityEngine.Vector3,System.Single,UnityEngine.Vector3,System.Single,System.Byte,System.UInt32)", 441851948, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		public void RpcSettleDetachedPart(uint partNetId, Vector3 position, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(partNetId);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation);
			SendRPCInternal("System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::RpcSettleDetachedPart(System.UInt32,UnityEngine.Vector3,UnityEngine.Quaternion)", 781848429, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)2);
			List<KeyValuePair<byte, string>> list = new List<KeyValuePair<byte, string>>();
			foreach (KeyValuePair<byte, uint> slotAttachment in SlotAttachments)
			{
				if (slotAttachment.Value != 0)
				{
					string value = ctx.ToGuid(slotAttachment.Value);
					if (!string.IsNullOrEmpty(value))
					{
						list.Add(new KeyValuePair<byte, string>(slotAttachment.Key, value));
					}
				}
			}
			writer.Write(list.Count);
			foreach (KeyValuePair<byte, string> item in list)
			{
				writer.Write(item.Key);
				writer.Write(item.Value);
			}
			writer.Write(InteractableStates.Count);
			foreach (KeyValuePair<byte, byte> interactableState in InteractableStates)
			{
				writer.Write(interactableState.Key);
				writer.Write(interactableState.Value);
			}
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
			if (!NetworkServer.active)
			{
				return;
			}
			byte b = reader.ReadByte();
			int num = reader.ReadInt();
			for (int i = 0; i < num; i++)
			{
				byte slotId = reader.ReadByte();
				string guid = reader.ReadString();
				if (ctx.TryToNetId(guid, out var objectNetId))
				{
					ServerAttachToSlot(slotId, objectNetId);
				}
			}
			if (b >= 2)
			{
				List<KeyValuePair<byte, byte>> list = new List<KeyValuePair<byte, byte>>();
				int num2 = reader.ReadInt();
				for (int j = 0; j < num2; j++)
				{
					byte key = reader.ReadByte();
					byte value = reader.ReadByte();
					list.Add(new KeyValuePair<byte, byte>(key, value));
				}
				if (list.Count > 0)
				{
					ApplyRestoredInteractablesAsync(list).Forget();
				}
			}
		}

		private async UniTaskVoid ApplyRestoredInteractablesAsync(List<KeyValuePair<byte, byte>> states)
		{
			await UniTask.DelayFrame(10, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
			if (!NetworkServer.active)
			{
				return;
			}
			foreach (KeyValuePair<byte, byte> state in states)
			{
				byte b = VehicleInteractableKey.DecodeBaseId(state.Key);
				if (b != 0 && b != 5 && b != 9 && b != 10)
				{
					ServerSetInteractableState(state.Key, state.Value);
				}
			}
		}

		public VehicleNetworkSync()
		{
			InitSyncObject(SlotAttachments);
			InitSyncObject(InteractableStates);
			_Mirror_SyncVarHookDelegate__isBraking = OnIsBrakingChanged;
			_Mirror_SyncVarHookDelegate__odometerKm = OnOdometerKmChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdAttachToSlot__Byte__UInt32__NetworkConnectionToClient(byte slotId, uint objectNetId, NetworkConnectionToClient sender)
		{
			uint value;
			if (objectNetId == 0)
			{
				if (sender != null)
				{
					TargetAttachRejected(sender, slotId, objectNetId);
				}
			}
			else if (SlotAttachments.TryGetValue(slotId, out value) && value != 0)
			{
				if (sender != null)
				{
					TargetAttachRejected(sender, slotId, objectNetId);
				}
			}
			else if (!_registeredSlots.ContainsKey(slotId))
			{
				if (sender != null)
				{
					TargetAttachRejected(sender, slotId, objectNetId);
				}
			}
			else
			{
				SlotAttachments[slotId] = objectNetId;
			}
		}

		protected static void InvokeUserCode_CmdAttachToSlot__Byte__UInt32__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdAttachToSlot called on client.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_CmdAttachToSlot__Byte__UInt32__NetworkConnectionToClient(NetworkReaderExtensions.ReadByte(reader), reader.ReadVarUInt(), senderConnection);
			}
		}

		protected void UserCode_TargetAttachRejected__NetworkConnectionToClient__Byte__UInt32(NetworkConnectionToClient target, byte slotId, uint objectNetId)
		{
			if (_registeredSlots.TryGetValue(slotId, out var value))
			{
				value.OnAttachRejected(objectNetId);
			}
		}

		protected static void InvokeUserCode_TargetAttachRejected__NetworkConnectionToClient__Byte__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetAttachRejected called on server.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_TargetAttachRejected__NetworkConnectionToClient__Byte__UInt32(null, NetworkReaderExtensions.ReadByte(reader), reader.ReadVarUInt());
			}
		}

		protected void UserCode_CmdDetachFromSlot__Byte(byte slotId)
		{
			SlotAttachments.TryGetValue(slotId, out var _);
			SlotAttachments[slotId] = 0u;
		}

		protected static void InvokeUserCode_CmdDetachFromSlot__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdDetachFromSlot called on client.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_CmdDetachFromSlot__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdSetInteractableState__Byte__Byte(byte interactableId, byte state)
		{
			InteractableStates[interactableId] = state;
		}

		protected static void InvokeUserCode_CmdSetInteractableState__Byte__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetInteractableState called on client.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_CmdSetInteractableState__Byte__Byte(NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdSetBraking__Boolean(bool value)
		{
			Network_isBraking = value;
		}

		protected static void InvokeUserCode_CmdSetBraking__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdSetBraking called on client.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_CmdSetBraking__Boolean(reader.ReadBool());
			}
		}

		protected void UserCode_CmdPushDashboardTelemetry__Single__Single__Single__String(float rpm, float speedKmh, float fuel, string gearName)
		{
			Network_engineRpm = rpm;
			Network_vehicleSpeedKmh = speedKmh;
			Network_fuelAmount = fuel;
			Network_gearName = gearName;
		}

		protected static void InvokeUserCode_CmdPushDashboardTelemetry__Single__Single__Single__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPushDashboardTelemetry called on client.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_CmdPushDashboardTelemetry__Single__Single__Single__String(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadString());
			}
		}

		protected void UserCode_CmdReportOdometerKm__Int32(int km)
		{
			if (km > _odometerKm)
			{
				Network_odometerKm = Mathf.Min(km, 999999);
			}
		}

		protected static void InvokeUserCode_CmdReportOdometerKm__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdReportOdometerKm called on client.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_CmdReportOdometerKm__Int32(reader.ReadVarInt());
			}
		}

		protected void UserCode_CmdReportCollision__Vector3__Vector3__Single__Vector3__Single__Byte__UInt32(Vector3 impactPoint, Vector3 impactNormal, float force, Vector3 relativeVelocity, float relativeSpeed, byte severity, uint collidedObjectNetId)
		{
			VehicleCollisionData data = new VehicleCollisionData
			{
				ImpactPoint = impactPoint,
				ImpactNormal = impactNormal,
				Force = force,
				RelativeVelocity = relativeVelocity,
				RelativeSpeed = relativeSpeed,
				Severity = (CollisionSeverity)severity,
				CollidedObjectNetId = collidedObjectNetId
			};
			_vehicleManager?.EventBus.FireCollisionServer(data);
			RpcBroadcastCollision(impactPoint, impactNormal, force, relativeVelocity, relativeSpeed, severity, collidedObjectNetId);
		}

		protected static void InvokeUserCode_CmdReportCollision__Vector3__Vector3__Single__Vector3__Single__Byte__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdReportCollision called on client.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_CmdReportCollision__Vector3__Vector3__Single__Vector3__Single__Byte__UInt32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadFloat(), reader.ReadVector3(), reader.ReadFloat(), NetworkReaderExtensions.ReadByte(reader), reader.ReadVarUInt());
			}
		}

		protected void UserCode_RpcBroadcastCollision__Vector3__Vector3__Single__Vector3__Single__Byte__UInt32(Vector3 impactPoint, Vector3 impactNormal, float force, Vector3 relativeVelocity, float relativeSpeed, byte severity, uint collidedObjectNetId)
		{
			if (!base.isOwned)
			{
				VehicleCollisionData data = new VehicleCollisionData
				{
					ImpactPoint = impactPoint,
					ImpactNormal = impactNormal,
					Force = force,
					RelativeVelocity = relativeVelocity,
					RelativeSpeed = relativeSpeed,
					Severity = (CollisionSeverity)severity,
					CollidedObjectNetId = collidedObjectNetId
				};
				_vehicleManager?.EventBus.FireCollisionAllClients(data);
			}
		}

		protected static void InvokeUserCode_RpcBroadcastCollision__Vector3__Vector3__Single__Vector3__Single__Byte__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcBroadcastCollision called on server.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_RpcBroadcastCollision__Vector3__Vector3__Single__Vector3__Single__Byte__UInt32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadFloat(), reader.ReadVector3(), reader.ReadFloat(), NetworkReaderExtensions.ReadByte(reader), reader.ReadVarUInt());
			}
		}

		protected void UserCode_RpcSettleDetachedPart__UInt32__Vector3__Quaternion(uint partNetId, Vector3 position, Quaternion rotation)
		{
			if (NetworkClient.spawned.TryGetValue(partNetId, out var value))
			{
				if (value.TryGetComponent<Rigidbody>(out var component))
				{
					component.isKinematic = true;
				}
				if (value.TryGetComponent<NetworkedTransform>(out var component2))
				{
					component2.ClientForceClearParent();
				}
				value.transform.SetPositionAndRotation(position, rotation);
			}
		}

		protected static void InvokeUserCode_RpcSettleDetachedPart__UInt32__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcSettleDetachedPart called on server.");
			}
			else
			{
				((VehicleNetworkSync)obj).UserCode_RpcSettleDetachedPart__UInt32__Vector3__Quaternion(reader.ReadVarUInt(), reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		static VehicleNetworkSync()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdAttachToSlot(System.Byte,System.UInt32,Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdAttachToSlot__Byte__UInt32__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdDetachFromSlot(System.Byte)", InvokeUserCode_CmdDetachFromSlot__Byte, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdSetInteractableState(System.Byte,System.Byte)", InvokeUserCode_CmdSetInteractableState__Byte__Byte, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdSetBraking(System.Boolean)", InvokeUserCode_CmdSetBraking__Boolean, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdPushDashboardTelemetry(System.Single,System.Single,System.Single,System.String)", InvokeUserCode_CmdPushDashboardTelemetry__Single__Single__Single__String, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdReportOdometerKm(System.Int32)", InvokeUserCode_CmdReportOdometerKm__Int32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::CmdReportCollision(UnityEngine.Vector3,UnityEngine.Vector3,System.Single,UnityEngine.Vector3,System.Single,System.Byte,System.UInt32)", InvokeUserCode_CmdReportCollision__Vector3__Vector3__Single__Vector3__Single__Byte__UInt32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::RpcBroadcastCollision(UnityEngine.Vector3,UnityEngine.Vector3,System.Single,UnityEngine.Vector3,System.Single,System.Byte,System.UInt32)", InvokeUserCode_RpcBroadcastCollision__Vector3__Vector3__Single__Vector3__Single__Byte__UInt32);
			RemoteProcedureCalls.RegisterRpc(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::RpcSettleDetachedPart(System.UInt32,UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcSettleDetachedPart__UInt32__Vector3__Quaternion);
			RemoteProcedureCalls.RegisterRpc(typeof(VehicleNetworkSync), "System.Void NomadDrive.Features.Vehicle.Networking.VehicleNetworkSync::TargetAttachRejected(Mirror.NetworkConnectionToClient,System.Byte,System.UInt32)", InvokeUserCode_TargetAttachRejected__NetworkConnectionToClient__Byte__UInt32);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_isBraking);
				writer.WriteFloat(_engineRpm);
				writer.WriteFloat(_vehicleSpeedKmh);
				writer.WriteFloat(_fuelAmount);
				writer.WriteString(_gearName);
				writer.WriteVarInt(_odometerKm);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(_isBraking);
			}
			if ((syncVarDirtyBits & 2L) != 0L)
			{
				writer.WriteFloat(_engineRpm);
			}
			if ((syncVarDirtyBits & 4L) != 0L)
			{
				writer.WriteFloat(_vehicleSpeedKmh);
			}
			if ((syncVarDirtyBits & 8L) != 0L)
			{
				writer.WriteFloat(_fuelAmount);
			}
			if ((syncVarDirtyBits & 0x10L) != 0L)
			{
				writer.WriteString(_gearName);
			}
			if ((syncVarDirtyBits & 0x20L) != 0L)
			{
				writer.WriteVarInt(_odometerKm);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _isBraking, _Mirror_SyncVarHookDelegate__isBraking, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _engineRpm, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _vehicleSpeedKmh, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _fuelAmount, null, reader.ReadFloat());
				GeneratedSyncVarDeserialize(ref _gearName, null, reader.ReadString());
				GeneratedSyncVarDeserialize(ref _odometerKm, _Mirror_SyncVarHookDelegate__odometerKm, reader.ReadVarInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isBraking, _Mirror_SyncVarHookDelegate__isBraking, reader.ReadBool());
			}
			if ((num & 2L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _engineRpm, null, reader.ReadFloat());
			}
			if ((num & 4L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _vehicleSpeedKmh, null, reader.ReadFloat());
			}
			if ((num & 8L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _fuelAmount, null, reader.ReadFloat());
			}
			if ((num & 0x10L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _gearName, null, reader.ReadString());
			}
			if ((num & 0x20L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _odometerKm, _Mirror_SyncVarHookDelegate__odometerKm, reader.ReadVarInt());
			}
		}
	}
}
