using System;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	[Serializable]
	public class ArmControllerData
	{
		[field: SerializeField]
		public Arm Arm { get; set; }

		[field: SerializeField]
		public StrechArmController StretchArmController { get; set; }
	}
}
