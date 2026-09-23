using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mimicraft.Customization;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class TemplateBrowserView : MonoBehaviour
	{
		[Serializable]
		private class CardRefs
		{
			public GameObject Root;

			public TextMeshProUGUI NameLabel;

			public Button LoadButton;

			public Button DeleteButton;

			public Button DuplicateButton;

			public GameObject Indicator;

			public Image Thumbnail;

			public Color RestColor;

			public bool HasRestColor;
		}

		private const int Capacity = 24;

		private const float CardWidth = 140f;

		private const float CardHeight = 116f;

		private const float Spacing = 12f;

		private const int Columns = 6;

		[SerializeField]
		private List<CardRefs> cards = new List<CardRefs>();

		private readonly List<TemplateListEntry> templates = new List<TemplateListEntry>();

		[SerializeField]
		private Button closeButton;

		[SerializeField]
		private Button newCardButton;

		[Tooltip("Kart görünümü. Üzerinde TemplateCardView olan bir prefab ver; havuz bundan üretilir. Boş bırakılırsa kartlar koddan çizilir (eski davranış).")]
		[SerializeField]
		private TemplateCardView cardPrefab;

		[Tooltip("Kartların içine dizileceği obje - genelde Grid Layout Group taşıyan panel. Boş bırakılırsa '+ Yeni' butonunun üst objesi kullanılır.")]
		[SerializeField]
		private Transform cardParent;

		[Tooltip("İsimle arama. Yazdıkça liste süzülür. İsteğe bağlı.")]
		[SerializeField]
		private TMP_InputField searchField;

		[Tooltip("Şu anki modeli kaydeder. Adı bir pencere sorar. İsteğe bağlı.")]
		[SerializeField]
		private Button saveButton;

		[Tooltip("Seçili şablonun bir kopyasını çıkarır. Seçim yokken kapalı. İsteğe bağlı.")]
		[SerializeField]
		private Button duplicateButton;

		[Tooltip("Seçili şablonu siler, önce onay sorar. Seçim yokken kapalı. İsteğe bağlı.")]
		[SerializeField]
		private Button deleteButton;

		[Tooltip("Seçili şablon için FBX dışa aktarma panelini açar. Seçim yokken kapalı. Diskteki KAYITLI hâli dışa aktarılır, editördeki kaydedilmemiş değişiklikler değil. İsteğe bağlı.")]
		[SerializeField]
		private Button exportButton;

		[Tooltip("Dışa aktarma paneli. Boş bırakılırsa sahnede aranır (kapalı objeler dahil).")]
		[SerializeField]
		private ModelExportView exportView;

		[Tooltip("Etiket süzgeci. Kayıtlı modellerdeki etiketlerden kendisi doluyor; ilk satır her zaman 'hepsi'. Kaydederken de seçili etiket öneri olarak gelir. İsteğe bağlı - bağlanmazsa etiketler yine kaydedilir, sadece süzülemez.")]
		[SerializeField]
		private TMP_Dropdown tagFilter;

		private string tagChoice = "";

		private readonly List<string> tagRows = new List<string>();

		private int selected = -1;

		private readonly List<TemplateListEntry> shown = new List<TemplateListEntry>();

		private string openCategory = "";

		public Func<string> SaveNameRequested { get; set; }

		public Func<string> SaveTagRequested { get; set; }

		public static bool Available => Category != null;

		public static string Category
		{
			get
			{
				if (!VoxelEditorSettings.IsMovementMode && !VoxelEditorSettings.SinglePieceEditing && !VoxelEditorSettings.TutorialLocksExtras)
				{
					return TemplateSession.CategoryOf(VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera));
				}
				return null;
			}
		}

		private static VoxelEditorController Subject => VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera);

		public bool IsOpen => base.gameObject.activeSelf;

		public event Action<string, string> SaveRequested;

		public event Action<TemplateListEntry> TemplateLoadRequested;

		public static TemplateBrowserView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "TemplateBrowser");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0.08f, 0.08f, 0.08f, 0.97f);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "Header");
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.sizeDelta = new Vector2(0f, 56f);
			rectTransform2.anchoredPosition = Vector2.zero;
			UIFactory.CreateLabel(rectTransform2, "Title", "Şablonlar", 22);
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform2, "CloseButton", "Kapat", out text);
			RectTransform obj = (RectTransform)button.transform;
			obj.anchorMin = new Vector2(1f, 0.5f);
			obj.anchorMax = new Vector2(1f, 0.5f);
			obj.pivot = new Vector2(1f, 0.5f);
			obj.anchoredPosition = new Vector2(-16f, 0f);
			obj.sizeDelta = new Vector2(90f, 36f);
			RectTransform rectTransform3 = UIFactory.CreateRect(rectTransform, "Grid");
			rectTransform3.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform3.pivot = new Vector2(0.5f, 0.5f);
			rectTransform3.anchoredPosition = new Vector2(0f, -20f);
			GridLayoutGroup gridLayoutGroup = rectTransform3.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(140f, 116f);
			gridLayoutGroup.spacing = new Vector2(12f, 12f);
			gridLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = 6;
			ContentSizeFitter contentSizeFitter = rectTransform3.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			Button button2 = CreateNewCard(rectTransform3);
			List<CardRefs> list = new List<CardRefs>();
			for (int i = 0; i < 24; i++)
			{
				list.Add(CreateTemplateCard(rectTransform3, i));
			}
			rectTransform.gameObject.SetActive(value: false);
			TemplateBrowserView templateBrowserView = rectTransform.gameObject.AddComponent<TemplateBrowserView>();
			templateBrowserView.closeButton = button;
			templateBrowserView.newCardButton = button2;
			templateBrowserView.cards = list;
			return templateBrowserView;
		}

		private static Button CreateNewCard(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "NewCard");
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0.22f, 0.4f, 0.26f, 0.95f);
			Button result = rectTransform.gameObject.AddComponent<Button>();
			RectTransform obj = (RectTransform)UIFactory.CreateLabel(rectTransform, "Label", "+ Yeni", 18).transform;
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.one;
			obj.offsetMin = Vector2.zero;
			obj.offsetMax = Vector2.zero;
			return result;
		}

		private static CardRefs CreateTemplateCard(Transform parent, int index)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, $"Card{index}");
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.95f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 4f;
			verticalLayoutGroup.padding = new RectOffset(6, 6, 6, 6);
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			((RectTransform)UIFactory.CreatePanel(rectTransform, "Icon", new Color(0.5f, 0.4f, 0.3f, 1f)).transform).sizeDelta = new Vector2(128f, 44f);
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "NameLabel", "", 13);
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(128f, 18f);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "ButtonRow");
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform2.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 4f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			rectTransform2.sizeDelta = new Vector2(128f, 24f);
			float x = (128f - horizontalLayoutGroup.spacing) / 2f;
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform2, "LoadButton", "Yükle", out text);
			((RectTransform)button.transform).sizeDelta = new Vector2(x, 24f);
			Button button2 = UIFactory.CreateButton(rectTransform2, "DeleteButton", "Sil", out text);
			((RectTransform)button2.transform).sizeDelta = new Vector2(x, 24f);
			rectTransform.gameObject.SetActive(value: false);
			return new CardRefs
			{
				Root = rectTransform.gameObject,
				NameLabel = textMeshProUGUI,
				LoadButton = button,
				DeleteButton = button2
			};
		}

		private void Awake()
		{
			EnsureCards();
			if (closeButton != null)
			{
				closeButton.onClick.RemoveAllListeners();
				closeButton.onClick.AddListener(Hide);
			}
			if (newCardButton != null)
			{
				newCardButton.onClick.RemoveAllListeners();
				newCardButton.onClick.AddListener(SaveAsNew);
			}
			if (searchField != null)
			{
				searchField.onValueChanged.RemoveAllListeners();
				searchField.onValueChanged.AddListener(delegate
				{
					ApplyFilter();
				});
			}
			if (saveButton != null)
			{
				saveButton.onClick.RemoveAllListeners();
				saveButton.onClick.AddListener(SaveCurrent);
			}
			if (tagFilter != null)
			{
				tagFilter.onValueChanged.RemoveAllListeners();
				tagFilter.onValueChanged.AddListener(delegate(int row)
				{
					tagChoice = ((row > 0 && row - 1 < tagRows.Count) ? tagRows[row - 1] : "");
					ApplyFilter();
				});
			}
			if (duplicateButton != null)
			{
				duplicateButton.onClick.RemoveAllListeners();
				duplicateButton.onClick.AddListener(DuplicateSelected);
			}
			if (deleteButton != null)
			{
				deleteButton.onClick.RemoveAllListeners();
				deleteButton.onClick.AddListener(DeleteSelected);
			}
			if (exportButton != null)
			{
				exportButton.onClick.RemoveAllListeners();
				exportButton.onClick.AddListener(ExportSelected);
			}
			for (int num = 0; num < cards.Count; num++)
			{
				CardRefs cardRefs = cards[num];
				int index = num;
				if (cardRefs.LoadButton != null)
				{
					cardRefs.LoadButton.onClick.RemoveAllListeners();
					cardRefs.LoadButton.onClick.AddListener(delegate
					{
						if (index < shown.Count && !IsPlaceholder(shown[index]))
						{
							TemplateSession.LoadEntry(shown[index], VoxelEditorSettings.ActiveCamera);
							this.TemplateLoadRequested?.Invoke(shown[index]);
							Hide();
						}
					});
				}
				if (cardRefs.DeleteButton != null)
				{
					cardRefs.DeleteButton.onClick.RemoveAllListeners();
					cardRefs.DeleteButton.onClick.AddListener(delegate
					{
						if (index < shown.Count && !IsPlaceholder(shown[index]))
						{
							Ask(shown[index]);
						}
					});
				}
				if (cardRefs.DuplicateButton != null)
				{
					cardRefs.DuplicateButton.onClick.RemoveAllListeners();
					cardRefs.DuplicateButton.onClick.AddListener(delegate
					{
						if (index < shown.Count && !IsPlaceholder(shown[index]))
						{
							Duplicate(shown[index]);
						}
					});
				}
				Button button = ((cardRefs.Root != null) ? cardRefs.Root.GetComponent<Button>() : null);
				if (button != null)
				{
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(delegate
					{
						Select(index);
					});
				}
			}
		}

		private void EnsureCards()
		{
			for (int num = cards.Count - 1; num >= 0; num--)
			{
				if (cards[num] == null || cards[num].Root == null)
				{
					cards.RemoveAt(num);
				}
			}
			if (cards.Count > 0)
			{
				return;
			}
			Transform transform = ((cardParent != null) ? cardParent : ((newCardButton != null) ? newCardButton.transform.parent : null));
			if (transform == null)
			{
				Debug.LogWarning("[TemplateBrowserView] Kartların dizileceği obje yok - Card Parent alanını doldur (ya da '+ Yeni' butonunu ızgaranın içine koy).", this);
				return;
			}
			for (int i = 0; i < 24; i++)
			{
				cards.Add((cardPrefab != null) ? StampCard(transform, i) : CreateTemplateCard(transform, i));
			}
		}

		private CardRefs StampCard(Transform grid, int index)
		{
			TemplateCardView templateCardView = UnityEngine.Object.Instantiate(cardPrefab, grid);
			templateCardView.name = $"Card{index}";
			templateCardView.gameObject.SetActive(value: false);
			if (!templateCardView.IsUsable)
			{
				if (index == 0)
				{
					Debug.LogWarning("[TemplateBrowserView] Kart prefab'inda Name Label bos - kart hangi sablonu gosterdigini soyleyemez, kartlar koddan cizilecek.", this);
				}
				UnityEngine.Object.Destroy(templateCardView.gameObject);
				return CreateTemplateCard(grid, index);
			}
			return new CardRefs
			{
				Root = templateCardView.gameObject,
				NameLabel = templateCardView.NameLabel,
				LoadButton = templateCardView.LoadButton,
				DeleteButton = templateCardView.DeleteButton,
				DuplicateButton = templateCardView.DuplicateButton,
				Indicator = templateCardView.SelectionIndicator,
				Thumbnail = templateCardView.Thumbnail
			};
		}

		private void Update()
		{
			if (!IsOpen)
			{
				return;
			}
			if (VoxelEditorSettings.IsMovementMode || !AnyEditable())
			{
				Hide();
				return;
			}
			string category = Category;
			if (category != null && !string.Equals(category, openCategory, StringComparison.Ordinal))
			{
				openCategory = category;
				Refresh();
			}
		}

		private static bool AnyEditable()
		{
			foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
			{
				if (TemplateSession.CategoryOf(item) != null)
				{
					return true;
				}
			}
			return false;
		}

		private void OnEnable()
		{
			TemplatePortraitService.PortraitWritten += OnPortraitWritten;
		}

		private void OnDisable()
		{
			TemplatePortraitService.PortraitWritten -= OnPortraitWritten;
		}

		private void OnPortraitWritten(string filePath)
		{
			if (!IsOpen)
			{
				return;
			}
			foreach (TemplateListEntry item in shown)
			{
				if (item.FilePath == filePath)
				{
					Redraw();
					break;
				}
			}
		}

		public void Show()
		{
			if (Available)
			{
				base.gameObject.SetActive(value: true);
				if (searchField != null)
				{
					searchField.SetTextWithoutNotify("");
				}
				openCategory = Category ?? "";
				Refresh();
				TemplatePortraitService.RequestMissing(Category ?? "");
			}
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		public string Report()
		{
			int num = 0;
			foreach (CardRefs card in cards)
			{
				if (card != null && card.Root != null)
				{
					num++;
				}
			}
			VoxelEditorController focused = VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("acik={0} uygun={1} kategori='{2}' ", IsOpen, Available, Category ?? "<null>") + "acilis='" + openCategory + "'");
			stringBuilder.AppendLine("odak=" + ((focused != null) ? focused.name : "<yok>") + " " + $"duzenlenebilir={AnyEditable()} hareketModu={VoxelEditorSettings.IsMovementMode}");
			stringBuilder.AppendLine($"kart havuzu={cards.Count} kullanilabilir={num} " + "kartPrefab=" + ((cardPrefab != null) ? "var" : "YOK") + " kartParent=" + ((cardParent != null) ? cardParent.name : "YOK"));
			stringBuilder.AppendLine("kaydet dinleyicisi=" + ((this.SaveRequested != null) ? "var" : "YOK") + " kaydetButonu=" + ((saveButton != null) ? "var" : "YOK") + " etiketSuzgeci=" + ((tagFilter != null) ? "var" : "YOK"));
			stringBuilder.AppendLine($"diskte={TemplateStorage.ListTemplates().Count} bu cekmecede={templates.Count} " + $"ekranda={shown.Count} etiket='{tagChoice}'");
			stringBuilder.Append("klasor=" + Path.Combine(Application.persistentDataPath, "Templates"));
			return stringBuilder.ToString();
		}

		public void Toggle()
		{
			if (IsOpen)
			{
				Hide();
			}
			else
			{
				Show();
			}
		}

		private void Refresh()
		{
			string b = Category ?? "";
			templates.Clear();
			foreach (TemplateListEntry item in TemplateStorage.ListTemplates())
			{
				if (string.Equals(item.Category, b, StringComparison.OrdinalIgnoreCase))
				{
					templates.Add(item);
				}
			}
			RefreshTagFilter();
			ApplyFilter();
		}

		private void RefreshTagFilter()
		{
			if (!(tagFilter == null))
			{
				tagRows.Clear();
				tagRows.AddRange(TemplateStorage.TagsOf(templates));
				List<string> list = new List<string>(tagRows.Count + 1) { Loc.Get("Library.AllTags") };
				list.AddRange(tagRows);
				tagFilter.ClearOptions();
				tagFilter.AddOptions(list);
				int num = ((!string.IsNullOrEmpty(tagChoice)) ? (tagRows.FindIndex((string t) => string.Equals(t, tagChoice, StringComparison.OrdinalIgnoreCase)) + 1) : 0);
				if (num <= 0)
				{
					num = 0;
					tagChoice = "";
				}
				tagFilter.SetValueWithoutNotify(num);
				tagFilter.RefreshShownValue();
				tagFilter.gameObject.SetActive(tagRows.Count > 0);
			}
		}

		private void ApplyFilter()
		{
			string previous = ((selected >= 0 && selected < shown.Count) ? shown[selected].FilePath : null);
			string text = ((searchField != null) ? searchField.text : null);
			shown.Clear();
			foreach (TemplateListEntry template in templates)
			{
				if ((string.IsNullOrEmpty(tagChoice) || string.Equals(template.Tag, tagChoice, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrWhiteSpace(text) || (template.ModelName ?? "").IndexOf(text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0))
				{
					shown.Add(template);
				}
			}
			if (!TemplateSession.HasOpenTemplate(Category))
			{
				shown.Insert(0, Placeholder());
			}
			selected = IndexOfOpen(previous);
			Redraw();
		}

		private TemplateListEntry Placeholder()
		{
			return new TemplateListEntry("", Loc.Get("Library.Unsaved"), Category ?? "", "");
		}

		private static bool IsPlaceholder(TemplateListEntry entry)
		{
			return string.IsNullOrEmpty(entry.FilePath);
		}

		private int IndexOfOpen(string previous)
		{
			if (shown.Count > 0 && IsPlaceholder(shown[0]))
			{
				return 0;
			}
			string currentTemplateFilePath = VoxelEditorSettings.CurrentTemplateFilePath;
			for (int i = 0; i < shown.Count; i++)
			{
				if (shown[i].FilePath == currentTemplateFilePath)
				{
					return i;
				}
			}
			for (int j = 0; j < shown.Count; j++)
			{
				if (previous != null && shown[j].FilePath == previous)
				{
					return j;
				}
			}
			return -1;
		}

		private void Redraw()
		{
			for (int i = 0; i < cards.Count; i++)
			{
				CardRefs cardRefs = cards[i];
				if (cardRefs.Root == null)
				{
					continue;
				}
				bool flag = i < shown.Count;
				cardRefs.Root.SetActive(flag);
				if (flag)
				{
					if (cardRefs.NameLabel != null)
					{
						cardRefs.NameLabel.text = shown[i].ModelName;
					}
					ShowPicture(cardRefs, shown[i]);
					bool active = !IsPlaceholder(shown[i]);
					if (cardRefs.DeleteButton != null)
					{
						cardRefs.DeleteButton.gameObject.SetActive(active);
					}
					if (cardRefs.DuplicateButton != null)
					{
						cardRefs.DuplicateButton.gameObject.SetActive(active);
					}
					Highlight(cardRefs, i == selected);
				}
			}
			bool interactable = selected >= 0 && selected < shown.Count && !IsPlaceholder(shown[selected]);
			if (duplicateButton != null)
			{
				duplicateButton.interactable = interactable;
			}
			if (deleteButton != null)
			{
				deleteButton.interactable = interactable;
			}
			if (exportButton != null)
			{
				exportButton.interactable = interactable;
			}
		}

		private static void ShowPicture(CardRefs refs, TemplateListEntry entry)
		{
			if (!(refs.Thumbnail == null))
			{
				Sprite sprite = (IsPlaceholder(entry) ? null : SavedThumbnails.Load(entry.FilePath));
				refs.Thumbnail.sprite = sprite;
				if (refs.Thumbnail.gameObject.activeSelf != (sprite != null))
				{
					refs.Thumbnail.gameObject.SetActive(sprite != null);
				}
			}
		}

		private static void Highlight(CardRefs refs, bool on)
		{
			Button component;
			if (refs.Indicator != null)
			{
				if (refs.Indicator.activeSelf != on)
				{
					refs.Indicator.SetActive(on);
				}
			}
			else if (!(refs.Root == null) && refs.Root.TryGetComponent<Button>(out component))
			{
				if (!refs.HasRestColor)
				{
					refs.HasRestColor = true;
					refs.RestColor = component.colors.normalColor;
				}
				ColorBlock colors = component.colors;
				colors.normalColor = (on ? colors.highlightedColor : refs.RestColor);
				component.colors = colors;
			}
		}

		private void Select(int index)
		{
			selected = ((index >= 0 && index < shown.Count) ? index : (-1));
			Redraw();
		}

		public void SaveCurrent()
		{
			if (TemplateSession.HasOpenTemplate(Category))
			{
				Commit(VoxelEditorSettings.CurrentTemplateName, VoxelEditorSettings.CurrentTemplateTag, asNew: false);
			}
			else
			{
				SaveAsNew();
			}
		}

		public void SaveAsNew()
		{
			string prefill = ((SaveNameRequested != null) ? SaveNameRequested() : "");
			bool slot = TemplateSession.IsSlot(Category);
			DialogView.Prompt(Loc.Get("Library.SaveName"), prefill, delegate(string name)
			{
				if (slot)
				{
					Commit(name, "", asNew: true);
				}
				else
				{
					string prefill2 = ((!string.IsNullOrEmpty(tagChoice)) ? tagChoice : ((SaveTagRequested != null) ? SaveTagRequested() : ""));
					DialogView.Prompt(Loc.Get("Library.SaveTag"), prefill2, delegate(string typed)
					{
						Commit(name, typed, asNew: true);
					}, null, null, requiresInput: false);
				}
			});
		}

		private void Commit(string name, string tag, bool asNew)
		{
			if (!TemplateSession.SaveCurrent(name, tag, asNew))
			{
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get("Library.NothingToSave"));
				}
				return;
			}
			this.SaveRequested?.Invoke(name, tag);
			if (ToastView.Instance != null)
			{
				ToastView.Instance.Show(Loc.Format("Library.Saved", name));
			}
			if (!string.IsNullOrWhiteSpace(tag))
			{
				tagChoice = tag.Trim();
			}
			Refresh();
		}

		private void DuplicateSelected()
		{
			if (selected >= 0 && selected < shown.Count && !IsPlaceholder(shown[selected]))
			{
				Duplicate(shown[selected]);
			}
		}

		private void Duplicate(TemplateListEntry entry)
		{
			string text = TemplateStorage.DuplicateTemplate(entry.FilePath, TemplateStorage.UnusedName(entry.ModelName));
			if (text != null)
			{
				TemplatePortraitService.RequestFor(text);
			}
			Refresh();
		}

		private void ExportSelected()
		{
			if (selected >= 0 && selected < shown.Count && !IsPlaceholder(shown[selected]))
			{
				if (exportView == null)
				{
					exportView = UnityEngine.Object.FindFirstObjectByType<ModelExportView>(FindObjectsInactive.Include);
				}
				if (exportView == null)
				{
					Debug.LogWarning("[TemplateBrowserView] Sahnede ModelExportView yok - dışa aktarma paneli açılamıyor. Paneli kurup üzerine ModelExportView ekle.", this);
				}
				else
				{
					exportView.Show(shown[selected]);
				}
			}
		}

		private void DeleteSelected()
		{
			if (selected >= 0 && selected < shown.Count)
			{
				Ask(shown[selected]);
			}
		}

		private void Ask(TemplateListEntry entry)
		{
			DialogView.Confirm(Loc.Format("Library.DeleteConfirm", entry.ModelName), delegate
			{
				TemplateSession.DeleteEntry(entry.FilePath);
				Refresh();
			});
		}
	}
}
