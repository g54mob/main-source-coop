using UnityEngine;

namespace Mirror.Examples.Common
{
	[AddComponentMenu("")]
	public class FaceCamera : MonoBehaviour
	{
		private void LateUpdate()
		{
			base.transform.forward = Camera.main.transform.forward;
		}
	}
}
