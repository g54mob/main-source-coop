using Mimicraft.VoxelEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class TransformPanelView : MonoBehaviour
	{
		private const float PanelWidth = 160f;

		[SerializeField]
		private Button modeButton;

		[SerializeField]
		private TextMeshProUGUI modeButtonText;

		[Tooltip("Move/Rotate butonunun ikonu. Taşı modunda Move Sprite, Döndür modunda Rotate Sprite gösterir.")]
		[SerializeField]
		private Image modeIcon;

		[SerializeField]
		private Button spaceButton;

		[SerializeField]
		private Image spaceIcon;

		[SerializeField]
		private Slider snapMoveSlider;

		[SerializeField]
		private Slider snapRotateSlider;

		[SerializeField]
		private TextMeshProUGUI snapMoveSliderValueText;

		[SerializeField]
		private TextMeshProUGUI snapRotateSliderValueText;

		[Header("Retired - hidden on sight, see class doc")]
		[SerializeField]
		private Button snapMoveButton;

		[SerializeField]
		private Button snapRotateButton;

		[Header("Space Sprites")]
		public Sprite globalSpaceSprite;

		public Sprite localSpaceSprite;

		[Header("Mode Sprites")]
		public Sprite moveSprite;

		public Sprite rotateSprite;

		private VoxelEditorController boundController;

		private bool suppressCallback;

		public static TransformPanelView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "TransformPanel");
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			TextMeshProUGUI label;
			Button button = BuildModeButton(rectTransform, out label);
			TextMeshProUGUI text;
			Button button2 = UIFactory.CreateButton(rectTransform, "SpaceButton", "Local", out text);
			((RectTransform)button2.transform).sizeDelta = new Vector2(160f, 28f);
			Slider slider = UIFactory.CreateSlider(rectTransform, "SnapMoveSlider", 0.1f, 5f, 1f);
			((RectTransform)slider.transform).sizeDelta = new Vector2(160f, 20f);
			Slider slider2 = UIFactory.CreateSlider(rectTransform, "SnapRotateSlider", 1f, 90f, 15f);
			((RectTransform)slider2.transform).sizeDelta = new Vector2(160f, 20f);
			TransformPanelView transformPanelView = rectTransform.gameObject.AddComponent<TransformPanelView>();
			transformPanelView.modeButton = button;
			transformPanelView.modeButtonText = label;
			transformPanelView.spaceButton = button2;
			transformPanelView.snapMoveSlider = slider;
			transformPanelView.snapRotateSlider = slider2;
			return transformPanelView;
		}

		public void EnsureModeButton()
		{
			if (!(modeButton != null))
			{
				RectTransform parent = (RectTransform)base.transform;
				modeButton = BuildModeButton(parent, out modeButtonText);
				modeButton.transform.SetSiblingIndex(0);
				BindModeButton();
			}
		}

		private static Button BuildModeButton(RectTransform parent, out TextMeshProUGUI label)
		{
			Button button = UIFactory.CreateButton(parent, "TransformModeButton", "Taşı (F)", out label);
			((RectTransform)button.transform).sizeDelta = new Vector2(160f, 28f);
			return button;
		}

		private void Awake()
		{
			spaceButton.onClick.RemoveAllListeners();
			spaceButton.onClick.AddListener(delegate
			{
				if (boundController != null)
				{
					boundController.UseGlobalSpace = !boundController.UseGlobalSpace;
				}
			});
			BindModeButton();
			snapMoveSlider.onValueChanged.RemoveAllListeners();
			snapMoveSlider.onValueChanged.AddListener(delegate(float v)
			{
				if (!suppressCallback && boundController != null)
				{
					boundController.SnapMoveIncrement = v;
				}
			});
			snapRotateSlider.onValueChanged.RemoveAllListeners();
			snapRotateSlider.onValueChanged.AddListener(delegate(float v)
			{
				if (!suppressCallback && boundController != null)
				{
					boundController.SnapRotateIncrement = v;
				}
			});
		}

		private void BindModeButton()
		{
			if (modeButton == null)
			{
				return;
			}
			modeButton.onClick.RemoveAllListeners();
			modeButton.onClick.AddListener(delegate
			{
				if (!(boundController == null))
				{
					boundController.TransformMode = ((boundController.TransformMode == TransformMode.Move) ? TransformMode.Rotate : TransformMode.Move);
				}
			});
		}

		public void ReportModeOptions()
		{
			bool offered = modeButton != null && modeButton.gameObject.activeSelf;
			EditorModeOptions.SetOffered(TransformMode.Move, offered);
			EditorModeOptions.SetOffered(TransformMode.Rotate, offered);
			EditorModeOptions.SetSwitchOffered("Transform.AxisSpace", spaceButton != null && spaceButton.gameObject.activeSelf);
		}

		public void Bind(VoxelEditorController controller)
		{
			boundController = controller;
			if (!(controller == null))
			{
				ReportModeOptions();
				if (snapMoveButton != null && snapMoveButton.gameObject.activeSelf)
				{
					snapMoveButton.gameObject.SetActive(value: false);
				}
				if (snapRotateButton != null && snapRotateButton.gameObject.activeSelf)
				{
					snapRotateButton.gameObject.SetActive(value: false);
				}
				bool flag = controller.TransformMode == TransformMode.Rotate;
				if (modeButtonText != null)
				{
					modeButtonText.text = (flag ? "Döndür (F)" : "Taşı (F)");
				}
				if (modeButton != null)
				{
					modeButton.image.color = (flag ? ToolbarView.ActiveColor : ToolbarView.InactiveColor);
				}
				Sprite sprite = (flag ? rotateSprite : moveSprite);
				if (modeIcon != null && sprite != null && modeIcon.sprite != sprite)
				{
					modeIcon.sprite = sprite;
				}
				spaceButton.interactable = !flag;
				if (spaceIcon != null)
				{
					spaceIcon.sprite = (controller.UseGlobalSpace ? globalSpaceSprite : localSpaceSprite);
				}
				if (snapMoveSliderValueText != null)
				{
					snapMoveSliderValueText.text = controller.SnapMoveIncrement.ToString("0.##");
				}
				if (snapRotateSliderValueText != null)
				{
					snapRotateSliderValueText.text = controller.SnapRotateIncrement.ToString("0.##");
				}
				suppressCallback = true;
				snapMoveSlider.value = controller.SnapMoveIncrement;
				snapRotateSlider.value = controller.SnapRotateIncrement;
				suppressCallback = false;
			}
		}
	}
}
