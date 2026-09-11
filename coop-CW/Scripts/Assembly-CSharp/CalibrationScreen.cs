using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.Settings.UI;

public class CalibrationScreen : RetrievableResourceSingleton<CalibrationScreen>, INavigationPage
{
	public GameObject selectLanguageScreen;

	public GameObject voiceChatScreen;

	public GameObject calibrationScreen;

	public GameObject spokenLanguageScreen;

	public Action onClosed;

	public RawImage calibrationTexture;

	public MainMenuMainPage mainPage;

	public GameObject voiceCell;

	public GameObject voiceModeCell;

	public VoiceSettingUICell voiceSettingUICell;

	public EnumSettingUI enumSettingsUI;

	public FloatSettingUI floatSettingUI;

	public EnumSettingUI selectedlanguageSettingUI;

	public EnumSettingUI preferredLanguageSettingUI;

	public EnumSettingUI secondaryLanguageSettingUI;

	public EnumSettingUI thirdLanguageSettingUI;

	private LanguageSetting languageSetting;

	private VoiceSetting voiceSetting;

	private VoiceChatModeSetting voiceChatModeSetting;

	private BrightnessSetting brightnessSetting;

	public TextMeshProUGUI m_ThisGameUsesVoiceText;

	public TextMeshProUGUI m_UseHeadPhonesText;

	public TextMeshProUGUI m_SelectMicrophoneText;

	public TextMeshProUGUI m_SelectVoiceText;

	public TextMeshProUGUI m_AdjustBrightnessText;

	public TextMeshProUGUI m_AdjustMonsterText;

	public TextMeshProUGUI m_BrightnessText;

	public TextMeshProUGUI m_ContinueText;

	public TextMeshProUGUI m_ContinueText2;

	[SerializeField]
	private List<GameObject> m_continueButtons;

	private int m_buttonIndex;

	public static void ShowVoiceChatScreen(Action onClosed = null)
	{
		RetrievableResourceSingleton<CalibrationScreen>.Instance.onClosed = onClosed;
		RetrievableResourceSingleton<CalibrationScreen>.Instance.selectLanguageScreen.SetActive(value: true);
		RetrievableResourceSingleton<CalibrationScreen>.Instance.selectedlanguageSettingUI.Setup(GameHandler.Instance.SettingsHandler.GetSetting<LanguageSetting>(), GameHandler.Instance.SettingsHandler);
		EventSystem.current.SetSelectedGameObject(RetrievableResourceSingleton<CalibrationScreen>.Instance.m_continueButtons[RetrievableResourceSingleton<CalibrationScreen>.Instance.m_buttonIndex]);
	}

	private IEnumerator Start()
	{
		yield return 2;
		voiceSetting = GameHandler.Instance.SettingsHandler.GetSetting<VoiceSetting>();
		voiceChatModeSetting = GameHandler.Instance.SettingsHandler.GetSetting<VoiceChatModeSetting>();
		brightnessSetting = GameHandler.Instance.SettingsHandler.GetSetting<BrightnessSetting>();
		voiceCell.SetActive(value: true);
		voiceSettingUICell.Setup(voiceSetting, GameHandler.Instance.SettingsHandler);
		m_SelectMicrophoneText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CalibrationSelectMic);
		m_SelectVoiceText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CalibrationSelectVoice);
		voiceModeCell.SetActive(value: true);
		enumSettingsUI.Setup(voiceChatModeSetting, GameHandler.Instance.SettingsHandler);
		m_ThisGameUsesVoiceText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CalibrationUseVoice);
		m_UseHeadPhonesText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CalibrationUseHeadphones);
		m_AdjustBrightnessText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CalibrationBrightness);
		m_AdjustMonsterText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CalibrationMonster);
		m_BrightnessText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Brightness);
		m_ContinueText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Continue);
		m_ContinueText2.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Continue);
	}

	public void GoToVoiceChatScreen()
	{
		selectLanguageScreen.SetActive(value: false);
		voiceChatScreen.SetActive(value: true);
		m_buttonIndex++;
		EventSystem.current.SetSelectedGameObject(m_continueButtons[m_buttonIndex]);
	}

	public void GoToSpokenLanguageScreen()
	{
		spokenLanguageScreen.SetActive(value: true);
		voiceChatScreen.SetActive(value: false);
		preferredLanguageSettingUI.Setup(GameHandler.Instance.SettingsHandler.GetSetting<PreferredLanguageMatchmakingSetting>(), GameHandler.Instance.SettingsHandler);
		secondaryLanguageSettingUI.Setup(GameHandler.Instance.SettingsHandler.GetSetting<SecondLanguageMatchmakingSetting>(), GameHandler.Instance.SettingsHandler);
		thirdLanguageSettingUI.Setup(GameHandler.Instance.SettingsHandler.GetSetting<ThirdLanguageMatchmakingSetting>(), GameHandler.Instance.SettingsHandler);
		m_buttonIndex++;
		EventSystem.current.SetSelectedGameObject(m_continueButtons[m_buttonIndex]);
	}

	public void GoToCalibrationScreen()
	{
		calibrationScreen.SetActive(value: true);
		voiceChatScreen.SetActive(value: false);
		spokenLanguageScreen.SetActive(value: false);
		floatSettingUI.Setup(brightnessSetting, GameHandler.Instance.SettingsHandler);
		m_buttonIndex++;
		EventSystem.current.SetSelectedGameObject(m_continueButtons[m_buttonIndex]);
	}

	public void Hide()
	{
		voiceChatScreen.SetActive(value: false);
		calibrationScreen.SetActive(value: false);
		onClosed?.Invoke();
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (calibrationScreen.activeSelf)
		{
			calibrationTexture.texture = CalibrationTextureHolder.GetTexture(brightnessSetting);
		}
	}

	private void OnDisable()
	{
		mainPage.EnableMain();
	}

	public GameObject GetFirstSelectedGameObject()
	{
		return m_continueButtons[m_buttonIndex];
	}
}
