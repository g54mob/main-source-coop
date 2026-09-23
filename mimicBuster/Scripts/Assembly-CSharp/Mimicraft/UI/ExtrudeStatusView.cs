using System;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ExtrudeStatusView : MonoBehaviour
	{
		private static readonly Color ActiveColor = new Color(0.41960785f, 0.6f, 0.56078434f, 1f);

		private static readonly Color InactiveColor = new Color(0.7372549f, 0.8745098f, 72f / 85f, 1f);

		private static readonly Color OverLimitColor = new Color(1f, 0.3f, 0.3f, 1f);

		private const float RowWidth = 160f;

		[SerializeField]
		private TextMeshProUGUI label;

		[SerializeField]
		private Button extrudeModeButton;

		[SerializeField]
		private Image extrudeModeBackground;

		[SerializeField]
		private Button createModeButton;

		[SerializeField]
		private Image createModeBackground;

		[SerializeField]
		private Button toggleModeButton;

		[SerializeField]
		private TextMeshProUGUI toggleModeLabel;

		[SerializeField]
		private Button toggleSelectionButton;

		[SerializeField]
		private TextMeshProUGUI toggleSelectionLabel;

		[SerializeField]
		private Image toggleSelectionBackground;

		[SerializeField]
		private Button freeSelectButton;

		[SerializeField]
		private Image freeSelectBackground;

		[SerializeField]
		private Button faceSelectButton;

		[SerializeField]
		private Image faceSelectBackground;

		[SerializeField]
		private Image extrudeModeIcon;

		[SerializeField]
		private Image selectionModeIcon;

		[Header("Shape Sub Mode")]
		[Tooltip("Extrude ile Bevel arasında geçiş yapan buton. Bağlanmazsa panel eskisi gibi çalışır, sadece Bevel'e geçilemez.")]
		[SerializeField]
		private Button shapeSubModeButton;

		[SerializeField]
		private Image shapeSubModeIcon;

		[SerializeField]
		private TextMeshProUGUI shapeSubModeLabel;

		[Tooltip("Bevel'in profilini değiştiren buton: düz kesim (Linear) ile yuvarlak fileto (Rounded) arasında. Yalnızca Bevel alt modundayken görünür.")]
		[SerializeField]
		private Button bevelProfileButton;

		[SerializeField]
		private Image bevelProfileIcon;

		[SerializeField]
		private TextMeshProUGUI bevelProfileLabel;

		[Header("Shape Sub Mode Sprites")]
		public Sprite extrudeSubModeSprite;

		public Sprite bevelSubModeSprite;

		public Sprite linearProfileSprite;

		public Sprite roundedProfileSprite;

		[Header("Extrude Mode Sprites")]
		public Sprite extrudeSprite;

		public Sprite addSprite;

		[Header("Selection Mode Sprites")]
		public Sprite freeSelectSprite;

		public Sprite faceSelectSprite;

		private VoxelEditorController boundController;

		private const string ShapeModeCommand = "Editor/ToggleMode";

		private const string SecondChoiceCommand = "Editor/ToggleSubMode";

		private const string ThirdChoiceCommand = "Editor/ToggleExtraMode";

		private bool layoutDirty = true;

		private bool lastBevelling;

		private bool lastReadoutShown;

		private bool authoredCaptured;

		private bool shapeSubModeAuthored;

		private bool bevelProfileAuthored;

		private bool toggleModeAuthored;

		private bool extrudeModeAuthored;

		private bool createModeAuthored;

		private bool toggleSelectionAuthored;

		private bool freeSelectAuthored;

		private bool faceSelectAuthored;

		private static string UnavailableTooltip => "\n<size=88%><color=#E08A6A>" + Loc.Get("Tooltip.Unavailable") + "</color></size>";

		private void AssignShortcutsAndTooltips()
		{
			Advertise(shapeSubModeButton, "Tooltip.ShapeMode", "Editor/ToggleMode");
			Advertise(bevelProfileButton, "Tooltip.BevelProfile", "Editor/ToggleSubMode");
			Advertise(toggleSelectionButton, "Tooltip.SelectionMode", "Editor/ToggleSubMode");
			Advertise(freeSelectButton, "Tooltip.SelectionMode", "Editor/ToggleSubMode");
			Advertise(faceSelectButton, "Tooltip.SelectionMode", "Editor/ToggleSubMode");
			Advertise(toggleModeButton, "Tooltip.GrowOrNew", "Editor/ToggleExtraMode");
			Advertise(extrudeModeButton, "Tooltip.GrowOrNew", "Editor/ToggleExtraMode");
			Advertise(createModeButton, "Tooltip.GrowOrNew", "Editor/ToggleExtraMode");
		}

		private static void Advertise(Button button, string tooltipKey, string command)
		{
			if (!(button == null))
			{
				UITooltipTrigger uITooltipTrigger = button.GetComponent<UITooltipTrigger>();
				if (uITooltipTrigger == null)
				{
					uITooltipTrigger = button.gameObject.AddComponent<UITooltipTrigger>();
				}
				uITooltipTrigger.Key = tooltipKey;
				HotkeyLabel[] componentsInChildren = button.GetComponentsInChildren<HotkeyLabel>(includeInactive: true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].Command = command;
				}
			}
		}

		private void NoteLayout(bool bevelling)
		{
			bool flag = label != null && label.gameObject.activeSelf;
			if (bevelling != lastBevelling || flag != lastReadoutShown)
			{
				lastBevelling = bevelling;
				lastReadoutShown = flag;
				layoutDirty = true;
			}
		}

		private void OnEnable()
		{
			layoutDirty = true;
		}

		private void LateUpdate()
		{
			if (layoutDirty)
			{
				layoutDirty = false;
				RebuildLayoutChain();
			}
		}

		private void RebuildLayoutChain()
		{
			Transform parent = base.transform;
			while (parent != null && (!(parent != base.transform) || !(parent.GetComponent<Canvas>() != null)))
			{
				if (parent is RectTransform layoutRoot && (parent.GetComponent<LayoutGroup>() != null || parent.GetComponent<ContentSizeFitter>() != null))
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
				}
				parent = parent.parent;
			}
		}

		public static ExtrudeStatusView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "ExtrudeStatus");
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			rectTransform.sizeDelta = new Vector2(160f, 100f);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "ModeRow");
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform2.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			rectTransform2.sizeDelta = new Vector2(160f, 32f);
			float x = (160f - horizontalLayoutGroup.spacing) / 2f;
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform2, "ExtrudeModeButton", "Extrude", out text);
			((RectTransform)button.transform).sizeDelta = new Vector2(x, 32f);
			button.gameObject.AddComponent<UITooltipTrigger>().Text = "Extrude - grows this object";
			Button button2 = UIFactory.CreateButton(rectTransform2, "CreateModeButton", "Create", out text);
			((RectTransform)button2.transform).sizeDelta = new Vector2(x, 32f);
			button2.gameObject.AddComponent<UITooltipTrigger>().Text = "Create - splits the new part into its own object";
			RectTransform rectTransform3 = UIFactory.CreateRect(rectTransform, "SelectRow");
			HorizontalLayoutGroup horizontalLayoutGroup2 = rectTransform3.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup2.spacing = 8f;
			horizontalLayoutGroup2.childControlWidth = false;
			horizontalLayoutGroup2.childControlHeight = false;
			horizontalLayoutGroup2.childForceExpandWidth = false;
			horizontalLayoutGroup2.childForceExpandHeight = false;
			rectTransform3.sizeDelta = new Vector2(160f, 32f);
			Button button3 = UIFactory.CreateButton(rectTransform3, "FreeSelectModeButton", "Free Select", out text);
			((RectTransform)button3.transform).sizeDelta = new Vector2(x, 32f);
			button3.gameObject.AddComponent<UITooltipTrigger>().Text = "Free Select - drag a box manually";
			Button button4 = UIFactory.CreateButton(rectTransform3, "FaceModeButton", "Face", out text);
			((RectTransform)button4.transform).sizeDelta = new Vector2(x, 32f);
			button4.gameObject.AddComponent<UITooltipTrigger>().Text = "Face - one click selects the whole connected face";
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "StepsLabel", "+0", 20);
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(160f, 40f);
			ExtrudeStatusView extrudeStatusView = rectTransform.gameObject.AddComponent<ExtrudeStatusView>();
			extrudeStatusView.label = textMeshProUGUI;
			extrudeStatusView.extrudeModeButton = button;
			extrudeStatusView.extrudeModeBackground = button.GetComponent<Image>();
			extrudeStatusView.createModeButton = button2;
			extrudeStatusView.createModeBackground = button2.GetComponent<Image>();
			extrudeStatusView.freeSelectButton = button3;
			extrudeStatusView.freeSelectBackground = button3.GetComponent<Image>();
			extrudeStatusView.faceSelectButton = button4;
			extrudeStatusView.faceSelectBackground = button4.GetComponent<Image>();
			extrudeStatusView.AssignShortcutsAndTooltips();
			extrudeStatusView.authoredCaptured = false;
			extrudeStatusView.CaptureAuthoredButtons();
			return extrudeStatusView;
		}

		private void Awake()
		{
			Wire(extrudeModeButton, delegate
			{
				boundController.ExtrudeMode = ExtrudeMode.Extrude;
			});
			Wire(createModeButton, delegate
			{
				boundController.ExtrudeMode = ExtrudeMode.Create;
			});
			Wire(freeSelectButton, delegate
			{
				boundController.ExtrudeSelectionMode = ExtrudeSelectionMode.FreeSelect;
			});
			Wire(faceSelectButton, delegate
			{
				boundController.ExtrudeSelectionMode = ExtrudeSelectionMode.Face;
			});
			Wire(toggleModeButton, delegate
			{
				boundController.ExtrudeMode = ((boundController.ExtrudeMode != ExtrudeMode.Create) ? ExtrudeMode.Create : ExtrudeMode.Extrude);
			});
			Wire(toggleSelectionButton, delegate
			{
				boundController.ExtrudeSelectionMode = ((boundController.ExtrudeSelectionMode != ExtrudeSelectionMode.Face) ? ExtrudeSelectionMode.Face : ExtrudeSelectionMode.FreeSelect);
			});
			Wire(shapeSubModeButton, delegate
			{
				boundController.ShapeSubMode = ((boundController.ShapeSubMode != ShapeSubMode.Bevel) ? ShapeSubMode.Bevel : ShapeSubMode.Extrude);
			});
			Wire(bevelProfileButton, delegate
			{
				boundController.BevelProfile = ((boundController.BevelProfile != BevelProfile.Rounded) ? BevelProfile.Rounded : BevelProfile.Linear);
			});
			AssignShortcutsAndTooltips();
			CaptureAuthoredButtons();
		}

		private void CaptureAuthoredButtons()
		{
			if (!authoredCaptured)
			{
				authoredCaptured = true;
				shapeSubModeAuthored = IsOn(shapeSubModeButton);
				bevelProfileAuthored = IsOn(bevelProfileButton);
				toggleModeAuthored = IsOn(toggleModeButton);
				extrudeModeAuthored = IsOn(extrudeModeButton);
				createModeAuthored = IsOn(createModeButton);
				toggleSelectionAuthored = IsOn(toggleSelectionButton);
				freeSelectAuthored = IsOn(freeSelectButton);
				faceSelectAuthored = IsOn(faceSelectButton);
			}
		}

		private static bool IsOn(Button button)
		{
			if (button != null)
			{
				return button.gameObject.activeSelf;
			}
			return false;
		}

		public void ReportModeOptions()
		{
			CaptureAuthoredButtons();
			EditorModeOptions.SetOffered(ShapeSubMode.Extrude, shapeSubModeAuthored);
			EditorModeOptions.SetOffered(ShapeSubMode.Bevel, shapeSubModeAuthored);
			EditorModeOptions.SetOffered(BevelProfile.Linear, bevelProfileAuthored);
			EditorModeOptions.SetOffered(BevelProfile.Rounded, bevelProfileAuthored);
			EditorModeOptions.SetOffered(ExtrudeMode.Extrude, toggleModeAuthored || extrudeModeAuthored);
			EditorModeOptions.SetOffered(ExtrudeMode.Create, toggleModeAuthored || createModeAuthored);
			EditorModeOptions.SetOffered(ExtrudeSelectionMode.FreeSelect, toggleSelectionAuthored || freeSelectAuthored);
			EditorModeOptions.SetOffered(ExtrudeSelectionMode.Face, toggleSelectionAuthored || faceSelectAuthored);
		}

		private static void ResolveModes(VoxelEditorController controller)
		{
			ShapeSubMode shapeSubMode = EditorModeOptions.Resolve(controller.ShapeSubMode);
			if (shapeSubMode != controller.ShapeSubMode)
			{
				controller.ShapeSubMode = shapeSubMode;
			}
			BevelProfile bevelProfile = EditorModeOptions.Resolve(controller.BevelProfile);
			if (bevelProfile != controller.BevelProfile)
			{
				controller.BevelProfile = bevelProfile;
			}
			ExtrudeMode extrudeMode = EditorModeOptions.Resolve(controller.ExtrudeMode);
			if (extrudeMode != controller.ExtrudeMode)
			{
				controller.ExtrudeMode = extrudeMode;
			}
			ExtrudeSelectionMode extrudeSelectionMode = EditorModeOptions.Resolve(controller.ExtrudeSelectionMode);
			if (extrudeSelectionMode != controller.ExtrudeSelectionMode)
			{
				controller.ExtrudeSelectionMode = extrudeSelectionMode;
			}
		}

		private void RefreshCreateAvailability()
		{
			bool available = VoxelEditorSettings.IsExtrudeModeAvailable(ExtrudeMode.Create);
			SetAvailable(createModeButton, available);
			SetAvailable(toggleModeButton, available);
		}

		private static void SetAvailable(Button button, bool available)
		{
			if (!(button == null))
			{
				button.interactable = available;
				UITooltipTrigger component = button.GetComponent<UITooltipTrigger>();
				if (component != null)
				{
					component.Suffix = (available ? null : UnavailableTooltip);
				}
			}
		}

		private static void Tint(Image background, bool active)
		{
			if (background != null)
			{
				background.color = (active ? ActiveColor : InactiveColor);
			}
		}

		private static void SetShown(Button button, bool shown)
		{
			if (button != null)
			{
				button.gameObject.SetActive(shown);
			}
		}

		private void Wire(Button button, Action action)
		{
			if (button == null)
			{
				return;
			}
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(delegate
			{
				if (boundController != null)
				{
					action();
				}
			});
		}

		public bool Bind(VoxelEditorController controller)
		{
			ReportModeOptions();
			bool num = Refresh(controller);
			if (num)
			{
				NoteLayout(controller.ShapeSubMode == ShapeSubMode.Bevel);
			}
			return num;
		}

		private bool Refresh(VoxelEditorController controller)
		{
			boundController = controller;
			if (controller == null || controller.State != EditorState.Extrude)
			{
				return false;
			}
			if (!controller.IsFocusLocked)
			{
				ResolveModes(controller);
			}
			RefreshCreateAvailability();
			bool flag = controller.ShapeSubMode == ShapeSubMode.Bevel;
			if (shapeSubModeIcon != null)
			{
				shapeSubModeIcon.sprite = (flag ? bevelSubModeSprite : extrudeSubModeSprite);
			}
			if (shapeSubModeLabel != null)
			{
				shapeSubModeLabel.text = Loc.Get(flag ? "Shape.Bevel" : "Shape.Extrude");
			}
			if (bevelProfileButton != null)
			{
				bevelProfileButton.gameObject.SetActive(flag);
			}
			if (flag)
			{
				bool flag2 = controller.BevelProfile == BevelProfile.Rounded;
				if (bevelProfileIcon != null)
				{
					bevelProfileIcon.sprite = (flag2 ? roundedProfileSprite : linearProfileSprite);
				}
				if (bevelProfileLabel != null)
				{
					bevelProfileLabel.text = Loc.Get(flag2 ? "Shape.Rounded" : "Shape.Linear");
				}
			}
			SetShown(toggleModeButton, !flag);
			SetShown(toggleSelectionButton, !flag);
			SetShown(freeSelectButton, !flag);
			SetShown(faceSelectButton, !flag);
			if (flag)
			{
				bool isBevelDragging = controller.IsBevelDragging;
				label.gameObject.SetActive(isBevelDragging);
				if (isBevelDragging)
				{
					label.text = $"{controller.BevelCurrentRadius} unit";
					label.color = Color.white;
				}
				return true;
			}
			bool flag3 = controller.ExtrudeMode == ExtrudeMode.Create;
			if (extrudeModeIcon != null)
			{
				extrudeModeIcon.sprite = (flag3 ? extrudeSprite : addSprite);
			}
			bool flag4 = controller.ExtrudeSelectionMode == ExtrudeSelectionMode.Face;
			if (selectionModeIcon != null)
			{
				selectionModeIcon.sprite = (flag4 ? faceSelectSprite : freeSelectSprite);
			}
			bool isExtrudeDragging = controller.IsExtrudeDragging;
			label.gameObject.SetActive(isExtrudeDragging);
			if (isExtrudeDragging)
			{
				int extrudeCurrentSteps = controller.ExtrudeCurrentSteps;
				label.text = ((extrudeCurrentSteps >= 0) ? $"+{extrudeCurrentSteps} unit" : $"{extrudeCurrentSteps} unit");
				label.color = (controller.IsExtrudeOverLimit ? OverLimitColor : Color.white);
			}
			return true;
		}
	}
}
