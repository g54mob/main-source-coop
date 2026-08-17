using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class DirectionalLightController : MonoBehaviour
	{
		[SerializeField]
		private Light directionalLight;

		[SerializeField]
		private Camera mainCamera;

		[SerializeField]
		private Color startColor;

		[SerializeField]
		private Color endColor;

		private float angleX;

		private float angleY;

		private void OnEnable()
		{
			Tween.Custom(new TweenSettings<float>(45f, 10f, 10f, Ease.Linear, -1, CycleMode.Yoyo), delegate(float newX)
			{
				angleX = newX;
			});
			TweenSettings<float> settings = new TweenSettings<float>(45f, 405f, 20f, Ease.Linear, -1);
			Tween.Custom(this, settings, delegate(DirectionalLightController target, float newY)
			{
				target.angleY = newY;
			});
			TweenSettings<Color> settings2 = new TweenSettings<Color>(startColor, endColor, 10f, Ease.InCirc, -1, CycleMode.Rewind);
			Tween.LightColor(directionalLight, settings2);
			Tween.CameraBackgroundColor(mainCamera, settings2);
			Tween.Custom(settings2, delegate(Color color)
			{
				RenderSettings.fogColor = color;
			});
		}

		private void Update()
		{
			base.transform.localEulerAngles = new Vector3(angleX, angleY);
		}
	}
}
