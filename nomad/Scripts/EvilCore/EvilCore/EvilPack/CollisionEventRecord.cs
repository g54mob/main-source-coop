using UnityEngine;

namespace EvilCore.EvilPack
{
	public struct CollisionEventRecord
	{
		public CollisionEventKind Kind;

		public Collider Source;

		public Collider Other;

		public Vector3 ContactPoint;

		public Vector3 Direction;

		public float Magnitude;

		public int Frame;

		public float Time;
	}
}
