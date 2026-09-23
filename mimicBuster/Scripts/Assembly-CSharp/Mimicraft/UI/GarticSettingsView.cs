using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class GarticSettingsView : MonoBehaviour
	{
		[Tooltip("Panelin kökü. Oyun sürerken gizlenir, lobide ve oyun bitiminde görünür.")]
		[SerializeField]
		private GameObject root;

		[Tooltip("Kategori seçimi - harita ve mod seçicileriyle aynı kart ızgarası.")]
		[SerializeField]
		private WordCategoryPickerView categoryPicker;

		[Tooltip("Kelimelerin hangi dilde olacağı. Seçilen dilde kelime listesi OLAN kategoriler listelenir; olmayanlar hiç görünmez.\n\nOyunun arayüz dilinden ayrı bir seçim: aynı odada farklı dillerde oynayan insanlar olabilir, ve kelimelerin dili odanın kararıdır, herkesin kendi menüsününki değil.")]
		[SerializeField]
		private TMP_Dropdown wordLanguageDropdown;

		[SerializeField]
		private TMP_InputField scoreLimitField;

		[SerializeField]
		private TMP_InputField turnSecondsField;

		[Tooltip("Açıkken oda, tur geç kaldığında kendiliğinden de harf açar. Kapalıyken harf vermek yalnızca çizen oyuncunun elindedir.\n\nİsteğe bağlı - bağlanmazsa ayar kapalı kalır, ki varsayılanı da odur.")]
		[SerializeField]
		private Toggle autoHintsToggle;

		[Tooltip("Sadece host'ta tıklanabilir.")]
		[SerializeField]
		private Button startButton;

		[Tooltip("Host olmayanlara neden bekledikleri yazılır.")]
		[SerializeField]
		private TextMeshProUGUI statusLabel;

		private GarticRoundManager mode;

		private bool built;

		private readonly List<string> listedLanguages = new List<string>();

		private void Awake()
		{
			ResolveCategoryPicker();
			if (startButton != null)
			{
				startButton.onClick.RemoveAllListeners();
				startButton.onClick.AddListener(RequestStart);
			}
			if (categoryPicker != null)
			{
				categoryPicker.SelectionChanged += delegate
				{
					PushSettings();
				};
			}
			if (scoreLimitField != null)
			{
				scoreLimitField.onEndEdit.AddListener(delegate
				{
					PushSettings();
				});
			}
			if (turnSecondsField != null)
			{
				turnSecondsField.onEndEdit.AddListener(delegate
				{
					PushSettings();
				});
			}
			if (autoHintsToggle != null)
			{
				autoHintsToggle.onValueChanged.AddListener(delegate
				{
					PushSettings();
				});
			}
			if (wordLanguageDropdown != null)
			{
				wordLanguageDropdown.onValueChanged.RemoveAllListeners();
				wordLanguageDropdown.onValueChanged.AddListener(OnLanguagePicked);
			}
			BuildLanguageDropdown();
		}

		private void ResolveCategoryPicker()
		{
			if (!(categoryPicker != null))
			{
				categoryPicker = GetComponentInChildren<WordCategoryPickerView>(includeInactive: true) ?? Object.FindFirstObjectByType<WordCategoryPickerView>(FindObjectsInactive.Include);
				if (categoryPicker == null)
				{
					Debug.LogWarning("[GarticSettingsView] Kategori secici bulunamadi - kategori secimi sunucuya gitmez ve her tur varsayilan kategoriyle oynanir.", this);
				}
				else
				{
					Debug.LogWarning("[GarticSettingsView] Category Picker alani bos - sahnedeki secici kendi bulundu. Inspector'dan bagla.", this);
				}
			}
		}

		private void Update()
		{
			if (mode == null)
			{
				mode = GameModeController.Current as GarticRoundManager;
			}
			bool flag = NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer && mode != null && (mode.Phase.Value == GarticPhase.WaitingForPlayers || mode.Phase.Value == GarticPhase.GameOver);
			if (root != null && root.activeSelf != flag)
			{
				root.SetActive(flag);
			}
			if (flag && !(mode == null))
			{
				bool flag2 = NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer;
				if (!built || !flag2)
				{
					built = true;
					MirrorFromServer();
				}
				if (flag2 && mode.WordLanguage.Value.IsEmpty)
				{
					ShowLanguage(WordCategoryCatalog.DefaultLanguage());
					PushSettings();
				}
				if (startButton != null)
				{
					startButton.interactable = flag2 && mode.CanStart;
				}
				if (statusLabel != null)
				{
					statusLabel.text = ((!flag2) ? Loc.Get("WaitingForHost") : (mode.CanStart ? "" : Loc.Get("WaitingForPlayers")));
				}
			}
		}

		private void BuildLanguageDropdown()
		{
			if (wordLanguageDropdown == null)
			{
				return;
			}
			listedLanguages.Clear();
			listedLanguages.AddRange(WordCategoryCatalog.Languages());
			List<string> list = new List<string>(listedLanguages.Count);
			foreach (string listedLanguage in listedLanguages)
			{
				list.Add(Loc.LanguageName(listedLanguage));
			}
			wordLanguageDropdown.ClearOptions();
			wordLanguageDropdown.AddOptions(list);
		}

		private string SelectedLanguage()
		{
			if (wordLanguageDropdown != null)
			{
				int value = wordLanguageDropdown.value;
				if (value >= 0 && value < listedLanguages.Count)
				{
					return listedLanguages[value];
				}
			}
			if (!(mode != null))
			{
				return "";
			}
			return mode.WordLanguage.Value.ToString();
		}

		private void OnLanguagePicked(int index)
		{
			if (index >= 0 && index < listedLanguages.Count)
			{
				LobbyPrefs.SetString("Gartic.WordLanguage", listedLanguages[index]);
				if (categoryPicker != null)
				{
					categoryPicker.SetLanguage(listedLanguages[index]);
				}
				PushSettings();
			}
		}

		private void ShowLanguage(string language)
		{
			if (string.IsNullOrEmpty(language))
			{
				return;
			}
			if (wordLanguageDropdown != null)
			{
				int num = listedLanguages.IndexOf(language);
				if (num >= 0 && wordLanguageDropdown.value != num)
				{
					wordLanguageDropdown.SetValueWithoutNotify(num);
				}
			}
			if (categoryPicker != null)
			{
				categoryPicker.SetLanguage(language);
			}
		}

		private void MirrorFromServer()
		{
			if (scoreLimitField != null && !scoreLimitField.isFocused)
			{
				scoreLimitField.SetTextWithoutNotify(mode.ScoreLimit.Value.ToString());
			}
			if (turnSecondsField != null && !turnSecondsField.isFocused)
			{
				turnSecondsField.SetTextWithoutNotify(mode.TurnSeconds.Value.ToString());
			}
			if (autoHintsToggle != null && autoHintsToggle.isOn != mode.AutoHints.Value)
			{
				autoHintsToggle.SetIsOnWithoutNotify(mode.AutoHints.Value);
			}
			ShowLanguage(mode.WordLanguage.Value.ToString());
			if (categoryPicker != null)
			{
				categoryPicker.SelectById(mode.CategoryId.Value.ToString());
			}
		}

		private void PushSettings()
		{
			if (!(mode == null) && !(NetworkManager.Singleton == null) && NetworkManager.Singleton.IsServer)
			{
				string source = ((categoryPicker != null) ? categoryPicker.SelectedCategoryId : "");
				mode.ApplySettingsServerRpc(new FixedString32Bytes(source), ParseOr(scoreLimitField, 180), ParseOr(turnSecondsField, 60), autoHintsToggle != null && autoHintsToggle.isOn, new FixedString32Bytes(SelectedLanguage()));
			}
		}

		private static int ParseOr(TMP_InputField field, int fallback)
		{
			if (!(field != null) || !int.TryParse(field.text, out var result))
			{
				return fallback;
			}
			return result;
		}

		private void RequestStart()
		{
			if (!(mode == null))
			{
				PushSettings();
				mode.StartGameServerRpc();
			}
		}
	}
}
