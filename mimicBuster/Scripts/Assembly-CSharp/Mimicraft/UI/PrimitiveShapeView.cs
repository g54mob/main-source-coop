using Mimicraft.Analytics;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class PrimitiveShapeView : MonoBehaviour
	{
		private const float CardWidth = 190f;

		private const float RowHeight = 34f;

		private static PrimitiveShapeView instance;

		[Tooltip("Açılıp kapanacak kart. Boş bırakılırsa bu objenin kendisi açılıp kapanır - koddan kurulan pencere burayı kendi doldurur.")]
		[SerializeField]
		private RectTransform content;

		[Tooltip("Bir şekil satırının şablonu: üzerinde Button olan bir obje, içinde bir yazı. Pencere bunun kopyalarını üretir, her şekle bir tane, ve yazılarını kendisi yazar.\n\nKapalı bırak: şablonun kendisi satır olarak görünmez.\n\nSahneye ELLE koyduğun bir pencerede zorunludur - boşsa kart açılır ve içi boş kalır. Koddan kurulan pencere satırlarını kendisi çizer.")]
		[SerializeField]
		private Button rowTemplate;

		[Tooltip("Satırların ekleneceği obje. Boş bırakılırsa kartın kendisi kullanılır.")]
		[SerializeField]
		private RectTransform rowParent;

		[Tooltip("Kartı kapatan buton. İsteğe bağlı.")]
		[SerializeField]
		private Button closeButton;

		private bool rowsReady;

		private bool warnedAboutRows;

		private GameObject Target
		{
			get
			{
				if (!(content != null))
				{
					return base.gameObject;
				}
				return content.gameObject;
			}
		}

		public bool IsOpen => Target.activeSelf;

		public static bool Available
		{
			get
			{
				if (VoxelEditorSettings.TutorialLocksExtras)
				{
					return false;
				}
				if (ModelEditSession.Active == null)
				{
					return PlayerVoxelBody.Local != null;
				}
				return ModelEditSession.Active.SupportsPrimitives;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}

		private void Awake()
		{
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
			if (closeButton != null)
			{
				closeButton.onClick.RemoveListener(Hide);
				closeButton.onClick.AddListener(Hide);
			}
		}

		private void EnsureRows()
		{
			if (rowsReady)
			{
				return;
			}
			if (rowTemplate == null)
			{
				if (!warnedAboutRows)
				{
					warnedAboutRows = true;
					Debug.LogWarning("[PrimitiveShapeView] Row Template bos - kart acilir ama icinde hicbir sekil olmaz. Uzerinde Button olan bir satir objesi ver (kapali birak); pencere her sekil icin birer kopyasini uretir.", this);
				}
				return;
			}
			rowsReady = true;
			Transform parent = ((rowParent != null) ? rowParent : ((content != null) ? content : base.transform));
			VoxelPrimitive[] all = VoxelPrimitives.All;
			for (int i = 0; i < all.Length; i++)
			{
				VoxelPrimitive voxelPrimitive = all[i];
				VoxelPrimitive captured = voxelPrimitive;
				Button button = Object.Instantiate(rowTemplate, parent);
				button.name = voxelPrimitive.ToString();
				button.gameObject.SetActive(value: true);
				TMP_Text componentInChildren = button.GetComponentInChildren<TMP_Text>(includeInactive: true);
				if (componentInChildren != null)
				{
					LocalizedText.Attach(componentInChildren, VoxelPrimitives.NameKey(voxelPrimitive));
				}
				button.onClick.AddListener(delegate
				{
					Pick(captured);
				});
			}
			rowTemplate.transform.SetAsLastSibling();
		}

		public static PrimitiveShapeView Resolve(Transform canvas)
		{
			if (instance != null)
			{
				return instance;
			}
			instance = Object.FindFirstObjectByType<PrimitiveShapeView>(FindObjectsInactive.Include);
			if (instance != null)
			{
				return instance;
			}
			if (canvas == null)
			{
				return null;
			}
			instance = Build(canvas);
			return instance;
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

		public void Show()
		{
			if (!Available)
			{
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get("Primitives.Unavailable"));
				}
			}
			else
			{
				EnsureRows();
				Target.SetActive(value: true);
				Target.transform.SetAsLastSibling();
			}
		}

		public void Hide()
		{
			Target.SetActive(value: false);
		}

		private void Update()
		{
			if (IsOpen && !Available)
			{
				Hide();
			}
		}

		private void Pick(VoxelPrimitive shape)
		{
			Hide();
			IModelEditSession session = ModelEditSession.Active;
			if (session == null && !PlayerVoxelBody.CanLocalReset(report: true))
			{
				return;
			}
			DialogView.Confirm(Loc.Get("Primitives.Confirm"), delegate
			{
				if (session != null)
				{
					session.ApplyPrimitive(shape);
				}
				else
				{
					PlayerVoxelBody.RequestLocalPrimitive(shape);
				}
				Telemetry.Send("primitive_used", ("shape", shape.ToString().ToLowerInvariant()), ("origin", (session != null) ? "menu" : "round"));
			});
		}

		private static PrimitiveShapeView Build(Transform canvas)
		{
			RectTransform rectTransform = UIFactory.CreateRect(canvas, "PrimitiveShapes");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			PrimitiveShapeView view = rectTransform.gameObject.AddComponent<PrimitiveShapeView>();
			Image image = UIFactory.CreatePanel(rectTransform, "Card", new Color(0.12f, 0.13f, 0.15f, 0.96f));
			RectTransform rectTransform2 = (RectTransform)image.transform;
			rectTransform2.anchorMin = new Vector2(0.5f, 0f);
			rectTransform2.anchorMax = new Vector2(0.5f, 0f);
			rectTransform2.pivot = new Vector2(0.5f, 0f);
			rectTransform2.anchoredPosition = new Vector2(0f, 92f);
			rectTransform2.sizeDelta = new Vector2(190f, 0f);
			VerticalLayoutGroup verticalLayoutGroup = image.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 4f;
			verticalLayoutGroup.padding = new RectOffset(8, 8, 8, 8);
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			image.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(image.transform, "Title", "", 16);
			LocalizedText.Attach(textMeshProUGUI, "Primitives.Title");
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(0f, 26f);
			VoxelPrimitive[] all = VoxelPrimitives.All;
			for (int i = 0; i < all.Length; i++)
			{
				VoxelPrimitive voxelPrimitive = all[i];
				VoxelPrimitive captured = voxelPrimitive;
				TextMeshProUGUI text;
				Button button = UIFactory.CreateButton(image.transform, voxelPrimitive.ToString(), "", out text);
				LocalizedText.Attach(text, VoxelPrimitives.NameKey(voxelPrimitive));
				((RectTransform)button.transform).sizeDelta = new Vector2(0f, 34f);
				button.onClick.AddListener(delegate
				{
					view.Pick(captured);
				});
			}
			TextMeshProUGUI text2;
			Button button2 = UIFactory.CreateButton(image.transform, "Close", "", out text2);
			LocalizedText.Attach(text2, "Common.Close");
			((RectTransform)button2.transform).sizeDelta = new Vector2(0f, 34f);
			button2.onClick.AddListener(view.Hide);
			view.content = rectTransform2;
			view.rowsReady = true;
			rectTransform2.gameObject.SetActive(value: false);
			return view;
		}
	}
}
