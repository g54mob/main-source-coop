using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NWH.VehiclePhysics2.SetupWizard
{
	[Serializable]
	[CreateAssetMenu(fileName = "NWH Vehicle Physics 2", menuName = "NWH/Vehicle Physics 2/Vehicle Setup Wizard Preset", order = 1)]
	public class VehicleSetupWizardPreset : ScriptableObject
	{
		public enum VehicleType
		{
			Car = 0,
			SportsCar = 1,
			OffRoad = 2,
			MonsterTruck = 3,
			SemiTruck = 4,
			Trailer = 5,
			Motorcycle = 6
		}

		public enum DrivetrainConfiguration
		{
			FWD = 0,
			AWD = 1,
			RWD = 2
		}

		[Tooltip("General")]
		public VehicleType vehicleType;

		[Tooltip("Physical properties")]
		public float mass = 1500f;

		public float width = 1.8f;

		public float length = 4.5f;

		public float height = 1.4f;

		[Range(10f, 600f)]
		[Tooltip("Engine")]
		public float enginePower = 110f;

		public float engineMaxRPM = 6000f;

		[Tooltip("Transmission")]
		public float transmissionGearing = 1f;

		[Tooltip("Drivetrain")]
		public DrivetrainConfiguration drivetrainConfiguration = DrivetrainConfiguration.RWD;

		[FormerlySerializedAs("suspensionTravel")]
		public float suspensionTravelCoeff = 1f;

		[FormerlySerializedAs("suspensionStiffness")]
		public float suspensionStiffnessCoeff = 1f;
	}
}
