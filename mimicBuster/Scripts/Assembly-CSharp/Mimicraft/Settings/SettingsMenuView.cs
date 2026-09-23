using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.Settings
{
	public class SettingsMenuView : MonoBehaviour
	{
		[Serializable]
		public struct Category
		{
			public Button Button;

			public GameObject Panel;
		}

		[Header("Kök")]
		[Tooltip("Panelin tamamını taşıyan obje. Kapalı başlamalı - ayarlar ekranı istenmeden açılmaz.")]
		[SerializeField]
		private GameObject panelRoot;

		[SerializeField]
		private Button closeButton;

		[Header("Kategoriler")]
		[Tooltip("Sekmeler. Sıra Inspector'daki sıradır; ilki açılışta seçilir. Yeni bir kategori eklemek için buraya bir satır eklemek yeterli.")]
		[SerializeField]
		private Category[] categories = Array.Empty<Category>();

		[Tooltip("Seçili sekme butonunun rengi.")]
		[SerializeField]
		private Color selectedTabColor = new Color(0.25f, 0.45f, 0.25f, 0.95f);

		[SerializeField]
		private Color unselectedTabColor = new Color(0.18f, 0.18f, 0.18f, 0.9f);

		[Header("Görüntü")]
		[SerializeField]
		private TMP_Dropdown displayDropdown;

		[SerializeField]
		private TMP_Dropdown windowModeDropdown;

		[SerializeField]
		private TMP_Dropdown aspectDropdown;

		[SerializeField]
		private TMP_Dropdown resolutionDropdown;

		[SerializeField]
		private TMP_Dropdown vsyncDropdown;

		[Tooltip("Kare sınırı. VSync açıkken bu ayar hiçbir şey yapmaz - ikisi aynı şeyi isteyen iki ayrı mekanizma, ve uygulayıcı VSync'i seçer. Bkz. SettingsApplier.ApplyPerformance.")]
		[SerializeField]
		private TMP_Dropdown frameRateDropdown;

		[SerializeField]
		private TMP_Dropdown qualityDropdown;

		[Tooltip("Birinci şahıs görüş açısı. Oyuncu buna dokunana kadar kamera prefab'daki açıda kalır.")]
		[SerializeField]
		private Slider fieldOfViewSlider;

		[SerializeField]
		private TextMeshProUGUI fieldOfViewValueLabel;

		[Header("Ses")]
		[SerializeField]
		private Slider masterVolumeSlider;

		[SerializeField]
		private Slider sfxVolumeSlider;

		[SerializeField]
		private Slider musicVolumeSlider;

		[Tooltip("Sesli sohbette gelen seslerin seviyesi. Efektlerden ayrı, çünkü ikisi ters sebeplerle kısılır: efektler insanları duymak için, insanlar efektleri duymak için.")]
		[SerializeField]
		private Slider voiceVolumeSlider;

		[Tooltip("Kendi mikrofonunun kazancı. 1 = kaydedildiği gibi, 2 = iki katı.")]
		[SerializeField]
		private Slider micVolumeSlider;

		[Tooltip("Sesli sohbeti tamamen kapatır - kapalıyken hiç mikrofon açılmaz.")]
		[SerializeField]
		private Toggle voiceEnabledToggle;

		[Tooltip("Kayıt cihazı. Unity'nin listesinden doldurulur; ÇIKIŞ cihazı için karşılığı yok - Unity çıkış aygıtı seçmeyi desteklemiyor, oyun Windows'un varsayılanını kullanır.")]
		[SerializeField]
		private TMP_Dropdown micDeviceDropdown;

		[SerializeField]
		private TextMeshProUGUI masterVolumeValueLabel;

		[SerializeField]
		private TextMeshProUGUI sfxVolumeValueLabel;

		[SerializeField]
		private TextMeshProUGUI musicVolumeValueLabel;

		[Tooltip("Voice Volume slider'ının değer yazısı - yüzde olarak.")]
		[SerializeField]
		private TextMeshProUGUI voiceVolumeValueLabel;

		[Tooltip("Mic Volume slider'ının değer yazısı - yüzde olarak. Slider 0-2 aralığında olduğu için %100 = kaydedildiği gibi, %200 = iki katı.")]
		[SerializeField]
		private TextMeshProUGUI micVolumeValueLabel;

		[Tooltip("Pencere odakta değilken sesi kes. Varsayılan olarak AÇIK - yan yana iki kopya çalıştırmak bu projede test etmenin normal yolu.")]
		[SerializeField]
		private Toggle muteWhenUnfocusedToggle;

		[Header("Oynanış")]
		[SerializeField]
		private Slider fpsSensitivitySlider;

		[SerializeField]
		private Slider tpsSensitivitySlider;

		[Tooltip("Silahın kendi dürbün çarpanının ÜSTÜNE biner, yerine geçmez - bkz. WeaponDefinition.ScopeSensitivityMultiplier.")]
		[SerializeField]
		private Slider scopeSensitivitySlider;

		[SerializeField]
		private TextMeshProUGUI fpsSensitivityValueLabel;

		[SerializeField]
		private TextMeshProUGUI tpsSensitivityValueLabel;

		[SerializeField]
		private TextMeshProUGUI scopeSensitivityValueLabel;

		[SerializeField]
		private Toggle invertLookYToggle;

		[SerializeField]
		private Toggle toggleCrouchToggle;

		[SerializeField]
		private Toggle toggleScopeToggle;

		[Tooltip("Nişan almayı (ADS) basılı tutmak yerine aç/kapa yapan toggle.")]
		[SerializeField]
		private Toggle toggleAdsToggle;

		[Tooltip("Toggle ADS'nin bütün satırı (etiketiyle birlikte). Features.AimDownSights kapalıyken gizlenir - olmayan bir özelliğin ayarı menüde durmasın. Boşsa sadece toggle gizlenir.")]
		[SerializeField]
		private GameObject toggleAdsRow;

		[Tooltip("Geliştirici konsolunu (` tuşu) açar. Varsayılan kapalı. Tek başına hile vermez - hile komutları ayrıca sv_cheats'e bağlı, o da lobinin kararı.")]
		[SerializeField]
		private Toggle developerConsoleToggle;

		[Tooltip("Anonim kullanım verisi gönderilsin mi (Unity Analytics). Varsayılan açık. Kapatınca toplama o anda durur ve silme istenir. Yanına ne gönderilip ne gönderilmediğini söyleyen bir satır koy: Gameplay.AnalyticsConsent.Note anahtarı.")]
		[SerializeField]
		private Toggle analyticsConsentToggle;

		[Tooltip("Güvenli sohbet: küfür ve hakaret içeren kelimeler sohbette yıldızlanır. Varsayılan AÇIK. Yalnızca bu oyuncunun ekranını etkiler; listesi Assets/Resources/ChatFilterWords.txt dosyasındadır.")]
		[SerializeField]
		private Toggle safeChatToggle;

		[SerializeField]
		private Slider screenShakeSlider;

		[SerializeField]
		private TextMeshProUGUI screenShakeValueLabel;

		[Header("Editör")]
		[SerializeField]
		private Slider editorZoomSlider;

		[SerializeField]
		private TextMeshProUGUI editorZoomValueLabel;

		[Tooltip("İmlecin altındaki voxelin çevresinde kaç hücre işaretlensin. 1 varsayılan ve uzaktan çalışırken imleci bulunur kılan şey; 0 tam köşeye nişan alırken gerekli - halka nişan alınan köşenin üstünü örtüyor.")]
		[SerializeField]
		private Slider hoverRadiusSlider;

		[SerializeField]
		private TextMeshProUGUI hoverRadiusValueLabel;

		[Tooltip("Renk kutucukları. Butona basınca ortak renk seçici açılır; Image olan kutucuk o an yürürlükteki rengi gösterir.")]
		[SerializeField]
		private Button extrudeGizmoColorButton;

		[SerializeField]
		private Image extrudeGizmoColorSwatch;

		[SerializeField]
		private Button transformGizmoColorButton;

		[SerializeField]
		private Image transformGizmoColorSwatch;

		[SerializeField]
		private Button editorGridColorButton;

		[SerializeField]
		private Image editorGridColorSwatch;

		[Tooltip("Üç renk kutucuğunun paylaştığı tek seçici. Boş bırakılırsa kutucuklar hiçbir şey yapmaz.")]
		[SerializeField]
		private SettingsColorPicker colorPicker;

		[Header("Dil")]
		[Tooltip("Şimdilik tek seçenek. Oyundaki her metin onu çizen kodun içinde yazılı, yani ikinci bir dil proje çapında bir iş - bu ayar o iş yapıldığında seçimin duracağı yer.")]
		[SerializeField]
		private TMP_Dropdown languageDropdown;

		private readonly List<Resolution> listedResolutions = new List<Resolution>();

		private readonly List<string> listedAspectIds = new List<string>();

		private readonly List<FullScreenMode> listedWindowModes = new List<FullScreenMode>();

		private static readonly int[] FrameRateOptions = new int[8] { 0, 30, 60, 75, 120, 144, 165, 240 };

		private static readonly int[] VSyncOptions = new int[3] { 0, 1, 2 };

		private readonly List<string> listedLanguages = new List<string>();

		private readonly List<string> listedMicDevices = new List<string>();

		private bool applyingToControls;

		private bool claimed;

		public static bool IsAnyOpen
		{
			get
			{
				if (Instance != null)
				{
					return Instance.IsOpen;
				}
				return false;
			}
		}

		public static SettingsMenuView Instance { get; private set; }

		public bool IsOpen
		{
			get
			{
				if (panelRoot != null)
				{
					return panelRoot.activeInHierarchy;
				}
				return false;
			}
		}

		private void OnEnable()
		{
			Loc.Changed += OnLanguageChanged;
		}

		private void OnDisable()
		{
			Loc.Changed -= OnLanguageChanged;
		}

		private void OnLanguageChanged()
		{
			if (IsOpen)
			{
				RebuildDisplayDropdown();
				RebuildWindowModeDropdown();
				RebuildAspectDropdown();
				RebuildVSyncDropdown();
				RebuildFrameRateDropdown();
				RebuildQualityDropdown();
				RebuildLanguageDropdown();
				RefreshValueLabels();
			}
		}

		private void Awake()
		{
			if (Instance == null || Instance == this)
			{
				Instance = this;
			}
			GameSettings.Load();
			if (closeButton != null)
			{
				closeButton.onClick.RemoveAllListeners();
				closeButton.onClick.AddListener(Close);
			}
			for (int i = 0; i < categories.Length; i++)
			{
				int index = i;
				if (!(categories[i].Button == null))
				{
					categories[i].Button.onClick.RemoveAllListeners();
					categories[i].Button.onClick.AddListener(delegate
					{
						SelectCategory(index);
					});
				}
			}
			BindSlider(voiceVolumeSlider, GameSettings.SetVoiceVolume);
			BindSlider(micVolumeSlider, 0f, 2f, GameSettings.SetMicVolume);
			BindToggle(voiceEnabledToggle, GameSettings.SetVoiceEnabled);
			BindDropdown(micDeviceDropdown, OnMicDevicePicked);
			BindSlider(masterVolumeSlider, GameSettings.SetMasterVolume);
			BindSlider(sfxVolumeSlider, GameSettings.SetSfxVolume);
			BindSlider(musicVolumeSlider, GameSettings.SetMusicVolume);
			BindSlider(screenShakeSlider, GameSettings.SetScreenShake);
			BindSlider(fieldOfViewSlider, 50f, 110f, GameSettings.SetFieldOfView);
			float min = 0.1f;
			float max = 3f;
			BindSlider(fpsSensitivitySlider, min, max, GameSettings.SetFpsSensitivity);
			BindSlider(tpsSensitivitySlider, min, max, GameSettings.SetTpsSensitivity);
			BindSlider(scopeSensitivitySlider, min, max, GameSettings.SetScopeSensitivity);
			BindSlider(editorZoomSlider, min, max, GameSettings.SetEditorZoomSensitivity);
			BindWholeSlider(hoverRadiusSlider, 0, 5, GameSettings.SetEditorHoverRadius);
			BindToggle(muteWhenUnfocusedToggle, GameSettings.SetMuteWhenUnfocused);
			BindToggle(invertLookYToggle, GameSettings.SetInvertLookY);
			BindToggle(toggleCrouchToggle, GameSettings.SetToggleCrouch);
			BindToggle(toggleScopeToggle, GameSettings.SetToggleScope);
			BindToggle(toggleAdsToggle, GameSettings.SetToggleAds);
			BindToggle(developerConsoleToggle, GameSettings.SetDeveloperConsole);
			BindToggle(analyticsConsentToggle, GameSettings.SetAnalyticsConsent);
			BindToggle(safeChatToggle, GameSettings.SetSafeChat);
			GameObject gameObject = ((toggleAdsRow != null) ? toggleAdsRow : ((toggleAdsToggle != null) ? toggleAdsToggle.gameObject : null));
			if (gameObject != null && gameObject.activeSelf != Features.AimDownSights)
			{
				gameObject.SetActive(Features.AimDownSights);
			}
			BindDropdown(displayDropdown, delegate(int displayIndex)
			{
				GameSettings.SetDisplayIndex(displayIndex);
			});
			BindDropdown(windowModeDropdown, OnWindowModePicked);
			BindDropdown(aspectDropdown, OnAspectPicked);
			BindDropdown(resolutionDropdown, OnResolutionPicked);
			BindDropdown(vsyncDropdown, OnVSyncPicked);
			BindDropdown(frameRateDropdown, OnFrameRatePicked);
			BindDropdown(qualityDropdown, GameSettings.SetQualityLevel);
			BindDropdown(languageDropdown, OnLanguagePicked);
			BindColorButton(extrudeGizmoColorButton, "Colour.ExtrudeGizmo", () => GameSettings.ExtrudeGizmoColor, EditorPalette.DefaultExtrudeGizmo, GameSettings.SetExtrudeGizmoColor, extrudeGizmoColorSwatch);
			BindColorButton(transformGizmoColorButton, "Colour.TransformGizmo", () => GameSettings.TransformGizmoColor, EditorPalette.DefaultTransformGizmo, GameSettings.SetTransformGizmoColor, transformGizmoColorSwatch);
			BindColorButton(editorGridColorButton, "Colour.Grid", () => GameSettings.EditorGridColor, EditorPalette.DefaultGrid, GameSettings.SetEditorGridColor, editorGridColorSwatch);
			if (panelRoot != null)
			{
				panelRoot.SetActive(value: false);
			}
		}

		private void OnDestroy()
		{
			GameMenuState.SetMenuOpen(this, open: false);
			if (Instance == this)
			{
				Instance = null;
			}
		}

		private void Update()
		{
			bool isOpen = IsOpen;
			if (isOpen != claimed)
			{
				claimed = isOpen;
				GameMenuState.SetMenuOpen(this, isOpen);
			}
			if (isOpen)
			{
				if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
				if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
				{
					GameMenuState.RequestEscape(60, Close);
				}
			}
		}

		public void Open()
		{
			if (!(panelRoot == null) && !IsOpen)
			{
				GameSettings.Load();
				RebuildDisplayDropdown();
				RebuildWindowModeDropdown();
				RebuildAspectDropdown();
				RebuildResolutionDropdown();
				RebuildVSyncDropdown();
				RebuildFrameRateDropdown();
				RebuildQualityDropdown();
				RebuildLanguageDropdown();
				RebuildMicDeviceDropdown();
				ApplyToControls();
				SelectCategory(0);
				panelRoot.SetActive(value: true);
				GameMenuState.SetMenuOpen(this, open: true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		public void Close()
		{
			if (!(panelRoot == null) && IsOpen)
			{
				panelRoot.SetActive(value: false);
				if (colorPicker != null)
				{
					colorPicker.Close();
				}
				GameInput.CancelRebind();
				GameMenuState.SetMenuOpen(this, open: false);
			}
		}

		public void Toggle()
		{
			if (IsOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		private void SelectCategory(int index)
		{
			for (int i = 0; i < categories.Length; i++)
			{
				bool flag = i == index;
				if (categories[i].Panel != null)
				{
					categories[i].Panel.SetActive(flag);
				}
				if (categories[i].Button != null && categories[i].Button.targetGraphic != null)
				{
					categories[i].Button.targetGraphic.color = (flag ? selectedTabColor : unselectedTabColor);
				}
			}
		}

		private void RebuildDisplayDropdown()
		{
			if (!(displayDropdown == null))
			{
				IReadOnlyList<DisplayInfo> readOnlyList = DisplayCatalog.Displays();
				List<string> list = new List<string>(readOnlyList.Count);
				for (int i = 0; i < readOnlyList.Count; i++)
				{
					list.Add(DisplayCatalog.DisplayLabel(i, readOnlyList[i]));
				}
				SetOptions(displayDropdown, list, Mathf.Clamp(GameSettings.DisplayIndex, 0, Mathf.Max(0, list.Count - 1)));
			}
		}

		private void RebuildWindowModeDropdown()
		{
			if (windowModeDropdown == null)
			{
				return;
			}
			listedWindowModes.Clear();
			listedWindowModes.AddRange(DisplayCatalog.WindowModes());
			List<string> list = new List<string>(listedWindowModes.Count);
			foreach (FullScreenMode listedWindowMode in listedWindowModes)
			{
				list.Add(DisplayCatalog.WindowModeLabel(listedWindowMode));
			}
			int num = listedWindowModes.IndexOf(GameSettings.WindowMode);
			SetOptions(windowModeDropdown, list, (num >= 0) ? num : 0);
		}

		private void OnWindowModePicked(int index)
		{
			if (index >= 0 && index < listedWindowModes.Count)
			{
				GameSettings.SetWindowMode(listedWindowModes[index]);
			}
		}

		private void RebuildAspectDropdown()
		{
			if (aspectDropdown == null)
			{
				return;
			}
			listedAspectIds.Clear();
			listedAspectIds.AddRange(DisplayCatalog.AspectIds());
			List<string> list = new List<string>(listedAspectIds.Count);
			foreach (string listedAspectId in listedAspectIds)
			{
				list.Add(DisplayCatalog.AspectLabel(listedAspectId));
			}
			int num = listedAspectIds.IndexOf(GameSettings.AspectRatioId);
			SetOptions(aspectDropdown, list, (num >= 0) ? num : 0);
		}

		private void RebuildResolutionDropdown()
		{
			if (resolutionDropdown == null)
			{
				return;
			}
			string aspectId = SelectedAspectId();
			listedResolutions.Clear();
			listedResolutions.AddRange(DisplayCatalog.ResolutionsFor(aspectId));
			List<string> list = new List<string>(listedResolutions.Count);
			foreach (Resolution listedResolution in listedResolutions)
			{
				list.Add(DisplayCatalog.ResolutionLabel(listedResolution));
			}
			SetOptions(resolutionDropdown, list, IndexOfCurrentResolution());
		}

		private int IndexOfCurrentResolution()
		{
			int num = IndexOfSize(GameSettings.ResolutionWidth, GameSettings.ResolutionHeight);
			if (num >= 0)
			{
				return num;
			}
			num = IndexOfSize(Screen.width, Screen.height);
			if (num < 0)
			{
				return 0;
			}
			return num;
		}

		private int IndexOfSize(int width, int height)
		{
			for (int i = 0; i < listedResolutions.Count; i++)
			{
				if (listedResolutions[i].width == width && listedResolutions[i].height == height)
				{
					return i;
				}
			}
			return -1;
		}

		private string SelectedAspectId()
		{
			if (aspectDropdown == null || listedAspectIds.Count == 0)
			{
				return "";
			}
			int index = Mathf.Clamp(aspectDropdown.value, 0, listedAspectIds.Count - 1);
			return listedAspectIds[index];
		}

		private void OnAspectPicked(int index)
		{
			GameSettings.SetAspectRatioId(SelectedAspectId());
			RebuildResolutionDropdown();
		}

		private void OnResolutionPicked(int index)
		{
			if (index >= 0 && index < listedResolutions.Count)
			{
				GameSettings.SetResolution(listedResolutions[index].width, listedResolutions[index].height);
			}
		}

		private void RebuildVSyncDropdown()
		{
			if (!(vsyncDropdown == null))
			{
				SetOptions(vsyncDropdown, new List<string>
				{
					Loc.Get("Video.VSync.Off"),
					Loc.Get("Video.VSync.On"),
					Loc.Get("Video.VSync.Half")
				}, IndexOf(VSyncOptions, GameSettings.VSyncCount, 1));
			}
		}

		private void OnVSyncPicked(int index)
		{
			if (index >= 0 && index < VSyncOptions.Length)
			{
				GameSettings.SetVSyncCount(VSyncOptions[index]);
				RefreshValueLabels();
			}
		}

		private void RebuildFrameRateDropdown()
		{
			if (!(frameRateDropdown == null))
			{
				List<string> list = new List<string>(FrameRateOptions.Length);
				int[] frameRateOptions = FrameRateOptions;
				for (int i = 0; i < frameRateOptions.Length; i++)
				{
					int num = frameRateOptions[i];
					list.Add((num <= 0) ? Loc.Get("Video.FrameRate.Unlimited") : num.ToString());
				}
				SetOptions(frameRateDropdown, list, IndexOf(FrameRateOptions, GameSettings.FrameRateLimit, 0));
			}
		}

		private void OnFrameRatePicked(int index)
		{
			if (index >= 0 && index < FrameRateOptions.Length)
			{
				GameSettings.SetFrameRateLimit(FrameRateOptions[index]);
			}
		}

		private void RebuildQualityDropdown()
		{
			if (!(qualityDropdown == null))
			{
				List<string> list = new List<string>();
				string[] names = QualitySettings.names;
				foreach (string projectName in names)
				{
					list.Add(QualityLabel(projectName));
				}
				int qualityLevel = GameSettings.QualityLevel;
				if (qualityLevel < 0 || qualityLevel >= list.Count)
				{
					qualityLevel = QualitySettings.GetQualityLevel();
				}
				SetOptions(qualityDropdown, list, Mathf.Clamp(qualityLevel, 0, Mathf.Max(0, list.Count - 1)));
			}
		}

		private static string QualityLabel(string projectName)
		{
			if (string.IsNullOrWhiteSpace(projectName))
			{
				return projectName;
			}
			StringBuilder stringBuilder = new StringBuilder("Video.Quality.");
			foreach (char c in projectName)
			{
				if (char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(c);
				}
			}
			if (!Loc.TryGet(stringBuilder.ToString(), out var text))
			{
				return projectName;
			}
			return text;
		}

		private void RebuildMicDeviceDropdown()
		{
			if (micDeviceDropdown == null)
			{
				return;
			}
			listedMicDevices.Clear();
			string[] devices = Microphone.devices;
			if (devices != null)
			{
				listedMicDevices.AddRange(devices);
			}
			if (listedMicDevices.Count == 0)
			{
				SetOptions(micDeviceDropdown, new List<string> { Loc.Get("Audio.NoMicrophone") }, 0);
				micDeviceDropdown.interactable = false;
				return;
			}
			micDeviceDropdown.interactable = true;
			int num = listedMicDevices.IndexOf(GameSettings.MicDevice);
			SetOptions(micDeviceDropdown, new List<string>(listedMicDevices), (num >= 0) ? num : 0);
			if (num < 0 && !string.IsNullOrEmpty(GameSettings.MicDevice))
			{
				GameSettings.SetMicDevice(listedMicDevices[0]);
			}
		}

		private void OnMicDevicePicked(int index)
		{
			if (index >= 0 && index < listedMicDevices.Count)
			{
				GameSettings.SetMicDevice(listedMicDevices[index]);
			}
		}

		private void RebuildLanguageDropdown()
		{
			if (languageDropdown == null)
			{
				return;
			}
			listedLanguages.Clear();
			listedLanguages.AddRange(Loc.Languages);
			if (listedLanguages.Count == 0)
			{
				listedLanguages.Add("tr");
			}
			List<string> list = new List<string>(listedLanguages.Count);
			foreach (string listedLanguage in listedLanguages)
			{
				list.Add(Loc.LanguageName(listedLanguage));
			}
			int num = listedLanguages.IndexOf(GameSettings.LanguageId);
			SetOptions(languageDropdown, list, (num >= 0) ? num : 0);
		}

		private void OnLanguagePicked(int index)
		{
			if (index >= 0 && index < listedLanguages.Count)
			{
				GameSettings.SetLanguageId(listedLanguages[index]);
			}
		}

		private static int IndexOf(int[] options, int value, int fallback)
		{
			for (int i = 0; i < options.Length; i++)
			{
				if (options[i] == value)
				{
					return i;
				}
			}
			return fallback;
		}

		private void BindColorButton(Button button, string titleKey, Func<Color> read, Color fallback, Action<Color> write, Image swatch)
		{
			if (swatch != null)
			{
				swatch.color = read();
			}
			if (button == null)
			{
				return;
			}
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(delegate
			{
				if (!(colorPicker == null))
				{
					colorPicker.Open(titleKey, read(), fallback, delegate(Color colour)
					{
						write(colour);
						if (swatch != null)
						{
							swatch.color = colour;
						}
					});
				}
			});
		}

		private void ApplyToControls()
		{
			applyingToControls = true;
			SetSlider(masterVolumeSlider, GameSettings.MasterVolume);
			SetSlider(sfxVolumeSlider, GameSettings.SfxVolume);
			SetSlider(voiceVolumeSlider, GameSettings.VoiceVolume);
			SetSlider(micVolumeSlider, GameSettings.MicVolume);
			SetSlider(musicVolumeSlider, GameSettings.MusicVolume);
			SetSlider(screenShakeSlider, GameSettings.ScreenShake);
			SetSlider(fieldOfViewSlider, (GameSettings.FieldOfView > 0f) ? GameSettings.FieldOfView : 70f);
			SetSlider(fpsSensitivitySlider, GameSettings.FpsSensitivity);
			SetSlider(tpsSensitivitySlider, GameSettings.TpsSensitivity);
			SetSlider(scopeSensitivitySlider, GameSettings.ScopeSensitivity);
			SetSlider(editorZoomSlider, GameSettings.EditorZoomSensitivity);
			SetSlider(hoverRadiusSlider, GameSettings.EditorHoverRadius);
			SetToggle(muteWhenUnfocusedToggle, GameSettings.MuteWhenUnfocused);
			SetToggle(voiceEnabledToggle, GameSettings.VoiceEnabled);
			SetToggle(invertLookYToggle, GameSettings.InvertLookY);
			SetToggle(toggleCrouchToggle, GameSettings.ToggleCrouch);
			SetToggle(toggleScopeToggle, GameSettings.ToggleScope);
			SetToggle(toggleAdsToggle, GameSettings.ToggleAds);
			SetToggle(developerConsoleToggle, GameSettings.DeveloperConsole);
			SetToggle(analyticsConsentToggle, GameSettings.AnalyticsConsent);
			SetToggle(safeChatToggle, GameSettings.SafeChat);
			SetSwatch(extrudeGizmoColorSwatch, GameSettings.ExtrudeGizmoColor);
			SetSwatch(transformGizmoColorSwatch, GameSettings.TransformGizmoColor);
			SetSwatch(editorGridColorSwatch, GameSettings.EditorGridColor);
			applyingToControls = false;
			RefreshValueLabels();
		}

		private void RefreshValueLabels()
		{
			SetPercentLabel(masterVolumeValueLabel, GameSettings.MasterVolume);
			SetPercentLabel(sfxVolumeValueLabel, GameSettings.SfxVolume);
			SetPercentLabel(musicVolumeValueLabel, GameSettings.MusicVolume);
			SetPercentLabel(voiceVolumeValueLabel, GameSettings.VoiceVolume);
			SetPercentLabel(micVolumeValueLabel, GameSettings.MicVolume);
			SetPercentLabel(screenShakeValueLabel, GameSettings.ScreenShake);
			SetPercentLabel(fpsSensitivityValueLabel, GameSettings.FpsSensitivity);
			SetPercentLabel(tpsSensitivityValueLabel, GameSettings.TpsSensitivity);
			SetPercentLabel(scopeSensitivityValueLabel, GameSettings.ScopeSensitivity);
			SetPercentLabel(editorZoomValueLabel, GameSettings.EditorZoomSensitivity);
			if (hoverRadiusValueLabel != null)
			{
				hoverRadiusValueLabel.text = GameSettings.EditorHoverRadius.ToString();
			}
			if (fieldOfViewValueLabel != null)
			{
				float f = ((GameSettings.FieldOfView > 0f) ? GameSettings.FieldOfView : 70f);
				fieldOfViewValueLabel.text = $"{Mathf.RoundToInt(f)}°";
			}
		}

		private static void SetSwatch(Image swatch, Color colour)
		{
			if (swatch != null)
			{
				swatch.color = colour;
			}
		}

		private static void SetToggle(Toggle toggle, bool value)
		{
			if (toggle != null)
			{
				toggle.SetIsOnWithoutNotify(value);
			}
		}

		private static void SetPercentLabel(TextMeshProUGUI label, float value01)
		{
			if (label != null)
			{
				label.text = $"{Mathf.RoundToInt(value01 * 100f)}%";
			}
		}

		private void BindToggle(Toggle toggle, Action<bool> write)
		{
			if (toggle == null)
			{
				return;
			}
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener(delegate(bool value)
			{
				if (!applyingToControls)
				{
					write(value);
				}
			});
		}

		private void BindSlider(Slider slider, Action<float> write)
		{
			BindSlider(slider, 0f, 1f, write);
		}

		private void BindWholeSlider(Slider slider, int min, int max, Action<int> write)
		{
			if (slider == null)
			{
				return;
			}
			slider.wholeNumbers = true;
			slider.minValue = min;
			slider.maxValue = max;
			slider.onValueChanged.RemoveAllListeners();
			slider.onValueChanged.AddListener(delegate(float value)
			{
				if (!applyingToControls)
				{
					write(Mathf.RoundToInt(value));
					RefreshValueLabels();
				}
			});
		}

		private void BindSlider(Slider slider, float min, float max, Action<float> write)
		{
			if (slider == null)
			{
				return;
			}
			slider.minValue = min;
			slider.maxValue = max;
			slider.onValueChanged.RemoveAllListeners();
			slider.onValueChanged.AddListener(delegate(float value)
			{
				if (!applyingToControls)
				{
					write(value);
					RefreshValueLabels();
				}
			});
		}

		private void BindDropdown(TMP_Dropdown dropdown, Action<int> write)
		{
			if (dropdown == null)
			{
				return;
			}
			dropdown.onValueChanged.RemoveAllListeners();
			dropdown.onValueChanged.AddListener(delegate(int index)
			{
				if (!applyingToControls)
				{
					write(index);
				}
			});
		}

		private static void SetSlider(Slider slider, float value)
		{
			if (slider != null)
			{
				slider.SetValueWithoutNotify(value);
			}
		}

		private void SetOptions(TMP_Dropdown dropdown, List<string> labels, int value)
		{
			applyingToControls = true;
			dropdown.ClearOptions();
			dropdown.AddOptions(labels);
			dropdown.SetValueWithoutNotify(Mathf.Clamp(value, 0, Mathf.Max(0, labels.Count - 1)));
			dropdown.RefreshShownValue();
			applyingToControls = false;
		}
	}
}
