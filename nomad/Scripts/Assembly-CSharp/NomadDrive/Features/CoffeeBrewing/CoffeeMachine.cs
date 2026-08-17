using System;
using System.Runtime.InteropServices;
using Ami.BroAudio;
using Ami.BroAudio.Data;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.UI.Scripts;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Vehicle;
using PrimeTween;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.CoffeeBrewing
{
	[RequireComponent(typeof(SnappingPlanesManager))]
	public class CoffeeMachine : HeldItem
	{
		[SerializeField]
		private float brewingTime = 7.5f;

		[Header("Audio")]
		[Tooltip("AudioEntity with a Loop Region (intro + sustain + outro) played while the machine is brewing.")]
		[SerializeField]
		private SoundID brewingLoopSound;

		[Tooltip("Bool parameter on the brewing entity; set false on stop to drive the sustain -> outro transition.")]
		[SerializeField]
		private AudioParameter brewingParameter;

		private AudioHandle _brewingLoopInstance;

		private bool _brewingLoopPlaying;

		private InteractableToggle _toggle;

		private CoffeeRefillArea _refillArea;

		private SnappingPlanesManager _snappingPlanesManager;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		[Inject]
		private ILiquidTransferProcessorFactory _transferProcessorFactory;

		[SyncVar(hook = "OnBrewingStateChanged")]
		private byte _brewingState;

		public Action<byte, byte> _Mirror_SyncVarHookDelegate__brewingState;

		public BrewingState BrewingState
		{
			get
			{
				return (BrewingState)_brewingState;
			}
			private set
			{
				Network_brewingState = (byte)value;
			}
		}

		private SnappingPlane[] SnappingPlanes => _snappingPlanesManager.SnappingPlanes;

		public byte Network_brewingState
		{
			get
			{
				return _brewingState;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _brewingState, 512uL, _Mirror_SyncVarHookDelegate__brewingState);
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			InitializeComponents();
			SetupToggleListener();
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			if (_toggle != null)
			{
				_toggle.onToggleOn.RemoveListener(TryStartBrewing);
			}
			StopBrewingLoop();
		}

		private void InitializeComponents()
		{
			_refillArea = GetComponentInChildren<CoffeeRefillArea>();
			_toggle = GetComponentInChildren<InteractableToggle>();
			_snappingPlanesManager = GetComponent<SnappingPlanesManager>();
			if (_refillArea == null)
			{
				EvilLogger.LogError("CoffeeRefillArea component not found in children!", "InitializeComponents", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\CoffeeBrewing\\Scripts\\CoffeeMachine.cs", 78);
			}
			if (_toggle == null)
			{
				EvilLogger.LogError("InteractableToggle component not found in children!", "InitializeComponents", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\CoffeeBrewing\\Scripts\\CoffeeMachine.cs", 83);
			}
		}

		private void SetupToggleListener()
		{
			_toggle.onToggleOn.AddListener(TryStartBrewing);
		}

		private ILiquidContainer GetAttachedLiquidContainer()
		{
			SnappingPlane[] snappingPlanes = SnappingPlanes;
			foreach (SnappingPlane snappingPlane in snappingPlanes)
			{
				if (snappingPlane.IsAnyObjectPlaced && networkManager.TryGetNetworkObjectById(snappingPlane.SingleSlotPlacedEntityNetworkID, out var networkObject))
				{
					ILiquidContainer component = networkObject.GetComponent<ILiquidContainer>();
					if (component != null)
					{
						return component;
					}
				}
			}
			return null;
		}

		private bool ValidateBrewingConditions()
		{
			if (!IsLiquidContainerAttached())
			{
				ShowPlayerFeedbackMessage("@coffee.pot_missing");
				_toggle.ToggleOff();
				return false;
			}
			if (!HasCoffeeBeans())
			{
				ShowPlayerFeedbackMessage("@coffee.machine_empty");
				_toggle.ToggleOff();
				return false;
			}
			if (IsLiquidContainerFull())
			{
				ShowPlayerFeedbackMessage("@coffee.pot_full");
				_toggle.ToggleOff();
				return false;
			}
			return true;
		}

		private void ShowPlayerFeedbackMessage(string message)
		{
			_uiFeedbackManager.CreateFloatingMessage(message, FeedbackType.Warning);
		}

		private bool IsLiquidContainerAttached()
		{
			return GetAttachedLiquidContainer() != null;
		}

		private bool HasCoffeeBeans()
		{
			return _refillArea.CoffeeAmount > 0f;
		}

		private bool IsLiquidContainerFull()
		{
			return GetAttachedLiquidContainer()?.State.Equals(LiquidContainerState.Full) ?? false;
		}

		private void DisableInteractions()
		{
			DeactivateAllInteractions();
			SetInteractionAvailability(newValue: false);
			_toggle.DeactivateAllInteractions();
			_toggle.SetInteractionAvailability(newValue: false);
		}

		private void EnableInteractions()
		{
			ActivateAllInteractions();
			SetInteractionAvailability(newValue: true);
			_toggle.ActivateAllInteractions();
			_toggle.SetInteractionAvailability(newValue: true);
		}

		private void StartBrewingSequence()
		{
			Sequence.Create().ChainDelay(brewingTime).ChainCallback(OnBrewingSequenceComplete);
		}

		private void OnBrewingSequenceComplete()
		{
			if (base.isServer)
			{
				CmdCompleteBrewing();
			}
		}

		[Server]
		private void ServerProcessLiquidTransfer()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.CoffeeBrewing.CoffeeMachine::ServerProcessLiquidTransfer()' called when server was not active");
				return;
			}
			ILiquidContainer attachedLiquidContainer = GetAttachedLiquidContainer();
			if (attachedLiquidContainer == null)
			{
				EvilLogger.LogError("ServerProcessLiquidTransfer: Liquid container not found!", "ServerProcessLiquidTransfer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\CoffeeBrewing\\Scripts\\CoffeeMachine.cs", 191);
				return;
			}
			float num = CalculateTransferAmount(attachedLiquidContainer);
			_transferProcessorFactory.Create(attachedLiquidContainer, 0.5f).Start(num, LiquidType.Coffee);
			_refillArea.EmptyCoffee(num);
		}

		private float CalculateTransferAmount(ILiquidContainer liquidContainer)
		{
			float coffeeAmount = _refillArea.CoffeeAmount;
			float currentAmount = liquidContainer.CurrentAmount;
			float num = liquidContainer.Capacity - currentAmount;
			if (!(coffeeAmount > num))
			{
				return coffeeAmount;
			}
			return num;
		}

		private void OnBrewingStateChanged(byte oldValue, byte newValue)
		{
			if (!IsLateJoinCompleted)
			{
				return;
			}
			switch (newValue)
			{
			case 1:
				DisableInteractions();
				StartBrewingSequence();
				StartBrewingLoop();
				break;
			case 0:
				EnableInteractions();
				_toggle.ToggleOff();
				StopBrewingLoop();
				if (oldValue == 1)
				{
					ObjectivesEventBus.Raise(ObjectiveSignal.CoffeeBrewed, this);
				}
				break;
			}
		}

		private void StartBrewingLoop(bool skipIntro = false)
		{
			if (AudioManager != null && brewingLoopSound.IsValid())
			{
				if (_brewingLoopPlaying && !_brewingLoopInstance.IsValid)
				{
					_brewingLoopPlaying = false;
				}
				if (!_brewingLoopPlaying)
				{
					_brewingLoopInstance = AudioManager.PlayLoopRegion(brewingLoopSound, base.gameObject, skipIntro);
					AudioManager.SetParameter(_brewingLoopInstance, brewingParameter, value: true);
					_brewingLoopPlaying = true;
				}
			}
		}

		private void StopBrewingLoop()
		{
			if (_brewingLoopPlaying)
			{
				AudioManager?.StopLoopRegion(_brewingLoopInstance);
				_brewingLoopPlaying = false;
			}
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			if (BrewingState == BrewingState.Brewing)
			{
				StartBrewingLoop(skipIntro: true);
			}
		}

		private void TryStartBrewing()
		{
			if (ValidateBrewingConditions())
			{
				CmdStartBrewing();
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdStartBrewing()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.CoffeeBrewing.CoffeeMachine::CmdStartBrewing()", -340331162, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdCompleteBrewing()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void NomadDrive.Features.CoffeeBrewing.CoffeeMachine::CmdCompleteBrewing()", -592801019, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public CoffeeMachine()
		{
			_Mirror_SyncVarHookDelegate__brewingState = OnBrewingStateChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdStartBrewing()
		{
			ServerProcessLiquidTransfer();
			BrewingState = BrewingState.Brewing;
		}

		protected static void InvokeUserCode_CmdStartBrewing(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdStartBrewing called on client.");
			}
			else
			{
				((CoffeeMachine)obj).UserCode_CmdStartBrewing();
			}
		}

		protected void UserCode_CmdCompleteBrewing()
		{
			BrewingState = BrewingState.Idle;
		}

		protected static void InvokeUserCode_CmdCompleteBrewing(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdCompleteBrewing called on client.");
			}
			else
			{
				((CoffeeMachine)obj).UserCode_CmdCompleteBrewing();
			}
		}

		static CoffeeMachine()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(CoffeeMachine), "System.Void NomadDrive.Features.CoffeeBrewing.CoffeeMachine::CmdStartBrewing()", InvokeUserCode_CmdStartBrewing, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(CoffeeMachine), "System.Void NomadDrive.Features.CoffeeBrewing.CoffeeMachine::CmdCompleteBrewing()", InvokeUserCode_CmdCompleteBrewing, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _brewingState);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _brewingState);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _brewingState, _Mirror_SyncVarHookDelegate__brewingState, NetworkReaderExtensions.ReadByte(reader));
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _brewingState, _Mirror_SyncVarHookDelegate__brewingState, NetworkReaderExtensions.ReadByte(reader));
			}
		}
	}
}
