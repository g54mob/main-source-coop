using System;
using System.Collections.Generic;
using Mimicraft.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class GarticPlayerListView : MonoBehaviour
	{
		private sealed class Row
		{
			public GameObject Root;

			public TextMeshProUGUI Name;

			public TextMeshProUGUI Score;

			public Image Background;
		}

		private static readonly Color ModellerColor = new Color(0.55f, 0.85f, 1f);

		private static readonly Color GuessedColor = new Color(0.45f, 0.85f, 0.45f);

		private static readonly Color NormalColor = new Color(0.9f, 0.9f, 0.9f);

		[Tooltip("INAKTİF bir satır şablonu. İçinde tam olarak şu adlarla çocuklar olmalı: Name (TextMeshProUGUI), Score (TextMeshProUGUI). Klonlar şablonun yanına eklenir.")]
		[SerializeField]
		private GameObject rowTemplate;

		[Tooltip("Boş bırakılırsa sahnede aranır.")]
		[SerializeField]
		private GarticRoundManager mode;

		private readonly List<Row> rows = new List<Row>();

		private readonly List<PlayerScoreEntry> ordered = new List<PlayerScoreEntry>();

		private GarticRoundManager subscribed;

		private bool dirty = true;

		private void Update()
		{
			if (mode == null)
			{
				mode = GameModeController.Current as GarticRoundManager;
			}
			if (mode == null || rowTemplate == null || !mode.IsSpawned)
			{
				Unsubscribe();
				SetRowCount(0);
				return;
			}
			Subscribe();
			if (dirty)
			{
				dirty = false;
				BuildOrdered();
				SetRowCount(ordered.Count);
				for (int i = 0; i < ordered.Count; i++)
				{
					ApplyRow(rows[i], ordered[i], i);
				}
			}
		}

		private void OnDestroy()
		{
			Unsubscribe();
		}

		private void Subscribe()
		{
			if ((object)subscribed != mode)
			{
				Unsubscribe();
				subscribed = mode;
				mode.Names.OnListChanged += OnNamesChanged;
				mode.Scores.OnListChanged += OnScoresChanged;
				NetworkVariable<GarticPhase> phase = mode.Phase;
				phase.OnValueChanged = (NetworkVariable<GarticPhase>.OnValueChangedDelegate)Delegate.Combine(phase.OnValueChanged, new NetworkVariable<GarticPhase>.OnValueChangedDelegate(OnPhaseChanged));
				NetworkVariable<ulong> currentModeller = mode.CurrentModeller;
				currentModeller.OnValueChanged = (NetworkVariable<ulong>.OnValueChangedDelegate)Delegate.Combine(currentModeller.OnValueChanged, new NetworkVariable<ulong>.OnValueChangedDelegate(OnModellerChanged));
				dirty = true;
			}
		}

		private void Unsubscribe()
		{
			if ((object)subscribed != null)
			{
				subscribed.Names.OnListChanged -= OnNamesChanged;
				subscribed.Scores.OnListChanged -= OnScoresChanged;
				NetworkVariable<GarticPhase> phase = subscribed.Phase;
				phase.OnValueChanged = (NetworkVariable<GarticPhase>.OnValueChangedDelegate)Delegate.Remove(phase.OnValueChanged, new NetworkVariable<GarticPhase>.OnValueChangedDelegate(OnPhaseChanged));
				NetworkVariable<ulong> currentModeller = subscribed.CurrentModeller;
				currentModeller.OnValueChanged = (NetworkVariable<ulong>.OnValueChangedDelegate)Delegate.Remove(currentModeller.OnValueChanged, new NetworkVariable<ulong>.OnValueChangedDelegate(OnModellerChanged));
				subscribed = null;
				dirty = true;
			}
		}

		private void OnNamesChanged(NetworkListEvent<PlayerNameEntry> change)
		{
			dirty = true;
		}

		private void OnScoresChanged(NetworkListEvent<PlayerScoreEntry> change)
		{
			dirty = true;
		}

		private void OnPhaseChanged(GarticPhase previous, GarticPhase current)
		{
			dirty = true;
		}

		private void OnModellerChanged(ulong previous, ulong current)
		{
			dirty = true;
		}

		private void BuildOrdered()
		{
			ordered.Clear();
			foreach (PlayerNameEntry name in mode.Names)
			{
				int score = 0;
				foreach (PlayerScoreEntry score2 in mode.Scores)
				{
					if (score2.ClientId == name.ClientId)
					{
						score = score2.Score;
						break;
					}
				}
				ordered.Add(new PlayerScoreEntry
				{
					ClientId = name.ClientId,
					Score = score
				});
			}
			ordered.Sort((PlayerScoreEntry a, PlayerScoreEntry b) => b.Score.CompareTo(a.Score));
		}

		private void ApplyRow(Row row, PlayerScoreEntry entry, int rank)
		{
			bool flag = mode.Phase.Value != GarticPhase.WaitingForPlayers && entry.ClientId == mode.CurrentModeller.Value;
			if (row.Name != null)
			{
				string text = (flag ? "✎ " : $"{rank + 1}. ");
				row.Name.text = text + mode.GetPlayerName(entry.ClientId);
				row.Name.color = (flag ? ModellerColor : NormalColor);
			}
			if (row.Score != null)
			{
				row.Score.SetText("{0}", entry.Score);
			}
			if (row.Background != null)
			{
				bool flag2 = NetworkManager.Singleton != null && entry.ClientId == NetworkManager.Singleton.LocalClientId;
				row.Background.color = (flag2 ? new Color(GuessedColor.r, GuessedColor.g, GuessedColor.b, 0.18f) : new Color(0f, 0f, 0f, 0.25f));
			}
		}

		private void SetRowCount(int count)
		{
			bool flag = false;
			while (rows.Count < count)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(rowTemplate, rowTemplate.transform.parent);
				gameObject.name = $"PlayerRow{rows.Count}";
				gameObject.SetActive(value: true);
				Transform transform = gameObject.transform.Find("Name");
				Transform transform2 = gameObject.transform.Find("Score");
				rows.Add(new Row
				{
					Root = gameObject,
					Name = ((transform != null) ? transform.GetComponent<TextMeshProUGUI>() : null),
					Score = ((transform2 != null) ? transform2.GetComponent<TextMeshProUGUI>() : null),
					Background = gameObject.GetComponent<Image>()
				});
				flag = true;
			}
			for (int i = 0; i < rows.Count; i++)
			{
				bool flag2 = i < count;
				if (rows[i].Root.activeSelf != flag2)
				{
					rows[i].Root.SetActive(flag2);
					flag = true;
				}
			}
			if (flag && rowTemplate.transform.parent is RectTransform layoutRoot)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
			}
		}
	}
}
