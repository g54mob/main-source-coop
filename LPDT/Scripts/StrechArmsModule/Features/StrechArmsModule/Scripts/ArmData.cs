using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	[Serializable]
	public class ArmData
	{
		[field: SerializeField]
		public int PlayerId { get; set; }

		[field: SerializeField]
		public List<Arm> AvailableArms { get; set; }
	}
}
