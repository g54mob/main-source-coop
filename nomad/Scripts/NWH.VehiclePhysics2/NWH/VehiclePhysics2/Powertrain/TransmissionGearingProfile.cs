using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWH.VehiclePhysics2.Powertrain
{
	[Serializable]
	[CreateAssetMenu(fileName = "NWH Vehicle Physics 2", menuName = "NWH/Vehicle Physics 2/Gearing Profile", order = 1)]
	public class TransmissionGearingProfile : ScriptableObject
	{
		[SerializeField]
		[Tooltip("    List of forward gear ratios starting from 1st forward gear.")]
		public List<float> forwardGears = new List<float> { 8f, 5.5f, 4f, 3f, 2.2f, 1.7f, 1.3f };

		[SerializeField]
		[Tooltip("    List of reverse gear ratios starting from 1st reverse gear.")]
		public List<float> reverseGears = new List<float> { -5f };
	}
}
