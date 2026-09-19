using System;
using System.Collections.Generic;
using Fusion.Photon.Realtime;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public abstract class LobbySettingsItemViewBase : ViewBehaviour
	{
		public Action<string> OnRegionChanged;

		public Action OnDropdownShown;

		public abstract void SetRegionsInfo(IReadOnlyList<RegionInfo> regions);

		public abstract void SelectCurrentRegion(string appSettingsFixedRegion);

		public abstract void SetRegionSearchInProgress(bool isInProgress, string loadingCaption);
	}
}
