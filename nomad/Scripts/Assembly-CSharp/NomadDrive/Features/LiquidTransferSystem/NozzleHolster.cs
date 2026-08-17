using Ami.BroAudio;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class NozzleHolster : StaticInteractable
	{
		[SerializeField]
		private SoundID placeSound;

		[Inject]
		private IPlayerService _playerService;

		private const float RestMatchSqrTolerance = 0.25f;

		private StaticInteractionStateMachine<NozzleHolsterState> _stateMachine;

		private GasPumpBody _pumpBody;

		private IEquipmentManager _equipmentManager;

		protected override bool UseStateMachine => true;

		protected override void Awake()
		{
			base.Awake();
			_pumpBody = GetComponentInParent<GasPumpBody>();
		}

		protected override void InitializeStateMachine()
		{
			_stateMachine = new StaticInteractionStateMachine<NozzleHolsterState>(this);
			base.BaseStateMachine = _stateMachine;
			ConfigureStates();
			_stateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_stateMachine.RegisterState(NozzleHolsterState.Empty, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default).WithNameLabelVisibility(visible: false).WithInteractionLabelVisibility(visible: false)).RegisterState(NozzleHolsterState.CanPlace, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.place", PlaceNozzle).WithCrosshair(CrosshairType.Interact).WithNameLabelVisibility(visible: false)
				.WithInteractionLabelVisibility(visible: true));
		}

		protected override void Start()
		{
			base.Start();
			IgnoreHovering();
			if (_playerService != null)
			{
				if (_playerService.IsPlayerSpawned)
				{
					SubscribeToEquipment();
				}
				else
				{
					_playerService.OnPlayerRegistered += SubscribeToEquipment;
				}
			}
		}

		protected override void OnDestroy()
		{
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= SubscribeToEquipment;
			}
			if (_equipmentManager != null)
			{
				_equipmentManager.OnItemEquipped.RemoveListener(RefreshActivation);
				_equipmentManager.OnItemUnequipped.RemoveListener(RefreshActivation);
			}
			base.OnDestroy();
		}

		private void SubscribeToEquipment()
		{
			_equipmentManager = _playerService.EquipmentManager;
			if (_equipmentManager != null)
			{
				_equipmentManager.OnItemEquipped.AddListener(RefreshActivation);
				_equipmentManager.OnItemUnequipped.AddListener(RefreshActivation);
				RefreshActivation();
			}
		}

		private void RefreshActivation()
		{
			if (HoldsThisPumpsNozzle())
			{
				UnignoreHovering();
			}
			else
			{
				IgnoreHovering();
			}
			UpdateState();
		}

		private bool HoldsThisPumpsNozzle()
		{
			if (_equipmentManager?.EquippedEntity is GasPumpNozzle gasPumpNozzle)
			{
				Vector3 vector = ((_pumpBody != null) ? _pumpBody.NozzleRestPoint.position : base.transform.position);
				return (gasPumpNozzle.PumpRestPosition - vector).sqrMagnitude < 0.25f;
			}
			return false;
		}

		private NozzleHolsterState DetermineState()
		{
			if (!HoldsThisPumpsNozzle())
			{
				return NozzleHolsterState.Empty;
			}
			return NozzleHolsterState.CanPlace;
		}

		public override void UpdateState()
		{
			_stateMachine?.TransitionTo(DetermineState());
		}

		public override void OnHovered()
		{
			base.OnHovered();
			UpdateState();
		}

		public override void OnUnhovered()
		{
			base.OnUnhovered();
			UpdateState();
		}

		public override void ApplyStateFromManager(byte stateData, bool skipAnimation)
		{
			UpdateState();
		}

		private void PlaceNozzle()
		{
			if (_equipmentManager?.EquippedEntity is GasPumpNozzle heldItem)
			{
				_equipmentManager.Unequip(heldItem);
				if (placeSound.IsValid())
				{
					NetworkAudioRelay?.PlayOneShot(placeSound, base.transform.position);
				}
			}
		}
	}
}
