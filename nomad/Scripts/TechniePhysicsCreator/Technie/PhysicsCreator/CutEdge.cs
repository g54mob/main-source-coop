using UnityEngine;

namespace Technie.PhysicsCreator
{
	public class CutEdge
	{
		public Vector3 v0;

		public Vector3 v1;

		public float Length => Vector3.Distance(v0, v1);

		public CutEdge(Vector3 v0, Vector3 v1)
		{
			this.v0 = v0;
			this.v1 = v1;
		}

		public float CalcLength()
		{
			return Vector3.Distance(v0, v1);
		}
	}
}
