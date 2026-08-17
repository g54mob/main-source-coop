using System.Collections;
using NWH.Common.Vehicles;
using UnityEngine;
using UnityEngine.UI;

namespace NWH.Common.Demo
{
	[RequireComponent(typeof(Text))]
	public class DemoVehicleNameDisplay : MonoBehaviour
	{
		private Text vehicleText;

		private void Awake()
		{
			vehicleText = GetComponent<Text>();
			StartCoroutine(VehicleNameCoroutine());
		}

		private IEnumerator VehicleNameCoroutine()
		{
			while (true)
			{
				Vehicle activeVehicle = Vehicle.ActiveVehicle;
				if (activeVehicle != null)
				{
					vehicleText.text = activeVehicle.name + " [" + activeVehicle.GetType().Name + "]";
				}
				else
				{
					vehicleText.text = "[no active vehicle]";
				}
				yield return new WaitForSeconds(0.1f);
			}
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
		}
	}
}
