using UnityEngine;

namespace Mirror.Examples.Common.Controllers.Player
{
	[AddComponentMenu("Network/Player Controller (Reliable)")]
	[RequireComponent(typeof(NetworkTransformReliable))]
	public class PlayerControllerReliable : PlayerControllerBase
	{
		public override bool Weaved()
		{
			return true;
		}
	}
}
