using System;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	[Serializable]
	public class ObiArmResolutionAuthorityConfiguration
	{
		[SerializeField]
		private ObiArmResolutionTierConfiguration _lowTier;

		[SerializeField]
		private ObiArmResolutionTierConfiguration _highTier;

		public ObiArmResolutionTierConfiguration LowTier => _lowTier;

		public ObiArmResolutionTierConfiguration HighTier => _highTier;
	}
}
