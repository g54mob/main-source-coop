using UnityEngine;

namespace Mirror.Examples.TopDownShooter
{
	public class CameraTopDown : MonoBehaviour
	{
		public Transform playerTransform;

		public Vector3 offset;

		public float followSpeed = 5f;

		private void LateUpdate()
		{
			if (playerTransform != null)
			{
				Vector3 b = playerTransform.position + offset;
				base.transform.position = Vector3.Lerp(base.transform.position, b, followSpeed * Time.deltaTime);
			}
		}
	}
}
