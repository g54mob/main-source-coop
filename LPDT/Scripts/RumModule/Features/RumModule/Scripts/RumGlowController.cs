using System.Collections.Generic;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace Features.RumModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class RumGlowController : NetworkBehaviour
	{
		[SerializeField]
		private VisualEffect _visualEffect;

		[SerializeField]
		private Light _light;

		[SerializeField]
		private float _intensity = 1f;

		[SerializeField]
		private float _intensityLerpSpeed = 5f;

		[SerializeField]
		private List<Renderer> _renderers;

		[SerializeField]
		private float _emissionIntensity = 20f;

		private SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private IStat _glowStat;

		private bool _isEffectPlaying;

		[Inject]
		private void InjectDependencies(SpawnedEntityStatsModel spawnedEntityStatsModel)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_spawnedEntityStatsModel.OnPlayerStatRegistered += SetGlowStat;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= SetGlowStat;
		}

		private void SetGlowStat(int playerId)
		{
			if (!(base.Object == null) && base.Object.IsValid && playerId == base.Object.StateAuthority.PlayerId)
			{
				TryBindGlowStat();
			}
		}

		private void TryBindGlowStat()
		{
			if (!(base.Object == null) && base.Object.IsValid && _spawnedEntityStatsModel.PlayerStats.TryGetValue(base.Object.StateAuthority.PlayerId, out var value))
			{
				_glowStat = value.GetStat(EntityStatType.GlowEffect);
			}
		}

		public override void Spawned()
		{
			_visualEffect.Stop();
			_light.intensity = 0f;
			TryBindGlowStat();
		}

		private void Update()
		{
			if (_glowStat == null)
			{
				return;
			}
			if (_glowStat.FullValue > 0f)
			{
				if (!_isEffectPlaying && !base.HasStateAuthority)
				{
					_isEffectPlaying = true;
					_visualEffect.Play();
				}
				_light.intensity = Mathf.Lerp(_light.intensity, _intensity, Time.deltaTime * _intensityLerpSpeed);
			}
			else
			{
				if (_isEffectPlaying && !base.HasStateAuthority)
				{
					_isEffectPlaying = false;
					_visualEffect.Stop();
				}
				_light.intensity = Mathf.Lerp(_light.intensity, 0f, Time.deltaTime * _intensityLerpSpeed);
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
