using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Mimicraft.Tutorial
{
	public class TutorialLessonMenu : MonoBehaviour
	{
		[Tooltip("Açılıp kapanan panel. Boş bırakılırsa bu objenin kendisi.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Şablondan çoğaltılan ders butonlarının konacağı yer. Boş bırakılırsa şablonun kendi ebeveyni. Buraya bir Vertical Layout Group koy - kapalı dersler gizlenince boşluk kendiliğinden kapanır.")]
		[SerializeField]
		private Transform rowContainer;

		[Tooltip("Elle yerleştirilmiş bir butonu (TutorialLessonButton) olmayan her ders için çoğaltılan buton. Kendisi hep gizli kalır. İçindeki ilk TMP yazısına dersin adı yazılır.\n\nHer dersi elle yerleştirdiysen boş bırakabilirsin.")]
		[SerializeField]
		private Button rowTemplate;

		[Tooltip("Paneli kapatan buton. İsteğe bağlı - Esc her zaman kapatır.")]
		[SerializeField]
		private Button closeButton;

		private readonly Dictionary<string, GameObject> rows = new Dictionary<string, GameObject>();

		private bool built;

		public static TutorialLessonMenu Instance { get; private set; }

		private GameObject Panel
		{
			get
			{
				if (!(panel != null))
				{
					return base.gameObject;
				}
				return panel;
			}
		}

		public bool IsOpen => Panel.activeInHierarchy;

		public static void Open()
		{
			TutorialDirector instance = TutorialDirector.Instance;
			if (!(instance == null))
			{
				TutorialLessonMenu tutorialLessonMenu = Resolve(instance);
				if (tutorialLessonMenu != null)
				{
					tutorialLessonMenu.OpenThis();
				}
			}
		}

		public static void Close()
		{
			if (Instance != null)
			{
				Instance.CloseThis();
			}
		}

		public static void Toggle()
		{
			if (Instance != null && Instance.IsOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		private static TutorialLessonMenu Resolve(TutorialDirector director)
		{
			if (Instance != null)
			{
				return Instance;
			}
			Instance = Object.FindFirstObjectByType<TutorialLessonMenu>(FindObjectsInactive.Include);
			if (Instance != null)
			{
				return Instance;
			}
			Transform canvasTransform = director.CanvasTransform;
			if (canvasTransform == null)
			{
				return null;
			}
			Instance = CreateFallback(canvasTransform);
			return Instance;
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			if (IsOpen)
			{
				Panel.SetActive(value: false);
			}
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
			GameMenuState.SetMenuOpen(this, open: false);
		}

		private void OnDisable()
		{
			GameMenuState.SetMenuOpen(this, open: false);
		}

		private void Update()
		{
			if (!IsOpen)
			{
				GameMenuState.SetMenuOpen(this, open: false);
			}
			else if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(100, CloseThis);
			}
		}

		private void OpenThis()
		{
			EnsureBuilt();
			Relayout();
			Panel.SetActive(value: true);
			GameMenuState.SetMenuOpen(this, open: true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if (Panel.transform is RectTransform layoutRoot)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
			}
		}

		private void CloseThis()
		{
			Panel.SetActive(value: false);
			GameMenuState.SetMenuOpen(this, open: false);
		}

		private void EnsureBuilt()
		{
			if (built)
			{
				return;
			}
			built = true;
			if (closeButton != null)
			{
				closeButton.onClick.AddListener(CloseThis);
			}
			TutorialLessonButton[] componentsInChildren = Panel.GetComponentsInChildren<TutorialLessonButton>(includeInactive: true);
			foreach (TutorialLessonButton tutorialLessonButton in componentsInChildren)
			{
				string lessonId = tutorialLessonButton.LessonId;
				if (!string.IsNullOrEmpty(lessonId) && !rows.ContainsKey(lessonId))
				{
					if (!IsKnownLesson(lessonId))
					{
						Debug.LogWarning("[TutorialLessonMenu] '" + lessonId + "' diye bir ders yok - buton yok sayildi.", tutorialLessonButton);
						continue;
					}
					Button component = tutorialLessonButton.GetComponent<Button>();
					tutorialLessonButton.ApplyLabel();
					Wire(component, lessonId);
					rows[lessonId] = tutorialLessonButton.gameObject;
				}
			}
			List<string> list = new List<string>();
			foreach (string lessonId2 in TutorialCurriculum.LessonIds)
			{
				if (rows.ContainsKey(lessonId2))
				{
					continue;
				}
				if (rowTemplate == null)
				{
					list.Add(lessonId2);
					continue;
				}
				Transform parent = ((rowContainer != null) ? rowContainer : rowTemplate.transform.parent);
				Button button = Object.Instantiate(rowTemplate, parent);
				button.transform.SetAsFirstSibling();
				button.name = "Lesson_" + lessonId2;
				TutorialLessonButton component2 = button.GetComponent<TutorialLessonButton>();
				if (component2 != null)
				{
					Object.Destroy(component2);
				}
				TMP_Text componentInChildren = button.GetComponentInChildren<TMP_Text>(includeInactive: true);
				if (componentInChildren != null)
				{
					LocalizedText.Attach(componentInChildren, "Lesson." + lessonId2 + ".Name");
				}
				Wire(button, lessonId2);
				rows[lessonId2] = button.gameObject;
			}
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
			if (list.Count > 0)
			{
				Debug.LogWarning("[TutorialLessonMenu] Bu derslerin butonu yok ve Row Template bos: " + string.Join(", ", list), this);
			}
		}

		private static bool IsKnownLesson(string id)
		{
			foreach (string lessonId in TutorialCurriculum.LessonIds)
			{
				if (lessonId == id)
				{
					return true;
				}
			}
			return false;
		}

		private void Wire(Button button, string id)
		{
			if (button == null)
			{
				return;
			}
			button.onClick.AddListener(delegate
			{
				CloseThis();
				if (TutorialDirector.Instance != null)
				{
					TutorialDirector.Instance.StartLesson(id);
				}
			});
		}

		private void Relayout()
		{
			foreach (KeyValuePair<string, GameObject> row in rows)
			{
				if (row.Value != null)
				{
					row.Value.SetActive(TutorialCurriculum.IsLessonOffered(row.Key));
				}
			}
		}

		private static TutorialLessonMenu CreateFallback(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "TutorialLessons");
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(320f, 0f);
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0.09f, 0.11f, 0.14f, 0.96f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(20, 20, 14, 14);
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			rectTransform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Title", "", 18);
			textMeshProUGUI.fontStyle = FontStyles.Bold;
			textMeshProUGUI.alignment = TextAlignmentOptions.Center;
			LocalizedText.Attach(textMeshProUGUI, "Lessons.Title");
			textMeshProUGUI.gameObject.AddComponent<LayoutElement>().preferredHeight = 34f;
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "Rows");
			VerticalLayoutGroup verticalLayoutGroup2 = rectTransform2.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.spacing = 8f;
			verticalLayoutGroup2.childControlWidth = true;
			verticalLayoutGroup2.childControlHeight = true;
			verticalLayoutGroup2.childForceExpandWidth = true;
			verticalLayoutGroup2.childForceExpandHeight = false;
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform2, "LessonTemplate", "", out text);
			button.gameObject.AddComponent<LayoutElement>().preferredHeight = 32f;
			TextMeshProUGUI text2;
			Button button2 = UIFactory.CreateButton(rectTransform, "Close", "", out text2);
			LocalizedText.Attach(text2, "Common.Close");
			button2.gameObject.AddComponent<LayoutElement>().preferredHeight = 28f;
			TutorialLessonMenu tutorialLessonMenu = rectTransform.gameObject.AddComponent<TutorialLessonMenu>();
			tutorialLessonMenu.panel = rectTransform.gameObject;
			tutorialLessonMenu.rowContainer = rectTransform2;
			tutorialLessonMenu.rowTemplate = button;
			tutorialLessonMenu.closeButton = button2;
			rectTransform.gameObject.SetActive(value: false);
			return tutorialLessonMenu;
		}
	}
}
