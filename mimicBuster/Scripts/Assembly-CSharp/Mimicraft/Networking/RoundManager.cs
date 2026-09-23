using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Analytics;
using Mimicraft.Customization;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.UI;
using Mimicraft.Voice;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class RoundManager : GameModeController
	{
		public struct HunterSplit
		{
			public int Players;

			public int ConfiguredPercent;

			public int Percent;

			public int Raw;

			public int Hunters;

			public bool Adjusted => Hunters != Raw;
		}

		private const int DefaultPrepSeconds = 60;

		private const int DefaultHuntSeconds = 300;

		private const int DefaultRoundEndSeconds = 10;

		private const int DefaultMinPlayers = 2;

		private const float HiderSurvivalScoreIntervalSeconds = 2f;

		private const int HiderSurvivalScore = 1;

		public const int DefaultTauntIntervalSeconds = 30;

		public const int MinTauntIntervalSeconds = 10;

		public const int MaxTauntIntervalSeconds = 120;

		[Header("Taunt")]
		[Tooltip("Aynı anda taunt atan Modelciler arasındaki gecikme (saniye). 0 = hepsi tam aynı anda.\n\nHepsi aynı anda çalınca yakın iki ses tek bir sese biniyor ve Avcı kaç kişi olduğunu, hangisinin nerede olduğunu ayıramıyor. Küçük bir aralık onları kulakta ayırır; uzun olması gerekmez, 0.25 sn yeter.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float tauntStaggerSeconds = 0.25f;

		[Tooltip("Sıralamanın toplamda yayılabileceği en uzun süre (saniye). Kalabalık bir lobide gecikme x kişi sayısı uzayıp anı bir dizi ayrı sese çevirmesin diye: sığmazsa aralık kısaltılır, kimse aynı ana denk getirilmez.")]
		[SerializeField]
		[Range(0.25f, 4f)]
		private float tauntStaggerSpreadSeconds = 1.5f;

		[Header("Görüş alanı bonusu")]
		[Tooltip("Bir Avcının görüş alanındaki Modelcinin hayatta kalma puanı bu kadar çarpılır. 1 = bonus yok. Modelciye burnunun dibinde saklanmayı ödüllendirir.")]
		[SerializeField]
		[Min(1f)]
		private float losScoreMultiplier = 2f;

		[Tooltip("Bu mesafeden uzaktaki bir Avcı 'görüyor' sayılmaz.")]
		[SerializeField]
		[Min(1f)]
		private float losMaxDistance = 30f;

		[Tooltip("Avcının bakış yönüyle Modelci arasındaki en büyük açı (derece). Ekranın yarı görüş açısına yakın tutulmalı.")]
		[SerializeField]
		[Range(5f, 90f)]
		private float losHalfAngle = 35f;

		[Header("Boyut dengesi")]
		[Tooltip("Modelcinin boyutunun hayatta kalma puanına etkisi. 0 = boyut puanı hiç etkilemez. 1 = puan boyutla birebir ölçeklenir (iki katı büyük bir model iki katı puan alır). Aradaki değerler etkiyi yumuşatır.")]
		[SerializeField]
		[Min(0f)]
		private float sizeScoreInfluence = 1f;

		[SerializeField]
		private LobbySettingsSync lobbySettings;

		[SerializeField]
		private Transform[] hunterSpawnPoints;

		[SerializeField]
		private Transform[] hiderSpawnPoints;

		[Tooltip("Where players stand during Bekleniyor, before roles exist. Optional - left empty, the Hider spawns are used instead, which is what happened before this existed.")]
		[SerializeField]
		private Transform[] lobbySpawnPoints;

		[Tooltip("The barrier sealing the Hunters' starting room during Hazırlık - active while they wait, deactivated when Av begins. Leave EMPTY on maps that have no such room: Hunters are then frozen in place for Hazırlık instead, which is the old behaviour.")]
		[SerializeField]
		private GameObject hunterDoor;

		public readonly NetworkVariable<int> AliveHunters = new NetworkVariable<int>(0);

		public readonly NetworkVariable<int> AliveHiders = new NetworkVariable<int>(0);

		private readonly NetworkVariable<PlayerRole> lastRoundWinner = new NetworkVariable<PlayerRole>(PlayerRole.None);

		public readonly NetworkVariable<double> CinematicEndServerTime = new NetworkVariable<double>(0.0);

		public const float CinematicFlySeconds = 0.6f;

		public const float CinematicOrbitSeconds = 1.5f;

		public readonly NetworkVariable<ulong> MvpClientId = new NetworkVariable<ulong>(ulong.MaxValue);

		public readonly NetworkVariable<MvpKind> MvpReason = new NetworkVariable<MvpKind>(MvpKind.None);

		public readonly NetworkVariable<int> MvpValue = new NetworkVariable<int>(0);

		private readonly Dictionary<ulong, int> damageDealt = new Dictionary<ulong, int>();

		private readonly Dictionary<ulong, PlayerRole> roles = new Dictionary<ulong, PlayerRole>();

		private readonly Dictionary<ulong, PlayerRole> roundStartRoles = new Dictionary<ulong, PlayerRole>();

		private readonly HashSet<ulong> readyHiderClientIds = new HashSet<ulong>();

		private readonly Dictionary<ulong, PlayerRole> rolePreferences = new Dictionary<ulong, PlayerRole>();

		private readonly Dictionary<ulong, PlayerRole> forcedRoles = new Dictionary<ulong, PlayerRole>();

		private double nextHiderScoreServerTime;

		private double nextTauntServerTime;

		private readonly HashSet<ulong> concealedNonParticipants = new HashSet<ulong>();

		private bool hidersConcealed;

		private bool subscribedToChat;

		public readonly NetworkList<ulong> HunterClientIds = new NetworkList<ulong>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

		private Transform[] hunterRoomSpawnPoints;

		private int hunterRoomSpawnIndex;

		private DuelArena duels;

		public readonly NetworkList<PlayerScoreEntry> DuelWins = new NetworkList<PlayerScoreEntry>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

		private int duelReturnSpawnIndex;

		private const int HitHealthRewardAmount = 25;

		private GameModeDefinition cachedDefinition;

		private string cachedDefinitionId;

		private readonly Dictionary<ulong, double> pendingConversion = new Dictionary<ulong, double>();

		private readonly List<ulong> conversionDue = new List<ulong>();

		private int conversionSpawnIndex;

		private const int TickCountdownSeconds = 5;

		private int lastTickedSecond = -1;

		private readonly Dictionary<ulong, double> pendingPlacement = new Dictionary<ulong, double>();

		private readonly List<ulong> placementDue = new List<ulong>();

		private const float PlacementGiveUpSeconds = 10f;

		private int latePlacementSpawnIndex;

		private readonly HashSet<ulong> readyClients = new HashSet<ulong>();

		private readonly Dictionary<ulong, int> hunterTurns = new Dictionary<ulong, int>();

		private readonly Dictionary<ulong, int> lastHunterRound = new Dictionary<ulong, int>();

		private int roundNumber;

		private int lobbySpawnIndex;

		private int rescueSpawnIndex;

		public const int HiderFoundScore = 50;

		private const float DeathRagdollSeconds = 3f;

		private readonly Dictionary<ulong, double> pendingConceal = new Dictionary<ulong, double>();

		private readonly List<ulong> concealDue = new List<ulong>();

		private readonly List<PlayerTaunt> taunting = new List<PlayerTaunt>();

		private readonly Dictionary<ulong, double> nextChatAllowedServerTime = new Dictionary<ulong, double>();

		private const double ChatCooldownSeconds = 0.5;

		private float TauntIntervalSeconds => ResolveTauntInterval(CurrentSettings().TauntIntervalSeconds);

		public bool HasHunterDoor => hunterDoor != null;

		public PlayerRole LocalRolePreference { get; private set; }

		private bool HasHunterRoom
		{
			get
			{
				if (hunterRoomSpawnPoints != null)
				{
					return hunterRoomSpawnPoints.Length != 0;
				}
				return false;
			}
		}

		private DuelArena Duels => duels ?? (duels = new DuelArena(this));

		public override bool CanLocalPlayerEditModel
		{
			get
			{
				if (base.LocalRole == PlayerRole.Hider)
				{
					if (CurrentPhase.Value != RoundPhase.Prep)
					{
						return CurrentPhase.Value == RoundPhase.Hunt;
					}
					return true;
				}
				return false;
			}
		}

		public override bool IsModelSetupWindow => CurrentPhase.Value == RoundPhase.Prep;

		public override bool LocalPlayerShouldHaveModel => base.LocalRole == PlayerRole.Hider;

		public override bool LocalPlayerUsesHunterMovement => base.LocalRole == PlayerRole.Hunter;

		public override bool IsLocalPlayerFrozen
		{
			get
			{
				if (base.LocalRole == PlayerRole.Hunter && CurrentPhase.Value == RoundPhase.Prep)
				{
					return !HasHunterDoor;
				}
				return false;
			}
		}

		public override bool IsLocalPlayerArmed => base.LocalRole == PlayerRole.Hunter;

		public override bool LocalPlayerMayWallClimb => base.LocalRole == PlayerRole.Hider;

		public override bool IsLocalPlayerParticipating
		{
			get
			{
				if (base.LocalRole == PlayerRole.None)
				{
					return CurrentPhase.Value == RoundPhase.WaitingForPlayers;
				}
				return true;
			}
		}

		public override bool UsesRolePreference => true;

		public override int HitHealthReward => 25;

		public override bool ShowsScoreboardRole => true;

		public override bool ShowsScoreboardHealth => true;

		private bool ConvertsFoundHiders
		{
			get
			{
				GameModeDefinition gameModeDefinition = DefinitionInPlay();
				if (gameModeDefinition != null)
				{
					return gameModeDefinition.ConvertsFoundHiders;
				}
				return false;
			}
		}

		private float HiderConversionSeconds
		{
			get
			{
				GameModeDefinition gameModeDefinition = DefinitionInPlay();
				if (!(gameModeDefinition != null))
				{
					return 5f;
				}
				return gameModeDefinition.HiderConversionSeconds;
			}
		}

		public override MapMarkerModes MarkerMode => MapMarkerModes.Standard;

		public override ulong HighlightedClientId
		{
			get
			{
				if (MvpReason.Value == MvpKind.None)
				{
					return ulong.MaxValue;
				}
				return MvpClientId.Value;
			}
		}

		public override bool SupportsHostRoundControls => true;

		public DuelState LocalDuelState { get; private set; }

		public string LocalDuelOpponent { get; private set; } = "";

		public event Action DuelChanged;

		public static int ClampTauntInterval(int seconds)
		{
			return Mathf.Clamp(seconds, 10, 120);
		}

		public static int ResolveTauntInterval(int configured)
		{
			if (configured >= 10)
			{
				if (configured <= 120)
				{
					return configured;
				}
				return 120;
			}
			return 30;
		}

		public override bool ShouldShowNameTag(ulong clientId)
		{
			if (CurrentPhase.Value != RoundPhase.WaitingForPlayers)
			{
				return IsKnownHunter(clientId);
			}
			return true;
		}

		public bool IsKnownHunter(ulong clientId)
		{
			foreach (ulong hunterClientId in HunterClientIds)
			{
				if (hunterClientId == clientId)
				{
					return true;
				}
			}
			return false;
		}

		public void SetLobbySettings(LobbySettingsSync lobbySettings)
		{
			this.lobbySettings = lobbySettings;
		}

		public PlayerRole GetServerRole(ulong clientId)
		{
			if (!base.IsServer || !roles.TryGetValue(clientId, out var value))
			{
				return PlayerRole.None;
			}
			return value;
		}

		public override bool ServerAllowsShooting(ulong clientId)
		{
			return GetServerRole(clientId) == PlayerRole.Hunter;
		}

		public override VoicePolicy VoicePolicyFor(ulong speaker, ulong listener, VoiceChannel channel)
		{
			if (!base.IsServer)
			{
				return VoicePolicy.Silent;
			}
			if (channel != VoiceChannel.Team && (Duels.IsFighting(speaker) || Duels.IsFighting(listener)) && !Duels.ArePartners(speaker, listener))
			{
				return VoicePolicy.Silent;
			}
			if (CurrentPhase.Value != RoundPhase.Hunt)
			{
				if (channel == VoiceChannel.Team && CurrentPhase.Value == RoundPhase.Prep)
				{
					if (!SameSide(speaker, listener))
					{
						return VoicePolicy.Silent;
					}
					return VoicePolicy.Lobby;
				}
				return VoicePolicy.Lobby;
			}
			if (IsOut(speaker))
			{
				if (!IsOut(listener))
				{
					return VoicePolicy.Silent;
				}
				return VoicePolicy.Lobby;
			}
			if (IsOut(listener))
			{
				return VoicePolicy.Lobby;
			}
			if (channel == VoiceChannel.Team)
			{
				if (!SameSide(speaker, listener))
				{
					return VoicePolicy.Silent;
				}
				return VoicePolicy.Lobby;
			}
			return VoicePolicy.Nearby;
		}

		private bool IsOut(ulong clientId)
		{
			return GetServerRole(clientId) == PlayerRole.None;
		}

		private bool SameSide(ulong a, ulong b)
		{
			if (!roundStartRoles.TryGetValue(a, out var value))
			{
				value = GetServerRole(a);
			}
			if (!roundStartRoles.TryGetValue(b, out var value2))
			{
				value2 = GetServerRole(b);
			}
			if (value != PlayerRole.None)
			{
				return value == value2;
			}
			return false;
		}

		public void SetSpawnPoints(Transform[] hunterSpawnPoints, Transform[] hiderSpawnPoints)
		{
			this.hunterSpawnPoints = hunterSpawnPoints;
			this.hiderSpawnPoints = hiderSpawnPoints;
		}

		public override void SetMap(Transform[] hunterSpawnPoints, Transform[] hiderSpawnPoints, Transform[] lobbySpawnPoints, GameObject hunterDoor)
		{
			SetSpawnPoints(hunterSpawnPoints, hiderSpawnPoints);
			this.lobbySpawnPoints = lobbySpawnPoints;
			this.hunterDoor = hunterDoor;
			ApplyHunterDoor(CurrentPhase.Value);
			PlaceWaitingPlayers();
		}

		public override void SetHunterRoom(Transform[] spawnPoints)
		{
			hunterRoomSpawnPoints = spawnPoints;
			hunterRoomSpawnIndex = 0;
			if (base.IsServer && CurrentPhase.Value == RoundPhase.Prep && HasHunterRoom)
			{
				MoveHunters(hunterRoomSpawnPoints, ref hunterRoomSpawnIndex);
			}
		}

		public override void SetArena(Transform[] arenaSpawnPoints)
		{
			Duels.SetSpawnPoints(arenaSpawnPoints);
		}

		internal void ServerRecordDuelWin(ulong clientId)
		{
			if (!base.IsServer)
			{
				return;
			}
			for (int i = 0; i < DuelWins.Count; i++)
			{
				if (DuelWins[i].ClientId == clientId)
				{
					DuelWins[i] = new PlayerScoreEntry
					{
						ClientId = clientId,
						Score = DuelWins[i].Score + 1
					};
					return;
				}
			}
			DuelWins.Add(new PlayerScoreEntry
			{
				ClientId = clientId,
				Score = 1
			});
		}

		private void RemoveDuelWins(ulong clientId)
		{
			for (int i = 0; i < DuelWins.Count; i++)
			{
				if (DuelWins[i].ClientId == clientId)
				{
					DuelWins.RemoveAt(i);
					break;
				}
			}
		}

		public bool ServerIsDuelling(ulong clientId)
		{
			if (base.IsServer)
			{
				return Duels.IsFighting(clientId);
			}
			return false;
		}

		internal bool HasConnectedPlayer(ulong clientId)
		{
			if (base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value))
			{
				return value.PlayerObject != null;
			}
			return false;
		}

		internal void ServerTeleport(ulong clientId, Transform point)
		{
			if (!(point == null) && base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) && !(value.PlayerObject == null))
			{
				PlayerMovement component = value.PlayerObject.GetComponent<PlayerMovement>();
				if (component != null)
				{
					component.TeleportClientRpc(point.position, point.rotation);
				}
			}
		}

		internal void ServerBeginDeathRagdoll(ulong clientId)
		{
			if (base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) && !(value.PlayerObject == null))
			{
				PlayerRagdoll component = value.PlayerObject.GetComponent<PlayerRagdoll>();
				if (component != null)
				{
					component.ServerBeginDeathRagdoll();
				}
			}
		}

		internal void ServerClearRagdoll(ulong clientId)
		{
			if (base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) && !(value.PlayerObject == null))
			{
				PlayerRagdoll component = value.PlayerObject.GetComponent<PlayerRagdoll>();
				if (component != null)
				{
					component.ServerClearRagdoll();
				}
			}
		}

		internal void ServerHealPlayer(ulong clientId)
		{
			if (base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) && !(value.PlayerObject == null))
			{
				PlayerHealth component = value.PlayerObject.GetComponent<PlayerHealth>();
				if (component != null)
				{
					component.ServerResetHealth();
				}
			}
		}

		internal Transform NextHunterRoomPoint()
		{
			if (!HasHunterRoom)
			{
				return NextSpawnPoint(hunterSpawnPoints, ref duelReturnSpawnIndex);
			}
			return NextSpawnPoint(hunterRoomSpawnPoints, ref hunterRoomSpawnIndex);
		}

		internal void ServerSetVisibleOnlyTo(ulong subjectId, ulong observerId)
		{
			SetPlayerVisibility(subjectId, (ulong observer) => observer == observerId);
		}

		internal void ServerRestoreVisibility(ulong subjectId)
		{
			if (!concealedNonParticipants.Contains(subjectId))
			{
				SetPlayerVisibleToOthers(subjectId, visible: true);
			}
		}

		internal void NotifyDuel(ulong clientId, DuelState state, string opponent, string messageKey)
		{
			if (base.IsServer)
			{
				DuelStateClientRpc(state, opponent ?? "", messageKey ?? "", TargetClient(clientId));
			}
		}

		public override bool AllowsOutsidePlayArea(ulong clientId)
		{
			if ((HasHunterRoom || Duels.IsFighting(clientId)) && CurrentPhase.Value == RoundPhase.Prep && roles.TryGetValue(clientId, out var value))
			{
				return value == PlayerRole.Hunter;
			}
			return false;
		}

		private void MoveHunters(Transform[] points, ref int index)
		{
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				if (role.Value != PlayerRole.Hunter || !base.NetworkManager.ConnectedClients.TryGetValue(role.Key, out var value) || value.PlayerObject == null)
				{
					continue;
				}
				Transform transform = NextSpawnPoint(points, ref index);
				if (!(transform == null))
				{
					PlayerMovement component = value.PlayerObject.GetComponent<PlayerMovement>();
					if (component != null)
					{
						component.TeleportClientRpc(transform.position, transform.rotation);
					}
				}
			}
		}

		private void PlaceWaitingPlayers()
		{
			if (CurrentPhase.Value == RoundPhase.WaitingForPlayers)
			{
				PlaceConnectedPlayersAtLobbySpawns();
			}
		}

		public override bool ServerAllowsBodyEdit(ulong clientId)
		{
			if (CurrentPhase.Value == RoundPhase.Prep || CurrentPhase.Value == RoundPhase.Hunt)
			{
				return GetServerRole(clientId) == PlayerRole.Hider;
			}
			return false;
		}

		public override LobbySettingsData WithDefaults(LobbySettingsData settings)
		{
			settings = base.WithDefaults(settings);
			settings.PrepSeconds = (int)PhaseSeconds(settings.PrepSeconds, 60);
			settings.HuntSeconds = (int)PhaseSeconds(settings.HuntSeconds, 300);
			settings.RoundEndSeconds = (int)PhaseSeconds(settings.RoundEndSeconds, 10);
			settings.MinPlayers = ResolveMinPlayers(settings.MinPlayers);
			settings.TauntIntervalSeconds = ResolveTauntInterval(settings.TauntIntervalSeconds);
			return settings;
		}

		private GameModeDefinition DefinitionInPlay()
		{
			string text = CurrentSettings().ModeId.ToString();
			if (cachedDefinition == null || cachedDefinitionId != text)
			{
				cachedDefinition = GameModeCatalog.Find(text);
				cachedDefinitionId = text;
			}
			return cachedDefinition;
		}

		private void TickPendingConversion()
		{
			if (pendingConversion.Count == 0)
			{
				return;
			}
			if (CurrentPhase.Value != RoundPhase.Hunt)
			{
				pendingConversion.Clear();
				return;
			}
			conversionDue.Clear();
			foreach (KeyValuePair<ulong, double> item in pendingConversion)
			{
				if (Time.timeAsDouble >= item.Value)
				{
					conversionDue.Add(item.Key);
				}
			}
			foreach (ulong item2 in conversionDue)
			{
				ConvertToHunter(item2);
			}
		}

		private void ConvertToHunter(ulong clientId)
		{
			pendingConversion.Remove(clientId);
			if (HasConnectedPlayer(clientId) && GetServerRole(clientId) == PlayerRole.None)
			{
				pendingConceal.Remove(clientId);
				ServerClearRagdoll(clientId);
				if (concealedNonParticipants.Remove(clientId))
				{
					SetPlayerVisibleToOthers(clientId, visible: true);
				}
				ServerHealPlayer(clientId);
				ServerTeleport(clientId, NextSpawnPoint(hunterSpawnPoints, ref conversionSpawnIndex));
				roles[clientId] = PlayerRole.Hunter;
				ApplyRoundLoadout(clientId, PlayerRole.Hunter);
				AssignRoleClientRpc(PlayerRole.Hunter, TargetClient(clientId));
				PublishAliveCounts();
				PublishHunterRoster();
				SystemChat("Chat.PlayerTurned", GetPlayerName(clientId));
				CheckHuntWinConditions();
			}
		}

		public override void FillLobbyInfo(LobbySettingsData settings, List<LobbyInfoRow> rows)
		{
			base.FillLobbyInfo(settings, rows);
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.Preparation"), Loc.Format("Common.Seconds", settings.PrepSeconds)));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.Hunt"), Loc.Format("Common.Seconds", settings.HuntSeconds)));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.RoundEnd"), Loc.Format("Common.Seconds", settings.RoundEndSeconds)));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.MinPlayers"), ResolveMinPlayers(settings.MinPlayers).ToString()));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.TauntInterval"), Loc.Format("Common.Seconds", ResolveTauntInterval(settings.TauntIntervalSeconds))));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.SelfDamage"), GameModeController.SelfDamageLabel(GameModeController.ResolveSelfDamagePercent(settings.SelfDamagePercent))));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.HunterShare"), GameModeController.HunterShareLabel(GameModeController.ResolveHunterSharePercent(settings.HunterSharePercent), LobbyCapacity.Clamp(settings.MaxPlayers))));
		}

		public override bool TryGetScoreHighlight(out string text)
		{
			text = "";
			if (MvpReason.Value == MvpKind.None)
			{
				return false;
			}
			string playerName = GetPlayerName(MvpClientId.Value);
			string arg = Loc.Get((MvpReason.Value == MvpKind.TopDamage) ? "Scoreboard.Mvp.MostDamage" : "Scoreboard.Mvp.HighestScore");
			text = "<font=\"ari-w9500-bold SDF\">" + Loc.Get("Scoreboard.Mvp") + "\n" + $"<size=28><color=yellow>{playerName}</color></size></font>\n{arg} ({MvpValue.Value})</font>";
			return true;
		}

		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();
			if (base.IsServer)
			{
				base.NetworkManager.OnClientConnectedCallback += OnClientConnected;
				base.NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
				if (base.NetworkManager.SceneManager != null)
				{
					base.NetworkManager.SceneManager.OnLoadComplete += OnClientLoadedScene;
				}
				readyClients.Add(base.NetworkManager.LocalClientId);
				EnterPhase(RoundPhase.WaitingForPlayers);
			}
			NetworkVariable<RoundPhase> currentPhase = CurrentPhase;
			currentPhase.OnValueChanged = (NetworkVariable<RoundPhase>.OnValueChangedDelegate)Delegate.Combine(currentPhase.OnValueChanged, new NetworkVariable<RoundPhase>.OnValueChangedDelegate(OnPhaseChanged));
			ApplyHunterDoor(CurrentPhase.Value);
			TrySubscribeToChat();
		}

		private void TrySubscribeToChat()
		{
			if (!subscribedToChat && base.IsClient && !(ChatView.Instance == null))
			{
				ChatView.Instance.MessageSubmitted += OnChatSubmitted;
				subscribedToChat = true;
			}
		}

		private void OnChatSubmitted(string message)
		{
			SubmitChatServerRpc(new FixedString128Bytes(message));
		}

		public override void OnNetworkDespawn()
		{
			base.OnNetworkDespawn();
			if (base.IsServer && base.NetworkManager != null)
			{
				base.NetworkManager.OnClientConnectedCallback -= OnClientConnected;
				base.NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
				if (base.NetworkManager.SceneManager != null)
				{
					base.NetworkManager.SceneManager.OnLoadComplete -= OnClientLoadedScene;
				}
			}
			readyClients.Clear();
			NetworkVariable<RoundPhase> currentPhase = CurrentPhase;
			currentPhase.OnValueChanged = (NetworkVariable<RoundPhase>.OnValueChangedDelegate)Delegate.Remove(currentPhase.OnValueChanged, new NetworkVariable<RoundPhase>.OnValueChangedDelegate(OnPhaseChanged));
			if (subscribedToChat && ChatView.Instance != null)
			{
				ChatView.Instance.MessageSubmitted -= OnChatSubmitted;
			}
			subscribedToChat = false;
		}

		private void OnPhaseChanged(RoundPhase previous, RoundPhase current)
		{
			ApplyHunterDoor(current);
			switch (current)
			{
			case RoundPhase.Prep:
				AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.prepBeginClip);
				RaiseRoundRestarted();
				break;
			case RoundPhase.Hunt:
				AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.huntBeginClip);
				break;
			case RoundPhase.RoundEnd:
				if (lastRoundWinner.Value != PlayerRole.None)
				{
					bool flag = base.LocalRole == lastRoundWinner.Value;
					AudioLibrary.PlayOneShotClip((AudioLibrary.Instance == null) ? null : (flag ? AudioLibrary.Instance.roundSuccessClip : AudioLibrary.Instance.roundFailClip));
					LobbySettingsData lobbySettingsData = CurrentSettings();
					Telemetry.Send("round_end", ("game_mode", lobbySettingsData.ModeId.ToString()), ("map", lobbySettingsData.MapId.ToString()), ("player_role", base.LocalRole.ToString().ToLowerInvariant()), ("won", flag), ("winner", lastRoundWinner.Value.ToString().ToLowerInvariant()), ("players", base.NetworkManager.ConnectedClientsIds.Count), ("rounds_this_session", Telemetry.Bump("rounds")), ("is_host", base.IsHost));
				}
				break;
			}
		}

		private void ApplyHunterDoor(RoundPhase phase)
		{
			if (!(hunterDoor == null))
			{
				bool flag = phase == RoundPhase.WaitingForPlayers || phase == RoundPhase.Prep;
				if (hunterDoor.activeSelf != flag)
				{
					hunterDoor.SetActive(flag);
				}
			}
		}

		private void Update()
		{
			TrySubscribeToChat();
			UpdateTimerTick();
			if (!base.IsServer)
			{
				return;
			}
			SampleLatencies();
			TickPendingConceal();
			TickPendingConversion();
			TickPendingPlacement();
			Duels.Tick();
			if (CurrentPhase.Value == RoundPhase.WaitingForPlayers)
			{
				if (HasEnoughPlayers())
				{
					EnterPhase(RoundPhase.Prep);
				}
			}
			else if (!EndRoundIfNoHuntersLeft())
			{
				if (CurrentPhase.Value == RoundPhase.Hunt)
				{
					AwardHiderSurvivalScore();
					TauntLivingHiders();
				}
				if (base.NetworkManager.ServerTime.Time >= PhaseEndServerTime.Value)
				{
					AdvancePhase();
				}
			}
		}

		private bool EndRoundIfNoHuntersLeft()
		{
			RoundPhase value = CurrentPhase.Value;
			if ((value != RoundPhase.Prep && value != RoundPhase.Hunt) || AnyRoleRemaining(PlayerRole.Hunter))
			{
				return false;
			}
			if (value == RoundPhase.Hunt)
			{
				CheckHuntWinConditions();
				return true;
			}
			lastRoundWinner.Value = PlayerRole.None;
			AnnounceClientRpc("Round.NoHuntersLeft");
			EnterPhase(RoundPhase.RoundEnd);
			return true;
		}

		private void UpdateTimerTick()
		{
			RoundPhase value = CurrentPhase.Value;
			if (value != RoundPhase.Prep && value != RoundPhase.Hunt)
			{
				lastTickedSecond = -1;
				return;
			}
			int num = Mathf.CeilToInt((float)(PhaseEndServerTime.Value - base.NetworkManager.ServerTime.Time));
			if (num <= 5 && num > 0 && num != lastTickedSecond)
			{
				lastTickedSecond = num;
				AudioLibrary.PlayOneShotClip(AudioLibrary.Instance?.timerTickClip);
			}
		}

		private void OnClientConnected(ulong clientId)
		{
			MarkReady(clientId);
		}

		private void OnClientReady(ulong clientId)
		{
			if (CurrentPhase.Value == RoundPhase.WaitingForPlayers)
			{
				if (HasEnoughPlayers())
				{
					EnterPhase(RoundPhase.Prep);
				}
				return;
			}
			if (hidersConcealed)
			{
				SetHidersVisibleTo(clientId, visible: false);
			}
			RoundPhase value = CurrentPhase.Value;
			if (value == RoundPhase.Prep || value == RoundPhase.Hunt)
			{
				MakeLateJoinerHunter(clientId);
			}
			else
			{
				ConcealNonParticipant(clientId);
			}
		}

		private void MakeLateJoinerHunter(ulong clientId)
		{
			roles[clientId] = PlayerRole.Hunter;
			roundStartRoles[clientId] = PlayerRole.Hunter;
			hunterTurns[clientId] = HunterTurnsOf(clientId) + 1;
			lastHunterRound[clientId] = roundNumber;
			AssignRoleClientRpc(PlayerRole.Hunter, TargetClient(clientId));
			pendingPlacement[clientId] = Time.timeAsDouble + 10.0;
			PublishAliveCounts();
			PublishHunterRoster();
			SystemChat("Chat.JoinedAsHunter", GetPlayerName(clientId));
		}

		private void TickPendingPlacement()
		{
			if (pendingPlacement.Count == 0)
			{
				return;
			}
			RoundPhase value = CurrentPhase.Value;
			if (value != RoundPhase.Prep && value != RoundPhase.Hunt)
			{
				pendingPlacement.Clear();
				return;
			}
			placementDue.Clear();
			foreach (KeyValuePair<ulong, double> item in pendingPlacement)
			{
				if (HasConnectedPlayer(item.Key) || Time.timeAsDouble >= item.Value)
				{
					placementDue.Add(item.Key);
				}
			}
			foreach (ulong item2 in placementDue)
			{
				PlaceLateHunter(item2);
			}
		}

		private void PlaceLateHunter(ulong clientId)
		{
			pendingPlacement.Remove(clientId);
			if (HasConnectedPlayer(clientId) && GetServerRole(clientId) == PlayerRole.Hunter)
			{
				ServerHealPlayer(clientId);
				ApplyRoundLoadout(clientId, PlayerRole.Hunter);
				Transform[] points = ((CurrentPhase.Value == RoundPhase.Prep && HasHunterRoom) ? hunterRoomSpawnPoints : hunterSpawnPoints);
				ServerTeleport(clientId, NextSpawnPoint(points, ref latePlacementSpawnIndex));
			}
		}

		private void OnClientDisconnected(ulong clientId)
		{
			SystemChat("Chat.PlayerLeft", GetPlayerName(clientId));
			nextChatAllowedServerTime.Remove(clientId);
			bool num = roles.Remove(clientId);
			readyHiderClientIds.Remove(clientId);
			rolePreferences.Remove(clientId);
			forcedRoles.Remove(clientId);
			hunterTurns.Remove(clientId);
			lastHunterRound.Remove(clientId);
			RemoveName(clientId);
			concealedNonParticipants.Remove(clientId);
			pendingConversion.Remove(clientId);
			pendingPlacement.Remove(clientId);
			readyClients.Remove(clientId);
			Duels.OnDisconnected(clientId);
			RemoveDuelWins(clientId);
			PublishAliveCounts();
			PublishHunterRoster();
			if (num && CurrentPhase.Value != RoundPhase.WaitingForPlayers)
			{
				if (CurrentPhase.Value == RoundPhase.Prep)
				{
					EndPrepIfEveryHiderReady();
				}
				else if (CurrentPhase.Value == RoundPhase.Hunt)
				{
					CheckHuntWinConditions();
				}
			}
		}

		private void AdvancePhase()
		{
			switch (CurrentPhase.Value)
			{
			case RoundPhase.Prep:
				EnterPhase(RoundPhase.Hunt);
				break;
			case RoundPhase.Hunt:
				lastRoundWinner.Value = PlayerRole.Hider;
				AnnounceClientRpc("Round.TimeUpModelersWin");
				EnterPhase(RoundPhase.RoundEnd);
				break;
			case RoundPhase.RoundEnd:
				EnterPhase(HasEnoughPlayers() ? RoundPhase.Prep : RoundPhase.WaitingForPlayers);
				break;
			}
		}

		public void DevEnterPhase(RoundPhase phase)
		{
			if (base.IsServer)
			{
				EnterPhase(phase);
			}
		}

		public void DevForceRole(PlayerRole role, ulong? onlyClient)
		{
			if (!base.IsServer || base.NetworkManager == null)
			{
				return;
			}
			bool flag = hidersConcealed;
			if (flag)
			{
				RevealHiders();
			}
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				if (!onlyClient.HasValue || onlyClient.Value == connectedClientsId)
				{
					forcedRoles[connectedClientsId] = role;
					roles[connectedClientsId] = role;
					roundStartRoles[connectedClientsId] = role;
					ApplyRoundLoadout(connectedClientsId, role);
					AssignRoleClientRpc(role, TargetClient(connectedClientsId));
				}
			}
			if (flag)
			{
				ConcealHiders();
			}
			PublishAliveCounts();
			PublishHunterRoster();
		}

		public void DevClearForcedRoles(ulong? onlyClient)
		{
			if (base.IsServer)
			{
				if (!onlyClient.HasValue)
				{
					forcedRoles.Clear();
				}
				else
				{
					forcedRoles.Remove(onlyClient.Value);
				}
			}
		}

		public IEnumerable<KeyValuePair<ulong, PlayerRole>> DevRoles()
		{
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				yield return role;
			}
		}

		private void EnterPhase(RoundPhase next)
		{
			Duels.EndAll();
			CurrentPhase.Value = next;
			ClearAllSpotted();
			LobbySettingsData lobbySettingsData = CurrentSettings();
			float num = next switch
			{
				RoundPhase.Prep => PhaseSeconds(lobbySettingsData.PrepSeconds, 60), 
				RoundPhase.Hunt => PhaseSeconds(lobbySettingsData.HuntSeconds, 300), 
				RoundPhase.RoundEnd => PhaseSeconds(lobbySettingsData.RoundEndSeconds, 10), 
				_ => 0f, 
			};
			double num2 = ((next == RoundPhase.RoundEnd) ? CinematicSecondsForSurvivors() : 0.0);
			CinematicEndServerTime.Value = base.NetworkManager.ServerTime.Time + num2;
			PhaseEndServerTime.Value = ((next == RoundPhase.WaitingForPlayers) ? 0.0 : (base.NetworkManager.ServerTime.Time + num2 + (double)num));
			switch (next)
			{
			case RoundPhase.RoundEnd:
				PublishReveals();
				PublishMvp();
				break;
			case RoundPhase.WaitingForPlayers:
			case RoundPhase.Prep:
				Reveals.Clear();
				ClearMvp();
				break;
			}
			if (next == RoundPhase.Prep)
			{
				RevealHiders();
				RevealNonParticipants();
				AssignRoles();
				ConcealHiders();
				ResetAndSpawnPlayers();
			}
			else
			{
				RevealHiders();
			}
			switch (next)
			{
			case RoundPhase.Prep:
				AnnounceClientRpc("Round.PreparationBegan");
				break;
			case RoundPhase.Hunt:
			{
				int index = 0;
				MoveHunters(hunterSpawnPoints, ref index);
				nextHiderScoreServerTime = base.NetworkManager.ServerTime.Time + 2.0;
				nextTauntServerTime = base.NetworkManager.ServerTime.Time + (double)TauntIntervalSeconds;
				AnnounceClientRpc("Round.HuntBegan");
				break;
			}
			}
		}

		private bool HasEnoughPlayers()
		{
			return ReadyPlayerCount() >= MinRequiredPlayers();
		}

		private int ReadyPlayerCount()
		{
			int num = 0;
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				if (readyClients.Contains(connectedClientsId))
				{
					num++;
				}
			}
			return num;
		}

		private bool IsReady(ulong clientId)
		{
			return readyClients.Contains(clientId);
		}

		private void OnClientLoadedScene(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
		{
			if (sceneName == base.gameObject.scene.name)
			{
				MarkReady(clientId);
			}
		}

		private void MarkReady(ulong clientId)
		{
			if (readyClients.Add(clientId) && base.IsSpawned && base.NetworkManager.ConnectedClients.ContainsKey(clientId))
			{
				OnClientReady(clientId);
			}
		}

		public override void RequestStartRound()
		{
			RequestStartRoundServerRpc();
		}

		public override void RequestRestartRound()
		{
			RequestRestartRoundServerRpc();
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void RequestStartRoundServerRpc(RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(1543445559u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 1543445559u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (IsHostRequest(rpcParams) && !IsRoundRunning)
				{
					SystemChat("Chat.GameStarted", GetPlayerName(0uL));
					EnterPhase(RoundPhase.Prep);
				}
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void RequestRestartRoundServerRpc(RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(56261267u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 56261267u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (IsHostRequest(rpcParams) && IsRoundRunning)
				{
					SystemChat("Chat.GameRestarted", GetPlayerName(0uL));
					EnterPhase(RoundPhase.Prep);
				}
			}
		}

		private bool IsHostRequest(RpcParams rpcParams)
		{
			return rpcParams.Receive.SenderClientId == 0;
		}

		public static int ResolveMinPlayers(int configured)
		{
			if (configured >= 2)
			{
				return configured;
			}
			return 2;
		}

		private int MinRequiredPlayers()
		{
			return ResolveMinPlayers(CurrentSettings().MinPlayers);
		}

		private LobbySettingsData CurrentSettings()
		{
			if (lobbySettings == null)
			{
				lobbySettings = UnityEngine.Object.FindFirstObjectByType<LobbySettingsSync>();
			}
			if (!(lobbySettings != null))
			{
				return default(LobbySettingsData);
			}
			return lobbySettings.CurrentSettings;
		}

		private static float PhaseSeconds(int configured, int fallback)
		{
			return (configured > 0) ? configured : fallback;
		}

		private void AssignRoles()
		{
			roles.Clear();
			roundStartRoles.Clear();
			readyHiderClientIds.Clear();
			List<ulong> list = new List<ulong>();
			List<ulong> list2 = new List<ulong>();
			List<ulong> list3 = new List<ulong>();
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				if (IsReady(connectedClientsId))
				{
					switch (PreferenceOf(connectedClientsId))
					{
					case PlayerRole.Hunter:
						list.Add(connectedClientsId);
						break;
					case PlayerRole.Hider:
						list2.Add(connectedClientsId);
						break;
					default:
						list3.Add(connectedClientsId);
						break;
					}
				}
			}
			OrderByHunterFairness(list);
			OrderByHunterFairness(list3);
			OrderByHunterFairness(list2);
			List<ulong> list4 = new List<ulong>(list.Count + list3.Count + list2.Count);
			list4.AddRange(list);
			list4.AddRange(list3);
			list4.AddRange(list2);
			int hunters = SplitFor(list4.Count).Hunters;
			roundNumber++;
			for (int i = 0; i < list4.Count; i++)
			{
				ulong num = list4[i];
				PlayerRole playerRole = ((i < hunters) ? PlayerRole.Hunter : PlayerRole.Hider);
				if (forcedRoles.TryGetValue(num, out var value))
				{
					playerRole = value;
				}
				roles[num] = playerRole;
				if (playerRole == PlayerRole.Hunter)
				{
					hunterTurns[num] = HunterTurnsOf(num) + 1;
					lastHunterRound[num] = roundNumber;
				}
				roundStartRoles[num] = playerRole;
				ApplyRoundLoadout(num, playerRole);
				AssignRoleClientRpc(playerRole, TargetClient(num));
			}
			PublishAliveCounts();
			PublishHunterRoster();
		}

		private void ApplyRoundLoadout(ulong clientId, PlayerRole role)
		{
			if (!(base.NetworkManager == null) && base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) && !(value.PlayerObject == null))
			{
				PlayerWeapons component = value.PlayerObject.GetComponent<PlayerWeapons>();
				if (component != null)
				{
					component.ServerApplyRoundLoadout(role == PlayerRole.Hunter);
				}
			}
		}

		private int HunterTurnsOf(ulong clientId)
		{
			if (!hunterTurns.TryGetValue(clientId, out var value))
			{
				return 0;
			}
			return value;
		}

		private int LastHunterRoundOf(ulong clientId)
		{
			if (!lastHunterRound.TryGetValue(clientId, out var value))
			{
				return 0;
			}
			return value;
		}

		private void OrderByHunterFairness(List<ulong> clientIds)
		{
			Shuffle(clientIds);
			clientIds.Sort(delegate(ulong a, ulong b)
			{
				int num = HunterTurnsOf(a).CompareTo(HunterTurnsOf(b));
				return (num == 0) ? LastHunterRoundOf(a).CompareTo(LastHunterRoundOf(b)) : num;
			});
		}

		private PlayerRole PreferenceOf(ulong clientId)
		{
			if (!rolePreferences.TryGetValue(clientId, out var value))
			{
				return PlayerRole.None;
			}
			return value;
		}

		private int HunterCountFor(int playerCount)
		{
			return SplitFor(playerCount).Hunters;
		}

		public HunterSplit SplitFor(int playerCount)
		{
			LobbySettingsData lobbySettingsData = CurrentSettings();
			int num = GameModeController.ResolveHunterSharePercent(lobbySettingsData.HunterSharePercent);
			return new HunterSplit
			{
				Players = playerCount,
				ConfiguredPercent = lobbySettingsData.HunterSharePercent,
				Percent = num,
				Raw = ((playerCount > 0) ? Mathf.FloorToInt((float)(playerCount * num) / 100f + 0.5f) : 0),
				Hunters = GameModeController.HunterCountForShare(playerCount, num)
			};
		}

		public string SplitReport()
		{
			int num = ((base.NetworkManager != null) ? base.NetworkManager.ConnectedClientsIds.Count : 0);
			int num2 = ((base.NetworkManager != null) ? ReadyPlayerCount() : 0);
			HunterSplit hunterSplit = SplitFor(num2);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"players ready: {num2} of {num} connected   min players: {MinRequiredPlayers()}" + ((num2 >= MinRequiredPlayers()) ? "" : "   WAITING"));
			if (num2 < num)
			{
				stringBuilder.AppendLine("  (a connected player who is not ready is still loading this scene - not counted, not dealt in)");
			}
			stringBuilder.AppendLine($"hunter share: {hunterSplit.Percent}%" + ((hunterSplit.ConfiguredPercent == hunterSplit.Percent) ? "" : $"  (lobby sent {hunterSplit.ConfiguredPercent}, which reads as the default)"));
			stringBuilder.AppendLine($"  {hunterSplit.Percent}% of {hunterSplit.Players} = {hunterSplit.Raw}");
			stringBuilder.AppendLine($"RESULT: {hunterSplit.Hunters} Hunters / {hunterSplit.Players - hunterSplit.Hunters} Modelers");
			if (hunterSplit.Adjusted)
			{
				stringBuilder.AppendLine($"  Moved from {hunterSplit.Raw} so both sides have somebody: a round with no " + "Hunter can never end and one with no Modelci ends on its first frame.");
			}
			return stringBuilder.ToString();
		}

		private void AwardHiderSurvivalScore()
		{
			if (base.NetworkManager.ServerTime.Time < nextHiderScoreServerTime)
			{
				return;
			}
			nextHiderScoreServerTime += 2.0;
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				if (role.Value == PlayerRole.Hider)
				{
					bool flag = IsSpottedByAnyHunter(role.Key);
					SetSpotted(role.Key, flag);
					float num = SizeScoreMultiplier(role.Key) * (flag ? losScoreMultiplier : 1f);
					int amount = Mathf.Max(1, Mathf.CeilToInt(1f * num));
					AddScore(role.Key, amount);
				}
			}
		}

		private bool IsSpottedByAnyHunter(ulong hiderId)
		{
			if (losScoreMultiplier <= 1f)
			{
				return false;
			}
			if (!TryGetPlayerTransform(hiderId, out var result))
			{
				return false;
			}
			Vector3 vector = result.position + Vector3.up * 0.5f;
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				if (role.Value != PlayerRole.Hunter || !TryGetPlayerTransform(role.Key, out var result2))
				{
					continue;
				}
				Vector3 vector2 = result2.position + Vector3.up * 1.6f;
				Vector3 vector3 = vector - vector2;
				float magnitude = vector3.magnitude;
				if (!(magnitude > losMaxDistance) && !(magnitude < 0.01f))
				{
					Vector3 to = new Vector3(vector3.x, 0f, vector3.z);
					Vector3 vector4 = new Vector3(result2.forward.x, 0f, result2.forward.z);
					if (!(to.sqrMagnitude < 0.0001f) && !(vector4.sqrMagnitude < 0.0001f) && !(Vector3.Angle(vector4, to) > losHalfAngle) && (!Physics.Raycast(vector2, vector3.normalized, out var hitInfo, magnitude - 0.3f, -1, QueryTriggerInteraction.Ignore) || !(hitInfo.collider.GetComponentInParent<PlayerHealth>() == null)))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool TryGetPlayerTransform(ulong clientId, out Transform result)
		{
			result = null;
			if (!base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) || value.PlayerObject == null)
			{
				return false;
			}
			result = value.PlayerObject.transform;
			return true;
		}

		private void SetSpotted(ulong hiderId, bool spotted)
		{
			if (base.NetworkManager.ConnectedClients.TryGetValue(hiderId, out var value) && !(value.PlayerObject == null))
			{
				value.PlayerObject.GetComponent<PlayerSpotState>()?.SetSpotted(spotted);
			}
		}

		private void ClearAllSpotted()
		{
			if (!base.IsServer)
			{
				return;
			}
			foreach (KeyValuePair<ulong, NetworkClient> connectedClient in base.NetworkManager.ConnectedClients)
			{
				if (connectedClient.Value.PlayerObject != null)
				{
					connectedClient.Value.PlayerObject.GetComponent<PlayerSpotState>()?.SetSpotted(value: false);
				}
			}
		}

		public void RecordDamage(ulong shooterId, int amount)
		{
			if (base.IsServer && amount > 0)
			{
				damageDealt[shooterId] = (damageDealt.TryGetValue(shooterId, out var value) ? (value + amount) : amount);
			}
		}

		private void PublishMvp()
		{
			ulong num = ulong.MaxValue;
			int num2 = 0;
			MvpKind mvpKind = lastRoundWinner.Value switch
			{
				PlayerRole.Hider => MvpKind.TopScore, 
				PlayerRole.Hunter => MvpKind.TopDamage, 
				_ => MvpKind.None, 
			};
			if (mvpKind != MvpKind.None)
			{
				foreach (KeyValuePair<ulong, PlayerRole> roundStartRole in roundStartRoles)
				{
					if (roundStartRole.Value == lastRoundWinner.Value)
					{
						int num3 = ((mvpKind == MvpKind.TopScore) ? ScoreOf(roundStartRole.Key) : DamageOf(roundStartRole.Key));
						if (num3 > num2)
						{
							num2 = num3;
							num = roundStartRole.Key;
						}
					}
				}
			}
			MvpClientId.Value = ((num2 > 0) ? num : ulong.MaxValue);
			MvpReason.Value = ((num2 > 0) ? mvpKind : MvpKind.None);
			MvpValue.Value = num2;
			if (num2 > 0)
			{
				MvpAnnouncedClientRpc(TargetClient(num));
			}
		}

		[ClientRpc]
		private void MvpAnnouncedClientRpc(ClientRpcParams rpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(1868337822u, rpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 1868337822u, rpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.mvpClip : null);
			}
		}

		private void ClearMvp()
		{
			MvpClientId.Value = ulong.MaxValue;
			MvpReason.Value = MvpKind.None;
			MvpValue.Value = 0;
			damageDealt.Clear();
		}

		private int ScoreOf(ulong clientId)
		{
			foreach (PlayerScoreEntry score in Scores)
			{
				if (score.ClientId == clientId)
				{
					return score.Score;
				}
			}
			return 0;
		}

		private int DamageOf(ulong clientId)
		{
			if (!damageDealt.TryGetValue(clientId, out var value))
			{
				return 0;
			}
			return value;
		}

		private double CinematicSecondsForSurvivors()
		{
			int num = 0;
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				if (role.Value == PlayerRole.Hider)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return 0.0;
			}
			return 0.6f + (float)num * 2.1f;
		}

		public override ulong GetSteamId(ulong clientId)
		{
			foreach (PlayerNameEntry name in Names)
			{
				if (name.ClientId == clientId)
				{
					return name.SteamId;
				}
			}
			return 0uL;
		}

		private float SizeScoreMultiplier(ulong clientId)
		{
			if (Mathf.Approximately(sizeScoreInfluence, 0f))
			{
				return 1f;
			}
			if (!base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) || value.PlayerObject == null)
			{
				return 1f;
			}
			PlayerVoxelBody component = value.PlayerObject.GetComponent<PlayerVoxelBody>();
			if (component == null)
			{
				return 1f;
			}
			return Mathf.LerpUnclamped(1f, component.BodySizeRatio, sizeScoreInfluence);
		}

		private void TauntLivingHiders()
		{
			if (base.NetworkManager.ServerTime.Time < nextTauntServerTime)
			{
				return;
			}
			nextTauntServerTime += TauntIntervalSeconds;
			taunting.Clear();
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				if (role.Value == PlayerRole.Hider && base.NetworkManager.ConnectedClients.TryGetValue(role.Key, out var value) && !(value.PlayerObject == null))
				{
					PlayerTaunt component = value.PlayerObject.GetComponent<PlayerTaunt>();
					if (component != null)
					{
						taunting.Add(component);
					}
				}
			}
			float num = ((taunting.Count > 1) ? Mathf.Min(tauntStaggerSeconds, tauntStaggerSpreadSeconds / (float)(taunting.Count - 1)) : 0f);
			for (int i = 0; i < taunting.Count; i++)
			{
				taunting[i].PlayTauntClientRpc(forced: true, (float)i * num);
			}
			if (taunting.Count > 0)
			{
				AnnounceClientRpc("Round.ModelersMadeSound");
			}
		}

		private static ClientRpcParams TargetClient(ulong clientId)
		{
			return new ClientRpcParams
			{
				Send = new ClientRpcSendParams
				{
					TargetClientIds = new ulong[1] { clientId }
				}
			};
		}

		public override bool TryGetLobbySpawn(out Vector3 position, out Quaternion rotation)
		{
			Transform transform = NextSpawnPoint(lobbySpawnPoints, ref lobbySpawnIndex) ?? NextSpawnPoint(hiderSpawnPoints, ref lobbySpawnIndex);
			if (transform == null)
			{
				position = default(Vector3);
				rotation = default(Quaternion);
				return false;
			}
			position = transform.position;
			rotation = transform.rotation;
			return true;
		}

		public override bool TryGetRescueSpawn(ulong clientId, out Vector3 position, out Quaternion rotation)
		{
			PlayerRole value;
			Transform transform = ((roles.TryGetValue(clientId, out value) && value == PlayerRole.Hunter) ? NextSpawnPoint(hunterSpawnPoints, ref rescueSpawnIndex) : NextSpawnPoint(hiderSpawnPoints, ref rescueSpawnIndex));
			if (transform == null)
			{
				return base.TryGetRescueSpawn(clientId, out position, out rotation);
			}
			position = transform.position;
			rotation = transform.rotation;
			return true;
		}

		private static Transform NextSpawnPoint(Transform[] points, ref int index)
		{
			if (points == null || points.Length == 0)
			{
				return null;
			}
			for (int i = 0; i < points.Length; i++)
			{
				Transform transform = points[index++ % points.Length];
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		private void ResetAndSpawnPlayers()
		{
			int index = 0;
			int index2 = 0;
			Transform[] points = (HasHunterRoom ? hunterRoomSpawnPoints : hunterSpawnPoints);
			hunterRoomSpawnIndex = 0;
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				if (base.NetworkManager.ConnectedClients.TryGetValue(role.Key, out var value) && !(value.PlayerObject == null))
				{
					NetworkObject playerObject = value.PlayerObject;
					PlayerHealth component = playerObject.GetComponent<PlayerHealth>();
					if (component != null)
					{
						component.ServerResetHealth();
					}
					Transform transform = ((role.Value == PlayerRole.Hunter) ? NextSpawnPoint(points, ref index2) : NextSpawnPoint(hiderSpawnPoints, ref index));
					PlayerMovement component2 = playerObject.GetComponent<PlayerMovement>();
					if (component2 != null && transform != null)
					{
						component2.TeleportClientRpc(transform.position, transform.rotation);
					}
				}
			}
		}

		private void ConcealHiders()
		{
			if (hidersConcealed)
			{
				return;
			}
			hidersConcealed = true;
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				SetHidersVisibleTo(connectedClientsId, visible: false);
			}
		}

		private void RevealHiders()
		{
			if (!hidersConcealed)
			{
				return;
			}
			hidersConcealed = false;
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				SetHidersVisibleTo(connectedClientsId, visible: true);
			}
		}

		private void SetHidersVisibleTo(ulong observerId, bool visible)
		{
			if (GetServerRole(observerId) == PlayerRole.Hider)
			{
				return;
			}
			bool flag = base.IsHost && observerId == base.NetworkManager.LocalClientId;
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				if (role.Value != PlayerRole.Hider || role.Key == observerId || !base.NetworkManager.ConnectedClients.TryGetValue(role.Key, out var value) || value.PlayerObject == null)
				{
					continue;
				}
				if (flag)
				{
					PlayerVoxelBody component = value.PlayerObject.GetComponent<PlayerVoxelBody>();
					if (component != null)
					{
						component.SetLocallyConcealed(!visible);
					}
				}
				else if (visible)
				{
					value.PlayerObject.NetworkShow(observerId);
				}
				else
				{
					value.PlayerObject.NetworkHide(observerId);
				}
			}
		}

		public bool IsActiveParticipant(ulong clientId)
		{
			return GetServerRole(clientId) != PlayerRole.None;
		}

		public override bool ServerIsShotTarget(ulong clientId)
		{
			if (IsActiveParticipant(clientId))
			{
				return !Duels.IsFallen(clientId);
			}
			return false;
		}

		public override bool ServerMayDamage(ulong shooterId, ulong victimId)
		{
			if (GetServerRole(victimId) != PlayerRole.Hider)
			{
				return Duels.ArePartners(shooterId, victimId);
			}
			return true;
		}

		public override void ServerRecordDamage(ulong shooterId, ulong victimId, int amount)
		{
			if (!Duels.ArePartners(shooterId, victimId))
			{
				RecordDamage(shooterId, amount);
			}
		}

		public override void ServerReportKill(ulong killerId, ulong victimId)
		{
			if (!Duels.ConsumeDuelKill(victimId))
			{
				AddScore(killerId, 50);
				ReportKill(killerId, victimId);
			}
		}

		public override bool ServerOnPlayerDied(ulong clientId)
		{
			if (Duels.OnPlayerDied(clientId))
			{
				return true;
			}
			if (GetServerRole(clientId) == PlayerRole.Hider)
			{
				return ReportHiderFound(clientId);
			}
			ReportHunterEliminated(clientId);
			return true;
		}

		private void ConcealNonParticipant(ulong clientId)
		{
			pendingConceal.Remove(clientId);
			if (concealedNonParticipants.Add(clientId))
			{
				SetPlayerVisibleToOthers(clientId, visible: false);
			}
		}

		private void ConcealAfterDeathRagdoll(ulong clientId)
		{
			if (!base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) || value.PlayerObject == null)
			{
				ConcealNonParticipant(clientId);
				return;
			}
			PlayerRagdoll component = value.PlayerObject.GetComponent<PlayerRagdoll>();
			if (component == null || !component.ServerBeginDeathRagdoll())
			{
				ConcealNonParticipant(clientId);
			}
			else
			{
				pendingConceal[clientId] = Time.timeAsDouble + 3.0;
			}
		}

		private void TickPendingConceal()
		{
			if (pendingConceal.Count == 0)
			{
				return;
			}
			concealDue.Clear();
			foreach (KeyValuePair<ulong, double> item in pendingConceal)
			{
				if (Time.timeAsDouble >= item.Value)
				{
					concealDue.Add(item.Key);
				}
			}
			foreach (ulong item2 in concealDue)
			{
				ConcealNonParticipant(item2);
			}
		}

		protected virtual void RevealNonParticipants()
		{
			foreach (ulong concealedNonParticipant in concealedNonParticipants)
			{
				SetPlayerVisibleToOthers(concealedNonParticipant, visible: true);
			}
			concealedNonParticipants.Clear();
			pendingConceal.Clear();
			ClearAllRagdolls();
		}

		private void ClearAllRagdolls()
		{
			foreach (NetworkClient connectedClients in base.NetworkManager.ConnectedClientsList)
			{
				if (!(connectedClients.PlayerObject == null))
				{
					PlayerRagdoll component = connectedClients.PlayerObject.GetComponent<PlayerRagdoll>();
					if (component != null)
					{
						component.ServerClearRagdoll();
					}
				}
			}
		}

		protected bool RevealNonParticipant(ulong clientId)
		{
			if (!concealedNonParticipants.Remove(clientId))
			{
				return false;
			}
			SetPlayerVisibleToOthers(clientId, visible: true);
			return true;
		}

		private void SetPlayerVisibleToOthers(ulong subjectId, bool visible)
		{
			SetPlayerVisibility(subjectId, (ulong _) => visible);
		}

		private void SetPlayerVisibility(ulong subjectId, Func<ulong, bool> visibleTo)
		{
			if (!base.NetworkManager.ConnectedClients.TryGetValue(subjectId, out var value) || value.PlayerObject == null)
			{
				return;
			}
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				if (connectedClientsId == subjectId)
				{
					continue;
				}
				bool flag = visibleTo(connectedClientsId);
				if (base.IsHost && connectedClientsId == base.NetworkManager.LocalClientId)
				{
					PlayerVoxelBody component = value.PlayerObject.GetComponent<PlayerVoxelBody>();
					if (component != null)
					{
						component.SetLocallyConcealed(!flag);
					}
				}
				else if (flag)
				{
					value.PlayerObject.NetworkShow(connectedClientsId);
				}
				else
				{
					value.PlayerObject.NetworkHide(connectedClientsId);
				}
			}
		}

		private static void Shuffle(IList<ulong> list)
		{
			for (int num = list.Count - 1; num > 0; num--)
			{
				int num2 = UnityEngine.Random.Range(0, num + 1);
				int index = num;
				int index2 = num2;
				ulong num3 = list[num2];
				ulong num4 = list[num];
				ulong num5 = (list[index] = num3);
				num5 = (list[index2] = num4);
			}
		}

		[ClientRpc]
		private void AssignRoleClientRpc(PlayerRole role, ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(3461441085u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in role, default(FastBufferWriter.ForEnums));
				__endSendClientRpc(ref bufferWriter, 3461441085u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				base.LocalRole = role;
			}
		}

		[ClientRpc]
		private void EliminatedClientRpc(ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(317198445u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 317198445u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				RaiseLocalEliminated();
			}
		}

		public void RequestDuel(bool join)
		{
			RequestDuelServerRpc(join);
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void RequestDuelServerRpc(bool join, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(1350239846u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in join, default(FastBufferWriter.ForPrimitives));
				__endSendRpc(ref bufferWriter, 1350239846u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				Duels.SetQueued(rpcParams.Receive.SenderClientId, join);
			}
		}

		[ClientRpc]
		private void DuelStateClientRpc(DuelState state, string opponent, string messageKey, ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(1146634388u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in state, default(FastBufferWriter.ForEnums));
				bool value = opponent != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(opponent);
				}
				bool value2 = messageKey != null;
				bufferWriter.WriteValueSafe(in value2, default(FastBufferWriter.ForPrimitives));
				if (value2)
				{
					bufferWriter.WriteValueSafe(messageKey);
				}
				__endSendClientRpc(ref bufferWriter, 1146634388u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				LocalDuelState = state;
				LocalDuelOpponent = opponent ?? "";
				this.DuelChanged?.Invoke();
				switch (messageKey)
				{
				case "Duel.Started":
					Telemetry.Send("duel_started", ("duels_this_session", Telemetry.Bump("duels")));
					break;
				case "Duel.Won":
				case "Duel.Lost":
				case "Duel.Cancelled":
					Telemetry.Send("duel_result", ("outcome", messageKey.Substring("Duel.".Length).ToLowerInvariant()));
					break;
				}
				if (!string.IsNullOrEmpty(messageKey) && !(ToastView.Instance == null))
				{
					ToastView.Instance.Show((messageKey == "Duel.Started") ? Loc.Format(messageKey, LocalDuelOpponent) : Loc.Get(messageKey));
				}
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void RequestRolePreferenceServerRpc(PlayerRole preferred, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(222375821u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in preferred, default(FastBufferWriter.ForEnums));
				__endSendRpc(ref bufferWriter, 222375821u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ulong senderClientId = rpcParams.Receive.SenderClientId;
				if (preferred == PlayerRole.Hunter || preferred == PlayerRole.Hider)
				{
					rolePreferences[senderClientId] = preferred;
				}
				else
				{
					rolePreferences.Remove(senderClientId);
				}
				ConfirmRolePreferenceClientRpc(PreferenceOf(senderClientId), TargetClient(senderClientId));
			}
		}

		[ClientRpc]
		private void ConfirmRolePreferenceClientRpc(PlayerRole preferred, ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(3844162082u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in preferred, default(FastBufferWriter.ForEnums));
				__endSendClientRpc(ref bufferWriter, 3844162082u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				LocalRolePreference = preferred;
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void RequestReadyServerRpc(RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(1940732309u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 1940732309u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (CurrentPhase.Value == RoundPhase.Prep)
			{
				ulong senderClientId = rpcParams.Receive.SenderClientId;
				if (roles.TryGetValue(senderClientId, out var value) && value == PlayerRole.Hider)
				{
					readyHiderClientIds.Add(senderClientId);
					EndPrepIfEveryHiderReady();
				}
			}
		}

		private void EndPrepIfEveryHiderReady()
		{
			int num = CountRole(PlayerRole.Hider);
			if (num == 0)
			{
				lastRoundWinner.Value = PlayerRole.None;
				AnnounceClientRpc("Round.NoModelersLeft");
				EnterPhase(RoundPhase.RoundEnd);
			}
			else if (readyHiderClientIds.Count >= num)
			{
				EnterPhase(RoundPhase.Hunt);
			}
		}

		private void CheckHuntWinConditions()
		{
			if (!AnyRoleRemaining(PlayerRole.Hider))
			{
				lastRoundWinner.Value = PlayerRole.Hunter;
				AnnounceClientRpc("Round.AllModelersFound");
				EnterPhase(RoundPhase.RoundEnd);
			}
			else if (!AnyRoleRemaining(PlayerRole.Hunter))
			{
				lastRoundWinner.Value = PlayerRole.Hider;
				AnnounceClientRpc("Round.AllHuntersGone");
				EnterPhase(RoundPhase.RoundEnd);
			}
		}

		private int CountRole(PlayerRole role)
		{
			int num = 0;
			foreach (PlayerRole value in roles.Values)
			{
				if (value == role)
				{
					num++;
				}
			}
			return num;
		}

		private void PublishAliveCounts()
		{
			if (base.IsServer)
			{
				AliveHunters.Value = CountRole(PlayerRole.Hunter);
				AliveHiders.Value = CountRole(PlayerRole.Hider);
			}
		}

		private void PublishHunterRoster()
		{
			if (!base.IsServer)
			{
				return;
			}
			HunterClientIds.Clear();
			foreach (KeyValuePair<ulong, PlayerRole> role in roles)
			{
				if (role.Value == PlayerRole.Hunter)
				{
					HunterClientIds.Add(role.Key);
				}
			}
		}

		private void PublishReveals()
		{
			if (!base.IsServer)
			{
				return;
			}
			Reveals.Clear();
			foreach (KeyValuePair<ulong, PlayerRole> roundStartRole in roundStartRoles)
			{
				Reveals.Add(new PlayerRevealEntry
				{
					ClientId = roundStartRole.Key,
					Role = roundStartRole.Value,
					Health = ResolveHealthOf(roundStartRole.Key)
				});
			}
		}

		private static int ResolveHealthOf(ulong clientId)
		{
			if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var value) || value.PlayerObject == null)
			{
				return 0;
			}
			PlayerHealth component = value.PlayerObject.GetComponent<PlayerHealth>();
			if (!(component != null))
			{
				return 0;
			}
			return component.Health.Value;
		}

		public bool ReportHiderFound(ulong clientId)
		{
			if (!base.IsServer || CurrentPhase.Value != RoundPhase.Hunt)
			{
				return false;
			}
			if (!roles.TryGetValue(clientId, out var value) || value != PlayerRole.Hider)
			{
				return false;
			}
			SetSpotted(clientId, spotted: false);
			Vector3? vector = ResolvePlayerPosition(clientId);
			CharacterVoice voice = ResolveVoice(clientId);
			roles[clientId] = PlayerRole.None;
			AssignRoleClientRpc(PlayerRole.None, TargetClient(clientId));
			EliminatedClientRpc(TargetClient(clientId));
			if (base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value2) && value2.PlayerObject != null)
			{
				DebrisClientRpc(new NetworkObjectReference(value2.PlayerObject));
			}
			ConcealAfterDeathRagdoll(clientId);
			if (ConvertsFoundHiders)
			{
				pendingConversion[clientId] = Time.timeAsDouble + (double)HiderConversionSeconds;
			}
			PublishAliveCounts();
			SystemChat("Chat.PlayerCaught", GetPlayerName(clientId));
			if (vector.HasValue)
			{
				KillFeedbackClientRpc(vector.Value, wasHunter: false, voice);
			}
			CheckHuntWinConditions();
			return true;
		}

		public void ReportHunterEliminated(ulong clientId)
		{
			if (base.IsServer && CurrentPhase.Value == RoundPhase.Hunt && roles.TryGetValue(clientId, out var value) && value == PlayerRole.Hunter)
			{
				Vector3? vector = ResolvePlayerPosition(clientId);
				CharacterVoice voice = ResolveVoice(clientId);
				roles[clientId] = PlayerRole.None;
				AssignRoleClientRpc(PlayerRole.None, TargetClient(clientId));
				EliminatedClientRpc(TargetClient(clientId));
				ConcealAfterDeathRagdoll(clientId);
				PublishAliveCounts();
				PublishHunterRoster();
				SystemChat("Chat.EliminatedForMissing", GetPlayerName(clientId));
				KillFeedClientRpc(clientId, clientId);
				if (vector.HasValue)
				{
					KillFeedbackClientRpc(vector.Value, wasHunter: true, voice);
				}
				CheckHuntWinConditions();
			}
		}

		public void ReportKill(ulong killerId, ulong victimId)
		{
			if (base.IsServer)
			{
				KillFeedClientRpc(killerId, victimId);
			}
		}

		[ClientRpc]
		private void KillFeedClientRpc(ulong killerId, ulong victimId)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(3483274555u, clientRpcParams, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, killerId);
				BytePacker.WriteValueBitPacked(bufferWriter, victimId);
				__endSendClientRpc(ref bufferWriter, 3483274555u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsClient && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (!(KillFeedView.Instance == null))
			{
				if (killerId == victimId)
				{
					KillFeedView.Instance.ReportSelfElimination(GetPlayerName(victimId));
				}
				else
				{
					KillFeedView.Instance.ReportFound(GetPlayerName(killerId), GetPlayerName(victimId));
				}
			}
		}

		private static CharacterVoice ResolveVoice(ulong clientId)
		{
			if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var value) || value.PlayerObject == null)
			{
				return CharacterVoice.Male;
			}
			PlayerCharacterAppearance component = value.PlayerObject.GetComponent<PlayerCharacterAppearance>();
			if (!(component != null))
			{
				return CharacterVoice.Male;
			}
			return component.VoiceType;
		}

		private static Vector3? ResolvePlayerPosition(ulong clientId)
		{
			if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var value) || value.PlayerObject == null)
			{
				return null;
			}
			return value.PlayerObject.transform.position;
		}

		[ClientRpc]
		private void KillFeedbackClientRpc(Vector3 position, bool wasHunter, CharacterVoice voice)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(4095671987u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in position);
				bufferWriter.WriteValueSafe(in wasHunter, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in voice, default(FastBufferWriter.ForEnums));
				__endSendClientRpc(ref bufferWriter, 4095671987u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ImpactEffects.SpawnKillEffect(position);
				ImpactEffects.SpawnDeathSound(position, wasHunter, voice);
			}
		}

		[ClientRpc]
		private void DebrisClientRpc(NetworkObjectReference victim)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(3986661972u, clientRpcParams, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in victim, default(FastBufferWriter.ForNetworkSerializable));
				__endSendClientRpc(ref bufferWriter, 3986661972u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (victim.TryGet(out var networkObject))
				{
					networkObject.GetComponent<PlayerDebris>()?.Explode();
				}
			}
		}

		private bool AnyRoleRemaining(PlayerRole role)
		{
			foreach (PlayerRole value in roles.Values)
			{
				if (value == role)
				{
					return true;
				}
			}
			return false;
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void SubmitChatServerRpc(FixedString128Bytes message, RpcParams rpcParams = default(RpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcParams rpcParams2 = rpcParams;
				RpcAttribute.RpcAttributeParams attributeParams = new RpcAttribute.RpcAttributeParams
				{
					InvokePermission = RpcInvokePermission.Everyone
				};
				FastBufferWriter bufferWriter = __beginSendRpc(467666238u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in message, default(FastBufferWriter.ForFixedStrings));
				__endSendRpc(ref bufferWriter, 467666238u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			ulong senderClientId = rpcParams.Receive.SenderClientId;
			if (!nextChatAllowedServerTime.TryGetValue(senderClientId, out var value) || !(base.NetworkManager.ServerTime.Time < value))
			{
				nextChatAllowedServerTime[senderClientId] = base.NetworkManager.ServerTime.Time + 0.5;
				string text = SanitizeChat(message.ToString());
				if (!string.IsNullOrEmpty(text))
				{
					ChatClientRpc("<color=#FFD24A>" + GetPlayerName(senderClientId) + "</color>: " + text);
					BubblesOf(senderClientId)?.ServerShow(text, ChatBubbleKind.Message);
				}
			}
		}

		private static string SanitizeChat(string raw)
		{
			if (string.IsNullOrWhiteSpace(raw))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(raw.Length);
			string text = raw.Trim();
			foreach (char c in text)
			{
				if (!char.IsControl(c))
				{
					switch (c)
					{
					case '<':
						stringBuilder.Append('‹');
						break;
					case '>':
						stringBuilder.Append('›');
						break;
					default:
						stringBuilder.Append(c);
						break;
					}
					if (stringBuilder.Length >= 80)
					{
						break;
					}
				}
			}
			return stringBuilder.ToString().Trim();
		}

		[ClientRpc]
		private void ChatClientRpc(string composed)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(823790119u, clientRpcParams, RpcDelivery.Reliable);
				bool value = composed != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(composed);
				}
				__endSendClientRpc(ref bufferWriter, 823790119u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (ChatView.Instance != null)
				{
					ChatView.Instance.AddPlayerMessage(composed);
				}
			}
		}

		[ClientRpc]
		private void AnnounceClientRpc(string key)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				ClientRpcParams clientRpcParams = default(ClientRpcParams);
				FastBufferWriter bufferWriter = __beginSendClientRpc(525344040u, clientRpcParams, RpcDelivery.Reliable);
				bool value = key != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(key);
				}
				__endSendClientRpc(ref bufferWriter, 525344040u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				string message = Loc.Get(key);
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(message, 4f);
				}
				if (ChatView.Instance != null)
				{
					ChatView.Instance.AddSystemMessage(message);
				}
			}
		}

		protected override void __initializeVariables()
		{
			if (AliveHunters == null)
			{
				throw new Exception("RoundManager.AliveHunters cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			AliveHunters.Initialize(this);
			__nameNetworkVariable(AliveHunters, "AliveHunters");
			NetworkVariableFields.Add(AliveHunters);
			if (AliveHiders == null)
			{
				throw new Exception("RoundManager.AliveHiders cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			AliveHiders.Initialize(this);
			__nameNetworkVariable(AliveHiders, "AliveHiders");
			NetworkVariableFields.Add(AliveHiders);
			if (lastRoundWinner == null)
			{
				throw new Exception("RoundManager.lastRoundWinner cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			lastRoundWinner.Initialize(this);
			__nameNetworkVariable(lastRoundWinner, "lastRoundWinner");
			NetworkVariableFields.Add(lastRoundWinner);
			if (CinematicEndServerTime == null)
			{
				throw new Exception("RoundManager.CinematicEndServerTime cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			CinematicEndServerTime.Initialize(this);
			__nameNetworkVariable(CinematicEndServerTime, "CinematicEndServerTime");
			NetworkVariableFields.Add(CinematicEndServerTime);
			if (MvpClientId == null)
			{
				throw new Exception("RoundManager.MvpClientId cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			MvpClientId.Initialize(this);
			__nameNetworkVariable(MvpClientId, "MvpClientId");
			NetworkVariableFields.Add(MvpClientId);
			if (MvpReason == null)
			{
				throw new Exception("RoundManager.MvpReason cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			MvpReason.Initialize(this);
			__nameNetworkVariable(MvpReason, "MvpReason");
			NetworkVariableFields.Add(MvpReason);
			if (MvpValue == null)
			{
				throw new Exception("RoundManager.MvpValue cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			MvpValue.Initialize(this);
			__nameNetworkVariable(MvpValue, "MvpValue");
			NetworkVariableFields.Add(MvpValue);
			if (HunterClientIds == null)
			{
				throw new Exception("RoundManager.HunterClientIds cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			HunterClientIds.Initialize(this);
			__nameNetworkVariable(HunterClientIds, "HunterClientIds");
			NetworkVariableFields.Add(HunterClientIds);
			if (DuelWins == null)
			{
				throw new Exception("RoundManager.DuelWins cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			DuelWins.Initialize(this);
			__nameNetworkVariable(DuelWins, "DuelWins");
			NetworkVariableFields.Add(DuelWins);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(1543445559u, __rpc_handler_1543445559, "RequestStartRoundServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(56261267u, __rpc_handler_56261267, "RequestRestartRoundServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(1868337822u, __rpc_handler_1868337822, "MvpAnnouncedClientRpc", RpcInvokePermission.Server);
			__registerRpc(3461441085u, __rpc_handler_3461441085, "AssignRoleClientRpc", RpcInvokePermission.Server);
			__registerRpc(317198445u, __rpc_handler_317198445, "EliminatedClientRpc", RpcInvokePermission.Server);
			__registerRpc(1350239846u, __rpc_handler_1350239846, "RequestDuelServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(1146634388u, __rpc_handler_1146634388, "DuelStateClientRpc", RpcInvokePermission.Server);
			__registerRpc(222375821u, __rpc_handler_222375821, "RequestRolePreferenceServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(3844162082u, __rpc_handler_3844162082, "ConfirmRolePreferenceClientRpc", RpcInvokePermission.Server);
			__registerRpc(1940732309u, __rpc_handler_1940732309, "RequestReadyServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(3483274555u, __rpc_handler_3483274555, "KillFeedClientRpc", RpcInvokePermission.Server);
			__registerRpc(4095671987u, __rpc_handler_4095671987, "KillFeedbackClientRpc", RpcInvokePermission.Server);
			__registerRpc(3986661972u, __rpc_handler_3986661972, "DebrisClientRpc", RpcInvokePermission.Server);
			__registerRpc(467666238u, __rpc_handler_467666238, "SubmitChatServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(823790119u, __rpc_handler_823790119, "ChatClientRpc", RpcInvokePermission.Server);
			__registerRpc(525344040u, __rpc_handler_525344040, "AnnounceClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_1543445559(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).RequestStartRoundServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_56261267(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).RequestRestartRoundServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1868337822(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).MvpAnnouncedClientRpc(client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3461441085(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out PlayerRole value, default(FastBufferWriter.ForEnums));
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).AssignRoleClientRpc(value, client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_317198445(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).EliminatedClientRpc(client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1350239846(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).RequestDuelServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1146634388(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out DuelState value, default(FastBufferWriter.ForEnums));
				reader.ReadValueSafe(out bool value2, default(FastBufferWriter.ForPrimitives));
				string s = null;
				if (value2)
				{
					reader.ReadValueSafe(out s, false);
				}
				reader.ReadValueSafe(out bool value3, default(FastBufferWriter.ForPrimitives));
				string s2 = null;
				if (value3)
				{
					reader.ReadValueSafe(out s2, false);
				}
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).DuelStateClientRpc(value, s, s2, client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_222375821(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out PlayerRole value, default(FastBufferWriter.ForEnums));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).RequestRolePreferenceServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3844162082(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out PlayerRole value, default(FastBufferWriter.ForEnums));
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).ConfirmRolePreferenceClientRpc(value, client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1940732309(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).RequestReadyServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3483274555(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value);
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value2);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).KillFeedClientRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_4095671987(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out Vector3 value);
				reader.ReadValueSafe(out bool value2, default(FastBufferWriter.ForPrimitives));
				reader.ReadValueSafe(out CharacterVoice value3, default(FastBufferWriter.ForEnums));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).KillFeedbackClientRpc(value, value2, value3);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3986661972(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out NetworkObjectReference value, default(FastBufferWriter.ForNetworkSerializable));
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).DebrisClientRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_467666238(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out FixedString128Bytes value, default(FastBufferWriter.ForFixedStrings));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).SubmitChatServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_823790119(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				string s = null;
				if (value)
				{
					reader.ReadValueSafe(out s, false);
				}
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).ChatClientRpc(s);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_525344040(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				string s = null;
				if (value)
				{
					reader.ReadValueSafe(out s, false);
				}
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((RoundManager)target).AnnounceClientRpc(s);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "RoundManager";
		}
	}
}
