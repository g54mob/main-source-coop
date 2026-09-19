using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.BeachPresetModule.Scripts.Core.Configurations;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.BeachPresetModule.Scripts.Data;
using Features.LevelModule.Scripts;
using Features.LevelModule.Scripts.RoomVariations;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Core
{
	public class BeachPresetApplyByLevelSystem : ILevelBeachPresetApplication, ITickable
	{
		private readonly BeachByLevelsPresetConfiguration _beachByLevelsPreset;

		private readonly BeachPresetRuntimeModel _presetRuntimeModel;

		private readonly LevelModel _levelModel;

		private readonly DiContainer _container;

		private readonly SpawnedRoomsModel _spawnedRoomsModel;

		private BeachPresetRuntimeContext _activeContext;

		private BeachPreset _activePreset;

		private Transform _runtimeAnchor;

		private readonly List<IBeachRenderStateEnforcer> _activeEnforcers = new List<IBeachRenderStateEnforcer>();

		public BeachPresetApplyByLevelSystem(DiContainer container, BeachPresetRuntimeModel presetRuntimeModel, BeachByLevelsPresetConfiguration beachByLevelsPreset, LevelModel levelModel, SpawnedRoomsModel spawnedRoomsModel)
		{
			_container = container;
			_presetRuntimeModel = presetRuntimeModel;
			_beachByLevelsPreset = beachByLevelsPreset;
			_levelModel = levelModel;
			_spawnedRoomsModel = spawnedRoomsModel;
		}

		public void Tick()
		{
			for (int i = 0; i < _activeEnforcers.Count; i++)
			{
				_activeEnforcers[i].EnforceRenderState();
			}
		}

		public async UniTask ApplyForCurrentLevelAsync()
		{
			LevelType targetLevel = _levelModel.LastLoadedLevel;
			Debug.Log($"[HoopSpawn] Beach preset apply invoked for {targetLevel} (rooms={_spawnedRoomsModel.IsAllTasksCompleted}, anchor={_presetRuntimeModel.RuntimeAnchor != null}) — awaiting readiness.");
			await UniTask.WaitUntil(() => _spawnedRoomsModel.IsAllTasksCompleted && _presetRuntimeModel.RuntimeAnchor != null).TimeoutWithoutException(TimeSpan.FromSeconds(12.0));
			if (_presetRuntimeModel.RuntimeAnchor == null)
			{
				Debug.LogWarning($"[HoopSpawn] Beach preset readiness timed out for {targetLevel} (runtime anchor still null after the floored wait) — skipping beach preset to avoid stranding Level Enter.");
				return;
			}
			Debug.Log($"[HoopSpawn] Beach preset readiness satisfied for {targetLevel} — applying base preset.");
			ApplyBasePresetAfterLevelLoaded(targetLevel);
		}

		private void ApplyBasePresetAfterLevelLoaded(LevelType level)
		{
			if (_beachByLevelsPreset.PresetsByLevel.TryGetValue(level, out var value))
			{
				Debug.Log($"[HoopSpawn] Preset resolved for {level} — running behaviours (incl. beach interactable spawn).");
				ApplyPreset(value, _presetRuntimeModel.RuntimeAnchor, level);
			}
			else
			{
				Debug.LogWarning($"[HoopSpawn] No beach preset mapped for {level} — no beach interactables (incl. basketball hoop) will spawn.");
			}
		}

		private void ApplyPreset(BeachPreset preset, Transform runtimeAnchor, LevelType levelType)
		{
			if (preset == null)
			{
				ClearActivePreset();
				return;
			}
			ClearActivePreset();
			_runtimeAnchor = runtimeAnchor;
			_activePreset = preset;
			_activeContext = CreateContext(preset, levelType);
			preset.Apply(_activeContext, _container);
			foreach (BeachBehaviour item2 in preset.GetEnabledBehavioursInApplyOrder())
			{
				if (item2 is IBeachRenderStateEnforcer item)
				{
					_activeEnforcers.Add(item);
				}
			}
		}

		public void ClearActivePreset()
		{
			_activeEnforcers.Clear();
			if (_activePreset != null && _activeContext != null)
			{
				_activePreset.Clear(_activeContext);
			}
			_activePreset = null;
			_activeContext = null;
			_runtimeAnchor = null;
		}

		private BeachPresetRuntimeContext CreateContext(BeachPreset preset, LevelType levelType)
		{
			return new BeachPresetRuntimeContext(preset, _runtimeAnchor, levelType);
		}
	}
}
