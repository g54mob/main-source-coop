using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public abstract class PickerCardView : MonoBehaviour
	{
		[Tooltip("Seçili / seçili değil rengini alan arka plan. Boşsa kartın kökündeki Image.")]
		[SerializeField]
		private Image background;

		[Tooltip("Kartı seçen buton. Boşsa kartın kökündeki Button. Oynanamayan içerikte tıklanamaz yapılır.")]
		[SerializeField]
		private Button button;

		[Tooltip("Görselin basılacağı Image. Boşsa 'Preview' adlı çocuk aranır.")]
		[SerializeField]
		private Image preview;

		[Tooltip("Görsel YOKKEN açılacak obje - bir ikon ya da 'görsel yok' yazısı. İsteğe bağlı: boşsa görsel alanı griye boyanır.")]
		[SerializeField]
		private GameObject previewPlaceholder;

		[Tooltip("Adın yazılacağı metin. Boşsa 'NameLabel' adlı çocuk aranır. Rich Text açık kalmalı - yakında / DEV rozetleri etiketle yazılıyor.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Açıklamanın yazılacağı metin. İsteğe bağlı; boşsa 'DescriptionLabel' adlı çocuk aranır, o da yoksa kart açıklamasız listelenir.")]
		[SerializeField]
		private TextMeshProUGUI descriptionLabel;

		[SerializeField]
		private bool preserveAspect = true;

		public Image Background => background;

		public Button Button => button;

		public Image Preview => preview;

		public TextMeshProUGUI NameLabel => nameLabel;

		public TextMeshProUGUI DescriptionLabel => descriptionLabel;

		public void ResolveMissing()
		{
			if (background == null)
			{
				background = GetComponent<Image>();
			}
			if (button == null)
			{
				button = GetComponent<Button>();
			}
			if (preview == null)
			{
				preview = FindChild<Image>("Preview");
			}
			if (nameLabel == null)
			{
				nameLabel = FindChild<TextMeshProUGUI>("NameLabel");
			}
			if (descriptionLabel == null)
			{
				descriptionLabel = FindChild<TextMeshProUGUI>("DescriptionLabel");
			}
		}

		public void SetPreview(Sprite sprite, Color emptyColor)
		{
			bool flag = sprite != null;
			if (preview != null)
			{
				if (flag)
				{
					preview.enabled = true;
					preview.sprite = sprite;
					preview.preserveAspect = preserveAspect;
				}
				else if (previewPlaceholder != null)
				{
					preview.enabled = false;
				}
				else
				{
					preview.color = emptyColor;
				}
			}
			if (previewPlaceholder != null && previewPlaceholder.activeSelf == flag)
			{
				previewPlaceholder.SetActive(!flag);
			}
		}

		public void SetName(string text)
		{
			if (nameLabel != null)
			{
				nameLabel.text = text;
			}
		}

		public void SetDescription(string text)
		{
			if (descriptionLabel != null)
			{
				descriptionLabel.text = text;
			}
		}

		private T FindChild<T>(string childName) where T : Component
		{
			Transform transform = base.transform.Find(childName);
			if (!(transform != null))
			{
				return null;
			}
			return transform.GetComponent<T>();
		}
	}
}
