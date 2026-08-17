using System.Collections.Generic;
using System.Linq;
using NWH.Common.Cameras;
using NWH.Common.CoM;
using NWH.Common.Utility;
using NWH.Common.Vehicles;
using NWH.VehiclePhysics2.Modules.MotorcycleModule;
using NWH.VehiclePhysics2.Powertrain;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using NWH.WheelController3D;
using UnityEngine;

namespace NWH.VehiclePhysics2.SetupWizard
{
	public class VehicleSetupWizard : MonoBehaviour
	{
		public enum WheelControllerType
		{
			WheelController3D = 0,
			UnityWheelCollider = 1
		}

		[Tooltip("    Should a default vehicle camera and camera changer be added?")]
		public bool addCamera = true;

		public WheelControllerType wheelControllerType;

		[Tooltip("    Should character enter/exit points be added?")]
		public bool addCharacterEnterExitPoints = true;

		[Tooltip("    Wheel GameObjects in order: front-left, front-right, rear-left, rear-right, etc.")]
		public List<GameObject> wheelGameObjects = new List<GameObject>();

		public bool removeWizardOnComplete = true;

		public VehicleSetupWizardPreset preset;

		public static VehicleController RunSetup(GameObject targetGO, List<GameObject> wheelGOs, WheelControllerType wheelControllerType, bool addCamera = true, bool addCharacterEnterExitPoints = true)
		{
			Debug.Log("======== VEHICLE SETUP START ========");
			Transform transform = targetGO.transform;
			if (transform.localScale != Vector3.one)
			{
				Debug.LogError("Scale of a parent object should be [1,1,1] for Rigidbody and VehicleController to function properly.");
				return null;
			}
			targetGO.tag = "Vehicle";
			if (targetGO.GetComponentsInChildren<Collider>().Length == 0)
			{
				Debug.LogError("No colliders present on the vehicle. Attach at least one collider (BoxCollider, MeshCollider, etc.)!");
				return null;
			}
			Debug.Log("Adding Rigidbody to " + targetGO.name);
			Rigidbody rigidbody = targetGO.gameObject.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = targetGO.gameObject.AddComponent<Rigidbody>();
				if (rigidbody == null)
				{
					Debug.LogError("Failed to add a Rigidbody. Make sure the Rigidbody is ");
				}
			}
			rigidbody.mass = 1400f;
			rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			rigidbody.ResetCenterOfMass();
			rigidbody.ResetInertiaTensor();
			foreach (GameObject wheelGO in wheelGOs)
			{
				string text = wheelGO.name + "_WheelController";
				Debug.Log("Creating new wheel controller object " + text);
				GameObject gameObject = new GameObject(text);
				gameObject.tag = "Wheel";
				gameObject.transform.SetParent(transform);
				gameObject.transform.SetPositionAndRotation(wheelGO.transform.position, transform.rotation);
				Debug.Log("   |-> Adding WheelController to " + gameObject.name);
				WheelUAPI wheelUAPI;
				if (wheelControllerType == WheelControllerType.UnityWheelCollider)
				{
					(wheelUAPI = gameObject.AddComponent<WheelColliderUAPI>()).WheelVisual = wheelGO;
					WheelCollider component = gameObject.GetComponent<WheelCollider>();
					if (component != null)
					{
						JointSpring suspensionSpring = gameObject.GetComponent<WheelCollider>().suspensionSpring;
						suspensionSpring.targetPosition = 0.3f;
						component.suspensionSpring = suspensionSpring;
						component.wheelDampingRate = 0.1f;
					}
				}
				else
				{
					WheelController wheelController = gameObject.AddComponent<WheelController>();
					wheelController.FindOrSpawnVisualContainers();
					wheelUAPI = wheelController;
					wheelGO.transform.SetParent(wheelUAPI.WheelVisual.transform);
					wheelGO.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				}
				MeshRenderer component2 = wheelGO.GetComponent<MeshRenderer>();
				if (component2 != null)
				{
					float y = component2.bounds.extents.y;
					float num = component2.bounds.extents.x * 2f;
					if (y < 0.05f || y > 1f)
					{
						Debug.LogWarning($"Detected unusual wheel radius of {y}. Adjust WheelController's radius field manually.");
					}
					else
					{
						Debug.Log($"   |-> Setting radius to {y}");
						wheelUAPI.Radius = y;
					}
					if (num < 0.02f || num > 1f || num > y)
					{
						Debug.LogWarning($"Detected unusual wheel width of {num}. Adjust WheelController's width field manually.");
						continue;
					}
					Debug.Log($"   |-> Setting width to {num}");
					wheelUAPI.Width = num;
				}
				else
				{
					Debug.LogWarning("Radius and width could not be auto configured. Wheel " + wheelGO.name + " does not contain a MeshFilter.");
				}
			}
			VehicleController vehicleController = targetGO.GetComponent<VehicleController>();
			if (vehicleController == null)
			{
				Debug.Log("Adding VehicleController to " + targetGO.name);
				vehicleController = targetGO.AddComponent<VehicleController>();
				vehicleController.SetDefaults();
			}
			rigidbody.centerOfMass = vehicleController.transform.InverseTransformPoint(CalculateCenterOfMass(vehicleController));
			if (addCamera)
			{
				Debug.Log("Adding Cameras.");
				GameObject gameObject2 = new GameObject("Cameras");
				gameObject2.transform.SetParent(transform);
				Debug.Log("Adding a camera follow.");
				GameObject obj = new GameObject("Vehicle Camera");
				obj.transform.SetParent(gameObject2.transform);
				Transform transform2 = vehicleController.transform;
				obj.transform.SetPositionAndRotation(transform2.position, transform2.rotation);
				obj.AddComponent<Camera>().fieldOfView = 80f;
				obj.AddComponent<AudioListener>();
				CameraMouseDrag cameraMouseDrag = obj.AddComponent<CameraMouseDrag>();
				cameraMouseDrag.target = vehicleController.transform;
				cameraMouseDrag.tag = "MainCamera";
				gameObject2.AddComponent<CameraChanger>();
			}
			if (addCharacterEnterExitPoints)
			{
				Debug.Log("Adding enter/exit points.");
				GameObject obj2 = new GameObject("LeftEnterExitPoint");
				GameObject gameObject3 = new GameObject("RightEnterExitPoint");
				obj2.transform.SetParent(transform);
				gameObject3.transform.SetParent(transform);
				obj2.transform.position = transform.position - transform.right;
				gameObject3.transform.position = transform.position + transform.right;
				obj2.tag = "EnterExitPoint";
				gameObject3.tag = "EnterExitPoint";
			}
			Debug.Log("Validating setup.");
			vehicleController.Validate();
			Debug.Log("Setup done.");
			Debug.Log("======== VEHICLE SETUP END ========");
			return vehicleController;
		}

