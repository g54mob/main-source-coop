using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Vehicle;
using UnityEngine;

namespace NomadDrive.Features.Furnitures
{
	[RequireComponent(typeof(SnappingPlanesManager))]
	public class KitchenStand : HeldItem
	{
		[SerializeField]
		private InteractableHinge smallCabinetRightDoorHinge;

		[SerializeField]
		private InteractableHinge smallCabinetLeftDoorHinge;

		[SerializeField]
		private InteractableHinge largeCabinetRightDoorHinge;

		[SerializeField]
		private InteractableHinge largeCabinetLeftDoorHinge;

		[SerializeField]
		private SnappingPlane smallCabinetRightDoorSnappingPlane;

		[SerializeField]
		private SnappingPlane smallCabinetLeftDoorSnappingPlane;

		[SerializeField]
		private SnappingPlane largeCabinetRightDoorSnappingPlane;

		[SerializeField]
		private SnappingPlane largeCabinetLeftDoorSnappingPlane;

		[SerializeField]
		private InteractableRail drawerRail;

		[SerializeField]
		private SnappingPlane drawerSnappingPlane;

		private InteractionStateMachine<KitchenStandState> _kitchenStandStateMachine;

		protected override bool UseStateMachine => true;

		protected override bool UseDefaultStateMachine => false;

		protected override void InitializeStateMachine()
		{
			_kitchenStandStateMachine = new InteractionStateMachine<KitchenStandState>(this);
			base.BaseStateMachine = _kitchenStandStateMachine;
			ConfigureKitchenStandStates();
			_kitchenStandStateMachine.Initialize(DetermineKitchenStandState());
		}

		private void ConfigureKitchenStandStates()
		{
			_kitchenStandStateMachine.RegisterState(KitchenStandState.Idle, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(KitchenStandState.DoorsOpen, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Invalid).WithNameLabelVisibility(visible: true).WithInteractionLabelVisibility(visible: false)).RegisterState(KitchenStandState.Equipped, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		public override void UpdateState()
		{
			_kitchenStandStateMachine?.TransitionTo(DetermineKitchenStandState());
		}

		private KitchenStandState DetermineKitchenStandState()
		{
			if (base.IsEquipped)
			{
				return KitchenStandState.Equipped;
			}
			if (IsAnyDoorOpened())
			{
				return KitchenStandState.DoorsOpen;
			}
			return KitchenStandState.Idle;
		}

		protected override void Start()
		{
			base.Start();
			Init();
		}

		private void Init()
		{
			smallCabinetRightDoorSnappingPlane.Disable();
			smallCabinetLeftDoorSnappingPlane.Disable();
			largeCabinetRightDoorSnappingPlane.Disable();
			largeCabinetLeftDoorSnappingPlane.Disable();
			drawerSnappingPlane.Disable();
			SetupSmallCabinetLeftDoor();
			SetupSmallCabinetRightDoor();
			SetupLargeCabinetLeftDoor();
			SetupLargeCabinetRightDoor();
			SetupDrawer();
		}

		private void HandleDoorOpenEvent(InteractableHinge hinge, SnappingPlane snappingPlane)
		{
			hinge.OnOpened.AddListener(UpdateState);
			hinge.OnOpened.AddListener(delegate
			{
				snappingPlane.ActivateSnappedObjectsInteraction();
			});
			hinge.OnOpened.AddListener(delegate
			{
				snappingPlane.Enable();
			});
		}

		private void HandleDoorCloseEvent(InteractableHinge hinge, SnappingPlane snappingPlane)
		{
			hinge.OnClosed.AddListener(UpdateState);
			hinge.OnClosed.AddListener(delegate
			{
				snappingPlane.DeactivateSnappedObjectsInteraction();
			});
			hinge.OnClosed.AddListener(delegate
			{
				snappingPlane.Disable();
			});
		}

		private void HandleDrawerEvents(InteractableRail rail, SnappingPlane snappingPlane)
		{
			rail.OnOpened.AddListener(UpdateState);
			rail.OnOpened.AddListener(delegate
			{
				snappingPlane.ActivateSnappedObjectsInteraction();
			});
			rail.OnOpened.AddListener(delegate
			{
				snappingPlane.Enable();
			});
			rail.OnClosed.AddListener(UpdateState);
			rail.OnClosed.AddListener(delegate
			{
				snappingPlane.DeactivateSnappedObjectsInteraction();
			});
			rail.OnClosed.AddListener(delegate
			{
				snappingPlane.Disable();
			});
		}

		private void SetupSmallCabinetLeftDoor()
		{
			HandleDoorOpenEvent(smallCabinetLeftDoorHinge, smallCabinetLeftDoorSnappingPlane);
			HandleDoorCloseEvent(smallCabinetLeftDoorHinge, smallCabinetLeftDoorSnappingPlane);
			smallCabinetLeftDoorHinge.SetDurations(0.5f, 0.5f);
		}

		private void SetupSmallCabinetRightDoor()
		{
			HandleDoorOpenEvent(smallCabinetRightDoorHinge, smallCabinetRightDoorSnappingPlane);
			HandleDoorCloseEvent(smallCabinetRightDoorHinge, smallCabinetRightDoorSnappingPlane);
			smallCabinetRightDoorHinge.SetDurations(0.5f, 0.5f);
		}

		private void SetupLargeCabinetLeftDoor()
		{
			HandleDoorOpenEvent(largeCabinetLeftDoorHinge, largeCabinetLeftDoorSnappingPlane);
			HandleDoorCloseEvent(largeCabinetLeftDoorHinge, largeCabinetLeftDoorSnappingPlane);
			largeCabinetLeftDoorHinge.SetDurations(0.5f, 0.5f);
		}

		private void SetupLargeCabinetRightDoor()
		{
			HandleDoorOpenEvent(largeCabinetRightDoorHinge, largeCabinetRightDoorSnappingPlane);
			HandleDoorCloseEvent(largeCabinetRightDoorHinge, largeCabinetRightDoorSnappingPlane);
			largeCabinetRightDoorHinge.SetDurations(0.5f, 0.5f);
		}

		private void SetupDrawer()
		{
			HandleDrawerEvents(drawerRail, drawerSnappingPlane);
			drawerRail.SetDurations(0.5f, 0.5f);
		}

		private bool IsAnyDoorOpened()
		{
			if (!smallCabinetRightDoorHinge.IsOpen && !smallCabinetLeftDoorHinge.IsOpen && !largeCabinetRightDoorHinge.IsOpen && !largeCabinetLeftDoorHinge.IsOpen)
			{
				return drawerRail.IsOpen;
			}
			return true;
		}

		private bool IsAllDoorsClosed()
		{
			if (!smallCabinetRightDoorHinge.IsOpen && !smallCabinetLeftDoorHinge.IsOpen && !largeCabinetRightDoorHinge.IsOpen && !largeCabinetLeftDoorHinge.IsOpen)
			{
				return !drawerRail.IsOpen;
			}
			return false;
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
