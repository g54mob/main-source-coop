using UnityEngine;
using UnityEngine.AI;

namespace Features.NavigationModule.Scripts
{
	public static class NavMeshPathExtensions
	{
		public static float GetLength(this NavMeshPath path)
		{
			if (path == null || path.corners == null || path.corners.Length < 2)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 1; i < path.corners.Length; i++)
			{
				num += Vector3.Distance(path.corners[i - 1], path.corners[i]);
			}
			return num;
		}
	}
}
