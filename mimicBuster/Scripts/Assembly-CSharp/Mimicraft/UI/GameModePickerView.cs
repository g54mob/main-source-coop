using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using UnityEngine;
using UnityEngine.Events;

namespace Mimicraft.UI
{
	public class GameModePickerView : MonoBehaviour
	{
		private static readonly Color SelectedColor = new Color(0.18f, 0.35f, 0.31f, 0.95f);

		private static readonly Color UnselectedColor = new Color(0.16f, 0.19f, 0.19f, 0.95f);

		private static readonly Color EmptyPreviewColor = new Color(0.32f, 0.34f, 0.38f, 1f);

		[Tooltip("Kartlarin icine dizilecegi GridLayoutGroup'lu obje.")]
		[SerializeField]
		private RectTransform grid;

		[Tooltip("Elle hazirlanmis kart sablonu. Uzerine GameModeCardView ekleyip parcalarini (arka plan, buton, gorsel, ad, aciklama) oradan bagla. Bilesen yoksa ya da bir alan bossa eski isimlerle aranir: 'Preview', 'NameLabel', 'DescriptionLabel'. Pasif birakilmali - klonlari aktif edilir.")]
		[SerializeField]
		private GameObject templateCard;

		private readonly List<PickerCardView> cards = new List<PickerCardView>();

		private readonly List<string> cardModeIds = new List<string>();

		private int selected = -1;

		private bool built;

		public string SelectedModeId
		{
			get
			{
				if (selected < 0 || selected >= cardModeIds.Count)
				{
					return "";
				}
				return cardModeIds[selected];
			}
		}

		public GameModeDefinition SelectedMode => GameModeCatalog.Find(SelectedModeId);

		public event Action<GameModeDefinition> SelectionChanged;

		public bool TrySelectModeId(string modeId)
		{
			if (string.IsNullOrEmpty(modeId))
			{
				return false;
			}
			int num = cardModeIds.IndexOf(modeId);
			if (num < 0)
			{
				return false;
			}
			GameModeDefinition gameModeDefinition = GameModeCatalog.Find(modeId);
			if (gameModeDefinition == null || !gameModeDefinition.Availability.IsPlayable())
			{
				return false;
			}
			Select(num);
			return true;
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
			for (int i = 0; i < cards.Count && i < cardModeIds.Count; i++)
			{
				GameModeDefinition gameModeDefinition = GameModeCatalog.Find(cardModeIds[i]);
				if (gameModeDefinition != null && cards[i] != null)
				{
					ApplyCardText(cards[i], gameModeDefinition);
				}
			}
			RebuildLayout();
		}

		private void EnsureBuilt()
		{
			if (!built && !(grid == null) && !(templateCard == null))
			{
				built = true;
				BuildCards();
			}
		}

		private void RebuildLayout()
		{
			UILayout.RebuildFromDeferred(this, base.transform);
		}

		private void BuildCards()
		{
			foreach (GameModeDefinition item in GameModeCatalog.All)
			{
				if (item.Availability.IsListed())
				{
					int index = cardModeIds.Count;
					cardModeIds.Add(item.ModeId);
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
			for (int i = 0; i < cardModeIds.Count; i++)
			{
				GameModeDefinition gameModeDefinition = GameModeCatalog.Find(cardModeIds[i]);
				if (gameModeDefinition != null && gameModeDefinition.Availability.IsPlayable())
				{
					return i;
				}
			}
			return -1;
		}

		private static PickerCardView CreateCard(GameObject template, GameModeDefinition mode, UnityAction onClick)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(template, template.transform.parent);
			PickerCardView pickerCardView = gameObject.GetComponent<PickerCardView>();
			if (pickerCardView == null)
			{
				pickerCardView = gameObject.AddComponent<GameModeCardView>();
			}
			pickerCardView.ResolveMissing();
			if (pickerCardView.Background != null)
			{
				pickerCardView.Background.color = UnselectedColor;
			}
			bool flag = mode.Availability.IsPlayable();
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
				LockedContentTap.Attach(gameObject, "mode", mode.ModeId, mode.Availability);
			}
			pickerCardView.SetPreview(mode.Preview, EmptyPreviewColor);
			ApplyCardText(pickerCardView, mode);
			gameObject.SetActive(value: true);
			return pickerCardView;
		}

		private static void ApplyCardText(PickerCardView view, GameModeDefinition mode)
		{
			GameObject gameObject = view.gameObject;
			view.SetName(ContentFlare.Apply(gameObject, mode.DisplayName, mode.Availability));
			view.SetDescription(mode.Description);
			UITooltipTrigger uITooltipTrigger = gameObject.GetComponent<UITooltipTrigger>();
			if (uITooltipTrigger == null)
			{
				uITooltipTrigger = gameObject.AddComponent<UITooltipTrigger>();
			}
			uITooltipTrigger.Key = "";
			uITooltipTrigger.Text = mode.Description;
			uITooltipTrigger.enabled = !string.IsNullOrWhiteSpace(mode.Description);
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
			GameModeSelection.Select(SelectedModeId);
			this.SelectionChanged?.Invoke(SelectedMode);
		}
	}
}
