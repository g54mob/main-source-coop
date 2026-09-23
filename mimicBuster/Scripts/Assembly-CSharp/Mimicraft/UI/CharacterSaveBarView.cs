using System.Collections.Generic;
using Mimicraft.Customization;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class CharacterSaveBarView : MonoBehaviour
	{
		[Header("Üst çubuk")]
		[Tooltip("Karakter adı alanı.")]
		[SerializeField]
		private TMP_InputField nameField;

		[Tooltip("Açık dosyanın üstüne yazan buton.")]
		[SerializeField]
		private Button saveButton;

		[Tooltip("Sıfırdan yeni bir karakter başlatan buton. Parçalar varsayılana döner ve açık dosya bırakılır; diske hiçbir şey yazılmaz, Kaydet'e basana kadar.")]
		[SerializeField]
		private Button createNewButton;

		[Tooltip("Ekrandan çıkan buton.")]
		[SerializeField]
		private Button closeButton;

		[Tooltip("Kaydedildi / yüklendi / silindi bilgisinin yazılacağı yer. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI statusLabel;

		[Tooltip("Durum yazısının ekranda kalma süresi, saniye.")]
		[SerializeField]
		[Min(0.5f)]
		private float statusSeconds = 4f;

		[Header("Kayıtlı karakterler")]
		[Tooltip("Listeyi açıp kapayan buton. İsteğe bağlı - liste sürekli açık da olabilir.")]
		[SerializeField]
		private Button browseButton;

		[Tooltip("Browse butonuyla açılıp kapanacak obje. Boş bırakılırsa liste hep açık kalır.")]
		[SerializeField]
		private GameObject listPanel;

		[Tooltip("Satırların altına ekleneceği obje. Boş bırakılırsa List Panel kullanılır.")]
		[SerializeField]
		private Transform rowContainer;

		[Tooltip("Bir satırın prefab'ı - üzerinde CharacterSaveRowView olmalı.")]
		[SerializeField]
		private CharacterSaveRowView rowTemplate;

		[Tooltip("Hiç kayıtlı karakter yokken açılacak obje. İsteğe bağlı.")]
		[SerializeField]
		private GameObject emptyState;

		[Header("Hazır karakterler")]
		[Tooltip("'Yeni'ye basınca çıkan preset listesi. Boş bırakılırsa bu ekranın altında aranır; hiç yoksa 'Yeni' eskisi gibi doğrudan boş karakteri açar.")]
		[SerializeField]
		private PresetPickerView presetPicker;

		[SerializeField]
		private Sprite emptySprite;

		private readonly List<CharacterSaveRowView> rows = new List<CharacterSaveRowView>();

		private readonly List<CharacterListEntry> entries = new List<CharacterListEntry>();

		private CharacterCustomizationView screen;

		private float statusClearTime;

		private bool warnedAboutTemplate;

		private bool warnedAboutPresets;

		private void Awake()
		{
			if (saveButton != null)
			{
				saveButton.onClick.AddListener(OnSave);
			}
			if (createNewButton != null)
			{
				createNewButton.onClick.AddListener(OnCreateNew);
			}
			if (closeButton != null)
			{
				closeButton.onClick.AddListener(OnClose);
			}
			if (browseButton != null)
			{
				browseButton.onClick.AddListener(ToggleList);
			}
			if (nameField != null)
			{
				nameField.onValueChanged.AddListener(OnNameChanged);
			}
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
		}

		public void Bind(CharacterCustomizationView screen)
		{
			this.screen = screen;
			if (presetPicker == null && screen != null)
			{
				presetPicker = screen.GetComponentInChildren<PresetPickerView>(includeInactive: true);
			}
			if (nameField != null)
			{
				nameField.SetTextWithoutNotify((screen != null) ? screen.CurrentName : "");
			}
			RefreshList();
		}

		private void OnEnable()
		{
			CharacterPortraitService.PortraitWritten += OnPortraitWritten;
			if (screen != null)
			{
				RefreshList();
			}
		}

		private void OnDisable()
		{
			CharacterPortraitService.PortraitWritten -= OnPortraitWritten;
		}

		private void OnPortraitWritten(string characterFilePath)
		{
			if (base.isActiveAndEnabled)
			{
				RefreshList();
			}
		}

		private void OnNameChanged(string value)
		{
			screen?.SetName(value);
		}

		private void OnClose()
		{
			screen?.Close();
		}

		private void OnSave()
		{
			if (!(screen == null))
			{
				screen.Save();
				RefreshList();
				SetStatus("'" + screen.CurrentName + "' kaydedildi.");
			}
		}

		private void OnCreateNew()
		{
			if (screen == null)
			{
				return;
			}
			IReadOnlyList<CharacterAssetDefinition> presets = screen.Presets;
			if (presetPicker == null || presets.Count == 0)
			{
				if (!warnedAboutPresets)
				{
					warnedAboutPresets = true;
					Debug.LogWarning((presetPicker == null) ? ("[CharacterSaveBarView] '" + base.name + "' bir PresetPickerView bulamadi - ne Inspector'da atanmis ne de Customization ekraninin altinda var. 'Yeni' hazir karakter listesi acamaz.") : "[CharacterSaveBarView] Bu rig'de hic preset yok - CharacterRigDefinition'daki Presets listesini doldur. 'Yeni' dogrudan bos karakteri aciyor.", this);
				}
				StartNew(null);
				return;
			}
			List<PresetPickerData> list = new List<PresetPickerData>
			{
				new PresetPickerData
				{
					name = Loc.Get("Preset.Empty"),
					icon = (emptySprite ?? null)
				}
			};
			foreach (CharacterAssetDefinition item in presets)
			{
				if (item != null)
				{
					list.Add(new PresetPickerData
					{
						name = item.DisplayName,
						icon = (item.icon ? item.icon : (emptySprite ?? null))
					});
				}
			}
			presetPicker.Toggle(this, Loc.Get("Preset.NewCharacterTitle"), list, delegate(int chosen)
			{
				StartNew((chosen > 0 && chosen <= presets.Count) ? presets[chosen - 1] : null);
			});
		}

		private void StartNew(CharacterAssetDefinition preset)
		{
			if (!(screen == null))
			{
				screen.CreateNew((preset != null) ? preset.DisplayName : "", preset?.asset);
				if (nameField != null)
				{
					nameField.SetTextWithoutNotify(screen.CurrentName);
				}
				RefreshList();
				SetStatus(Loc.Format("WeaponSkin.PresetStarted", screen.CurrentName));
			}
		}

		private void Duplicate(string path, string entryName)
		{
			if (screen == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(screen.Duplicate(path)))
			{
				SetStatus("'" + entryName + "' kopyalanamadi.");
				return;
			}
			if (nameField != null)
			{
				nameField.SetTextWithoutNotify(screen.CurrentName);
			}
			RefreshList();
			SetStatus("'" + entryName + "' kopyalandi - artik kopya uzerinde calisiyorsun.");
		}

		private void ToggleList()
		{
			if (!(listPanel == null))
			{
				bool flag = !listPanel.activeSelf;
				listPanel.SetActive(flag);
				if (flag)
				{
					RefreshList();
				}
			}
		}

		public void RefreshList()
		{
			entries.Clear();
			entries.AddRange(CharacterStorage.List());
			if (emptyState != null)
			{
				emptyState.SetActive(entries.Count == 0);
			}
			if (screen != null && screen.Rig != null)
			{
				CharacterPortraitService.RequestMissing(screen.Rig);
			}
			if (rowTemplate == null)
			{
				if (entries.Count > 0 && !warnedAboutTemplate)
				{
					warnedAboutTemplate = true;
					Debug.LogWarning("[CharacterSaveBarView] Row Template atanmamis - kayitli karakterler listelenmeyecek. Uzerinde CharacterSaveRowView olan bir satir prefab'i ata.", this);
				}
				return;
			}
			Transform parent = ((rowContainer != null) ? rowContainer : ((listPanel != null) ? listPanel.transform : base.transform));
			while (rows.Count < entries.Count)
			{
				CharacterSaveRowView characterSaveRowView = Object.Instantiate(rowTemplate, parent);
				characterSaveRowView.gameObject.SetActive(value: true);
				rows.Add(characterSaveRowView);
			}
			string text = ((screen != null) ? screen.CurrentFilePath : "");
			for (int i = 0; i < rows.Count; i++)
			{
				CharacterSaveRowView characterSaveRowView2 = rows[i];
				if (characterSaveRowView2 == null)
				{
					continue;
				}
				if (i >= entries.Count)
				{
					if (characterSaveRowView2.gameObject.activeSelf)
					{
						characterSaveRowView2.gameObject.SetActive(value: false);
					}
					continue;
				}
				if (!characterSaveRowView2.gameObject.activeSelf)
				{
					characterSaveRowView2.gameObject.SetActive(value: true);
				}
				CharacterListEntry characterListEntry = entries[i];
				bool isOpen = !string.IsNullOrEmpty(text) && characterListEntry.FilePath == text;
				characterSaveRowView2.Bind(characterListEntry.FilePath, characterListEntry.CharacterName, isOpen, Load, Delete, Duplicate);
			}
		}

		private void Load(string path, string entryName)
		{
			if (!(screen == null))
			{
				screen.Load(path);
				if (nameField != null)
				{
					nameField.SetTextWithoutNotify(screen.CurrentName);
				}
				RefreshList();
				SetStatus("'" + entryName + "' yuklendi.");
			}
		}

		private void Delete(string path, string entryName)
		{
			if (!(screen == null))
			{
				screen.Delete(path);
				RefreshList();
				SetStatus("'" + entryName + "' silindi.");
			}
		}

		private void SetStatus(string text)
		{
			if (!(statusLabel == null))
			{
				statusLabel.text = text;
				statusClearTime = Time.unscaledTime + statusSeconds;
			}
		}

		private void Update()
		{
			if (statusLabel != null && statusLabel.text.Length > 0 && Time.unscaledTime >= statusClearTime)
			{
				statusLabel.text = "";
			}
		}
	}
}
