using System.Collections.Generic;
using UnityEngine;

namespace Features.NavigationModule.Scripts
{
	public class NavPoint
	{
		public Vector3 Position { get; set; }

		public List<NavPoint> Adjacent { get; set; }

		public NavPoint(Vector3 position, List<NavPoint> adjacent = null)
		{
			Position = position;
			Adjacent = adjacent ?? new List<NavPoint>();
		}

		public void AddAdjacent(NavPoint point)
		{
			Adjacent.Add(point);
		}
	}
}