		public static void RunConfiguration(VehicleController targetVC, VehicleSetupWizardPreset preset)
		{
			Debug.Log("=== RUNNING CONFIGURATION (" + preset.name + ") ===");
			if (preset == null)
			{
				Debug.LogError("Configuration can not be ran with null VehicleSetupWizardPreset.");
				return;
			}
			List<WheelUAPI> list = targetVC.GetComponentsInChildren<WheelUAPI>().ToList();
			if (list.Count == 0)
			{
				Debug.LogError("Vehicle does not have any wheels. Stopping configuration.");
				return;
			}
			if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.Motorcycle && targetVC.powertrain.wheels.Count != 2)
			{
				Debug.LogError("Wheel count for a motorcycle needs to be 2, in order: front, rear.");
				return;
			}
			targetVC.GetComponent<Rigidbody>().mass = preset.mass;
			VariableCenterOfMass component = targetVC.GetComponent<VariableCenterOfMass>();
			if ((bool)component)
			{
				component.baseMass = preset.mass;
				if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.Motorcycle)
				{
					component.useDefaultInertia = false;
					component.inertiaTensor = new Vector3(200f, 200f, 200f);
				}
			}
			if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.SemiTruck)
			{
				targetVC.stateSettings = Resources.Load("NWH Vehicle Physics 2/Defaults/SemiTruckStateSettings") as StateSettings;
			}
			else if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.Trailer)
			{
				targetVC.stateSettings = Resources.Load("NWH Vehicle Physics 2/Defaults/TrailerStateSettings") as StateSettings;
			}
			else
			{
				targetVC.stateSettings = Resources.Load("NWH Vehicle Physics 2/Defaults/DefaultStateSettings") as StateSettings;
			}
			EngineComponent engine = targetVC.powertrain.engine;
			ClutchComponent clutch = targetVC.powertrain.clutch;
			TransmissionComponent transmission = targetVC.powertrain.transmission;
			float engineInertiaBasedOnVehicleType = GetEngineInertiaBasedOnVehicleType(preset.vehicleType);
			float engineMaxRPM = preset.engineMaxRPM;
			float idleRPM = engineMaxRPM * 0.15f;
			float engagementRPM = engineMaxRPM * 0.2f;
			float engagementRange = engineMaxRPM * 0.05f;
			float downshiftRPM = engineMaxRPM * 0.3f;
			float upshiftRPM = engineMaxRPM * 0.7f;
			engine.inertia = engineInertiaBasedOnVehicleType;
			engine.idleRPM = idleRPM;
			engine.maxPower = preset.enginePower;
			engine.revLimiterRPM = preset.engineMaxRPM;
			engine.forcedInduction.useForcedInduction = preset.vehicleType == VehicleSetupWizardPreset.VehicleType.SportsCar;
			engine.UpdatePeakPowerAndTorque();
			float num = PowertrainComponent.PowerInKWToTorque(UnitConverter.RPMToAngularVelocity(engine.EstimatedPeakPowerRPM), engine.EstimatedPeakPower);
			clutch.inertia = engineInertiaBasedOnVehicleType * 0.4f;
			clutch.engagementRPM = engagementRPM;
			clutch.engagementRange = engagementRange;
			targetVC.powertrain.clutch.slipTorque = Mathf.Max(800f, num * 4f);
			transmission.inertia = engineInertiaBasedOnVehicleType * 0.2f;
			float num2 = ((preset.vehicleType == VehicleSetupWizardPreset.VehicleType.SemiTruck) ? 0.4f : 0.2f);
			targetVC.powertrain.transmission.shiftDuration = num2;
			targetVC.powertrain.transmission.postShiftBan = 0.3f + num2;
			targetVC.powertrain.transmission.UpshiftRPM = upshiftRPM;
			targetVC.powertrain.transmission.DownshiftRPM = downshiftRPM;
			float finalGearRatio = 6f * (targetVC.powertrain.wheels[0].wheelUAPI.Radius / 0.45f) * preset.transmissionGearing;
			targetVC.powertrain.transmission.finalGearRatio = finalGearRatio;
			if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.SportsCar)
			{
				targetVC.powertrain.transmission.gears = new List<float> { -3.79f, 0f, 3.08f, 2.19f, 1.63f, 1.29f, 1.03f, 0.84f, 0.66f };
			}
			else if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.SemiTruck)
			{
				targetVC.powertrain.transmission.gears = new List<float>
				{
					-8f, -11f, 0f, 25f, 18f, 13.2f, 10f, 7.9f, 5.5f, 4.7f,
					4.38f, 3.74f, 3.2f, 2.73f, 2.29f, 1.95f, 1.62f, 1.38f, 1.17f, 1f,
					0.86f, 0.73f
				};
			}
			targetVC.brakes.maxTorque = preset.mass * 1.4f;
			if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.Motorcycle)
			{
				targetVC.powertrain.differentials.Clear();
				targetVC.powertrain.transmission.Output = targetVC.powertrain.wheels[1];
			}
			else if (targetVC.powertrain.wheels.Count == 4 && targetVC.powertrain.differentials.Count == 3)
			{
				if (preset.drivetrainConfiguration == VehicleSetupWizardPreset.DrivetrainConfiguration.FWD)
				{
					targetVC.powertrain.differentials[2].biasAB = 0f;
				}
				else if (preset.drivetrainConfiguration == VehicleSetupWizardPreset.DrivetrainConfiguration.RWD)
				{
					targetVC.powertrain.differentials[2].biasAB = 1f;
				}
				if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.SportsCar)
				{
					targetVC.powertrain.differentials[1].powerStiffness = 1f;
					targetVC.powertrain.differentials[1].coastStiffness = 0.5f;
					targetVC.powertrain.differentials[1].slipTorque = 250f;
				}
				else if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.OffRoad || preset.vehicleType == VehicleSetupWizardPreset.VehicleType.MonsterTruck)
				{
					targetVC.powertrain.differentials[1].powerStiffness = 1f;
					targetVC.powertrain.differentials[1].coastStiffness = 1f;
					targetVC.powertrain.differentials[1].slipTorque = 5000f;
				}
			}
			float num3 = 0.35f;
			float num4 = preset.mass * 50f / (float)list.Count;
			float num5 = Mathf.Sqrt(num4) * 25f;
			num3 *= preset.suspensionTravelCoeff;
			num4 *= preset.suspensionStiffnessCoeff;
			num5 *= preset.suspensionStiffnessCoeff;
			num3 = Mathf.Clamp(num3, 0.1f, 1f);
			foreach (WheelUAPI item in list)
			{
				item.SpringMaxLength = num3;
				item.SpringMaxForce = num4;
				item.DamperReboundRate = num5;
				item.DamperBumpRate = num5;
				item.Mass = Mathf.Clamp(preset.mass / 1500f, 0.2f, 6f) * 20f;
				if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.Motorcycle)
				{
					item.Width = 0.03f;
					item.LateralFrictionGrip = 1.4f;
					item.LongitudinalFrictionGrip = 1.2f;
				}
			}
			foreach (WheelGroup wheelGroup in targetVC.powertrain.wheelGroups)
			{
				if (wheelGroup.steerCoefficient == 0f)
				{
					wheelGroup.addAckerman = false;
				}
				if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.Motorcycle)
				{
					wheelGroup.addAckerman = false;
					wheelGroup.ToeAngle = 0f;
				}
			}
			AudioClip clip = ((preset.vehicleType == VehicleSetupWizardPreset.VehicleType.SportsCar) ? (Resources.Load(GetResourcePath("Sounds/SportsCar")) as AudioClip) : ((preset.vehicleType == VehicleSetupWizardPreset.VehicleType.MonsterTruck) ? (Resources.Load(GetResourcePath("Sounds/MuscleCar")) as AudioClip) : ((preset.vehicleType != VehicleSetupWizardPreset.VehicleType.SemiTruck) ? (Resources.Load(GetResourcePath("Sounds/Car")) as AudioClip) : (Resources.Load(GetResourcePath("Sounds/SemiTruck")) as AudioClip))));
			targetVC.soundManager.engineRunningComponent.Clip = clip;
			targetVC.soundManager.engineRunningComponent.pitchRange = targetVC.powertrain.engine.revLimiterRPM / 2500f;
			if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.Motorcycle)
			{
				targetVC.gameObject.AddComponent<MotorcycleModuleWrapper>();
			}
			if (preset.vehicleType == VehicleSetupWizardPreset.VehicleType.Motorcycle)
			{
				targetVC.GetComponent<Rigidbody>().inertiaTensor = new Vector3(200f, 200f, 200f);
			}
			Debug.Log("Vehicle configured using " + preset.name + " preset.");
			Debug.Log("======== VEHICLE CONFIGURATION SUCCESS ========");
		}

		private static float GetEngineInertiaBasedOnVehicleType(VehicleSetupWizardPreset.VehicleType vehicleType)
		{
			return vehicleType switch
			{
				VehicleSetupWizardPreset.VehicleType.SemiTruck => 1.5f, 
				VehicleSetupWizardPreset.VehicleType.SportsCar => 0.2f, 
				VehicleSetupWizardPreset.VehicleType.Motorcycle => 0.15f, 
				_ => 0.3f, 
			};
		}

		private static string GetResourcePath(string name)
		{
			return "NWH Vehicle Physics 2/VehicleSetupWizard/" + name;
		}

		private static TransmissionGearingProfile LoadGearingProfile(string name)
		{
			return Resources.Load("NWH Vehicle Physics 2/VehicleSetupWizard/GearingProfile/" + name) as TransmissionGearingProfile;
		}

		private static Vector3 CalculateCenterOfMass(VehicleController vc)
		{
			Vector3 result = Vector3.zero;
			Vector3 zero = Vector3.zero;
			int num = 0;
			WheelUAPI[] componentsInChildren = vc.gameObject.GetComponentsInChildren<WheelUAPI>();
			foreach (WheelUAPI wheelUAPI in componentsInChildren)
			{
				zero += wheelUAPI.transform.position;
				num++;
			}
			if (num > 0)
			{
				result = zero / num;
			}
			return result;
		}
	}
}
