using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class ArcTipPulsingAnimation : MonoBehaviour
	{
		private static readonly int RangeId = Shader.PropertyToID("_Range");

		[SerializeField]
		private Renderer _renderer;

		[SerializeField]
		private float _minRadius = 0.06f;

		[SerializeField]
		private float _maxRadius = 0.5f;

		[SerializeField]
		private float _pulseDuration = 1.5f;

		private MaterialPropertyBlock _propertyBlock;

		private float _timer;

		private void Awake()
		{
			_propertyBlock = new MaterialPropertyBlock();
		}

		private void OnEnable()
		{
			_timer = 0f;
			ApplyRadius(_minRadius);
		}

		private void Update()
		{
			if (!(_pulseDuration <= 0f))
			{
				_timer += Time.deltaTime;
				if (_timer >= _pulseDuration)
				{
					_timer = 0f;
				}
				float t = _timer / _pulseDuration;
				ApplyRadius(Mathf.Lerp(_minRadius, _maxRadius, t));
			}
		}

		private void ApplyRadius(float radius)
		{
			if (!(_renderer == null))
			{
				_renderer.GetPropertyBlock(_propertyBlock);
				_propertyBlock.SetFloat(RangeId, radius);
				_renderer.SetPropertyBlock(_propertyBlock);
			}
		}
	}
}
