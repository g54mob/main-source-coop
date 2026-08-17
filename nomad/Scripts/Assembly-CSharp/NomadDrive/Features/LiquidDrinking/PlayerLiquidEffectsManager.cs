using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using NomadDrive.Features.LiquidTransferSystem;
using NomadDrive.Features.Player;
using NomadDrive.Features.Player.Core;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.LiquidDrinking
{
	public class PlayerLiquidEffectsManager : MonoBehaviour, IPlayerComponent
	{
		[Header("Harmful Liquid Audio")]
		[Tooltip("Cough one-shot played when the player drinks a stat-damaging liquid (entry with any negative effect).")]
		[SerializeField]
		private SoundID harmfulDrinkCoughSound;

		[Tooltip("Minimum seconds between coughs while drinking a harmful liquid.")]
		[SerializeField]
		[Min(0.5f)]
		private float coughCooldown = 3f;

		[Inject]
		private INetworkedAudioManager _networkedAudio;

		private PlayerStatsManager _statsManager;

		private readonly List<ActiveLiquidEffect> _activeEffects = new List<ActiveLiquidEffect>();

		private bool _isInitialized;

		private float _nextCoughTime;

		public int SetupPriority => 16;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer)
			{
				_statsManager = GetComponent<PlayerStatsManager>();
				_isInitialized = true;
			}
			else
			{
				base.enabled = false;
			}
		}

		public void ApplyLiquidEffects(LiquidType liquidType, float amountConsumedLiters, LiquidStatEffectsConfig config)
		{
			if (!_isInitialized || _statsManager == null || config == null || amountConsumedLiters <= 0f || !config.TryGetEffectEntry(liquidType, out var entry))
			{
				return;
			}
			TryPlayHarmfulCough(entry);
			foreach (KeyValuePair<PlayerStatType, float> item in entry.statEffectsPerLiter)
			{
				float num = item.Value * amountConsumedLiters;
				if (!Mathf.Approximately(num, 0f))
				{
					if (entry.effectDelay <= 0f && entry.effectDuration <= 0f)
					{
						ApplyStatEffect(item.Key, num);
					}
					else
					{
						_activeEffects.Add(new ActiveLiquidEffect(item.Key, num, entry.effectDelay, entry.effectDuration));
					}
				}
			}
		}

		private void TryPlayHarmfulCough(LiquidStatEffectsConfig.LiquidEffectEntry entry)
		{
			if (!(Time.time < _nextCoughTime) && harmfulDrinkCoughSound.IsValid() && IsHarmfulEntry(entry))
			{
				_nextCoughTime = Time.time + coughCooldown;
				_networkedAudio?.PlayOneShot(harmfulDrinkCoughSound, base.transform.position);
			}
		}

		private static bool IsHarmfulEntry(LiquidStatEffectsConfig.LiquidEffectEntry entry)
		{
			if (entry?.statEffectsPerLiter == null)
			{
				return false;
			}
			foreach (KeyValuePair<PlayerStatType, float> item in entry.statEffectsPerLiter)
			{
				if (item.Value < 0f)
				{
					return true;
				}
			}
			return false;
		}

		private void Update()
		{
			if (_activeEffects.Count == 0)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			for (int num = _activeEffects.Count - 1; num >= 0; num--)
			{
				ActiveLiquidEffect activeLiquidEffect = _activeEffects[num];
				if (activeLiquidEffect.IsDelayed)
				{
					activeLiquidEffect.DelayRemaining -= deltaTime;
				}
				else
				{
					if (activeLiquidEffect.Duration <= 0f)
					{
						float amount = activeLiquidEffect.TotalAmount - activeLiquidEffect.AppliedAmount;
						ApplyStatEffect(activeLiquidEffect.StatType, amount);
						activeLiquidEffect.AppliedAmount = activeLiquidEffect.TotalAmount;
					}
					else
					{
						activeLiquidEffect.Elapsed += deltaTime;
						float num2 = Mathf.Clamp01(activeLiquidEffect.Elapsed / activeLiquidEffect.Duration);
						float num3 = activeLiquidEffect.TotalAmount * num2;
						float num4 = num3 - activeLiquidEffect.AppliedAmount;
						if (!Mathf.Approximately(num4, 0f))
						{
							ApplyStatEffect(activeLiquidEffect.StatType, num4);
							activeLiquidEffect.AppliedAmount = num3;
						}
					}
					if (activeLiquidEffect.IsComplete)
					{
						_activeEffects.RemoveAt(num);
					}
				}
			}
		}

		private void ApplyStatEffect(PlayerStatType statType, float amount)
		{
			PlayerStat stat = GetStat(statType);
			if (stat != null)
			{
				if (amount >= 0f)
				{
					stat.IncreaseValue(amount);
				}
				else
				{
					stat.DecreaseValue(0f - amount);
				}
			}
		}

		private PlayerStat GetStat(PlayerStatType statType)
		{
			return statType switch
			{
				PlayerStatType.Nutrition => _statsManager.Nutrition, 
				PlayerStatType.Hydration => _statsManager.Hydration, 
				PlayerStatType.Energy => _statsManager.Energy, 
				PlayerStatType.Health => _statsManager.Health, 
				_ => null, 
			};
		}

		public void ClearAllEffects()
		{
			_activeEffects.Clear();
		}
	}
}
