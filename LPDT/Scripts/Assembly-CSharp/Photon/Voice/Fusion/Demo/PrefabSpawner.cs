using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Photon.Voice.Fusion.Demo
{
	public class PrefabSpawner : MonoBehaviour, INetworkRunnerCallbacks, IPublicFacingInterface
	{
		[SerializeField]
		private NetworkObject prefab;

		private Dictionary<PlayerRef, NetworkObject> spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();

		[SerializeField]
		private bool debugLogs;

		void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
		{
			if (debugLogs)
			{
				Debug.Log($"OnPlayerJoined {player} mode = {runner.GameMode}");
			}
			GameMode gameMode = runner.GameMode;
			if (gameMode == GameMode.Single || (uint)(gameMode - 3) <= 1u)
			{
				SpawnPlayer(runner, player);
			}
		}

		void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
		{
			if (debugLogs)
			{
				Debug.Log($"OnPlayerLeft {player} mode = {runner.GameMode}");
			}
			GameMode gameMode = runner.GameMode;
			if (gameMode == GameMode.Single || (uint)(gameMode - 3) <= 1u)
			{
				TryDespawnPlayer(runner, player);
			}
		}

		void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
		{
		}

		void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
		{
		}

		void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
		{
			if (!debugLogs)
			{
				return;
			}
			Debug.Log($"OnShutdown mode = {runner.GameMode} reason = {shutdownReason}");
			foreach (KeyValuePair<PlayerRef, NetworkObject> spawnedPlayer in spawnedPlayers)
			{
				Debug.LogWarning($"Prefab not despawned? {spawnedPlayer.Key}:{spawnedPlayer.Value?.Id}");
			}
		}

		void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
		{
			if (debugLogs)
			{
				Debug.Log($"OnConnectedToServer mode = {runner.GameMode}");
			}
			if (runner.GameMode == GameMode.Shared)
			{
				SpawnPlayer(runner, runner.LocalPlayer);
			}
		}

		void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
		{
			if (debugLogs)
			{
				Debug.Log($"OnDisconnectedFromServer mode = {runner.GameMode}");
			}
			if (runner.GameMode == GameMode.Shared)
			{
				TryDespawnPlayer(runner, runner.LocalPlayer);
			}
		}

		void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
		{
		}

		void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
		{
		}

		void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
		{
		}

		void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
		{
		}

		void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
		{
		}

		void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
		{
		}

		void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
		{
		}

		void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
		{
		}

		void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
		{
		}

		public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
		}

		public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
		}

		void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey reliableKey, float progress)
		{
		}

		private void SpawnPlayer(NetworkRunner runner, PlayerRef player)
		{
			NetworkObject networkObject = runner.Spawn(prefab, Vector3.zero, Quaternion.identity, player);
			if (debugLogs)
			{
				if (spawnedPlayers.TryGetValue(player, out var value))
				{
					Debug.LogWarning($"Replacing NO {value?.Id} w/ {networkObject?.Id} for {player}");
				}
				else
				{
					Debug.Log($"Spawned NO {networkObject?.Id} for {player}");
				}
			}
			spawnedPlayers[player] = networkObject;
		}

		private bool TryDespawnPlayer(NetworkRunner runner, PlayerRef player)
		{
			if (spawnedPlayers.TryGetValue(player, out var value))
			{
				if (debugLogs)
				{
					Debug.Log($"Despawning NO {value?.Id} for {player}");
				}
				runner.Despawn(value);
				return spawnedPlayers.Remove(player);
			}
			if (debugLogs)
			{
				Debug.LogWarning($"No spawned NO found for player {player}");
			}
			return false;
		}
	}
}
