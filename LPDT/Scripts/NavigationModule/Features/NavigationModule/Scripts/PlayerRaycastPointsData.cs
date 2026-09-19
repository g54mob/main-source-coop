using System.Collections.Generic;
using UnityEngine;

namespace Features.NavigationModule.Scripts
{
	public class PlayerRaycastPointsData
	{
		public Dictionary<PlayerRaycastPoint, Transform> RaycastPoints = new Dictionary<PlayerRaycastPoint, Transform>();
	}
}
