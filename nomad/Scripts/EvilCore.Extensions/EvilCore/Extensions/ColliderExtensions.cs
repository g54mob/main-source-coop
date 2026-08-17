using UnityEngine;

namespace EvilCore.Extensions
{
	public static class ColliderExtensions
	{
		private static readonly Collider[] OverlapCache = new Collider[32];

		public static bool GetPenetrationsInLayer(this Collider source, LayerMask mask, out Vector3 totalCorrection)
		{
			totalCorrection = Vector3.zero;
			if (source == null)
			{
				return false;
			}
			int num = Physics.OverlapBoxNonAlloc(source.bounds.center, source.bounds.extents, OverlapCache, source.transform.rotation, mask);
			bool result = false;
			for (int i = 0; i < num; i++)
			{
				Collider collider = OverlapCache[i];
				if (!(collider == source) && source.ComputePenetration(collider, out var direction, out var distance))
				{
					result = true;
					totalCorrection += direction * distance;
				}
			}
			return result;
		}

		public static bool GetPenetrationsInLayer(this Collider source, LayerMask mask, Collider[] ignoreColliders, out Vector3 totalCorrection)
		{
			totalCorrection = Vector3.zero;
			if (source == null)
			{
				return false;
			}
			int num = Physics.OverlapBoxNonAlloc(source.bounds.center, source.bounds.extents, OverlapCache, source.transform.rotation, mask);
			bool result = false;
			for (int i = 0; i < num; i++)
			{
				Collider collider = OverlapCache[i];
				if (!(collider == source) && (ignoreColliders == null || !IsInIgnoreList(collider, ignoreColliders)) && source.ComputePenetration(collider, out var direction, out var distance))
				{
					result = true;
					totalCorrection += direction * distance;
				}
			}
			return result;
		}

		public static bool GetPenetrationsInLayerUnified(this Collider[] sourceColliders, LayerMask mask, out Vector3 correction)
		{
			return sourceColliders.GetPenetrationsInLayerUnified(mask, null, out correction);
		}

		public static bool GetPenetrationsInLayerUnified(this Collider[] sourceColliders, LayerMask mask, Collider[] additionalIgnoreColliders, out Vector3 correction)
		{
			correction = Vector3.zero;
			if (sourceColliders == null || sourceColliders.Length == 0)
			{
				return false;
			}
			Bounds bounds = sourceColliders[0].bounds;
			for (int i = 1; i < sourceColliders.Length; i++)
			{
				if (sourceColliders[i] != null)
				{
					bounds.Encapsulate(sourceColliders[i].bounds);
				}
			}
			int num = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, OverlapCache, Quaternion.identity, mask);
			if (num == 0)
			{
				return false;
			}
			Vector3 zero = Vector3.zero;
			bool result = false;
			for (int j = 0; j < num; j++)
			{
				Collider collider = OverlapCache[j];
				if (collider == null || IsInIgnoreList(collider, sourceColliders) || (additionalIgnoreColliders != null && IsInIgnoreList(collider, additionalIgnoreColliders)))
				{
					continue;
				}
				Vector3 vector = Vector3.zero;
				float num2 = 0f;
				for (int k = 0; k < sourceColliders.Length; k++)
				{
					if (!(sourceColliders[k] == null) && sourceColliders[k].ComputePenetration(collider, out var direction, out var distance) && distance > num2)
					{
						num2 = distance;
						vector = direction;
					}
				}
				if (num2 > 0f)
				{
					result = true;
					zero += vector * num2;
				}
			}
			correction = zero;
			return result;
		}

		private static bool IsInIgnoreList(Collider collider, Collider[] ignoreList)
		{
			for (int i = 0; i < ignoreList.Length; i++)
			{
				if (ignoreList[i] == collider)
				{
					return true;
				}
			}
			return false;
		}

		private static bool ComputePenetration(this Collider source, Collider target, out Vector3 direction, out float distance)
		{
			direction = Vector3.zero;
			distance = 0f;
			if (source == null || target == null)
			{
				return false;
			}
			return Physics.ComputePenetration(source, source.transform.position, source.transform.rotation, target, target.transform.position, target.transform.rotation, out direction, out distance);
		}
	}
}
