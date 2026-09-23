using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mimicraft.Localization;
using Mimicraft.UI;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Networking
{
	public class SteamManager : MonoBehaviour
	{
		private const uint AppId = 5231380u;

		private const float ReinitRetryIntervalSeconds = 2f;

		private static SteamManager instance;

		private static bool steamEverInitialized;

		private float nextReinitAttemptTime;

		private static Lobby? activeLobby;

		public const string ConnectArgument = "+connect_lobby";

		private static readonly Dictionary<ulong, Texture2D> avatarCache = new Dictionary<ulong, Texture2D>();

		private static readonly Dictionary<ulong, Sprite> avatarSprites = new Dictionary<ulong, Sprite>();

		private bool inviteEventsHooked;

		public static bool IsInitialized => SteamClient.IsValid;

		public static SteamId LocalSteamId => SteamClient.SteamId;

		public static string LocalName => SteamClient.Name;

		public static Lobby? ActiveLobby
		{
			get
			{
				return activeLobby;
			}
			set
			{
				activeLobby = value;
				AnnounceLobby(value);
			}
		}

		private static void AnnounceLobby(Lobby? lobby)
		{
			if (!SteamClient.IsValid)
			{
				return;
			}
			try
			{
				if (lobby.HasValue)
				{
					SteamFriends.SetRichPresence("connect", "+connect_lobby " + lobby.Value.Id.ToString());
					SteamFriends.SetRichPresence("steam_player_group", lobby.Value.Id.ToString());
				}
				else
				{
					SteamFriends.SetRichPresence("connect", null);
					SteamFriends.SetRichPresence("steam_player_group", null);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[Steam] Zengin durum yazilamadi: " + ex.Message);
			}
		}

		public static bool TryParseConnectLobby(string connect, out ulong lobbyId)
		{
			lobbyId = 0uL;
			if (string.IsNullOrEmpty(connect))
			{
				return false;
			}
			string[] array = connect.Trim().Split(' ');
			if (array.Length >= 2 && array[0] == "+connect_lobby")
			{
				return ulong.TryParse(array[1], out lobbyId);
			}
			return false;
		}

		public static Task<Texture2D> FetchLocalAvatarAsync()
		{
			return FetchAvatarAsync(LocalSteamId);
		}

		public static async Task<Texture2D> FetchAvatarAsync(SteamId steamId)
		{
			if (!IsInitialized || steamId.Value == 0L)
			{
				return null;
			}
			if (avatarCache.TryGetValue(steamId.Value, out var value))
			{
				return value;
			}
			Texture2D texture = null;
			try
			{
				Image? image = await SteamFriends.GetMediumAvatarAsync(steamId);
				if (image.HasValue)
				{
					texture = ToTexture(image.Value);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning($"Steam avatarı alınamadı ({steamId}): {ex.Message}");
			}
			avatarCache[steamId.Value] = texture;
			return texture;
		}

		public static async Task<Sprite> FetchAvatarSpriteAsync(SteamId steamId)
		{
			if (avatarSprites.TryGetValue(steamId.Value, out var value))
			{
				return value;
			}
			Texture2D texture2D = await FetchAvatarAsync(steamId);
			if (avatarSprites.TryGetValue(steamId.Value, out value))
			{
				return value;
			}
			Sprite sprite = ((texture2D == null) ? null : Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f)));
			avatarSprites[steamId.Value] = sprite;
			return sprite;
		}

		private static Texture2D ToTexture(Image image)
		{
			Texture2D texture2D = new Texture2D((int)image.Width, (int)image.Height, TextureFormat.RGBA32, mipChain: false);
			texture2D.LoadRawTextureData(FlipRows(image.Data, (int)image.Width, (int)image.Height));
			texture2D.Apply();
			return texture2D;
		}

		private static byte[] FlipRows(byte[] source, int width, int height)
		{
			byte[] array = new byte[source.Length];
			int num = width * 4;
			for (int i = 0; i < height; i++)
			{
				Buffer.BlockCopy(source, i * num, array, (height - 1 - i) * num, num);
			}
			return array;
		}

		public static void LeaveActiveLobby()
		{
			if (ActiveLobby.HasValue)
			{
				if (SteamClient.IsValid)
				{
					ActiveLobby.Value.Leave();
				}
				ActiveLobby = null;
			}
		}

		private void Awake()
		{
			if (instance != null && instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			TryInitialize();
		}

		private void Start()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i + 1 < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == "+connect_lobby" && ulong.TryParse(commandLineArgs[i + 1], out var result))
				{
					JoinInvited(new Lobby(result));
					break;
				}
			}
		}

		private static void HookInviteEvents()
		{
			SteamFriends.OnGameLobbyJoinRequested -= OnLobbyJoinRequested;
			SteamFriends.OnGameLobbyJoinRequested += OnLobbyJoinRequested;
			SteamFriends.OnGameRichPresenceJoinRequested -= OnRichPresenceJoinRequested;
			SteamFriends.OnGameRichPresenceJoinRequested += OnRichPresenceJoinRequested;
		}

		private static void OnLobbyJoinRequested(Lobby lobby, SteamId invitedBy)
		{
			if (instance != null)
			{
				instance.JoinInvited(lobby);
			}
		}

		private static void OnRichPresenceJoinRequested(Friend friend, string connect)
		{
			if (instance != null && TryParseConnectLobby(connect, out var lobbyId))
			{
				instance.JoinInvited(new Lobby(lobbyId));
			}
		}

		private void JoinInvited(Lobby lobby)
		{
			StartCoroutine(JoinInvitedRoutine(lobby));
		}

		private IEnumerator JoinInvitedRoutine(Lobby lobby)
		{
			float deadline = Time.unscaledTime + 20f;
			while (NetworkManager.Singleton == null && Time.unscaledTime < deadline)
			{
				yield return null;
			}
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null)
			{
				Report(Loc.Get("Status.NetworkManagerMissing"));
				yield break;
			}
			if (singleton.IsListening)
			{
				Report(Loc.Get("Invite.LeaveFirst"));
				yield break;
			}
			if (lobby.GetData("has_password") == "1")
			{
				Report(Loc.Get("Invite.Locked"));
				yield break;
			}
			LobbyPassword.Offer(singleton, "");
			SteamLobbyJoin.JoinAsync(lobby, SceneManager.GetActiveScene(), Report);
		}

		private static void Report(string message)
		{
			if (ToastView.Instance != null)
			{
				ToastView.Instance.Show(message);
			}
			Debug.Log("[Steam] " + message);
		}

		private void OnDestroy()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		private void Update()
		{
			if (steamEverInitialized && !SteamClient.IsValid && !(Time.unscaledTime < nextReinitAttemptTime))
			{
				nextReinitAttemptTime = Time.unscaledTime + 2f;
				TryInitialize();
			}
		}

		private static void TryInitialize()
		{
			if (!SteamClient.IsValid)
			{
				try
				{
					SteamClient.Init(5231380u);
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Steam initialize edilemedi: " + ex.Message);
				}
				steamEverInitialized |= SteamClient.IsValid;
				if (SteamClient.IsValid)
				{
					HookInviteEvents();
					AnnounceLobby(activeLobby);
				}
				if (SteamClient.IsValid)
				{
					SteamPing.Prepare();
				}
			}
		}

		private void OnApplicationQuit()
		{
			if (SteamClient.IsValid)
			{
				SteamClient.Shutdown();
			}
		}
	}
}
