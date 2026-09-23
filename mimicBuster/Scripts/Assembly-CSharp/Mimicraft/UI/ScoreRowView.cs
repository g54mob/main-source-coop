using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ScoreRowView : MonoBehaviour
	{
		[Tooltip("Satırın zemini - sıra rengi, kendi satırın vurgusu ve MVP rengi buraya yazılır. Boşsa satırın kökündeki Image kullanılır.")]
		[SerializeField]
		private Image background;

		[Tooltip("Steam avatarı. Boşsa 'Avatar' adlı çocuk aranır. Steam'den gelmeyen oyuncuda kapanır.")]
		[SerializeField]
		private Image avatar;

		[Tooltip("Oyuncu adı. Boşsa 'Name' adlı çocuk aranır. Rich Text açık kalmalı - 'sen' etiketi renkli yazılıyor.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Puan. Boşsa 'Score' adlı çocuk aranır.")]
		[SerializeField]
		private TextMeshProUGUI scoreLabel;

		[Tooltip("Gecikme (ping). İsteğe bağlı; boşsa 'Ping' adlı çocuk aranır.")]
		[SerializeField]
		private TextMeshProUGUI pingLabel;

		[Tooltip("Rol sütunu - yalnızca round bitince dolar. İsteğe bağlı; boşsa 'Role' adlı çocuk aranır.")]
		[SerializeField]
		private TextMeshProUGUI roleLabel;

		[Tooltip("Can sütunu - yalnızca round bitince dolar. İsteğe bağlı; boşsa 'Health' adlı çocuk aranır.")]
		[SerializeField]
		private TextMeshProUGUI healthLabel;

		[Tooltip("O oyuncunun ses seviyesi. İsteğe bağlı; boşsa 'Volume' adlı çocuk aranır. Kendi satırında gizlenir. Min/Max değerleri KODDAN kurulur.")]
		[SerializeField]
		private Slider volume;

		[Tooltip("O oyuncuyu susturma anahtarı. İsteğe bağlı; boşsa 'Mute' adlı çocuk aranır. Kendi satırında gizlenir.")]
		[SerializeField]
		private Toggle mute;

		[Tooltip("Susturma anahtarının yazısı. İsteğe bağlı; boşsa 'MuteLabel' adlı çocuk aranır.")]
		[SerializeField]
		private TextMeshProUGUI muteLabel;

		[Tooltip("Steam profilini overlay'de açan buton. İsteğe bağlı; boşsa 'Profile' adlı çocuk aranır. Steam kapalıysa ya da o oyuncunun hesabı yoksa kendiliğinden gizlenir.")]
		[SerializeField]
		private Button profile;

		public Image Background => background;

		public Image Avatar => avatar;

		public TextMeshProUGUI NameLabel => nameLabel;

		public TextMeshProUGUI ScoreLabel => scoreLabel;

		public TextMeshProUGUI PingLabel => pingLabel;

		public TextMeshProUGUI RoleLabel => roleLabel;

		public TextMeshProUGUI HealthLabel => healthLabel;

		public Slider Volume => volume;

		public Toggle Mute => mute;

		public TextMeshProUGUI MuteLabel => muteLabel;

		public Button Profile => profile;

		public void ResolveMissing()
		{
			if (background == null)
			{
				background = GetComponent<Image>();
			}
			if (avatar == null)
			{
				avatar = FindChild<Image>("Avatar");
			}
			if (nameLabel == null)
			{
				nameLabel = FindChild<TextMeshProUGUI>("Name");
			}
			if (scoreLabel == null)
			{
				scoreLabel = FindChild<TextMeshProUGUI>("Score");
			}
			if (pingLabel == null)
			{
				pingLabel = FindChild<TextMeshProUGUI>("Ping");
			}
			if (roleLabel == null)
			{
				roleLabel = FindChild<TextMeshProUGUI>("Role");
			}
			if (healthLabel == null)
			{
				healthLabel = FindChild<TextMeshProUGUI>("Health");
			}
			if (volume == null)
			{
				volume = FindChild<Slider>("Volume");
			}
			if (mute == null)
			{
				mute = FindChild<Toggle>("Mute");
			}
			if (muteLabel == null)
			{
				muteLabel = FindChild<TextMeshProUGUI>("MuteLabel");
			}
			if (profile == null)
			{
				profile = FindChild<Button>("Profile");
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
