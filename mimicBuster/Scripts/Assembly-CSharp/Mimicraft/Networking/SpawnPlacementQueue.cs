using System.Collections.Generic;
using Mimicraft.Gameplay;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public class SpawnPlacementQueue : MonoBehaviour
	{
		private const float RetryInterval = 0.25f;

		private const float GiveUpSeconds = 30f;

		private static SpawnPlacementQueue instance;

		private static readonly Dictionary<ulong, float> waiting = new Dictionary<ulong, float>();

		private float nextRetryTime;

		private readonly List<ulong> finished = new List<ulong>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
			waiting.Clear();
		}

		public static void NoteUnplaced(ulong clientId)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			if (!(singleton == null) && singleton.IsServer)
			{
				if (!waiting.ContainsKey(clientId))
				{
					waiting[clientId] = Time.unscaledTime + 30f;
				}
				Ensure();
			}
		}

		public static void Forget(ulong clientId)
		{
			waiting.Remove(clientId);
		}

		private static void Ensure()
		{
			if (!(instance != null))
			{
				GameObject obj = new GameObject("SpawnPlacementQueue")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				instance = obj.AddComponent<SpawnPlacementQueue>();
				Object.DontDestroyOnLoad(obj);
			}
		}

		private void Update()
		{
			if (waiting.Count == 0 || Time.unscaledTime < nextRetryTime)
			{
				return;
			}
			nextRetryTime = Time.unscaledTime + 0.25f;
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsServer)
			{
				waiting.Clear();
				return;
			}
			GameModeController current = GameModeController.Current;
			finished.Clear();
			foreach (KeyValuePair<ulong, float> item in waiting)
			{
				ulong key = item.Key;
				if (!singleton.ConnectedClients.TryGetValue(key, out var value) || value.PlayerObject == null)
				{
					if (!singleton.ConnectedClients.ContainsKey(key))
					{
						finished.Add(key);
					}
					continue;
				}
				if (current == null || !current.TryGetLobbySpawn(out var position, out var rotation))
				{
					if (Time.unscaledTime >= item.Value)
					{
						Debug.LogWarning($"[SpawnPlacementQueue] {key} icin {30f:0} saniyedir " + "spawn noktasi yok - oyuncu bulundugu yerde birakiliyor. Modun sahnesinde spawn marker'i var mi?");
						finished.Add(key);
					}
					continue;
				}
				PlayerMovement component = value.PlayerObject.GetComponent<PlayerMovement>();
				if (component == null)
				{
					finished.Add(key);
					continue;
				}
				component.TeleportClientRpc(position, rotation);
				finished.Add(key);
				Debug.Log($"[SpawnPlacementQueue] {key} dogdugunda spawn noktasi yoktu - " + $"mod hazir olunca {position} noktasina alindi.");
			}
			foreach (ulong item2 in finished)
			{
				waiting.Remove(item2);
			}
		}
	}
}
