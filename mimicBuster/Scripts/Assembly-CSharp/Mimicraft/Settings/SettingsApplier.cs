using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;

namespace Mimicraft.Settings
{
	public class SettingsApplier : MonoBehaviour
	{
		public const string ResourcePath = "SettingsRoot";

		[Header("Ses")]
		[Tooltip("Ana/Efekt/Müzik seslerini taşıyan AudioMixer. Boş bırakılırsa yalnızca ana ses çalışır (AudioListener üzerinden) ve efekt/müzik ayrı ayrı ayarlanamaz - bunun için mixer şart, çünkü sesleri gruplara ayıran tek şey o.")]
		[SerializeField]
		private AudioMixer audioMixer;

		[Tooltip("Mixer'da Expose edilmiş parametre adları. Mixer'daki grubun Volume alanına sağ tıklayıp 'Expose ... to script' dedikten sonra Audio Mixer penceresinin sağ üstündeki Exposed Parameters listesinden bu adlara yeniden adlandır.")]
		[SerializeField]
		private string masterVolumeParameter = "MasterVolume";

		[SerializeField]
		private string sfxVolumeParameter = "SfxVolume";

		[SerializeField]
		private string musicVolumeParameter = "MusicVolume";

		private Coroutine windowJob;

		private int appliedDisplayIndex = int.MinValue;

		private int appliedWidth = int.MinValue;

		private int appliedHeight = int.MinValue;

		private FullScreenMode appliedMode = (FullScreenMode)(-1);

		private const float WindowSettleSeconds = 0.4f;

		private int watchedWidth;

		private int watchedHeight;

		private FullScreenMode watchedMode;

		private int watchedDisplay = int.MinValue;

		private float windowSettlesAt;

		private bool windowMoving;

		private const float DisplayPollSeconds = 0.5f;

		private float nextDisplayPoll;

		private bool focused = true;

		private const float AudioSettleSeconds = 30f;

		private float settleUntil;

		private float settleStarted;

		private int settleRewrites;

		private static AudioMixerGroup sfxGroup;

		private static AudioMixerGroup musicGroup;

		private bool warnedMissingMixer;

		private static readonly HashSet<string> warned = new HashSet<string>();

		public static SettingsApplier Instance { get; private set; }

		private float EffectiveMaster
		{
			get
			{
				if (!GameSettings.MuteWhenUnfocused || focused)
				{
					return GameSettings.MasterVolume;
				}
				return 0f;
			}
		}

		public static AudioMixerGroup SfxGroup => FindGroup(ref sfxGroup, "SFX");

		public static AudioMixerGroup MusicGroup => FindGroup(ref musicGroup, "Music");

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void EnsureExists()
		{
			if (!(Instance != null))
			{
				SettingsApplier settingsApplier = Resources.Load<SettingsApplier>("SettingsRoot");
				if (settingsApplier == null)
				{
					Debug.LogWarning("[Settings] Resources/SettingsRoot bulunamadi - ayarlar ekrani yuklenmedi. Prefab'i bir Resources klasorune bu isimle koy.");
				}
				else
				{
					Object.Instantiate(settingsApplier).name = settingsApplier.name;
				}
			}
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			GameSettings.Load();
			GameSettings.Changed += OnSettingsChanged;
			ApplyAll();
			SeedWindowWatch();
		}

		private void Update()
		{
			WatchWindow();
			SettleAudio();
		}

