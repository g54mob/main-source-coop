using UnityEngine;

namespace pworld.Scripts.PPhys
{
	public class PConstraintRotationLimit : MonoBehaviour
	{
		public Vector3 maxRotation = new Vector3(360f, 360f, 360f);

		public Vector3 minRotation = new Vector3(360f, 360f, 360f);

		private Vector3 startRot;

		private void Start()
		{
			startRot = base.transform.rotation.eulerAngles;
		}

		private void LateUpdate()
		{
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			eulerAngles.x = ConstrainAxis(minRotation.x, maxRotation.x, eulerAngles.x);
			eulerAngles.y = ConstrainAxis(minRotation.y, maxRotation.y, eulerAngles.y);
			eulerAngles.z = ConstrainAxis(minRotation.z, maxRotation.y, eulerAngles.z);
			base.transform.localRotation = Quaternion.Euler(eulerAngles);
		}

		private float ConstrainAxis(float min, float max, float val)
		{
			val = Mathf.Max(0f - min, val);
			val = Mathf.Min(max, val);
			return val;
		}
	}
}
