using System;
using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Extensions;
using EvilCore.UI.Settings.Tabs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EvilCore.UI.Settings
{
	public class SettingsPanel : MonoBehaviour
	{
		[Header("Tab Buttons")]
		[SerializeField]
		private List<Button> tabButtons = new List<Button>();

		[Header("Tab Contents")]
		[SerializeField]
		private List<GameObject> tabContents = new List<GameObject>();

		[Header("Tab Labels")]
		[SerializeField]
		private List<TextMeshProUGUI> tabLabels = new List<TextMeshProUGUI>();

		[Header("Navigation")]
		[SerializeField]
		private Button backButton;

		[Header("Visual")]
		[SerializeField]
		private Color activeTabColor = Color.white;

		[SerializeField]
		private Color inactiveTabColor = new Color(0.6f, 0.6f, 0.6f, 1f);

		[Header("Sound")]
		[SerializeField]
		private SoundID tabChangeSound;

		[SerializeField]
		private SoundID backSound;

		[Inject]
		private IAudioManager _audioManager;

		private int _activeTabIndex = -1;

		private Action _onBackPressed;

		private bool _tabsInitialized;

		public void Initialize(Action onBackPressed)
		{
			_onBackPressed = onBackPressed;
			base.gameObject.InjectGameObject();
			backButton?.onClick.AddListener(OnBackClicked);
			for (int i = 0; i < tabButtons.Count; i++)
			{
				int index = i;
				tabButtons[i]?.onClick.AddListener(delegate
				{
					if (index != _activeTabIndex)
					{
						PlaySound(tabChangeSound);
					}
					ShowTab(index);
				});
			}
			InitializeAllTabs();
		}

		private void OnDestroy()
		{
			backButton?.onClick.RemoveAllListeners();
			foreach (Button tabButton in tabButtons)
			{
				tabButton?.onClick.RemoveAllListeners();
			}
		}

		public void OnPanelShown()
		{
			_activeTabIndex = -1;
			ShowTab(0);
		}

		public void ShowTab(int tabIndex)
		{
			if (tabIndex < 0 || tabIndex >= tabContents.Count || tabIndex == _activeTabIndex)
			{
				return;
			}
			for (int i = 0; i < tabContents.Count; i++)
			{
				bool flag = i == tabIndex;
				if (tabContents[i] != null)
				{
					tabContents[i].SetActive(flag);
				}
				if (i < tabLabels.Count && tabLabels[i] != null)
				{
					tabLabels[i].color = (flag ? activeTabColor : inactiveTabColor);
				}
			}
			_activeTabIndex = tabIndex;
		}

		private void OnBackClicked()
		{
			PlaySound(backSound);
			_onBackPressed?.Invoke();
		}

		private void InitializeAllTabs()
		{
			if (_tabsInitialized)
			{
				return;
			}
			_tabsInitialized = true;
			foreach (GameObject tabContent in tabContents)
			{
				if (!(tabContent == null))
				{
					tabContent.GetComponent<SettingsDisplayTab>()?.Initialize();
					tabContent.GetComponent<SettingsGraphicsTab>()?.Initialize();
					tabContent.GetComponent<SettingsAudioTab>()?.Initialize();
					tabContent.GetComponent<SettingsGameplayTab>()?.Initialize();
					tabContent.GetComponent<SettingsControlsTab>()?.Initialize();
					tabContent.GetComponent<SettingsLanguageTab>()?.Initialize();
				}
			}
		}

		private void PlaySound(SoundID sound)
		{
			if (sound.IsValid())
			{
				_audioManager?.PlayOneShotUI(sound);
			}
		}
	}
}
