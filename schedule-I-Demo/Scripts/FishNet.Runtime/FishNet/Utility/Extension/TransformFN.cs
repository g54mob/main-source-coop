using FishNet.Documenting;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Utility.Extension
{
	[APIExclude]
	public static class TransformFN
	{
		public static TransformProperties GetWorldProperties(this Transform t)
		{
			return new TransformProperties(t.position, t.rotation, t.localScale);
		}

		public static void SetWorldProperties(this TransformPropertiesCls tp, Transform t)
		{
			tp.Position = t.position;
			tp.Rotation = t.rotation;
			tp.LocalScale = t.localScale;
		}

		public static void SetTransformOffsets(this Transform t, Transform target, ref Vector3 pos, ref Quaternion rot)
		{
			if (!(target == null))
			{
				pos = target.position - t.position;
				rot = target.rotation * Quaternion.Inverse(t.rotation);
			}
		}

		public static TransformProperties GetTransformOffsets(this Transform t, Transform target)
		{
			if (target == null)
			{
				return default(TransformProperties);
			}
			return new TransformProperties(target.position - t.position, target.rotation * Quaternion.Inverse(t.rotation), target.localScale - t.localScale);
		}

		public static void SetLocalPositionAndRotation(this Transform t, Vector3 pos, Quaternion rot)
		{
			t.localPosition = pos;
			t.localRotation = rot;
		}

		public static void SetLocalPositionRotationAndScale(this Transform t, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			t.localPosition = pos;
			t.localRotation = rot;
			t.localScale = scale;
		}
	}
}
