using System;
using System.Collections.Generic;
using Fusion.Photon.Realtime;

namespace Features.SettingsMenuModule.Scripts.Data
{
	public class RegionsPingModel
	{
		private const string BASE_REGION = "eu";

		private List<RegionInfo> _regions = new List<RegionInfo>();

		private string _currentRegion = "eu";

		private bool _isRegionsSearchInProgress;

		public IReadOnlyList<RegionInfo> Regions => _regions;

		public bool IsRegionsSearchInProgress => _isRegionsSearchInProgress;

		public string CurrentRegion
		{
			get
			{
				return _currentRegion;
			}
			set
			{
				_currentRegion = value;
				this.OnCurrentRegionUpdated?.Invoke();
			}
		}

		public event Action OnRegionsInfoUpdated;

		public event Action OnCurrentRegionUpdated;

		public event Action<bool> OnRegionsSearchInProgressChanged;

		public void SetNewRegionsInfo(List<RegionInfo> regions)
		{
			_regions = regions;
			this.OnRegionsInfoUpdated?.Invoke();
		}

		public void SetRegionsSearchInProgress(bool isInProgress)
		{
			if (_isRegionsSearchInProgress != isInProgress)
			{
				_isRegionsSearchInProgress = isInProgress;
				this.OnRegionsSearchInProgressChanged?.Invoke(isInProgress);
			}
		}
	}
}
