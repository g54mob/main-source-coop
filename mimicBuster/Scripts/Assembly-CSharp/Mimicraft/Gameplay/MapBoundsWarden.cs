using System.Collections.Generic;
using Mimicraft.Networking;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class MapBoundsWarden : MonoBehaviour
	{
		private struct Foothold
		{
			public Vector3 Position;

			public Quaternion Rotation;

			public float At;

			public int Returns;
		}

		private const float CheckInterval = 1f;

		private const float GraceSeconds = 2f;

		private static MapBoundsWarden instance;

		private const float MemorySeconds = 15f;

		private const int MaxReturns = 3;

		private float nextCheckTime;

		private readonly Dictionary<ulong, float> outsideSince = new Dictionary<ulong, float>();

		private readonly Dictionary<ulong, Foothold> footholds = new Dictionary<ulong, Foothold>();

		private readonly List<ulong> departed = new List<ulong>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}

		public static void Ensure()
		{
			if (!(instance != null))
			{
				GameObject obj = new GameObject("MapBoundsWarden")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				instance = obj.AddComponent<MapBoundsWarden>();
				Object.DontDestroyOnLoad(obj);
			}
		}

		private void Update()
		{
			if (Time.unscaledTime < nextCheckTime)
			{
				return;
			}
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsServer)
			{
				return;
			}
			nextCheckTime = Time.unscaledTime + 1f;
			GameModeController current = GameModeController.Current;
			bool anyBaked = MapBounds.AnyBaked;
			foreach (NetworkClient connectedClients in singleton.ConnectedClientsList)
			{
				if (connectedClients.PlayerObject == null)
				{
					continue;
				}
				bool allowOutside = current != null && current.AllowsOutsidePlayArea(connectedClients.ClientId);
				PlayerVoxelBody.PlacementVerdict placementVerdict = CheckBody(connectedClients, allowOutside);
				if (placementVerdict == PlayerVoxelBody.PlacementVerdict.SendBack)
				{
					outsideSince.Remove(connectedClients.ClientId);
					SendBack(connectedClients, connectedClients.PlayerObject.transform.position);
					continue;
				}
				if (!anyBaked)
				{
					if (placementVerdict == PlayerVoxelBody.PlacementVerdict.Clear)
					{
						Remember(connectedClients);
					}
					continue;
				}
				Vector3 vector = MapBounds.SamplePointFor(connectedClients.PlayerObject.transform);
				if (MapBounds.IsInsidePlayArea(vector))
				{
					outsideSince.Remove(connectedClients.ClientId);
					if (placementVerdict == PlayerVoxelBody.PlacementVerdict.Clear)
					{
						Remember(connectedClients);
					}
					continue;
				}
				if (current != null && current.AllowsOutsidePlayArea(connectedClients.ClientId))
				{
					outsideSince.Remove(connectedClients.ClientId);
					continue;
				}
				float value;
				float num = (outsideSince.TryGetValue(connectedClients.ClientId, out value) ? (value + 1f) : 0f);
				outsideSince[connectedClients.ClientId] = num;
				if (!(num < 2f))
				{
					outsideSince.Remove(connectedClients.ClientId);
					SendBack(connectedClients, vector);
				}
			}
			Prune(singleton);
		}

		private static PlayerVoxelBody.PlacementVerdict CheckBody(NetworkClient client, bool allowOutside)
		{
			PlayerVoxelBody component = client.PlayerObject.GetComponent<PlayerVoxelBody>();
			if (!(component != null))
			{
				return PlayerVoxelBody.PlacementVerdict.Clear;
			}
			return component.ServerCheckPlacement(1f, allowOutside);
		}

		private void Remember(NetworkClient client)
		{
			PlayerMovement component = client.PlayerObject.GetComponent<PlayerMovement>();
			if (!(component == null) && component.IsSupported)
			{
				Transform transform = client.PlayerObject.transform;
				footholds.TryGetValue(client.ClientId, out var value);
				bool flag = (value.Position - transform.position).sqrMagnitude > 4f;
				footholds[client.ClientId] = new Foothold
				{
					Position = transform.position,
					Rotation = transform.rotation,
					At = Time.unscaledTime,
					Returns = ((!flag) ? value.Returns : 0)
				};
			}
		}

		private void Prune(NetworkManager manager)
		{
			departed.Clear();
			foreach (ulong key in footholds.Keys)
			{
				if (!manager.ConnectedClients.ContainsKey(key))
				{
					departed.Add(key);
				}
			}
			foreach (ulong item in departed)
			{
				footholds.Remove(item);
				outsideSince.Remove(item);
			}
		}

		private void SendBack(NetworkClient client, Vector3 sample)
		{
			GameModeController current = GameModeController.Current;
			PlayerMovement component = client.PlayerObject.GetComponent<PlayerMovement>();
			if (!(current == null) && !(component == null))
			{
				Vector3 position;
				Quaternion rotation;
				if (footholds.TryGetValue(client.ClientId, out var value) && Time.unscaledTime - value.At <= 15f && value.Returns < 3)
				{
					value.Returns++;
					footholds[client.ClientId] = value;
					Debug.LogWarning($"[MapBounds] {client.ClientId} oyun alani disindaydi ya da govdesi " + $"geometrinin icindeydi ({sample}) - son bastigi yere geri alindi ({value.Position}).");
					component.TeleportClientRpc(value.Position, value.Rotation);
				}
				else if (current.TryGetRescueSpawn(client.ClientId, out position, out rotation))
				{
					footholds.Remove(client.ClientId);
					Debug.LogWarning($"[MapBounds] {client.ClientId} oyun alani disindaydi ({sample}) - guvenilir " + "bir son konum yok, spawn noktasina alindi.");
					component.TeleportClientRpc(position, rotation);
				}
			}
		}
	}
}
