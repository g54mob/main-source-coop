using NWH.Common.SceneManagement;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NWH.WheelController3D
{
	[RequireComponent(typeof(VehicleChanger))]
	public class GUIController : MonoBehaviour
	{
		public FrictionPreset genericFrictionPreset;

		public FrictionPreset gravelFrictionPreset;

		public FrictionPreset iceFrictionPreset;

		public FrictionPreset snowFrictionPreset;

		public Text speedText;

		public FrictionPreset tarmacDryFrictionPreset;

		public FrictionPreset tarmacWetFrictionPreset;

		private void Update()
		{
			if (Vehicle.ActiveVehicle != null)
			{
				float f = Mathf.RoundToInt(Vehicle.ActiveVehicle.Speed * 3.6f);
				speedText.text = Mathf.Abs(f).ToString();
			}
		}

		private WheelUAPI[] GetActiveVehicleWheels()
		{
			return Vehicle.ActiveVehicle.GetComponentsInChildren<WheelUAPI>();
		}

		public void AdjustFriction(FrictionPreset p)
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			for (int i = 0; i < activeVehicleWheels.Length; i++)
			{
				activeVehicleWheels[i].FrictionPreset = p;
			}
		}

		public void NextVehicle()
		{
			VehicleChanger.Instance.NextVehicle();
		}

		public void DecreaseBump()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI wheelUAPI in activeVehicleWheels)
			{
				wheelUAPI.DamperBumpRate -= wheelUAPI.DamperBumpRate * 0.1f;
				wheelUAPI.DamperBumpRate = Mathf.Clamp(wheelUAPI.DamperBumpRate, 1000f, 5000f);
			}
		}

		public void DecreaseCamber()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI obj in activeVehicleWheels)
			{
				float value = obj.Camber - 2f;
				value = Mathf.Clamp(value, -8f, 8f);
				obj.Camber = value;
			}
		}

		public void DecreaseRebound()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI wheelUAPI in activeVehicleWheels)
			{
				wheelUAPI.DamperReboundRate -= wheelUAPI.DamperReboundRate * 0.1f;
				wheelUAPI.DamperReboundRate = Mathf.Clamp(wheelUAPI.DamperReboundRate, 1000f, 5000f);
			}
		}

		public void DecreaseSpringLength()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI wheelUAPI in activeVehicleWheels)
			{
				wheelUAPI.SpringMaxLength -= wheelUAPI.SpringMaxLength * 0.1f;
				wheelUAPI.SpringMaxLength = Mathf.Clamp(wheelUAPI.SpringMaxLength, 0.15f, 0.6f);
			}
		}

		public void DecreaseSpringStrength()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI wheelUAPI in activeVehicleWheels)
			{
				wheelUAPI.SpringMaxForce -= wheelUAPI.SpringMaxForce * 0.1f;
				wheelUAPI.SpringMaxForce = Mathf.Clamp(wheelUAPI.SpringMaxForce, 14000f, 45000f);
			}
		}

		public void IncreaseBump()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI wheelUAPI in activeVehicleWheels)
			{
				wheelUAPI.DamperBumpRate += wheelUAPI.DamperBumpRate * 0.1f;
				wheelUAPI.DamperBumpRate = Mathf.Clamp(wheelUAPI.DamperBumpRate, 1000f, 5000f);
			}
		}

		public void IncreaseCamber()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI obj in activeVehicleWheels)
			{
				float value = obj.Camber + 2f;
				value = Mathf.Clamp(value, -8f, 8f);
				obj.Camber = value;
			}
		}

		public void IncreaseRebound()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI wheelUAPI in activeVehicleWheels)
			{
				wheelUAPI.DamperReboundRate += wheelUAPI.DamperReboundRate * 0.1f;
				wheelUAPI.DamperReboundRate = Mathf.Clamp(wheelUAPI.DamperReboundRate, 1000f, 5000f);
			}
		}

		public void IncreaseSpringLength()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI wheelUAPI in activeVehicleWheels)
			{
				wheelUAPI.SpringMaxLength += wheelUAPI.SpringMaxLength * 0.1f;
				wheelUAPI.SpringMaxLength = Mathf.Clamp(wheelUAPI.SpringMaxLength, 0.15f, 0.6f);
			}
		}

		public void IncreaseSpringStrength()
		{
			WheelUAPI[] activeVehicleWheels = GetActiveVehicleWheels();
			foreach (WheelUAPI wheelUAPI in activeVehicleWheels)
			{
				wheelUAPI.SpringMaxForce += wheelUAPI.SpringMaxForce * 0.1f;
				wheelUAPI.SpringMaxForce = Mathf.Clamp(wheelUAPI.SpringMaxForce, 14000f, 45000f);
			}
		}

		public void LevelReset()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void SurfaceGeneric()
		{
			AdjustFriction(genericFrictionPreset);
		}

		public void SurfaceGravel()
		{
			AdjustFriction(gravelFrictionPreset);
		}

		public void SurfaceIce()
		{
			AdjustFriction(iceFrictionPreset);
		}

		public void SurfaceSnow()
		{
			AdjustFriction(snowFrictionPreset);
		}

		public void SurfaceTarmacDry()
		{
			AdjustFriction(tarmacDryFrictionPreset);
		}

		public void SurfaceTarmacWet()
		{
			AdjustFriction(tarmacWetFrictionPreset);
		}
	}
}
