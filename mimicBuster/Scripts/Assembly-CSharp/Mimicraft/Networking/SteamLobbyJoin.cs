using System;
using System.Threading.Tasks;
using Mimicraft.Localization;
using Mimicraft.UI;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Networking
{
	public static class SteamLobbyJoin
	{
		public const float JoinLobbySeconds = 10f;

		public static async Task<bool> JoinAsync(Lobby lobby, Scene keepScene, Action<string> report)
		{
			NetworkManager manager = NetworkManager.Singleton;
			if (manager == null)
			{
				report?.Invoke(Loc.Get("Status.NetworkManagerMissing"));
				return false;
			}
			FacepunchTransport transport = manager.GetComponent<FacepunchTransport>();
			if (transport == null)
			{
				report?.Invoke(Loc.Get("Status.FacepunchMissing"));
				return false;
			}
			report?.Invoke(Loc.Get("Status.JoiningLobby"));
			Lobby? joined = await JoinLobbyWithTimeout(lobby.Id);
			if (!joined.HasValue)
			{
				report?.Invoke(Loc.Get("Status.LobbyJoinFailed"));
				return false;
			}
			if (!GameVersion.CanJoin(joined.Value.GetData("version"), out var refusal))
			{
				joined.Value.Leave();
				report?.Invoke(refusal);
				GameVersion.ShowRefusal(refusal);
				return false;
			}
			SteamManager.ActiveLobby = joined.Value;
			manager.NetworkConfig.NetworkTransport = transport;
			transport.targetSteamId = joined.Value.Owner.Id;
			report?.Invoke(Loc.Get("Status.Connecting"));
			LoadingScreen.Show("Loading.Connecting");
			await StageScenes(keepScene);
			ServerTickRate.ApplyForJoining(manager, joined.Value.GetData("tickrate"));
			LobbyPassword.Prepare(manager);
			manager.StartClient();
			ConnectWatchdog.Watch(manager);
			return true;
		}

		public static async Task<Lobby?> JoinLobbyWithTimeout(SteamId lobbyId)
		{
			Task<Lobby?> join = SteamMatchmaking.JoinLobbyAsync(lobbyId);
			if (await Task.WhenAny(join, Task.Delay(TimeSpan.FromSeconds(10.0))) == join)
			{
				return join.Result;
			}
			Debug.LogWarning($"[SteamLobbyJoin] Steam {10f:0} saniyede lobiye almadi - vazgeciliyor.");
			LeaveWhenJoined(join);
			return null;
		}

		private static async void LeaveWhenJoined(Task<Lobby?> join)
		{
			Lobby? lobby = await join;
			if (lobby.HasValue && SteamClient.IsValid)
			{
				lobby.Value.Leave();
			}
		}

		public static async Task StageScenes(Scene keep)
		{
			if (keep.IsValid() && keep.isLoaded && SceneManager.GetActiveScene() != keep)
			{
				SceneManager.SetActiveScene(keep);
			}
			await SessionScenes.ReleaseExtras(keep);
		}
	}
}
