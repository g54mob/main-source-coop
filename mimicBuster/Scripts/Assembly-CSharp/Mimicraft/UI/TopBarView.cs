using Mimicraft.Cameras;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class TopBarView : MonoBehaviour
	{
		[SerializeField]
		private Button loadButton;

		[SerializeField]
		private Button saveButton;

		[SerializeField]
		private Button resetButton;

		[SerializeField]
		private Button movementModeButton;

		[SerializeField]
		private TemplateBrowserView browser;

		[SerializeField]
		private PlayModeManager playModeManager;

		public static TopBarView Create(Transform parent, TemplateBrowserView browser, PlayModeManager playModeManager)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "TopBar");
			rectTransform.anchorMin = new Vector2(0.5f, 1f);
			rectTransform.anchorMax = new Vector2(0.5f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, -16f);
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 8f;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			ContentSizeFitter contentSizeFitter = rectTransform.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform, "LoadButton", "Şablon Yükle", out text);
			((RectTransform)button.transform).sizeDelta = new Vector2(130f, 32f);
			Button button2 = UIFactory.CreateButton(rectTransform, "SaveButton", "Kaydet", out text);
			((RectTransform)button2.transform).sizeDelta = new Vector2(100f, 32f);
			Button button3 = UIFactory.CreateButton(rectTransform, "ResetButton", "Sıfırla", out text);
			((RectTransform)button3.transform).sizeDelta = new Vector2(100f, 32f);
			Button button4 = UIFactory.CreateButton(rectTransform, "MovementModeButton", "Hareket Modu", out text);
			((RectTransform)button4.transform).sizeDelta = new Vector2(130f, 32f);
			TopBarView topBarView = rectTransform.gameObject.AddComponent<TopBarView>();
			topBarView.loadButton = button;
			topBarView.saveButton = button2;
			topBarView.resetButton = button3;
			topBarView.movementModeButton = button4;
			topBarView.browser = browser;
			topBarView.playModeManager = playModeManager;
			return topBarView;
		}

		private void Awake()
		{
			loadButton.onClick.RemoveAllListeners();
			loadButton.onClick.AddListener(delegate
			{
				if (browser != null)
				{
					browser.Show();
				}
			});
			resetButton.onClick.RemoveAllListeners();
			resetButton.onClick.AddListener(delegate
			{
				DialogView.Confirm("Tüm sahne sıfırlanacak ve tüm parçalar silinecek. Emin misiniz?", ResetEverything);
			});
			saveButton.onClick.RemoveAllListeners();
			saveButton.onClick.AddListener(delegate
			{
				if (browser != null)
				{
					browser.SaveCurrent();
				}
				else
				{
					DialogView.Prompt(Loc.Get("Library.SaveName"), VoxelEditorSettings.CurrentTemplateName, delegate(string name)
					{
						if (!TemplateSession.SaveCurrent(name, "") && ToastView.Instance != null)
						{
							ToastView.Instance.Show(Loc.Get("Library.NothingToSave"));
						}
					});
				}
			});
			movementModeButton.onClick.RemoveAllListeners();
			movementModeButton.onClick.AddListener(delegate
			{
				if (playModeManager != null)
				{
					playModeManager.SetMovementMode(active: true);
				}
			});
			if (browser != null)
			{
				browser.SaveNameRequested = () => VoxelEditorSettings.CurrentTemplateName;
				browser.SaveTagRequested = () => VoxelEditorSettings.CurrentTemplateTag;
			}
			WarnAboutMissingReferences();
		}

		private void WarnAboutMissingReferences()
		{
			string text = "";
			if (browser == null)
			{
				text += " Şablon Tarayıcı (Şablon Yükle),";
			}
			if (playModeManager == null)
			{
				text += " Play Mode Manager (Hareket Modu),";
			}
			if (text.Length > 0)
			{
				Debug.LogWarning("[TopBarView] Inspector'da bağlanmamış referanslar - ilgili butonlar devre dışı:" + text.TrimEnd(','), this);
			}
		}

		private void Update()
		{
			bool flag = TemplateSession.SupportsTemplates(VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera));
			saveButton.interactable = flag;
			loadButton.interactable = flag && browser != null;
			movementModeButton.interactable = playModeManager != null;
		}

		private void ResetEverything()
		{
			VoxelEditorController original = VoxelFocusManager.GetOriginal();
			if (original == null)
			{
				return;
			}
			foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
			{
				if (item != original)
				{
					item.gameObject.SetActive(value: false);
				}
			}
			original.gameObject.SetActive(value: true);
			original.transform.position = Vector3.zero;
			original.transform.rotation = Quaternion.identity;
			original.Model.ResetToDefaultCube();
			original.Model.DisplayName = null;
			VoxelFocusManager.SetStickyFocus(original);
			Camera activeCamera = VoxelEditorSettings.ActiveCamera;
			OrbitCamera orbitCamera = ((activeCamera != null) ? activeCamera.GetComponent<OrbitCamera>() : null);
			if (orbitCamera != null)
			{
				orbitCamera.SetFocusPoint(original.transform.TransformPoint(original.Model.GetInitialBoundsCenterLocal()));
			}
			UndoManager.Clear();
			VoxelEditorSettings.CurrentTemplateFilePath = null;
			VoxelEditorSettings.CurrentTemplateName = "";
		}
	}
}
