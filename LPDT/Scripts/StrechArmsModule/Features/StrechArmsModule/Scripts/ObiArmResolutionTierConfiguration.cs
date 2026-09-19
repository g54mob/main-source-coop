using System;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	[Serializable]
	public class ObiArmResolutionTierConfiguration
	{
		[SerializeField]
		private int _substeps = 4;

		public int Substeps => _substeps;
	}
}
