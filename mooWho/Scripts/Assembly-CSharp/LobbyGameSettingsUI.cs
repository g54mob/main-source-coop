using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyGameSettingsUI : MonoBehaviour
{
	private struct RowInstance
	{
		public string nameKey;

		public TMP_Text nameText;

		public Button minus;

		public Button plus;

		public TMP_Text valueText;

		public Func<string> getValue;

		public CanvasGroup canvasGroup;

		public RectTransform rect;
	}

	private struct RowDef
	{
		public string nameKey;

		public Action onMinus;

		public Action onPlus;

		public Func<string> getValue;
	}

	[Header("Panel")]
	public GameObject panelRoot;

	[Tooltip("Örnek satır (LobbyGameSettingsPanel/SettingItem) — SettingNameText/MinusButton/PlusButton/ValueText çocuklarını içerir. Runtime'da bundan N kopya üretilip kendisi gizlenir.")]
	public GameObject itemTemplate;

	public KeyCode toggleKey = KeyCode.E;

	[Tooltip("LobbyGameSettingsPanel/ResetButton — tüm ayarları default'a döndürür")]
	public Button resetButton;

	[Tooltip("ResetButton'ın içindeki Text (TMP) — BTN_RESET anahtarıyla otomatik çevrilir")]
	public TMP_Text resetButtonText;

	[Tooltip("Paneli kapatan Back butonu — host olmasa da herkes kullanabilir")]
	public Button backButton;

	[Tooltip("BackButton'ın içindeki Text (TMP) — BTN_BACK anahtarıyla otomatik çevrilir")]
	public TMP_Text backButtonText;

	[Header("Animasyon (basit pop)")]
	public float panelPopDuration = 0.2f;

	public float rowPopDuration = 0.18f;

	[Tooltip("Satırlar arası gecikme — panel açılınca sırayla pop'lasınlar diye")]
	public float rowStagger = 0.05f;

	private readonly List<RowInstance> _rows = new List<RowInstance>();

	private bool _isShowing;

	private Coroutine _panelAnimRoutine;

	private readonly List<Coroutine> _rowPopRoutines = new List<Coroutine>();

	public static LobbyGameSettingsUI Instance { get; private set; }

	public static bool IsShowing
	{
		get
		{
			if (Instance != null)
			{
				return Instance._isShowing;
			}
			return false;
		}
	}

	private void Awake()
	{
		Instance = this;
		if (panelRoot != null)
		{
			panelRoot.SetActive(value: false);
		}
		BuildRows();
		resetButton?.onClick.AddListener(delegate
		{
			LobbyGameSettings.Instance?.ResetToDefaults();
		});
		backButton?.onClick.AddListener(delegate
		{
			SetShowing(show: false);
		});
		AttachButtonFX(resetButton);
		AttachButtonFX(backButton);
		Localization.OnLanguageChanged += RefreshValues;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
		Localization.OnLanguageChanged -= RefreshValues;
	}

	private void Update()
	{
		if (_isShowing && Input.GetKeyDown(KeyCode.Escape))
		{
			SetShowing(show: false);
		}
		else if (Input.GetKeyDown(toggleKey) && CanToggle())
		{
			SetShowing(!_isShowing);
		}
	}

	private RowDef[] BuildRowDefs()
	{
		return new RowDef[9]
		{
			new RowDef
			{
				nameKey = "SETTING_MAP",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.CycleMap(-1);
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.CycleMap(1);
				},
				getValue = () => (!(LobbyGameSettings.Instance != null)) ? "-" : MapLabel(LobbyGameSettings.Instance.selectedMap)
			},
			new RowDef
			{
				nameKey = "SETTING_MAX_RECORD_LENGTH",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.DecrementRecordLength();
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.IncrementRecordLength();
				},
				getValue = () => (!(LobbyGameSettings.Instance != null)) ? "-" : Localization.GetFormatted("TIMER_SECONDS_SHORT", LobbyGameSettings.Instance.maxRecordSeconds)
			},
			new RowDef
			{
				nameKey = "SETTING_GAME_DURATION",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.DecrementDuration();
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.IncrementDuration();
				},
				getValue = () => (!(LobbyGameSettings.Instance != null)) ? "-" : LobbyGameSettings.Instance.durationMinutes.ToString()
			},
			new RowDef
			{
				nameKey = "SETTING_HUNTER_COUNT",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.DecrementHunterCount();
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.IncrementHunterCount();
				},
				getValue = () => (!(LobbyGameSettings.Instance != null)) ? "-" : LobbyGameSettings.Instance.hunterCount.ToString()
			},
			new RowDef
			{
				nameKey = "SETTING_HUNTER_PENALTY",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.DecrementHunterPenalty();
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.IncrementHunterPenalty();
				},
				getValue = () => (!(LobbyGameSettings.Instance != null)) ? "-" : LobbyGameSettings.Instance.hunterPenaltySeconds.ToString()
			},
			new RowDef
			{
				nameKey = "SETTING_HUNTER_AMMO",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.DecrementAmmo();
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.IncrementAmmo();
				},
				getValue = () => (!(LobbyGameSettings.Instance != null)) ? "-" : LobbyGameSettings.Instance.hunterAmmo.ToString()
			},
			new RowDef
			{
				nameKey = "SETTING_BUZZING_INTERVAL",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.DecrementBuzzing();
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.IncrementBuzzing();
				},
				getValue = delegate
				{
					LobbyGameSettings instance = LobbyGameSettings.Instance;
					if (instance == null)
					{
						return "-";
					}
					return (instance.buzzingInterval > 0) ? instance.buzzingInterval.ToString() : Localization.Get("SETTING_OFF");
				}
			},
			new RowDef
			{
				nameKey = "SETTING_ANIMAL_SOUND",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.CycleAnimalSound(-1);
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.CycleAnimalSound(1);
				},
				getValue = () => (!(LobbyGameSettings.Instance != null)) ? "-" : LevelLabel(LobbyGameSettings.Instance.animalSound)
			},
			new RowDef
			{
				nameKey = "SETTING_NPC_POPULATION",
				onMinus = delegate
				{
					LobbyGameSettings.Instance?.CycleNpcPopulation(-1);
				},
				onPlus = delegate
				{
					LobbyGameSettings.Instance?.CycleNpcPopulation(1);
				},
				getValue = () => (!(LobbyGameSettings.Instance != null)) ? "-" : LevelLabel(LobbyGameSettings.Instance.npcPopulation)
			}
		};
	}

	private void BuildRows()
	{
		if (itemTemplate == null)
		{
			Debug.LogError("[LobbyGameSettingsUI] itemTemplate atanmamış — satırlar üretilemedi.");
			return;
		}
		itemTemplate.SetActive(value: false);
		Transform parent = itemTemplate.transform.parent;
		RowDef[] array = BuildRowDefs();
		for (int i = 0; i < array.Length; i++)
		{
			RowDef def = array[i];
			GameObject gameObject = UnityEngine.Object.Instantiate(itemTemplate, parent);
			gameObject.SetActive(value: true);
			RowInstance item = new RowInstance
			{
				nameKey = def.nameKey,
				nameText = gameObject.transform.Find("SettingNameText")?.GetComponent<TMP_Text>(),
				minus = gameObject.transform.Find("MinusButton")?.GetComponent<Button>(),
				plus = gameObject.transform.Find("PlusButton")?.GetComponent<Button>(),
				valueText = gameObject.transform.Find("ValueText")?.GetComponent<TMP_Text>(),
				getValue = def.getValue,
				canvasGroup = GetOrAddCanvasGroup(gameObject),
				rect = gameObject.GetComponent<RectTransform>()
			};
			item.minus?.onClick.AddListener(delegate
			{
				def.onMinus?.Invoke();
			});
			item.plus?.onClick.AddListener(delegate
			{
				def.onPlus?.Invoke();
			});
			AttachButtonFX(item.minus);
			AttachButtonFX(item.plus);
			_rows.Add(item);
		}
	}

	private void AttachButtonFX(Button btn)
	{
		if (!(btn == null))
		{
			UIButtonFX uIButtonFX = btn.GetComponent<UIButtonFX>();
			if (uIButtonFX == null)
			{
				uIButtonFX = btn.gameObject.AddComponent<UIButtonFX>();
			}
			MenuManager instance = MenuManager.Instance;
			if (instance != null)
			{
				uIButtonFX.Configure(instance, instance.hoverScale, instance.clickScale, instance.fxSpeed);
			}
		}
	}

	private bool CanToggle()
	{
		if (RoleAssignmentUI.IsShowing || AdminPanelUI.IsOpen)
		{
			return false;
		}
		if (MenuManager.IsSettingsOpen)
		{
			return false;
		}
		return true;
	}

	private void SetShowing(bool show)
	{
		_isShowing = show;
		StopAllPanelAnimations();
		if (panelRoot != null)
		{
			panelRoot.SetActive(show);
		}
		if (show)
		{
			RefreshValues();
			RefreshInteractable();
			CursorManager.Instance?.PushUI();
			_panelAnimRoutine = StartCoroutine(AnimatePanelIn());
		}
		else
		{
			CursorManager.Instance?.PopUI();
		}
	}

	private IEnumerator AnimatePanelIn()
	{
		CanvasGroup panelCg = GetOrAddCanvasGroup(panelRoot);
		RectTransform panelRect = panelRoot.GetComponent<RectTransform>();
		panelCg.alpha = 0f;
		if (panelRect != null)
		{
			panelRect.localScale = Vector3.one * 0.9f;
		}
		foreach (RowInstance row in _rows)
		{
			if (row.canvasGroup != null)
			{
				row.canvasGroup.alpha = 0f;
			}
			if (row.rect != null)
			{
				row.rect.localScale = Vector3.one * 0.7f;
			}
		}
		float t = 0f;
		while (t < panelPopDuration)
		{
			t += Time.deltaTime;
			float t2 = (panelCg.alpha = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / panelPopDuration), 3f));
			if (panelRect != null)
			{
				panelRect.localScale = Vector3.one * Mathf.Lerp(0.9f, 1f, t2);
			}
			yield return null;
		}
		panelCg.alpha = 1f;
		if (panelRect != null)
		{
			panelRect.localScale = Vector3.one;
		}
		foreach (RowInstance row2 in _rows)
		{
			_rowPopRoutines.Add(StartCoroutine(PopRow(row2)));
			yield return new WaitForSeconds(rowStagger);
		}
		_panelAnimRoutine = null;
	}

	private IEnumerator PopRow(RowInstance row)
	{
		float t = 0f;
		while (t < rowPopDuration)
		{
			t += Time.deltaTime;
			float num = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / rowPopDuration), 3f);
			if (row.canvasGroup != null)
			{
				row.canvasGroup.alpha = num;
			}
			if (row.rect != null)
			{
				row.rect.localScale = Vector3.one * Mathf.Lerp(0.7f, 1f, num);
			}
			yield return null;
		}
		if (row.canvasGroup != null)
		{
			row.canvasGroup.alpha = 1f;
		}
		if (row.rect != null)
		{
			row.rect.localScale = Vector3.one;
		}
	}

	private void StopAllPanelAnimations()
	{
		if (_panelAnimRoutine != null)
		{
			StopCoroutine(_panelAnimRoutine);
			_panelAnimRoutine = null;
		}
		foreach (Coroutine rowPopRoutine in _rowPopRoutines)
		{
			if (rowPopRoutine != null)
			{
				StopCoroutine(rowPopRoutine);
			}
		}
		_rowPopRoutines.Clear();
	}

	private static CanvasGroup GetOrAddCanvasGroup(GameObject go)
	{
		CanvasGroup canvasGroup = go.GetComponent<CanvasGroup>();
		if (canvasGroup == null)
		{
			canvasGroup = go.AddComponent<CanvasGroup>();
		}
		return canvasGroup;
	}

	private void RefreshInteractable()
	{
		bool interactable = LobbyGameSettings.Instance != null && LobbyGameSettings.Instance.isServer;
		foreach (RowInstance row in _rows)
		{
			if (row.minus != null)
			{
				row.minus.interactable = interactable;
			}
			if (row.plus != null)
			{
				row.plus.interactable = interactable;
			}
		}
		if (resetButton != null)
		{
			resetButton.interactable = interactable;
		}
	}

	public void RefreshValues()
	{
		foreach (RowInstance row in _rows)
		{
			if (row.nameText != null)
			{
				row.nameText.text = Localization.Get(row.nameKey);
			}
			if (row.valueText != null)
			{
				row.valueText.text = row.getValue();
			}
		}
		if (resetButtonText != null)
		{
			resetButtonText.text = Localization.Get("BTN_RESET");
		}
		if (backButtonText != null)
		{
			backButtonText.text = Localization.Get("BTN_BACK");
		}
	}

	private static string LevelLabel(AnimalSoundLevel level)
	{
		return level switch
		{
			AnimalSoundLevel.Low => Localization.Get("LEVEL_LOW"), 
			AnimalSoundLevel.High => Localization.Get("LEVEL_HIGH"), 
			_ => Localization.Get("LEVEL_MEDIUM"), 
		};
	}

	private static string LevelLabel(NpcPopulationLevel level)
	{
		return level switch
		{
			NpcPopulationLevel.Low => Localization.Get("LEVEL_LOW"), 
			NpcPopulationLevel.High => Localization.Get("LEVEL_HIGH"), 
			_ => Localization.Get("LEVEL_MEDIUM"), 
		};
	}

	private static string MapLabel(MapType map)
	{
		if (map == MapType.Forest)
		{
			return Localization.Get("MAP_FOREST");
		}
		return Localization.Get("MAP_FARM");
	}
}
