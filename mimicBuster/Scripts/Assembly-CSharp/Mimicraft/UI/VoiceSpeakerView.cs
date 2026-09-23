using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Voice;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class VoiceSpeakerView : MonoBehaviour
	{
		private class Row
		{
			public GameObject Root;

			public Image Avatar;

			public TextMeshProUGUI Initial;

			public TextMeshProUGUI Name;

			public TextMeshProUGUI Channel;

			public Image Level;

			public ulong AvatarSteamId;
		}

		[Tooltip("Satırların ekleneceği obje. Dikey bir LayoutGroup koyarsan satırlar kendiliğinden dizilir. Bu objeden farklıysa, kimse konuşmuyorken kapatılır.")]
		[SerializeField]
		private RectTransform container;

		[Tooltip("Elle hazırlanmış satır şablonu. PASİF bırakılır; klonları aktif edilir. İsimleriyle aranan çocuklar (hepsi isteğe bağlı): Avatar (Image), Initial (TMP), Name (TMP), Channel (TMP), Level (Image, Image Type = Filled). Yazı tipi, renk ve hizalama tamamen şablondan gelir.")]
		[SerializeField]
		private GameObject rowTemplate;

		[Tooltip("Seviye çubuğunun hassasiyeti. Konuşmanın RMS'i genelde 0.05-0.2 arasında kalır, yani çubuğun dolabilmesi için çarpılması gerekir. Büyüt = daha çabuk dolar.")]
		[SerializeField]
		[Min(1f)]
		private float levelScale = 5f;

		[Tooltip("Herkese konuşan biri için vurgu rengi.")]
		[SerializeField]
		private Color allColor = new Color(0.85f, 0.85f, 0.85f, 1f);

		[Tooltip("Takımına konuşan biri için vurgu rengi.")]
		[SerializeField]
		private Color teamColor = new Color(0.45f, 0.85f, 0.5f, 1f);

		private readonly List<Row> rows = new List<Row>();

		private readonly List<VoiceSpeakers.Speaker> shown = new List<VoiceSpeakers.Speaker>();

		private void Awake()
		{
			if (rowTemplate != null)
			{
				rowTemplate.SetActive(value: false);
			}
		}

		private void OnEnable()
		{
			VoiceSpeakers.Changed += Rebuild;
			Rebuild();
		}

		private void OnDisable()
		{
			VoiceSpeakers.Changed -= Rebuild;
		}

		private void Update()
		{
			for (int i = 0; i < shown.Count && i < rows.Count; i++)
			{
				Image level = rows[i].Level;
				if (level != null)
				{
					level.fillAmount = Mathf.Clamp01(VoiceSpeakers.LevelOf(shown[i].ClientId) * levelScale);
				}
			}
		}

		private void Rebuild()
		{
			if (rowTemplate == null)
			{
				return;
			}
			Collect();
			if (container != null && container.gameObject != base.gameObject)
			{
				container.gameObject.SetActive(shown.Count > 0);
			}
			EnsureRowCount(shown.Count);
			for (int i = 0; i < rows.Count; i++)
			{
				Row row = rows[i];
				if (i >= shown.Count)
				{
					row.Root.SetActive(value: false);
					continue;
				}
				row.Root.SetActive(value: true);
				Fill(row, shown[i]);
			}
		}

		private void Collect()
		{
			shown.Clear();
			shown.AddRange(VoiceSpeakers.Active);
			ulong local = ((NetworkManager.Singleton != null) ? NetworkManager.Singleton.LocalClientId : ulong.MaxValue);
			shown.Sort((VoiceSpeakers.Speaker a, VoiceSpeakers.Speaker b) => (a.ClientId == local != (b.ClientId == local)) ? ((a.ClientId != local) ? 1 : (-1)) : a.ClientId.CompareTo(b.ClientId));
		}

		private void Fill(Row row, VoiceSpeakers.Speaker speaker)
		{
			GameModeController current = GameModeController.Current;
			string text = ((current != null) ? current.GetPlayerName(speaker.ClientId) : "");
			Color color = ((speaker.Channel == VoiceChannel.Team) ? teamColor : allColor);
			if (row.Name != null)
			{
				row.Name.text = text;
				row.Name.color = color;
			}
			if (row.Channel != null)
			{
				row.Channel.text = Loc.Get((speaker.Channel == VoiceChannel.Team) ? "Voice.Team" : "Voice.All");
				row.Channel.color = color;
			}
			if (row.Level != null)
			{
				row.Level.color = color;
			}
			ApplyAvatar(row, (current != null) ? current.GetSteamId(speaker.ClientId) : 0, text);
		}

		private void ApplyAvatar(Row row, ulong steamId, string name)
		{
			if (steamId == 0L || !SteamManager.IsInitialized)
			{
				row.AvatarSteamId = 0uL;
				if (row.Avatar != null)
				{
					row.Avatar.enabled = false;
				}
				ShowInitial(row, name);
				return;
			}
			if (row.AvatarSteamId == steamId && row.Avatar != null && row.Avatar.sprite != null)
			{
				row.Avatar.enabled = true;
				if (row.Initial != null)
				{
					row.Initial.enabled = false;
				}
				return;
			}
			row.AvatarSteamId = steamId;
			ShowInitial(row, name);
			if (row.Avatar != null)
			{
				row.Avatar.enabled = false;
			}
			LoadAvatar(row, steamId);
		}

		private async void LoadAvatar(Row row, ulong steamId)
		{
			Sprite sprite = await SteamManager.FetchAvatarSpriteAsync(steamId);
			if (!(sprite == null) && !(row.Avatar == null) && row.AvatarSteamId == steamId)
			{
				row.Avatar.sprite = sprite;
				row.Avatar.enabled = true;
				if (row.Initial != null)
				{
					row.Initial.enabled = false;
				}
			}
		}

		private static void ShowInitial(Row row, string name)
		{
			if (!(row.Initial == null))
			{
				row.Initial.enabled = true;
				row.Initial.text = (string.IsNullOrWhiteSpace(name) ? "?" : name.Substring(0, 1).ToUpperInvariant());
			}
		}

		private void EnsureRowCount(int needed)
		{
			while (rows.Count < needed)
			{
				GameObject gameObject = Object.Instantiate(rowTemplate, (container != null) ? container : rowTemplate.transform.parent);
				gameObject.name = $"Speaker{rows.Count}";
				rows.Add(Bind(gameObject));
			}
		}

		private static Row Bind(GameObject root)
		{
			return new Row
			{
				Root = root,
				Avatar = FindImage(root, "Avatar"),
				Initial = FindLabel(root, "Initial"),
				Name = FindLabel(root, "Name"),
				Channel = FindLabel(root, "Channel"),
				Level = FindImage(root, "Level")
			};
		}

		private static Image FindImage(GameObject root, string childName)
		{
			Transform transform = root.transform.Find(childName);
			if (!(transform != null))
			{
				return null;
			}
			return transform.GetComponent<Image>();
		}

		private static TextMeshProUGUI FindLabel(GameObject root, string childName)
		{
			Transform transform = root.transform.Find(childName);
			if (!(transform != null))
			{
				return null;
			}
			return transform.GetComponent<TextMeshProUGUI>();
		}
	}
}
