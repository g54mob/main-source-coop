using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class EditHistoryView : MonoBehaviour
	{
		private const int Capacity = 20;

		private const float RowWidth = 220f;

		private const float RowHeight = 20f;

		[SerializeField]
		private List<TextMeshProUGUI> rows = new List<TextMeshProUGUI>();

		private CanvasGroup group;

		public bool IsOpen
		{
			get
			{
				return VoxelEditorSettings.ShowHistory;
			}
			set
			{
				VoxelEditorSettings.ShowHistory = value;
			}
		}

		public void Toggle()
		{
			IsOpen = !IsOpen;
		}

		public static EditHistoryView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "EditHistory");
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(0f, 0f);
			rectTransform.pivot = new Vector2(0f, 0f);
			rectTransform.anchoredPosition = new Vector2(16f, 16f);
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 2f;
			verticalLayoutGroup.padding = new RectOffset(10, 10, 10, 10);
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			rectTransform.gameObject.AddComponent<Image>().color = new Color(0.12f, 0.12f, 0.12f, 0.85f);
			ContentSizeFitter contentSizeFitter = rectTransform.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Title", "", 16);
			LocalizedText.Attach(textMeshProUGUI, "History");
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(220f, 22f);
			List<TextMeshProUGUI> list = new List<TextMeshProUGUI>();
			for (int i = 0; i < 20; i++)
			{
				TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform, $"Row{i}", "", 12);
				textMeshProUGUI2.alignment = TextAlignmentOptions.MidlineLeft;
				((RectTransform)textMeshProUGUI2.transform).sizeDelta = new Vector2(220f, 20f);
				textMeshProUGUI2.gameObject.SetActive(value: false);
				list.Add(textMeshProUGUI2);
			}
			EditHistoryView editHistoryView = rectTransform.gameObject.AddComponent<EditHistoryView>();
			editHistoryView.rows = list;
			return editHistoryView;
		}

		private void Awake()
		{
			group = GetComponent<CanvasGroup>();
			if (group == null)
			{
				group = base.gameObject.AddComponent<CanvasGroup>();
			}
		}

		private void Update()
		{
			bool isOpen = IsOpen;
			if (group != null)
			{
				group.alpha = (isOpen ? 1f : 0f);
				group.blocksRaycasts = isOpen;
				group.interactable = isOpen;
			}
			if (isOpen)
			{
				Refresh();
			}
		}

		private void Refresh()
		{
			IReadOnlyList<IUndoableCommand> undoStack = UndoManager.UndoStack;
			int num = 0;
			int num2 = undoStack.Count - 1;
			while (num2 >= 0 && num < rows.Count)
			{
				string description = undoStack[num2].Description;
				if (!string.IsNullOrEmpty(description))
				{
					rows[num].gameObject.SetActive(value: true);
					rows[num].text = description;
					num++;
				}
				num2--;
			}
			for (int i = num; i < rows.Count; i++)
			{
				rows[i].gameObject.SetActive(value: false);
			}
		}
	}
}
