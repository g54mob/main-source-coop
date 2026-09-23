using System.Collections.Generic;
using Mimicraft.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class LobbyInfoView : MonoBehaviour
	{
		private class Row
		{
			public GameObject Root;

			public TextMeshProUGUI Caption;

			public TextMeshProUGUI Value;
		}

		private const float CardWidth = 240f;

		private const float CardPadding = 12f;

		private const float ContentWidth = 216f;

		private const float RowHeight = 18f;

		private const float RefreshInterval = 0.5f;

		[SerializeField]
		private TextMeshProUGUI titleLabel;

		[SerializeField]
		private TextMeshProUGUI playerCountValue;

		[Tooltip("Mod ayarı satırlarının ekleneceği yer. Boş bırakılırsa kartın kendisi kullanılır - satırlar mevcut dikey düzenin sonuna eklenir.")]
		[SerializeField]
		private RectTransform rowsRoot;

		[SerializeField]
		private Color rowColor = Color.white;

		[SerializeField]
		[HideInInspector]
		private TextMeshProUGUI mapValue;

		[SerializeField]
		[HideInInspector]
		private TextMeshProUGUI modeValue;

		[SerializeField]
		[HideInInspector]
		private TextMeshProUGUI prepValue;

		[SerializeField]
		[HideInInspector]
		private TextMeshProUGUI huntValue;

		[SerializeField]
		[HideInInspector]
		private TextMeshProUGUI roundEndValue;

		[SerializeField]
		[HideInInspector]
		private TextMeshProUGUI minHidersValue;

		[SerializeField]
		[HideInInspector]
		private TextMeshProUGUI minHuntersValue;

		private readonly List<Row> rows = new List<Row>();

		private readonly List<GameModeController.LobbyInfoRow> pending = new List<GameModeController.LobbyInfoRow>();

		private LobbySettingsData settings;

		private bool hasSettings;

		private float nextRefresh;

		public static LobbyInfoView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "LobbyInfo");
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchoredPosition = new Vector2(24f, -24f);
			rectTransform.sizeDelta = new Vector2(240f, 190f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
			image.raycastTarget = false;
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 4f;
			verticalLayoutGroup.padding = new RectOffset(12, 12, 12, 12);
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Title", "Lobi", 16);
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.fontStyle = FontStyles.Bold;
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(216f, 22f);
			GameObject rowObject;
			TextMeshProUGUI captionLabel;
			TextMeshProUGUI textMeshProUGUI2 = CreateRow(rectTransform, "PlayerCountRow", "Bağlı oyuncu", out rowObject, out captionLabel, Color.white);
			CreateSeparator(rectTransform);
			LobbyInfoView lobbyInfoView = rectTransform.gameObject.AddComponent<LobbyInfoView>();
			lobbyInfoView.titleLabel = textMeshProUGUI;
			lobbyInfoView.playerCountValue = textMeshProUGUI2;
			return lobbyInfoView;
		}

		private static TextMeshProUGUI CreateRow(Transform parent, string name, string caption, out GameObject rowObject, out TextMeshProUGUI captionLabel, Color forecolor)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, name);
			rectTransform.sizeDelta = new Vector2(216f, 18f);
			rowObject = rectTransform.gameObject;
			captionLabel = UIFactory.CreateLabel(rectTransform, "Caption", caption, 13);
			captionLabel.alignment = TextAlignmentOptions.MidlineLeft;
			captionLabel.color = forecolor;
			Stretch((RectTransform)captionLabel.transform);
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "Value", "-", 13);
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineRight;
			Stretch((RectTransform)textMeshProUGUI.transform);
			return textMeshProUGUI;
		}

		private static void CreateSeparator(Transform parent)
		{
			Image image = UIFactory.CreatePanel(parent, "Separator", new Color(1f, 1f, 1f, 0.15f));
			image.raycastTarget = false;
			((RectTransform)image.transform).sizeDelta = new Vector2(216f, 1f);
		}

		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		private void Awake()
		{
			RetireLegacyRows();
			EnsureCardResizes();
		}

		private void RetireLegacyRows()
		{
			Retire(mapValue);
			Retire(modeValue);
			Retire(prepValue);
			Retire(huntValue);
			Retire(roundEndValue);
			Retire(minHidersValue);
			Retire(minHuntersValue);
			mapValue = (modeValue = (prepValue = (huntValue = null)));
			roundEndValue = (minHidersValue = (minHuntersValue = null));
		}

		private static void Retire(TextMeshProUGUI value)
		{
			if (!(value == null))
			{
				Transform parent = value.transform.parent;
				((parent != null) ? parent.gameObject : value.gameObject).SetActive(value: false);
			}
		}

		private void EnsureCardResizes()
		{
			if (!(GetComponent<LayoutGroup>() == null) && !(GetComponent<ContentSizeFitter>() != null))
			{
				ContentSizeFitter contentSizeFitter = base.gameObject.AddComponent<ContentSizeFitter>();
				contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
				contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			}
		}

		private void Update()
		{
			if (!(Time.unscaledTime < nextRefresh))
			{
				nextRefresh = Time.unscaledTime + 0.5f;
				Rebuild();
			}
		}

		public void SetSettings(LobbySettingsData settings)
		{
			this.settings = settings;
			hasSettings = true;
			Rebuild();
		}

		public void SetConnectedCount(int count)
		{
			playerCountValue.text = count.ToString();
		}

		private void Rebuild()
		{
			if (hasSettings)
			{
				string text = settings.LobbyName.ToString();
				titleLabel.text = (string.IsNullOrEmpty(text) ? "Lobi" : text);
				pending.Clear();
				GameModeController current = GameModeController.Current;
				if (current != null)
				{
					current.FillLobbyInfo(settings, pending);
				}
				else
				{
					GameModeController.FillSharedLobbyInfo(settings, pending);
				}
				ApplyRows();
			}
		}

		private void ApplyRows()
		{
			while (rows.Count < pending.Count)
			{
				rows.Add(BuildRow(rows.Count));
			}
			for (int i = 0; i < rows.Count; i++)
			{
				Row row = rows[i];
				bool flag = i < pending.Count;
				if (row.Root.activeSelf != flag)
				{
					row.Root.SetActive(flag);
				}
				if (flag)
				{
					GameModeController.LobbyInfoRow lobbyInfoRow = pending[i];
					if (row.Caption.text != lobbyInfoRow.Caption)
					{
						row.Caption.text = lobbyInfoRow.Caption;
					}
					if (row.Value.text != lobbyInfoRow.Value)
					{
						row.Value.text = lobbyInfoRow.Value;
					}
				}
			}
		}

		private Row BuildRow(int index)
		{
			GameObject rowObject;
			TextMeshProUGUI captionLabel;
			TextMeshProUGUI value = CreateRow((rowsRoot != null) ? rowsRoot : base.transform, $"SettingRow{index}", "", out rowObject, out captionLabel, rowColor);
			return new Row
			{
				Root = rowObject,
				Caption = captionLabel,
				Value = value
			};
		}
	}
}
