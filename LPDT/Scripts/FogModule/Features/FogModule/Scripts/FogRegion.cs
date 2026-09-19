using System.Collections.Generic;
using UnityEngine;

namespace Features.FogModule.Scripts
{
	public class FogRegion
	{
		private readonly IReadOnlyList<FogRegionBox> _fogRegionBoxes;

		public FogRegionType RegionType { get; }

		public FogLocationType FogLocation { get; }

		public FogLocationType BlendTargetFogLocation { get; }

		public int Priority { get; }

		public FogRegion(FogRegionType regionType, FogLocationType fogLocation, FogLocationType blendTargetFogLocation, int priority, IReadOnlyList<FogRegionBox> fogRegionBoxes)
		{
			RegionType = regionType;
			FogLocation = fogLocation;
			BlendTargetFogLocation = blendTargetFogLocation;
			Priority = priority;
			_fogRegionBoxes = fogRegionBoxes;
		}

		public bool TryEvaluate(Vector3 worldPosition, out FogBlend fogBlend)
		{
			fogBlend = default(FogBlend);
			for (int i = 0; i < _fogRegionBoxes.Count; i++)
			{
				FogRegionBox fogRegionBox = _fogRegionBoxes[i];
				if (fogRegionBox.Contains(worldPosition))
				{
					fogBlend = ((RegionType == FogRegionType.Blend) ? new FogBlend(FogLocation, BlendTargetFogLocation, fogRegionBox.GetBlendRatio(worldPosition)) : new FogBlend(FogLocation, FogLocation, 0f));
					return true;
				}
			}
			return false;
		}
	}
}
