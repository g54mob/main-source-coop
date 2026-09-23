using System.Text;
using Mimicraft.Networking;
using TMPro;
using UnityEngine;

namespace Mimicraft.UI
{
	public class GarticWordDisplayView : MonoBehaviour
	{
		[Tooltip("Kelimenin/ipucunun yazılacağı etiket.")]
		[SerializeField]
		private TextMeshProUGUI label;

		[Tooltip("Çizen kişiye kendi kelimesi gösterilirken kullanılan renk.")]
		[SerializeField]
		private Color ownWordColor = new Color(0.55f, 0.9f, 0.55f);

		[Tooltip("Tahmin edenlere maskelenmiş ipucu gösterilirken kullanılan renk.")]
		[SerializeField]
		private Color hintColor = new Color(0.85f, 0.85f, 0.85f);

		[Tooltip("Açılmış harflerin rengi - hem tahmin edenlerin gördüğü ipucunda, hem çizenin kendi kelimesinde.\n\nÇizen için de renklendiriliyor, çünkü hangi harfleri verdiğini görmesi gereken kişi o: buton kaç harf kaldığını söylüyor, bu hangileri olduğunu söylüyor.")]
		[SerializeField]
		private Color revealedColor = new Color(1f, 0.82f, 0.35f);

		private string ownWord = "";

		private string appliedSource = "";

		private string appliedText = "";

		private bool appliedWasOwnWord;

		public static GarticWordDisplayView Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
			if (label != null)
			{
				label.text = "";
			}
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void SetWord(string word)
		{
			ownWord = word ?? "";
		}

		private string ColorMask(string mask)
		{
			if (string.IsNullOrEmpty(mask))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder(mask.Length * 2);
			string value = "<color=#" + ColorUtility.ToHtmlStringRGB(revealedColor) + ">";
			foreach (char c in mask)
			{
				if (c == '_' || c == ' ')
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append(value).Append(c).Append("</color>");
				}
			}
			return stringBuilder.ToString();
		}

		private string ColorOwnWord(string word, string mask)
		{
			if (string.IsNullOrEmpty(word))
			{
				return "";
			}
			if (mask.Length != Mathf.Max(0, word.Length * 2 - 1))
			{
				return word;
			}
			StringBuilder stringBuilder = new StringBuilder(word.Length * 2);
			string value = "<color=#" + ColorUtility.ToHtmlStringRGB(revealedColor) + ">";
			for (int i = 0; i < word.Length; i++)
			{
				char c = word[i];
				if (c != ' ' && mask[i * 2] != '_')
				{
					stringBuilder.Append(value).Append(c).Append("</color>");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private void Update()
		{
			if (label == null)
			{
				return;
			}
			if (!(GameModeController.Current is GarticRoundManager garticRoundManager))
			{
				label.text = "";
				return;
			}
			string text = garticRoundManager.MaskedWord.Value.ToString();
			bool flag = !string.IsNullOrEmpty(ownWord);
			string text2 = (flag ? ownWord : text);
			if (appliedWasOwnWord == flag && appliedSource == text2)
			{
				if (label.text != appliedText)
				{
					label.text = appliedText;
				}
			}
			else
			{
				appliedWasOwnWord = flag;
				appliedSource = text2;
				label.color = (flag ? ownWordColor : hintColor);
				appliedText = (flag ? ColorOwnWord(ownWord, text) : ColorMask(text));
				label.text = appliedText;
			}
		}
	}
}
