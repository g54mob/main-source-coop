using Sirenix.OdinInspector;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Testing
{
	[CreateAssetMenu(fileName = "NewVehiclePartConfig", menuName = "NomadDrive/Vehicle/Vehicle Part Config (Testing)")]
	public class VehiclePartConfig : SerializedScriptableObject
	{
		public SlotPartConfig engine = new SlotPartConfig();

		public SlotPartConfig battery = new SlotPartConfig();

		public SlotPartConfig hood = new SlotPartConfig();

		public SlotPartConfig frontLeftTire = new SlotPartConfig();

		public SlotPartConfig frontRightTire = new SlotPartConfig();

		public SlotPartConfig rearLeftTire = new SlotPartConfig();

		public SlotPartConfig rearRightTire = new SlotPartConfig();

		public SlotPartConfig leftDoor = new SlotPartConfig();

		public SlotPartConfig rightDoor = new SlotPartConfig();

		public SlotPartConfig frontLeftSeat = new SlotPartConfig();

		public SlotPartConfig frontRightSeat = new SlotPartConfig();

		public SlotPartConfig leftHeadlight = new SlotPartConfig();

		public SlotPartConfig rightHeadlight = new SlotPartConfig();

		public SlotPartConfig leftBrakelight = new SlotPartConfig();

		public SlotPartConfig rightBrakelight = new SlotPartConfig();

		public SlotPartConfig steeringWheel = new SlotPartConfig();

		public SlotPartConfig handbrake = new SlotPartConfig();

		public SlotPartConfig leftSunvisor = new SlotPartConfig();

		public SlotPartConfig rightSunvisor = new SlotPartConfig();

		public SlotPartConfig generator = new SlotPartConfig();

		[Range(0f, 100f)]
		public float fuelPercentage = 100f;

		private void EnableAll()
		{
			SetAllEnabled(enabled: true);
		}

		private void DisableAll()
		{
			SetAllEnabled(enabled: false);
		}

		private void EnableEssentialOnly()
		{
			SetAllEnabled(enabled: false);
			frontLeftTire.enabled = true;
			frontRightTire.enabled = true;
			rearLeftTire.enabled = true;
			rearRightTire.enabled = true;
			steeringWheel.enabled = true;
			frontLeftSeat.enabled = true;
			frontRightSeat.enabled = true;
			handbrake.enabled = true;
		}

		private void SetAllConditions(float conditionValue = 100f)
		{
			engine.condition = conditionValue;
			battery.condition = conditionValue;
			hood.condition = conditionValue;
			frontLeftTire.condition = conditionValue;
			frontRightTire.condition = conditionValue;
			rearLeftTire.condition = conditionValue;
			rearRightTire.condition = conditionValue;
			leftDoor.condition = conditionValue;
			rightDoor.condition = conditionValue;
			frontLeftSeat.condition = conditionValue;
			frontRightSeat.condition = conditionValue;
			leftHeadlight.condition = conditionValue;
			rightHeadlight.condition = conditionValue;
			leftBrakelight.condition = conditionValue;
			rightBrakelight.condition = conditionValue;
			steeringWheel.condition = conditionValue;
			handbrake.condition = conditionValue;
			leftSunvisor.condition = conditionValue;
			rightSunvisor.condition = conditionValue;
			generator.condition = conditionValue;
		}

		private void SetAllEnabled(bool enabled)
		{
			engine.enabled = enabled;
			battery.enabled = enabled;
			hood.enabled = enabled;
			frontLeftTire.enabled = enabled;
			frontRightTire.enabled = enabled;
			rearLeftTire.enabled = enabled;
			rearRightTire.enabled = enabled;
			leftDoor.enabled = enabled;
			rightDoor.enabled = enabled;
			frontLeftSeat.enabled = enabled;
			frontRightSeat.enabled = enabled;
			leftHeadlight.enabled = enabled;
			rightHeadlight.enabled = enabled;
			leftBrakelight.enabled = enabled;
			rightBrakelight.enabled = enabled;
			steeringWheel.enabled = enabled;
			handbrake.enabled = enabled;
			leftSunvisor.enabled = enabled;
			rightSunvisor.enabled = enabled;
			generator.enabled = enabled;
		}

		private void Reset()
		{
			InitializeAllSlots();
			SetAllConditions();
			fuelPercentage = 100f;
		}

		private void InitializeAllSlots()
		{
			if (engine == null)
			{
				engine = new SlotPartConfig();
			}
			if (battery == null)
			{
				battery = new SlotPartConfig();
			}
			if (hood == null)
			{
				hood = new SlotPartConfig();
			}
			if (frontLeftTire == null)
			{
				frontLeftTire = new SlotPartConfig();
			}
			if (frontRightTire == null)
			{
				frontRightTire = new SlotPartConfig();
			}
			if (rearLeftTire == null)
			{
				rearLeftTire = new SlotPartConfig();
			}
			if (rearRightTire == null)
			{
				rearRightTire = new SlotPartConfig();
			}
			if (leftDoor == null)
			{
				leftDoor = new SlotPartConfig();
			}
			if (rightDoor == null)
			{
				rightDoor = new SlotPartConfig();
			}
			if (frontLeftSeat == null)
			{
				frontLeftSeat = new SlotPartConfig();
			}
			if (frontRightSeat == null)
			{
				frontRightSeat = new SlotPartConfig();
			}
			if (leftHeadlight == null)
			{
				leftHeadlight = new SlotPartConfig();
			}
			if (rightHeadlight == null)
			{
				rightHeadlight = new SlotPartConfig();
			}
			if (leftBrakelight == null)
			{
				leftBrakelight = new SlotPartConfig();
			}
			if (rightBrakelight == null)
			{
				rightBrakelight = new SlotPartConfig();
			}
			if (steeringWheel == null)
			{
				steeringWheel = new SlotPartConfig();
			}
			if (handbrake == null)
			{
				handbrake = new SlotPartConfig();
			}
			if (leftSunvisor == null)
			{
				leftSunvisor = new SlotPartConfig();
			}
			if (rightSunvisor == null)
			{
				rightSunvisor = new SlotPartConfig();
			}
			if (generator == null)
			{
				generator = new SlotPartConfig();
			}
		}
	}
}
