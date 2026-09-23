using System;
using Mimicraft.Customization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class CharacterSaveRowView : MonoBehaviour
	{
		[Tooltip("Karakterin adının yazılacağı yer.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Bu karakteri yükleyen buton.")]
		[SerializeField]
		private Button loadButton;

		[Tooltip("Bu karakteri silen buton.")]
		[SerializeField]
		private Button deleteButton;

		[Tooltip("Karakterin portresi. İsteğe bağlı - atanmazsa satır eskisi gibi yalnızca ad gösterir.")]
		[SerializeField]
		private Image thumbnail;

		[Tooltip("Portresi HENÜZ OLMAYAN karakterlerde açılacak obje - yer tutucu ikon. İsteğe bağlı.")]
		[SerializeField]
		private GameObject thumbnailPlaceholder;

		[Tooltip("Bu karakterin bir kopyasını oluşturup açan buton. İsteğe bağlı.")]
		[SerializeField]
		private Button duplicateButton;

		[Tooltip("Bu satır ŞU AN AÇIK olan karakteri gösteriyorsa açılacak obje - vurgu çerçevesi, işaret gibi. İsteğe bağlı ama önerilir: Kaydet o dosyaya yazıyor.")]
		[SerializeField]
		private GameObject openIndicator;

		public void Bind(string filePath, string characterName, bool isOpen, Action<string, string> onLoad, Action<string, string> onDelete, Action<string, string> onDuplicate)
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
				nameLabel.text = characterName;
			}
			if (openIndicator != null)
			{
				openIndicator.SetActive(isOpen);
			}
			if (loadButton != null)
			{
				loadButton.onClick.RemoveAllListeners();
				loadButton.onClick.AddListener(delegate
				{
					onLoad?.Invoke(filePath, characterName);
				});
			}
			if (deleteButton != null)
			{
				deleteButton.onClick.RemoveAllListeners();
				deleteButton.onClick.AddListener(delegate
				{
					onDelete?.Invoke(filePath, characterName);
				});
			}
			if (duplicateButton != null)
			{
				duplicateButton.onClick.RemoveAllListeners();
				duplicateButton.onClick.AddListener(delegate
				{
					onDuplicate?.Invoke(filePath, characterName);
				});
			}
		}
	}
}
