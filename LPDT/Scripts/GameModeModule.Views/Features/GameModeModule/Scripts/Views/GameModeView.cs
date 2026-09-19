using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Features.GameModeModule.Scripts.Views
{
	public class GameModeView : GameModeViewBase
	{
		[SerializeField]
		private GameObject _gameModeContainer;

		[SerializeField]
		private TMP_Dropdown _gameModeDropdown;

		public override event Action<string> OnGameModeChanged;

		protected override void OnEnable()
		{
			base.OnEnable();
			_gameModeDropdown.onValueChanged.AddListener(InvokeOnGameModeChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_gameModeDropdown.onValueChanged.RemoveListener(InvokeOnGameModeChanged);
		}

		public override void RefreshGameModeDropdown(GameModeType activeGameMode)
		{
			_gameModeDropdown.ClearOptions();
			List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
			List<GameModeType> list2 = new List<GameModeType>();
			foreach (GameModeType value in Enum.GetValues(typeof(GameModeType)))
			{
				if (value != GameModeType.None)
				{
					list.Add(new TMP_Dropdown.OptionData(value.ToString()));
					list2.Add(value);
				}
			}
			_gameModeDropdown.AddOptions(list);
			int num = list2.IndexOf(activeGameMode);
			if (num < 0)
			{
				num = 0;
			}
			_gameModeDropdown.SetValueWithoutNotify(num);
			_gameModeDropdown.RefreshShownValue();
		}

		public override void SetGameModeContainerActive(bool isActive)
		{
			_gameModeContainer.SetActive(isActive);
		}

		private void InvokeOnGameModeChanged(int index)
		{
			string text = _gameModeDropdown.options[index].text;
			OnGameModeChanged?.Invoke(text);
		}
	}
}
