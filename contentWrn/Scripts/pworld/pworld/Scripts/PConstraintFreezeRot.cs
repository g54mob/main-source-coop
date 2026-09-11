using Unity.Mathematics;
using UnityEngine;

namespace pworld.Scripts
{
	public class PConstraintFreezeRot : MonoBehaviour
	{
		public bool3 freeze;

		private Vector3 startRot;

		private void Awake()
		{
			startRot = base.transform.rotation.eulerAngles;
		}

		private void LateUpdate()
		{
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			eulerAngles.x = (freeze.x ? startRot.x : eulerAngles.x);
			eulerAngles.y = (freeze.y ? startRot.y : eulerAngles.y);
			eulerAngles.z = (freeze.z ? startRot.z : eulerAngles.z);
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}
	}
}
