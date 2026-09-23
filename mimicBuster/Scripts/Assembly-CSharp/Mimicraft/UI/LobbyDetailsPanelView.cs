using System;
using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class LobbyDetailsPanelView : MonoBehaviour
	{
		[Tooltip("Açılıp kapanan panel. Boş bırakılırsa bu objenin kendisi kullanılır.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Lobinin adı - panelin başlığı.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Haritanın görseli. Bu istemcinin katalogunda olmayan bir harita için boş kalır.")]
		[SerializeField]
		private Image mapImage;

		[Tooltip("Harita görseli yokken açılacak obje. İsteğe bağlı.")]
		[SerializeField]
		private GameObject mapImagePlaceholder;

		[Tooltip("Lobinin BÜTÜN parametreleri - mod, harita, oyuncu sayısı, süreler, minimumlar, şifre, bağlantı türü ve adres - tek metin halinde buraya yazılır.\n\nSütun hizası ve soluk başlıklar TMP etiketleriyle yapılıyor, yani bu etikette Rich Text AÇIK kalmalı. Kapatırsan etiketlerin kendisi görünür.")]
		[SerializeField]
		private TextMeshProUGUI infoLabel;

		[SerializeField]
		private Button joinButton;

		[SerializeField]
		private Button closeButton;

		private Action pendingJoin;

		private void Awake()
		{
			if (panel == null)
			{
				panel = base.gameObject;
			}
			if (closeButton != null)
			{
				closeButton.onClick.AddListener(Hide);
			}
			if (joinButton != null)
			{
				joinButton.onClick.AddListener(JoinShown);
			}
			panel.SetActive(value: false);
		}

		public void Show(in LobbyListing listing, Action onJoin)
		{
			pendingJoin = onJoin;
			if (nameLabel != null)
			{
				nameLabel.text = (string.IsNullOrEmpty(listing.Name) ? Loc.Get("LobbyBrowser.Unnamed") : listing.Name);
			}
			if (infoLabel != null)
			{
				infoLabel.text = LobbyListingText.Summary(in listing);
			}
			if (mapImage != null)
			{
				Sprite sprite = LobbyListingText.MapPreview(in listing);
				mapImage.sprite = sprite;
				mapImage.enabled = sprite != null;
				if (mapImagePlaceholder != null)
				{
					mapImagePlaceholder.SetActive(sprite == null);
				}
			}
			if (panel != null)
			{
				panel.SetActive(value: true);
			}
			UILayout.RebuildFrom((panel != null) ? panel.transform : base.transform);
		}

		public void Hide()
		{
			pendingJoin = null;
			if (panel != null)
			{
				panel.SetActive(value: false);
			}
		}

		private void JoinShown()
		{
			Action action = pendingJoin;
			Hide();
			action?.Invoke();
		}
	}
}
