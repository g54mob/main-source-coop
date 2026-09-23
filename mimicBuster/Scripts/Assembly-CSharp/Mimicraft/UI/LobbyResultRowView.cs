using System;
using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class LobbyResultRowView : MonoBehaviour
	{
		[Serializable]
		public struct CountryFlag
		{
			public string code;

			public Sprite flag;
		}

		[Tooltip("Lobinin adı.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Oyun modunun adı - 'Prop Hunt', 'Gartic' gibi. Mod bildirmeyen bir host'ta boş kalır.")]
		[SerializeField]
		private TextMeshProUGUI modeLabel;

		[Tooltip("Haritanın adı.")]
		[SerializeField]
		private TextMeshProUGUI mapLabel;

		[Tooltip("Haritanın görseli - MapScriptableObject'teki Preview. Bu istemcinin katalogunda olmayan bir harita için boş kalır.")]
		[SerializeField]
		private Image mapImage;

		[Tooltip("Harita görseli yokken açılacak obje. İsteğe bağlı.")]
		[SerializeField]
		private GameObject mapImagePlaceholder;

		[Tooltip("Lobinin ne yaptığı - 'Oyuncular bekleniyor', 'Av sürüyor' gibi. Host söylemiyorsa (eski sürüm) boş kalır.")]
		[SerializeField]
		private TextMeshProUGUI phaseLabel;

		[Tooltip("Doluluk - '3/16'. Bildirmeyen bir host'ta sadece sayı yazar.")]
		[SerializeField]
		private TextMeshProUGUI playersLabel;

		[Tooltip("Steam mi Direct mi.")]
		[SerializeField]
		private TextMeshProUGUI transportLabel;

		[Tooltip("Tahmini gecikme - '48 ms'. Steam lobilerinde host'un yayınladığı ağ konumuyla hesaplanır, hiçbir paket gönderilmeden. LAN lobilerinde ve bunu yayınlamayan eski sürümlerde tire görünür.\n\nRenk TMP etiketiyle veriliyor, yani bu etikette Rich Text AÇIK kalmalı.")]
		[SerializeField]
		private TextMeshProUGUI pingLabel;

		[Tooltip("Host'un ülkesi - Steam'in iki harfli kodu (TR, DE). LAN lobilerinde ve eski sürümlerde boş kalır. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI countryLabel;

		[Tooltip("Ülke bayrağı için görsel. Bayrak sprite'larını Country Flags listesinden eşleştirir; liste boşsa gizli kalır. İsteğe bağlı.")]
		[SerializeField]
		private Image countryFlag;

		[Tooltip("Ülke kodu ile bayrak görselini eşleyen liste. Kod iki harf olmalı: TR, DE, BR...")]
		[SerializeField]
		private CountryFlag[] countryFlags = new CountryFlag[0];

		[Tooltip("Lobi ŞİFRELİYSE açılacak obje - kilit ikonu gibi. İsteğe bağlı ama önerilir: şifreyi Katıl'a bastıktan sonra öğrenmek kötü bir sürpriz.")]
		[SerializeField]
		private GameObject passwordIndicator;

		[SerializeField]
		private Button joinButton;

		[Tooltip("Detay panelini açan buton. İsteğe bağlı - bağlamazsan satır eskisi gibi sadece Katıl'dan ibaret olur.")]
		[SerializeField]
		private Button detailsButton;

		private Sprite FindFlag(string country)
		{
			if (string.IsNullOrEmpty(country) || countryFlags == null)
			{
				return null;
			}
			CountryFlag[] array = countryFlags;
			for (int i = 0; i < array.Length; i++)
			{
				CountryFlag countryFlag = array[i];
				if (!string.IsNullOrEmpty(countryFlag.code) && string.Equals(countryFlag.code.Trim(), country, StringComparison.OrdinalIgnoreCase))
				{
					return countryFlag.flag;
				}
			}
			return null;
		}

		public void Bind(in LobbyListing listing, Action onJoin, Action onDetails)
		{
			Write(nameLabel, string.IsNullOrEmpty(listing.Name) ? Loc.Get("LobbyBrowser.Unnamed") : listing.Name);
			Write(modeLabel, LobbyListingText.Mode(in listing));
			Write(mapLabel, LobbyListingText.Map(in listing));
			Write(phaseLabel, LobbyListingText.Phase(in listing));
			Write(playersLabel, LobbyListingText.Players(in listing));
			Write(transportLabel, LobbyListingText.Transport(in listing));
			Write(pingLabel, LobbyListingText.PingColored(in listing));
			string text = LobbyListingText.Country(in listing);
			Write(countryLabel, text);
			if (countryFlag != null)
			{
				Sprite sprite = FindFlag(text);
				countryFlag.sprite = sprite;
				countryFlag.enabled = sprite != null;
			}
			if (mapImage != null)
			{
				Sprite sprite2 = LobbyListingText.MapPreview(in listing);
				mapImage.sprite = sprite2;
				mapImage.enabled = sprite2 != null;
				if (mapImagePlaceholder != null)
				{
					mapImagePlaceholder.SetActive(sprite2 == null);
				}
			}
			if (passwordIndicator != null)
			{
				passwordIndicator.SetActive(listing.HasPassword);
			}
			Wire(joinButton, onJoin);
			if (detailsButton != null)
			{
				detailsButton.gameObject.SetActive(onDetails != null);
			}
			Wire(detailsButton, onDetails);
		}

		private static void Write(TextMeshProUGUI label, string text)
		{
			if (label != null)
			{
				label.text = text;
			}
		}

		private static void Wire(Button button, Action action)
		{
			if (button == null)
			{
				return;
			}
			button.onClick.RemoveAllListeners();
			if (action != null)
			{
				button.onClick.AddListener(delegate
				{
					action();
				});
			}
		}
	}
}
