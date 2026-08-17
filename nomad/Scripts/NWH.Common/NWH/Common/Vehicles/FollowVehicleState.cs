using UnityEngine;

namespace NWH.Common.Vehicles
{
	[DefaultExecutionOrder(21)]
	public class FollowVehicleState : MonoBehaviour
	{
		private Vehicle _vc;

		private void OnEnable()
		{
			_vc = GetComponentInParent<Vehicle>();
			if (_vc == null)
			{
				Debug.LogError("VehicleController not found.");
			}
			_vc.onEnable.AddListener(OnVehicleWake);
			_vc.onDisable.AddListener(OnVehicleSleep);
			if (_vc.enabled)
			{
				OnVehicleWake();
			}
			else
			{
				OnVehicleSleep();
			}
		}

		private void OnVehicleWake()
		{
			base.gameObject.SetActive(value: true);
		}

		private void OnVehicleSleep()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
