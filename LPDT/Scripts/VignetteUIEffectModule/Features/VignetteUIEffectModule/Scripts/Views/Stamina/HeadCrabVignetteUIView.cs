using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.VignetteUIEffectModule.Scripts.Views.Stamina
{
	public class HeadCrabVignetteUIView : VignetteUIEffectUIViewBase
	{
		[Serializable]
		private class ReactiveVignetteLayerSettings
		{
			[SerializeField]
			private Image _image;

			[SerializeField]
			[Range(0f, 1f)]
			private float _minReactIntensity;

			[SerializeField]
			[Range(0f, 1f)]
			private float _maxReactIntensity = 1f;

			public Image Image => _image;

			public float MinReactIntensity => _minReactIntensity;

			public float MaxReactIntensity => _maxReactIntensity;
		}

		private sealed class ReactiveVignetteLayerRuntime
		{
			private readonly Image _image;

			private readonly float _minReactIntensity;

			private readonly float _maxReactIntensity;

			private readonly bool _wasEnabled;

			private readonly Material _baseMaterial;

			private Material _materialInstance;

			private bool _hasInnerRadius;

			private float _lastInnerRadius = float.NaN;

			public ReactiveVignetteLayerRuntime(Image image, float minReactIntensity, float maxReactIntensity)
			{
				_image = image;
				_minReactIntensity = Mathf.Clamp01(minReactIntensity);
				_maxReactIntensity = Mathf.Clamp01(maxReactIntensity);
				_baseMaterial = ((image != null) ? image.material : null);
				_wasEnabled = image != null && image.enabled;
			}

			public void Initialize()
			{
				if (!(_image == null) && !(_baseMaterial == null))
				{
					_materialInstance = UnityEngine.Object.Instantiate(_baseMaterial);
					_image.material = _materialInstance;
					_hasInnerRadius = _materialInstance.HasProperty(InnerRadius);
					_lastInnerRadius = float.NaN;
				}
			}

			public void ApplyIntensity(float intensity)
			{
				if (_image == null)
				{
					return;
				}
				bool flag = _wasEnabled && intensity > 0f && intensity >= _minReactIntensity;
				if (_image.enabled != flag)
				{
					_image.enabled = flag;
				}
				if (flag && !(_materialInstance == null) && _hasInnerRadius)
				{
					float num = 1f - EvaluateLayerIntensity(intensity);
					if (!Mathf.Approximately(_lastInnerRadius, num))
					{
						_lastInnerRadius = num;
						_materialInstance.SetFloat(InnerRadius, num);
					}
				}
			}

			public void Dispose()
			{
				if (!(_image == null))
				{
					_image.enabled = _wasEnabled;
					_image.material = _baseMaterial;
					if (!(_materialInstance == null))
					{
						UnityEngine.Object.Destroy(_materialInstance);
						_materialInstance = null;
					}
				}
			}

			private float EvaluateLayerIntensity(float intensity)
			{
				if (_maxReactIntensity <= _minReactIntensity)
				{
					if (!(intensity >= _maxReactIntensity))
					{
						return 0f;
					}
					return 1f;
				}
				return Mathf.InverseLerp(_minReactIntensity, _maxReactIntensity, intensity);
			}
		}

		private static readonly int InnerRadius = Shader.PropertyToID("_InnerRadius");

		private const string INNER_RADIUS_PROPERTY = "_InnerRadius";

		[SerializeField]
		private List<ReactiveVignetteLayerSettings> _reactiveLayers = new List<ReactiveVignetteLayerSettings>();

		private readonly List<ReactiveVignetteLayerRuntime> _runtimeLayers = new List<ReactiveVignetteLayerRuntime>();

		private float _lastAppliedIntensity;

		protected override void OnEnable()
		{
			InitializeLayers();
			float lastAppliedIntensity = _lastAppliedIntensity;
			_lastAppliedIntensity = float.NaN;
			ApplyIntensity(lastAppliedIntensity);
		}

		protected override void OnDisable()
		{
			DisposeLayers();
		}

		public override void PlayFocusAnimation()
		{
		}

		public override void ApplyIntensity(float intensity)
		{
			float num = Mathf.Clamp01(intensity);
			if (Mathf.Approximately(_lastAppliedIntensity, num))
			{
				return;
			}
			_lastAppliedIntensity = num;
			foreach (ReactiveVignetteLayerRuntime runtimeLayer in _runtimeLayers)
			{
				runtimeLayer.ApplyIntensity(_lastAppliedIntensity);
			}
		}

		public override void DisposeView()
		{
			DisposeLayers();
		}

		private void InitializeLayers()
		{
			DisposeLayers();
			foreach (ReactiveVignetteLayerSettings reactiveLayer in _reactiveLayers)
			{
				AddRuntimeLayer(reactiveLayer);
			}
			InitializeRuntimeLayers();
		}

		private void AddRuntimeLayer(ReactiveVignetteLayerSettings layerSettings)
		{
			if (!(layerSettings?.Image == null))
			{
				_runtimeLayers.Add(new ReactiveVignetteLayerRuntime(layerSettings.Image, layerSettings.MinReactIntensity, layerSettings.MaxReactIntensity));
			}
		}

		private void InitializeRuntimeLayers()
		{
			foreach (ReactiveVignetteLayerRuntime runtimeLayer in _runtimeLayers)
			{
				runtimeLayer.Initialize();
			}
		}

		private void DisposeLayers()
		{
			foreach (ReactiveVignetteLayerRuntime runtimeLayer in _runtimeLayers)
			{
				runtimeLayer.Dispose();
			}
			_runtimeLayers.Clear();
		}
	}
}
