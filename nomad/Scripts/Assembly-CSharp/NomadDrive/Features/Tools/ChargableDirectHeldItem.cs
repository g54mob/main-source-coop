using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.SaveSystem;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.Tools
{
	public abstract class ChargableDirectHeldItem : DirectHeldItem, INetworkSaveable
	{
		[Header("Battery")]
		[SerializeField]
		private float maxCharge = 300f;

		[FormerlySerializedAs("consumptionRate")]
		[SerializeField]
		private float batteryConsumptionSpeed = 1f;

		[SerializeField]
		private bool spawnWithFullCharge;

		[SerializeField]
		private SoundID batteryInsertSound;

		[SyncVar(hook = "OnHasBatteryChanged")]
		private bool _hasBattery;

		[SyncVar(hook = "OnCurrentChargeChanged")]
		private float _currentCharge;

		[Inject]
		private EquippedItemBatteryInfoPanel _batteryInfoPanel;

		[Inject]
		private IGameUIManager _guiManager;

		private InteractionStateMachine<ChargableItemState> _chargableStateMachine;

		private ConsumableBattery _hoveredBattery;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate__hasBattery;

		public Action<float, float> _Mirror_SyncVarHookDelegate__currentCharge;

		public bool HasCharge => _currentCharge > 0f;

		public float CurrentCharge => _currentCharge;

		public float ChargeRatio
		{
			get
			{
				if (!(maxCharge > 0f))
				{
					return 0f;
				}
				return Mathf.Clamp01(_currentCharge / maxCharge);
			}
		}

		public bool HasBattery => _hasBattery;

		public bool CanInsertBattery => !_hasBattery;

		protected abstract bool IsActivelyConsuming { get; }

		protected override bool UseStateMachine => true;

		protected override bool UseDefaultStateMachine => false;

		public string ContributorKey => "charge";

		public bool Network_hasBattery
		{
			get
			{
				return _hasBattery;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _hasBattery, 512uL, _Mirror_SyncVarHookDelegate__hasBattery);
			}
		}

		public float Network_currentCharge
		{
			get
			{
				return _currentCharge;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _currentCharge, 1024uL, _Mirror_SyncVarHookDelegate__currentCharge);
			}
		}

		protected virtual void OnBatteryDepleted()
		{
		}

		protected override void InitializeStateMachine()
		{
			_chargableStateMachine = new InteractionStateMachine<ChargableItemState>(this);
			base.BaseStateMachine = _chargableStateMachine;
			ConfigureStates();
			_chargableStateMachine.Initialize(DetermineChargableState());
		}

		protected override void ConfigureStates()
		{
			_chargableStateMachine.RegisterState(ChargableItemState.Idle, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(ChargableItemState.IdleWithBattery, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithHoldInteraction(InteractionKey.Secondary, "@interaction.insert_battery", HandleInsertBattery, 0.5f)
				.WithCrosshair(CrosshairType.Interact)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(ChargableItemState.Equipped, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		protected ChargableItemState DetermineChargableState()
		{
			if (base.IsEquipped)
			{
				return ChargableItemState.Equipped;
			}
			if (HasCompatibleBattery())
			{
				return ChargableItemState.IdleWithBattery;
			}
			return ChargableItemState.Idle;
		}

		public override void UpdateState()
		{
			_chargableStateMachine?.TransitionTo(DetermineChargableState());
		}

		protected bool HasCompatibleBattery()
		{
			if (GetEquippedBattery() != null)
			{
				return CanInsertBattery;
			}
			return false;
		}

		protected ConsumableBattery GetEquippedBattery()
		{
			if (playerService?.EquipmentManager?.EquippedEntity == null)
			{
				return null;
			}
			if (!playerService.EquipmentManager.EquippedEntity.TryGetComponent<ConsumableBattery>(out var component))
			{
				return null;
			}
			return component;
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			ConsumableBattery equippedBattery = GetEquippedBattery();
			if (equippedBattery != null && CanInsertBattery)
			{
				_hoveredBattery = equippedBattery;
			}
			_batteryInfoPanel?.SetItem(interactableName, ChargeRatio);
			_guiManager?.ShowCanvasGroup(GameCanvasGroupName.EquippedItemBatteryInfo, interactable: false, blockRaycast: false);
			_batteryInfoPanel?.SetWorldTarget(base.gameObject);
			UpdateState();
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			_hoveredBattery = null;
			if (!base.IsEquipped)
			{
				_batteryInfoPanel?.ClearItem();
				_guiManager?.HideCanvasGroup(GameCanvasGroupName.EquippedItemBatteryInfo);
				_batteryInfoPanel?.ClearWorldTarget();
			}
		}

		protected virtual void HandleInsertBattery()
		{
			if (!(_hoveredBattery == null))
			{
				playerService.EquipmentManager.Consume();
				CmdInsertBattery(_hoveredBattery.netId);
				UpdateState();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdInsertBattery(uint batteryNetId)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(batteryNetId);
			SendCommandInternal("System.Void NomadDrive.Features.Tools.ChargableDirectHeldItem::CmdInsertBattery(System.UInt32)", -1777448982, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcPlayBatteryInsertSound()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void NomadDrive.Features.Tools.ChargableDirectHeldItem::RpcPlayBatteryInsertSound()", -170810814, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (spawnWithFullCharge)
			{
				Network_currentCharge = maxCharge;
				Network_hasBattery = true;
			}
		}

		protected virtual void Update()
		{
			if (base.isServer && IsActivelyConsuming && !(_currentCharge <= 0f))
			{
				Network_currentCharge = _currentCharge - batteryConsumptionSpeed * Time.deltaTime;
				if (_currentCharge <= 0f)
				{
					Network_currentCharge = 0f;
					Network_hasBattery = false;
					OnBatteryDepleted();
				}
			}
		}

		private void OnHasBatteryChanged(bool oldValue, bool newValue)
		{
			if (IsLateJoinCompleted)
			{
				UpdateState();
			}
		}

		private void OnCurrentChargeChanged(float oldValue, float newValue)
		{
			if (IsLateJoinCompleted)
			{
				_batteryInfoPanel?.UpdateCharge(ChargeRatio);
			}
		}

		public override void OnEquip()
		{
			base.OnEquip();
			_batteryInfoPanel?.SetItem(interactableName, ChargeRatio);
			_guiManager?.ShowCanvasGroup(GameCanvasGroupName.EquippedItemBatteryInfo, interactable: false, blockRaycast: false);
			_batteryInfoPanel?.ClearWorldTarget();
		}

		public override void OnUnequip()
		{
			base.OnUnequip();
			_batteryInfoPanel?.ClearItem();
			_guiManager?.HideCanvasGroup(GameCanvasGroupName.EquippedItemBatteryInfo);
			_batteryInfoPanel?.ClearWorldTarget();
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			UpdateState();
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_currentCharge);
			writer.Write(_hasBattery);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			float network_currentCharge = reader.ReadFloat();
			bool network_hasBattery = reader.ReadBool();
			if (NetworkServer.active)
			{
				Network_currentCharge = network_currentCharge;
				Network_hasBattery = network_hasBattery;
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		protected ChargableDirectHeldItem()
		{
			_Mirror_SyncVarHookDelegate__hasBattery = OnHasBatteryChanged;
			_Mirror_SyncVarHookDelegate__currentCharge = OnCurrentChargeChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdInsertBattery__UInt32(uint batteryNetId)
		{
			ConsumableBattery component;
			if (!networkManager.TryGetNetworkObjectById(batteryNetId, out var networkObject))
			{
				EvilLogger.LogError($"[ChargableDirectHeldItem] Battery with netId {batteryNetId} not found", "CmdInsertBattery", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\Interactables\\ChargableDirectHeldItem.cs", 164);
			}
			else if (networkObject.TryGetComponent<ConsumableBattery>(out component) && !_hasBattery)
			{
				Network_hasBattery = true;
				Network_currentCharge = Mathf.Min(_currentCharge + component.CurrentCharge, maxCharge);
				component.Destroy();
				RpcPlayBatteryInsertSound();
			}
		}

		protected static void InvokeUserCode_CmdInsertBattery__UInt32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdInsertBattery called on client.");
			}
			else
			{
				((ChargableDirectHeldItem)obj).UserCode_CmdInsertBattery__UInt32(reader.ReadVarUInt());
			}
		}

		protected void UserCode_RpcPlayBatteryInsertSound()
		{
			AudioManager?.PlayOneShotAttached(batteryInsertSound, base.gameObject);
		}

		protected static void InvokeUserCode_RpcPlayBatteryInsertSound(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPlayBatteryInsertSound called on server.");
			}
			else
			{
				((ChargableDirectHeldItem)obj).UserCode_RpcPlayBatteryInsertSound();
			}
		}

		static ChargableDirectHeldItem()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(ChargableDirectHeldItem), "System.Void NomadDrive.Features.Tools.ChargableDirectHeldItem::CmdInsertBattery(System.UInt32)", InvokeUserCode_CmdInsertBattery__UInt32, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(ChargableDirectHeldItem), "System.Void NomadDrive.Features.Tools.ChargableDirectHeldItem::RpcPlayBatteryInsertSound()", InvokeUserCode_RpcPlayBatteryInsertSound);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_hasBattery);
				writer.WriteFloat(_currentCharge);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteBool(_hasBattery);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteFloat(_currentCharge);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _hasBattery, _Mirror_SyncVarHookDelegate__hasBattery, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _currentCharge, _Mirror_SyncVarHookDelegate__currentCharge, reader.ReadFloat());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _hasBattery, _Mirror_SyncVarHookDelegate__hasBattery, reader.ReadBool());
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _currentCharge, _Mirror_SyncVarHookDelegate__currentCharge, reader.ReadFloat());
			}
		}
	}
}
