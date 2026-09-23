using System;
using Mimicraft.Customization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class WeaponSkinRowView : MonoBehaviour
	{
		[Tooltip("Skinin görselinin çizileceği Image. İsteğe bağlı - boş bırakırsan satır sadece isimden ibaret olur ve hiçbir PNG çözülmez.")]
		[SerializeField]
		private Image thumbnail;

		[Tooltip("Görsel henüz çekilmemişse açılacak obje - kutu ikonu gibi. İsteğe bağlı.")]
		[SerializeField]
		private GameObject thumbnailPlaceholder;

		[Tooltip("Skinin adının yazılacağı yer.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Bu skini yükleyip silaha giydiren buton.")]
		[SerializeField]
		private Button loadButton;

		[Tooltip("Bu skinin bir kopyasını oluşturup açan buton. İsteğe bağlı.")]
		[SerializeField]
		private Button duplicateButton;

		[Tooltip("Bu skini silen buton.")]
		[SerializeField]
		private Button deleteButton;

		[Tooltip("Bu satır ŞU AN AÇIK olan skini gösteriyorsa açılacak obje - vurgu çerçevesi gibi. İsteğe bağlı ama önerilir: Kaydet o dosyaya yazıyor.")]
		[SerializeField]
		private GameObject openIndicator;

		[Tooltip("Bu skin ESKİ tek-dosya biçiminde kaydedilmişse açılacak obje. İsteğe bağlı - kullanıcının bir kez kaydederek yeni biçime taşıyabileceğini belli etmek için.")]
		[SerializeField]
		private GameObject legacyIndicator;

		public void Bind(string filePath, string skinName, bool isOpen, bool isLegacy, Action<string> onLoad, Action<string> onDuplicate, Action<string> onDelete)
		{
			if (thumbnail != null)
			{
				Sprite sprite = SavedThumbnails.Load(filePath);
				thumbnail.sprite = sprite;
				thumbnail.enabled = sprite != null;
				if (thumbnailPlaceholder != null)
				{
					thumbnailPlaceholder.SetActive(sprite == null);
				}
			}
			if (nameLabel != null)
			{
				nameLabel.text = skinName;
			}
			if (openIndicator != null)
			{
				openIndicator.SetActive(isOpen);
			}
			if (legacyIndicator != null)
			{
				legacyIndicator.SetActive(isLegacy);
			}
			Wire(loadButton, filePath, onLoad);
			Wire(duplicateButton, filePath, onDuplicate);
			Wire(deleteButton, filePath, onDelete);
		}

		private static void Wire(Button button, string filePath, Action<string> action)
		{
			if (!(button == null))
			{
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(delegate
				{
					action?.Invoke(filePath);
				});
			}
		}
	}
}
