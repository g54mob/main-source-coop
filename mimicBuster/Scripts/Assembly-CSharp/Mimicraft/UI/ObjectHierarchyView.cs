using System;
using System.Collections.Generic;
using Mimicraft.Cameras;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ObjectHierarchyView : MonoBehaviour
	{
		[Serializable]
		private class RowRefs
		{
			public GameObject Root;

			public TMP_InputField NameField;

			public Button FocusButton;

			public Button DuplicateButton;

			public Button DeleteButton;

			public UITooltipTrigger DeleteTooltip;
		}

		private const int Capacity = 16;

		private const float RowWidth = 260f;

		private const float RowHeight = 32f;

		private const float NameFieldWidth = 120f;

		private const float FocusButtonWidth = 64f;

		private const float IconButtonSize = 28f;

		private const string DeleteTooltipDefault = "Sil";

		private const string DeleteTooltipLastObject = "Son obje silinemez";

		private const string DeleteTooltipTooSmall = "Bu obje silinirse model minimum boyutun altına düşer";

		[SerializeField]
		private List<RowRefs> rows = new List<RowRefs>();

		private readonly List<VoxelEditorController> objects = new List<VoxelEditorController>();

		private readonly List<VoxelBodyPiece> bodyPieces = new List<VoxelBodyPiece>();

		private readonly List<bool> deleteAllowed = new List<bool>();

		public static ObjectHierarchyView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "ObjectHierarchy");
			rectTransform.anchorMin = new Vector2(0f, 0.5f);
			rectTransform.anchorMax = new Vector2(0f, 0.5f);
			rectTransform.pivot = new Vector2(0f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(16f, 0f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 8f;
			verticalLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0.12f, 0.12f, 0.12f, 0.85f);
			ContentSizeFitter contentSizeFitter = rectTransform.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			((RectTransform)UIFactory.CreateLabel(rectTransform, "Title", "Nesneler", 16).transform).sizeDelta = new Vector2(260f, 22f);
			List<RowRefs> list = new List<RowRefs>();
			for (int i = 0; i < 16; i++)
			{
				list.Add(CreateRow(rectTransform, i));
			}
			ObjectHierarchyView objectHierarchyView = rectTransform.gameObject.AddComponent<ObjectHierarchyView>();
			objectHierarchyView.rows = list;
			return objectHierarchyView;
		}

		private static RowRefs CreateRow(Transform parent, int index)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, $"Row{index}");
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.95f);
			HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = 4f;
			horizontalLayoutGroup.padding = new RectOffset(4, 4, 2, 2);
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			rectTransform.sizeDelta = new Vector2(260f, 32f);
			TMP_InputField tMP_InputField = UIFactory.CreateInputField(rectTransform, "NameField", Loc.Get("Name"));
			((RectTransform)tMP_InputField.transform).sizeDelta = new Vector2(120f, 28f);
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform, "FocusButton", Loc.Get("Focus"), out text);
			((RectTransform)button.transform).sizeDelta = new Vector2(64f, 28f);
			button.gameObject.AddComponent<UITooltipTrigger>().Text = "Bu objeye odaklan ve kamerayı getir";
			Sprite iconSprite = PlaceholderIcons.CreateSquare(new Color(0.35f, 0.75f, 0.65f, 1f));
			Image iconImage;
			Button button2 = UIFactory.CreateIconButton(rectTransform, "DuplicateButton", iconSprite, new Vector2(16f, 16f), out iconImage);
			((RectTransform)button2.transform).sizeDelta = new Vector2(28f, 28f);
			button2.gameObject.AddComponent<UITooltipTrigger>().Text = "Kopyala";
			Sprite iconSprite2 = PlaceholderIcons.CreateSquare(new Color(0.9f, 0.3f, 0.3f, 1f));
			Button button3 = UIFactory.CreateIconButton(rectTransform, "DeleteButton", iconSprite2, new Vector2(16f, 16f), out iconImage);
			((RectTransform)button3.transform).sizeDelta = new Vector2(28f, 28f);
			UITooltipTrigger uITooltipTrigger = button3.gameObject.AddComponent<UITooltipTrigger>();
			uITooltipTrigger.Text = "Sil";
			rectTransform.gameObject.SetActive(value: false);
			return new RowRefs
			{
				Root = rectTransform.gameObject,
				NameField = tMP_InputField,
				FocusButton = button,
				DuplicateButton = button2,
				DeleteButton = button3,
				DeleteTooltip = uITooltipTrigger
			};
		}

		private void Awake()
		{
			for (int i = 0; i < rows.Count; i++)
			{
				RowRefs rowRefs = rows[i];
				int index = i;
				rowRefs.NameField.onEndEdit.RemoveAllListeners();
				rowRefs.NameField.onEndEdit.AddListener(delegate(string name)
				{
					if (index < objects.Count)
					{
						string text = name.Trim();
						if (!string.IsNullOrEmpty(text))
						{
							objects[index].Model.DisplayName = text;
						}
					}
				});
				rowRefs.FocusButton.onClick.RemoveAllListeners();
				rowRefs.FocusButton.onClick.AddListener(delegate
				{
					if (index < objects.Count)
					{
						VoxelEditorController voxelEditorController = objects[index];
						VoxelFocusManager.SetStickyFocus(voxelEditorController);
						Camera activeCamera = VoxelEditorSettings.ActiveCamera;
						OrbitCamera orbitCamera = ((activeCamera != null) ? activeCamera.GetComponent<OrbitCamera>() : null);
						if (orbitCamera != null)
						{
							orbitCamera.SetFocusPoint(voxelEditorController.transform.TransformPoint(voxelEditorController.Model.GetCurrentBoundsCenterLocal()));
						}
					}
				});
				rowRefs.DuplicateButton.onClick.RemoveAllListeners();
				rowRefs.DuplicateButton.onClick.AddListener(delegate
				{
					if (index < objects.Count)
					{
						UndoManager.Push(new ToggleObjectActiveCommand(VoxelSplitFactory.CreateDuplicate(objects[index].Model, VoxelEditorSettings.ActiveCamera), activeAfterRedo: true, "Duplicate: " + objects[index].Model.DisplayName));
					}
				});
				rowRefs.DeleteButton.onClick.RemoveAllListeners();
				rowRefs.DeleteButton.onClick.AddListener(delegate
				{
					if (index < objects.Count)
					{
						int num = 0;
						foreach (VoxelEditorController item in VoxelFocusManager.GetAllUsable())
						{
							_ = item;
							num++;
						}
						if (num > 1 && index < deleteAllowed.Count && deleteAllowed[index])
						{
							string displayName = objects[index].Model.DisplayName;
							GameObject obj = objects[index].gameObject;
							obj.SetActive(value: false);
							UndoManager.Push(new ToggleObjectActiveCommand(obj, activeAfterRedo: false, "Delete: " + displayName));
						}
					}
				});
			}
		}

		private void Update()
		{
			Refresh();
		}

		private void Refresh()
		{
			VoxelFocusManager.GatherBody(bodyPieces, objects);
			bool flag = objects.Count > 1;
			RefreshDeleteAllowed(flag);
			for (int i = 0; i < rows.Count; i++)
			{
				bool flag2 = i < objects.Count;
				rows[i].Root.SetActive(flag2);
				if (flag2)
				{
					if (!rows[i].NameField.isFocused)
					{
						rows[i].NameField.text = objects[i].Model.DisplayName;
					}
					rows[i].DeleteButton.interactable = deleteAllowed[i];
					bool flag3 = !VoxelEditorSettings.SinglePieceEditing;
					if (rows[i].DuplicateButton != null && rows[i].DuplicateButton.gameObject.activeSelf != flag3)
					{
						rows[i].DuplicateButton.gameObject.SetActive(flag3);
					}
					if (rows[i].DeleteTooltip != null)
					{
						rows[i].DeleteTooltip.Text = (deleteAllowed[i] ? "Sil" : ((!flag) ? "Son obje silinemez" : "Bu obje silinirse model minimum boyutun altına düşer"));
					}
				}
			}
		}

		private void RefreshDeleteAllowed(bool canDelete)
		{
			deleteAllowed.Clear();
			for (int i = 0; i < objects.Count; i++)
			{
				deleteAllowed.Add(canDelete && VoxelBodyRules.WouldRemainValidWithout(bodyPieces, i));
			}
		}
	}
}
