using System;
using System.Collections.Generic;
using System.Linq;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using UnityEngine;

namespace NWH.VehiclePhysics2.Powertrain
{
	[Serializable]
	public class Powertrain : ManagerVehicleComponent
	{
		public ClutchComponent clutch = new ClutchComponent();

		public List<DifferentialComponent> differentials = new List<DifferentialComponent>();

		public EngineComponent engine = new EngineComponent();

		public TransmissionComponent transmission = new TransmissionComponent();

		public List<WheelGroup> wheelGroups = new List<WheelGroup>();

		public List<WheelComponent> wheels = new List<WheelComponent>();

		public int wheelGroupCount => wheelGroups.Count;

		public int wheelCount => wheels.Count;

		public override void VC_SetVehicleController(VehicleController vc)
		{
			base.VC_SetVehicleController(vc);
			wheelGroups.ForEach(delegate(WheelGroup w)
			{
				w.vc = vc;
			});
		}

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (vehicleController.input.InputSwappedThrottle > 0.02f)
			{
				for (int i = 0; i < wheelCount; i++)
				{
					wheels[i].wheelUAPI.WakeFromSleep();
				}
			}
			for (int j = 0; j < wheelGroupCount; j++)
			{
				wheelGroups[j].Update();
			}
			engine.IntegrateDownwards(vehicleController.fixedDeltaTime);
		}

		public override void VC_SetDefaults()
		{
			engine.name = "Engine";
			clutch.name = "Clutch";
			transmission.name = "Transmission";
			base.VC_SetDefaults();
			wheels = new List<WheelComponent>();
			WheelUAPI[] componentsInChildren = vehicleController.GetComponentsInChildren<WheelUAPI>();
			foreach (WheelUAPI wheelUAPI in componentsInChildren)
			{
				Debug.Log("VehicleController setup: Found wheel '" + wheelUAPI.transform.name + "'");
				WheelComponent wheelComponent = new WheelComponent();
				wheelComponent.name = "Wheel" + wheelUAPI.transform.name;
				wheelComponent.wheelUAPI = wheelUAPI;
				wheels.Add(wheelComponent);
			}
			if (wheels.Count == 0)
			{
				Debug.LogWarning("No WheelControllers found, skipping powertrain auto-setup.");
				return;
			}
			wheels = wheels.OrderByDescending((WheelComponent w) => w.wheelUAPI.transform.localPosition.z).ToList();
			List<int> list = new List<int>();
			int num = 1;
			float num2 = wheels[0].wheelUAPI.transform.localPosition.z;
			for (int num3 = 0; num3 < wheels.Count; num3++)
			{
				float z = wheels[num3].wheelUAPI.transform.localPosition.z;
				if (Mathf.Abs(z - num2) > 0.2f)
				{
					num++;
				}
				else if (num3 > 0 && wheels[num3].wheelUAPI.transform.localPosition.x < wheels[num3 - 1].wheelUAPI.transform.localPosition.x)
				{
					List<WheelComponent> list2 = wheels;
					int index = num3 - 1;
					List<WheelComponent> list3 = wheels;
					int index2 = num3;
					WheelComponent wheelComponent2 = wheels[num3];
					WheelComponent wheelComponent3 = wheels[num3 - 1];
					WheelComponent wheelComponent4 = (list2[index] = wheelComponent2);
					wheelComponent4 = (list3[index2] = wheelComponent3);
				}
				list.Add(num - 1);
				num2 = z;
			}
			wheelGroups = new List<WheelGroup>();
			for (int num4 = 0; num4 < num; num4++)
			{
				string arg = ((num4 == 0) ? "Front" : ((num4 == num - 1) ? "Rear" : "Middle"));
				string text = $"{arg} Axle {num4}";
				wheelGroups.Add(new WheelGroup
				{
					name = text,
					brakeCoefficient = ((num4 == 0 || num > 2) ? 1f : 0.7f),
					handbrakeCoefficient = ((num4 == num - 1) ? 1f : 0f),
					steerCoefficient = ((num4 == 0) ? 1f : ((num4 == 1 && num > 2) ? 0.5f : 0f)),
					addAckerman = true,
					isSolid = false
				});
				Debug.Log("VehicleController setup: Creating WheelGroup '" + text + "'");
			}
			differentials = new List<DifferentialComponent>();
			Debug.Log("[Powertrain] Adding 'Front Differential'");
			differentials.Add(new DifferentialComponent
			{
				name = "Front Differential"
			});
			Debug.Log("[Powertrain] Adding 'Rear Differential'");
			differentials.Add(new DifferentialComponent
			{
				name = "Rear Differential"
			});
			Debug.Log("[Powertrain] Adding 'Center Differential'");
			differentials.Add(new DifferentialComponent
			{
				name = "Center Differential"
			});
			differentials[2].Output = differentials[0];
			differentials[2].OutputB = differentials[1];
			Debug.Log("[Powertrain] Setting transmission output to '" + differentials[2].name + "'");
			transmission.Output = differentials[2];
			for (int num5 = 0; num5 < wheels.Count; num5++)
			{
				int index3 = list[num5];
				wheels[num5].wheelGroupSelector = new WheelGroupSelector
				{
					index = index3
				};
				Debug.Log("[Powertrain] Adding '" + wheels[num5].name + "' to '" + wheelGroups[index3].name + "'");
			}
			int count = wheelGroups.Count;
			count = ((num > 2) ? 2 : num);
			for (int num6 = 0; num6 < count; num6++)
			{
				List<WheelComponent> list4 = wheelGroups[num6].FindWheelsBelongingToGroup(ref wheels, num6);
				if (list4.Count == 2)
				{
					Debug.Log("[Powertrain] Setting output of '" + differentials[num6].name + "' to '" + list4[0].name + "'");
					if (list4[0].wheelUAPI.transform.position.x < -0.01f)
					{
						differentials[num6].Output = list4[0];
						differentials[num6].OutputB = list4[1];
					}
					else if (list4[0].wheelUAPI.transform.position.x > 0.01f)
					{
						differentials[num6].Output = list4[1];
						differentials[num6].OutputB = list4[0];
					}
					else
					{
						Debug.LogWarning("[Powertrain] Powertrain settings for center wheels have to be manually set up. If powered either connect it directly to transmission (motorcycle) or to one side of center differential (trike).");
					}
				}
			}
			FillComponentList();
		}

