using Ami.BroAudio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.LiquidTransferSystem.UI;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Tools;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.LiquidDrinking
{
	[RequireComponent(typeof(LiquidContainerComponent))]
	public class DrinkableHeldItem : DirectHeldItem, ITransferDrinkLock
	{
		[Header("Drinking")]
		[SerializeField]
		private LiquidStatEffectsConfig liquidStatEffectsConfig;

		[SerializeField]
		private float drinkingSpeedOverride;

		[Header("Initial Liquid (set to Empty/0 to skip)")]
		[SerializeField]
		private LiquidType initialLiquidType;

		[SerializeField]
		private float initialLiquidAmount;

		[Header("Visual")]
		[SerializeField]
		private LiquidFillVisual liquidFillVisual;

		[Header("Audio")]
		[SerializeField]
		private SoundID drinkSipSound;

		[SerializeField]
		[Min(0.05f)]
		private float drinkSipInterval = 0.5f;

		[Inject]
		protected LiquidContainerInfoPanel LiquidContainerInfoPanel;

		private InteractionStateMachine<DrinkableItemState> _drinkableStateMachine;

		private PlayerLiquidEffectsManager _effectsManager;

		private bool _isDrinking;

		private float _sipTimer;

		private bool _transferLocked;

		[field: SerializeField]
		public LiquidContainerComponent LiquidContainer { get; private set; }

		public override string UseActionPromptId => "LiquidDrinking_Drink";

		protected override bool UseStateMachine => true;

		protected override bool UseDefaultStateMachine => false;

		private float EffectiveDrinkingSpeed
		{
			get
			{
				if (!(drinkingSpeedOverride > 0f))
				{
					if (!(liquidStatEffectsConfig != null))
					{
						return 0.5f;
					}
					return liquidStatEffectsConfig.DrinkingSpeed;
				}
				return drinkingSpeedOverride;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			LiquidContainer = GetComponent<LiquidContainerComponent>();
			if (LiquidContainer == null)
			{
				EvilLogger.LogError("[DrinkableHeldItem] LiquidContainerComponent is missing on " + base.gameObject.name, "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\LiquidDrinking\\Scripts\\DrinkableHeldItem.cs", 63);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (LiquidContainer != null)
			{
				liquidFillVisual?.Bind(LiquidContainer);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			liquidFillVisual?.Unbind();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			liquidFillVisual?.Refresh();
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			if (LiquidContainer != null && initialLiquidType != LiquidType.Empty)
			{
				LiquidContainer.SetLiquidType(initialLiquidType);
				if (!LiquidContainer.RandomizeInitialFill && initialLiquidAmount > 0f)
				{
					LiquidContainer.SetAmount(initialLiquidAmount);
				}
			}
		}

		protected override void InitializeStateMachine()
		{
			_drinkableStateMachine = new InteractionStateMachine<DrinkableItemState>(this);
			base.BaseStateMachine = _drinkableStateMachine;
			ConfigureStates();
			_drinkableStateMachine.Initialize(DetermineState());
		}

		protected override void ConfigureStates()
		{
			_drinkableStateMachine.RegisterState(DrinkableItemState.Idle, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(DrinkableItemState.IdleEmpty, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(DrinkableItemState.Equipped, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default))
				.RegisterState(DrinkableItemState.EquippedEmpty, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		private DrinkableItemState DetermineState()
		{
			bool flag = LiquidContainer == null || LiquidContainer.State == LiquidContainerState.Empty;
			if (base.IsEquipped)
			{
				if (!flag)
				{
					return DrinkableItemState.Equipped;
				}
				return DrinkableItemState.EquippedEmpty;
			}
			if (!flag)
			{
				return DrinkableItemState.Idle;
			}
			return DrinkableItemState.IdleEmpty;
		}

		public override void UpdateState()
		{
			_drinkableStateMachine?.TransitionTo(DetermineState());
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			LiquidContainerInfoPanel?.OnLiquidContainerHovered(LiquidContainer);
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			LiquidContainerInfoPanel?.OnLiquidContainerHovered(null);
		}

		public override void OnUseButtonDown()
		{
			base.OnUseButtonDown();
			if (_transferLocked || LiquidContainer == null || LiquidContainer.State == LiquidContainerState.Empty)
			{
				return;
			}
			ObjectivesEventBus.Raise(ObjectiveSignal.DrinkConsumed, LiquidContainer.CurrentLiquidType);
			if (!(liquidStatEffectsConfig == null))
			{
				if ((object)_effectsManager == null)
				{
					_effectsManager = playerService?.LocalPlayer?.GetComponent<PlayerLiquidEffectsManager>();
				}
				_isDrinking = true;
				_sipTimer = drinkSipInterval;
			}
		}

		public override void OnUseButton()
		{
			base.OnUseButton();
			if (!_isDrinking || LiquidContainer == null)
			{
				return;
			}
			if (LiquidContainer.State == LiquidContainerState.Empty)
			{
				StopDrinking();
				return;
			}
			float num = EffectiveDrinkingSpeed * Time.deltaTime;
			float num2 = LiquidContainer.Drain(num);
			float num3 = num - num2;
			if (num3 > 0f && _effectsManager != null)
			{
				LiquidType currentLiquidType = LiquidContainer.CurrentLiquidType;
				_effectsManager.ApplyLiquidEffects(currentLiquidType, num3, liquidStatEffectsConfig);
			}
			_sipTimer += Time.deltaTime;
			if (_sipTimer >= drinkSipInterval)
			{
				_sipTimer = 0f;
				if (drinkSipSound.IsValid())
				{
					NetworkAudioRelay?.PlayOneShot(drinkSipSound, base.transform.position);
				}
			}
			if (LiquidContainer.State == LiquidContainerState.Empty)
			{
				StopDrinking();
			}
		}

		public override void OnUseButtonUp()
		{
			base.OnUseButtonUp();
			StopDrinking();
		}

		private void StopDrinking()
		{
			_isDrinking = false;
			_sipTimer = 0f;
			UpdateState();
		}

		public void SetTransferLocked(bool locked)
		{
			_transferLocked = locked;
			if (locked && _isDrinking)
			{
				StopDrinking();
			}
		}

		public override void OnEquip()
		{
			base.OnEquip();
			_effectsManager = playerService?.LocalPlayer?.GetComponent<PlayerLiquidEffectsManager>();
		}

		public override void OnUnequip()
		{
			base.OnUnequip();
			_isDrinking = false;
			_sipTimer = 0f;
			_transferLocked = false;
		}

		protected override void SetForLateJoiner()
		{
			base.SetForLateJoiner();
			LiquidContainer?.SetLateJoinCompleted();
			UpdateState();
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
