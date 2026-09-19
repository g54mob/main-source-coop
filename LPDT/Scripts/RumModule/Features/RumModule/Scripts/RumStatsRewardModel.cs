using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts;

namespace Features.RumModule.Scripts
{
	public class RumStatsRewardModel : ISessionCleanup
	{
		private List<TemporalRumData> _activeTemporalRumsData = new List<TemporalRumData>();

		private List<RumData> _activeRumsData = new List<RumData>();

		private bool _isRestoring;

		public IReadOnlyList<TemporalRumData> ActiveTemporalRumsData => _activeTemporalRumsData;

		public IReadOnlyList<RumData> ActiveRumsData => _activeRumsData;

		public event Action OnRumDataChanged;

		public event Action OnTemporalRumDataChanged;

		public event Action<RumData> OnRumDataAdded;

		public event Action<TemporalRumData> OnTemporalRumDataAdded;

		public void Cleanup()
		{
			_activeTemporalRumsData.Clear();
			this.OnTemporalRumDataChanged?.Invoke();
			_activeRumsData.Clear();
			this.OnRumDataChanged?.Invoke();
		}

		public void AddTemporalRumsData(TemporalRumData temporalRumData)
		{
			_activeTemporalRumsData.Add(temporalRumData);
			this.OnTemporalRumDataChanged?.Invoke();
			this.OnTemporalRumDataAdded?.Invoke(temporalRumData);
			PersistToSession();
		}

		public void AddRumsData(RumData currentRumData)
		{
			_activeRumsData.Add(currentRumData);
			this.OnRumDataChanged?.Invoke();
			this.OnRumDataAdded?.Invoke(currentRumData);
			PersistToSession();
		}

		public void RemoveTemporalRumsData(TemporalRumData temporalRum)
		{
			_activeTemporalRumsData.Remove(temporalRum);
			this.OnTemporalRumDataChanged?.Invoke();
			PersistToSession();
		}

		public void RemoveRumsData(RumData rumData)
		{
			_activeRumsData.Remove(rumData);
			this.OnRumDataChanged?.Invoke();
			PersistToSession();
		}

		public void RestoreFromSession(RumConfiguration rumConfiguration, IPlayerStatsUpgradeService playerStatsUpgradeService)
		{
			if (!PlayerSessionPrefs.TryGetSavedRums(out var activeRums))
			{
				return;
			}
			_isRestoring = true;
			foreach (RumSessionEntry entry in activeRums)
			{
				RumData rumData = rumConfiguration.RumsData.Find((RumData configRumData) => configRumData.RumType == (RumType)entry.RumType);
				if (rumData == null)
				{
					continue;
				}
				List<ModifierStatsData> list = new List<ModifierStatsData>();
				foreach (ModifierStatsData rumModifier in rumData.RumModifiers)
				{
					list.Add(rumModifier);
					playerStatsUpgradeService.ApplyModifierStatsSynchronized(rumModifier);
				}
				if (rumData.WithDuration)
				{
					AddTemporalRumsData(new TemporalRumData(rumData, list)
					{
						Duration = entry.Duration
					});
				}
				else
				{
					AddRumsData(rumData);
				}
			}
			_isRestoring = false;
			PersistToSession();
		}

		private void PersistToSession()
		{
			if (_isRestoring)
			{
				return;
			}
			List<RumSessionEntry> list = new List<RumSessionEntry>();
			foreach (RumData activeRumsDatum in _activeRumsData)
			{
				list.Add(new RumSessionEntry
				{
					RumType = (int)activeRumsDatum.RumType,
					Duration = 0f,
					IsTemporal = false
				});
			}
			foreach (TemporalRumData activeTemporalRumsDatum in _activeTemporalRumsData)
			{
				list.Add(new RumSessionEntry
				{
					RumType = (int)activeTemporalRumsDatum.RumData.RumType,
					Duration = activeTemporalRumsDatum.Duration,
					IsTemporal = true
				});
			}
			PlayerSessionPrefs.SaveActiveRums(list);
		}
	}
}
