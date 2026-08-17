using UnityEngine;

namespace Mirror.Examples.Common.Controllers.Flyer
{
	[AddComponentMenu("Network/Flyer Controller (Unreliable)")]
	[RequireComponent(typeof(NetworkTransformUnreliable))]
	public class FlyerControllerUnreliable : FlyerControllerBase
	{
		public override bool Weaved()
		{
			return true;
		}
	}
}
