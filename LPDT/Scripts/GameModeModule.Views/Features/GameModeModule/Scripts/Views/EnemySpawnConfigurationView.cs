using System;
using System.Collections.Generic;
using Features.AIModule.Scripts;
using TMPro;
using UnityEngine;

namespace Features.GameModeModule.Scripts.Views
{
	public class EnemySpawnConfigurationView : EnemyConfigurationViewBase
	{
		[SerializeField]
		private GameObject configurationContainer;

		[SerializeField]
		private TMP_Dropdown configurationDropDown;

		public override event Action<string> OnEnemyConfigurationChanged;

		protected override void OnEnable()
		{
			base.OnEnable();
			configurationDropDown.onValueChanged.AddListener(InvokeConfigurationChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			configurationDropDown.onValueChanged.RemoveListener(InvokeConfigurationChanged);
		}

		public override void RefreshEnemyConfigurationDropdown(EnemySpawnConfigurationType activeGameMode)
		{
			configurationDropDown.ClearOptions();
			List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
			List<EnemySpawnConfigurationType> list2 = new List<EnemySpawnConfigurationType>();
			foreach (EnemySpawnConfigurationType value in Enum.GetValues(typeof(EnemySpawnConfigurationType)))
			{
				if (value != EnemySpawnConfigurationType.None)
				{
					list.Add(new TMP_Dropdown.OptionData(value.ToString()));
					list2.Add(value);
				}
			}
			configurationDropDown.AddOptions(list);
			int num = list2.IndexOf(activeGameMode);
			if (num < 0)
			{
				num = 0;
			}
			configurationDropDown.SetValueWithoutNotify(num);
			configurationDropDown.RefreshShownValue();
		}

		public override void SetEnemyConfigurationContainerActive(bool isActive)
		{
			configurationContainer.SetActive(isActive);
		}

		private void InvokeConfigurationChanged(int index)
		{
			string text = configurationDropDown.options[index].text;
			OnEnemyConfigurationChanged?.Invoke(text);
		}
	}
}
