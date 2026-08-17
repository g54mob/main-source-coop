using UnityEngine;

namespace Coffee.UIEffectInternal
{
	internal static class TransformExtensionsForTransformSensitivity
	{
		private const float k_DefaultEpsilon = 0.1f;

		public static bool HasChanged(this Transform self, ref Matrix4x4 prev, TransformSensitivity sensitivity)
		{
			return self.HasChanged_Internal(null, ref prev, Convert(sensitivity));
		}

		public static bool HasChanged(this Transform self, Transform baseTransform, ref Matrix4x4 prev, TransformSensitivity sensitivity)
		{
			return self.HasChanged_Internal(baseTransform, ref prev, Convert(sensitivity));
		}

		private static float Convert(TransformSensitivity self)
		{
			return self switch
			{
				TransformSensitivity.Low => 0.0625f, 
				TransformSensitivity.Medium => 0.00390625f, 
				TransformSensitivity.High => 0.00024414062f, 
				_ => 1f / (float)(1 << (int)self), 
			};
		}

		private static bool HasChanged_Internal(this Transform self, Transform baseTransform, ref Matrix4x4 prev, float epsilon)
		{
			if (!self)
			{
				return false;
			}
			int key = (baseTransform ? baseTransform.GetHashCode() : 0);
			if (FrameCache.TryGet<bool>(self, "HasChanged", key, out var result))
			{
				return result;
			}
			Matrix4x4 matrix4x = (baseTransform ? (baseTransform.worldToLocalMatrix * self.localToWorldMatrix) : self.localToWorldMatrix) * Matrix4x4.Scale(Vector3.one * 10000f);
			result = !Approximately(matrix4x, prev, epsilon);
			FrameCache.Set(self, "HasChanged", key, result);
			if (result)
			{
				prev = matrix4x;
			}
			return result;
		}

		private static bool Approximately(Matrix4x4 self, Matrix4x4 other, float epsilon = 0.1f)
		{
			for (int i = 0; i < 16; i++)
			{
				if (epsilon < Mathf.Abs(self[i] - other[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
