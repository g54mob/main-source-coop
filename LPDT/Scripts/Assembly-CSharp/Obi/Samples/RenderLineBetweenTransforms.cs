using UnityEngine;

namespace Obi.Samples
{
	[RequireComponent(typeof(LineRenderer))]
	public class RenderLineBetweenTransforms : MonoBehaviour
	{
		public Transform transformA;

		public Transform transformB;

		private LineRenderer line;

		private void Awake()
		{
			line = GetComponent<LineRenderer>();
		}

		private void Update()
		{
			if (transformA != null && transformB != null)
			{
				line.SetPositions(new Vector3[2] { transformA.position, transformB.position });
			}
		}
	}
}