		private void OnDestroy()
		{
			GameSettings.Changed -= OnSettingsChanged;
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void ApplyAll()
		{
			ApplyAudio();
			ApplyPerformance();
			RequestWindow();
		}

		private void RequestWindow()
		{
			if (base.isActiveAndEnabled)
			{
				if (windowJob != null)
				{
					StopCoroutine(windowJob);
				}
				windowJob = StartCoroutine(ApplyWindowJob());
			}
		}

		private IEnumerator ApplyWindowJob()
		{
			yield return null;
			ApplyDisplay();
			yield return null;
			yield return null;
			ApplyWindow();
			yield return null;
			SeedWindowWatch();
			windowJob = null;
		}

		private void OnSettingsChanged()
		{
			ApplyAudio();
			ApplyPerformance();
			if (GameSettings.DisplayIndex != appliedDisplayIndex || GameSettings.ResolutionWidth != appliedWidth || GameSettings.ResolutionHeight != appliedHeight || GameSettings.WindowMode != appliedMode)
			{
				RequestWindow();
			}
		}

		private void WatchWindow()
		{
			if (!focused || !IsWindowSane())
			{
				windowMoving = false;
				return;
			}
			bool flag = Screen.width != watchedWidth || Screen.height != watchedHeight || Screen.fullScreenMode != watchedMode;
			if (!flag && Time.unscaledTime >= nextDisplayPoll)
			{
				nextDisplayPoll = Time.unscaledTime + 0.5f;
				flag = CurrentDisplayIndex() != watchedDisplay;
			}
			if (flag)
			{
				watchedWidth = Screen.width;
				watchedHeight = Screen.height;
				watchedMode = Screen.fullScreenMode;
				watchedDisplay = CurrentDisplayIndex();
				windowMoving = true;
				windowSettlesAt = Time.unscaledTime + 0.4f;
			}
			else if (windowMoving && !(Time.unscaledTime < windowSettlesAt))
			{
				windowMoving = false;
				RememberWindow();
			}
		}

		private static bool IsWindowSane()
		{
			if (Screen.width >= 320)
			{
				return Screen.height >= 240;
			}
			return false;
		}

		private void RememberWindow()
		{
			bool flag = watchedWidth != GameSettings.ResolutionWidth || watchedHeight != GameSettings.ResolutionHeight || watchedMode != GameSettings.WindowMode;
			bool flag2 = watchedDisplay >= 0 && watchedDisplay != GameSettings.DisplayIndex;
			if (flag || flag2)
			{
				appliedWidth = watchedWidth;
				appliedHeight = watchedHeight;
				appliedMode = watchedMode;
				if (flag2)
				{
					appliedDisplayIndex = watchedDisplay;
				}
				if (flag)
				{
					GameSettings.SetResolution(watchedWidth, watchedHeight);
					GameSettings.SetWindowMode(watchedMode);
				}
				if (flag2)
				{
					GameSettings.SetDisplayIndex(watchedDisplay);
				}
			}
		}

		private static int CurrentDisplayIndex()
		{
			DisplayInfo mainWindowDisplayInfo = Screen.mainWindowDisplayInfo;
			IReadOnlyList<DisplayInfo> readOnlyList = DisplayCatalog.Displays();
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				if (readOnlyList[i].Equals(mainWindowDisplayInfo))
				{
					return i;
				}
			}
			return -1;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			bool flag = hasFocus && !focused;
			focused = hasFocus;
			ApplyAudio();
			if (hasFocus && flag)
			{
				windowMoving = false;
				SeedWindowWatch();
			}
		}

		private void SeedWindowWatch()
		{
			watchedWidth = Screen.width;
			watchedHeight = Screen.height;
			watchedMode = Screen.fullScreenMode;
			watchedDisplay = CurrentDisplayIndex();
		}

		private void ApplyAudio()
		{
			AudioListener.pause = false;
			if (audioMixer == null)
			{
				AudioListener.volume = EffectiveMaster;
				WarnMissingMixerOnce();
				return;
			}
			AudioListener.volume = 1f;
			SetMixerVolume(masterVolumeParameter, EffectiveMaster);
			SetMixerVolume(sfxVolumeParameter, GameSettings.SfxVolume);
			SetMixerVolume(musicVolumeParameter, GameSettings.MusicVolume);
			settleUntil = Time.unscaledTime + 30f;
			settleStarted = Time.unscaledTime;
			settleRewrites = 0;
		}

