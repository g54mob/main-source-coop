using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class WordCategoryPickerView : MonoBehaviour
	{
		private static readonly Color SelectedColor = new Color(0.18f, 0.35f, 0.31f, 0.95f);

		private static readonly Color UnselectedColor = new Color(0.16f, 0.19f, 0.19f, 0.95f);

		private static readonly Color EmptyPreviewColor = new Color(0.32f, 0.34f, 0.38f, 1f);

		[Tooltip("Kartların dizileceği GridLayoutGroup'lu obje.")]
		[SerializeField]
		private RectTransform grid;

		[Tooltip("Elle hazırlanmış kart şablonu. İç yapısı: 'Preview' (Image) ve 'NameLabel' (TextMeshProUGUI). PASİF bırakılmalı - klonları aktif edilir.")]
		[SerializeField]
		private GameObject templateCard;

		private readonly List<Image> cardBackgrounds = new List<Image>();

		private readonly List<string> cardCategoryIds = new List<string>();

		private int selected = -1;

		private bool built;

		private string language = "";

		public string SelectedCategoryId
		{
			get
			{
				if (selected < 0 || selected >= cardCategoryIds.Count)
				{
					return "";
				}
				return cardCategoryIds[selected];
			}
		}

		public event Action<string> SelectionChanged;

		private void Awake()
		{
			EnsureBuilt();
		}

		private void OnEnable()
		{
			if (built)
			{
				RebuildLayout();
			}
		}

		private void EnsureBuilt()
		{
			if (!built && !(grid == null) && !(templateCard == null))
			{
				built = true;
				BuildCards();
			}
		}

		private void BuildCards()
		{
			IReadOnlyList<WordCategory> readOnlyList2;
			if (!string.IsNullOrEmpty(language))
			{
				IReadOnlyList<WordCategory> readOnlyList = WordCategoryCatalog.For(language);
				readOnlyList2 = readOnlyList;
			}
			else
			{
				readOnlyList2 = WordCategoryCatalog.All;
			}
			IReadOnlyList<WordCategory> readOnlyList3 = readOnlyList2;
			for (int i = 0; i < readOnlyList3.Count; i++)
			{
				int index = i;
				cardCategoryIds.Add(readOnlyList3[i].CategoryId);
				cardBackgrounds.Add(CreateCard(templateCard, readOnlyList3[i], delegate
				{
					Select(index, notify: true);
				}));
			}
			Select((readOnlyList3.Count <= 0) ? (-1) : 0, notify: false);
			RebuildLayout();
		}

		public void SetLanguage(string value)
		{
			if (!(language == value))
			{
				language = value ?? "";
				string selectedCategoryId = SelectedCategoryId;
				ClearCards();
				BuildCards();
				if (!string.IsNullOrEmpty(selectedCategoryId))
				{
					SelectById(selectedCategoryId);
				}
			}
		}

		private void ClearCards()
		{
			foreach (Image cardBackground in cardBackgrounds)
			{
				if (cardBackground != null)
				{
					UnityEngine.Object.Destroy(cardBackground.gameObject);
				}
			}
			cardBackgrounds.Clear();
			cardCategoryIds.Clear();
			selected = -1;
		}

		private static Image CreateCard(GameObject template, WordCategory category, UnityAction onClick)
		{
			GameObject obj = UnityEngine.Object.Instantiate(template, template.transform.parent);
			Image component = obj.GetComponent<Image>();
			component.color = UnselectedColor;
			obj.GetComponent<Button>().onClick.AddListener(onClick);
			Transform transform = obj.transform.Find("Preview");
			if (transform != null)
			{
				Image component2 = transform.GetComponent<Image>();
				if (category.Preview != null)
				{
					component2.sprite = category.Preview;
					component2.preserveAspect = true;
				}
				else
				{
					component2.color = EmptyPreviewColor;
				}
			}
			Transform transform2 = obj.transform.Find("NameLabel");
			if (transform2 != null)
			{
				transform2.GetComponent<TextMeshProUGUI>().text = category.DisplayName;
			}
			obj.SetActive(value: true);
			return component;
		}

		public void SelectById(string categoryId)
		{
			int num = cardCategoryIds.IndexOf(categoryId);
			if (num >= 0 && num != selected)
			{
				Select(num, notify: false);
			}
		}

		private void Select(int index, bool notify)
		{
			selected = index;
			for (int i = 0; i < cardBackgrounds.Count; i++)
			{
				cardBackgrounds[i].color = ((i == index) ? SelectedColor : UnselectedColor);
			}
			if (notify)
			{
				this.SelectionChanged?.Invoke(SelectedCategoryId);
			}
		}

		private void RebuildLayout()
		{
			UILayout.RebuildFromDeferred(this, base.transform);
		}
	}
}
