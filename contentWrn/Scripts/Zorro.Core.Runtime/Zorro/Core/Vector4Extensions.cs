using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core
{
	public static class Vector4Extensions
	{
		public static float2 xy(this Vector4 from)
		{
			return new float2(from.x, from.y);
		}
	}
}
