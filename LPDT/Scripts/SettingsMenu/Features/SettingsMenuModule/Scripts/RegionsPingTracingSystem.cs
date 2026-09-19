using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Features.CoroutineUtils.Scripts;
using Features.ProgressSavingModule.Scripts.Implementation;
using Features.SettingsMenuModule.Scripts.Data;
using Fusion;
using Fusion.Photon.Realtime;
using UnityEngine;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class RegionsPingTracingSystem : IInitializable, IDisposable
	{
		private const string BASE_REGION = "eu";

		private readonly RegionsPingConfiguration _pingConfiguration;

		private readonly RegionsPingModel _regionsPingModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly CurrentSettingsModel _currentSettingsModel;

		private readonly ISavingService _savingService;

		private Coroutine _pingRoutine;

		private bool _activeTracing;

		public RegionsPingTracingSystem(RegionsPingModel regionsPingModel, ICoroutineRunner coroutineRunner, RegionsPingConfiguration pingConfiguration, CurrentSettingsModel currentSettingsModel, ISavingService savingService)
		{
			_regionsPingModel = regionsPingModel;
			_coroutineRunner = coroutineRunner;
			_pingConfiguration = pingConfiguration;
			_currentSettingsModel = currentSettingsModel;
			_savingService = savingService;
		}

		public void Initialize()
		{
			_activeTracing = true;
			_pingRoutine = _coroutineRunner.StartCoroutine(PingRoutine());
			_regionsPingModel.OnCurrentRegionUpdated += UpdateCurrentRegion;
			_regionsPingModel.OnRegionsInfoUpdated += TrySelectBest;
		}

		public void Dispose()
		{
			_activeTracing = false;
			if (_pingRoutine != null && _coroutineRunner.IsActive)
			{
				_coroutineRunner.StopCoroutine(_pingRoutine);
			}
			_regionsPingModel.OnCurrentRegionUpdated -= UpdateCurrentRegion;
			_regionsPingModel.OnRegionsInfoUpdated -= TrySelectBest;
		}

		private void UpdateCurrentRegion()
		{
			SetFixedRegion(_regionsPingModel.CurrentRegion);
		}

		private void TrySelectBest()
		{
			if (!_currentSettingsModel.HasFixedRegion)
			{
				SetFixedRegion((_regionsPingModel.Regions?.ToList() ?? new List<RegionInfo>()).OrderBy((RegionInfo info) => info.RegionPing).ToList().FirstOrDefault()
					.RegionCode);
					PhotonAppSettings.Global.AppSettings.FixedRegion = ResolveFixedRegion();
				}
			}

			private IEnumerator PingRoutine()
			{
				WaitForSecondsRealtime pingRoutineDelay = new WaitForSecondsRealtime(_pingConfiguration.RegionsPingUpdateDelay);
				PhotonAppSettings.Global.AppSettings.FixedRegion = ResolveFixedRegion();
				if (_currentSettingsModel.HasFixedRegion)
				{
					_regionsPingModel.CurrentRegion = _currentSettingsModel.FixedRegion;
				}
				while (_activeTracing)
				{
					bool isInitialLoad = _regionsPingModel.Regions.Count == 0;
					if (isInitialLoad)
					{
						_regionsPingModel.SetRegionsSearchInProgress(isInProgress: true);
					}
					Task<List<RegionInfo>> task = GetAvailableRegions();
					yield return new WaitUntil(() => task.IsCompleted);
					if (task.IsCompletedSuccessfully)
					{
						_regionsPingModel.SetNewRegionsInfo(task.Result);
					}
					if (isInitialLoad)
					{
						_regionsPingModel.SetRegionsSearchInProgress(isInProgress: false);
					}
					yield return pingRoutineDelay;
				}
				_pingRoutine = null;
			}

			private async Task<List<RegionInfo>> GetAvailableRegions(CancellationToken cancellation = default(CancellationToken))
			{
				return await NetworkRunner.GetAvailableRegions(null, cancellation);
			}

			private void SetFixedRegion(string region)
			{
				_currentSettingsModel.FixedRegion = region;
				_savingService.SaveDataForGroup(SavingGroup.Settings);
			}

			private string ResolveFixedRegion()
			{
				if (!_currentSettingsModel.HasFixedRegion)
				{
					return "eu";
				}
				return _currentSettingsModel.FixedRegion;
			}
		}
	}
