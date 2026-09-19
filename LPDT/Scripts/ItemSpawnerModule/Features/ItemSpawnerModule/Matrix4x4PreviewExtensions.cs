using UnityEngine;

namespace Features.ItemSpawnerModule
{
	public static class Matrix4x4PreviewExtensions
	{
		public static void DecomposeTRS(this Matrix4x4 matrix, out Vector3 position, out Quaternion rotation, out Vector3 scale)
		{
			position = matrix.GetColumn(3);
			rotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
			scale = new Vector3(matrix.GetColumn(0).magnitude, matrix.GetColumn(1).magnitude, matrix.GetColumn(2).magnitude);
		}
	}
}
