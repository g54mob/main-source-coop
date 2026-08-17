using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class CameraProjectionMatrixAnimation : Clickable
	{
		[SerializeField]
		private Camera mainCamera;

		private float interpolationFactor;

		private bool isOrthographic;

		private Tween tween;

		public bool IsAnimating => tween.isAlive;

		public override void OnClick()
		{
			AnimateCameraProjection();
		}

		public void AnimateCameraProjection()
		{
			isOrthographic = !isOrthographic;
			tween.Stop();
			tween = Tween.Custom(this, interpolationFactor, isOrthographic ? 1 : 0, 0.6f, delegate(CameraProjectionMatrixAnimation target, float t)
			{
				target.InterpolateProjectionMatrix(t);
			}, Ease.InOutSine).OnComplete(this, delegate(CameraProjectionMatrixAnimation target)
			{
				target.mainCamera.orthographic = target.isOrthographic;
				target.mainCamera.ResetProjectionMatrix();
			});
		}

		private void InterpolateProjectionMatrix(float _interpolationFactor)
		{
			interpolationFactor = _interpolationFactor;
			int width = Screen.width;
			uint height = (uint)Screen.height;
			float num = (float)(uint)width / (float)height;
			float orthographicSize = mainCamera.orthographicSize;
			Matrix4x4 matrix4x = Matrix4x4.Perspective(mainCamera.fieldOfView, num, mainCamera.nearClipPlane, mainCamera.farClipPlane);
			Matrix4x4 matrix4x2 = Matrix4x4.Ortho((0f - orthographicSize) * num, orthographicSize * num, 0f - orthographicSize, orthographicSize, mainCamera.nearClipPlane, mainCamera.farClipPlane);
			Matrix4x4 projectionMatrix = default(Matrix4x4);
			for (int i = 0; i < 16; i++)
			{
				projectionMatrix[i] = Mathf.Lerp(matrix4x[i], matrix4x2[i], _interpolationFactor);
			}
			mainCamera.projectionMatrix = projectionMatrix;
		}
	}
}
