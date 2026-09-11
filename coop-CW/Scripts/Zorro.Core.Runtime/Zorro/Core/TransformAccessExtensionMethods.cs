using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace Zorro.Core
{
	public static class TransformAccessExtensionMethods
	{
		public static float3 TransformDirection(this TransformAccess transformAccess, float3 direction)
		{
			return (Vector3)(transformAccess.localToWorldMatrix * (Vector3)direction);
		}

		public static float3 TransformDirection(this TransformAccess transformAccess, Vector3 direction)
		{
			return (Vector3)(transformAccess.localToWorldMatrix * direction);
		}

		public static float3 TransformPoint(this TransformAccess transformAccess, float3 direction)
		{
			return transformAccess.position + (Vector3)(transformAccess.localToWorldMatrix * (Vector3)direction);
		}

		public static float3 InverseTransformPoint(this TransformAccess transformAccess, float3 pos)
		{
			return (Vector3)(transformAccess.worldToLocalMatrix * (Vector3)pos);
		}

		public static float3 InverseTransformPoint(this TransformAccess transformAccess, Vector3 pos)
		{
			return (Vector3)(transformAccess.worldToLocalMatrix * pos);
		}
	}
}
