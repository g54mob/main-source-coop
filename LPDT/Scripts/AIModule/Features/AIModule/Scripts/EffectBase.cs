using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public abstract class EffectBase : MonoBehaviour
	{
		private readonly int _intensity = Shader.PropertyToID("_Intensity");

		[SerializeField]
		private List<Renderer> _renderers;

		[SerializeField]
		private float _effectDuration;

		[SerializeField]
		private AnimationCurve _easingType;

		[SerializeField]
		[Range(0f, 1f)]
		private float _maxValue;

		[SerializeField]
		private bool _withMaterialIndex;

		[SerializeField]
		private int _materialIndex;

		private Tweener[] _hitEffectAnimations;

		private MaterialPropertyBlock[] _propertyBlocks;

		private void Awake()
		{
			_renderers.RemoveAll((Renderer rendererComponent) => rendererComponent == null);
			_hitEffectAnimations = new Tweener[_renderers.Count];
			_propertyBlocks = new MaterialPropertyBlock[_renderers.Count];
			for (int num = 0; num < _renderers.Count; num++)
			{
				_propertyBlocks[num] = new MaterialPropertyBlock();
			}
		}

		protected void ToggleEffect()
		{
			for (int i = 0; i < _renderers.Count; i++)
			{
				if (_renderers[i] == null)
				{
					continue;
				}
				float startIntensity = 0f;
				float endValue = 1f;
				if (_hitEffectAnimations[i] != null && _hitEffectAnimations[i].IsActive())
				{
					_hitEffectAnimations[i].Restart();
					continue;
				}
				int currentIndex = i;
				if (!_withMaterialIndex)
				{
					_hitEffectAnimations[i] = DOTween.To(() => startIntensity, delegate(float x)
					{
						UpdatePropertyBlockIntensity(currentIndex, x);
					}, endValue, _effectDuration).SetEase(_easingType).OnComplete(delegate
					{
						ResetPropertyBlockIntensity(currentIndex);
					});
				}
				else
				{
					_hitEffectAnimations[i] = DOTween.To(() => startIntensity, delegate(float x)
					{
						UpdateMaterialPropertyBlockIntensity(currentIndex, x);
					}, endValue, _effectDuration).SetEase(_easingType).OnComplete(delegate
					{
						ResetPropertyBlockIntensity(currentIndex);
					});
				}
			}
		}

		private void UpdatePropertyBlockIntensity(int rendererIndex, float intensity)
		{
			if (rendererIndex >= 0 && rendererIndex < _renderers.Count && _renderers[rendererIndex] != null)
			{
				Renderer renderer = _renderers[rendererIndex];
				MaterialPropertyBlock materialPropertyBlock = _propertyBlocks[rendererIndex];
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat(_intensity, intensity * _maxValue);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}

		private void UpdateMaterialPropertyBlockIntensity(int rendererIndex, float intensity)
		{
			if (rendererIndex >= 0 && rendererIndex < _renderers.Count && _renderers[rendererIndex] != null)
			{
				Renderer renderer = _renderers[rendererIndex];
				MaterialPropertyBlock materialPropertyBlock = _propertyBlocks[rendererIndex];
				renderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);
				materialPropertyBlock.SetFloat(_intensity, intensity * _maxValue);
				renderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
			}
		}

		private void ResetPropertyBlockIntensity(int rendererIndex)
		{
			if (rendererIndex >= 0 && rendererIndex < _renderers.Count && _renderers[rendererIndex] != null)
			{
				Renderer renderer = _renderers[rendererIndex];
				MaterialPropertyBlock materialPropertyBlock = _propertyBlocks[rendererIndex];
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat(_intensity, 0f);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}
}
