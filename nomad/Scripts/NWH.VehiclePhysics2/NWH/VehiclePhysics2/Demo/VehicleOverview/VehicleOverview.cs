using System.Collections.Generic;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using UnityEngine;

namespace NWH.VehiclePhysics2.Demo.VehicleOverview
{
	public class VehicleOverview : MonoBehaviour
	{
		public static VehicleOverview Instance;

		public VehicleController vc;

		public Transform parent;

		public GameObject wheelGroupPrefab;

		private readonly List<WheelGroupUI> wheelGroupUIs = new List<WheelGroupUI>();

		private VehicleController _prevVc;

		private void Initialize()
		{
			foreach (Transform item in parent)
			{
				if (!(item.name == "VehicleOverviewLegend"))
				{
					Object.Destroy(item.gameObject);
				}
			}
			foreach (WheelGroup wheelGroup in vc.powertrain.wheelGroups)
			{
				wheelGroupUIs.Add(InitGroupUI(wheelGroup));
			}
		}

		private void Awake()
		{
			Instance = this;
		}

		private void Update()
		{
			vc = Vehicle.ActiveVehicle as VehicleController;
			if (!(vc == null))
			{
				if (_prevVc != vc)
				{
					Initialize();
				}
				_prevVc = vc;
			}
		}

		private WheelGroupUI InitGroupUI(WheelGroup wheelGroup)
		{
			WheelGroupUI component = Object.Instantiate(wheelGroupPrefab, parent).GetComponent<WheelGroupUI>();
			component.Initialize(wheelGroup);
			return component;
		}

		private Color GetColorFromValue(float currentValue, float maxValue)
		{
			float num = Mathf.Clamp01(currentValue / maxValue);
			return new Color(num, num, num, 1f);
		}
	}
}
