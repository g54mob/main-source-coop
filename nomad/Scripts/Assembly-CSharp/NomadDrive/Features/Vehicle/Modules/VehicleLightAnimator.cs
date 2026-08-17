using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public sealed class VehicleLightAnimator
	{
		private static readonly int EmissiveColorProp = Shader.PropertyToID("_EmissiveColor");

		private static readonly int EmissionColorProp = Shader.PropertyToID("_EmissionColor");

		private readonly Dictionary<Light, Sequence> _lightSequences = new Dictionary<Light, Sequence>();

		private readonly Dictionary<(Renderer, int), Sequence> _emissionSequences = new Dictionary<(Renderer, int), Sequence>();

		private readonly Dictionary<(Renderer, int), Color> _baseEmissionColors = new Dictionary<(Renderer, int), Color>();

		private readonly Dictionary<(Renderer, int), float> _lastEmissionIntensity = new Dictionary<(Renderer, int), float>();

		private MaterialPropertyBlock _propertyBlock;

		public void Apply(LightAnimationRequest request)
		{
			StopExistingTweens(request);
			if (request.duration <= 0f)
			{
				ApplyInstantInternal(request);
				return;
			}
			AnimateLight(request);
			AnimateEmission(request);
		}

		public void ApplyInstant(LightAnimationRequest request)
		{
			StopExistingTweens(request);
			ApplyInstantInternal(request);
		}

		public void Clear(Light light)
		{
			if (light == null)
			{
				return;
			}
			if (_lightSequences.TryGetValue(light, out var value))
			{
				if (value.isAlive)
				{
					value.Stop();
				}
				_lightSequences.Remove(light);
			}
			light.enabled = false;
			light.intensity = 0f;
		}

		public void ClearEmission(Renderer renderer, int materialIndex)
		{
			if (renderer == null)
			{
				return;
			}
			(Renderer, int) key = (renderer, materialIndex);
			if (_emissionSequences.TryGetValue(key, out var value))
			{
				if (value.isAlive)
				{
					value.Stop();
				}
				_emissionSequences.Remove(key);
			}
			ApplyEmissionImmediate(renderer, materialIndex, 0f);
		}

		public void StopAll()
		{
			foreach (Sequence value in _lightSequences.Values)
			{
				if (value.isAlive)
				{
					value.Stop();
				}
			}
			_lightSequences.Clear();
			foreach (Sequence value2 in _emissionSequences.Values)
			{
				if (value2.isAlive)
				{
					value2.Stop();
				}
			}
			_emissionSequences.Clear();
		}

		private void ApplyInstantInternal(LightAnimationRequest request)
		{
			if (request.light != null)
			{
				bool enabled = request.targetIntensity > 0f;
				request.light.enabled = enabled;
				request.light.intensity = request.targetIntensity;
				if (request.targetRange.HasValue)
				{
					request.light.range = request.targetRange.Value;
				}
			}
			SetAdditionalLightsEnabled(request, request.targetIntensity > 0f);
			if (request.emissionRenderer != null)
			{
				ApplyEmissionImmediate(request.emissionRenderer, request.emissionMaterialIndex, request.targetEmissionIntensity);
			}
		}

		private void StopExistingTweens(LightAnimationRequest request)
		{
			if (request.light != null && _lightSequences.TryGetValue(request.light, out var value))
			{
				if (value.isAlive)
				{
					value.Stop();
				}
				_lightSequences.Remove(request.light);
			}
			if (!(request.emissionRenderer != null))
			{
				return;
			}
			(Renderer, int) key = (request.emissionRenderer, request.emissionMaterialIndex);
			if (_emissionSequences.TryGetValue(key, out var value2))
			{
				if (value2.isAlive)
				{
					value2.Stop();
				}
				_emissionSequences.Remove(key);
			}
		}

		private void AnimateLight(LightAnimationRequest request)
		{
			Light light = request.light;
			if (light == null)
			{
				return;
			}
			bool flag = request.targetIntensity > 0f;
			if (flag && !light.enabled)
			{
				light.enabled = true;
			}
			if (flag)
			{
				SetAdditionalLightsEnabled(request, enabled: true);
			}
			if (request.targetRange.HasValue && Mathf.Approximately(light.range, 0f) && flag)
			{
				light.range = request.targetRange.Value;
			}
			Sequence value = Sequence.Create();
			Tween tween = (HasCurve(request.customEase) ? Tween.LightIntensity(light, request.targetIntensity, request.duration, request.customEase) : Tween.LightIntensity(light, request.targetIntensity, request.duration, request.ease));
			value.Group(tween);
			if (request.targetRange.HasValue && !Mathf.Approximately(light.range, request.targetRange.Value))
			{
				float range = light.range;
				float value2 = request.targetRange.Value;
				Tween tween2 = (HasCurve(request.customEase) ? Tween.Custom(light, range, value2, request.duration, delegate(Light l, float v)
				{
					l.range = v;
				}, request.customEase) : Tween.Custom(light, range, value2, request.duration, delegate(Light l, float v)
				{
					l.range = v;
				}, request.ease));
				value.Group(tween2);
			}
			float finalIntensity = request.targetIntensity;
			IReadOnlyList<Light> additionalLights = request.additionalLights;
			value.OnComplete(delegate
			{
				if (finalIntensity <= 0f)
				{
					if (light != null)
					{
						light.enabled = false;
					}
					SetAdditionalLightsEnabled(additionalLights, enabled: false);
				}
			});
			_lightSequences[light] = value;
		}

		private static void SetAdditionalLightsEnabled(LightAnimationRequest request, bool enabled)
		{
			SetAdditionalLightsEnabled(request.additionalLights, enabled);
		}

		private static void SetAdditionalLightsEnabled(IReadOnlyList<Light> lights, bool enabled)
		{
			if (lights == null)
			{
				return;
			}
			for (int i = 0; i < lights.Count; i++)
			{
				if (lights[i] != null)
				{
					lights[i].enabled = enabled;
				}
			}
		}

		private void AnimateEmission(LightAnimationRequest request)
		{
			Renderer emissionRenderer = request.emissionRenderer;
			if (emissionRenderer == null)
			{
				return;
			}
			int matIndex = request.emissionMaterialIndex;
			Material[] sharedMaterials = emissionRenderer.sharedMaterials;
			int num = ((sharedMaterials != null) ? sharedMaterials.Length : 0);
			if (matIndex >= 0 && matIndex < num)
			{
				Color baseColor = GetOrCacheBaseEmissionColor(emissionRenderer, matIndex);
				float lastEmissionIntensity = GetLastEmissionIntensity(emissionRenderer, matIndex);
				float endValue = Mathf.Clamp01(request.targetEmissionIntensity);
				Sequence value = Sequence.Create();
				Tween tween = (HasCurve(request.customEase) ? Tween.Custom(emissionRenderer, lastEmissionIntensity, endValue, request.duration, delegate(Renderer r, float v)
				{
					WriteEmission(r, matIndex, baseColor, v);
				}, request.customEase) : Tween.Custom(emissionRenderer, lastEmissionIntensity, endValue, request.duration, delegate(Renderer r, float v)
				{
					WriteEmission(r, matIndex, baseColor, v);
				}, request.ease));
				value.Group(tween);
				_emissionSequences[(emissionRenderer, matIndex)] = value;
			}
		}

		private void ApplyEmissionImmediate(Renderer renderer, int materialIndex, float intensity)
		{
			if (!(renderer == null))
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				int num = ((sharedMaterials != null) ? sharedMaterials.Length : 0);
				if (materialIndex >= 0 && materialIndex < num)
				{
					Color orCacheBaseEmissionColor = GetOrCacheBaseEmissionColor(renderer, materialIndex);
					WriteEmission(renderer, materialIndex, orCacheBaseEmissionColor, Mathf.Clamp01(intensity));
				}
			}
		}

		private void WriteEmission(Renderer renderer, int materialIndex, Color baseColor, float intensity)
		{
			if (!(renderer == null))
			{
				if (_propertyBlock == null)
				{
					_propertyBlock = new MaterialPropertyBlock();
				}
				float num = Mathf.Clamp01(intensity);
				Color value = baseColor * num;
				renderer.GetPropertyBlock(_propertyBlock, materialIndex);
				_propertyBlock.SetColor(EmissiveColorProp, value);
				_propertyBlock.SetColor(EmissionColorProp, value);
				renderer.SetPropertyBlock(_propertyBlock, materialIndex);
				_lastEmissionIntensity[(renderer, materialIndex)] = num;
			}
		}

		private Color GetOrCacheBaseEmissionColor(Renderer renderer, int materialIndex)
		{
			(Renderer, int) key = (renderer, materialIndex);
			if (_baseEmissionColors.TryGetValue(key, out var value))
			{
				return value;
			}
			Material[] sharedMaterials = renderer.sharedMaterials;
			if (sharedMaterials == null || materialIndex < 0 || materialIndex >= sharedMaterials.Length || sharedMaterials[materialIndex] == null)
			{
				_baseEmissionColors[key] = Color.black;
				return Color.black;
			}
			Material material = sharedMaterials[materialIndex];
			Color color = Color.black;
			if (material.HasProperty(EmissiveColorProp))
			{
				color = material.GetColor(EmissiveColorProp);
			}
			else if (material.HasProperty(EmissionColorProp))
			{
				color = material.GetColor(EmissionColorProp);
			}
			_baseEmissionColors[key] = color;
			return color;
		}

		public float GetLastEmissionIntensity(Renderer renderer, int materialIndex)
		{
			if (!_lastEmissionIntensity.TryGetValue((renderer, materialIndex), out var value))
			{
				return 0f;
			}
			return value;
		}

		private static bool HasCurve(AnimationCurve curve)
		{
			if (curve != null)
			{
				return curve.length > 0;
			}
			return false;
		}
	}
}
