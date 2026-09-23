using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class MapPickerView : MonoBehaviour
	{
		private const float CardWidth = 148f;

		private const float CardHeight = 106f;

		private const float Spacing = 6f;

		private const int Columns = 2;

		private const float CaptionHeight = 20f;

		private const float CardContentWidth = 136f;

		private static readonly Color SelectedColor = new Color(0.18f, 0.35f, 0.31f, 0.95f);

		private static readonly Color UnselectedColor = new Color(0.16f, 0.19f, 0.19f, 0.95f);

		private static readonly Color EmptyPreviewColor = new Color(0.32f, 0.34f, 0.38f, 1f);

		[SerializeField]
		private RectTransform grid;

		[Tooltip("Elle hazirlanmis kart sablonu. Uzerine MapCardView ekleyip parcalarini (arka plan, buton, gorsel, ad, aciklama) oradan bagla. Bilesen yoksa ya da bir alan bossa eski isimlerle aranir: 'Preview', 'NameLabel', 'DescriptionLabel'. Pasif birakilmali - klonlari aktif edilir.")]
		[SerializeField]
		private GameObject templateCard;

		private readonly List<PickerCardView> cards = new List<PickerCardView>();

		private readonly List<string> cardMapIds = new List<string>();

		private int selected = -1;

		private bool built;

		public string SelectedMapId
		{
			get
			{
				if (selected < 0 || selected >= cardMapIds.Count)
				{
					return "";
				}
				return cardMapIds[selected];
			}
		}

		public event Action<string> SelectionChanged;

		public bool TrySelectMapId(string mapId)
		{
			if (string.IsNullOrEmpty(mapId))
			{
				return false;
			}
			int num = cardMapIds.IndexOf(mapId);
			if (num < 0)
			{
				return false;
			}
			MapScriptableObject mapScriptableObject = MapCatalog.Find(mapId);
			if (mapScriptableObject == null || !mapScriptableObject.Availability.IsPlayable())
			{
				return false;
			}
			Select(num);
			return true;
		}

		public static MapPickerView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "MapPicker");
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 4f;
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Caption", "Harita", 13);
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(296f, 20f);
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "Grid");
			GridLayoutGroup gridLayoutGroup = rectTransform2.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(148f, 106f);
			gridLayoutGroup.spacing = new Vector2(6f, 6f);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = 2;
			MapPickerView mapPickerView = rectTransform.gameObject.AddComponent<MapPickerView>();
			mapPickerView.grid = rectTransform2;
			mapPickerView.EnsureBuilt();
			return mapPickerView;
		}

		private void Awake()
		{
			EnsureBuilt();
		}

		private void OnEnable()
		{
			Loc.Changed += RefreshTexts;
			if (built)
			{
				RefreshTexts();
				RebuildLayout();
			}
		}

		private void OnDisable()
		{
			Loc.Changed -= RefreshTexts;
		}

		private void RefreshTexts()
		{
			for (int i = 0; i < cards.Count && i < cardMapIds.Count; i++)
			{
				MapScriptableObject mapScriptableObject = MapCatalog.Find(cardMapIds[i]);
				if (mapScriptableObject != null && cards[i] != null)
				{
					ApplyCardText(cards[i], mapScriptableObject);
				}
			}
			RebuildLayout();
		}

		private void RebuildLayout()
		{
			UILayout.RebuildFromDeferred(this, base.transform);
		}

		private void EnsureBuilt()
		{
			if (!built && !(grid == null))
			{
				built = true;
				BuildCards();
			}
		}

		private void BuildCards()
		{
			foreach (MapScriptableObject item in MapCatalog.All)
			{
				if (item.Availability.IsListed())
				{
					int index = cardMapIds.Count;
					cardMapIds.Add(item.MapId);
					cards.Add(CreateCard(templateCard, item, delegate
					{
						Select(index);
					}));
				}
			}
			Select(FirstPlayableIndex());
			RebuildLayout();
		}

		private int FirstPlayableIndex()
		{
			for (int i = 0; i < cardMapIds.Count; i++)
			{
				MapScriptableObject mapScriptableObject = MapCatalog.Find(cardMapIds[i]);
				if (mapScriptableObject != null && mapScriptableObject.Availability.IsPlayable())
				{
					return i;
				}
			}
			return -1;
		}

		private static PickerCardView CreateCard(GameObject template, MapScriptableObject map, UnityAction onClick)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(template, template.transform.parent);
			PickerCardView pickerCardView = gameObject.GetComponent<PickerCardView>();
			if (pickerCardView == null)
			{
				pickerCardView = gameObject.AddComponent<MapCardView>();
			}
			pickerCardView.ResolveMissing();
			if (pickerCardView.Background != null)
			{
				pickerCardView.Background.color = UnselectedColor;
			}
			bool flag = map.Availability.IsPlayable();
			if (pickerCardView.Button != null)
			{
				pickerCardView.Button.interactable = flag;
				if (flag)
				{
					pickerCardView.Button.onClick.AddListener(onClick);
				}
			}
			if (!flag)
			{
				LockedContentTap.Attach(gameObject, "map", map.MapId, map.Availability);
			}
			pickerCardView.SetPreview(map.Preview, EmptyPreviewColor);
			ApplyCardText(pickerCardView, map);
			gameObject.SetActive(value: true);
			return pickerCardView;
		}

		private static void ApplyCardText(PickerCardView view, MapScriptableObject map)
		{
			GameObject card = view.gameObject;
			string displayName = (map.DevelopmentOnly ? (map.DisplayName + "  <size=70%><color=#FFB020>[DEV]</color></size>") : map.DisplayName);
			view.SetName(ContentFlare.Apply(card, displayName, map.Availability));
			view.SetDescription(map.Description);
		}

		private void Select(int index)
		{
			selected = index;
			for (int i = 0; i < cards.Count; i++)
			{
				if (cards[i] != null && cards[i].Background != null)
				{
					cards[i].Background.color = ((i == index) ? SelectedColor : UnselectedColor);
				}
			}
			this.SelectionChanged?.Invoke(SelectedMapId);
		}
	}
}