		private void SettleAudio()
		{
			if (settleUntil <= 0f)
			{
				return;
			}
			if (audioMixer == null)
			{
				settleUntil = 0f;
			}
			else if (Confirmed(masterVolumeParameter, EffectiveMaster) && Confirmed(sfxVolumeParameter, GameSettings.SfxVolume) && Confirmed(musicVolumeParameter, GameSettings.MusicVolume))
			{
				settleUntil = 0f;
				float num = Time.unscaledTime - settleStarted;
				if (settleRewrites > 0)
				{
					Debug.Log($"[Settings] Ses seviyeleri {num:0.00} sn ve {settleRewrites} yazma " + "denemesinden sonra tuttu.\n" + AudioReport(probe: false));
				}
				else if (Debug.isDebugBuild)
				{
					Debug.Log($"[Settings] Ses seviyeleri ilk denemede tuttu ({num:0.00} sn): " + $"master={GameSettings.MasterVolume:0.000} " + $"sfx={GameSettings.SfxVolume:0.000} muzik={GameSettings.MusicVolume:0.000}");
				}
			}
			else if (Time.unscaledTime >= settleUntil)
			{
				settleUntil = 0f;
				WarnOnce("[Settings] AudioMixer yazilan ses seviyelerini kabul etmiyor - " + $"{30f:0} saniye boyunca denendi. Mixer'daki Expose edilmis " + "parametreler gercekten ilgili grubun Volume alanina mi bagli? (Konsolda `audio` komutu her ikisini de yazdirir.)");
			}
			else
			{
				settleRewrites++;
				SetMixerVolume(masterVolumeParameter, EffectiveMaster);
				SetMixerVolume(sfxVolumeParameter, GameSettings.SfxVolume);
				SetMixerVolume(musicVolumeParameter, GameSettings.MusicVolume);
			}
		}

