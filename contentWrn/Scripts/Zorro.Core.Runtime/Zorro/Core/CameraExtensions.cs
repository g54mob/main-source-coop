using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core
{
	public static class CameraExtensions
	{
		public static Bounds2D Get2DBounds(this Camera self)
		{
			float3 float5 = self.transform.position;
			float orthographicSize = self.orthographicSize;
			return new Bounds2D
			{
				Center = float5.xy,
				HalfSize = new float2(self.aspect * orthographicSize, orthographicSize)
			};
		}

		public static AspectRatio GetAspectRatio(this Camera self)
		{
			return AspectRatioUtility.GetAspectRatio(self.aspect);
		}
	}
}
