using System.Collections.Generic;
using UnityEngine;

public class GeneralSettingsManager : MonoBehaviour
{
	private static GeneralSettingsManager _instance;

	private static bool _quitting;

	[Header("Mouse")]
	[Tooltip("Sensitivity slider aralığı — 1 = NetworkedCameraController.mouseSensitivity'nin kendi (Inspector) değeri, değişmeden")]
	public float minSensitivity = 0.1f;

	public float maxSensitivity = 3f;

	[Header("Varsayılan Çözünürlük")]
	[Tooltip("Kayıtlı tercih yoksa ve mevcut ekran çözünürlüğü listede bulunamazsa hedeflenecek varsayılan çözünürlük.")]
	public int defaultResolutionWidth = 1920;

	public int defaultResolutionHeight = 1080;

	[Header("Varsayılanlar (Reset butonu)")]
	[Range(0.1f, 3f)]
	[SerializeField]
	private float defaultMouseSensitivity = 1f;

	[SerializeField]
	private bool defaultInvertY;

	[Tooltip("VSync varsayılan AÇIK — ekran yırtılmasını (screen tearing) önler, çoğu oyuncu için beklenen davranış.")]
	[SerializeField]
	private bool defaultVSync = true;

	[Tooltip("Fullscreen varsayılan AÇIK — PC oyunlarında standart beklenti.")]
	[SerializeField]
	private bool defaultFullscreen = true;

	[Tooltip("Reset butonu ve ilk kurulumda (kayıtlı tercih yoksa) hedeflenecek grafik kalite seviyesi index'i (QualitySettings.names sırasına göre — 0 = High). QualitySettings.GetQualityLevel() KASITLI kullanılmıyor: o anlık/çalışma-zamanı değeri döndürüyor, yani kullanıcı Low'a geçip Reset'e basarsa 'varsayılan' olarak Low'u geri verirdi — Reset her zaman AYNI (High) seviyeye dönmeli.")]
	[SerializeField]
	private int defaultQualityLevel;

	private const string SensitivityPrefKey = "Settings.MouseSensitivity";

	private const string InvertYPrefKey = "Settings.InvertY";

	private const string QualityPrefKey = "Settings.QualityLevel";

	private const string VSyncPrefKey = "Settings.VSync";

	private const string FullscreenPrefKey = "Settings.Fullscreen";

	private const string ResolutionIndexPrefKey = "Settings.ResolutionIndex";

	private bool _initialized;

	public static GeneralSettingsManager Instance
	{
		get
		{
			if (_instance == null && !_quitting)
			{
				_instance = new GameObject("GeneralSettingsManager (auto)").AddComponent<GeneralSettingsManager>();
			}
			return _instance;
		}
	}

	public float MouseSensitivity { get; private set; }

	public bool InvertY { get; private set; }

	public int QualityLevel { get; private set; }

	public bool VSync { get; private set; }

	public bool Fullscreen { get; private set; }

	public int ResolutionIndex { get; private set; }

