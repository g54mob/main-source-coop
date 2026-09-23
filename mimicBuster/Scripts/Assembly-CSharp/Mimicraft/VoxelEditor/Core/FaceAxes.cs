using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class FaceAxes
	{
		public const int FaceCount = 6;

		public static readonly Vector3Int[] Normals = new Vector3Int[6]
		{
			new Vector3Int(0, 0, -1),
			new Vector3Int(0, 0, 1),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(1, 0, 0)
		};

		public static int IndexOf(Vector3Int normal)
		{
			for (int i = 0; i < Normals.Length; i++)
			{
				if (Normals[i] == normal)
				{
					return i;
				}
			}
			return -1;
		}

		public static void GetBasis(Vector3Int faceNormal, out Vector3Int right, out Vector3Int up)
		{
			if (faceNormal.x != 0)
			{
				right = new Vector3Int(0, 0, 1);
				up = new Vector3Int(0, 1, 0);
			}
			else if (faceNormal.y != 0)
			{
				right = new Vector3Int(1, 0, 0);
				up = new Vector3Int(0, 0, 1);
			}
			else
			{
				right = new Vector3Int(1, 0, 0);
				up = new Vector3Int(0, 1, 0);
			}
		}
	}
}
