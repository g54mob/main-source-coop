using System;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using Zenject;

namespace Features.RumModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class DrunkEyeFlushApplier : NetworkBehaviour
	{
		private static readonly int ColorProperty = Shader.PropertyToID("_Color");

		private static readonly int BaseColorProperty = Shader.PropertyToID("_BaseColor");

		[SerializeField]
		private Renderer[] _eyeRenderers;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private DrunkennessConfiguration _configuration;

		private IStat _drunkennessStat;

		private Color[] _baseColors;

		private float _lastFlush = -1f;

		[Inject]
		private void InjectDependencies(SpawnedEntityStatsModel spawnedEntityStatsModel, DrunkennessConfiguration configuration)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_configuration = configuration;
		}

		public override void Spawned()
		{
			CacheBaseColors();
			if (_spawnedEntityStatsModel != null)
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered += OnPlayerStatRegistered;
			}
			TryBindStat();
			ApplyFlush(0f);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_spawnedEntityStatsModel != null)
			{
				_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			}
			UnbindStat();
			ApplyFlush(0f);
			base.Despawned(runner, hasState);
		}

		private void Update()
		{
			if (_drunkennessStat != null && !(_configuration == null))
			{
				float flush = _configuration.EvaluateEyeFlush(_drunkennessStat.FullValue);
				ApplyFlush(flush);
			}
		}

		private void OnPlayerStatRegistered(int playerId)
		{
			if (!(base.Object == null) && base.Object.IsValid && playerId == base.Object.InputAuthority.PlayerId)
			{
				TryBindStat();
			}
		}

		private void TryBindStat()
		{
			if (!(base.Object == null) && base.Object.IsValid && _spawnedEntityStatsModel != null && _spawnedEntityStatsModel.PlayerStats.TryGetValue(base.Object.InputAuthority.PlayerId, out var value))
			{
				UnbindStat();
				_drunkennessStat = value.GetStat(EntityStatType.Drunkenness);
			}
		}

		private void UnbindStat()
		{
			_drunkennessStat = null;
		}

		private void CacheBaseColors()
		{
			if (_eyeRenderers == null)
			{
				_baseColors = Array.Empty<Color>();
				return;
			}
			_baseColors = new Color[_eyeRenderers.Length];
			for (int i = 0; i < _eyeRenderers.Length; i++)
			{
				Renderer renderer = _eyeRenderers[i];
				if (renderer == null || renderer.sharedMaterial == null)
				{
					_baseColors[i] = Color.white;
					continue;
				}
				Material sharedMaterial = renderer.sharedMaterial;
				if (sharedMaterial.HasProperty(BaseColorProperty))
				{
					_baseColors[i] = sharedMaterial.GetColor(BaseColorProperty);
				}
				else if (sharedMaterial.HasProperty(ColorProperty))
				{
					_baseColors[i] = sharedMaterial.GetColor(ColorProperty);
				}
				else
				{
					_baseColors[i] = sharedMaterial.color;
				}
			}
		}

		private void ApplyFlush(float flush01)
		{
			if (Mathf.Approximately(flush01, _lastFlush))
			{
				return;
			}
			_lastFlush = flush01;
			if (_eyeRenderers == null || _baseColors == null)
			{
				return;
			}
			Color b = ((_configuration != null) ? _configuration.EyeFlushColor : Color.red);
			for (int i = 0; i < _eyeRenderers.Length; i++)
			{
				Renderer renderer = _eyeRenderers[i];
				if (!(renderer == null))
				{
					Color color = Color.Lerp(_baseColors[i], b, flush01);
					Material material = renderer.material;
					if (material.HasProperty(BaseColorProperty))
					{
						material.SetColor(BaseColorProperty, color);
					}
					if (material.HasProperty(ColorProperty))
					{
						material.SetColor(ColorProperty, color);
					}
					material.color = color;
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
