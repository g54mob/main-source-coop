using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.Vehicle;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Furnitures
{
	[RequireComponent(typeof(SnappingPlanesManager))]
	public class Fridge : HeldItem
	{
		[SerializeField]
		private InteractableHinge mainDoorHinge = new InteractableHinge();

		[SerializeField]
		private InteractableHinge freezerDoorHinge = new InteractableHinge();

		[SerializeField]
		private List<SnappingPlane> mainDoorSnappingPlanes = new List<SnappingPlane>();

		[SerializeField]
		private List<SnappingPlane> freezerDoorSnappingPlanes = new List<SnappingPlane>();

		[Header("Audio")]
		[SerializeField]
		private SoundID idleSound;

		private InteractionStateMachine<FridgeState> _fridgeStateMachine;

		private AudioHandle _idleInstance;

		private bool _hasIdleInstance;

		public UnityEvent OnFridgeActivated { get; } = new UnityEvent();

		public UnityEvent OnFridgeDeactivated { get; } = new UnityEvent();

		public bool IsInitialized { get; set; }

		protected override bool UseStateMachine => true;

		protected override bool UseDefaultStateMachine => false;

		protected override void InitializeStateMachine()
		{
			_fridgeStateMachine = new InteractionStateMachine<FridgeState>(this);
			base.BaseStateMachine = _fridgeStateMachine;
			ConfigureFridgeStates();
			_fridgeStateMachine.Initialize(DetermineFridgeState());
		}

		private void ConfigureFridgeStates()
		{
			_fridgeStateMachine.RegisterState(FridgeState.Idle, (InteractionStateConfig config) => config.WithBasicInteraction(InteractionKey.Primary, "@interaction.equip", HandleEquip).WithCondition(() => playerService?.EquipmentManager?.IsItemEquipped != true && !IsLocalPlayerSitting()).WithCrosshair(CrosshairType.StartGrab)
				.WithNameLabelVisibility(visible: true)
				.WithInteractionLabelVisibility(visible: false)).RegisterState(FridgeState.DoorsOpen, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Invalid).WithNameLabelVisibility(visible: true).WithInteractionLabelVisibility(visible: false)).RegisterState(FridgeState.Equipped, (InteractionStateConfig config) => config.WithCrosshair(CrosshairType.Default));
		}

		public override void UpdateState()
		{
			_fridgeStateMachine?.TransitionTo(DetermineFridgeState());
		}

		private FridgeState DetermineFridgeState()
		{
			if (base.IsEquipped)
			{
				return FridgeState.Equipped;
			}
			if (IsAnyDoorOpened())
			{
				return FridgeState.DoorsOpen;
			}
			return FridgeState.Idle;
		}

		public void Init()
		{
			foreach (SnappingPlane mainDoorSnappingPlane in mainDoorSnappingPlanes)
			{
				mainDoorSnappingPlane.Disable();
			}
			foreach (SnappingPlane freezerDoorSnappingPlane in freezerDoorSnappingPlanes)
			{
				freezerDoorSnappingPlane.Disable();
			}
			SetSnappingPlaneSubscriptionForOpeningDoors();
			SetSnappingPlanesSubscriptionForClosingDoors();
			mainDoorHinge.OnOpened.AddListener(UpdateState);
			mainDoorHinge.OnClosed.AddListener(UpdateState);
			freezerDoorHinge.OnOpened.AddListener(UpdateState);
			freezerDoorHinge.OnClosed.AddListener(UpdateState);
			freezerDoorHinge.SetDurations(0.4f, 0.5f);
			mainDoorHinge.SetDurations(0.4f, 0.5f);
			IsInitialized = true;
			if (AudioManager != null && !_hasIdleInstance && idleSound.IsValid())
			{
				_idleInstance = AudioManager.PlayEventAttached(idleSound, base.gameObject);
				_hasIdleInstance = true;
			}
		}

		protected void OnDestroy()
		{
			if (_hasIdleInstance)
			{
				AudioManager?.StopEvent(_idleInstance, AudioStopMode.Immediate);
				_hasIdleInstance = false;
			}
		}

		protected override void Start()
		{
			base.Start();
			Init();
		}

		private void SetSnappingPlaneSubscriptionForOpeningDoors()
		{
			freezerDoorHinge.OnOpened.AddListener(delegate
			{
				foreach (SnappingPlane freezerDoorSnappingPlane in freezerDoorSnappingPlanes)
				{
					freezerDoorSnappingPlane.ActivateSnappedObjectsPhysics();
					freezerDoorSnappingPlane.ActivateSnappedObjectsInteraction();
					freezerDoorSnappingPlane.Enable();
				}
			});
			mainDoorHinge.OnOpened.AddListener(delegate
			{
				foreach (SnappingPlane mainDoorSnappingPlane in mainDoorSnappingPlanes)
				{
					mainDoorSnappingPlane.ActivateSnappedObjectsPhysics();
					mainDoorSnappingPlane.ActivateSnappedObjectsInteraction();
					mainDoorSnappingPlane.Enable();
				}
			});
		}

		private void SetSnappingPlanesSubscriptionForClosingDoors()
		{
			freezerDoorHinge.OnClosed.AddListener(delegate
			{
				foreach (SnappingPlane freezerDoorSnappingPlane in freezerDoorSnappingPlanes)
				{
					freezerDoorSnappingPlane.DeactivateSnappedObjectsPhysics();
					freezerDoorSnappingPlane.DeactivateSnappedObjectsInteraction();
					freezerDoorSnappingPlane.Disable();
				}
			});
			mainDoorHinge.OnClosed.AddListener(delegate
			{
				foreach (SnappingPlane mainDoorSnappingPlane in mainDoorSnappingPlanes)
				{
					mainDoorSnappingPlane.DeactivateSnappedObjectsPhysics();
					mainDoorSnappingPlane.DeactivateSnappedObjectsInteraction();
					mainDoorSnappingPlane.Disable();
				}
			});
		}

		public void ActivatePower()
		{
			OnFridgeActivated.Invoke();
		}

		public void DeactivatePower()
		{
			OnFridgeDeactivated.Invoke();
		}

		public bool IsAllDoorsOpened()
		{
			if (mainDoorHinge.IsOpen)
			{
				return freezerDoorHinge.IsOpen;
			}
			return false;
		}

		private bool IsAllDoorsClosed()
		{
			if (!mainDoorHinge.IsOpen)
			{
				return !freezerDoorHinge.IsOpen;
			}
			return false;
		}

		private bool IsAnyDoorOpened()
		{
			if (!mainDoorHinge.IsOpen)
			{
				return freezerDoorHinge.IsOpen;
			}
			return true;
		}

		private bool IsAnyDoorClosed()
		{
			if (mainDoorHinge.IsOpen)
			{
				return !freezerDoorHinge.IsOpen;
			}
			return true;
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
