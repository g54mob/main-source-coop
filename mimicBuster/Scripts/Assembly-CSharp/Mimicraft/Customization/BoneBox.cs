using UnityEngine;

namespace Mimicraft.Customization
{
	public readonly struct BoneBox
	{
		public readonly Vector3 Center;

		public readonly Vector3 Size;

		public readonly Quaternion Rotation;

		public readonly bool Oriented;

		public Vector3 AxisAlignedSize
		{
			get
			{
				if (!Oriented)
				{
					return Size;
				}
				Vector3 vector = Size * 0.5f;
				Vector3 vector2 = Rotation * new Vector3(vector.x, 0f, 0f);
				Vector3 vector3 = Rotation * new Vector3(0f, vector.y, 0f);
				Vector3 vector4 = Rotation * new Vector3(0f, 0f, vector.z);
				return 2f * new Vector3(Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x) + Mathf.Abs(vector4.x), Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y) + Mathf.Abs(vector4.y), Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z) + Mathf.Abs(vector4.z));
			}
		}

		public BoneBox(Vector3 center, Vector3 size, Quaternion rotation, bool oriented)
		{
			Center = center;
			Size = size;
			Rotation = rotation;
			Oriented = oriented;
		}
	}
}
