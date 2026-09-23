using System.Collections.Generic;
using System.Text;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class DeathmatchHudView : MonoBehaviour
	{
		[Tooltip("Aşama satırı: ısınma / geri sayım / maç bitti. Boşsa kendisi oluşturur.")]
		[SerializeField]
		private TextMeshProUGUI stageLabel;

		[Tooltip("Kalan süre (mm:ss). Boşsa kendisi oluşturur.")]
		[SerializeField]
		private TextMeshProUGUI timerLabel;

		[Tooltip("Kendi öldürme sayın / skor limiti. Boşsa kendisi oluşturur.")]
		[SerializeField]
		private TextMeshProUGUI scoreLabel;

		[Tooltip("Lider ve puanı. Boşsa kendisi oluşturur.")]
		[SerializeField]
		private TextMeshProUGUI leaderLabel;

		[Tooltip("Ölüyken yeniden doğuş geri sayımı. Boşsa kendisi oluşturur.")]
		[SerializeField]
		private TextMeshProUGUI respawnLabel;

		[Tooltip("Kendi canın. Boşsa sol altta kendisi oluşturur.")]
		[SerializeField]
		private TextMeshProUGUI healthLabel;

		[Tooltip("Oyuncuların öldürme sıralaması - '1. Ali  12' satırları, sen sarı. Boşsa sağ üstte kendisi oluşturur. Rich Text açık olmalı.")]
		[SerializeField]
		private TextMeshProUGUI standingsLabel;

		[Tooltip("Sıralamada gösterilecek en fazla oyuncu. Sen bu sayının dışında kalırsan altına ayrıca eklenirsin.")]
		[SerializeField]
		[Min(1f)]
		private int standingsRows = 5;

		private DeathmatchRoundManager mode;

		private bool built;

		private bool standingsDirty = true;

		private bool standingsSubscribed;

		private string standingsText = "";

		private readonly List<(ulong ClientId, int Score)> standings = new List<(ulong, int)>();

		private DeathmatchRoundManager.Stage standingsStage;

		private PlayerHealth localHealth;

		private NetworkManager NetworkManager => NetworkManager.Singleton;

		private void Update()
		{
			if (mode == null)
			{
				mode = GameModeController.Current as DeathmatchRoundManager;
				if (mode == null)
				{
					return;
				}
			}
			EnsureBuilt();
			double num = ((NetworkManager != null) ? NetworkManager.ServerTime.Time : 0.0);
			DeathmatchRoundManager.Stage value = mode.CurrentStage.Value;
			Set(stageLabel, StageText(value));
			Set(timerLabel, (value == DeathmatchRoundManager.Stage.Playing && mode.TimeLimitSeconds.Value > 0) ? Clock(mode.PhaseEndServerTime.Value - num) : "");
			Set(scoreLabel, (value == DeathmatchRoundManager.Stage.Playing || value == DeathmatchRoundManager.Stage.MatchEnd) ? string.Format(Loc.Get("Deathmatch.Kills"), OwnScore(), mode.ScoreLimit.Value) : "");
			Set(leaderLabel, LeaderText(value));
			Set(respawnLabel, mode.IsLocalDead ? string.Format(Loc.Get("Deathmatch.Respawn"), Mathf.Max(0, Mathf.CeilToInt((float)(mode.LocalRespawnAtServerTime - num)))) : "");
			UpdateStandings(value);
			UpdateHealth();
		}

		private void UpdateStandings(DeathmatchRoundManager.Stage stage)
		{
			if (standingsLabel == null)
			{
				return;
			}
			if (!standingsSubscribed && mode.IsSpawned)
			{
				standingsSubscribed = true;
				mode.Scores.OnListChanged += OnStandingsChanged;
				mode.Names.OnListChanged += OnNamesChanged;
			}
			if (stage != standingsStage)
			{
				standingsStage = stage;
				standingsDirty = true;
			}
			if (stage != DeathmatchRoundManager.Stage.Playing && stage != DeathmatchRoundManager.Stage.MatchEnd)
			{
				Set(standingsLabel, "");
				return;
			}
			if (standingsDirty)
			{
				standingsDirty = false;
				standingsText = BuildStandings();
			}
			Set(standingsLabel, standingsText);
		}

		private void OnStandingsChanged(NetworkListEvent<PlayerScoreEntry> _)
		{
			standingsDirty = true;
		}

		private void OnNamesChanged(NetworkListEvent<PlayerNameEntry> _)
		{
			standingsDirty = true;
		}

		private void OnDestroy()
		{
			if (standingsSubscribed && !(mode == null))
			{
				mode.Scores.OnListChanged -= OnStandingsChanged;
				mode.Names.OnListChanged -= OnNamesChanged;
			}
		}

		private string BuildStandings()
		{
			standings.Clear();
			for (int i = 0; i < mode.Names.Count; i++)
			{
				ulong clientId = mode.Names[i].ClientId;
				standings.Add((clientId, ScoreOf(clientId)));
			}
			standings.Sort(((ulong ClientId, int Score) a, (ulong ClientId, int Score) b) => b.Score.CompareTo(a.Score));
			ulong num = ((NetworkManager != null) ? NetworkManager.LocalClientId : ulong.MaxValue);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			for (int num2 = 0; num2 < standings.Count && num2 < standingsRows; num2++)
			{
				AppendStanding(stringBuilder, num2, standings[num2], num);
				flag |= standings[num2].ClientId == num;
			}
			if (!flag)
			{
				for (int num3 = standingsRows; num3 < standings.Count; num3++)
				{
					if (standings[num3].ClientId == num)
					{
						stringBuilder.Append("...\n");
						AppendStanding(stringBuilder, num3, standings[num3], num);
						break;
					}
				}
			}
			return stringBuilder.ToString().TrimEnd('\n');
		}

		private void AppendStanding(StringBuilder text, int index, (ulong ClientId, int Score) entry, ulong self)
		{
			string text2 = $"{index + 1}. {mode.GetPlayerName(entry.ClientId)}  <b>{entry.Score}</b>";
			text.Append((entry.ClientId == self) ? ("<color=yellow>" + text2 + "</color>") : text2).Append('\n');
		}

		private int ScoreOf(ulong clientId)
		{
			for (int i = 0; i < mode.Scores.Count; i++)
			{
				if (mode.Scores[i].ClientId == clientId)
				{
					return mode.Scores[i].Score;
				}
			}
			return 0;
		}

		private void UpdateHealth()
		{
			if (healthLabel == null)
			{
				return;
			}
			if (localHealth == null)
			{
				NetworkManager networkManager = NetworkManager;
				if (networkManager != null && networkManager.LocalClient != null && networkManager.LocalClient.PlayerObject != null)
				{
					localHealth = networkManager.LocalClient.PlayerObject.GetComponent<PlayerHealth>();
				}
			}
			if (localHealth == null)
			{
				Set(healthLabel, "");
				return;
			}
			int value = localHealth.Health.Value;
			Set(healthLabel, $"{value}");
			healthLabel.color = ((value > 60) ? new Color(0.45f, 0.9f, 0.45f) : ((value > 30) ? new Color(0.95f, 0.75f, 0.3f) : new Color(0.95f, 0.35f, 0.3f)));
		}

		private string StageText(DeathmatchRoundManager.Stage stage)
		{
			double num = ((NetworkManager != null) ? NetworkManager.ServerTime.Time : 0.0);
			return stage switch
			{
				DeathmatchRoundManager.Stage.Warmup => string.Format(Loc.Get("Deathmatch.Warmup"), mode.PlayerCount.Value, mode.MinPlayers.Value), 
				DeathmatchRoundManager.Stage.Countdown => string.Format(Loc.Get("Deathmatch.Starting"), Mathf.Max(0, Mathf.CeilToInt((float)(mode.PhaseEndServerTime.Value - num)))), 
				DeathmatchRoundManager.Stage.MatchEnd => Loc.Get("Deathmatch.MatchOver"), 
				_ => "", 
			};
		}

		private string LeaderText(DeathmatchRoundManager.Stage stage)
		{
			switch (stage)
			{
			case DeathmatchRoundManager.Stage.MatchEnd:
				if (mode.Winner.Value != ulong.MaxValue)
				{
					return string.Format(Loc.Get("Deathmatch.Winner"), mode.GetPlayerName(mode.Winner.Value));
				}
				return Loc.Get("Deathmatch.Draw");
			default:
				return "";
			case DeathmatchRoundManager.Stage.Playing:
			{
				ulong num = ulong.MaxValue;
				int num2 = 0;
				for (int i = 0; i < mode.Scores.Count; i++)
				{
					if (mode.Scores[i].Score > num2)
					{
						num2 = mode.Scores[i].Score;
						num = mode.Scores[i].ClientId;
					}
				}
				if (num != ulong.MaxValue)
				{
					return string.Format(Loc.Get("Deathmatch.Leader"), mode.GetPlayerName(num), num2);
				}
				return "";
			}
			}
		}

		private int OwnScore()
		{
			if (NetworkManager == null)
			{
				return 0;
			}
			ulong localClientId = NetworkManager.LocalClientId;
			for (int i = 0; i < mode.Scores.Count; i++)
			{
				if (mode.Scores[i].ClientId == localClientId)
				{
					return mode.Scores[i].Score;
				}
			}
			return 0;
		}

		private static string Clock(double seconds)
		{
			int num = Mathf.Max(0, Mathf.CeilToInt((float)seconds));
			return $"{num / 60}:{num % 60:00}";
		}

		private static void Set(TextMeshProUGUI label, string text)
		{
			if (!(label == null))
			{
				if (label.text != text)
				{
					label.text = text;
				}
				bool flag = !string.IsNullOrEmpty(text);
				if (label.gameObject.activeSelf != flag)
				{
					label.gameObject.SetActive(flag);
				}
			}
		}

		private void EnsureBuilt()
		{
			if (built)
			{
				return;
			}
			built = true;
			if (healthLabel == null)
			{
				RectTransform rectTransform = UIFactory.CreateRect(base.transform, "DeathmatchHealth");
				rectTransform.anchorMin = new Vector2(0f, 0f);
				rectTransform.anchorMax = new Vector2(0f, 0f);
				rectTransform.pivot = new Vector2(0f, 0f);
				rectTransform.anchoredPosition = new Vector2(24f, 24f);
				rectTransform.sizeDelta = new Vector2(200f, 44f);
				healthLabel = UIFactory.CreateLabel(rectTransform, "Health", "", 34);
				healthLabel.alignment = TextAlignmentOptions.BottomLeft;
				healthLabel.raycastTarget = false;
				((RectTransform)healthLabel.transform).sizeDelta = new Vector2(200f, 44f);
			}
			if (standingsLabel == null)
			{
				RectTransform rectTransform2 = UIFactory.CreateRect(base.transform, "DeathmatchStandings");
				rectTransform2.anchorMin = new Vector2(1f, 1f);
				rectTransform2.anchorMax = new Vector2(1f, 1f);
				rectTransform2.pivot = new Vector2(1f, 1f);
				rectTransform2.anchoredPosition = new Vector2(-24f, -24f);
				rectTransform2.sizeDelta = new Vector2(320f, 220f);
				standingsLabel = UIFactory.CreateLabel(rectTransform2, "Standings", "", 18);
				standingsLabel.alignment = TextAlignmentOptions.TopRight;
				standingsLabel.richText = true;
				standingsLabel.raycastTarget = false;
				((RectTransform)standingsLabel.transform).sizeDelta = new Vector2(320f, 220f);
			}
			if (!(stageLabel != null) || !(timerLabel != null) || !(scoreLabel != null) || !(leaderLabel != null) || !(respawnLabel != null))
			{
				RectTransform rectTransform3 = UIFactory.CreateRect(base.transform, "DeathmatchHud");
				rectTransform3.anchorMin = new Vector2(0.5f, 1f);
				rectTransform3.anchorMax = new Vector2(0.5f, 1f);
				rectTransform3.pivot = new Vector2(0.5f, 1f);
				rectTransform3.anchoredPosition = new Vector2(0f, -16f);
				rectTransform3.sizeDelta = new Vector2(640f, 160f);
				VerticalLayoutGroup verticalLayoutGroup = rectTransform3.gameObject.AddComponent<VerticalLayoutGroup>();
				verticalLayoutGroup.spacing = 2f;
				verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
				verticalLayoutGroup.childControlWidth = false;
				verticalLayoutGroup.childControlHeight = false;
				verticalLayoutGroup.childForceExpandWidth = false;
				verticalLayoutGroup.childForceExpandHeight = false;
				if ((object)stageLabel == null)
				{
					stageLabel = Line(rectTransform3, "Stage", 22);
				}
				if ((object)timerLabel == null)
				{
					timerLabel = Line(rectTransform3, "Timer", 30);
				}
				if ((object)scoreLabel == null)
				{
					scoreLabel = Line(rectTransform3, "Score", 18);
				}
				if ((object)leaderLabel == null)
				{
					leaderLabel = Line(rectTransform3, "Leader", 16);
				}
				if ((object)respawnLabel == null)
				{
					respawnLabel = Line(rectTransform3, "Respawn", 24);
				}
			}
		}

		private static TextMeshProUGUI Line(Transform parent, string name, int size)
		{
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(parent, name, "", size);
			textMeshProUGUI.alignment = TextAlignmentOptions.Center;
			textMeshProUGUI.raycastTarget = false;
			((RectTransform)textMeshProUGUI.transform).sizeDelta = new Vector2(640f, (float)size + 10f);
			return textMeshProUGUI;
		}
	}
}
