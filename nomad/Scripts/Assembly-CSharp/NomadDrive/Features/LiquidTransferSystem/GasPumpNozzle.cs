using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Interaction;
using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class GasPumpNozzle : HeldItemLiquidContainer
	{
		[Header("Pump Nozzle Settings")]
		[SerializeField]
		private float maxTetherDistance = 8f;

		[SerializeField]
		private Transform ropeStartPoint;

		[SerializeField]
		private SoundID equipSound;

		[SyncVar]
		private Vector3 _pumpRestPosition;

		[SyncVar]
		private Quaternion _pumpRestRotation;

		private static readonly List<GasPumpNozzle> ActiveNozzles;

		private bool _registeredInRegistry;

		private InteractionStateMachine<GasPumpNozzleState> _nozzleStateMachine;

		private bool _isReturning;

		private GasPumpBody _pumpBody;

		private LiquidContainerSnapHandler _snapHandler;

		public static IReadOnlyList<GasPumpNozzle> Registry => ActiveNozzles;

		public Transform RopeStartPoint => ropeStartPoint;

		public Vector3 PumpRestPosition => FloatingOriginManager.ToRenderWorld(_pumpRestPosition);

		protected override bool UseDefaultStateMachine => false;

		protected override bool UseStateMachine => true;

		public Vector3 Network_pumpRestPosition
		{
			get
			{
				return _pumpRestPosition;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _pumpRestPosition, 512uL, null);
			}
		}

		public Quaternion Network_pumpRestRotation
		{
			get
			{
				return _pumpRestRotation;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _pumpRestRotation, 1024uL, null);
			}
		}

		public static event Action<GasPumpNozzle> OnNozzleRegistered;

		protected override void Awake()
		{
			base.Awake();
			_snapHandler = GetComponent<LiquidContainerSnapHandler>();
		}

		protected override void InitializeStateMachine()
		{
			_nozzleStateMachine = new InteractionStateMachine<GasPumpNozzleState>(this);
			base.BaseStateMachine = _nozzleStateMachine;
			ConfigureStates();
			_nozzleStateMachine.Initialize(DetermineNozzleState());
		}

		protected override void ConfigureStates()
		{
			_nozzleStateMachine.RegisterState(GasPumpNozzleState.Idle, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(GasPumpNozzleState.Equipped, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		private GasPumpNozzleState DetermineNozzleState()
		{
			if (!base.IsEquipped)
			{
				return GasPumpNozzleState.Idle;
			}
			return GasPumpNozzleState.Equipped;
		}

		public override void UpdateState()
		{
			_nozzleStateMachine?.TransitionTo(DetermineNozzleState());
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			Network_pumpRestPosition = FloatingOriginManager.ToTrueWorld(base.transform.position);
			Network_pumpRestRotation = base.transform.rotation;
			base.LiquidContainer.SetLiquidType(LiquidType.Gasoline);
			base.LiquidContainer.SetAmount(base.LiquidContainer.Capacity);
			RegisterInRegistry();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			RegisterInRegistry();
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			UnregisterFromRegistry();
		}

		public override void OnStopServer()
		{
			base.OnStopServer();
			UnregisterFromRegistry();
		}

		private void RegisterInRegistry()
		{
			if (!_registeredInRegistry)
			{
				_registeredInRegistry = true;
				ActiveNozzles.Add(this);
				GasPumpNozzle.OnNozzleRegistered?.Invoke(this);
			}
		}

		private void UnregisterFromRegistry()
		{
			if (_registeredInRegistry)
			{
				_registeredInRegistry = false;
				ActiveNozzles.Remove(this);
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			CmdRequestLiquidSync();
			UpdateState();
		}

		[Command(requiresAuthority = false)]
		private void CmdRequestLiquidSync(NetworkConnectionToClient sender = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.GasPumpNozzle::CmdRequestLiquidSync(Mirror.NetworkConnectionToClient)", 1964912681, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[TargetRpc]
		private void TargetSyncLiquid(NetworkConnectionToClient _, float amount, LiquidType type)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteFloat(amount);
			GeneratedNetworkCode._Write_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(writer, type);
			SendTargetRPCInternal(_, "System.Void NomadDrive.Features.LiquidTransferSystem.GasPumpNozzle::TargetSyncLiquid(Mirror.NetworkConnectionToClient,System.Single,NomadDrive.Features.LiquidTransferSystem.LiquidType)", 1789257742, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public void SetPumpBody(GasPumpBody pumpBody)
		{
			_pumpBody = pumpBody;
		}

		private void Update()
		{
			if (base.isOwned && base.IsEquipped && (!(_snapHandler != null) || !_snapHandler.IsSnapped) && !(_pumpRestPosition == Vector3.zero) && Vector3.Distance(base.transform.position, FloatingOriginManager.ToRenderWorld(_pumpRestPosition)) > maxTetherDistance && playerService.EquipmentManager.EquippedEntity == this)
			{
				playerService.EquipmentManager.Unequip(this);
			}
		}

		public override void OnEquip()
		{
			base.OnEquip();
			if (equipSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShotAttached(equipSound, base.netIdentity);
			}
		}

		public override void OnUnequip()
		{
			base.OnUnequip();
			ScheduleReturnToPump().Forget();
		}

		public override void StartPlacing()
		{
			playerService.EquipmentManager.Unequip(this);
		}

		private async UniTaskVoid ScheduleReturnToPump()
		{
			if (!_isReturning)
			{
				_isReturning = true;
				await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
				CmdReturnToPump();
				_isReturning = false;
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdReturnToPump()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.LiquidTransferSystem.GasPumpNozzle::CmdReturnToPump()", -612864529, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcReturnToPump(Vector3 truePosition, Quaternion rotation)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(truePosition);
			writer.WriteQuaternion(rotation);
			SendRPCInternal("System.Void NomadDrive.Features.LiquidTransferSystem.GasPumpNozzle::RpcReturnToPump(UnityEngine.Vector3,UnityEngine.Quaternion)", -773688412, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		static GasPumpNozzle()
		{
			ActiveNozzles = new List<GasPumpNozzle>();
			RemoteProcedureCalls.RegisterCommand(typeof(GasPumpNozzle), "System.Void NomadDrive.Features.LiquidTransferSystem.GasPumpNozzle::CmdRequestLiquidSync(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdRequestLiquidSync__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(GasPumpNozzle), "System.Void NomadDrive.Features.LiquidTransferSystem.GasPumpNozzle::CmdReturnToPump()", InvokeUserCode_CmdReturnToPump, requiresAuthority: false);
			RemoteProcedureCalls.RegisterRpc(typeof(GasPumpNozzle), "System.Void NomadDrive.Features.LiquidTransferSystem.GasPumpNozzle::RpcReturnToPump(UnityEngine.Vector3,UnityEngine.Quaternion)", InvokeUserCode_RpcReturnToPump__Vector3__Quaternion);
			RemoteProcedureCalls.RegisterRpc(typeof(GasPumpNozzle), "System.Void NomadDrive.Features.LiquidTransferSystem.GasPumpNozzle::TargetSyncLiquid(Mirror.NetworkConnectionToClient,System.Single,NomadDrive.Features.LiquidTransferSystem.LiquidType)", InvokeUserCode_TargetSyncLiquid__NetworkConnectionToClient__Single__LiquidType);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRequestLiquidSync__NetworkConnectionToClient(NetworkConnectionToClient sender)
		{
			if (sender != null)
			{
				TargetSyncLiquid(sender, base.LiquidContainer.CurrentAmount, base.LiquidContainer.CurrentLiquidType);
			}
		}

		protected static void InvokeUserCode_CmdRequestLiquidSync__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestLiquidSync called on client.");
			}
			else
			{
				((GasPumpNozzle)obj).UserCode_CmdRequestLiquidSync__NetworkConnectionToClient(senderConnection);
			}
		}

		protected void UserCode_TargetSyncLiquid__NetworkConnectionToClient__Single__LiquidType(NetworkConnectionToClient _, float amount, LiquidType type)
		{
			base.LiquidContainer.ApplyLateJoinerState(amount, type);
			UpdateState();
		}

		protected static void InvokeUserCode_TargetSyncLiquid__NetworkConnectionToClient__Single__LiquidType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("TargetRPC TargetSyncLiquid called on server.");
			}
			else
			{
				((GasPumpNozzle)obj).UserCode_TargetSyncLiquid__NetworkConnectionToClient__Single__LiquidType(null, reader.ReadFloat(), GeneratedNetworkCode._Read_NomadDrive_002EFeatures_002ELiquidTransferSystem_002ELiquidType(reader));
			}
		}

		protected void UserCode_CmdReturnToPump()
		{
			base.transform.SetPositionAndRotation(FloatingOriginManager.ToRenderWorld(_pumpRestPosition), _pumpRestRotation);
			SetInteractionAvailability(newValue: true);
			SetRigidCollidersTriggered(newValue: false);
			if (base.netIdentity.connectionToClient != null)
			{
				base.netIdentity.RemoveClientAuthority();
			}
			if (NetworkServer.localConnection != null)
			{
				base.netIdentity.AssignClientAuthority(NetworkServer.localConnection);
			}
			RpcReturnToPump(_pumpRestPosition, _pumpRestRotation);
		}

		protected static void InvokeUserCode_CmdReturnToPump(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdReturnToPump called on client.");
			}
			else
			{
				((GasPumpNozzle)obj).UserCode_CmdReturnToPump();
			}
		}

		protected void UserCode_RpcReturnToPump__Vector3__Quaternion(Vector3 truePosition, Quaternion rotation)
		{
			base.transform.SetPositionAndRotation(FloatingOriginManager.ToRenderWorld(truePosition), rotation);
			UpdateState();
		}

		protected static void InvokeUserCode_RpcReturnToPump__Vector3__Quaternion(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcReturnToPump called on server.");
			}
			else
			{
				((GasPumpNozzle)obj).UserCode_RpcReturnToPump__Vector3__Quaternion(reader.ReadVector3(), reader.ReadQuaternion());
			}
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVector3(_pumpRestPosition);
				writer.WriteQuaternion(_pumpRestRotation);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				writer.WriteVector3(_pumpRestPosition);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteQuaternion(_pumpRestRotation);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _pumpRestPosition, null, reader.ReadVector3());
				GeneratedSyncVarDeserialize(ref _pumpRestRotation, null, reader.ReadQuaternion());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _pumpRestPosition, null, reader.ReadVector3());
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _pumpRestRotation, null, reader.ReadQuaternion());
			}
		}
	}
}