		public override void VC_Validate(VehicleController vc)
		{
			base.VC_Validate(vc);
			if (state.isEnabled)
			{
				engine.VC_Validate(vc);
				clutch.VC_Validate(vc);
				transmission.VC_Validate(vc);
				differentials.ForEach(delegate(DifferentialComponent diff)
				{
					diff.VC_Validate(vc);
				});
				wheels.ForEach(delegate(WheelComponent wheel)
				{
					wheel.VC_Validate(vc);
				});
			}
		}

		public void Repair()
		{
			engine.Damage = 0f;
			clutch.Damage = 0f;
			transmission.Damage = 0f;
			differentials.ForEach(delegate(DifferentialComponent diff)
			{
				diff.Damage = 0f;
			});
			wheels.ForEach(delegate(WheelComponent wheel)
			{
				wheel.Damage = 0f;
			});
		}

		public List<string> Inspector_GetPowertrainComponentNames()
		{
			List<string> list = new List<string>();
			list.Add("[none]");
			list.Add(engine.name);
			list.Add(clutch.name);
			list.Add(transmission.name);
			list.AddRange(differentials.Select((DifferentialComponent diff) => diff.name));
			list.AddRange(wheels.Select((WheelComponent wheel) => wheel.name));
			return list;
		}

		public List<PowertrainComponent> Inspector_GetPowertrainComponents()
		{
			List<PowertrainComponent> list = new List<PowertrainComponent>();
			list.Add(null);
			list.Add(engine);
			list.Add(clutch);
			list.Add(transmission);
			list.AddRange(differentials);
			list.AddRange(wheels);
			return list;
		}

		public PowertrainComponent Inspector_GetPowertrainComponentFromNameHash(int nameHash)
		{
			return Inspector_GetPowertrainComponents().FirstOrDefault((PowertrainComponent c) => c != null && c.name.GetHashCode() == nameHash);
		}

		protected override void VC_Initialize()
		{
			wheelGroups.ForEach(delegate(WheelGroup w)
			{
				w.Initialize();
			});
			base.VC_Initialize();
		}

		protected override void FillComponentList()
		{
			_components = new List<VehicleComponent>();
			_components.Add(engine);
			_components.Add(clutch);
			_components.Add(transmission);
			_components.AddRange(differentials);
			_components.AddRange(wheels);
		}
	}
}
