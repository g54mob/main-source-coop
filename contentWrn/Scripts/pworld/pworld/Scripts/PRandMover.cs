using UnityEngine;
using pworld.Scripts.Extensions;

namespace pworld.Scripts
{
	public class PRandMover : MonoBehaviour
	{
		public float time;

		public Vector3 center;

		public float range;

		private float elapsed;

		private void Start()
		{
		}

		private void Update()
		{
			if (elapsed > time)
			{
				base.transform.position = center + (Random.insideUnitCircle * Random.Range(0f, range)).PToVec3xoy();
				elapsed = 0f;
			}
			elapsed += Time.deltaTime;
		}
	}
}
