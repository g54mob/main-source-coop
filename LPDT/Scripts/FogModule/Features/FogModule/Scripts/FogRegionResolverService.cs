using System.Collections.Generic;
using UnityEngine;

namespace Features.FogModule.Scripts
{
	public class FogRegionResolverService
	{
		public bool TryResolve(IReadOnlyList<FogRegion> fogRegions, Vector3 worldPosition, out FogBlend fogBlend)
		{
			fogBlend = default(FogBlend);
			bool flag = false;
			int num = 0;
			for (int i = 0; i < fogRegions.Count; i++)
			{
				FogRegion fogRegion = fogRegions[i];
				if ((!flag || fogRegion.Priority > num) && fogRegion.TryEvaluate(worldPosition, out var fogBlend2) && fogBlend2.IsValid)
				{
					fogBlend = fogBlend2;
					num = fogRegion.Priority;
					flag = true;
				}
			}
			return flag;
		}
	}
}
