using System;
using Mimicraft.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class AboutView : MonoBehaviour
	{
		private const string SeenKey = "Mimicraft.AboutSeenBuild";

		[Tooltip("Açılan panel. Boş bırakılırsa bu bileşenin kendi objesi açılıp kapanır.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Metnin yazılacağı etiket. Rich Text açık olmalı - iletişim satırları kalın yazılıyor.")]
		[SerializeField]
		private TextMeshProUGUI bodyLabel;

		[Tooltip("Sürüm numarasının yazılacağı etiket (isteğe bağlı). '0.1.2 - DEMO' gibi - bir hata raporunda ilk sorulacak şey bu.")]
		[SerializeField]
		private TextMeshProUGUI versionLabel;

		[SerializeField]
		private Button closeButton;

		[Header("İletişim")]
		[Tooltip("Discord davet bağlantısı. Hem metnin altına yazılıyor hem de Discord düğmesi bunu açıyor.")]
		[SerializeField]
		private string discordUrl = "https://discord.gg/DAVET-KODU";

		[Tooltip("Discord davet bağlantısı. Hem metnin altına yazılıyor hem de Discord düğmesi bunu açıyor.")]
		[SerializeField]
		private string steamUrl = "https://store.steampowered.com/app/STEAM-APP-ID";

		[Tooltip("İletişim e-posta adresi. Metnin altına yazılıyor; e-posta düğmesi varsa mail uygulamasını bu adresle açıyor.")]
		[SerializeField]
		private string contactMail = "iletisim@ornek.com";

		[Tooltip("İsteğe bağlı - basınca Discord davetini tarayıcıda açar.")]
		[SerializeField]
		private Button discordButton;

		[Tooltip("İsteğe bağlı - basınca Steam sayfasını tarayıcıda açar.")]
		[SerializeField]
		private Button steamButton;

		[Tooltip("İsteğe bağlı - basınca varsayılan mail uygulamasını bu adrese açar.")]
		[SerializeField]
		private Button mailButton;

		[Header("Metinler")]
		[Tooltip("DEMO sürümünün metnini taşıyan yerelleştirme anahtarı. Metnin kendisi Assets/Localization/UI.csv içinde - 11 dilde. Boşsa demo sürümünde panel hiç açılmaz.")]
		[SerializeField]
		private string demoTextKey = "About.Demo.Body";

		[Tooltip("PLAYTEST sürümünün metninin anahtarı. Boşsa playtest sürümünde panel hiç açılmaz.")]
		[SerializeField]
		private string playtestTextKey = "About.Playtest.Body";

		[Tooltip("Tam sürümün metninin anahtarı. Boş bırakıldı, çünkü yayınlanmış bir oyunun verecek böyle bir notu yoktur - boşken tam sürümde panel hiç açılmaz.")]
		[SerializeField]
		private string releaseTextKey = "";

		[Header("Metin ezmesi (isteğe bağlı)")]
		[Tooltip("Doldurursan yerelleştirmeyi EZER ve her dilde bu metin görünür. Hızlı bir deneme ya da tabloları güncellemeden çıkılması gereken bir build için; kalıcı metin tabloya yazılmalı, yoksa oyun o notu tek dilde verir.")]
		[SerializeField]
		[TextArea(6, 20)]
		private string demoTextOverride = "";

		[SerializeField]
		[TextArea(6, 20)]
		private string playtestTextOverride = "";

		[SerializeField]
		[TextArea(6, 20)]
		private string releaseTextOverride = "";

		private Action closed;

		private static string warnedKey;

		public static AboutView Instance { get; private set; }

		private GameObject Target
		{
			get
			{
				if (!(panel != null))
				{
					return base.gameObject;
				}
				return panel;
			}
		}

		private string Text
		{
			get
			{
				var (text, text2) = Build.Kind switch
				{
					BuildKind.Demo => (demoTextKey, demoTextOverride), 
					BuildKind.Playtest => (playtestTextKey, playtestTextOverride), 
					_ => (releaseTextKey, releaseTextOverride), 
				};
				if (!string.IsNullOrWhiteSpace(text2))
				{
					return text2;
				}
				if (string.IsNullOrEmpty(text))
				{
					return "";
				}
				if (Loc.TryGet(text, out var text3))
				{
					return text3;
				}
				WarnMissingOnce(text);
				return "";
			}
		}

		private void Awake()
		{
			Instance = this;
			if (closeButton != null)
			{
				closeButton.onClick.AddListener(Hide);
			}
			if (discordButton != null)
			{
				discordButton.onClick.AddListener(OpenDiscord);
			}
			if (steamButton != null)
			{
				steamButton.onClick.AddListener(OpenSteam);
			}
			if (mailButton != null)
			{
				mailButton.onClick.AddListener(OpenMail);
			}
			Target.SetActive(value: false);
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
			GameMenuState.SetMenuOpen(this, open: false);
		}

		private static void WarnMissingOnce(string key)
		{
			if (!(warnedKey == key))
			{
				warnedKey = key;
				Debug.LogWarning("[AboutView] '" + key + "' yerellestirme tablolarinda yok - metin gosterilemiyor. Tools > Mimicraft > Yerellestirme > Tablolari CSV'den guncelle calistirildi mi?");
			}
		}

		public static bool TryShowForNewBuild(Action onClosed)
		{
			if (Instance == null || string.IsNullOrWhiteSpace(Instance.Text))
			{
				return false;
			}
			if (PlayerPrefs.GetString("Mimicraft.AboutSeenBuild", "") == Build.VersionLabel)
			{
				return false;
			}
			PlayerPrefs.SetString("Mimicraft.AboutSeenBuild", Build.VersionLabel);
			PlayerPrefs.Save();
			Instance.Show(onClosed);
			return true;
		}

		public static void ForgetSeenBuild()
		{
			PlayerPrefs.DeleteKey("Mimicraft.AboutSeenBuild");
			PlayerPrefs.Save();
		}

		public void Show()
		{
			Show(null);
		}

		public void Show(Action onClosed)
		{
			closed = onClosed;
			if (bodyLabel != null)
			{
				bodyLabel.text = BuildBody();
			}
			if (versionLabel != null)
			{
				versionLabel.text = Build.VersionLabel;
			}
			Target.SetActive(value: true);
			GameMenuState.SetMenuOpen(this, open: true);
		}

		public void Hide()
		{
			Target.SetActive(value: false);
			GameMenuState.SetMenuOpen(this, open: false);
			Action action = closed;
			closed = null;
			action?.Invoke();
		}

		private void Update()
		{
			if (Target.activeSelf && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				GameMenuState.RequestEscape(100, Hide);
			}
		}

		private string BuildBody()
		{
			string text = Text;
			if (!string.IsNullOrWhiteSpace(discordUrl))
			{
				text = text + "\n\n<b>Discord:</b> " + discordUrl;
			}
			if (!string.IsNullOrWhiteSpace(contactMail))
			{
				text = text + "\n<b>E-posta:</b> " + contactMail;
			}
			return text.TrimStart('\n');
		}

		public void OpenDiscord()
		{
			if (!string.IsNullOrWhiteSpace(discordUrl))
			{
				Application.OpenURL(discordUrl);
			}
		}

		public void OpenSteam()
		{
			if (!string.IsNullOrWhiteSpace(steamUrl))
			{
				Application.OpenURL(steamUrl);
			}
		}

		public void OpenMail()
		{
			if (!string.IsNullOrWhiteSpace(contactMail))
			{
				Application.OpenURL("mailto:" + contactMail + "?subject=" + UnityWebRequest.EscapeURL("Mimic Busters " + Build.VersionLabel));
			}
		}
	}
}
