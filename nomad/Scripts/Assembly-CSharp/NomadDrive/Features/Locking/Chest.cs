using System.Collections.Generic;
using EvilCore.EvilSave;
using EvilCore.UI.Scripts;
using Mirror;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.SaveSystem;
using UnityEngine;

namespace NomadDrive.Features.Locking
{
	public class Chest : HeldItem, INetworkSaveable, ISnappingPlaneContainer
	{
		[Header("Chest Tier")]
		[SerializeField]
		private ChestType chestType;

		[Header("References")]
		[SerializeField]
		private InteractableHinge lidHinge;

		[SerializeField]
		private List<SnappingPlane> interiorSnappingPlanes = new List<SnappingPlane>();

		private InteractionStateMachine<ChestState> _chestStateMachine;

		private SnappingPlaneContainerHelper _containerHelper;

		public ChestType ChestType => chestType;

		protected override bool UseStateMachine => true;

		protected override bool UseDefaultStateMachine => false;

		public string ContributorKey => "chest";

		public SnappingPlane[] SnappingPlanes => _containerHelper?.SnappingPlanes;

		protected override void InitializeStateMachine()
		{
			_chestStateMachine = new InteractionStateMachine<ChestState>(this);
			base.BaseStateMachine = _chestStateMachine;
			ConfigureChestStates();
			_chestStateMachine.Initialize(DetermineChestState());
		}

		private void ConfigureChestStates()
		{
			_chestStateMachine.RegisterState(ChestState.Idle, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(ChestState.DoorsOpen, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Invalid).WithNameLabelVisibility(visible: true).WithInteractionLabelVisibility(visible: false)).RegisterState(ChestState.Equipped, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		public override void UpdateState()
		{
			_chestStateMachine?.TransitionTo(DetermineChestState());
		}

		private ChestState DetermineChestState()
		{
			if (base.IsEquipped)
			{
				return ChestState.Equipped;
			}
			if (lidHinge != null && lidHinge.IsOpen)
			{
				return ChestState.DoorsOpen;
			}
			return ChestState.Idle;
		}

		protected override void Start()
		{
			base.Start();
			Init();
		}

		private void Init()
		{
			InitializeContainer();
			if (lidHinge == null)
			{
				return;
			}
			foreach (SnappingPlane interiorSnappingPlane in interiorSnappingPlanes)
			{
				if (!(interiorSnappingPlane == null))
				{
					interiorSnappingPlane.Disable();
				}
			}
			SetSnappingPlaneSubscriptions();
			lidHinge.OnOpened.AddListener(UpdateState);
			lidHinge.OnClosed.AddListener(UpdateState);
			lidHinge.SetDurations(0.5f, 0.5f);
			base.OnEquipped.AddListener(HandleChestEquipped);
			base.OnUnequipped.AddListener(HandleChestUnequipped);
			if (lidHinge.IsOpen)
			{
				HandleLidOpenedForInterior();
			}
		}

		private void HandleChestEquipped()
		{
			if (!(lidHinge == null))
			{
				lidHinge.IgnoreHovering();
				lidHinge.SetInteractionAvailability(newValue: false);
			}
		}

		private void HandleChestUnequipped()
		{
			if (!(lidHinge == null))
			{
				lidHinge.UnignoreHovering();
				lidHinge.SetInteractionAvailability(newValue: true);
			}
		}

		private void SetSnappingPlaneSubscriptions()
		{
			lidHinge.OnOpened.AddListener(HandleLidOpenedForInterior);
			lidHinge.OnClosed.AddListener(HandleLidClosedForInterior);
		}

		private void HandleLidOpenedForInterior()
		{
			foreach (SnappingPlane interiorSnappingPlane in interiorSnappingPlanes)
			{
				if (!(interiorSnappingPlane == null))
				{
					interiorSnappingPlane.Enable();
					interiorSnappingPlane.ActivateSnappedObjectsPhysics();
					interiorSnappingPlane.ActivateSnappedObjectsInteraction();
				}
			}
		}

		private void HandleLidClosedForInterior()
		{
			foreach (SnappingPlane interiorSnappingPlane in interiorSnappingPlanes)
			{
				if (!(interiorSnappingPlane == null))
				{
					interiorSnappingPlane.DeactivateSnappedObjectsPhysics();
					interiorSnappingPlane.DeactivateSnappedObjectsInteraction();
					interiorSnappingPlane.Disable();
				}
			}
		}

		private void OnDestroy()
		{
			if (!(lidHinge == null))
			{
				lidHinge.OnOpened.RemoveListener(UpdateState);
				lidHinge.OnClosed.RemoveListener(UpdateState);
				lidHinge.OnOpened.RemoveListener(HandleLidOpenedForInterior);
				lidHinge.OnClosed.RemoveListener(HandleLidClosedForInterior);
				base.OnEquipped.RemoveListener(HandleChestEquipped);
				base.OnUnequipped.RemoveListener(HandleChestUnequipped);
			}
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(lidHinge != null && lidHinge.IsLocked);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			bool flag = reader.ReadBool();
			if (NetworkServer.active && !(lidHinge == null) && !flag)
			{
				Lock componentInChildren = GetComponentInChildren<Lock>(includeInactive: true);
				if (componentInChildren != null)
				{
					componentInChildren.ServerSnapRemoved();
				}
				lidHinge.ServerSetLocked(locked: false);
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		private void InitializeContainer()
		{
			_containerHelper = new SnappingPlaneContainerHelper(this);
			_containerHelper.Initialize();
		}

		public SnappingPlane GetSnappingPlaneByIndex(ushort index)
		{
			if (_containerHelper == null)
			{
				InitializeContainer();
			}
			return _containerHelper.GetSnappingPlaneByIndex(index);
		}

		public ushort GetSnappingPlaneIDBySnappingPlaneReference(SnappingPlane snappingPlane)
		{
			if (_containerHelper == null)
			{
				return 0;
			}
			return _containerHelper.GetSnappingPlaneIDBySnappingPlaneReference(snappingPlane);
		}

		public bool IsAnySnappingPlaneOneShotSlotFull()
		{
			return _containerHelper?.IsAnySnappingPlaneOneShotSlotFull() ?? false;
		}

		public bool IsSnappingPlaneOccupied(int snappingPlaneID)
		{
			return _containerHelper?.IsSnappingPlaneOccupied(snappingPlaneID) ?? false;
		}

		public bool IsSnappingPlaneOccupied(SnappingPlane snappingPlane)
		{
			return _containerHelper?.IsSnappingPlaneOccupied(snappingPlane) ?? false;
		}

		public void EnableAllSnappingPlanes()
		{
			_containerHelper?.EnableAllSnappingPlanes();
		}

		public void DisableAllSnappingPlanes()
		{
			_containerHelper?.DisableAllSnappingPlanes();
		}

		public void EnableSnappingPlane(int snappingPlaneID)
		{
			_containerHelper?.EnableSnappingPlane(snappingPlaneID);
		}

		public void DisableSnappingPlane(int snappingPlaneID)
		{
			_containerHelper?.DisableSnappingPlane(snappingPlaneID);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
