using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class DuelScoreRowView : MonoBehaviour
	{
		[Tooltip("Sıra numarası - 1, 2, 3. İsteğe bağlı; koymazsan sıra yalnızca satırın yerinden okunur.")]
		[SerializeField]
		private TMP_Text rankLabel;

		[Tooltip("Oyuncunun adı. Bu olmadan satır kimi gösterdiğini söyleyemez.")]
		[SerializeField]
		private TMP_Text nameLabel;

		[Tooltip("Kazanılan 1v1 sayısı.")]
		[SerializeField]
		private TMP_Text winsLabel;

		[Tooltip("Yalnızca bu makinedeki oyuncunun satırında açılacak obje - çerçeve, ok, parlama, ne istersen. İsteğe bağlı.")]
		[SerializeField]
		private GameObject ownMarker;

		[Tooltip("Bu makinedeki oyuncunun satırında rengi değişecek şeyler (yazılar, zemin). Boş bırakabilirsin; dokunulanların özgün rengi hatırlanır ve satır başkasına ait olduğunda geri konur.")]
		[SerializeField]
		private Graphic[] ownTinted = Array.Empty<Graphic>();

		[SerializeField]
		private Color ownColor = new Color(1f, 0.82f, 0.35f);

		[Tooltip("Sıra numarasının sonuna eklenecek şey. Nokta yaygın; boş bırakabilirsin.")]
		[SerializeField]
		private string rankSuffix = ".";

		private Color[] authoredColors;

		public bool IsUsable => nameLabel != null;

		internal void Bind(TMP_Text rank, TMP_Text playerName, TMP_Text wins, Graphic[] tinted, Color own)
		{
			rankLabel = rank;
			nameLabel = playerName;
			winsLabel = wins;
			ownTinted = tinted ?? Array.Empty<Graphic>();
			ownColor = own;
			authoredColors = null;
		}

		public void Apply(int rank, string playerName, int wins, bool own)
		{
			if (rankLabel != null)
			{
				rankLabel.text = rank + rankSuffix;
			}
			if (nameLabel != null)
			{
				nameLabel.text = playerName;
			}
			if (winsLabel != null)
			{
				winsLabel.text = wins.ToString();
			}
			if (ownMarker != null)
			{
				ownMarker.SetActive(own);
			}
			Tint(own);
		}

		private void Tint(bool own)
		{
			if (ownTinted == null || ownTinted.Length == 0)
			{
				return;
			}
			if (authoredColors == null || authoredColors.Length != ownTinted.Length)
			{
				authoredColors = new Color[ownTinted.Length];
				for (int i = 0; i < ownTinted.Length; i++)
				{
					authoredColors[i] = ((ownTinted[i] != null) ? ownTinted[i].color : Color.white);
				}
			}
			for (int j = 0; j < ownTinted.Length; j++)
			{
				if (ownTinted[j] != null)
				{
					ownTinted[j].color = (own ? ownColor : authoredColors[j]);
				}
			}
		}
	}
}
