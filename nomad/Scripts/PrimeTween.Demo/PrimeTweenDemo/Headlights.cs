using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class Headlights : Animatable
	{
		[SerializeField]
		private AnimationCurve ease;

		[SerializeField]
		private Light[] lights;

		private bool isOn;

		public override void OnClick()
		{
			Animate(!isOn);
		}

		public override Sequence Animate(bool _isOn)
		{
			isOn = _isOn;
			Sequence result = Sequence.Create();
			Light[] array = lights;
			foreach (Light target in array)
			{
				result.Group(Tween.LightIntensity(target, _isOn ? 0.7f : 0f, 0.8f, ease));
			}
			return result;
		}
	}
}
