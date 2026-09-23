using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class GarticHudView : MonoBehaviour
	{
		[Tooltip("Kalan süre - \"1:23\". Süresi olmayan fazlarda (oyuncu beklerken, oyun bitince) boşalır.")]
		[SerializeField]
		private TextMeshProUGUI timerLabel;

		[Tooltip("Fazın adı ve sırası kimdeyse onun ismi.")]
		[SerializeField]
		private TextMeshProUGUI phaseLabel;

		[Tooltip("İsteğe bağlı ilerleme çubuğu - Image'ın Type'ı Filled olmalı. Fazın başında dolu, sonunda boş.")]
		[SerializeField]
		private Image timerFill;

		[Tooltip("Sürenin normal rengi.")]
		[SerializeField]
		private Color normalColor = Color.white;

		[Tooltip("Son saniyelerin rengi.")]
		[SerializeField]
		private Color lowTimeColor = new Color(0.95f, 0.35f, 0.3f);

		[Tooltip("Kaç saniye kalınca renk değişsin.")]
		[SerializeField]
		[Min(0f)]
		private int lowTimeSeconds = 10;

		private GarticRoundManager mode;

		private int appliedSeconds = int.MinValue;

		private string appliedPhaseText;

		private void Update()
		{
			if (mode == null)
			{
				mode = GameModeController.Current as GarticRoundManager;
			}
			if (mode == null || NetworkManager.Singleton == null)
			{
				Apply(-1, "");
			}
			else
			{
				Apply(SecondsRemaining(), BuildPhaseText());
			}
		}

		private int SecondsRemaining()
		{
			if (mode.PhaseEndServerTime.Value <= 0.0)
			{
				return -1;
			}
			double num = mode.PhaseEndServerTime.Value - NetworkManager.Singleton.ServerTime.Time;
			return Mathf.Max(0, Mathf.CeilToInt((float)num));
		}

		private string BuildPhaseText()
		{
			string playerName = GetPlayerName(mode.CurrentModeller.Value);
			return mode.Phase.Value switch
			{
				GarticPhase.WaitingForPlayers => Loc.Get("WaitingForPlayers"), 
				GarticPhase.WordSelect => Loc.Format("ChoosingWord", playerName), 
				GarticPhase.Drawing => Loc.Format("Turn", playerName), 
				GarticPhase.TurnEnd => Loc.Get("RoundOver"), 
				GarticPhase.GameOver => Loc.Get("GameOver"), 
				_ => "", 
			};
		}

		private string GetPlayerName(ulong clientId)
		{
			return mode.GetPlayerName(clientId);
		}

		private void Apply(int seconds, string phaseText)
		{
			if (seconds != appliedSeconds)
			{
				appliedSeconds = seconds;
				ApplyTimer(seconds);
			}
			if (phaseText != appliedPhaseText)
			{
				appliedPhaseText = phaseText;
				if (phaseLabel != null)
				{
					phaseLabel.text = phaseText;
				}
			}
		}

		private void ApplyTimer(int seconds)
		{
			if (timerLabel != null)
			{
				timerLabel.text = ((seconds < 0) ? "" : $"{seconds / 60}:{seconds % 60:00}");
				timerLabel.color = ((seconds >= 0 && seconds <= lowTimeSeconds) ? lowTimeColor : normalColor);
			}
			if (!(timerFill == null))
			{
				float num = PhaseLengthSeconds();
				bool flag = seconds >= 0 && num > 0.01f;
				timerFill.fillAmount = (flag ? Mathf.Clamp01((float)seconds / num) : 0f);
				if (timerFill.enabled != flag)
				{
					timerFill.enabled = flag;
				}
			}
		}

		private float PhaseLengthSeconds()
		{
			return mode.Phase.Value switch
			{
				GarticPhase.WordSelect => 15f, 
				GarticPhase.Drawing => Mathf.Max(60, mode.TurnSeconds.Value), 
				GarticPhase.TurnEnd => 6f, 
				_ => 0f, 
			};
		}
	}
}
