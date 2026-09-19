using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsView : SettingsViewBase
	{
		private static readonly int _selectedId = Animator.StringToHash("IsSelected");

		private readonly Dictionary<SettingsTab, SelectableSettingsTab> _tabs = new Dictionary<SettingsTab, SelectableSettingsTab>();

		[SerializeField]
		private SelectableSettingsTab _generalTab;

		[SerializeField]
		private SelectableSettingsTab _graphicsTab;

		[SerializeField]
		private SelectableSettingsTab _audioTab;

		[SerializeField]
		private SelectableSettingsTab _playersAudioTab;

		[SerializeField]
		private Toggle _generalButton;

		[SerializeField]
		private Toggle _graphicsButton;

		[SerializeField]
		private Toggle _audioButton;

		[SerializeField]
		private Toggle _playersAudioButton;

		[SerializeField]
		private Button _backButton;

		[SerializeField]
		private Button _saveButton;

		[SerializeField]
		private Button _reportButton;

		[SerializeField]
		private Animator _generalButtonAnimator;

		[SerializeField]
		private Animator _graphicsButtonAnimator;

		[SerializeField]
		private Animator _audioButtonAnimator;

		[SerializeField]
		private Animator _playersAudioButtonAnimator;

		public override Dictionary<SettingsTab, SelectableSettingsTab> TabsMap => _tabs;

		private void Awake()
		{
			RegisterTabs();
		}

		public override void SetActiveTab(SettingsTab tab)
		{
			ShowTab(tab);
		}

		public override bool TrySetSaveButtonInteractable(bool interactable)
		{
			if (_saveButton.interactable == interactable)
			{
				return false;
			}
			_saveButton.interactable = interactable;
			return true;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			SubscribeToTabButtons();
			_backButton.onClick.AddListener(BackPressed);
			_saveButton.onClick.AddListener(ApplySavesClicked);
			_reportButton.onClick.AddListener(ReportClicked);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			UnsubscribeFromTabButtons();
			_backButton.onClick.RemoveListener(BackPressed);
			_saveButton.onClick.RemoveListener(ApplySavesClicked);
			_reportButton.onClick.RemoveListener(ReportClicked);
		}

		private void ApplySavesClicked()
		{
			OnApply?.Invoke();
		}

		private void BackPressed()
		{
			OnClose?.Invoke();
		}

		private void ReportClicked()
		{
			OnSendReport?.Invoke();
		}

		private void ShowTab(SettingsTab tab)
		{
			HideAllTabs();
			if (!_tabs[tab].gameObject.activeSelf)
			{
				SetTabButtonSelected(tab);
				_tabs[tab].gameObject.SetActive(value: true);
			}
		}

		private void SetTabButtonSelected(SettingsTab tab)
		{
			switch (tab)
			{
			case SettingsTab.General:
				_generalButtonAnimator.SetBool(_selectedId, value: true);
				_graphicsButtonAnimator.SetBool(_selectedId, value: false);
				_audioButtonAnimator.SetBool(_selectedId, value: false);
				_playersAudioButtonAnimator.SetBool(_selectedId, value: false);
				break;
			case SettingsTab.Graphics:
				_generalButtonAnimator.SetBool(_selectedId, value: false);
				_graphicsButtonAnimator.SetBool(_selectedId, value: true);
				_audioButtonAnimator.SetBool(_selectedId, value: false);
				_playersAudioButtonAnimator.SetBool(_selectedId, value: false);
				break;
			case SettingsTab.Audio:
				_generalButtonAnimator.SetBool(_selectedId, value: false);
				_graphicsButtonAnimator.SetBool(_selectedId, value: false);
				_audioButtonAnimator.SetBool(_selectedId, value: true);
				_playersAudioButtonAnimator.SetBool(_selectedId, value: false);
				break;
			case SettingsTab.PlayersAudio:
				_generalButtonAnimator.SetBool(_selectedId, value: false);
				_graphicsButtonAnimator.SetBool(_selectedId, value: false);
				_audioButtonAnimator.SetBool(_selectedId, value: false);
				_playersAudioButtonAnimator.SetBool(_selectedId, value: true);
				break;
			default:
				throw new ArgumentOutOfRangeException("tab", tab, null);
			}
		}

		private void HideAllTabs()
		{
			foreach (KeyValuePair<SettingsTab, SelectableSettingsTab> tab in _tabs)
			{
				tab.Value.gameObject.SetActive(value: false);
			}
		}

		private void SubscribeToTabButtons()
		{
			_generalButton.onValueChanged.AddListener(OnGeneralButtonClicked);
			_graphicsButton.onValueChanged.AddListener(OnGraphicsButtonClicked);
			_audioButton.onValueChanged.AddListener(OnAudioButtonClicked);
			_playersAudioButton.onValueChanged.AddListener(OnPlayersAudioButtonClicked);
		}

		private void UnsubscribeFromTabButtons()
		{
			_generalButton.onValueChanged.RemoveListener(OnGeneralButtonClicked);
			_graphicsButton.onValueChanged.RemoveListener(OnGraphicsButtonClicked);
			_audioButton.onValueChanged.RemoveListener(OnAudioButtonClicked);
			_playersAudioButton.onValueChanged.RemoveListener(OnPlayersAudioButtonClicked);
		}

		private void OnGeneralButtonClicked(bool isOn)
		{
			if (isOn)
			{
				HandleOnTabButtonClicked(SettingsTab.General);
			}
		}

		private void OnGraphicsButtonClicked(bool isOn)
		{
			if (isOn)
			{
				HandleOnTabButtonClicked(SettingsTab.Graphics);
			}
		}

		private void OnAudioButtonClicked(bool isOn)
		{
			if (isOn)
			{
				HandleOnTabButtonClicked(SettingsTab.Audio);
			}
		}

		private void OnPlayersAudioButtonClicked(bool isOn)
		{
			if (isOn)
			{
				HandleOnTabButtonClicked(SettingsTab.PlayersAudio);
			}
		}

		private void RegisterTabs()
		{
			if (_generalTab != null)
			{
				_tabs.Add(SettingsTab.General, _generalTab);
			}
			if (_graphicsTab != null)
			{
				_tabs.Add(SettingsTab.Graphics, _graphicsTab);
			}
			if (_audioTab != null)
			{
				_tabs.Add(SettingsTab.Audio, _audioTab);
			}
			if (_playersAudioTab != null)
			{
				_tabs.Add(SettingsTab.PlayersAudio, _playersAudioTab);
			}
		}

		private void HandleOnTabButtonClicked(SettingsTab controls)
		{
			OnTabButtonClicked?.Invoke(controls);
		}
	}
}
