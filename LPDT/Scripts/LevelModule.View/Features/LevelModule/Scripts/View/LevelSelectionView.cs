using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Features.LevelModule.Scripts.View
{
	public class LevelSelectionView : LevelSelectionViewBase
	{
		[SerializeField]
		private GameObject _levelSelectionContainer;

		[SerializeField]
		private TMP_Dropdown _levelSelectionDropdown;

		[SerializeField]
		private List<LevelType> _levelTypes;

		public override event Action<string> OnSelectedLevelChanged;

		protected override void OnEnable()
		{
			base.OnEnable();
			_levelSelectionDropdown.onValueChanged.AddListener(InvokeOnLevelSelected);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_levelSelectionDropdown.onValueChanged.RemoveListener(InvokeOnLevelSelected);
		}

		public override void RefreshLevelSelectionDropdown(LevelType currentLevelType)
		{
			_levelSelectionDropdown.ClearOptions();
			List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
			List<LevelType> list2 = new List<LevelType>();
			foreach (LevelType levelType in _levelTypes)
			{
				list.Add(new TMP_Dropdown.OptionData(Regex.Replace(levelType.ToString(), "(?<!^)([A-Z])", " $1")));
				list2.Add(levelType);
			}
			_levelSelectionDropdown.AddOptions(list);
			int num = list2.IndexOf(currentLevelType);
			if (num < 0)
			{
				num = 0;
			}
			_levelSelectionDropdown.SetValueWithoutNotify(num);
			_levelSelectionDropdown.RefreshShownValue();
		}

		public override void SetLevelSelectionActive(bool isActive)
		{
			_levelSelectionContainer.SetActive(isActive);
		}

		public override void InitializeLevelSelectionDropdown()
		{
			RefreshLevelSelectionDropdown(_levelTypes.First());
			InvokeOnLevelSelected(0);
		}

		private void InvokeOnLevelSelected(int index)
		{
			string text = _levelSelectionDropdown.options[index].text;
			OnSelectedLevelChanged?.Invoke(text);
		}
	}
}
