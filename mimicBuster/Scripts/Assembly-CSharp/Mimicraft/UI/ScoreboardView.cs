using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using Mimicraft.Voice;
using Steamworks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ScoreboardView : MonoBehaviour
	{
		private class Row
		{
			public GameObject Root;

			public Image Background;

			public Image Avatar;

			public TextMeshProUGUI Name;

			public TextMeshProUGUI Score;

			public TextMeshProUGUI Ping;

			public TextMeshProUGUI Role;

			public TextMeshProUGUI Health;

			public ulong AvatarSteamId;

			public Slider Volume;

			public Toggle Mute;

			public TextMeshProUGUI MuteLabel;

			public Button Profile;
		}

		private static readonly Color RowEven = new Color(1f, 1f, 1f, 0.03f);

		private static readonly Color RowOdd = new Color(1f, 1f, 1f, 0.07f);

		private static readonly Color LocalRow = new Color(0.25f, 0.45f, 0.35f, 0.35f);

		private static readonly Color MvpRow = new Color(0.55f, 0.44f, 0.12f, 0.45f);

		[Tooltip("The whole board, switched on while the key is held. Usually this object's own panel child. Left empty, this GameObject is used.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Elle bir kez dizilmiş, oyuncu başına kopyalanan İNAKTİF satır. Kopyalar bunun yanına doğar, yani listeyi tutan şey bunun PARENT'ıdır.\n\nParçalarını satırın üzerindeki ScoreRowView bileşenine bağla: zemin, avatar, ad, puan, ping, rol, can, ses seviyesi, susturma ve Steam profili butonu. Hepsi isteğe bağlı, bağlanmayan bir sütun sadece görünmez.\n\nBileşen yoksa kendiliğinden eklenir ve boş bıraktığın alanlar eski isimlerden doldurulur (Avatar, Name, Score, Ping, Role, Health, Volume, Mute, MuteLabel, Profile), yani bugünkü şablon olduğu gibi çalışmaya devam eder.")]
		[SerializeField]
		private GameObject rowTemplate;

		[Tooltip("BOŞ BIRAK. Oynanan modun denetleyicisi her karede kendisi bulunur (GameModeController.Current).\n\nTablo artık mod sahnelerinde değil Game sahnesinde duruyor, yani mod sahneleri onun altından gelip geçiyor. Buraya elle sürüklenen bir referans ilk mod bittiğinde yok olmuş bir objeyi gösterir; alan yalnızca tek bir modun sahnesinde denerken işe yarar.")]
		[SerializeField]
		private GameModeController roundManager;

		[Tooltip("Rol sütununun başlığı. İsteğe bağlı: verilirse sütun gizlendiğinde başlığı da gizlenir. Modun kendisi karar verir (GameModeController.ShowsScoreboardRole).")]
		[SerializeField]
		private GameObject roleHeader;

		[Tooltip("Can sütununun başlığı. İsteğe bağlı, rol başlığıyla aynı mantık.")]
		[SerializeField]
		private GameObject healthHeader;

		[Tooltip("Optional headline above the table - 'MVP: Ali — En çok hasar (204)'. Hidden entirely while a round is running, because no MVP has been published yet.")]
		[SerializeField]
		private TextMeshProUGUI mvpLabel;

		[SerializeField]
		private GameObject mvpPanel;

		[SerializeField]
		private Gradient pingGradient;

		private const float PingMin = 50f;

		private const float PingMax = 300f;

		private readonly List<Row> rows = new List<Row>();

		private readonly List<(ulong ClientId, int Score)> ordered = new List<(ulong, int)>();

		private string appliedHighlight;

		private bool visible;

		private bool dirty = true;

		private float nextPingRefresh;

		private bool subscribed;

		private GameModeController appliedColumnsFor;

		private void Awake()
		{
			if (panel == null)
			{
				panel = base.gameObject;
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
			}
			if (rowTemplate != null)
			{
				rowTemplate.SetActive(value: false);
			}
			if (panel != base.gameObject)
			{
				panel.SetActive(value: false);
			}
			if (mvpPanel != null)
			{
				mvpPanel.SetActive(value: false);
			}
		}

		private void OnEnable()
		{
			Subscribe();
			Loc.Changed += ForgetCachedText;
		}

		private void OnDisable()
		{
			Unsubscribe();
			Loc.Changed -= ForgetCachedText;
			if (visible)
			{
				visible = false;
				GameMenuState.SetOverlayOpen(this, open: false);
			}
		}

		private void Subscribe()
		{
			if (!subscribed && !(roundManager == null))
			{
				subscribed = true;
				roundManager.Scores.OnListChanged += OnScoresChanged;
				roundManager.Names.OnListChanged += OnNamesChanged;
				roundManager.Reveals.OnListChanged += OnRevealsChanged;
			}
		}

		private void Unsubscribe()
		{
			if (subscribed && !(roundManager == null))
			{
				subscribed = false;
				roundManager.Scores.OnListChanged -= OnScoresChanged;
				roundManager.Names.OnListChanged -= OnNamesChanged;
				roundManager.Reveals.OnListChanged -= OnRevealsChanged;
			}
		}

		private void OnScoresChanged(NetworkListEvent<PlayerScoreEntry> _)
		{
			dirty = true;
		}

		private void OnNamesChanged(NetworkListEvent<PlayerNameEntry> _)
		{
			dirty = true;
		}

		private void OnRevealsChanged(NetworkListEvent<PlayerRevealEntry> _)
		{
			dirty = true;
		}

		private void Update()
		{
			GameModeController current = GameModeController.Current;
			if (roundManager == null || (current != null && current != roundManager))
			{
				Unsubscribe();
				roundManager = current;
				subscribed = false;
				dirty = true;
				appliedColumnsFor = null;
			}
			if (roundManager == null || rowTemplate == null)
			{
				if (visible)
				{
					visible = false;
					if (panel != base.gameObject)
					{
						panel.SetActive(value: false);
					}
					else
					{
						SetRowsShown(shown: false);
					}
					ApplyOverlay(claimed: false);
				}
				return;
			}
			ApplyColumns();
			if (!subscribed)
			{
				Subscribe();
			}
			UpdateMvp();
			bool flag = !GameMenuState.IsMenuOpen && GameInput.Scoreboard.IsPressed();
			if (flag != visible)
			{
				visible = flag;
				if (panel != base.gameObject)
				{
					panel.SetActive(visible);
				}
				else
				{
					SetRowsShown(visible);
				}
				ApplyOverlay(visible);
				if (visible)
				{
					dirty = true;
				}
			}
			if (visible)
			{
				if (dirty)
				{
					dirty = false;
					Rebuild();
				}
				else if (Time.unscaledTime >= nextPingRefresh)
				{
					nextPingRefresh = Time.unscaledTime + 0.5f;
					RefreshPings();
				}
			}
		}

		private void ApplyOverlay(bool claimed)
		{
			GameMenuState.SetOverlayOpen(this, claimed);
			PlayerEditSession playerEditSession = LocalEditSession();
			if (playerEditSession != null)
			{
				playerEditSession.RefreshCursorState();
				return;
			}
			Cursor.lockState = ((!claimed) ? CursorLockMode.Locked : CursorLockMode.None);
			Cursor.visible = claimed;
		}

		private static PlayerEditSession LocalEditSession()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || singleton.LocalClient?.PlayerObject == null)
			{
				return null;
			}
			return singleton.LocalClient.PlayerObject.GetComponent<PlayerEditSession>();
		}

		private void SetRowsShown(bool shown)
		{
			foreach (Row row in rows)
			{
				row.Root.SetActive(shown && row.Root.activeSelf);
			}
		}

		private void Rebuild()
		{
			ordered.Clear();
			foreach (PlayerNameEntry name in roundManager.Names)
			{
				ordered.Add((name.ClientId, ScoreOf(name.ClientId)));
			}
			ordered.Sort(((ulong ClientId, int Score) a, (ulong ClientId, int Score) b) => b.Score.CompareTo(a.Score));
			ulong num = ((NetworkManager.Singleton != null) ? NetworkManager.Singleton.LocalClientId : ulong.MaxValue);
			ulong highlightedClientId = roundManager.HighlightedClientId;
			EnsureRowCount(ordered.Count);
			for (int num2 = 0; num2 < rows.Count; num2++)
			{
				Row row = rows[num2];
				if (num2 >= ordered.Count)
				{
					row.Root.SetActive(value: false);
					continue;
				}
				var (num3, num4) = ordered[num2];
				row.Root.SetActive(value: true);
				if (row.Background != null)
				{
					row.Background.color = ((num3 == highlightedClientId) ? MvpRow : ((num3 == num) ? LocalRow : ((num2 % 2 == 0) ? RowEven : RowOdd)));
				}
				SetText(row.Name, roundManager.GetPlayerName(num3) + ((num3 == num) ? ("  <color=yellow>" + Loc.Get("Scoreboard.You") + "</color>") : ""));
				SetText(row.Score, num4.ToString());
				ApplyReveal(row, num3);
				ApplyPing(row, num3);
				ulong steamId = roundManager.GetSteamId(num3);
				ApplyAvatar(row, steamId);
				ApplyProfileButton(row, steamId);
				ApplyVoiceControls(row, num3, num);
			}
			nextPingRefresh = Time.unscaledTime + 0.5f;
		}

		private void EnsureRowCount(int needed)
		{
			while (rows.Count < needed)
			{
				GameObject obj = UnityEngine.Object.Instantiate(rowTemplate, rowTemplate.transform.parent);
				obj.name = $"Row{rows.Count}";
				Row row = BindRow(obj);
				WireVoice(row, rows.Count);
				rows.Add(row);
			}
		}

		private void WireVoice(Row row, int index)
		{
			if (row.Volume != null)
			{
				row.Volume.minValue = 0f;
				row.Volume.maxValue = 2f;
				row.Volume.onValueChanged.AddListener(delegate(float value)
				{
					if (index < ordered.Count)
					{
						VoiceMuteStore.SetVolume(ordered[index].ClientId, value);
					}
				});
			}
			if (row.Mute != null)
			{
				row.Mute.onValueChanged.AddListener(delegate(bool muted)
				{
					if (index < ordered.Count)
					{
						VoiceMuteStore.SetMuted(ordered[index].ClientId, muted);
					}
				});
			}
			if (!(row.Profile != null))
			{
				return;
			}
			row.Profile.onClick.AddListener(delegate
			{
				if (index < ordered.Count)
				{
					OpenSteamProfile(ordered[index].ClientId);
				}
			});
		}

		private void OpenSteamProfile(ulong clientId)
		{
			if (!SteamManager.IsInitialized)
			{
				return;
			}
			ulong num = ((roundManager != null) ? roundManager.GetSteamId(clientId) : 0);
			if (num == 0L)
			{
				return;
			}
			try
			{
				SteamFriends.OpenUserOverlay(new SteamId
				{
					Value = num
				}, "steamid");
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Scoreboard] Steam profili acilamadi: " + ex.Message);
			}
		}

		private static void ApplyProfileButton(Row row, ulong steamId)
		{
			if (!(row.Profile == null))
			{
				bool flag = SteamManager.IsInitialized && steamId != 0;
				if (row.Profile.gameObject.activeSelf != flag)
				{
					row.Profile.gameObject.SetActive(flag);
				}
			}
		}

		private void ApplyVoiceControls(Row row, ulong clientId, ulong localId)
		{
			bool flag = clientId == localId;
			if (row.Volume != null)
			{
				row.Volume.gameObject.SetActive(!flag);
				if (!flag)
				{
					row.Volume.SetValueWithoutNotify(VoiceMuteStore.VolumeFor(clientId));
				}
			}
			if (row.Mute != null)
			{
				row.Mute.gameObject.SetActive(!flag);
				if (!flag)
				{
					row.Mute.SetIsOnWithoutNotify(VoiceMuteStore.IsMuted(clientId));
				}
			}
			if (row.MuteLabel != null)
			{
				row.MuteLabel.text = Loc.Get("Scoreboard.Mute");
			}
		}

		private static Row BindRow(GameObject root)
		{
			ScoreRowView scoreRowView = root.GetComponent<ScoreRowView>();
			if (scoreRowView == null)
			{
				scoreRowView = root.AddComponent<ScoreRowView>();
			}
			scoreRowView.ResolveMissing();
			return new Row
			{
				Root = root,
				Background = scoreRowView.Background,
				Avatar = scoreRowView.Avatar,
				Name = scoreRowView.NameLabel,
				Score = scoreRowView.ScoreLabel,
				Ping = scoreRowView.PingLabel,
				Role = scoreRowView.RoleLabel,
				Health = scoreRowView.HealthLabel,
				Volume = scoreRowView.Volume,
				Mute = scoreRowView.Mute,
				MuteLabel = scoreRowView.MuteLabel,
				Profile = scoreRowView.Profile
			};
		}

		private static void SetText(TextMeshProUGUI label, string text)
		{
			if (label != null)
			{
				label.text = text;
			}
		}

		private void UpdateMvp()
		{
			string text;
			bool flag = roundManager.TryGetScoreHighlight(out text);
			GameObject gameObject = ((mvpPanel != null) ? mvpPanel : ((mvpLabel != null) ? mvpLabel.gameObject : null));
			if (gameObject != null && gameObject.activeSelf != flag)
			{
				gameObject.SetActive(flag);
			}
			if (!flag || mvpLabel == null)
			{
				appliedHighlight = null;
			}
			else if (!(text == appliedHighlight))
			{
				appliedHighlight = text;
				mvpLabel.text = text;
			}
		}

		private void ForgetCachedText()
		{
			appliedHighlight = null;
		}

		private void RefreshPings()
		{
			for (int i = 0; i < rows.Count && i < ordered.Count; i++)
			{
				ApplyPing(rows[i], ordered[i].ClientId);
			}
		}

		private void ApplyPing(Row row, ulong clientId)
		{
			SetText(row.Ping, roundManager.TryGetPing(clientId, out var pingMs) ? $"<color=#{ColorUtility.ToHtmlStringRGB(GetPingColor(pingMs))}>{pingMs} ms</color>" : "—");
		}

		private Color GetPingColor(int ping)
		{
			if (pingGradient == null)
			{
				return Color.white;
			}
			return pingGradient.Evaluate(Mathf.InverseLerp(50f, 300f, ping));
		}

		private void ApplyColumns()
		{
			if (!(appliedColumnsFor == roundManager))
			{
				appliedColumnsFor = roundManager;
				SetShown(roleHeader, roundManager.ShowsScoreboardRole);
				SetShown(healthHeader, roundManager.ShowsScoreboardHealth);
			}
		}

		private static void SetShown(GameObject target, bool shown)
		{
			if (target != null && target.activeSelf != shown)
			{
				target.SetActive(shown);
			}
		}

		private void ApplyReveal(Row row, ulong clientId)
		{
			bool showsScoreboardRole = roundManager.ShowsScoreboardRole;
			bool showsScoreboardHealth = roundManager.ShowsScoreboardHealth;
			if (row.Role != null && row.Role.gameObject.activeSelf != showsScoreboardRole)
			{
				row.Role.gameObject.SetActive(showsScoreboardRole);
			}
			if (row.Health != null && row.Health.gameObject.activeSelf != showsScoreboardHealth)
			{
				row.Health.gameObject.SetActive(showsScoreboardHealth);
			}
			if (!showsScoreboardRole && !showsScoreboardHealth)
			{
				return;
			}
			foreach (PlayerRevealEntry reveal in roundManager.Reveals)
			{
				if (reveal.ClientId == clientId)
				{
					SetText(row.Role, (reveal.Role == PlayerRole.Hunter) ? Loc.Get("Role.Hunter") : ((reveal.Role == PlayerRole.Hider) ? Loc.Get("Role.Modeler") : "-"));
					SetText(row.Health, reveal.Health.ToString());
					return;
				}
			}
			SetText(row.Role, "");
			SetText(row.Health, "");
		}

		private void ApplyAvatar(Row row, ulong steamId)
		{
			if (!(row.Avatar == null))
			{
				if (steamId == 0L || !SteamManager.IsInitialized)
				{
					row.AvatarSteamId = 0uL;
					row.Avatar.enabled = false;
				}
				else if (row.AvatarSteamId == steamId && row.Avatar.sprite != null)
				{
					row.Avatar.enabled = true;
				}
				else
				{
					row.AvatarSteamId = steamId;
					row.Avatar.enabled = false;
					LoadAvatar(row, steamId);
				}
			}
		}

		private async void LoadAvatar(Row row, ulong steamId)
		{
			Texture2D texture2D = await SteamManager.FetchAvatarAsync(steamId);
			if (!(texture2D == null) && !(row.Avatar == null) && row.AvatarSteamId == steamId)
			{
				row.Avatar.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
				row.Avatar.enabled = true;
			}
		}

		private int ScoreOf(ulong clientId)
		{
			foreach (PlayerScoreEntry score in roundManager.Scores)
			{
				if (score.ClientId == clientId)
				{
					return score.Score;
				}
			}
			return 0;
		}
	}
}