	public Resolution[] AvailableResolutions { get; private set; }

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		_instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		Initialize();
	}

	private void OnApplicationQuit()
	{
		_quitting = true;
	}

	private void Initialize()
	{
		if (!_initialized)
		{
			_initialized = true;
			AvailableResolutions = BuildDistinctResolutions();
			LoadSaved();
			ApplyQuality(QualityLevel);
			ApplyVSync(VSync);
			ApplyFullscreenAndResolution(Fullscreen, ResolutionIndex);
		}
	}

	private Resolution[] BuildDistinctResolutions()
	{
		Resolution[] resolutions = Screen.resolutions;
		List<Resolution> list = new List<Resolution>();
		Resolution[] array = resolutions;
		for (int i = 0; i < array.Length; i++)
		{
			Resolution resolution = array[i];
			bool flag = false;
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].width == resolution.width && list[j].height == resolution.height)
				{
					if (resolution.refreshRateRatio.value > list[j].refreshRateRatio.value)
					{
						list[j] = resolution;
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(resolution);
			}
		}
		if (list.Count <= 1)
		{
			list.Clear();
			AddResolution(list, 1280, 720);
			AddResolution(list, 1366, 768);
			AddResolution(list, 1600, 900);
			AddResolution(list, 1920, 1080);
			AddResolution(list, 2560, 1440);
			AddResolution(list, 3840, 2160);
		}
		bool flag2 = false;
		foreach (Resolution item in list)
		{
			if (item.width == defaultResolutionWidth && item.height == defaultResolutionHeight)
			{
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			AddResolution(list, defaultResolutionWidth, defaultResolutionHeight);
		}
		list.Sort((Resolution a, Resolution b) => (a.width * a.height).CompareTo(b.width * b.height));
		return list.ToArray();
	}

	private static void AddResolution(List<Resolution> list, int width, int height)
	{
		list.Add(new Resolution
		{
			width = width,
			height = height,
			refreshRateRatio = new RefreshRate
			{
				numerator = 60u,
				denominator = 1u
			}
		});
	}

	private void LoadSaved()
	{
		MouseSensitivity = PlayerPrefs.GetFloat("Settings.MouseSensitivity", defaultMouseSensitivity);
		InvertY = PlayerPrefs.GetInt("Settings.InvertY", defaultInvertY ? 1 : 0) == 1;
		QualityLevel = PlayerPrefs.GetInt("Settings.QualityLevel", defaultQualityLevel);
		VSync = PlayerPrefs.GetInt("Settings.VSync", defaultVSync ? 1 : 0) == 1;
		Fullscreen = PlayerPrefs.GetInt("Settings.Fullscreen", defaultFullscreen ? 1 : 0) == 1;
		ResolutionIndex = PlayerPrefs.GetInt("Settings.ResolutionIndex", FindDefaultResolutionIndex());
	}

	private int FindDefaultResolutionIndex()
	{
		int num = FindResolutionIndex(defaultResolutionWidth, defaultResolutionHeight);
		if (num >= 0)
		{
			return num;
		}
		int num2 = FindResolutionIndex(Screen.currentResolution.width, Screen.currentResolution.height);
		if (num2 >= 0)
		{
			return num2;
		}
		return Mathf.Max(0, AvailableResolutions.Length - 1);
	}

	private int FindResolutionIndex(int width, int height)
	{
		for (int i = 0; i < AvailableResolutions.Length; i++)
		{
			if (AvailableResolutions[i].width == width && AvailableResolutions[i].height == height)
			{
				return i;
			}
		}
		return -1;
	}

	public void PreviewMouseSensitivity(float value)
	{
		MouseSensitivity = Mathf.Clamp(value, minSensitivity, maxSensitivity);
	}

	public void PreviewInvertY(bool value)
	{
		InvertY = value;
	}

	public void PreviewQualityLevel(int level)
	{
		QualityLevel = level;
		ApplyQuality(level);
	}

	public void PreviewVSync(bool enabled)
	{
		VSync = enabled;
		ApplyVSync(enabled);
	}

	public void PreviewFullscreen(bool fullscreen)
	{
		Fullscreen = fullscreen;
		ApplyFullscreenAndResolution(Fullscreen, ResolutionIndex);
	}

	public void PreviewResolution(int index)
	{
		if (AvailableResolutions != null && AvailableResolutions.Length != 0)
		{
			ResolutionIndex = Mathf.Clamp(index, 0, AvailableResolutions.Length - 1);
			ApplyFullscreenAndResolution(Fullscreen, ResolutionIndex);
		}
	}

	public void SaveCurrent()
	{
		PlayerPrefs.SetFloat("Settings.MouseSensitivity", MouseSensitivity);
		PlayerPrefs.SetInt("Settings.InvertY", InvertY ? 1 : 0);
		PlayerPrefs.SetInt("Settings.QualityLevel", QualityLevel);
		PlayerPrefs.SetInt("Settings.VSync", VSync ? 1 : 0);
		PlayerPrefs.SetInt("Settings.Fullscreen", Fullscreen ? 1 : 0);
		PlayerPrefs.SetInt("Settings.ResolutionIndex", ResolutionIndex);
		PlayerPrefs.Save();
	}

	public void ResetToDefaults()
	{
		PreviewMouseSensitivity(defaultMouseSensitivity);
		PreviewInvertY(defaultInvertY);
		PreviewQualityLevel(defaultQualityLevel);
		PreviewVSync(defaultVSync);
		PreviewFullscreen(defaultFullscreen);
		PreviewResolution(FindDefaultResolutionIndex());
	}

	private void ApplyQuality(int level)
	{
		if (level >= 0 && level < QualitySettings.names.Length)
		{
			QualitySettings.SetQualityLevel(level, applyExpensiveChanges: true);
		}
	}

	private void ApplyVSync(bool enabled)
	{
		QualitySettings.vSyncCount = (enabled ? 1 : 0);
	}

	private void ApplyFullscreenAndResolution(bool fullscreen, int resolutionIndex)
	{
		if (AvailableResolutions != null && AvailableResolutions.Length != 0 && resolutionIndex >= 0 && resolutionIndex < AvailableResolutions.Length)
		{
			Resolution resolution = AvailableResolutions[resolutionIndex];
			Screen.SetResolution(resolution.width, resolution.height, fullscreen);
		}
	}
}
