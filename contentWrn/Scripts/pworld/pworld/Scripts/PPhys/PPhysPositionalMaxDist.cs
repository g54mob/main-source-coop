using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PPhysPositionalMaxDist : PPhysPositional
	{
		public Transform root;

		public float maxDist;

		private Vector3 target;

		public override Vector3 Target
		{
			get
			{
				Vector3 vector = Vector3.ClampMagnitude(transTarget.position - root.position, maxDist);
				return root.position + vector;
			}
			set
			{
				transTarget.transform.position = value;
			}
		}
	}
}
