using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.UI;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	public class DeathmatchRoundManager : GameModeController
	{
		public enum Stage : byte
		{
			Warmup = 0,
			Countdown = 1,
			Playing = 2,
			MatchEnd = 3
		}

		[Header("Varsayılanlar - lobi ayarı boş (0) ise")]
		[SerializeField]
		[Min(1f)]
		private int defaultMinPlayers = 2;

		[SerializeField]
		[Min(1f)]
		private int defaultScoreLimit = 30;

		[SerializeField]
		[Min(0f)]
		private int defaultTimeLimitSeconds = 600;

		[SerializeField]
		[Min(0f)]
		private int defaultExtraWarmupSeconds = 10;

		[SerializeField]
		[Min(0f)]
		private int defaultRespawnSeconds = 3;

		[Tooltip("Maç bitince skor tablosunun ekranda kaldığı süre, saniye - sonra ısınmaya dönülür ve yeterli oyuncu varsa geri sayım yeniden başlar.")]
		[SerializeField]
		[Min(1f)]
		private float matchEndSeconds = 10f;

		[Tooltip("Bir öldürmenin puanı.")]
		[SerializeField]
		[Min(1f)]
		private int pointsPerKill = 1;

		[Tooltip("Bir yumruğun verdiği hasar - yere yıkmanın yanında. Can 100.")]
		[SerializeField]
		[Min(0f)]
		private int punchDamage = 15;

		[Tooltip("Yerde yatan bir bedene sıkılan merminin, isabet ettiği kemiğe verdiği hız, m/sn. Mermi yine içinden geçer; bu yalnızca bedenin tepki vermesi için. Küçük tut - 0.5 civarı. 0 = kapalı.")]
		[SerializeField]
		[Min(0f)]
		private float downedShotImpulse = 0.6f;

		[Header("Can yenilenmesi (Call of Duty tarzı)")]
		[Tooltip("Son hasardan kaç saniye sonra can dolmaya başlar. 0 = yenilenme yok.")]
		[SerializeField]
		[Min(0f)]
		private float regenDelaySeconds = 5f;

		[Tooltip("Dolarken saniyede kaç puan. 100 can, 25 = dört saniyede tam.")]
		[SerializeField]
		[Min(0f)]
		private float regenPerSecond = 25f;

		public readonly NetworkVariable<Stage> CurrentStage = new NetworkVariable<Stage>(Stage.Warmup);

		public readonly NetworkVariable<int> MinPlayers = new NetworkVariable<int>(0);

		public readonly NetworkVariable<int> ScoreLimit = new NetworkVariable<int>(0);

		public readonly NetworkVariable<int> TimeLimitSeconds = new NetworkVariable<int>(0);

		public readonly NetworkVariable<int> RespawnSeconds = new NetworkVariable<int>(0);

		public readonly NetworkVariable<ulong> Winner = new NetworkVariable<ulong>(ulong.MaxValue);

		public readonly NetworkVariable<int> PlayerCount = new NetworkVariable<int>(0);

		private ulong lastKillerId = ulong.MaxValue;

		private readonly HashSet<ulong> dead = new HashSet<ulong>();

		private readonly Dictionary<ulong, double> respawnAt = new Dictionary<ulong, double>();

		private readonly HashSet<ulong> untoldDeaths = new HashSet<ulong>();

		private readonly List<ulong> scratch = new List<ulong>();

		private readonly Dictionary<ulong, WeaponDefinition> nextWeapon = new Dictionary<ulong, WeaponDefinition>();

		private Transform[] spawnPoints = Array.Empty<Transform>();

		private int spawnIndex;

		private LobbySettingsSync lobbySettings;

		private float nextSettingsLookup;

		private int extraWarmupSeconds;

		private const float SafeSpawnDistance = 12f;

		private const float FarEnoughShare = 0.6f;

		private const int RecentSpawnMemory = 3;

		private readonly List<Transform> spawnCandidates = new List<Transform>();

		private readonly List<float> spawnDistances = new List<float>();

		private readonly List<Transform> recentSpawns = new List<Transform>();

		private readonly Dictionary<ulong, Transform> lastSpawnOf = new Dictionary<ulong, Transform>();

		public bool IsLocalDead { get; private set; }

		public double LocalRespawnAtServerTime { get; private set; }

		private double Now => base.NetworkManager.ServerTime.Time;

		public override MapMarkerModes MarkerMode => MapMarkerModes.Deathmatch;

		public override bool IsLocalPlayerArmed => true;

		public override bool LocalPlayerUsesHunterMovement => true;

		public override bool LocalPlayerMayShoot
		{
			get
			{
				if (!IsLocalDead)
				{
					return CurrentStage.Value != Stage.MatchEnd;
				}
				return false;
			}
		}

		public override bool PenalisesMissedShots => false;

		public override bool LocalPlayerShouldHaveModel => false;

		public override bool CanLocalPlayerEditModel => false;

		public override bool IsModelSetupWindow => false;

		public override bool LocalPlayerMayWallClimb => false;

		public override bool IsLocalPlayerFrozen
		{
			get
			{
				if (!IsLocalDead)
				{
					return CurrentStage.Value == Stage.MatchEnd;
				}
				return true;
			}
		}

		public override bool IsLocalPlayerParticipating => !IsLocalDead;

		public override bool UsesRolePreference => false;

		public override bool PlaysHitConfirm => false;

		public override int PunchDamage => punchDamage;

		public override float ShotImpulseOnDowned => downedShotImpulse;

		public override bool ShowsDamageDirection => true;

		public bool WeaponMenuOpen { get; set; }

		public override bool LocalPlayerNeedsCursor => WeaponMenuOpen;

		public bool CanChooseWeapon => CurrentStage.Value != Stage.MatchEnd;

		public bool WeaponChoiceAppliesNow
		{
			get
			{
				if (!IsLocalDead && CurrentStage.Value != Stage.Warmup)
				{
					return CurrentStage.Value == Stage.Countdown;
				}
				return true;
			}
		}

		public override bool IsRoundRunning
		{
			get
			{
				if (CurrentStage.Value != Stage.Countdown)
				{
					return CurrentStage.Value == Stage.Playing;
				}
				return true;
			}
		}

		public override bool SupportsHostRoundControls => true;

		public override MusicCue Music => CurrentStage.Value switch
		{
			Stage.Countdown => MusicCue.Prep, 
			Stage.Playing => MusicCue.Hunt, 
			Stage.MatchEnd => MusicCue.RoundEnd, 
			_ => MusicCue.Lobby, 
		};

		public override ulong HighlightedClientId
		{
			get
			{
				if (CurrentStage.Value != Stage.MatchEnd)
				{
					return ulong.MaxValue;
				}
				return Winner.Value;
			}
		}

		public override bool ServerAllowsShooting(ulong clientId)
		{
			if (!dead.Contains(clientId))
			{
				return CurrentStage.Value != Stage.MatchEnd;
			}
			return false;
		}

		public override bool ServerAllowsBodyEdit(ulong clientId)
		{
			return false;
		}

		public override bool ShouldShowNameTag(ulong clientId)
		{
			return true;
		}

		public void RequestWeapon(string weaponId)
		{
			if (!string.IsNullOrEmpty(weaponId) && base.IsSpawned)
			{
				RequestWeaponServerRpc(new FixedString32Bytes(weaponId));
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void RequestWeaponServerRpc(FixedString32Bytes weaponId, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(1577665701u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in weaponId, default(FastBufferWriter.ForFixedStrings));
				__endSendRpc(ref bufferWriter, 1577665701u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			ulong senderClientId = rpcParams.Receive.SenderClientId;
			if (CurrentStage.Value == Stage.MatchEnd)
			{
				return;
			}
			WeaponDefinition weaponDefinition = WeaponCatalog.Find(weaponId.ToString());
			if (weaponDefinition == null || weaponDefinition.HeldPrefab == null)
			{
				return;
			}
			PlayerWeapons playerWeapons = ComponentOf<PlayerWeapons>(senderClientId);
			if (dead.Contains(senderClientId) || CurrentStage.Value == Stage.Warmup || CurrentStage.Value == Stage.Countdown)
			{
				nextWeapon.Remove(senderClientId);
				if (playerWeapons != null)
				{
					playerWeapons.ServerEquip(weaponDefinition);
				}
			}
			else if (playerWeapons != null && playerWeapons.Equipped == weaponDefinition)
			{
				nextWeapon.Remove(senderClientId);
			}
			else
			{
				nextWeapon[senderClientId] = weaponDefinition;
			}
		}

		public override void RequestStartRound()
		{
			RequestStartMatchServerRpc();
		}

		public override void RequestRestartRound()
		{
			RequestRestartMatchServerRpc();
		}

		public override void FillLobbyInfo(LobbySettingsData settings, List<LobbyInfoRow> rows)
		{
			base.FillLobbyInfo(settings, rows);
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.MinPlayers"), Or(settings.MinPlayers, defaultMinPlayers).ToString()));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.ScoreLimit"), Or(settings.ScoreLimit, defaultScoreLimit).ToString()));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.TimeLimit"), Seconds(Or(settings.TimeLimitSeconds, defaultTimeLimitSeconds))));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.Warmup"), Seconds(Or(settings.ExtraWarmupSeconds, defaultExtraWarmupSeconds))));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.Respawn"), Seconds(Or(settings.RespawnSeconds, defaultRespawnSeconds))));
		}

		private static string Seconds(int seconds)
		{
			return string.Format(Loc.Get("Lobby.Row.Seconds"), seconds);
		}

		private static int Or(int value, int fallback)
		{
			if (value <= 0)
			{
				return fallback;
			}
			return value;
		}

		public override LobbySettingsData WithDefaults(LobbySettingsData settings)
		{
			settings = base.WithDefaults(settings);
			settings.MinPlayers = Or(settings.MinPlayers, defaultMinPlayers);
			settings.ScoreLimit = Or(settings.ScoreLimit, defaultScoreLimit);
			settings.TimeLimitSeconds = Or(settings.TimeLimitSeconds, defaultTimeLimitSeconds);
			settings.ExtraWarmupSeconds = Or(settings.ExtraWarmupSeconds, defaultExtraWarmupSeconds);
			settings.RespawnSeconds = Or(settings.RespawnSeconds, defaultRespawnSeconds);
			return settings;
		}

		public override bool TryGetHealthRegen(out float delaySeconds, out float perSecond)
		{
			delaySeconds = regenDelaySeconds;
			perSecond = regenPerSecond;
			if (regenDelaySeconds > 0f)
			{
				return regenPerSecond > 0f;
			}
			return false;
		}

		public override bool ServerIsShotTarget(ulong clientId)
		{
			return !dead.Contains(clientId);
		}

		public override bool ServerMayDamage(ulong shooterId, ulong victimId)
		{
			if (shooterId != victimId && !dead.Contains(victimId))
			{
				return CurrentStage.Value != Stage.MatchEnd;
			}
			return false;
		}

		public override bool ServerOnPlayerDied(ulong clientId)
		{
			if (!base.IsServer || dead.Contains(clientId))
			{
				return false;
			}
			dead.Add(clientId);
			respawnAt[clientId] = Now + (double)Mathf.Max(0, RespawnSeconds.Value);
			untoldDeaths.Add(clientId);
			PlayerRagdoll playerRagdoll = ComponentOf<PlayerRagdoll>(clientId);
			if (playerRagdoll != null)
			{
				playerRagdoll.ServerBeginDeathRagdoll();
			}
			return true;
		}

		public override void ServerReportKill(ulong killerId, ulong victimId)
		{
			if (!base.IsServer)
			{
				return;
			}
			int num;
			if (CurrentStage.Value == Stage.Playing)
			{
				num = ((killerId != victimId) ? 1 : 0);
				if (num != 0)
				{
					AddScore(killerId, pointsPerKill);
				}
			}
			else
			{
				num = 0;
			}
			KillFeedClientRpc(killerId, victimId);
			TellDeath(victimId, killerId);
			if (killerId != victimId)
			{
				KillConfirmClientRpc(TargetClient(killerId));
			}
			if (num != 0 && ScoreLimit.Value > 0 && ScoreOf(killerId) >= ScoreLimit.Value)
			{
				EndMatch();
			}
		}

		private void TellDeath(ulong victimId, ulong killerId)
		{
			if (untoldDeaths.Remove(victimId))
			{
				DiedClientRpc(killerId, respawnAt.TryGetValue(victimId, out var value) ? value : Now, TargetClient(victimId));
			}
		}

		private void FlushDeathNotices()
		{
			if (untoldDeaths.Count == 0)
			{
				return;
			}
			scratch.Clear();
			scratch.AddRange(untoldDeaths);
			foreach (ulong item in scratch)
			{
				TellDeath(item, item);
			}
		}

		public override string LocalDeathSubtitle(PlayerRole lastRole)
		{
			if (lastKillerId == ulong.MaxValue || base.NetworkManager == null || lastKillerId == base.NetworkManager.LocalClientId)
			{
				return Loc.Get("Death.Self");
			}
			return string.Format(Loc.Get("Death.KilledBy"), GetPlayerName(lastKillerId));
		}

		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();
			base.LocalRole = PlayerRole.Hunter;
			if (base.IsServer)
			{
				base.NetworkManager.OnClientConnectedCallback += OnClientConnected;
				base.NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
				PublishSettings();
				SetStage(Stage.Warmup, RoundPhase.Prep);
				PhaseEndServerTime.Value = 0.0;
				PlayerCount.Value = base.NetworkManager.ConnectedClientsList.Count;
			}
		}

		public override void OnNetworkDespawn()
		{
			if (base.IsServer && base.NetworkManager != null)
			{
				base.NetworkManager.OnClientConnectedCallback -= OnClientConnected;
				base.NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
			}
			if (lobbySettings != null)
			{
				lobbySettings.SettingsChanged -= OnSettingsChanged;
			}
			base.OnNetworkDespawn();
		}

		private void OnClientConnected(ulong clientId)
		{
			PlayerCount.Value = base.NetworkManager.ConnectedClientsList.Count;
		}

		private void OnClientDisconnected(ulong clientId)
		{
			RemoveName(clientId);
			RemoveScore(clientId);
			dead.Remove(clientId);
			respawnAt.Remove(clientId);
			untoldDeaths.Remove(clientId);
			nextWeapon.Remove(clientId);
			lastSpawnOf.Remove(clientId);
			PlayerCount.Value = Mathf.Max(0, base.NetworkManager.ConnectedClientsList.Count - 1);
		}

		private void PublishSettings()
		{
			if (lobbySettings == null)
			{
				lobbySettings = UnityEngine.Object.FindFirstObjectByType<LobbySettingsSync>();
				if (lobbySettings != null)
				{
					lobbySettings.SettingsChanged += OnSettingsChanged;
				}
			}
			LobbySettingsData data = ((lobbySettings != null) ? lobbySettings.CurrentSettings : default(LobbySettingsData));
			OnSettingsChanged(data);
		}

		private void OnSettingsChanged(LobbySettingsData data)
		{
			if (base.IsServer)
			{
				MinPlayers.Value = Or(data.MinPlayers, defaultMinPlayers);
				ScoreLimit.Value = Or(data.ScoreLimit, defaultScoreLimit);
				TimeLimitSeconds.Value = ((data.TimeLimitSeconds > 0) ? data.TimeLimitSeconds : defaultTimeLimitSeconds);
				RespawnSeconds.Value = ((data.RespawnSeconds > 0) ? data.RespawnSeconds : defaultRespawnSeconds);
				extraWarmupSeconds = ((data.ExtraWarmupSeconds > 0) ? data.ExtraWarmupSeconds : defaultExtraWarmupSeconds);
			}
		}

		private void Update()
		{
			if (!base.IsServer)
			{
				return;
			}
			SampleLatencies();
			if (lobbySettings == null && Time.time >= nextSettingsLookup)
			{
				nextSettingsLookup = Time.time + 0.5f;
				PublishSettings();
			}
			TickRespawns();
			switch (CurrentStage.Value)
			{
			case Stage.Warmup:
				if (PlayerCount.Value >= MinPlayers.Value)
				{
					BeginCountdown();
				}
				break;
			case Stage.Countdown:
				if (PlayerCount.Value < MinPlayers.Value)
				{
					BeginWarmup();
				}
				else if (Now >= PhaseEndServerTime.Value)
				{
					BeginMatch();
				}
				break;
			case Stage.Playing:
				if (TimeLimitSeconds.Value > 0 && Now >= PhaseEndServerTime.Value)
				{
					EndMatch();
				}
				break;
			case Stage.MatchEnd:
				if (Now >= PhaseEndServerTime.Value)
				{
					BeginWarmup();
				}
				break;
			}
		}

		private void LateUpdate()
		{
			if (base.IsServer)
			{
				FlushDeathNotices();
			}
		}

		private void SetStage(Stage stage, RoundPhase phase)
		{
			CurrentStage.Value = stage;
			CurrentPhase.Value = phase;
		}

		private void BeginWarmup()
		{
			SetStage(Stage.Warmup, RoundPhase.Prep);
			PhaseEndServerTime.Value = 0.0;
		}

		private void BeginCountdown()
		{
			SetStage(Stage.Countdown, RoundPhase.Prep);
			PhaseEndServerTime.Value = Now + (double)Mathf.Max(0, extraWarmupSeconds);
		}

		private void BeginMatch()
		{
			Scores.Clear();
			Winner.Value = ulong.MaxValue;
			RespawnEverybody();
			SetStage(Stage.Playing, RoundPhase.Hunt);
			PhaseEndServerTime.Value = ((TimeLimitSeconds.Value > 0) ? (Now + (double)TimeLimitSeconds.Value) : 0.0);
			MatchStartedClientRpc();
			SystemChat("Chat.MatchStarted");
		}

		private void EndMatch()
		{
			if (CurrentStage.Value != Stage.MatchEnd)
			{
				Winner.Value = TopScorer(out var tie);
				SetStage(Stage.MatchEnd, RoundPhase.RoundEnd);
				PhaseEndServerTime.Value = Now + (double)matchEndSeconds;
				if (tie || Winner.Value == ulong.MaxValue)
				{
					SystemChat("Chat.MatchDraw");
					return;
				}
				SystemChat("Chat.MatchWon", GetPlayerName(Winner.Value));
				MvpAnnouncedClientRpc(TargetClient(Winner.Value));
			}
		}

		[ClientRpc]
		private void MvpAnnouncedClientRpc(ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(3933707521u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 3933707521u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				AudioLibrary instance = AudioLibrary.Instance;
				if (!(instance == null))
				{
					AudioLibrary.PlayOneShotClip((instance.deathmatchMvpClip != null) ? instance.deathmatchMvpClip : instance.mvpClip);
				}
			}
		}

		private ulong TopScorer(out bool tie)
		{
			ulong result = ulong.MaxValue;
			int num = int.MinValue;
			tie = false;
			for (int i = 0; i < Scores.Count; i++)
			{
				if (Scores[i].Score > num)
				{
					num = Scores[i].Score;
					result = Scores[i].ClientId;
					tie = false;
				}
				else if (Scores[i].Score == num)
				{
					tie = true;
				}
			}
			if (!tie)
			{
				return result;
			}
			return ulong.MaxValue;
		}

		private int ScoreOf(ulong clientId)
		{
			for (int i = 0; i < Scores.Count; i++)
			{
				if (Scores[i].ClientId == clientId)
				{
					return Scores[i].Score;
				}
			}
			return 0;
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void RequestStartMatchServerRpc(RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(1887724711u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 1887724711u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (rpcParams.Receive.SenderClientId == 0L && !IsRoundRunning)
				{
					SystemChat("Chat.GameStarted", GetPlayerName(0uL));
					BeginMatch();
				}
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void RequestRestartMatchServerRpc(RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(4194562217u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 4194562217u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (rpcParams.Receive.SenderClientId == 0L && IsRoundRunning)
				{
					SystemChat("Chat.GameRestarted", GetPlayerName(0uL));
					BeginMatch();
				}
			}
		}

		private void TickRespawns()
		{
			if (respawnAt.Count == 0)
			{
				return;
			}
			scratch.Clear();
			foreach (KeyValuePair<ulong, double> item in respawnAt)
			{
				if (Now >= item.Value)
				{
					scratch.Add(item.Key);
				}
			}
			foreach (ulong item2 in scratch)
			{
				Respawn(item2, announce: true);
			}
		}

		private void Respawn(ulong clientId, bool announce)
		{
			respawnAt.Remove(clientId);
			dead.Remove(clientId);
			untoldDeaths.Remove(clientId);
			if (!base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) || value.PlayerObject == null)
			{
				return;
			}
			PlayerRagdoll component = value.PlayerObject.GetComponent<PlayerRagdoll>();
			if (component != null)
			{
				component.ServerClearRagdoll();
			}
			PlayerHealth component2 = value.PlayerObject.GetComponent<PlayerHealth>();
			if (component2 != null)
			{
				component2.ServerResetHealth();
			}
			if (nextWeapon.TryGetValue(clientId, out var value2))
			{
				nextWeapon.Remove(clientId);
				PlayerWeapons component3 = value.PlayerObject.GetComponent<PlayerWeapons>();
				if (component3 != null && value2 != null)
				{
					component3.ServerEquip(value2);
				}
			}
			PlayerMovement component4 = value.PlayerObject.GetComponent<PlayerMovement>();
			if (component4 != null && TryPickSpawn(clientId, out var position, out var rotation))
			{
				component4.TeleportClientRpc(position, rotation);
			}
			if (announce)
			{
				RespawnedClientRpc(TargetClient(clientId));
			}
		}

		private void RespawnEverybody()
		{
			scratch.Clear();
			foreach (NetworkClient connectedClients in base.NetworkManager.ConnectedClientsList)
			{
				scratch.Add(connectedClients.ClientId);
			}
			foreach (ulong item in scratch)
			{
				Respawn(item, announce: false);
			}
		}

		private bool TryPickSpawn(ulong forClient, out Vector3 position, out Quaternion rotation)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			spawnCandidates.Clear();
			spawnDistances.Clear();
			float num = 0f;
			Transform[] array = spawnPoints;
			foreach (Transform transform in array)
			{
				if (transform == null)
				{
					continue;
				}
				float num2 = float.MaxValue;
				foreach (NetworkClient connectedClients in base.NetworkManager.ConnectedClientsList)
				{
					if (connectedClients.ClientId != forClient && !(connectedClients.PlayerObject == null) && !dead.Contains(connectedClients.ClientId))
					{
						num2 = Mathf.Min(num2, (connectedClients.PlayerObject.transform.position - transform.position).magnitude);
					}
				}
				spawnCandidates.Add(transform);
				spawnDistances.Add(num2);
				num = Mathf.Max(num, num2);
			}
			if (spawnCandidates.Count == 0)
			{
				return TryGetLobbySpawn(out position, out rotation);
			}
			float num3 = Mathf.Min(12f, num * 0.6f);
			for (int num4 = spawnCandidates.Count - 1; num4 >= 0; num4--)
			{
				if (spawnDistances[num4] < num3)
				{
					spawnCandidates.RemoveAt(num4);
				}
			}
			lastSpawnOf.TryGetValue(forClient, out var value);
			int recent = Mathf.Min(3, spawnCandidates.Count / 3);
			Transform transform2 = PickAvoiding(value, recent) ?? PickAvoiding(value, 0) ?? PickAvoiding(null, 0);
			if (transform2 == null)
			{
				return TryGetLobbySpawn(out position, out rotation);
			}
			lastSpawnOf[forClient] = transform2;
			recentSpawns.Remove(transform2);
			recentSpawns.Add(transform2);
			while (recentSpawns.Count > 3)
			{
				recentSpawns.RemoveAt(0);
			}
			position = transform2.position;
			rotation = transform2.rotation;
			return true;
		}

		private Transform PickAvoiding(Transform avoid, int recent)
		{
			int start = Mathf.Max(0, recentSpawns.Count - recent);
			int num = 0;
			foreach (Transform spawnCandidate in spawnCandidates)
			{
				if (Allowed(spawnCandidate))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			foreach (Transform spawnCandidate2 in spawnCandidates)
			{
				if (Allowed(spawnCandidate2) && num2-- == 0)
				{
					return spawnCandidate2;
				}
			}
			return null;
			bool Allowed(Transform point)
			{
				if (point == avoid)
				{
					return false;
				}
				for (int i = start; i < recentSpawns.Count; i++)
				{
					if (recentSpawns[i] == point)
					{
						return false;
					}
				}
				return true;
			}
		}

		[ClientRpc]
		private void DiedClientRpc(ulong killerId, double respawnAtServerTime, ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(2561535777u, clientRpcParams, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, killerId);
				bufferWriter.WriteValueSafe(in respawnAtServerTime, default(FastBufferWriter.ForPrimitives));
				__endSendClientRpc(ref bufferWriter, 2561535777u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				IsLocalDead = true;
				LocalRespawnAtServerTime = respawnAtServerTime;
				lastKillerId = killerId;
				RaiseLocalEliminated();
			}
		}

		[ClientRpc]
		private void RespawnedClientRpc(ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(2922520989u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 2922520989u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				IsLocalDead = false;
				RaiseRoundRestarted();
			}
		}

		[ClientRpc]
		private void MatchStartedClientRpc()
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(522249452u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 522249452u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				IsLocalDead = false;
				RaiseRoundRestarted();
			}
		}

		[ClientRpc]
		private void KillConfirmClientRpc(ClientRpcParams clientRpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(2806695098u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 2806695098u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ImpactEffects.PlayKillConfirm();
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(1074496641u, clientRpcParams, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, killerId);
				BytePacker.WriteValueBitPacked(bufferWriter, victimId);
				__endSendClientRpc(ref bufferWriter, 1074496641u, clientRpcParams, RpcDelivery.Reliable);
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
					KillFeedView.Instance.ReportKill(GetPlayerName(killerId), GetPlayerName(victimId));
				}
			}
		}

		public override void SetMap(Transform[] hunterSpawnPoints, Transform[] hiderSpawnPoints, Transform[] lobbySpawnPoints, GameObject hunterDoor)
		{
			List<Transform> list = new List<Transform>();
			Append(list, hunterSpawnPoints);
			Append(list, hiderSpawnPoints);
			Append(list, lobbySpawnPoints);
			spawnPoints = list.ToArray();
			spawnIndex = 0;
			if (hunterDoor != null && hunterDoor.activeSelf)
			{
				hunterDoor.SetActive(value: false);
			}
			if (spawnPoints.Length != 0)
			{
				PlaceConnectedPlayersAtLobbySpawns();
			}
		}

		private static void Append(List<Transform> into, Transform[] points)
		{
			if (points == null)
			{
				return;
			}
			foreach (Transform transform in points)
			{
				if (transform != null && !into.Contains(transform))
				{
					into.Add(transform);
				}
			}
		}

		public override bool TryGetLobbySpawn(out Vector3 position, out Quaternion rotation)
		{
			for (int i = 0; i < spawnPoints.Length; i++)
			{
				Transform transform = spawnPoints[spawnIndex++ % spawnPoints.Length];
				if (!(transform == null))
				{
					position = transform.position;
					rotation = transform.rotation;
					return true;
				}
			}
			return base.TryGetLobbySpawn(out position, out rotation);
		}

		public override bool TryGetScoreHighlight(out string text)
		{
			text = "";
			if (CurrentStage.Value != Stage.MatchEnd)
			{
				return false;
			}
			text = ((Winner.Value == ulong.MaxValue) ? Loc.Get("Deathmatch.Draw") : string.Format(Loc.Get("Deathmatch.Winner"), GetPlayerName(Winner.Value)));
			return true;
		}

		private T ComponentOf<T>(ulong clientId) where T : Component
		{
			if (!base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) || !(value.PlayerObject != null))
			{
				return null;
			}
			return value.PlayerObject.GetComponent<T>();
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

		protected override void __initializeVariables()
		{
			if (CurrentStage == null)
			{
				throw new Exception("DeathmatchRoundManager.CurrentStage cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			CurrentStage.Initialize(this);
			__nameNetworkVariable(CurrentStage, "CurrentStage");
			NetworkVariableFields.Add(CurrentStage);
			if (MinPlayers == null)
			{
				throw new Exception("DeathmatchRoundManager.MinPlayers cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			MinPlayers.Initialize(this);
			__nameNetworkVariable(MinPlayers, "MinPlayers");
			NetworkVariableFields.Add(MinPlayers);
			if (ScoreLimit == null)
			{
				throw new Exception("DeathmatchRoundManager.ScoreLimit cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			ScoreLimit.Initialize(this);
			__nameNetworkVariable(ScoreLimit, "ScoreLimit");
			NetworkVariableFields.Add(ScoreLimit);
			if (TimeLimitSeconds == null)
			{
				throw new Exception("DeathmatchRoundManager.TimeLimitSeconds cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			TimeLimitSeconds.Initialize(this);
			__nameNetworkVariable(TimeLimitSeconds, "TimeLimitSeconds");
			NetworkVariableFields.Add(TimeLimitSeconds);
			if (RespawnSeconds == null)
			{
				throw new Exception("DeathmatchRoundManager.RespawnSeconds cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			RespawnSeconds.Initialize(this);
			__nameNetworkVariable(RespawnSeconds, "RespawnSeconds");
			NetworkVariableFields.Add(RespawnSeconds);
			if (Winner == null)
			{
				throw new Exception("DeathmatchRoundManager.Winner cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			Winner.Initialize(this);
			__nameNetworkVariable(Winner, "Winner");
			NetworkVariableFields.Add(Winner);
			if (PlayerCount == null)
			{
				throw new Exception("DeathmatchRoundManager.PlayerCount cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			PlayerCount.Initialize(this);
			__nameNetworkVariable(PlayerCount, "PlayerCount");
			NetworkVariableFields.Add(PlayerCount);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(1577665701u, __rpc_handler_1577665701, "RequestWeaponServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(3933707521u, __rpc_handler_3933707521, "MvpAnnouncedClientRpc", RpcInvokePermission.Server);
			__registerRpc(1887724711u, __rpc_handler_1887724711, "RequestStartMatchServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(4194562217u, __rpc_handler_4194562217, "RequestRestartMatchServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(2561535777u, __rpc_handler_2561535777, "DiedClientRpc", RpcInvokePermission.Server);
			__registerRpc(2922520989u, __rpc_handler_2922520989, "RespawnedClientRpc", RpcInvokePermission.Server);
			__registerRpc(522249452u, __rpc_handler_522249452, "MatchStartedClientRpc", RpcInvokePermission.Server);
			__registerRpc(2806695098u, __rpc_handler_2806695098, "KillConfirmClientRpc", RpcInvokePermission.Server);
			__registerRpc(1074496641u, __rpc_handler_1074496641, "KillFeedClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_1577665701(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out FixedString32Bytes value, default(FastBufferWriter.ForFixedStrings));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).RequestWeaponServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3933707521(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).MvpAnnouncedClientRpc(client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1887724711(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).RequestStartMatchServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_4194562217(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).RequestRestartMatchServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2561535777(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value);
				reader.ReadValueSafe(out double value2, default(FastBufferWriter.ForPrimitives));
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).DiedClientRpc(value, value2, client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2922520989(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).RespawnedClientRpc(client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_522249452(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).MatchStartedClientRpc();
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2806695098(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).KillConfirmClientRpc(client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1074496641(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value);
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value2);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((DeathmatchRoundManager)target).KillFeedClientRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "DeathmatchRoundManager";
		}
	}
}
