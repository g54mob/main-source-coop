using System.Collections.Generic;
using Features.VignetteUIEffectModule.Scripts.Views;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.VignetteUIEffectModule.Scripts
{
	[CreateAssetMenu(fileName = "VignetteUIEffectViewsConfiguration_Default", menuName = "Configurations/VignetteUIEffect/VignetteUIEffectViewsConfiguration")]
	public class VignetteUIEffectViewsConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<VignetteUIEffectType, VignetteUIEffectUIViewBase> EffectViewPrefabs { get; private set; }

		public bool TryGetViewPrefab(VignetteUIEffectType effectType, out VignetteUIEffectUIViewBase viewPrefab)
		{
			viewPrefab = null;
			if (effectType == VignetteUIEffectType.None || EffectViewPrefabs == null)
			{
				return false;
			}
			if (EffectViewPrefabs.TryGetValue(effectType, out viewPrefab))
			{
				return viewPrefab != null;
			}
			return false;
		}

		public IEnumerable<VignetteUIEffectType> EnumerateConfiguredEffectTypes()
		{
			if (EffectViewPrefabs == null)
			{
				yield break;
			}
			foreach (VignetteUIEffectType key in EffectViewPrefabs.Keys)
			{
				if (key != VignetteUIEffectType.None && EffectViewPrefabs.TryGetValue(key, out var value) && value != null)
				{
					yield return key;
				}
			}
		}
	}
}
