using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class MimicSaveRowView : MonoBehaviour
	{
		[Tooltip("Modelin adı.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Bu mimic'i açan buton.")]
		[SerializeField]
		private Button loadButton;

		[Tooltip("Bu mimic'i silen buton. İsteğe bağlı.")]
		[SerializeField]
		private Button deleteButton;

		[Tooltip("Kaydedilirken çekilen küçük görsel. İsteğe bağlı.")]
		[SerializeField]
		private Image thumbnail;

		[Tooltip("Görseli HENÜZ OLMAYAN kayıtlarda açılacak yer tutucu. İsteğe bağlı.")]
		[SerializeField]
		private GameObject thumbnailPlaceholder;

		[Tooltip("Bu satır ŞU AN AÇIK olan mimic'i gösteriyorsa açılacak obje - vurgu çerçevesi, onay ikonu, ne istersen. Açık olan aynı zamanda round'a başladığın modeldir.")]
		[SerializeField]
		private GameObject openIndicator;

		private Action load;

		private Action delete;

		private void Awake()
		{
			if (loadButton != null)
			{
				loadButton.onClick.AddListener(delegate
				{
					load?.Invoke();
				});
			}
			if (deleteButton != null)
			{
				deleteButton.onClick.AddListener(delegate
				{
					delete?.Invoke();
				});
			}
		}

		public void Bind(string modelName, Sprite picture, bool isOpen, Action onLoad, Action onDelete)
		{
			load = onLoad;
			delete = onDelete;
			if (nameLabel != null)
			{
				nameLabel.text = modelName;
			}
			if (thumbnail != null)
			{
				thumbnail.sprite = picture;
				thumbnail.enabled = picture != null;
			}
			if (thumbnailPlaceholder != null && thumbnailPlaceholder.activeSelf != (picture == null))
			{
				thumbnailPlaceholder.SetActive(picture == null);
			}
			if (openIndicator != null && openIndicator.activeSelf != isOpen)
			{
				openIndicator.SetActive(isOpen);
			}
			UITooltipTrigger.AttachKey(loadButton, "Tooltip.Mimic.Load");
			UITooltipTrigger.AttachKey(deleteButton, "Tooltip.Mimic.Delete");
			if (loadButton != null)
			{
				loadButton.interactable = !isOpen;
			}
		}
	}
}
