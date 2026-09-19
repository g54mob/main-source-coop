using System;
using System.Collections.Generic;
using Features.StatsUsageModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.PlayerStatsVisualizationModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ParticleStatsVisualizer : NetworkBehaviour
	{
		[Serializable]
		private sealed class ParticleStatRangeRule
		{
			[field: SerializeField]
			public EntityStatType StatType { get; private set; }

			[field: SerializeField]
			public float MinValue { get; private set; }

			[field: SerializeField]
			public float MaxValue { get; private set; } = 999999f;

			[field: SerializeField]
			public ParticleSystem Particle { get; private set; }

			[field: SerializeField]
			public bool DisableGameObjectWhenInactive { get; private set; } = true;
		}

		[SerializeField]
		private List<ParticleStatRangeRule> _rules = new List<ParticleStatRangeRule>();

		[SerializeField]
		private bool _initializeOnStart = true;

		private IPlayerStatsUpgradeService _playerStatsUpgradeService;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private EntityStatEntityNetworkedBase _localPlayerStats;

		private Dictionary<EntityStatType, List<int>> _ruleIndicesByStat;

		private int _localPlayerId;

		[Inject]
		private void InjectDependencies(SpawnedEntityStatsModel spawnedEntityStatsModel, IPlayerStatsUpgradeService playerStatsUpgradeService)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_playerStatsUpgradeService = playerStatsUpgradeService;
		}

		public override void Spawned()
		{
			if (_initializeOnStart)
			{
				InitializeTargetPlayer(base.Object.InputAuthority.PlayerId);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			UnsubscribeFromStats();
		}

		public void InitializeTargetPlayer(int localPlayerPlayerId)
		{
			_localPlayerId = localPlayerPlayerId;
			if (_playerStatsUpgradeService.IsPlayerStatsReady())
			{
				InitializeStats();
			}
			else
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered += OnPlayerStatRegistered;
			}
		}

		private void OnPlayerStatRegistered(int player)
		{
			if (player == _localPlayerId)
			{
				InitializeStats();
				_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			}
		}

		private void InitializeStats()
		{
			UnsubscribeFromStats();
			if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(_localPlayerId, out var value))
			{
				_localPlayerStats = value;
				_localPlayerStats.OnFullValueChanged += OnFullStatValueChanged;
				BuildRuleLookup();
				RefreshAllRules();
			}
		}

		private void UnsubscribeFromStats()
		{
			if (_localPlayerStats != null)
			{
				_localPlayerStats.OnFullValueChanged -= OnFullStatValueChanged;
				_localPlayerStats = null;
			}
		}

		private void BuildRuleLookup()
		{
			_ruleIndicesByStat = new Dictionary<EntityStatType, List<int>>();
			if (_rules == null)
			{
				return;
			}
			for (int i = 0; i < _rules.Count; i++)
			{
				ParticleStatRangeRule particleStatRangeRule = _rules[i];
				if (particleStatRangeRule != null)
				{
					if (!_ruleIndicesByStat.TryGetValue(particleStatRangeRule.StatType, out var value))
					{
						value = new List<int>();
						_ruleIndicesByStat.Add(particleStatRangeRule.StatType, value);
					}
					value.Add(i);
				}
			}
		}

		private void OnFullStatValueChanged(EntityStatType statType)
		{
			RefreshRulesForStat(statType);
		}

		private void RefreshAllRules()
		{
			if (_rules != null)
			{
				for (int i = 0; i < _rules.Count; i++)
				{
					RefreshRuleByIndex(i);
				}
			}
		}

		private void RefreshRulesForStat(EntityStatType statType)
		{
			if (_ruleIndicesByStat != null && _ruleIndicesByStat.TryGetValue(statType, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					RefreshRuleByIndex(value[i]);
				}
			}
		}

		private void RefreshRuleByIndex(int ruleIndex)
		{
			if (_localPlayerStats == null || _rules == null || (uint)ruleIndex >= (uint)_rules.Count)
			{
				return;
			}
			ParticleStatRangeRule particleStatRangeRule = _rules[ruleIndex];
			if (!(particleStatRangeRule?.Particle == null))
			{
				IStat stat = _localPlayerStats.GetStat(particleStatRangeRule.StatType);
				if (stat == null)
				{
					ApplyRule(particleStatRangeRule, isInRange: false);
					return;
				}
				float num = Mathf.Min(particleStatRangeRule.MinValue, particleStatRangeRule.MaxValue);
				float num2 = Mathf.Max(particleStatRangeRule.MinValue, particleStatRangeRule.MaxValue);
				bool isInRange = stat.FullValue >= num && stat.FullValue <= num2;
				ApplyRule(particleStatRangeRule, isInRange);
			}
		}

		private static void ApplyRule(ParticleStatRangeRule rule, bool isInRange)
		{
			ParticleSystem particle = rule.Particle;
			GameObject gameObject = particle.gameObject;
			if (isInRange)
			{
				if (!gameObject.activeSelf)
				{
					gameObject.SetActive(value: true);
				}
				if (!particle.isPlaying)
				{
					particle.Play(withChildren: true);
				}
			}
			else
			{
				if (particle.isPlaying)
				{
					particle.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
				}
				if (rule.DisableGameObjectWhenInactive && gameObject.activeSelf)
				{
					gameObject.SetActive(value: false);
				}
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
