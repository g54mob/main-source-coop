using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class SelectableSettingsTabNavigationUpdater : MonoBehaviour
	{
		[SerializeField]
		private Button _targetButton;

		private SettingsModel _settingsModel;

		private void Awake()
		{
			_settingsModel.OnSettingsTabSelected += OnSettingsTabSelected;
		}

		private void OnDestroy()
		{
			_settingsModel.OnSettingsTabSelected -= OnSettingsTabSelected;
		}

		[Inject]
		private void InjectDependencies(SettingsModel settingsModel)
		{
			_settingsModel = settingsModel;
		}

		private void OnSettingsTabSelected(SettingsTab settingsTab)
		{
			Navigation navigation = _targetButton.navigation;
			if (_settingsModel.ActiveTabs.TryGetValue(settingsTab, out var value))
			{
				navigation.selectOnUp = value.PrioritySelectable;
				_targetButton.navigation = navigation;
			}
		}
	}
}
