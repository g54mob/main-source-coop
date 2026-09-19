using System;
using System.Collections.Generic;

namespace Features.VignetteUIEffectModule.Scripts
{
	public class VignetteUIEffectModel
	{
		private readonly Dictionary<VignetteUIEffectType, IVignetteUIEffectVisual> _activeLayers = new Dictionary<VignetteUIEffectType, IVignetteUIEffectVisual>();

		private readonly Dictionary<VignetteUIEffectType, List<VignetteUIEffect>> _appliedVignetteEffects = new Dictionary<VignetteUIEffectType, List<VignetteUIEffect>>();

		public IReadOnlyDictionary<VignetteUIEffectType, IVignetteUIEffectVisual> ActiveLayers => _activeLayers;

		public IReadOnlyDictionary<VignetteUIEffectType, List<VignetteUIEffect>> AppliedVignetteEffects => _appliedVignetteEffects;

		public bool IsVignetteDisabled { get; set; }

		public event Action<VignetteUIEffectType> OnLayerRegistered;

		public event Action<VignetteUIEffectType> OnLayerUnregistered;

		public void RegisterLayer(VignetteUIEffectType effectType, IVignetteUIEffectVisual visual)
		{
			if (effectType != VignetteUIEffectType.None)
			{
				_activeLayers[effectType] = visual;
				this.OnLayerRegistered?.Invoke(effectType);
			}
		}

		public void UnregisterLayer(VignetteUIEffectType effectType)
		{
			if (_activeLayers.Remove(effectType))
			{
				this.OnLayerUnregistered?.Invoke(effectType);
			}
		}

		public void PlayFocusAnimation(VignetteUIEffectType effectType)
		{
			if (effectType != VignetteUIEffectType.None && _activeLayers.TryGetValue(effectType, out var value))
			{
				value.PlayFocusAnimation();
			}
		}

		public void ApplyVignetteEffect(VignetteUIEffectType effectType, VignetteUIEffect vignetteEffect)
		{
			if (effectType != VignetteUIEffectType.None && !_appliedVignetteEffects.TryAdd(effectType, new List<VignetteUIEffect> { vignetteEffect }))
			{
				_appliedVignetteEffects[effectType].Add(vignetteEffect);
			}
		}

		public void RemoveVignetteEffect(VignetteUIEffectType effectType, VignetteUIEffect vignetteEffect)
		{
			if (_appliedVignetteEffects.TryGetValue(effectType, out var value))
			{
				value.Remove(vignetteEffect);
				if (value.Count == 0)
				{
					_appliedVignetteEffects.Remove(effectType);
				}
			}
		}

		public bool ContainsEffect(VignetteUIEffectType effectType, VignetteUIEffect vignetteEffect)
		{
			if (_appliedVignetteEffects.TryGetValue(effectType, out var value))
			{
				return value.Contains(vignetteEffect);
			}
			return false;
		}
	}
}
