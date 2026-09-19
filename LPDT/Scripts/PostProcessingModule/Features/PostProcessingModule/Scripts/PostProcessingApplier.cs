using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Features.PostProcessingModule.Scripts
{
	public class PostProcessingApplier : MonoBehaviour
	{
		[SerializeField]
		private Volume _ppVolume;

		private PostProcessingModel _postProcessingModel;

		private Vignette _attachedVignette;

		private Color _initialVignetteColor;

		private float _initialVignetteIntensity;

		private float _initialVignetteSmoothness;

		[Inject]
		public void InjectDependencies(PostProcessingModel postProcessingModel)
		{
			_postProcessingModel = postProcessingModel;
		}

		private void Start()
		{
			if (_ppVolume.profile.TryGet<Vignette>(out var component))
			{
				_attachedVignette = component;
				_initialVignetteColor = component.color.value;
				_initialVignetteIntensity = component.intensity.value;
				_initialVignetteSmoothness = component.smoothness.value;
			}
		}

		private void Update()
		{
			if ((object)_attachedVignette == null || !_postProcessingModel.AppliedVignettedEffects.TryGetValue(_ppVolume, out var value))
			{
				return;
			}
			Color color = Color.clear;
			float num = 0f;
			float num2 = 0f;
			foreach (VignetteEffect item in value)
			{
				color = item.EffectColor * item.EffectIntensity + color * (1f - item.EffectIntensity);
				num += item.EffectIntensity / (float)value.Count;
				num2 += item.EffectSmoothness / (float)value.Count;
			}
			_attachedVignette.color.value = _initialVignetteColor + color;
			if (_postProcessingModel.IsVignetteDisabled)
			{
				_attachedVignette.intensity.value = 0f;
			}
			else
			{
				_attachedVignette.intensity.value = _initialVignetteIntensity + num;
			}
			_attachedVignette.smoothness.value = _initialVignetteSmoothness + num2;
		}
	}
}
