using System;
using System.Collections.Generic;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsModel
	{
		private readonly Dictionary<SettingsTab, SelectableSettingsTab> _activeTabs = new Dictionary<SettingsTab, SelectableSettingsTab>();

		private SettingsTab _activeTab;

		public IReadOnlyDictionary<SettingsTab, SelectableSettingsTab> ActiveTabs => _activeTabs;

		public event Action<SettingsTab> OnSettingsTabSelected;

		public void SetActiveTab(SettingsTab tab)
		{
			_activeTab = tab;
			this.OnSettingsTabSelected?.Invoke(_activeTab);
		}

		public void RegisterSelectableSettingsTab(SettingsTab tab, SelectableSettingsTab settingsTab)
		{
			_activeTabs.Add(tab, settingsTab);
		}

		public void UnregisterSelectableSettingsTab(SettingsTab tab)
		{
			_activeTabs.Remove(tab);
		}
	}
}
