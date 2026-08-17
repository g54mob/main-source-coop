using Ami.BroAudio;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.LiquidTransferSystem.UI;
using NomadDrive.Features.Player;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace NomadDrive.Features.Vehicle.Interactables
{
	public class VehicleLiquidCap : VehicleInteractable, ILiquidSnapTarget
	{
		[Header("Lid Animation")]
		[SerializeField]
		private Vector3 lidOpenRotation = new Vector3(0f, 0f, 125f);

		[SerializeField]
		private Vector3 lidClosedRotation = new Vector3(0f, 0f, 0f);

		[SerializeField]
		private Ease lidOpenEase = Ease.OutBounce;

		[SerializeField]
		private Ease lidCloseEase = Ease.OutBounce;

		[SerializeField]
		private float lidOpenDuration = 1f;

		[SerializeField]
		private float lidCloseDuration = 1f;

		[Header("Transfer Settings")]
		[SerializeField]
		private float defaultTransferSpeed = 0.75f;

		[Header("Audio")]
		[SerializeField]
		private SoundID capOpenSound;

		[SerializeField]
		private SoundID capCloseSound;

		[Header("Snap")]
		[SerializeField]
		private Transform fillAnchor;

		[FormerlySerializedAs("capType")]
		[SerializeField]
		private LiquidSnapTargetType targetType;

		[Inject]
		private LiquidContainerInfoPanel _liquidContainerInfoPanel;

		[Inject]
		private IPlayerService _playerReferenceService;

		public ILiquidContainer RoutedLiquidContainer;

		private bool _isLidOpen;

		public Transform FillAnchor => fillAnchor;

		public LiquidSnapTargetType TargetType => targetType;

		public uint SnapNetId
		{
			get
			{
				if (!(RoutedLiquidContainer is LiquidContainerComponent liquidContainerComponent))
				{
					return 0u;
				}
				return liquidContainerComponent.netId;
			}
		}

		protected override bool UseStateMachine => false;

		protected override bool HasNetworkState => true;

		public void AssignTargetLiquidContainer(ILiquidContainer targetLiquidContainer)
		{
			RoutedLiquidContainer = targetLiquidContainer;
		}

		public void RemoveTargetLiquidContainer()
		{
			RoutedLiquidContainer = null;
		}

		public override void ApplyStateFromNetwork(byte stateData, bool skipAnimation)
		{
			bool flag = stateData != 0;
			if (_isLidOpen == flag)
			{
				return;
			}
			_isLidOpen = flag;
			if (!(base.ModelTransform == null))
			{
				if (skipAnimation)
				{
					base.ModelTransform.localRotation = Quaternion.Euler(_isLidOpen ? lidOpenRotation : lidClosedRotation);
				}
				else if (_isLidOpen)
				{
					Tween.LocalRotation(base.ModelTransform, Quaternion.Euler(lidOpenRotation), lidOpenDuration, lidOpenEase);
				}
				else
				{
					Tween.LocalRotation(base.ModelTransform, Quaternion.Euler(lidClosedRotation), lidCloseDuration, lidCloseEase);
				}
			}
		}

		public override void OnHovered()
		{
			base.OnHovered();
			if (RoutedLiquidContainer != null)
			{
				_liquidContainerInfoPanel?.OnLiquidContainerHovered(RoutedLiquidContainer);
			}
			TriggerSnap(active: true);
			OpenLid();
			UpdateInteractions();
			if (capOpenSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(capOpenSound, base.transform.position);
			}
		}

		public override void OnUnhovered()
		{
			base.OnUnhovered();
			_liquidContainerInfoPanel?.OnLiquidContainerHovered(null);
			TriggerSnap(active: false);
			CloseLid();
			UpdateInteractions();
			if (capCloseSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(capCloseSound, base.transform.position);
			}
		}

		private void TriggerSnap(bool active)
		{
			if (RoutedLiquidContainer == null)
			{
				return;
			}
			HeldItem heldItem = _playerReferenceService?.EquipmentManager?.EquippedEntity;
			if (!(heldItem == null) && heldItem.TryGetComponent<LiquidContainerSnapHandler>(out var component))
			{
				if (active)
				{
					component.Snap(this);
				}
				else
				{
					component.Unsnap();
				}
			}
		}

		private void OpenLid()
		{
			if (!_isLidOpen)
			{
				RequestStateChange(1);
			}
		}

		private void CloseLid()
		{
			if (_isLidOpen)
			{
				RequestStateChange(0);
			}
		}

		private void UpdateInteractions()
		{
			ILiquidContainer container;
			if (_playerReferenceService?.EquipmentManager == null)
			{
				DeactivateAllInteractions();
			}
			else if (!_playerReferenceService.EquipmentManager.TryGetEquippedILiquidContainer(out container))
			{
				DeactivateAllInteractions();
			}
			else
			{
				ActivateAllInteractions();
			}
		}

		private void ActivateAllInteractions()
		{
			foreach (NomadDrive.Features.Interaction.Interaction activeInteraction in base.ActiveInteractions)
			{
				if (activeInteraction.StateHandler.CurrentState == InteractionState.Deactivated)
				{
					activeInteraction.Activate();
				}
			}
			base.OnInteractionActivityPerformed.Invoke();
		}

		private void DeactivateAllInteractions()
		{
			foreach (NomadDrive.Features.Interaction.Interaction activeInteraction in base.ActiveInteractions)
			{
				if (activeInteraction.StateHandler.CurrentState != InteractionState.Deactivated)
				{
					activeInteraction.Deactivate();
				}
			}
			base.OnInteractionActivityPerformed.Invoke();
		}
	}
}
