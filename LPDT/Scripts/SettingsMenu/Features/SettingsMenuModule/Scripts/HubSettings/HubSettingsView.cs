using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public class HubSettingsView : HubSettingsViewBase
	{
		private static readonly int _selectedId = Animator.StringToHash("IsSelected");

		private readonly Dictionary<HubSettingsTab, SelectableSettingsTab> _tabs = new Dictionary<HubSettingsTab, SelectableSettingsTab>();

		[SerializeField]
		private SelectableSettingsTab _generalTab;

		[SerializeField]
		private SelectableSettingsTab _audioTab;

		[SerializeField]
		private Toggle _generalButton;

		[SerializeField]
		private Toggle _audioButton;

		[SerializeField]
		private Button _backButton;

		[SerializeField]
		private Button _saveButton;

		[SerializeField]
		private Animator _generalButtonAnimator;

		[SerializeField]
		private Animator _audioButtonAnimator;

		[SerializeField]
		private TMP_Text _headerText;

		[SerializeField]
		private TMP_Text _backButtonText;

		[SerializeField]
		private TMP_Text _saveButtonText;

		[SerializeField]
		private string _headerLocalizationKey = "Settings";

		[SerializeField]
		private string _backButtonLocalizationKey = "Back";

		[SerializeField]
		private string _saveButtonLocalizationKey = "Save";

		[SerializeField]
		private string _generalButtonLocalizationKey = "General";

		[SerializeField]
		private string _audioButtonLocalizationKey = "Audio";

		[SerializeField]
		private TMP_Text _generalHeaderText;

		[SerializeField]
		private TMP_Text _audioHeaderText;

		private void Awake()
		{
			RegisterTabs();
		}

		public override void SetActiveTab(HubSettingsTab tab)
		{
			ShowTab(tab);
		}

		public override void SetSaveButtonInteractable(bool interactable)
		{
			_saveButton.interactable = interactable;
		}

		protected override void OnEnable()
		{
			UpdateLocalization();
			SubscribeToTabButtons();
			_backButton.onClick.AddListener(BackPressed);
			_saveButton.onClick.AddListener(ApplySavesClicked);
		}

		protected override void OnDisable()
		{
			UnsubscribeFromTabButtons();
			_backButton.onClick.RemoveListener(BackPressed);
			_saveButton.onClick.RemoveListener(ApplySavesClicked);
		}

		private void ApplySavesClicked()
		{
			OnApply?.Invoke();
		}

		private void BackPressed()
		{
			OnClose?.Invoke();
		}

		private void ShowTab(HubSettingsTab tab)
		{
			HideAllTabs();
			if (!_tabs[tab].gameObject.activeSelf)
			{
				SetTabButtonSelected(tab);
				_tabs[tab].gameObject.SetActive(value: true);
			}
		}

		public void SetTabButtonSelected(HubSettingsTab tab)
		{
			switch (tab)
			{
			case HubSettingsTab.General:
				_generalButtonAnimator.SetBool(_selectedId, value: true);
				_audioButtonAnimator.SetBool(_selectedId, value: false);
				break;
			case HubSettingsTab.Audio:
				_generalButtonAnimator.SetBool(_selectedId, value: false);
				_audioButtonAnimator.SetBool(_selectedId, value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("tab", tab, null);
			}
		}

		private void HideAllTabs()
		{
			foreach (KeyValuePair<HubSettingsTab, SelectableSettingsTab> tab in _tabs)
			{
				tab.Value.gameObject.SetActive(value: false);
			}
		}

		private void SubscribeToTabButtons()
		{
			_generalButton.onValueChanged.AddListener(OnGeneralButtonClicked);
			_audioButton.onValueChanged.AddListener(OnAudioSettingsButtonClicked);
		}

		private void UnsubscribeFromTabButtons()
		{
			_generalButton.onValueChanged.RemoveListener(OnGeneralButtonClicked);
			_audioButton.onValueChanged.RemoveListener(OnAudioSettingsButtonClicked);
		}

		private void OnGeneralButtonClicked(bool isOn)
		{
			if (isOn)
			{
				HandleOnTabButtonClicked(HubSettingsTab.General);
			}
		}

		private void OnAudioSettingsButtonClicked(bool isOn)
		{
			if (isOn)
			{
				HandleOnTabButtonClicked(HubSettingsTab.Audio);
			}
		}

		private void RegisterTabs()
		{
			_tabs.Add(HubSettingsTab.General, _generalTab);
			_tabs.Add(HubSettingsTab.Audio, _audioTab);
		}

		private void UpdateLocalization()
		{
			_headerText.SetText(_headerLocalizationKey);
			_backButtonText.SetText(_backButtonLocalizationKey);
			_saveButtonText.SetText(_saveButtonLocalizationKey);
			UpdateTabsLocalization();
		}

		private void UpdateTabsLocalization()
		{
			_generalHeaderText.SetText(_generalButtonLocalizationKey);
			_audioHeaderText.SetText(_audioButtonLocalizationKey);
		}

		private void HandleOnTabButtonClicked(HubSettingsTab controls)
		{
			OnTabButtonClicked?.Invoke(controls);
		}
	}
}
