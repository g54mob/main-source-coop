using System.Globalization;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class InGameLobbySettingsView : MonoBehaviour
	{
		[Tooltip("Açılıp kapanan panelin KÖKÜ. Bu bileşen panelin kendisinde durmak zorunda değil; başka bir objeye koyduysan paneli buraya sürükle.\n\nBoş bırakılırsa bu objenin kendisi kullanılır.\n\nAlanlar ve LobbySettingRow satırları bu kökün ALTINDA aranır, bu bileşenin altında değil, yani satırları panelin içinde bırakabilirsin.\n\nDİKKAT: bu bileşeni buraya verdiğin panelin İÇİNE koyma. Panel kapanınca bileşen de kapanır ve bir daha açamaz. Panelin dışında bir objeye koy (menü kökü, bir manager).")]
		[SerializeField]
		private GameObject panelRoot;

		[Header("Ortak")]
		[Tooltip("Lobi adı. Tarayıcıda ve arkadaş listesinde görünen ad.")]
		[SerializeField]
		private TMP_InputField lobbyNameField;

		[Tooltip("Lobiye alınacak en fazla kişi. Üst sınır 16. Küçültmek kimseyi atmaz, yalnızca yeni katılımları keser.")]
		[SerializeField]
		private TMP_InputField maxPlayersField;

		[Header("Prop hunt")]
		[SerializeField]
		private TMP_InputField prepSecondsField;

		[SerializeField]
		private TMP_InputField huntSecondsField;

		[SerializeField]
		private TMP_InputField roundEndSecondsField;

		[SerializeField]
		private TMP_InputField minHidersField;

		[SerializeField]
		private TMP_InputField minHuntersField;

		[Tooltip("Zorunlu taunt aralığı, saniye. Varsayılan 30, sınırlar 10 ile 120.")]
		[SerializeField]
		private TMP_InputField tauntIntervalField;

		[Tooltip("Iskalama cezası çarpanı. Min/Max/Whole Numbers değerleri KODDAN kurulur.")]
		[SerializeField]
		private Slider selfDamageSlider;

		[Tooltip("Slider'ın yanındaki çarpan yazısı - 'x1', 'x1.25'. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI selfDamageLabel;

		[Tooltip("Round başında Avcı olacakların oranı. Min/Max/Whole Numbers değerleri KODDAN kurulur.\n\nBir sonraki round'un dağılımını değiştirir; süren round'daki tarafları değiştirmez.")]
		[SerializeField]
		private Slider hunterShareSlider;

		[Tooltip("Slider'ın yanındaki oran yazısı - '%25 · 4 Avcı / 12 Modelci'. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI hunterShareLabel;

		[Header("Deathmatch")]
		[SerializeField]
		private TMP_InputField minPlayersField;

		[SerializeField]
		private TMP_InputField scoreLimitField;

		[SerializeField]
		private TMP_InputField timeLimitField;

		[SerializeField]
		private TMP_InputField extraWarmupField;

		[SerializeField]
		private TMP_InputField respawnTimeField;

		[Header("Denetim")]
		[Tooltip("Değişiklikleri sunucuya gönderen buton. Yalnızca host'ta çalışır.")]
		[SerializeField]
		private Button applyButton;

		[Tooltip("Alanları kayıtlı değerlere geri alan buton. İsteğe bağlı.")]
		[SerializeField]
		private Button revertButton;

		[Tooltip("Paneli kapatan buton. İsteğe bağlı - panelin dışından da Close() çağrılabilir.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("Bu paneli AÇAN buton - duraklatma menüsündeki 'Lobi Ayarları'. Buraya verirsen yalnızca host'ta görünür, client'larda tamamen gizlenir.\n\nButonun kendisini değil, gizlenmesini istediğin objeyi ver: satırın başlığı ya da ikonu ayrı bir objeyse bu ikisini saran objeyi sürükle.\n\nBoş bırakılırsa hiçbir şey gizlenmez ve panel eskisi gibi herkese açılır (client'ta salt okunur).")]
		[SerializeField]
		private GameObject openButton;

		[Tooltip("Sonucu ve neden düzenlenemediğini yazan satır. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI statusLabel;

		private LobbySettingRow[] settingRows;

		private FixedString32Bytes appliedRowsForMode;

		private bool wired;

		private bool? shownOpenButton;

		private static bool CanEdit
		{
			get
			{
				if (LobbySettingsSync.Instance != null)
				{
					return LobbySettingsSync.Instance.IsServer;
				}
				return false;
			}
		}

		public bool IsOpen => Root.activeSelf;

		private GameObject Root
		{
			get
			{
				if (!(panelRoot != null))
				{
					return base.gameObject;
				}
				return panelRoot;
			}
		}

		private void Awake()
		{
			if (panelRoot == null)
			{
				panelRoot = base.gameObject;
			}
			else if (panelRoot != base.gameObject && base.transform.IsChildOf(panelRoot.transform))
			{
				Debug.LogWarning("[LobbyAyarlari] '" + base.name + "' kontrol ettigi panelin ICINDE duruyor ('" + panelRoot.name + "'). Panel kapaninca bu bilesen de kapanir ve bir daha acamaz - bileseni panelin disina tasi.", this);
			}
			if (panelRoot.activeSelf)
			{
				panelRoot.SetActive(value: false);
			}
		}

		private void Update()
		{
			bool canEdit = CanEdit;
			if (shownOpenButton != canEdit)
			{
				shownOpenButton = canEdit;
				if (openButton != null && openButton.activeSelf != canEdit)
				{
					openButton.SetActive(canEdit);
				}
			}
			if (!canEdit && openButton != null && IsOpen)
			{
				Close();
			}
		}

		private void OnEnable()
		{
			Wire();
			if (panelRoot == base.gameObject)
			{
				Revert();
			}
		}

		public void SetOpen(bool open)
		{
			Wire();
			GameObject root = Root;
			if (root.activeSelf != open)
			{
				root.SetActive(open);
			}
			if (open)
			{
				Revert();
			}
		}

		public void Open()
		{
			SetOpen(open: true);
		}

		public void Close()
		{
			SetOpen(open: false);
		}

		public void Toggle()
		{
			SetOpen(!IsOpen);
		}

		private void Wire()
		{
			if (!wired)
			{
				wired = true;
				if (applyButton != null)
				{
					applyButton.onClick.AddListener(Apply);
				}
				if (revertButton != null)
				{
					revertButton.onClick.AddListener(Revert);
				}
				if (closeButton != null)
				{
					closeButton.onClick.AddListener(Close);
				}
				BindSelfDamageSlider();
				BindHunterShareSlider();
			}
		}

		public void Revert()
		{
			LobbySettingsSync instance = LobbySettingsSync.Instance;
			if (instance == null)
			{
				SetStatus(Loc.Get("LobbySettings.NoSession"));
				SetEditable(editable: false);
				return;
			}
			LobbySettingsData settings = instance.CurrentSettings;
			GameModeController current = GameModeController.Current;
			if (current != null)
			{
				settings = current.WithDefaults(settings);
			}
			SetText(lobbyNameField, settings.LobbyName.ToString());
			SetNumber(maxPlayersField, settings.MaxPlayers);
			SetNumber(prepSecondsField, settings.PrepSeconds);
			SetNumber(huntSecondsField, settings.HuntSeconds);
			SetNumber(roundEndSecondsField, settings.RoundEndSeconds);
			SetNumber(minHidersField, settings.MinHiders);
			SetNumber(minHuntersField, settings.MinHunters);
			SetNumber(tauntIntervalField, settings.TauntIntervalSeconds);
			SetNumber(minPlayersField, settings.MinPlayers);
			SetNumber(scoreLimitField, settings.ScoreLimit);
			SetNumber(timeLimitField, settings.TimeLimitSeconds);
			SetNumber(extraWarmupField, settings.ExtraWarmupSeconds);
			SetNumber(respawnTimeField, settings.RespawnSeconds);
			if (selfDamageSlider != null)
			{
				selfDamageSlider.SetValueWithoutNotify(GameModeController.ResolveSelfDamagePercent(settings.SelfDamagePercent) / 25);
			}
			if (hunterShareSlider != null)
			{
				hunterShareSlider.SetValueWithoutNotify(GameModeController.ResolveHunterSharePercent(settings.HunterSharePercent) / 5);
			}
			RefreshSelfDamageLabel();
			RefreshHunterShareLabel();
			ApplySettingRows(settings.ModeId);
			bool isServer = instance.IsServer;
			SetEditable(isServer);
			SetStatus(isServer ? "" : Loc.Get("LobbySettings.HostOnly"));
		}

		public void Apply()
		{
			LobbySettingsSync instance = LobbySettingsSync.Instance;
			if (instance == null)
			{
				SetStatus(Loc.Get("LobbySettings.NoSession"));
				return;
			}
			LobbySettingsData currentSettings = instance.CurrentSettings;
			string text = TextOf(lobbyNameField);
			if (!string.IsNullOrWhiteSpace(text))
			{
				currentSettings.LobbyName = new FixedString64Bytes(Clip(text, 60));
			}
			currentSettings.MaxPlayers = LobbyCapacity.Clamp(Number(maxPlayersField, currentSettings.MaxPlayers));
			currentSettings.PrepSeconds = Number(prepSecondsField, currentSettings.PrepSeconds);
			currentSettings.HuntSeconds = Number(huntSecondsField, currentSettings.HuntSeconds);
			currentSettings.RoundEndSeconds = Number(roundEndSecondsField, currentSettings.RoundEndSeconds);
			currentSettings.MinHiders = Number(minHidersField, currentSettings.MinHiders);
			currentSettings.MinHunters = Number(minHuntersField, currentSettings.MinHunters);
			int num = Number(tauntIntervalField, currentSettings.TauntIntervalSeconds);
			currentSettings.TauntIntervalSeconds = ((num <= 0) ? 30 : RoundManager.ClampTauntInterval(num));
			currentSettings.SelfDamagePercent = SelfDamageSetting(currentSettings.SelfDamagePercent);
			currentSettings.HunterSharePercent = HunterShareSetting(currentSettings.HunterSharePercent);
			currentSettings.MinPlayers = Number(minPlayersField, currentSettings.MinPlayers);
			currentSettings.ScoreLimit = Number(scoreLimitField, currentSettings.ScoreLimit);
			currentSettings.TimeLimitSeconds = Number(timeLimitField, currentSettings.TimeLimitSeconds);
			currentSettings.ExtraWarmupSeconds = Number(extraWarmupField, currentSettings.ExtraWarmupSeconds);
			currentSettings.RespawnSeconds = Number(respawnTimeField, currentSettings.RespawnSeconds);
			if (!instance.ServerApplySettings(currentSettings))
			{
				SetStatus(Loc.Get("LobbySettings.HostOnly"));
				return;
			}
			Revert();
			SetStatus(Loc.Get("LobbySettings.Applied"));
		}

		private void ApplySettingRows(FixedString32Bytes modeId)
		{
			if (appliedRowsForMode.Equals(modeId) && settingRows != null)
			{
				return;
			}
			appliedRowsForMode = modeId;
			if (settingRows == null)
			{
				settingRows = Root.GetComponentsInChildren<LobbySettingRow>(includeInactive: true);
			}
			GameModeDefinition mode = GameModeCatalog.Find(modeId.ToString());
			LobbySettingRow[] array = settingRows;
			foreach (LobbySettingRow lobbySettingRow in array)
			{
				if (lobbySettingRow != null)
				{
					lobbySettingRow.Apply(mode);
				}
			}
			UILayout.RebuildFromDeferred(this, Root.transform);
		}

		private void BindSelfDamageSlider()
		{
			if (!(selfDamageSlider == null))
			{
				selfDamageSlider.wholeNumbers = true;
				selfDamageSlider.minValue = 1f;
				selfDamageSlider.maxValue = 8f;
				selfDamageSlider.onValueChanged.AddListener(delegate
				{
					RefreshSelfDamageLabel();
				});
			}
		}

		private int SelfDamageSetting(int fallback)
		{
			if (!(selfDamageSlider == null))
			{
				return GameModeController.ResolveSelfDamagePercent(Mathf.RoundToInt(selfDamageSlider.value) * 25);
			}
			return fallback;
		}

		private void RefreshSelfDamageLabel()
		{
			if (selfDamageLabel != null)
			{
				selfDamageLabel.text = GameModeController.SelfDamageLabel(SelfDamageSetting(100));
			}
		}

		private void BindHunterShareSlider()
		{
			if (hunterShareSlider == null)
			{
				return;
			}
			hunterShareSlider.wholeNumbers = true;
			hunterShareSlider.minValue = 1f;
			hunterShareSlider.maxValue = 19f;
			hunterShareSlider.onValueChanged.AddListener(delegate
			{
				RefreshHunterShareLabel();
			});
			if (maxPlayersField != null)
			{
				maxPlayersField.onEndEdit.AddListener(delegate
				{
					RefreshHunterShareLabel();
				});
			}
		}

		private int HunterShareSetting(int fallback)
		{
			if (!(hunterShareSlider == null))
			{
				return GameModeController.ResolveHunterSharePercent(Mathf.RoundToInt(hunterShareSlider.value) * 5);
			}
			return fallback;
		}

		private void RefreshHunterShareLabel()
		{
			if (hunterShareLabel != null)
			{
				hunterShareLabel.text = GameModeController.HunterShareLabel(HunterShareSetting(30), LobbyCapacity.Clamp(Number(maxPlayersField, 16)));
			}
		}

		private void SetEditable(bool editable)
		{
			SetInteractable(lobbyNameField, editable);
			SetInteractable(maxPlayersField, editable);
			SetInteractable(prepSecondsField, editable);
			SetInteractable(huntSecondsField, editable);
			SetInteractable(roundEndSecondsField, editable);
			SetInteractable(minHidersField, editable);
			SetInteractable(minHuntersField, editable);
			SetInteractable(tauntIntervalField, editable);
			SetInteractable(minPlayersField, editable);
			SetInteractable(scoreLimitField, editable);
			SetInteractable(timeLimitField, editable);
			SetInteractable(extraWarmupField, editable);
			SetInteractable(respawnTimeField, editable);
			if (selfDamageSlider != null)
			{
				selfDamageSlider.interactable = editable;
			}
			if (hunterShareSlider != null)
			{
				hunterShareSlider.interactable = editable;
			}
			if (applyButton != null)
			{
				applyButton.interactable = editable;
			}
			if (revertButton != null)
			{
				revertButton.interactable = editable;
			}
		}

		private static void SetInteractable(TMP_InputField field, bool editable)
		{
			if (field != null)
			{
				field.interactable = editable;
			}
		}

		private static void SetText(TMP_InputField field, string text)
		{
			if (field != null)
			{
				field.SetTextWithoutNotify(text ?? "");
			}
		}

		private static void SetNumber(TMP_InputField field, int value)
		{
			SetText(field, (value > 0) ? value.ToString(CultureInfo.InvariantCulture) : "");
		}

		private static string TextOf(TMP_InputField field)
		{
			if (!(field != null))
			{
				return "";
			}
			return field.text;
		}

		private static int Number(TMP_InputField field, int fallback)
		{
			string text = TextOf(field);
			if (string.IsNullOrWhiteSpace(text))
			{
				if (!(field == null))
				{
					return 0;
				}
				return fallback;
			}
			if (!int.TryParse(text, out var result))
			{
				return fallback;
			}
			return result;
		}

		private static string Clip(string text, int max)
		{
			if (text.Length > max)
			{
				return text.Substring(0, max);
			}
			return text;
		}

		private void SetStatus(string text)
		{
			if (statusLabel != null)
			{
				statusLabel.text = text;
			}
		}
	}
}
