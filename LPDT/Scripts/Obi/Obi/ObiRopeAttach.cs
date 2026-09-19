using UnityEngine;

namespace Obi
{
	public class ObiRopeAttach : MonoBehaviour
	{
		public ObiPathSmoother smoother;

		[Range(0f, 1f)]
		public float m;

		public void LateUpdate()
		{
			if (smoother != null && smoother.actor.isLoaded)
			{
				Transform transform = smoother.actor.solver.transform;
				ObiPathFrame sectionAt = smoother.GetSectionAt(m);
				base.transform.position = transform.TransformPoint(sectionAt.position);
				base.transform.rotation = transform.rotation * Quaternion.LookRotation(sectionAt.tangent, sectionAt.binormal);
			}
		}
	}
}
