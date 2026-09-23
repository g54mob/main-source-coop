using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class RoundHudView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI phaseLabel;

		[SerializeField]
		private TextMeshProUGUI roleLabel;

		[SerializeField]
		private TextMeshProUGUI huntersLabel;

		[SerializeField]
		private TextMeshProUGUI hidersLabel;

		[SerializeField]
		private RectTransform huntersLabelPanel;

		[SerializeField]
		private RectTransform hidersLabelPanel;

		[SerializeField]
		private Button readyButton;

		[SerializeField]
		private GameObject healthChip;

		[SerializeField]
		private TextMeshProUGUI healthLabel;

		[SerializeField]
		private GameObject scoreCard;

		[SerializeField]
		private TextMeshProUGUI scoreLabel;

		[SerializeField]
		private RoundManager roundManager;

		private bool hasRequestedReady;

		private RoundPhase lastPhase = (RoundPhase)(-1);

		private PlayerRole lastRole = (PlayerRole)(-1);

		private int lastHunters = -1;

		private int lastHiders = -1;

		private bool lastSurvivorsVisible;

		private int lastSeconds = -1;

		private int lastHealth = int.MinValue;

		private bool lastReadyVisible;

		private bool appliedOnce;

		private bool scoresDirty = true;

		private bool appliedShown = true;

		private NetworkList<PlayerScoreEntry> subscribedScores;

		private NetworkList<PlayerNameEntry> subscribedNames;

		private NetworkList<PlayerRevealEntry> subscribedReveals;

		private readonly List<(ulong ClientId, int Score)> scoreRows = new List<(ulong, int)>();

		private PlayerHealth localHealth;

		public static bool Shown { get; set; } = true;

		public static RoundHudView Create(Transform parent, RoundManager roundManager)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "RoundHud");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "StatusCard");
			rectTransform2.anchorMin = new Vector2(0.5f, 1f);
			rectTransform2.anchorMax = new Vector2(0.5f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.anchoredPosition = new Vector2(0f, -24f);
			rectTransform2.sizeDelta = new Vector2(280f, 94f);
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
			image.raycastTarget = false;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform2, "PhaseLabel", "", 22);
			RectTransform obj = (RectTransform)textMeshProUGUI.transform;
			obj.anchorMin = new Vector2(0f, 1f);
			obj.anchorMax = new Vector2(1f, 1f);
			obj.pivot = new Vector2(0.5f, 1f);
			obj.anchoredPosition = new Vector2(0f, -8f);
			obj.sizeDelta = new Vector2(0f, 30f);
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform2, "RoleLabel", "", 15);
			RectTransform obj2 = (RectTransform)textMeshProUGUI2.transform;
			obj2.anchorMin = new Vector2(0f, 1f);
			obj2.anchorMax = new Vector2(1f, 1f);
			obj2.pivot = new Vector2(0.5f, 1f);
			obj2.anchoredPosition = new Vector2(0f, -40f);
			obj2.sizeDelta = new Vector2(0f, 22f);
			TextMeshProUGUI textMeshProUGUI3 = UIFactory.CreateLabel(rectTransform2, "SurvivorsLabel", "");
			RectTransform obj3 = (RectTransform)textMeshProUGUI3.transform;
			obj3.anchorMin = new Vector2(0f, 1f);
			obj3.anchorMax = new Vector2(1f, 1f);
			obj3.pivot = new Vector2(0.5f, 1f);
			obj3.anchoredPosition = new Vector2(0f, -64f);
			obj3.sizeDelta = new Vector2(0f, 20f);
			textMeshProUGUI3.color = new Color(0.78f, 0.78f, 0.78f);
			TextMeshProUGUI text;
			Button button = UIFactory.CreateButton(rectTransform, "ReadyButton", Loc.Get("Overlay.Ready"), out text);
			RectTransform obj4 = (RectTransform)button.transform;
			obj4.anchorMin = new Vector2(0.5f, 1f);
			obj4.anchorMax = new Vector2(0.5f, 1f);
			obj4.pivot = new Vector2(0.5f, 1f);
			obj4.anchoredPosition = new Vector2(0f, -104f);
			obj4.sizeDelta = new Vector2(150f, 34f);
			GameObject chipObject;
			TextMeshProUGUI textMeshProUGUI4 = CreateHealthLabel(rectTransform, out chipObject);
			GameObject card;
			TextMeshProUGUI textMeshProUGUI5 = CreateScoreCard(rectTransform, out card);
			RoundHudView roundHudView = rectTransform.gameObject.AddComponent<RoundHudView>();
			roundHudView.phaseLabel = textMeshProUGUI;
			roundHudView.roleLabel = textMeshProUGUI2;
			roundHudView.readyButton = button;
			roundHudView.healthChip = chipObject;
			roundHudView.healthLabel = textMeshProUGUI4;
			roundHudView.scoreCard = card;
			roundHudView.scoreLabel = textMeshProUGUI5;
			roundHudView.roundManager = roundManager;
			return roundHudView;
		}

		private static TextMeshProUGUI CreateHealthLabel(Transform parent, out GameObject chipObject)
		{
			Image image = UIFactory.CreatePanel(parent, "HealthChip", new Color(0.1f, 0.1f, 0.1f, 0.8f));
			image.raycastTarget = false;
			chipObject = image.gameObject;
			RectTransform obj = (RectTransform)image.transform;
			obj.anchorMin = Vector2.zero;
			obj.anchorMax = Vector2.zero;
			obj.pivot = Vector2.zero;
			obj.anchoredPosition = new Vector2(24f, 24f);
			obj.sizeDelta = new Vector2(140f, 36f);
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(image.transform, "HealthLabel", "", 16);
			RectTransform obj2 = (RectTransform)textMeshProUGUI.transform;
			obj2.anchorMin = Vector2.zero;
			obj2.anchorMax = Vector2.one;
			obj2.offsetMin = Vector2.zero;
			obj2.offsetMax = Vector2.zero;
			return textMeshProUGUI;
		}

		private static TextMeshProUGUI CreateScoreCard(Transform parent, out GameObject card)
		{
			Image image = UIFactory.CreatePanel(parent, "ScoreCard", new Color(0.1f, 0.1f, 0.1f, 0.8f));
			image.raycastTarget = false;
			RectTransform obj = (RectTransform)image.transform;
			obj.anchorMin = new Vector2(1f, 1f);
			obj.anchorMax = new Vector2(1f, 1f);
			obj.pivot = new Vector2(1f, 1f);
			obj.anchoredPosition = new Vector2(-24f, -24f);
			obj.sizeDelta = new Vector2(200f, 170f);
			card = image.gameObject;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(image.transform, "ScoreLabel", "");
			textMeshProUGUI.alignment = TextAlignmentOptions.TopLeft;
			textMeshProUGUI.textWrappingMode = TextWrappingModes.NoWrap;
			RectTransform obj2 = (RectTransform)textMeshProUGUI.transform;
			obj2.anchorMin = Vector2.zero;
			obj2.anchorMax = Vector2.one;
			obj2.offsetMin = new Vector2(12f, 10f);
			obj2.offsetMax = new Vector2(-12f, -10f);
			return textMeshProUGUI;
		}

		public void SetRoundManager(RoundManager roundManager)
		{
			this.roundManager = roundManager;
		}

		private void Awake()
		{
			readyButton.onClick.RemoveAllListeners();
			readyButton.onClick.AddListener(RequestReady);
			LocalizedText.AttachIfUnset(readyButton.GetComponentInChildren<TMP_Text>(includeInactive: true), "Overlay.Ready");
		}

		private void OnEnable()
		{
			Loc.Changed += OnLanguageChanged;
		}

		private void OnDisable()
		{
			Loc.Changed -= OnLanguageChanged;
			UnsubscribeScores();
		}

		private void OnLanguageChanged()
		{
			appliedOnce = false;
			scoresDirty = true;
		}

		private void Update()
		{
			if (roundManager == null)
			{
				roundManager = GameModeController.Current as RoundManager;
			}
			if (roundManager == null || NetworkManager.Singleton == null)
			{
				return;
			}
			SubscribeScores();
			RoundPhase value = roundManager.CurrentPhase.Value;
			PlayerRole localRole = roundManager.LocalRole;
			if (value != RoundPhase.Prep)
			{
				hasRequestedReady = false;
			}
			int num = SecondsRemaining(value);
			if (!appliedOnce || value != lastPhase || num != lastSeconds)
			{
				phaseLabel.text = BuildPhaseText(value, num);
				lastPhase = value;
				lastSeconds = num;
			}
			if (!appliedOnce || localRole != lastRole)
			{
				TextMeshProUGUI textMeshProUGUI = roleLabel;
				textMeshProUGUI.text = localRole switch
				{
					PlayerRole.Hunter => Loc.Get("Role.Hunter"), 
					PlayerRole.Hider => Loc.Get("Role.Modeler"), 
					_ => "", 
				};
				lastRole = localRole;
			}
			int value2 = roundManager.AliveHunters.Value;
			int value3 = roundManager.AliveHiders.Value;
			bool flag = value != RoundPhase.WaitingForPlayers;
			if (!appliedOnce || value2 != lastHunters || value3 != lastHiders || flag != lastSurvivorsVisible)
			{
				if ((bool)huntersLabel)
				{
					huntersLabel.text = (flag ? $"{value2}" : "");
				}
				if ((bool)hidersLabel)
				{
					hidersLabel.text = (flag ? $"{value3}" : "");
				}
				if ((bool)huntersLabelPanel)
				{
					huntersLabelPanel.gameObject.SetActive(flag);
				}
				if ((bool)hidersLabelPanel)
				{
					hidersLabelPanel.gameObject.SetActive(flag);
				}
				lastHunters = value2;
				lastHiders = value3;
				lastSurvivorsVisible = flag;
			}
			bool flag2 = value == RoundPhase.Prep && localRole == PlayerRole.Hider && !hasRequestedReady;
			if (!appliedOnce || flag2 != lastReadyVisible)
			{
				readyButton.gameObject.SetActive(flag2);
				lastReadyVisible = flag2;
			}
			int num2 = ResolveHealth();
			if (!appliedOnce || num2 != lastHealth)
			{
				SetActiveIfChanged(healthChip, num2 >= 0);
				if (num2 >= 0)
				{
					healthLabel.SetText("{0}", num2);
				}
				TextMeshProUGUI textMeshProUGUI2 = healthLabel;
				Color color = ((num2 >= 50) ? ((num2 < 75) ? Color.yellow : Color.green) : ((num2 < 25) ? Color.red : Color.orangeRed));
				textMeshProUGUI2.color = color;
				lastHealth = num2;
			}
			if (appliedShown != Shown)
			{
				appliedShown = Shown;
				scoresDirty = true;
			}
			if (scoresDirty)
			{
				string text = BuildScoreText();
				scoreLabel.text = text;
				SetActiveIfChanged(scoreCard, Shown && text.Length > 0);
				scoresDirty = false;
			}
			appliedOnce = true;
		}

		private static void SetActiveIfChanged(GameObject target, bool active)
		{
			if (target != null && target.activeSelf != active)
			{
				target.SetActive(active);
			}
		}

		private void SubscribeScores()
		{
			if (subscribedScores != roundManager.Scores || subscribedNames != roundManager.Names || subscribedReveals != roundManager.Reveals)
			{
				UnsubscribeScores();
				subscribedScores = roundManager.Scores;
				if (subscribedScores != null)
				{
					subscribedScores.OnListChanged += OnScoresChanged;
				}
				subscribedNames = roundManager.Names;
				if (subscribedNames != null)
				{
					subscribedNames.OnListChanged += OnNamesChanged;
				}
				subscribedReveals = roundManager.Reveals;
				if (subscribedReveals != null)
				{
					subscribedReveals.OnListChanged += OnRevealsChanged;
				}
				scoresDirty = true;
			}
		}

		private void UnsubscribeScores()
		{
			if (subscribedScores != null)
			{
				subscribedScores.OnListChanged -= OnScoresChanged;
				subscribedScores = null;
			}
			if (subscribedNames != null)
			{
				subscribedNames.OnListChanged -= OnNamesChanged;
				subscribedNames = null;
			}
			if (subscribedReveals != null)
			{
				subscribedReveals.OnListChanged -= OnRevealsChanged;
				subscribedReveals = null;
			}
		}

		private void OnScoresChanged(NetworkListEvent<PlayerScoreEntry> change)
		{
			scoresDirty = true;
		}

		private void OnNamesChanged(NetworkListEvent<PlayerNameEntry> change)
		{
			scoresDirty = true;
		}

		private void OnRevealsChanged(NetworkListEvent<PlayerRevealEntry> change)
		{
			scoresDirty = true;
		}

		private int ResolveHealth()
		{
			if (localHealth == null)
			{
				NetworkObject networkObject = NetworkManager.Singleton.LocalClient?.PlayerObject;
				localHealth = ((networkObject != null) ? networkObject.GetComponent<PlayerHealth>() : null);
			}
			if (!(localHealth != null))
			{
				return -1;
			}
			return localHealth.Health.Value;
		}

		private int SecondsRemaining(RoundPhase phase)
		{
			if (phase == RoundPhase.WaitingForPlayers)
			{
				return -1;
			}
			double num = Math.Max(NetworkManager.Singleton.ServerTime.Time, roundManager.CinematicEndServerTime.Value);
			double num2 = roundManager.PhaseEndServerTime.Value - num;
			return Mathf.Max(0, Mathf.CeilToInt((float)num2));
		}

		private string BuildScoreText()
		{
			if (roundManager.Names.Count == 0)
			{
				return "";
			}
			scoreRows.Clear();
			foreach (PlayerNameEntry name in roundManager.Names)
			{
				scoreRows.Add((name.ClientId, ScoreOf(name.ClientId)));
			}
			scoreRows.Sort(((ulong ClientId, int Score) a, (ulong ClientId, int Score) b) => b.Score.CompareTo(a.Score));
			ulong localClientId = NetworkManager.Singleton.LocalClientId;
			string text = GameInput.DisplayFor("Common/Scoreboard");
			StringBuilder stringBuilder = new StringBuilder(Loc.Get("Scoreboard.Title"));
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("  <color=yellow><size=90%>[" + text + "]</size></color>");
			}
			foreach (var (num, num2) in scoreRows)
			{
				stringBuilder.Append('\n');
				stringBuilder.Append($"{roundManager.GetPlayerName(num)}: {num2}");
				if (num == localClientId)
				{
					stringBuilder.Append(" <color=yellow>" + Loc.Get("Scoreboard.You") + "</color>");
				}
			}
			return stringBuilder.ToString();
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

		private static string BuildPhaseText(RoundPhase phase, int seconds)
		{
			string text;
			switch (phase)
			{
			case RoundPhase.WaitingForPlayers:
				return "<size=18>" + Loc.Get("Phase.WaitingForPlayers") + "</size>";
			case RoundPhase.Prep:
				text = Loc.Get("Phase.Preparation");
				break;
			case RoundPhase.Hunt:
				text = Loc.Get("Phase.Hunt");
				break;
			case RoundPhase.RoundEnd:
				text = Loc.Get("Phase.RoundEnd");
				break;
			default:
				text = phase.ToString();
				break;
			}
			string text2 = text;
			return "<size=18>" + text2 + "</size>\n" + FormatCountdown(seconds);
		}

		private static string FormatCountdown(int seconds)
		{
			if (seconds < 0)
			{
				seconds = 0;
			}
			return $"{seconds / 60:00}:{seconds % 60:00}";
		}

		private void RequestReady()
		{
			hasRequestedReady = true;
			roundManager.RequestReadyServerRpc();
		}
	}
}
