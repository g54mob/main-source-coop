using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Networking
{
	public class DuelArena
	{
		public const int PointsNeeded = 2;

		private const double RestSeconds = 3.0;

		private const double CorpseSeconds = 2.0;

		private readonly RoundManager round;

		private readonly List<ulong> queue = new List<ulong>();

		private readonly Dictionary<ulong, ulong> partners = new Dictionary<ulong, ulong>();

		private readonly Dictionary<ulong, double> resting = new Dictionary<ulong, double>();

		private readonly Dictionary<ulong, double> fallen = new Dictionary<ulong, double>();

		private readonly HashSet<ulong> duelKills = new HashSet<ulong>();

		private Transform[] points = Array.Empty<Transform>();

		private readonly List<ulong> due = new List<ulong>();

		public bool HasArena
		{
			get
			{
				if (points != null)
				{
					return points.Length >= 2;
				}
				return false;
			}
		}

		public DuelArena(RoundManager round)
		{
			this.round = round;
		}

		public bool IsFighting(ulong clientId)
		{
			return partners.ContainsKey(clientId);
		}

		public bool IsFallen(ulong clientId)
		{
			return fallen.ContainsKey(clientId);
		}

		public bool ArePartners(ulong a, ulong b)
		{
			if (partners.TryGetValue(a, out var value))
			{
				return value == b;
			}
			return false;
		}

		public DuelState StateOf(ulong clientId)
		{
			if (partners.ContainsKey(clientId))
			{
				return DuelState.Fighting;
			}
			if (!queue.Contains(clientId) && !resting.ContainsKey(clientId))
			{
				return DuelState.None;
			}
			return DuelState.Queued;
		}

		public void SetSpawnPoints(Transform[] value)
		{
			points = value ?? Array.Empty<Transform>();
			if (!HasArena)
			{
				EndAll();
			}
		}

		public void SetQueued(ulong clientId, bool join)
		{
			if (!join)
			{
				if (partners.TryGetValue(clientId, out var value))
				{
					End(clientId);
					resting.Remove(clientId);
					round.NotifyDuel(clientId, DuelState.None, "", "Duel.Cancelled");
					round.NotifyDuel(value, StateOf(value), "", "Duel.OpponentLeft");
				}
				else if (queue.Remove(clientId) | resting.Remove(clientId))
				{
					round.NotifyDuel(clientId, DuelState.None, "", "");
				}
			}
			else if (!partners.ContainsKey(clientId) && !queue.Contains(clientId) && !resting.ContainsKey(clientId))
			{
				if (round.CurrentPhase.Value != RoundPhase.Prep || round.GetServerRole(clientId) != PlayerRole.Hunter)
				{
					round.NotifyDuel(clientId, DuelState.None, "", "Duel.Unavailable");
					return;
				}
				if (!HasArena)
				{
					round.NotifyDuel(clientId, DuelState.None, "", "Duel.NoArena");
					return;
				}
				queue.Add(clientId);
				round.NotifyDuel(clientId, DuelState.Queued, "", "");
				Tick();
			}
		}

		public void Tick()
		{
			if (round.CurrentPhase.Value != RoundPhase.Prep)
			{
				EndAll();
				return;
			}
			DropIneligible();
			CollectFallen();
			AdmitRested();
			while (HasArena && queue.Count >= 2)
			{
				int index = UnityEngine.Random.Range(0, queue.Count);
				ulong a = queue[index];
				queue.RemoveAt(index);
				int index2 = UnityEngine.Random.Range(0, queue.Count);
				ulong b = queue[index2];
				queue.RemoveAt(index2);
				Start(a, b);
			}
		}

		private void CollectFallen()
		{
			if (fallen.Count == 0)
			{
				return;
			}
			double timeAsDouble = Time.timeAsDouble;
			due.Clear();
			foreach (KeyValuePair<ulong, double> item in fallen)
			{
				if (item.Value <= timeAsDouble)
				{
					due.Add(item.Key);
				}
			}
			foreach (ulong item2 in due)
			{
				End(item2);
			}
		}

		private void AdmitRested()
		{
			if (resting.Count == 0)
			{
				return;
			}
			double timeAsDouble = Time.timeAsDouble;
			due.Clear();
			foreach (KeyValuePair<ulong, double> item in resting)
			{
				if (item.Value <= timeAsDouble)
				{
					due.Add(item.Key);
				}
			}
			foreach (ulong item2 in due)
			{
				resting.Remove(item2);
				if (round.HasConnectedPlayer(item2) && round.GetServerRole(item2) == PlayerRole.Hunter)
				{
					queue.Add(item2);
				}
				else
				{
					round.NotifyDuel(item2, DuelState.None, "", "");
				}
			}
		}

		private void DropIneligible()
		{
			for (int num = queue.Count - 1; num >= 0; num--)
			{
				ulong clientId = queue[num];
				if (!round.HasConnectedPlayer(clientId) || round.GetServerRole(clientId) != PlayerRole.Hunter)
				{
					queue.RemoveAt(num);
				}
			}
			if (resting.Count == 0)
			{
				return;
			}
			due.Clear();
			foreach (KeyValuePair<ulong, double> item in resting)
			{
				if (!round.HasConnectedPlayer(item.Key) || round.GetServerRole(item.Key) != PlayerRole.Hunter)
				{
					due.Add(item.Key);
				}
			}
			foreach (ulong item2 in due)
			{
				resting.Remove(item2);
			}
		}

		private void Start(ulong a, ulong b)
		{
			partners[a] = b;
			partners[b] = a;
			round.ServerSetVisibleOnlyTo(a, b);
			round.ServerSetVisibleOnlyTo(b, a);
			round.ServerHealPlayer(a);
			round.ServerHealPlayer(b);
			round.ServerTeleport(a, points[0]);
			round.ServerTeleport(b, points[1]);
			round.NotifyDuel(a, DuelState.Fighting, round.GetPlayerName(b), "Duel.Started");
			round.NotifyDuel(b, DuelState.Fighting, round.GetPlayerName(a), "Duel.Started");
		}

		public void End(ulong clientId, bool requeue = true)
		{
			if (partners.TryGetValue(clientId, out var value))
			{
				partners.Remove(clientId);
				partners.Remove(value);
				fallen.Remove(clientId);
				fallen.Remove(value);
				Finish(clientId, requeue);
				Finish(value, requeue);
			}
		}

		private void Finish(ulong clientId, bool requeue)
		{
			round.ServerRestoreVisibility(clientId);
			if (round.HasConnectedPlayer(clientId))
			{
				round.ServerClearRagdoll(clientId);
				round.ServerHealPlayer(clientId);
				Transform transform = round.NextHunterRoomPoint();
				if (transform != null)
				{
					round.ServerTeleport(clientId, transform);
				}
				if (requeue && round.GetServerRole(clientId) == PlayerRole.Hunter)
				{
					resting[clientId] = Time.timeAsDouble + 3.0;
					round.NotifyDuel(clientId, DuelState.Queued, "", "");
				}
				else
				{
					round.NotifyDuel(clientId, DuelState.None, "", "");
				}
			}
		}

		public void EndAll()
		{
			if (partners.Count > 0)
			{
				foreach (ulong item in new List<ulong>(partners.Keys))
				{
					End(item, requeue: false);
				}
				fallen.Clear();
			}
			if (queue.Count == 0 && resting.Count == 0)
			{
				return;
			}
			List<ulong> list = new List<ulong>(queue);
			list.AddRange(resting.Keys);
			queue.Clear();
			resting.Clear();
			foreach (ulong item2 in list)
			{
				round.NotifyDuel(item2, DuelState.None, "", "");
			}
		}

		public void OnDisconnected(ulong clientId)
		{
			queue.Remove(clientId);
			resting.Remove(clientId);
			if (partners.TryGetValue(clientId, out var value))
			{
				End(clientId);
				round.NotifyDuel(value, StateOf(value), "", "Duel.OpponentLeft");
			}
		}

		public bool OnPlayerDied(ulong clientId)
		{
			if (!partners.TryGetValue(clientId, out var value) || fallen.ContainsKey(clientId))
			{
				return false;
			}
			duelKills.Add(clientId);
			round.ServerRecordDuelWin(value);
			round.ServerBeginDeathRagdoll(clientId);
			fallen[clientId] = Time.timeAsDouble + 2.0;
			round.NotifyDuel(value, DuelState.Fighting, round.GetPlayerName(clientId), "Duel.Won");
			round.NotifyDuel(clientId, DuelState.Fighting, round.GetPlayerName(value), "Duel.Lost");
			return true;
		}

		public bool ConsumeDuelKill(ulong victimId)
		{
			return duelKills.Remove(victimId);
		}
	}
}
