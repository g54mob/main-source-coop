using System;
using System.Collections.Generic;
using System.Globalization;
using EvilCore.Localization;
using EvilCore.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.MainMenu.Panels
{
	public class LoadGamePanel : MonoBehaviour
	{
		[Header("Slot List")]
		[SerializeField]
		private RectTransform slotListContent;

		[SerializeField]
		private SaveSlotEntryUI slotEntryPrefab;

		[Header("Status")]
		[SerializeField]
		private TextMeshProUGUI statusText;

		[Header("Actions")]
		[SerializeField]
		private Button backButton;

		[Inject]
		private IMainMenuUIManager _uiManager;

		[Inject]
		private INetworkManager _networkManager;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IGameSaveService _gameSaveService;

		private readonly List<SaveSlotEntryUI> _entries = new List<SaveSlotEntryUI>();

		private void Start()
		{
			backButton?.onClick.AddListener(delegate
			{
				_uiManager.ShowMainPanel();
			});
		}

		public void OnPanelShown()
		{
			RefreshSlots();
		}

		private string L(string key)
		{
			return _localizationService?.Localize(key) ?? key;
		}

		private void RefreshSlots()
		{
			ClearEntries();
			SaveSlotInfo[] array = ((_gameSaveService != null) ? _gameSaveService.GetSaveSlots() : Array.Empty<SaveSlotInfo>());
			if (array.Length == 0)
			{
				if (statusText != null)
				{
					statusText.text = L("@main_menu.load_empty");
				}
				return;
			}
			if (statusText != null)
			{
				statusText.text = string.Empty;
			}
			SaveSlotInfo[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				SaveSlotInfo slot = array2[i];
				SaveSlotEntryUI saveSlotEntryUI = UnityEngine.Object.Instantiate(slotEntryPrefab, slotListContent);
				saveSlotEntryUI.Setup(slot, FormatDate(slot), FormatPlaytime(slot.PlaytimeSeconds), OnContinueClicked, OnDeleteClicked);
				_entries.Add(saveSlotEntryUI);
			}
		}

		private string FormatDate(SaveSlotInfo slot)
		{
			DateTime lastSavedLocal = slot.LastSavedLocal;
			if (lastSavedLocal == DateTime.MinValue)
			{
				return string.Empty;
			}
			if (_localizationService != null)
			{
				return _localizationService.FormatDate(lastSavedLocal) + " " + _localizationService.FormatTime(lastSavedLocal.Hour, lastSavedLocal.Minute);
			}
			return lastSavedLocal.ToString("g", CultureInfo.CurrentCulture);
		}

		private static string FormatPlaytime(float seconds)
		{
			int num = Mathf.Max(0, Mathf.RoundToInt(seconds / 60f));
			int num2 = num / 60;
			int num3 = num % 60;
			if (num2 <= 0)
			{
				return $"{num3}m";
			}
			return $"{num2}h {num3}m";
		}

		private void OnContinueClicked(SaveSlotInfo slot)
		{
			_uiManager.ShowLoadingGame();
			_networkManager.StartSinglePlayerContinue(slot.SlotId);
		}

		private void OnDeleteClicked(SaveSlotInfo slot)
		{
			_gameSaveService.DeleteSaveSlot(slot.SlotId);
			RefreshSlots();
		}

		private void ClearEntries()
		{
			foreach (SaveSlotEntryUI entry in _entries)
			{
				if (entry != null)
				{
					UnityEngine.Object.Destroy(entry.gameObject);
				}
			}
			_entries.Clear();
		}

		private void OnDestroy()
		{
			backButton?.onClick.RemoveAllListeners();
			ClearEntries();
		}
	}
}
