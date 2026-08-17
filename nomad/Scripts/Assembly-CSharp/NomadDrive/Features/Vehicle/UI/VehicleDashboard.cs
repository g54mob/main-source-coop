using TMPro;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.UI
{
	public class VehicleDashboard : MonoBehaviour
	{
		[SerializeField]
		private VehicleDashboardLight leftBlinker;

		[SerializeField]
		private VehicleDashboardLight rightBlinker;

		[SerializeField]
		private VehicleDashboardLight lowBeam;

		[SerializeField]
		private VehicleDashboardLight highBeam;

		[SerializeField]
		private VehicleDashboardLight engineError;

		[SerializeField]
		private VehicleDashboardLight batteryError;

		[SerializeField]
		private VehicleDashboardLight handbrake;

		[SerializeField]
		private VehicleDashboardLight overheatWarning;

		[SerializeField]
		private VehicleAnalogGauge rpmGauge;

		[SerializeField]
		private VehicleAnalogGauge speedGauge;

		[SerializeField]
		private VehicleDigitalGauge gearGauge;

		[SerializeField]
		private VehicleAnalogGauge fuelGauge;

		[SerializeField]
		private VehicleAnalogGauge temperatureGauge;

		[SerializeField]
		private VehicleDigitalGauge odometerGauge;

		[SerializeField]
		private TMP_Text[] speedCheckpointTexts;

		[SerializeField]
		private TMP_Text[] rpmCheckpointTexts;

		[SerializeField]
		private TMP_Text rpmStepMultiplierText;

		public void SetRpmGaugeMaxValue(float maxRpm)
		{
			if (rpmCheckpointTexts == null || rpmCheckpointTexts.Length == 0)
			{
				rpmGauge.MaxValue = maxRpm;
				return;
			}
			int num = rpmCheckpointTexts.Length;
			int num2 = Mathf.CeilToInt(maxRpm / (float)num);
			float maxValue = num2 * num;
			rpmGauge.MaxValue = maxValue;
			for (int i = 0; i < num; i++)
			{
				if (rpmCheckpointTexts[i] != null)
				{
					rpmCheckpointTexts[i].text = (i + 1).ToString();
				}
			}
			if (rpmStepMultiplierText != null)
			{
				rpmStepMultiplierText.text = num2.ToString();
			}
		}

		public void UpdateRpmGauge(float rpmValue)
		{
			rpmGauge.Value = rpmValue;
		}

		public void SetSpeedGaugeMaxValue(float maxSpeed)
		{
			if (speedCheckpointTexts == null || speedCheckpointTexts.Length == 0)
			{
				speedGauge.MaxValue = maxSpeed;
				return;
			}
			int num = speedCheckpointTexts.Length;
			int num2 = num - 1;
			int num3 = Mathf.CeilToInt(maxSpeed / (float)num2 / 5f) * 5;
			float maxValue = num3 * num2;
			speedGauge.MaxValue = maxValue;
			for (int i = 0; i < num; i++)
			{
				if (speedCheckpointTexts[i] != null)
				{
					speedCheckpointTexts[i].text = (num3 * i).ToString();
				}
			}
		}

		public void UpdateSpeedGauge(float speed)
		{
			speedGauge.Value = speed;
		}

		public void SetFuelGaugeMaxValue(float capacity)
		{
			fuelGauge.MaxValue = capacity;
		}

		public void UpdateFuelGauge(float amount)
		{
			fuelGauge.Value = amount;
		}

		public void SetTemperatureGaugeMaxValue(float overheatThreshold)
		{
			if (!(temperatureGauge == null))
			{
				temperatureGauge.MaxValue = overheatThreshold;
			}
		}

		public void UpdateTemperatureGauge(float temperature)
		{
			if (!(temperatureGauge == null))
			{
				temperatureGauge.Value = temperature;
			}
		}

		public void UpdateOdometer(int km)
		{
			if (!(odometerGauge == null))
			{
				odometerGauge.NumericalValue = km;
			}
		}

		public void UpdateGearGauge(string gear)
		{
			gearGauge.StringValue = gear;
		}

		public void SetOverheatWarningLight(bool state)
		{
			if (!(overheatWarning == null))
			{
				if (state)
				{
					overheatWarning.On();
				}
				else
				{
					overheatWarning.Off();
				}
			}
		}

		private void ActivateAllDashboardLights()
		{
			leftBlinker.On();
			rightBlinker.On();
			lowBeam.On();
			highBeam.On();
			engineError.On();
			batteryError.On();
			handbrake.On();
			if (overheatWarning != null)
			{
				overheatWarning.On();
			}
		}

		private void DeactivateAllDashboardLights()
		{
			leftBlinker.Off();
			rightBlinker.Off();
			lowBeam.Off();
			highBeam.Off();
			engineError.Off();
			batteryError.Off();
			handbrake.Off();
			if (overheatWarning != null)
			{
				overheatWarning.Off();
			}
		}

		public void SetEngineErrorLight(bool state)
		{
			if (state)
			{
				engineError.On();
			}
			else
			{
				engineError.Off();
			}
		}

		public void SetBatteryErrorLight(bool state)
		{
			if (state)
			{
				batteryError.On();
			}
			else
			{
				batteryError.Off();
			}
		}

		public void SetHandbrakeErrorLight(bool state)
		{
			if (state)
			{
				handbrake.On();
			}
			else
			{
				handbrake.Off();
			}
		}

		public void ActivateLeftBlinkerInfoLight()
		{
			if (rightBlinker.IsOn)
			{
				rightBlinker.Off();
			}
			leftBlinker.On();
		}

		public void ActivateRightBlinkerInfoLight()
		{
			if (leftBlinker.IsOn)
			{
				leftBlinker.Off();
			}
			rightBlinker.On();
		}

		public void ActivateLowBeamInfoLight()
		{
			if (highBeam.IsOn)
			{
				highBeam.Off();
			}
			lowBeam.On();
		}

		public void ActivateHighBeamInfoLight()
		{
			if (lowBeam.IsOn)
			{
				lowBeam.Off();
			}
			highBeam.On();
		}

		public void DeactivateHeadlightInfoLight()
		{
			lowBeam.Off();
			highBeam.Off();
		}

		public void ActivateHandbrakeInfoLight()
		{
			handbrake.On();
		}

		public void DeactivateHandbrakeInfoLight()
		{
			handbrake.Off();
		}
	}
}
