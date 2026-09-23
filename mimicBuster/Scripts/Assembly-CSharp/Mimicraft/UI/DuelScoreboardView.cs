using System;
using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class DuelScoreboardView : MonoBehaviour
	{
		[Tooltip("Satır tasarımın. Üzerinde DuelScoreRowView olan bir prefab ya da sahnedeki bir obje ver; board bunun kopyalarını üretir. Sahnedeki bir objeyse şablonun kendisi kapatılır.\n\nBoş bırakılırsa satırlar koddan çizilir - çalışır ama tasarımsızdır.")]
		[SerializeField]
		private DuelScoreRowView rowTemplate;

		[Tooltip("Satırların dizileceği obje. Boş bırakılırsa bu objenin kendisi kullanılır. Üzerinde Vertical Layout Group varsa yerleşimi o yapar.")]
		[SerializeField]
		private RectTransform rowParent;

		[Tooltip("Başlık. İsteğe bağlı - metnini bu görünüm yazar ve dil değişince günceller.")]
		[SerializeField]
		private TMP_Text titleLabel;

		[Tooltip("Kimse kazanmamışken gösterilecek yazı. İsteğe bağlı.")]
		[SerializeField]
		private TMP_Text emptyLabel;

		[Tooltip("En fazla kaç satır gösterilecek. Listede daha fazlası varsa en çok kazananlar gösterilir.")]
		[SerializeField]
		[Min(1f)]
		private int maxRows = 8;

		[Header("Şablon yokken çizilen satır")]
		[Tooltip("Yalnızca Row Template boşken kullanılır.")]
		[SerializeField]
		[Min(8f)]
		private float rowHeight = 44f;

		[SerializeField]
		[Min(6f)]
		private float fontSize = 24f;

		[SerializeField]
		private Color rowColor = new Color(1f, 1f, 1f, 0.06f);

		[SerializeField]
		private Color textColor = new Color(0.92f, 0.92f, 0.92f);

		[Tooltip("Bu makinedeki oyuncunun satırının rengi. Şablon kullanıyorsan bu ayar şablonun kendi Own Color'ındadır, burada değil.")]
		[SerializeField]
		private Color ownColor = new Color(1f, 0.82f, 0.35f);

		private RoundManager round;

		private readonly List<DuelScoreRowView> rows = new List<DuelScoreRowView>();

		private readonly List<PlayerScoreEntry> shown = new List<PlayerScoreEntry>();

		private readonly List<PlayerScoreEntry> sorted = new List<PlayerScoreEntry>();

		private int shownNameCount = -1;

		private bool templateHidden;

		private bool warnedAboutTemplate;

		private Transform RowHome
		{
			get
			{
				if (!(rowParent != null))
				{
					return base.transform;
				}
				return rowParent;
			}
		}

		private void OnEnable()
		{
			Loc.Changed += Redraw;
			shownNameCount = -1;
		}

		private void OnDisable()
		{
			Loc.Changed -= Redraw;
		}

		private void Update()
		{
			if (round == null)
			{
				round = GameModeController.Current as RoundManager;
				if (round == null)
				{
					return;
				}
			}
			if (HasChanged())
			{
				Redraw();
			}
		}

		private bool HasChanged()
		{
			if (round.Names.Count != shownNameCount || round.DuelWins.Count != shown.Count)
			{
				return true;
			}
			for (int i = 0; i < shown.Count; i++)
			{
				PlayerScoreEntry playerScoreEntry = round.DuelWins[i];
				if (playerScoreEntry.ClientId != shown[i].ClientId || playerScoreEntry.Score != shown[i].Score)
				{
					return true;
				}
			}
			return false;
		}

		private void Redraw()
		{
			if (round == null)
			{
				return;
			}
			shown.Clear();
			sorted.Clear();
			foreach (PlayerScoreEntry duelWin in round.DuelWins)
			{
				shown.Add(duelWin);
				sorted.Add(duelWin);
			}
			shownNameCount = round.Names.Count;
			sorted.Sort((PlayerScoreEntry a, PlayerScoreEntry b) => (a.Score == b.Score) ? string.Compare(round.GetPlayerName(a.ClientId), round.GetPlayerName(b.ClientId), StringComparison.CurrentCultureIgnoreCase) : b.Score.CompareTo(a.Score));
			if (titleLabel != null)
			{
				titleLabel.text = Loc.Get("Duel.Board.Title");
			}
			if (emptyLabel != null)
			{
				emptyLabel.gameObject.SetActive(sorted.Count == 0);
				if (sorted.Count == 0)
				{
					emptyLabel.text = Loc.Get("Duel.Board.Empty");
				}
			}
			ulong num = ((NetworkManager.Singleton != null) ? NetworkManager.Singleton.LocalClientId : ulong.MaxValue);
			int num2 = Mathf.Min(sorted.Count, maxRows);
			for (int num3 = 0; num3 < num2; num3++)
			{
				DuelScoreRowView duelScoreRowView = RowAt(num3);
				if (duelScoreRowView == null)
				{
					break;
				}
				PlayerScoreEntry playerScoreEntry = sorted[num3];
				duelScoreRowView.gameObject.SetActive(value: true);
				duelScoreRowView.Apply(num3 + 1, round.GetPlayerName(playerScoreEntry.ClientId), playerScoreEntry.Score, playerScoreEntry.ClientId == num);
			}
			for (int num4 = num2; num4 < rows.Count; num4++)
			{
				if (rows[num4] != null)
				{
					rows[num4].gameObject.SetActive(value: false);
				}
			}
		}

		private DuelScoreRowView RowAt(int index)
		{
			while (rows.Count <= index)
			{
				DuelScoreRowView duelScoreRowView = CreateRow(rows.Count);
				if (duelScoreRowView == null)
				{
					return null;
				}
				rows.Add(duelScoreRowView);
			}
			return rows[index];
		}

		private DuelScoreRowView CreateRow(int index)
		{
			if (rowTemplate != null && rowTemplate.IsUsable)
			{
				HideTemplate();
				DuelScoreRowView duelScoreRowView = UnityEngine.Object.Instantiate(rowTemplate, RowHome);
				duelScoreRowView.name = $"Row{index}";
				duelScoreRowView.gameObject.SetActive(value: false);
				return duelScoreRowView;
			}
			if (rowTemplate != null && !warnedAboutTemplate)
			{
				warnedAboutTemplate = true;
				Debug.LogWarning("[DuelScoreboardView] Row Template'te Name Label bos - satirlar koddan cizilecek. Sablonun DuelScoreRowView'inde en azindan Name Label dolu olmali.", this);
			}
			return BuildFallbackRow(index);
		}

		private void HideTemplate()
		{
			if (!templateHidden)
			{
				templateHidden = true;
				if (rowTemplate.gameObject.scene.IsValid())
				{
					rowTemplate.gameObject.SetActive(value: false);
				}
			}
		}

		private DuelScoreRowView BuildFallbackRow(int index)
		{
			Transform rowHome = RowHome;
			Image image = UIFactory.CreatePanel(rowHome, $"Row{index}", rowColor);
			RectTransform rectTransform = (RectTransform)image.transform;
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.sizeDelta = new Vector2(0f, rowHeight);
			if (rowHome.GetComponent<LayoutGroup>() == null)
			{
				rectTransform.anchoredPosition = new Vector2(0f, (float)(-index) * (rowHeight + 4f));
			}
			LayoutElement layoutElement = image.gameObject.AddComponent<LayoutElement>();
			layoutElement.preferredHeight = rowHeight;
			layoutElement.minHeight = rowHeight;
			TMP_Text tMP_Text = UIFactory.CreateLabel(rectTransform, "Rank", "", Mathf.RoundToInt(fontSize));
			tMP_Text.alignment = TextAlignmentOptions.MidlineLeft;
			tMP_Text.color = textColor;
			RectTransform obj = (RectTransform)tMP_Text.transform;
			obj.anchorMin = new Vector2(0f, 0f);
			obj.anchorMax = new Vector2(0f, 1f);
			obj.pivot = new Vector2(0f, 0.5f);
			obj.sizeDelta = new Vector2(52f, 0f);
			obj.anchoredPosition = new Vector2(16f, 0f);
			TMP_Text tMP_Text2 = UIFactory.CreateLabel(rectTransform, "Name", "", Mathf.RoundToInt(fontSize));
			tMP_Text2.alignment = TextAlignmentOptions.MidlineLeft;
			tMP_Text2.color = textColor;
			tMP_Text2.textWrappingMode = TextWrappingModes.NoWrap;
			tMP_Text2.overflowMode = TextOverflowModes.Ellipsis;
			RectTransform obj2 = (RectTransform)tMP_Text2.transform;
			obj2.anchorMin = Vector2.zero;
			obj2.anchorMax = Vector2.one;
			obj2.offsetMin = new Vector2(72f, 0f);
			obj2.offsetMax = new Vector2(-96f, 0f);
			TMP_Text tMP_Text3 = UIFactory.CreateLabel(rectTransform, "Wins", "", Mathf.RoundToInt(fontSize));
			tMP_Text3.alignment = TextAlignmentOptions.MidlineRight;
			tMP_Text3.color = textColor;
			RectTransform obj3 = (RectTransform)tMP_Text3.transform;
			obj3.anchorMin = new Vector2(1f, 0f);
			obj3.anchorMax = new Vector2(1f, 1f);
			obj3.pivot = new Vector2(1f, 0.5f);
			obj3.sizeDelta = new Vector2(80f, 0f);
			obj3.anchoredPosition = new Vector2(-16f, 0f);
			DuelScoreRowView duelScoreRowView = image.gameObject.AddComponent<DuelScoreRowView>();
			duelScoreRowView.Bind(tMP_Text, tMP_Text2, tMP_Text3, new Graphic[3] { tMP_Text, tMP_Text2, tMP_Text3 }, ownColor);
			duelScoreRowView.gameObject.SetActive(value: false);
			return duelScoreRowView;
		}
	}
}
