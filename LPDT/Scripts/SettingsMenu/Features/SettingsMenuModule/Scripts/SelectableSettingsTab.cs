using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class SelectableSettingsTab : MonoBehaviour
	{
		private SettingsModel _settingsModel;

		[field: SerializeField]
		public List<Selectable> Selectables { get; private set; }

		[field: SerializeField]
		public Selectable PrioritySelectable { get; private set; }

		[field: SerializeField]
		public SettingsTab SettingsTab { get; private set; }

		public void UpdatePrioritySelectable(Selectable selectable)
		{
			PrioritySelectable = selectable;
		}

		[Inject]
		private void InjectDependencies(SettingsModel settingsModel)
		{
			_settingsModel = settingsModel;
		}

		private void Awake()
		{
			_settingsModel.RegisterSelectableSettingsTab(SettingsTab, this);
		}

		private void OnDestroy()
		{
			_settingsModel.UnregisterSelectableSettingsTab(SettingsTab);
		}
	}
}
