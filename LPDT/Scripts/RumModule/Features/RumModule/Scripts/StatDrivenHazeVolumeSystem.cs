using System;
using Features.GameUpdaterModule;
using Features.JacuzziBeachInteractableModule.Scripts.Rendering;
using Features.MultiplayerSessionServices.Scripts;
using Features.PostProcessingModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class StatDrivenHazeVolumeSystem : IInitializable, IDisposable
	{
		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PostProcessingModel _postProcessingModel;

		private readonly JacuzziHazeBlendAggregator _hazeBlendAggregator;

		private readonly DrunkennessConfiguration _configuration;

		private readonly IGameUpdater _gameUpdater;

		private IStat _drunkennessStat;

		private float _currentDrunkBlend;

		private float _targetDrunkBlend;

		private float _lastLoggedBlend = -1f;

		public StatDrivenHazeVolumeSystem(SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, PostProcessingModel postProcessingModel, JacuzziHazeBlendAggregator hazeBlendAggregator, DrunkennessConfiguration configuration, IGameUpdater gameUpdater)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_postProcessingModel = postProcessingModel;
			_hazeBlendAggregator = hazeBlendAggregator;
			_configuration = configuration;
			_gameUpdater = gameUpdater;
		}

		public void Initialize()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered += OnPlayerStatRegistered;
			_postProcessingModel.OnVolumeRegistered += OnVolumeRegistered;
			_postProcessingModel.OnVolumeUnRegistered += OnVolumeUnRegistered;
			_gameUpdater.OnUpdate += Tick;
			if (_multiplayerModel.NetworkRunner != null && _spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				OnPlayerStatRegistered(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
			RefreshTargetFromStat();
			_currentDrunkBlend = _targetDrunkBlend;
			_hazeBlendAggregator.SetDrunkBlend(_currentDrunkBlend);
		}

		public void Dispose()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			_postProcessingModel.OnVolumeRegistered -= OnVolumeRegistered;
			_postProcessingModel.OnVolumeUnRegistered -= OnVolumeUnRegistered;
			_gameUpdater.OnUpdate -= Tick;
			if (_drunkennessStat != null)
			{
				_drunkennessStat.OnFullValueChanged -= OnDrunkennessChanged;
			}
			_currentDrunkBlend = 0f;
			_targetDrunkBlend = 0f;
			_hazeBlendAggregator.SetDrunkBlend(0f);
		}

		private void OnPlayerStatRegistered(int playerId)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && _spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				if (_drunkennessStat != null)
				{
					_drunkennessStat.OnFullValueChanged -= OnDrunkennessChanged;
				}
				_drunkennessStat = value.GetStat(EntityStatType.Drunkenness);
				if (_drunkennessStat != null)
				{
					_drunkennessStat.OnFullValueChanged += OnDrunkennessChanged;
				}
				RefreshTargetFromStat();
			}
		}

		private void OnDrunkennessChanged(float _)
		{
			RefreshTargetFromStat();
		}

		private void OnVolumeRegistered(PostProcessingType type, Volume _)
		{
			if (type == PostProcessingType.SessionSceneMain)
			{
				RefreshTargetFromStat();
			}
		}

		private void OnVolumeUnRegistered(PostProcessingType type)
		{
			if (type == PostProcessingType.SessionSceneMain)
			{
				_hazeBlendAggregator.Apply();
			}
		}

		private void Tick()
		{
			RefreshTargetFromStat();
			float num = ((_configuration != null) ? Mathf.Max(0.01f, _configuration.HazeBlendLerpSpeed) : 1.25f);
			float num2 = Mathf.MoveTowards(_currentDrunkBlend, _targetDrunkBlend, num * Time.deltaTime);
			if (!Mathf.Approximately(num2, _currentDrunkBlend))
			{
				_currentDrunkBlend = num2;
				LogIfNeeded();
				_hazeBlendAggregator.SetDrunkBlend(_currentDrunkBlend);
			}
		}

		private void RefreshTargetFromStat()
		{
			float drunkenness = ((_drunkennessStat != null) ? _drunkennessStat.FullValue : 0f);
			_targetDrunkBlend = ((_configuration != null) ? _configuration.EvaluateHazeBlend(drunkenness) : 0f);
		}

		private void LogIfNeeded()
		{
			if (!(_configuration == null) && _configuration.DebugLog && (!(Mathf.Abs(_currentDrunkBlend - _lastLoggedBlend) < 0.15f) || (_currentDrunkBlend > 0f && _lastLoggedBlend < 0f)))
			{
				_lastLoggedBlend = _currentDrunkBlend;
				float num = ((_drunkennessStat != null) ? _drunkennessStat.FullValue : 0f);
				Volume value;
				JacuzziHazeVolume component;
				bool flag = _postProcessingModel.ActiveVolumes.TryGetValue(PostProcessingType.SessionSceneMain, out value) && value != null && value.profile != null && value.profile.TryGet<JacuzziHazeVolume>(out component);
				Debug.Log($"[Drunk] haze lerp drunk={num:0.###} current={_currentDrunkBlend:0.###} " + $"target={_targetDrunkBlend:0.###} volumeOk={flag}");
			}
		}
	}
}
