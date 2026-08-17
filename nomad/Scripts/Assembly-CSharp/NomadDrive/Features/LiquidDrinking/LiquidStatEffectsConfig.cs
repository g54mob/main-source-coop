using System;
using EvilCore;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.LiquidDrinking
{
	[CreateAssetMenu(fileName = "LiquidStatEffectsConfig", menuName = "NomadDrive/Liquids/Liquid Stat Effects Config")]
	public class LiquidStatEffectsConfig : ScriptableObject
	{
		[Serializable]
		public class LiquidEffectEntry
		{
			[Tooltip("Stat effects per liter consumed. Positive = buff, negative = debuff")]
			public SerializableDictionary<PlayerStatType, float> statEffectsPerLiter = new SerializableDictionary<PlayerStatType, float>();

			[Tooltip("Seconds before effects start applying (0 = instant start)")]
			[Min(0f)]
			public float effectDelay;

			[Tooltip("Seconds over which effects are spread (0 = instant apply)")]
			[Min(0f)]
			public float effectDuration;
		}

		[Header("Global Settings")]
		[Tooltip("Liters consumed per second while holding the drink button")]
		[SerializeField]
		private float drinkingSpeed = 0.5f;

		[Header("Per-Liquid Effects")]
		[SerializeField]
		private SerializableDictionary<LiquidType, LiquidEffectEntry> liquidEffects = new SerializableDictionary<LiquidType, LiquidEffectEntry>();

		public float DrinkingSpeed => drinkingSpeed;

		public bool TryGetEffectEntry(LiquidType liquidType, out LiquidEffectEntry entry)
		{
			return liquidEffects.TryGetValue(liquidType, out entry);
		}
	}
}
