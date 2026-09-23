using System;
using System.Collections.Generic;
using DG.Tweening;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.Tutorial;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ToolbarView : MonoBehaviour
	{
		[Serializable]
		private struct OrientButton
		{
			public Button button;

			public VoxelOrientation operation;
		}

		public static readonly Color ActiveColor = new Color(0.41960785f, 0.6f, 0.56078434f, 1f);

		public static readonly Color InactiveColor = new Color(0.7372549f, 0.8745098f, 72f / 85f, 1f);

		private static readonly (EditorState state, string label, string hotkey, Color iconColor)[] Buttons = new(EditorState, string, string, Color)[4]
		{
			(EditorState.Transform, "Transform", "Q", new Color(0.35f, 0.6f, 0.95f, 1f)),
			(EditorState.Extrude, "Extrude", "W", new Color(0.95f, 0.6f, 0.25f, 1f)),
			(EditorState.Paint, "Paint", "E", new Color(0.9f, 0.35f, 0.55f, 1f)),
			(EditorState.LoopCut, "Loop Cut", "R", new Color(0.75f, 0.4f, 0.95f, 1f))
		};

		[SerializeField]
		private RectTransform content;

		[SerializeField]
		private Image[] buttonBackgrounds = new Image[Buttons.Length];

		[SerializeField]
		private Button[] buttons = new Button[Buttons.Length];

		[SerializeField]
		private Button undoButton;

		[SerializeField]
		private Button redoButton;

		[SerializeField]
		private Button litToggleButton;

		[SerializeField]
		private Image litToggleBackground;

		[SerializeField]
		private Button dimensionToggleButton;

		[SerializeField]
		private Image dimensionToggleBackground;

		[Tooltip("Şablon kütüphanesini açıp kapatan buton. İsteğe bağlı - bağlanmazsa araç çubuğu eskisi gibi çalışır.")]
		[SerializeField]
		private Button libraryButton;

		[Tooltip("Butonun zemini. Kütüphane açıkken renk değiştirir - diğer aç/kapa butonlarıyla aynı davranış. Boş bırakılırsa butonun kendi Image'i kullanılır.")]
		[SerializeField]
		private Image libraryBackground;

		[Tooltip("Açılıp kapanacak kütüphane penceresi. Boş bırakılırsa sahnede aranır - pencere kapalı başladığı için arama devre dışı objeleri de kapsar.")]
		[SerializeField]
		private TemplateBrowserView library;

		[Tooltip("Düzenleme geçmişi panelini açıp kapatan buton. İsteğe bağlı - bağlanmazsa panel yalnızca H tuşuyla açılıp kapanır.")]
		[SerializeField]
		private Button historyToggleButton;

		[Tooltip("Butonun zemini. Panel açıkken renk değiştirir. Boş bırakılırsa butonun kendi Image'i kullanılır.")]
		[SerializeField]
		private Image historyToggleBackground;

		[Tooltip("Açılıp kapanacak geçmiş paneli. Boş bırakılırsa sahnede aranır.")]
		[SerializeField]
		private EditHistoryView editHistory;

		[Tooltip("Modeli bütün olarak çeviren/döndüren butonlar. Her satıra bir buton ve o butonun ne yapacağı yazılır; hiç satır eklemezsen araç çubuğu eskisi gibi çalışır.\n\nAynı işlemi birden fazla butona bağlayabilirsin (mesela hem üst barda hem yan panelde), ve istemediğin işlemi hiç koymayabilirsin - 180° dönüşler bu yüzden ayrı satırlar.")]
		[SerializeField]
		private OrientButton[] orientButtons = Array.Empty<OrientButton>();

		[Tooltip("Primitif şekiller penceresini açıp kapatan buton. İsteğe bağlı - bağlanmazsa araç çubuğu eskisi gibi çalışır.\n\nYalnızca Modelci'yken görünür: pencere bir GÖVDEYİ değiştiriyor, karakter ve silah ekranlarında ise gövde yok. Pencerenin kendisi koddan kurulur, sahneye bir şey eklemen gerekmez.")]
		[SerializeField]
		private Button primitivesButton;

		[Tooltip("Modeli baştaki küpe döndüren buton. İsteğe bağlı - bağlanmazsa araç çubuğu eskisi gibi çalışır.\n\nNeyin sıfırlanabileceğine ve sıfırlanıp sıfırlanamayacağına gövdenin kendisi karar veriyor (PlayerVoxelBody.RequestResetToCube), ve olmaz dediğinde sebebini de söylüyor - burada tekrar edilecek bir kural yok.")]
		[SerializeField]
		private Button resetButton;

		private static readonly Dictionary<Transform, Vector3> authoredScales = new Dictionary<Transform, Vector3>();

		private static readonly Color UnavailableColor = new Color(0.16f, 0.17f, 0.19f, 1f);

		private UITooltipTrigger[] tooltips = new UITooltipTrigger[Buttons.Length];

		private string[] toolTooltips = new string[Buttons.Length];

		public static ToolbarView Instance { get; private set; }

		private static string UnavailableTooltip => "\n<size=88%><color=#E08A6A>" + Loc.Get("Tooltip.Unavailable") + "</color></size>";

		private static string LessonTooltip => "\n<size=88%><color=#9AB8B4>" + Loc.Get("ToolLessonHint") + "</color></size>";

		private static string LockedByTutorialTooltip => "\n<size=88%><color=#FFD37A>" + Loc.Get("ToolLocked") + "</color></size>";

		public static ToolbarView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "Toolbar");
			rectTransform.anchorMin = new Vector2(0.5f, 0f);
			rectTransform.anchorMax = new Vector2(0.5f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.anchoredPosition = new Vector2(0f, 16f);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "Content");
			rectTransform2.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform2.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform2.pivot = new Vector2(0.5f, 0.5f);
			rectTransform2.anchoredPosition = Vector2.zero;
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform2.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			ContentSizeFitter contentSizeFitter = rectTransform2.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			Image[] array = new Image[Buttons.Length];
			Button[] array2 = new Button[Buttons.Length];
			Image iconImage;
			for (int i = 0; i < Buttons.Length; i++)
			{
				(EditorState state, string label, string hotkey, Color iconColor) tuple = Buttons[i];
				EditorState item = tuple.state;
				string item2 = tuple.label;
				string item3 = tuple.hotkey;
				Sprite iconSprite = PlaceholderIcons.CreateSquare(tuple.iconColor);
				Button button = UIFactory.CreateIconButton(rectTransform2, $"{item}Button", iconSprite, new Vector2(22f, 22f), out iconImage);
				((RectTransform)button.transform).sizeDelta = new Vector2(48f, 48f);
				button.gameObject.AddComponent<UITooltipTrigger>().Text = item2 + " (" + item3 + ")";
				array[i] = button.GetComponent<Image>();
				array2[i] = button;
			}
			RectTransform rectTransform3 = UIFactory.CreateRect(rectTransform, "UndoRedoContent");
			rectTransform3.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform3.pivot = new Vector2(0.5f, 0.5f);
			rectTransform3.anchoredPosition = new Vector2(0f, 50f);
			HorizontalLayoutGroup horizontalLayoutGroup2 = rectTransform3.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup2.spacing = 8f;
			horizontalLayoutGroup2.childForceExpandWidth = false;
			horizontalLayoutGroup2.childForceExpandHeight = false;
			horizontalLayoutGroup2.childAlignment = TextAnchor.MiddleCenter;
			ContentSizeFitter contentSizeFitter2 = rectTransform3.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter2.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter2.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			Sprite iconSprite2 = PlaceholderIcons.CreateSquare(new Color(0.7f, 0.7f, 0.7f, 1f));
			Button button2 = UIFactory.CreateIconButton(rectTransform3, "UndoButton", iconSprite2, new Vector2(16f, 16f), out iconImage);
			((RectTransform)button2.transform).sizeDelta = new Vector2(36f, 32f);
			button2.gameObject.AddComponent<UITooltipTrigger>().Text = "Undo (Ctrl+Z)";
			Sprite iconSprite3 = PlaceholderIcons.CreateSquare(new Color(0.8f, 0.8f, 0.8f, 1f));
			Button button3 = UIFactory.CreateIconButton(rectTransform3, "RedoButton", iconSprite3, new Vector2(16f, 16f), out iconImage);
			((RectTransform)button3.transform).sizeDelta = new Vector2(36f, 32f);
			button3.gameObject.AddComponent<UITooltipTrigger>().Text = "Redo (Ctrl+Y)";
			Sprite iconSprite4 = PlaceholderIcons.CreateSquare(new Color(0.95f, 0.85f, 0.3f, 1f));
			Button button4 = UIFactory.CreateIconButton(rectTransform3, "LitToggleButton", iconSprite4, new Vector2(16f, 16f), out iconImage);
			((RectTransform)button4.transform).sizeDelta = new Vector2(36f, 32f);
			button4.gameObject.AddComponent<UITooltipTrigger>().Text = "Toggle Lit/Unlit (Q)";
			Sprite iconSprite5 = PlaceholderIcons.CreateSquare(new Color(0.4f, 0.8f, 0.95f, 1f));
			Button button5 = UIFactory.CreateIconButton(rectTransform3, "DimensionToggleButton", iconSprite5, new Vector2(16f, 16f), out iconImage);
			((RectTransform)button5.transform).sizeDelta = new Vector2(36f, 32f);
			button5.gameObject.AddComponent<UITooltipTrigger>().Text = "Toggle Dimensions";
			ToolbarView toolbarView = rectTransform.gameObject.AddComponent<ToolbarView>();
			toolbarView.content = rectTransform2;
			toolbarView.buttonBackgrounds = array;
			toolbarView.buttons = array2;
			toolbarView.undoButton = button2;
			toolbarView.redoButton = button3;
			toolbarView.litToggleButton = button4;
			toolbarView.litToggleBackground = button4.GetComponent<Image>();
			toolbarView.dimensionToggleButton = button5;
			toolbarView.dimensionToggleBackground = button5.GetComponent<Image>();
			return toolbarView;
		}

		public RectTransform GetToolButton(EditorState state)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (Buttons[i].state == state && buttons[i] != null)
				{
					return (RectTransform)buttons[i].transform;
				}
			}
			return null;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		private void Awake()
		{
			Instance = this;
			CacheTooltips();
			for (int i = 0; i < Buttons.Length; i++)
			{
				EditorState state = Buttons[i].state;
				Button button = buttons[i];
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(delegate
				{
					VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera)?.SetState(state);
					Punch(button.transform);
				});
				if (button.GetComponent<TutorialLessonLink>() == null)
				{
					button.gameObject.AddComponent<TutorialLessonLink>();
				}
			}
			if (resetButton != null)
			{
				resetButton.onClick.RemoveAllListeners();
				resetButton.onClick.AddListener(delegate
				{
					Punch(resetButton.transform);
					AskThenReset();
				});
			}
			if (primitivesButton != null)
			{
				primitivesButton.onClick.RemoveAllListeners();
				primitivesButton.onClick.AddListener(delegate
				{
					Punch(primitivesButton.transform);
					PrimitiveShapeView.Resolve(RootCanvas())?.Toggle();
				});
			}
			undoButton.onClick.RemoveAllListeners();
			undoButton.onClick.AddListener(UndoIfIdle);
			redoButton.onClick.RemoveAllListeners();
			redoButton.onClick.AddListener(RedoIfIdle);
			litToggleButton.onClick.RemoveAllListeners();
			litToggleButton.onClick.AddListener(delegate
			{
				VoxelEditorSettings.UnlitMode = !VoxelEditorSettings.UnlitMode;
				litToggleBackground.color = (VoxelEditorSettings.UnlitMode ? InactiveColor : ActiveColor);
				Punch(litToggleButton.transform);
			});
			if (libraryButton != null)
			{
				if (libraryBackground == null)
				{
					libraryBackground = libraryButton.GetComponent<Image>();
				}
				ResolveLibrary();
				libraryButton.onClick.RemoveAllListeners();
				libraryButton.onClick.AddListener(delegate
				{
					TemplateBrowserView templateBrowserView = ResolveLibrary();
					if (templateBrowserView == null)
					{
						Debug.LogWarning("[ToolbarView] Kutuphane penceresi bulunamadi - Library alanini doldur ya da sahneye bir TemplateBrowserView koy.", this);
					}
					else
					{
						templateBrowserView.Toggle();
						Punch(libraryButton.transform);
					}
				});
			}
			if (historyToggleButton != null)
			{
				if (historyToggleBackground == null)
				{
					historyToggleBackground = historyToggleButton.GetComponent<Image>();
				}
				ResolveHistory();
				historyToggleButton.onClick.RemoveAllListeners();
				historyToggleButton.onClick.AddListener(delegate
				{
					EditHistoryView editHistoryView = ResolveHistory();
					if (editHistoryView != null)
					{
						editHistoryView.Toggle();
					}
					else
					{
						VoxelEditorSettings.ShowHistory = !VoxelEditorSettings.ShowHistory;
					}
					Punch(historyToggleButton.transform);
				});
			}
			for (int num = 0; num < orientButtons.Length; num++)
			{
				Button button2 = orientButtons[num].button;
				if (!(button2 == null))
				{
					VoxelOrientation operation = orientButtons[num].operation;
					button2.onClick.RemoveAllListeners();
					button2.onClick.AddListener(delegate
					{
						Punch(button2.transform);
						Orient(operation);
					});
				}
			}
			dimensionToggleButton.onClick.RemoveAllListeners();
			dimensionToggleButton.onClick.AddListener(delegate
			{
				VoxelEditorSettings.ShowDimensions = !VoxelEditorSettings.ShowDimensions;
				dimensionToggleBackground.color = (VoxelEditorSettings.ShowDimensions ? ActiveColor : InactiveColor);
				Punch(dimensionToggleButton.transform);
			});
		}

		private static void Orient(VoxelOrientation operation)
		{
			if (VoxelFocusManager.IsAnyGestureActive())
			{
				return;
			}
			VoxelEditorController focused = VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera);
			if (!(focused == null) && !(focused.Model == null) && !VoxelOrientCommand.TryApply(focused.Model, operation, out var error))
			{
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(error);
				}
				else
				{
					Debug.LogWarning("[ToolbarView] " + VoxelOrientCommand.LabelOf(operation) + ": " + error, null);
				}
			}
		}

		private TemplateBrowserView ResolveLibrary()
		{
			if (library == null)
			{
				library = UnityEngine.Object.FindFirstObjectByType<TemplateBrowserView>(FindObjectsInactive.Include);
			}
			return library;
		}

		private EditHistoryView ResolveHistory()
		{
			if (editHistory == null)
			{
				editHistory = UnityEngine.Object.FindFirstObjectByType<EditHistoryView>(FindObjectsInactive.Include);
			}
			return editHistory;
		}

		private void AskThenReset()
		{
			IModelEditSession active = ModelEditSession.Active;
			if (active == null)
			{
				if (PlayerVoxelBody.CanLocalReset(report: true))
				{
					Ask(delegate
					{
						PlayerVoxelBody.RequestLocalReset();
					});
				}
			}
			else if (!active.CanResetModel)
			{
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get("Reset.Unavailable"));
				}
			}
			else
			{
				Ask(active.ResetModel);
			}
		}

		private void Ask(Action onConfirm)
		{
			DialogView.Confirm(Loc.Get("Reset.Confirm"), onConfirm);
		}

		private Transform RootCanvas()
		{
			Canvas componentInParent = GetComponentInParent<Canvas>();
			if (!(componentInParent != null))
			{
				return null;
			}
			return componentInParent.rootCanvas.transform;
		}

		private static void Punch(Transform target)
		{
			if (!(target == null))
			{
				if (!authoredScales.TryGetValue(target, out var value))
				{
					value = target.localScale;
					authoredScales[target] = value;
				}
				target.DOKill();
				target.localScale = value;
				target.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f).SetEase(Ease.OutCubic);
			}
		}

		private static void UndoIfIdle()
		{
			if (!VoxelFocusManager.IsAnyGestureActive())
			{
				UndoManager.Undo();
			}
		}

		private static void RedoIfIdle()
		{
			if (!VoxelFocusManager.IsAnyGestureActive())
			{
				UndoManager.Redo();
			}
		}

		private void Update()
		{
			bool flag = VoxelFocusManager.IsAnyGestureActive();
			undoButton.interactable = UndoManager.CanUndo && !flag;
			redoButton.interactable = UndoManager.CanRedo && !flag;
			litToggleBackground.color = (VoxelEditorSettings.UnlitMode ? InactiveColor : ActiveColor);
			dimensionToggleBackground.color = (VoxelEditorSettings.ShowDimensions ? ActiveColor : InactiveColor);
			if (libraryBackground != null)
			{
				libraryBackground.color = ((library != null && library.IsOpen) ? ActiveColor : InactiveColor);
			}
			if (libraryButton != null && !VoxelEditorController.IsTypingInField && GameInput.ToggleLibrary.WasPressedThisFrame())
			{
				ResolveLibrary()?.Toggle();
			}
			if (historyToggleBackground != null)
			{
				historyToggleBackground.color = (VoxelEditorSettings.ShowHistory ? ActiveColor : InactiveColor);
			}
			if (primitivesButton != null && primitivesButton.gameObject.activeSelf != PrimitiveShapeView.Available)
			{
				primitivesButton.gameObject.SetActive(PrimitiveShapeView.Available);
			}
			if (libraryButton != null && libraryButton.gameObject.activeSelf != TemplateBrowserView.Available)
			{
				libraryButton.gameObject.SetActive(TemplateBrowserView.Available);
			}
			VoxelEditorController focused = VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera);
			content.gameObject.SetActive(focused != null);
			if (focused == null)
			{
				return;
			}
			for (int i = 0; i < Buttons.Length; i++)
			{
				EditorState item = Buttons[i].state;
				bool flag2 = VoxelEditorSettings.IsToolAvailable(item) && (item != EditorState.Transform || focused.AllowTransform);
				buttons[i].interactable = flag2;
				buttonBackgrounds[i].color = ((!flag2) ? UnavailableColor : ((focused.State == item) ? ActiveColor : InactiveColor));
				if (tooltips[i] != null)
				{
					tooltips[i].Suffix = ((!flag2) ? (VoxelEditorSettings.IsLockedByTutorial(item) ? LockedByTutorialTooltip : UnavailableTooltip) : ((TutorialDirector.Instance != null) ? LessonTooltip : null));
				}
			}
		}

		private void CacheTooltips()
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (!(buttons[i] == null))
				{
					tooltips[i] = buttons[i].GetComponent<UITooltipTrigger>();
					toolTooltips[i] = ((tooltips[i] != null) ? tooltips[i].Text : "");
				}
			}
		}
	}
}
