using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Features.VignetteUIEffectModule.Scripts
{
	public class VignetteUIEffectApplier : IInitializable, ITickable, IDisposable
	{
		private readonly VignetteUIEffectModel _vignetteUIEffectModel;

		private readonly Dictionary<VignetteUIEffectType, float> _lastAppliedIntensities = new Dictionary<VignetteUIEffectType, float>();

		public VignetteUIEffectApplier(VignetteUIEffectModel vignetteUIEffectModel)
		{
			_vignetteUIEffectModel = vignetteUIEffectModel;
		}

		public void Initialize()
		{
			_vignetteUIEffectModel.OnLayerUnregistered += OnLayerUnregistered;
		}

		public void Dispose()
		{
			_vignetteUIEffectModel.OnLayerUnregistered -= OnLayerUnregistered;
			_lastAppliedIntensities.Clear();
		}

		public void Tick()
		{
			IReadOnlyDictionary<VignetteUIEffectType, IVignetteUIEffectVisual> activeLayers = _vignetteUIEffectModel.ActiveLayers;
			if (activeLayers.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<VignetteUIEffectType, IVignetteUIEffectVisual> item in activeLayers)
			{
				float num = 0f;
				if (_vignetteUIEffectModel.AppliedVignetteEffects.TryGetValue(item.Key, out var value))
				{
					foreach (VignetteUIEffect item2 in value)
					{
						num += item2.EffectIntensity;
					}
				}
				if (_vignetteUIEffectModel.IsVignetteDisabled)
				{
					num = 0f;
				}
				if (!_lastAppliedIntensities.TryGetValue(item.Key, out var value2) || !Mathf.Approximately(value2, num))
				{
					item.Value.ApplyIntensity(num);
					_lastAppliedIntensities[item.Key] = num;
				}
			}
		}

		private void OnLayerUnregistered(VignetteUIEffectType effectType)
		{
			_lastAppliedIntensities.Remove(effectType);
		}
	}
}
