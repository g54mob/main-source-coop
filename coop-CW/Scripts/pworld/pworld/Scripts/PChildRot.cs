using UnityEngine;

namespace pworld.Scripts
{
	public class PChildRot : MonoBehaviour
	{
		public Transform parent;

		private void Start()
		{
		}

		private void Update()
		{
			base.transform.forward = parent.forward;
		}
	}
}
