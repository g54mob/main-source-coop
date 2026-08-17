using System.Runtime.InteropServices;
using EvilCore.Localization;
using EvilCore.Networking.Parenting;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Furnitures;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Tools;
using NomadDrive.Features.Vehicle;
using NomadDrive.Features.WorldGeneration.ObjectSpawning;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player.Downed.Chicken
{
	public class ChickenForm : HeldItem
	{
		[Header("Chicken Form")]
		[SerializeField]
		private float reviveHoldDuration = 3f;

		[SyncVar]
		private uint _controllerNetId;

		[SyncVar]
		private SittableSurface _seat;

		[Inject]
		private ILocalizationService _localizationService;

		private InteractionStateMachine<ChickenFormState> _chickenStateMachine;

		private NetworkedTransform _networkedTransform;

		protected NetworkBehaviourSyncVar ____seatNetId;

		public uint ControllerNetId => _controllerNetId;

		public Transform RidingVehicleTransform
		{
			get
			{
				if (Network_seat != null && Network_seat.TryGetMountVehicle(out var vehicle) && vehicle != null)
				{
					return vehicle.transform;
				}
				if (_networkedTransform != null && _networkedTransform.HasParent && _networkedTransform.ParentTransform != null)
				{
					VehicleManager componentInParent = _networkedTransform.ParentTransform.GetComponentInParent<VehicleManager>();
					if (componentInParent != null)
					{
						return componentInParent.transform;
					}
				}
				return null;
			}
		}

		protected override bool UseDefaultStateMachine => false;

		protected override bool UseStateMachine => true;

		public uint Network_controllerNetId
		{
			get
			{
				return _controllerNetId;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _controllerNetId, 512uL, null);
			}
		}

		public SittableSurface Network_seat
		{
			get
			{
				return GetSyncVarNetworkBehaviour(____seatNetId, ref _seat);
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter_NetworkBehaviour(value, ref _seat, 1024uL, null, ref ____seatNetId);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			TryGetComponent<NetworkedTransform>(out _networkedTransform);
			if (TryGetComponent<Rigidbody>(out var component))
			{
				component.interpolation = RigidbodyInterpolation.None;
			}
			if (!TryGetComponent<LootVisibilityRange>(out var component2))
			{
				component2 = base.gameObject.AddComponent<LootVisibilityRange>();
			}
			component2.SetVisRange(1000000f);
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			if (playerService != null && playerService.LocalPlayer != null && playerService.LocalPlayer.netId == _controllerNetId && playerService.LocalPlayer.TryGetComponent<PlayerDeathController>(out var component))
			{
				component.ReleaseFollowCameraParent();
				component.ReleaseTerrainObserver();
			}
		}

		public override void OnStopServer()
		{
			base.OnStopServer();
			ServerLeaveSeat();
		}

		[Server]
		public void ServerInitController(uint controllerNetId)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.Chicken.ChickenForm::ServerInitController(System.UInt32)' called when server was not active");
			}
			else
			{
				Network_controllerNetId = controllerNetId;
			}
		}

		[Server]
		public void ServerInitSeat(SittableSurface seat)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.Chicken.ChickenForm::ServerInitSeat(NomadDrive.Features.Furnitures.SittableSurface)' called when server was not active");
			}
			else
			{
				Network_seat = seat;
			}
		}

		public override void OnEquip()
		{
			base.OnEquip();
			CmdLeaveSeat();
		}

		[Command(requiresAuthority = false)]
		private void CmdLeaveSeat()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.Player.Downed.Chicken.ChickenForm::CmdLeaveSeat()", -2082685405, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Server]
		private void ServerLeaveSeat()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Player.Downed.Chicken.ChickenForm::ServerLeaveSeat()' called when server was not active");
				return;
			}
			if (Network_seat != null)
			{
				Network_seat.ServerVacate();
			}
			Network_seat = null;
		}

		protected override void InitializeStateMachine()
		{
			_chickenStateMachine = new InteractionStateMachine<ChickenFormState>(this);
			base.BaseStateMachine = _chickenStateMachine;
			ConfigureStates();
			_chickenStateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			string pickUpLabel = ((_localizationService != null) ? _localizationService.Localize("@interaction.carry_body") : "Carry");
			string reviveLabel = ((_localizationService != null) ? _localizationService.Localize("@interaction.revive") : "Revive");
			_chickenStateMachine.RegisterState(ChickenFormState.None, (InteractionStateConfig c) => c.WithCrosshair(CrosshairType.Default).WithNameLabelVisibility(visible: false).WithInteractionLabelVisibility(visible: false)).RegisterState(ChickenFormState.PickUp, (InteractionStateConfig c) => c.WithBasicInteraction(InteractionKey.Primary, pickUpLabel, HandleEquip).WithHoldInteraction(InteractionKey.Secondary, reviveLabel, HandleHandRevive, reviveHoldDuration).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true)).RegisterState(ChickenFormState.Revive, (InteractionStateConfig c) => c.WithHoldInteraction(InteractionKey.Secondary, reviveLabel, HandleReviverRevive, reviveHoldDuration).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: true));
		}

		public override void UpdateState()
		{
			_chickenStateMachine?.TransitionTo(DetermineState());
		}

		private ChickenFormState DetermineState()
		{
			if (base.IsEquipped)
			{
				return ChickenFormState.None;
			}
			IEquipmentManager equipmentManager = playerService?.EquipmentManager;
			if (equipmentManager == null)
			{
				return ChickenFormState.None;
			}
			if (equipmentManager.EquippedEntity != null && equipmentManager.EquippedEntity.TryGetComponent<Reviver>(out var _))
			{
				return ChickenFormState.Revive;
			}
			if (equipmentManager.IsItemEquipped || !(playerService.LocalPlayer != null) || playerService.LocalPlayer.IsDowned || IsLocalPlayerSitting())
			{
				return ChickenFormState.None;
			}
			return ChickenFormState.PickUp;
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			UpdateState();
		}

		private void HandleReviverRevive()
		{
			IEquipmentManager equipmentManager = playerService?.EquipmentManager;
			if (!(equipmentManager?.EquippedEntity == null) && equipmentManager.EquippedEntity.TryGetComponent<Reviver>(out var component))
			{
				uint consumedReviverNetId = component.netId;
				equipmentManager.Consume();
				CmdReviveController(consumedReviverNetId, fullHealth: true);
				ObjectivesEventBus.Raise(ObjectiveSignal.TeammateRevived, this);
			}
		}

		private void HandleHandRevive()
		{
			CmdReviveController(0u, fullHealth: false);
			ObjectivesEventBus.Raise(ObjectiveSignal.TeammateRevived, this);
		}

		[Command(requiresAuthority = false)]
		private void CmdReviveController(uint consumedReviverNetId, bool fullHealth)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarUInt(consumedReviverNetId);
			writer.WriteBool(fullHealth);
			SendCommandInternal("System.Void NomadDrive.Features.Player.Downed.Chicken.ChickenForm::CmdReviveController(System.UInt32,System.Boolean)", 174922573, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdLeaveSeat()
		{
			ServerLeaveSeat();
		}

		protected static void InvokeUserCode_CmdLeaveSeat(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdLeaveSeat called on client.");
			}
			else
			{
				((ChickenForm)obj).UserCode_CmdLeaveSeat();
			}
		}

		protected void UserCode_CmdReviveController__UInt32__Boolean(uint consumedReviverNetId, bool fullHealth)
		{
			if (consumedReviverNetId != 0 && NetworkServer.spawned.TryGetValue(consumedReviverNetId, out var value) && value != null)
			{
				NetworkServer.Destroy(value.gameObject);
			}
			SittableSurface network_seat = Network_seat;
			bool flag = false;
			if (NetworkServer.spawned.TryGetValue(_controllerNetId, out var value2) && value2 != null && value2.TryGetComponent<PlayerDeathController>(out var component))
			{
				component.ServerReviveFromChicken(base.transform.position, base.transform.rotation, network_seat, fullHealth);
				flag = true;
			}
			if (flag && network_seat != null)
			{
				Network_seat = null;
			}
			else
			{
				ServerLeaveSeat();
			}
			NetworkServer.Destroy(base.gameObject);
		}

		protected static void InvokeUserCode_CmdReviveController__UInt32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdReviveController called on client.");
			}
			else
			{
				((ChickenForm)obj).UserCode_CmdReviveController__UInt32__Boolean(reader.ReadVarUInt(), reader.ReadBool());
			}
		}

		static ChickenForm()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(ChickenForm), "System.Void NomadDrive.Features.Player.Downed.Chicken.ChickenForm::CmdLeaveSeat()", InvokeUserCode_CmdLeaveSeat, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(ChickenForm), "System.Void NomadDrive.Features.Player.Downed.Chicken.ChickenForm::CmdReviveController(System.UInt32,System.Boolean)", InvokeUserCode_CmdReviveController__UInt32__Boolean, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarUInt(_controllerNetId);
				writer.WriteNetworkBehaviour(Network_seat);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteVarUInt(_controllerNetId);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteNetworkBehaviour(Network_seat);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _controllerNetId, null, reader.ReadVarUInt());
				GeneratedSyncVarDeserialize_NetworkBehaviour(ref _seat, null, reader, ref ____seatNetId);
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _controllerNetId, null, reader.ReadVarUInt());
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize_NetworkBehaviour(ref _seat, null, reader, ref ____seatNetId);
			}
		}
	}
}
