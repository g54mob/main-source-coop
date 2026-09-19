using UnityEngine;

namespace Mirror.Examples.CouchCoop
{
	public class CameraViewForAll : MonoBehaviour
	{
		public Transform cameraTransform;

		public float camSpeed = 2f;

		public float orthoSizeSpeed = 2f;

		public Camera mainCamera;

		public float cameraZ = -5f;

		public float cameraBufferX = 0.1f;

		public float cameraBufferY = 0.1f;

		public float minOrthographicSize = 0.1f;

		public float targetYPosition = 4.5f;

		private Vector2Int boundsMin;

		private Vector2Int boundsMax;

		private Vector3 targetCameraPosition;

		private float targetOrthographicSize;

		private void Update()
		{
			if (CouchPlayer.playersList.Count > 0)
			{
				CalculateBounds();
				CalculateTargetCameraPosAndSize();
				MoveCamera();
			}
		}

		private void CalculateBounds()
		{
			boundsMin = new Vector2Int(int.MaxValue, int.MaxValue);
			boundsMax = new Vector2Int(int.MinValue, int.MinValue);
			foreach (GameObject players in CouchPlayer.playersList)
			{
				Vector3 position = players.transform.position;
				boundsMin.x = Mathf.Min(boundsMin.x, Mathf.FloorToInt(position.x));
				boundsMin.y = Mathf.Min(boundsMin.y, Mathf.FloorToInt(position.y));
				boundsMax.x = Mathf.Max(boundsMax.x, Mathf.CeilToInt(position.x));
				boundsMax.y = Mathf.Max(boundsMax.y, Mathf.CeilToInt(position.y));
			}
			boundsMin.x -= Mathf.FloorToInt(cameraBufferX);
			boundsMin.y -= Mathf.FloorToInt(cameraBufferY);
			boundsMax.x += Mathf.CeilToInt(cameraBufferX);
			boundsMax.y += Mathf.CeilToInt(cameraBufferY);
		}

		private void CalculateTargetCameraPosAndSize()
		{
			float num = (float)Screen.width / (float)Screen.height;
			float a = Mathf.Max((float)((boundsMax.x - boundsMin.x) / 2) / num, minOrthographicSize / num);
			float b = Mathf.Max(boundsMax.y - boundsMin.y / 2, minOrthographicSize);
			targetOrthographicSize = Mathf.Max(a, b);
			float x = (boundsMax.x + boundsMin.x) / 2;
			float y = ((targetYPosition != 0f) ? targetYPosition : ((float)((boundsMax.y + boundsMin.y) / 2)));
			targetCameraPosition = new Vector3(x, y, cameraZ);
		}

		private void MoveCamera()
		{
			cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition, camSpeed * Time.deltaTime);
			mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, orthoSizeSpeed * Time.deltaTime);
		}
	}
}
