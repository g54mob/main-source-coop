using NomadDrive.Features.Vehicle.Enums;
using NomadDrive.Features.Vehicle.Parts.Doors;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleDoorModule : VehicleModule
	{
		private const string DOORS = "Doors";

		[SerializeField]
		public VehicleDoorSlot LeftDoorSlotRef;

		[SerializeField]
		public VehicleDoorSlot RightDoorSlotRef;

		[SerializeField]
		public VehicleDoor InstalledLeftDoor;

		[SerializeField]
		public VehicleDoor InstalledRightDoor;

		private bool _isBatteryInstalled;

		private bool _isBatteryBroken;

		private void Start()
		{
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			if (module != null)
			{
				_isBatteryInstalled = module.IsBatteryInstalled;
				_isBatteryBroken = module.IsBatteryBroken;
			}
			InitDoors();
		}

		protected override void SubscribeEvents()
		{
			LeftDoorSlotRef.OnDoorAttached.AddListener(OnLeftDoorAttached);
			LeftDoorSlotRef.OnDoorDetached.AddListener(OnLeftDoorDetached);
			RightDoorSlotRef.OnDoorAttached.AddListener(OnRightDoorAttached);
			RightDoorSlotRef.OnDoorDetached.AddListener(OnRightDoorDetached);
			base.EventBus.OnBatteryUsefulChanged += OnBatteryUsefulChanged;
			base.EventBus.OnBatteryDepleted += OnBatteryDepleted;
			base.EventBus.OnBatteryInstalled += OnBatteryInstalled;
		}

		protected override void UnsubscribeEvents()
		{
			LeftDoorSlotRef.OnDoorAttached.RemoveListener(OnLeftDoorAttached);
			LeftDoorSlotRef.OnDoorDetached.RemoveListener(OnLeftDoorDetached);
			RightDoorSlotRef.OnDoorAttached.RemoveListener(OnRightDoorAttached);
			RightDoorSlotRef.OnDoorDetached.RemoveListener(OnRightDoorDetached);
			base.EventBus.OnBatteryUsefulChanged -= OnBatteryUsefulChanged;
			base.EventBus.OnBatteryDepleted -= OnBatteryDepleted;
			base.EventBus.OnBatteryInstalled -= OnBatteryInstalled;
		}

		private void RefreshBatteryState()
		{
			VehicleBatteryModule module = base.VehicleManager.GetModule<VehicleBatteryModule>();
			_isBatteryInstalled = module?.IsBatteryInstalled ?? false;
			_isBatteryBroken = module?.IsBatteryBroken ?? true;
		}

		private void OnBatteryUsefulChanged(bool isBatteryUseful)
		{
			RefreshBatteryState();
		}

		private void OnBatteryInstalled()
		{
			RefreshBatteryState();
			VehicleDoor installedLeftDoor = InstalledLeftDoor;
			if ((object)installedLeftDoor != null && installedLeftDoor.WindowControlType == WindowControlType.Electric)
			{
				InstalledLeftDoor.ResumeWindowAnimation();
			}
			VehicleDoor installedRightDoor = InstalledRightDoor;
			if ((object)installedRightDoor != null && installedRightDoor.WindowControlType == WindowControlType.Electric)
			{
				InstalledRightDoor.ResumeWindowAnimation();
			}
		}

		private void OnBatteryDepleted()
		{
			_isBatteryInstalled = false;
			_isBatteryBroken = true;
			VehicleDoor installedLeftDoor = InstalledLeftDoor;
			if ((object)installedLeftDoor != null && installedLeftDoor.WindowControlType == WindowControlType.Electric)
			{
				InstalledLeftDoor.PauseWindowAnimation();
			}
			VehicleDoor installedRightDoor = InstalledRightDoor;
			if ((object)installedRightDoor != null && installedRightDoor.WindowControlType == WindowControlType.Electric)
			{
				InstalledRightDoor.PauseWindowAnimation();
			}
		}

		private void OnLeftDoorWindowDown()
		{
			InstalledLeftDoor?.OpenWindowElectric(_isBatteryInstalled, _isBatteryBroken);
		}

		private void OnLeftDoorWindowUp()
		{
			InstalledLeftDoor?.CloseWindowElectric(_isBatteryInstalled, _isBatteryBroken);
		}

		private void OnRightDoorWindowDown()
		{
			InstalledRightDoor?.OpenWindowElectric(_isBatteryInstalled, _isBatteryBroken);
		}

		private void OnRightDoorWindowUp()
		{
			InstalledRightDoor?.CloseWindowElectric(_isBatteryInstalled, _isBatteryBroken);
		}

		private void OnLeftDoorWindowCrank()
		{
			InstalledLeftDoor?.ToggleWindowMechanical();
		}

		private void OnRightDoorWindowCrank()
		{
			InstalledRightDoor?.ToggleWindowMechanical();
		}

		private void OnLeftDoorAttached(VehicleDoor door)
		{
			InstalledLeftDoor = door;
			WireWindowControl(door, isLeft: true);
		}

		private void OnLeftDoorDetached()
		{
			UnwireWindowControl(InstalledLeftDoor, isLeft: true);
			InstalledLeftDoor = null;
		}

		private void OnRightDoorAttached(VehicleDoor door)
		{
			InstalledRightDoor = door;
			WireWindowControl(door, isLeft: false);
		}

		private void OnRightDoorDetached()
		{
			UnwireWindowControl(InstalledRightDoor, isLeft: false);
			InstalledRightDoor = null;
		}

		private void WireWindowControl(VehicleDoor door, bool isLeft)
		{
			if (door.WindowControlType == WindowControlType.Electric)
			{
				if (isLeft)
				{
					door.WindowDownButton.onButtonPressed.AddListener(OnLeftDoorWindowDown);
					door.WindowUpButton.onButtonPressed.AddListener(OnLeftDoorWindowUp);
				}
				else
				{
					door.WindowDownButton.onButtonPressed.AddListener(OnRightDoorWindowDown);
					door.WindowUpButton.onButtonPressed.AddListener(OnRightDoorWindowUp);
				}
			}
			else if (isLeft)
			{
				door.WindowCrank.onCrankTurned.AddListener(OnLeftDoorWindowCrank);
			}
			else
			{
				door.WindowCrank.onCrankTurned.AddListener(OnRightDoorWindowCrank);
			}
		}

		private void UnwireWindowControl(VehicleDoor door, bool isLeft)
		{
			if (door == null)
			{
				return;
			}
			if (door.WindowControlType == WindowControlType.Electric)
			{
				if (isLeft)
				{
					door.WindowDownButton.onButtonPressed.RemoveListener(OnLeftDoorWindowDown);
					door.WindowUpButton.onButtonPressed.RemoveListener(OnLeftDoorWindowUp);
				}
				else
				{
					door.WindowDownButton.onButtonPressed.RemoveListener(OnRightDoorWindowDown);
					door.WindowUpButton.onButtonPressed.RemoveListener(OnRightDoorWindowUp);
				}
			}
			else if (isLeft)
			{
				door.WindowCrank.onCrankTurned.RemoveListener(OnLeftDoorWindowCrank);
			}
			else
			{
				door.WindowCrank.onCrankTurned.RemoveListener(OnRightDoorWindowCrank);
			}
		}

		private void InitDoors()
		{
			if (InstalledLeftDoor != null)
			{
				WireWindowControl(InstalledLeftDoor, isLeft: true);
			}
			if (InstalledRightDoor != null)
			{
				WireWindowControl(InstalledRightDoor, isLeft: false);
			}
		}

		public bool IsFrontLeftDoorOpen()
		{
			if (InstalledLeftDoor != null)
			{
				return InstalledLeftDoor.VehicleDoorState.Equals(VehicleDoorState.Opened);
			}
			return false;
		}

		public bool IsFrontRightDoorOpen()
		{
			if (InstalledRightDoor != null)
			{
				return InstalledRightDoor.VehicleDoorState.Equals(VehicleDoorState.Opened);
			}
			return false;
		}

		public override void OnFrontSeatsTaken()
		{
			InstalledLeftDoor?.IgnoreHovering();
			InstalledRightDoor?.IgnoreHovering();
		}

		public override void OnFrontSeatsVacated()
		{
			InstalledLeftDoor?.UnignoreHovering();
			InstalledRightDoor?.UnignoreHovering();
		}
	}
}
