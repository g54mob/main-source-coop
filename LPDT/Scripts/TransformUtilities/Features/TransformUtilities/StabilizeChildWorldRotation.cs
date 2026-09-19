using UnityEngine;

namespace Features.TransformUtilities
{
	public sealed class StabilizeChildWorldRotation : MonoBehaviour
	{
		[SerializeField]
		private bool _lockWorldEulerX = true;

		[SerializeField]
		private bool _lockWorldEulerY;

		[SerializeField]
		private bool _lockWorldEulerZ = true;

		private Vector3 _frozenWorldEuler;

		private void Awake()
		{
			CaptureCurrentWorldEuler();
		}

		private void LateUpdate()
		{
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			if (_lockWorldEulerX)
			{
				eulerAngles.x = _frozenWorldEuler.x;
			}
			if (_lockWorldEulerY)
			{
				eulerAngles.y = _frozenWorldEuler.y;
			}
			if (_lockWorldEulerZ)
			{
				eulerAngles.z = _frozenWorldEuler.z;
			}
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}

		public void CaptureCurrentWorldEuler()
		{
			_frozenWorldEuler = base.transform.rotation.eulerAngles;
		}
	}
}
