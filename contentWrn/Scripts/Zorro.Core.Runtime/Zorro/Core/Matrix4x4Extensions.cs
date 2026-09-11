using Unity.Mathematics;
using UnityEngine;

namespace Zorro.Core
{
	public static class Matrix4x4Extensions
	{
		public static float3 TransformDirection(this Matrix4x4 localToWorld, float3 direction)
		{
			return (Vector3)(localToWorld * (Vector3)direction);
		}
	}
}