		private bool Confirmed(string parameter, float linear)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				return true;
			}
			if (!audioMixer.GetFloat(parameter, out var value))
			{
				return false;
			}
			return Mathf.Abs(value - Decibels(linear)) < 0.01f;
		}

		private static float Decibels(float linear)
		{
			if (!(linear <= 0.0001f))
			{
				return Mathf.Log10(linear) * 20f;
			}
			return -80f;
		}

		private static AudioMixerGroup FindGroup(ref AudioMixerGroup cache, string groupName)
		{
			if (cache != null)
			{
				return cache;
			}
			AudioMixer audioMixer = ((Instance != null) ? Instance.audioMixer : null);
			if (audioMixer == null)
			{
				return null;
			}
			AudioMixerGroup[] array = audioMixer.FindMatchingGroups(groupName);
			cache = ((array != null && array.Length != 0) ? array[0] : null);
			if (cache == null)
			{
				WarnOnce("[Settings] AudioMixer'da '" + groupName + "' adinda bir grup yok - o bus'a yonlendirilecek sesler mixer'i atlayacak.");
			}
			return cache;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetGroups()
		{
			sfxGroup = null;
			musicGroup = null;
		}

		public string AudioReport()
		{
			return AudioReport(probe: true);
		}

		public string AudioReport(bool probe)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"ayar: master={GameSettings.MasterVolume:0.000} " + $"sfx={GameSettings.SfxVolume:0.000} muzik={GameSettings.MusicVolume:0.000}");
			stringBuilder.AppendLine($"listener: volume={AudioListener.volume:0.000} pause={AudioListener.pause} " + $"odak={focused}");
			if (audioMixer == null)
			{
				stringBuilder.AppendLine("mixer: YOK - master AudioListener uzerinden, sfx/muzik hic uygulanmiyor.");
			}
			else
			{
				stringBuilder.AppendLine($"mixer: '{audioMixer.name}' settleUntil={settleUntil:0.00} " + $"simdi={Time.unscaledTime:0.00}");
				stringBuilder.AppendLine("  " + MixerLine(masterVolumeParameter, GameSettings.MasterVolume, probe));
				stringBuilder.AppendLine("  " + MixerLine(sfxVolumeParameter, GameSettings.SfxVolume, probe));
				stringBuilder.AppendLine("  " + MixerLine(musicVolumeParameter, GameSettings.MusicVolume, probe));
			}
			AppendSources(stringBuilder);
			return stringBuilder.ToString().TrimEnd();
		}

		private string MixerLine(string parameter, float linear, bool probe)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				return "(parametre adi bos)";
			}
			float num = Decibels(linear);
			float value;
			string text = (audioMixer.GetFloat(parameter, out value) ? $"{value:0.0} dB" : "OKUNAMADI");
			if (!probe)
			{
				bool flag = Mathf.Abs(value - num) < 0.01f;
				return string.Format("{0}: {1}, olmasi gereken {2:0.0} dB{3}", parameter, text, num, flag ? "" : "  <-- UYUSMUYOR");
			}
			bool flag2 = audioMixer.SetFloat(parameter, num);
			float value2;
			string text2 = (audioMixer.GetFloat(parameter, out value2) ? $"{value2:0.0} dB" : "OKUNAMADI");
			bool flag3 = flag2 && Mathf.Abs(value2 - num) < 0.01f;
			return $"{parameter}: onceki={text} olmasi gereken={num:0.0} dB | " + "SetFloat=" + (flag2 ? "kabul" : "RET") + " sonraki=" + text2 + (flag3 ? "" : "  <-- YAZILAMIYOR");
		}

		private static void AppendSources(StringBuilder text)
		{
			AudioSource[] array = Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			int num = 0;
			AudioSource[] array2 = array;
			foreach (AudioSource audioSource in array2)
			{
				if (!(audioSource == null) && audioSource.isPlaying)
				{
					num++;
					string key = ((audioSource.outputAudioMixerGroup != null) ? audioSource.outputAudioMixerGroup.name : "GRUP YOK (mixer'i atlıyor)");
					dictionary.TryGetValue(key, out var value);
					dictionary[key] = value + 1;
				}
			}
			text.AppendLine($"kaynaklar: {array.Length} tane, {num} tanesi caliyor");
			foreach (KeyValuePair<string, int> item in dictionary)
			{
				text.AppendLine($"  {item.Value} x {item.Key}");
			}
		}

		private void SetMixerVolume(string parameter, float linear)
		{
			if (!string.IsNullOrEmpty(parameter))
			{
				float value = Decibels(linear);
				if (!audioMixer.SetFloat(parameter, value))
				{
					WarnOnce("[Settings] AudioMixer'da '" + parameter + "' parametresi yok - Expose edilmis mi?");
				}
			}
		}

		private void WarnMissingMixerOnce()
		{
			if (!warnedMissingMixer)
			{
				warnedMissingMixer = true;
				Debug.LogWarning("[Settings] AudioMixer atanmamis - efekt ve muzik sesleri ayri ayri ayarlanamaz, yalnizca ana ses calisiyor.", this);
			}
		}

		private static void WarnOnce(string message)
		{
			if (warned.Add(message))
			{
				Debug.LogWarning(message);
			}
		}

		private void ApplyPerformance()
		{
			QualitySettings.vSyncCount = Mathf.Clamp(GameSettings.VSyncCount, 0, 2);
			Application.targetFrameRate = ((QualitySettings.vSyncCount > 0 || GameSettings.FrameRateLimit <= 0) ? (-1) : GameSettings.FrameRateLimit);
			int qualityLevel = GameSettings.QualityLevel;
			if (qualityLevel >= 0 && qualityLevel < QualitySettings.names.Length && QualitySettings.GetQualityLevel() != qualityLevel)
			{
				QualitySettings.SetQualityLevel(qualityLevel, applyExpensiveChanges: false);
			}
		}

		private void ApplyDisplay()
		{
			if (Application.isEditor || !focused || !IsWindowSane())
			{
				return;
			}
			IReadOnlyList<DisplayInfo> readOnlyList = DisplayCatalog.Displays();
			if (readOnlyList.Count != 0)
			{
				appliedDisplayIndex = GameSettings.DisplayIndex;
				int index = Mathf.Clamp(GameSettings.DisplayIndex, 0, readOnlyList.Count - 1);
				DisplayInfo display = readOnlyList[index];
				if (!Screen.mainWindowDisplayInfo.Equals(display))
				{
					Vector2Int position = new Vector2Int((display.width - Screen.width) / 2, (display.height - Screen.height) / 2);
					Screen.MoveMainWindowTo(in display, position);
				}
			}
		}

		private void ApplyWindow()
		{
			if (focused && IsWindowSane())
			{
				appliedWidth = GameSettings.ResolutionWidth;
				appliedHeight = GameSettings.ResolutionHeight;
				appliedMode = GameSettings.WindowMode;
				FullScreenMode fullScreenMode = GameSettings.WindowMode;
				if (fullScreenMode == FullScreenMode.ExclusiveFullScreen)
				{
					fullScreenMode = FullScreenMode.FullScreenWindow;
				}
				int num = ((GameSettings.ResolutionWidth > 0) ? GameSettings.ResolutionWidth : Screen.width);
				int num2 = ((GameSettings.ResolutionHeight > 0) ? GameSettings.ResolutionHeight : Screen.height);
				if (Screen.width != num || Screen.height != num2 || Screen.fullScreenMode != fullScreenMode)
				{
					Screen.SetResolution(num, num2, fullScreenMode);
				}
			}
		}
	}
}
