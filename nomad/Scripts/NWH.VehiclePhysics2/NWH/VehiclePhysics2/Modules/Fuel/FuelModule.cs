using System;
using NWH.Common.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace NWH.VehiclePhysics2.Modules.Fuel
{
	[Serializable]
	public class FuelModule : VehicleComponent
	{
		[Tooltip("     Current amount of fuel in liters.")]
		public float amount = 50f;

		[Tooltip("    Fuel capacity in liters.")]
		public float capacity = 50f;

		[Tooltip("In case you do not need physically accurate fuel consumption you can lower/rise the consumption in here.")]
		public float consumptionMultiplier = 1f;

		[Tooltip("Engine efficiency (in percent). 1 would mean that all the energy contained in fuel would go into output power.")]
		public float efficiency = 0.45f;

		[Tooltip("    Consumption when idling indicated in percentage of max consumption. 0.05f = 5% out of maximum.")]
		public float idleConsumption = 0.1f;

		public float maxConsumptionPerHour = 20f;

		[Tooltip("    Called when vehicle runs out of fuel.")]
		public UnityEvent onOutOfFuel;

		public UnityEvent onAmountChanged;

		private float _consumptionThisFrame;

		private float _distanceTraveled;

		private float _prevAmount;

		[SerializeField]
		private float consumptionLPer100km;

		[SerializeField]
		private float consumptionPerHour;

		public float ConsumptionKilometersPerLiter => UnitConverter.L100kmToKml(consumptionLPer100km);

		public float ConsumptionLitersPer100Kilometers => consumptionLPer100km;

		public float ConsumptionLitersPerSecond => consumptionPerHour / 3600f;

		public float ConsumptionMPG => UnitConverter.L100kmToMpg(consumptionLPer100km);

		public float FuelPercentage => Mathf.Clamp01(amount / capacity);

		public bool HasFuel
		{
			get
			{
				if (!base.IsActive)
				{
					return true;
				}
				if (amount > 0f)
				{
					return true;
				}
				return false;
			}
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (vehicleController.powertrain.engine.IsRunning)
			{
				maxConsumptionPerHour = vehicleController.powertrain.engine.maxPower / 10f * Mathf.Clamp01(1f - efficiency);
				consumptionPerHour = vehicleController.powertrain.engine.generatedPower / vehicleController.powertrain.engine.maxPower * maxConsumptionPerHour;
				consumptionPerHour = Mathf.Clamp(consumptionPerHour, maxConsumptionPerHour * idleConsumption, 1f / 0f) * consumptionMultiplier;
				amount -= consumptionPerHour / 3600f * vehicleController.fixedDeltaTime;
				amount = Mathf.Clamp(amount, 0f, capacity);
				if (amount == 0f)
				{
					vehicleController.powertrain.engine.StopEngine();
				}
				_distanceTraveled = vehicleController.Speed * vehicleController.fixedDeltaTime;
				_consumptionThisFrame = consumptionPerHour / 3600f * Time.fixedDeltaTime;
				float num = 3600f / Time.fixedDeltaTime;
				float num2 = _consumptionThisFrame * num;
				float num3 = _distanceTraveled * num / 100000f;
				consumptionLPer100km = ((num3 == 0f) ? 0f : Mathf.Clamp(num2 / num3, 0f, 99.9f));
				onAmountChanged.Invoke();
			}
			else
			{
				consumptionPerHour = 0f;
				consumptionLPer100km = 0f;
			}
			if (amount == 0f && _prevAmount > 0f)
			{
				onOutOfFuel.Invoke();
			}
			_prevAmount = amount;
		}

		public override bool VC_Enable(bool calledByParent)
		{
			if (base.VC_Enable(calledByParent))
			{
				_prevAmount = amount;
				return true;
			}
			return false;
		}
	}
}
