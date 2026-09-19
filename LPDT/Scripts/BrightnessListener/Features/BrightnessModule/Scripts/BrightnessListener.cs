using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Features.BrightnessModule.Scripts
{
	public class BrightnessListener : MonoBehaviour
	{
		[SerializeField]
		private Volume _volume;

		private BrightnessModel _brightnessModel;

		private ColorAdjustments _colorAdjustments;

		[Inject]
		public void InjectDependencies(BrightnessModel brightnessModel)
		{
			_brightnessModel = brightnessModel;
		}

		private void OnEnable()
		{
			OnBrightnessChanged();
			_brightnessModel.OnBrightnessChanged += OnBrightnessChanged;
		}

		private void OnDisable()
		{
			_brightnessModel.OnBrightnessChanged -= OnBrightnessChanged;
		}

		public void OnBrightnessChanged()
		{
			if (_volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments))
			{
				_colorAdjustments.postExposure.value = _brightnessModel.Brightness;
			}
		}
	}
}
