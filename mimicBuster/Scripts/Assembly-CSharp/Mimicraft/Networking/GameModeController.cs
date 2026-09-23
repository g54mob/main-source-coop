using System;
using System.Collections.Generic;
using System.Globalization;
using Mimicraft.Analytics;
using Mimicraft.Dev;
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
	public abstract class GameModeController : NetworkBehaviour
	{
		public readonly struct LobbyInfoRow
		{
			public readonly string Caption;

			public readonly string Value;

			public LobbyInfoRow(string caption, string value)
			{
				Caption = caption;
				Value = value;
			}
		}

		public readonly NetworkVariable<RoundPhase> CurrentPhase = new NetworkVariable<RoundPhase>(RoundPhase.WaitingForPlayers);

		public readonly NetworkVariable<double> PhaseEndServerTime = new NetworkVariable<double>(0.0);

		public readonly NetworkList<PlayerScoreEntry> Scores = new NetworkList<PlayerScoreEntry>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

		public readonly NetworkList<PlayerNameEntry> Names = new NetworkList<PlayerNameEntry>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

		public readonly NetworkList<PlayerRevealEntry> Reveals = new NetworkList<PlayerRevealEntry>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

		public const int DefaultSelfDamagePercent = 100;

		public const int MinSelfDamagePercent = 25;

		public const int MaxSelfDamagePercent = 200;

		public const int SelfDamageStepPercent = 25;

		public const int DefaultHunterSharePercent = 30;

		public const int MinHunterSharePercent = 5;

		public const int MaxHunterSharePercent = 95;

		public const int HunterShareStepPercent = 5;

		public readonly NetworkList<PlayerLatencyEntry> Latencies = new NetworkList<PlayerLatencyEntry>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

		private const float LatencySampleIntervalSeconds = 1f;

		private double nextLatencySampleServerTime;

		private readonly Dictionary<ulong, ushort> appLevelRtt = new Dictionary<ulong, ushort>();

		private uint pingToken;

		private double pingSentTime;

		private readonly List<ulong> departedLatencies = new List<ulong>();

		public const double VoteKickSeconds = 60.0;

		private const double VoteKickCooldownSeconds = 60.0;

		public readonly NetworkVariable<VoteKickState> VoteKick = new NetworkVariable<VoteKickState>();

		private readonly Dictionary<ulong, bool> voteAnswers = new Dictionary<ulong, bool>();

		private double voteAllowedAfterServerTime;

		public static GameModeController Current { get; private set; }

		public PlayerRole LocalRole { get; protected set; }

		public abstract MapMarkerModes MarkerMode { get; }

		public virtual bool CanLocalPlayerEditModel => false;

		public virtual bool IsRoundRunning => CurrentPhase.Value != RoundPhase.WaitingForPlayers;

		public virtual bool SupportsHostRoundControls => false;

		public virtual MusicCue Music => CurrentPhase.Value switch
		{
			RoundPhase.Prep => MusicCue.Prep, 
			RoundPhase.Hunt => MusicCue.Hunt, 
			RoundPhase.RoundEnd => MusicCue.RoundEnd, 
			_ => MusicCue.Lobby, 
		};

		public virtual bool IsModelSetupWindow => false;

		public virtual bool LocalPlayerShouldHaveModel => false;

		public virtual int MinBodyExtent => 4;

		public virtual int MinBodyIslandVoxels => 16;

		public virtual bool EnforcesBodyClearance => true;

		public virtual bool LocalPlayerUsesHunterMovement => false;

		public virtual bool IsLocalPlayerFrozen => false;

		public virtual bool IsLocalPlayerArmed => false;

		public virtual bool ModeUsesWeapons => true;

		public virtual bool LocalPlayerMayWallClimb => false;

		public virtual bool LocalPlayerMayLeaveEditMode => true;

		public virtual bool LocalPlayerNeedsCursor => false;

		public virtual bool UsesRolePreference => false;

		public virtual bool IsLocalPlayerParticipating => true;

		public virtual bool OffersCompanionChoice => false;

		public virtual bool LocalPlayerMayShoot => LocalRole == PlayerRole.Hunter;

		public virtual bool PlaysHitConfirm => true;

		public virtual int PunchDamage => 0;

		public virtual float ShotImpulseOnDowned => 0f;

		public virtual bool ShowsDamageDirection => false;

		public virtual bool PenalisesMissedShots => true;

		public virtual int HitHealthReward => 0;

		public int SelfDamagePercent => ResolveSelfDamagePercent((LobbySettingsSync.Instance != null) ? LobbySettingsSync.Instance.CurrentSettings.SelfDamagePercent : 0);

		public virtual bool ShowsScoreboardRole => false;

		public virtual bool ShowsScoreboardHealth => false;

		private static bool VoteKickAllowedHere
		{
			get
			{
				if (!LobbyBans.Supported)
				{
					return DevCheats.Enabled;
				}
				return true;
			}
		}

		public bool VoteKickAvailable
		{
			get
			{
				if (VoteKickAllowedHere && base.NetworkManager != null)
				{
					return base.NetworkManager.ConnectedClientsIds.Count > 2;
				}
				return false;
			}
		}

		public virtual ulong HighlightedClientId => ulong.MaxValue;

		public event Action OnLocalEliminated;

		public event Action OnRoundRestarted;

		private void PublishPhase(RoundPhase previous, RoundPhase current)
		{
			if (base.IsServer)
			{
				SteamLobbyData.PublishPhase(LobbyPhase.Of(current));
			}
		}

		private void BeginPublishingPhase()
		{
			NetworkVariable<RoundPhase> currentPhase = CurrentPhase;
			currentPhase.OnValueChanged = (NetworkVariable<RoundPhase>.OnValueChangedDelegate)Delegate.Remove(currentPhase.OnValueChanged, new NetworkVariable<RoundPhase>.OnValueChangedDelegate(PublishPhase));
			NetworkVariable<RoundPhase> currentPhase2 = CurrentPhase;
			currentPhase2.OnValueChanged = (NetworkVariable<RoundPhase>.OnValueChangedDelegate)Delegate.Combine(currentPhase2.OnValueChanged, new NetworkVariable<RoundPhase>.OnValueChangedDelegate(PublishPhase));
			PublishPhase(CurrentPhase.Value, CurrentPhase.Value);
		}

		protected void RaiseLocalEliminated()
		{
			this.OnLocalEliminated?.Invoke();
		}

		protected void RaiseRoundRestarted()
		{
			this.OnRoundRestarted?.Invoke();
		}

		public virtual void RequestStartRound()
		{
		}

		public virtual void RequestRestartRound()
		{
		}

		public virtual bool ServerAllowsBodyEdit(ulong clientId)
		{
			return false;
		}

		public virtual bool ShouldShowNameTag(ulong clientId)
		{
			return true;
		}

		public virtual void FillLobbyInfo(LobbySettingsData settings, List<LobbyInfoRow> rows)
		{
			FillSharedLobbyInfo(settings, rows);
		}

		public static void FillSharedLobbyInfo(LobbySettingsData settings, List<LobbyInfoRow> rows)
		{
			rows.Add(new LobbyInfoRow("Mod", GameModeCatalog.DisplayName(settings.ModeId.ToString())));
			rows.Add(new LobbyInfoRow("Harita", MapCatalog.DisplayName(settings.MapId.ToString())));
			rows.Add(new LobbyInfoRow(Loc.Get("Lobby.Row.MaxPlayers"), LobbyCapacity.Clamp(settings.MaxPlayers).ToString()));
		}

		public virtual void SetMap(Transform[] hunterSpawnPoints, Transform[] hiderSpawnPoints, Transform[] lobbySpawnPoints, GameObject hunterDoor)
		{
		}

		public virtual void SetArena(Transform[] arenaSpawnPoints)
		{
		}

		public virtual void SetHunterRoom(Transform[] hunterRoomSpawnPoints)
		{
		}

		public virtual bool AllowsOutsidePlayArea(ulong clientId)
		{
			return false;
		}

		public virtual bool ServerAllowsShooting(ulong clientId)
		{
			return false;
		}

		public virtual bool ServerIsShotTarget(ulong clientId)
		{
			return true;
		}

		public virtual bool ServerMayDamage(ulong shooterId, ulong victimId)
		{
			return false;
		}

		public virtual void ServerRecordDamage(ulong shooterId, ulong victimId, int amount)
		{
		}

		public virtual void ServerReportKill(ulong killerId, ulong victimId)
		{
		}

		public virtual bool ServerOnPlayerDied(ulong clientId)
		{
			return false;
		}

		public virtual bool TryGetHealthRegen(out float delaySeconds, out float perSecond)
		{
			delaySeconds = 0f;
			perSecond = 0f;
			return false;
		}

		public virtual string LocalDeathSubtitle(PlayerRole lastRole)
		{
			if (lastRole != PlayerRole.Hunter)
			{
				return "A Hunter found you.";
			}
			return "You've tried too many times.";
		}

		public virtual VoicePolicy VoicePolicyFor(ulong speaker, ulong listener, VoiceChannel channel)
		{
			return VoicePolicy.Silent;
		}

		public static int ResolveSelfDamagePercent(int configured)
		{
			if (configured < 25)
			{
				return 100;
			}
			return Mathf.Clamp(Mathf.RoundToInt((float)configured / 25f) * 25, 25, 200);
		}

		public static string SelfDamageLabel(int percent)
		{
			return Loc.Format("Lobby.Multiplier", ((float)percent / 100f).ToString("0.##", CultureInfo.InvariantCulture));
		}

		public int ScaleSelfDamage(int damage)
		{
			if (damage <= 0)
			{
				return 0;
			}
			return Mathf.Max(1, Mathf.RoundToInt((float)(damage * SelfDamagePercent) / 100f));
		}

		public static int ResolveHunterSharePercent(int configured)
		{
			if (configured < 5)
			{
				return 30;
			}
			return Mathf.Clamp(Mathf.RoundToInt((float)configured / 5f) * 5, 5, 95);
		}

		public static int HunterCountForShare(int playerCount, int percent)
		{
			if (playerCount <= 0)
			{
				return 0;
			}
			return Mathf.Clamp(Mathf.FloorToInt((float)(playerCount * percent) / 100f + 0.5f), 1, Mathf.Max(1, playerCount - 1));
		}

		public static string HunterShareLabel(int percent, int lobbySize)
		{
			int num = HunterCountForShare(lobbySize, percent);
			return Loc.Format("Lobby.HunterShare", percent, num, Mathf.Max(0, lobbySize - num));
		}

		protected void PlaceConnectedPlayersAtLobbySpawns()
		{
			if (!base.IsServer)
			{
				return;
			}
			foreach (NetworkClient connectedClients in base.NetworkManager.ConnectedClientsList)
			{
				if (!(connectedClients.PlayerObject == null))
				{
					PlayerMovement component = connectedClients.PlayerObject.GetComponent<PlayerMovement>();
					if (!(component == null) && TryGetLobbySpawn(out var position, out var rotation))
					{
						component.TeleportClientRpc(position, rotation);
					}
				}
			}
		}

		public virtual bool TryGetLobbySpawn(out Vector3 position, out Quaternion rotation)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			return false;
		}

		public virtual bool TryGetRescueSpawn(ulong clientId, out Vector3 position, out Quaternion rotation)
		{
			return TryGetLobbySpawn(out position, out rotation);
		}

		protected void SampleLatencies()
		{
			if (!base.IsSpawned || !base.IsServer || base.NetworkManager == null || base.NetworkManager.ServerTime.Time < nextLatencySampleServerTime)
			{
				return;
			}
			nextLatencySampleServerTime = base.NetworkManager.ServerTime.Time + 1.0;
			NetworkTransport networkTransport = ((base.NetworkManager.NetworkConfig != null) ? base.NetworkManager.NetworkConfig.NetworkTransport : null);
			if (networkTransport == null)
			{
				return;
			}
			bool flag = false;
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				bool flag2 = connectedClientsId == base.NetworkManager.LocalClientId;
				ushort value = 0;
				if (!flag2)
				{
					ulong currentRtt = networkTransport.GetCurrentRtt(connectedClientsId);
					if (currentRtt != 0)
					{
						value = (ushort)Mathf.Min(currentRtt, 65535f);
					}
					else
					{
						if (!appLevelRtt.TryGetValue(connectedClientsId, out value))
						{
							flag = true;
							continue;
						}
						flag = true;
					}
				}
				PlayerLatencyEntry playerLatencyEntry = new PlayerLatencyEntry
				{
					ClientId = connectedClientsId,
					PingMs = value,
					IsLocalHost = flag2
				};
				bool flag3 = false;
				for (int i = 0; i < Latencies.Count; i++)
				{
					if (Latencies[i].ClientId == connectedClientsId)
					{
						if (!Latencies[i].Equals(playerLatencyEntry))
						{
							Latencies[i] = playerLatencyEntry;
						}
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					Latencies.Add(playerLatencyEntry);
				}
			}
			for (int num = Latencies.Count - 1; num >= 0; num--)
			{
				if (!base.NetworkManager.ConnectedClients.ContainsKey(Latencies[num].ClientId))
				{
					Latencies.RemoveAt(num);
				}
			}
			departedLatencies.Clear();
			foreach (ulong key in appLevelRtt.Keys)
			{
				if (!base.NetworkManager.ConnectedClients.ContainsKey(key))
				{
					departedLatencies.Add(key);
				}
			}
			foreach (ulong departedLatency in departedLatencies)
			{
				appLevelRtt.Remove(departedLatency);
			}
			if (flag)
			{
				StartPingRoundTrip();
			}
		}

		private void StartPingRoundTrip()
		{
			pingToken++;
			pingSentTime = Time.realtimeSinceStartupAsDouble;
			PingClientRpc(pingToken);
		}

		[Rpc(SendTo.NotServer)]
		private void PingClientRpc(uint token)
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				RpcAttribute.RpcAttributeParams attributeParams = default(RpcAttribute.RpcAttributeParams);
				RpcParams rpcParams = default(RpcParams);
				FastBufferWriter bufferWriter = __beginSendRpc(2342366214u, rpcParams, attributeParams, SendTo.NotServer, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, token);
				__endSendRpc(ref bufferWriter, 2342366214u, rpcParams, attributeParams, SendTo.NotServer, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				PongServerRpc(token);
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void PongServerRpc(uint token, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(395819220u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, token);
				__endSendRpc(ref bufferWriter, 395819220u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (token == pingToken)
				{
					double num = (Time.realtimeSinceStartupAsDouble - pingSentTime) * 1000.0;
					appLevelRtt[rpcParams.Receive.SenderClientId] = (ushort)Mathf.Clamp((int)num, 0, 65535);
				}
			}
		}

		public virtual bool TryGetPing(ulong clientId, out int pingMs)
		{
			pingMs = 0;
			foreach (PlayerLatencyEntry latency in Latencies)
			{
				if (latency.ClientId == clientId)
				{
					pingMs = latency.PingMs;
					return !latency.IsLocalHost;
				}
			}
			return false;
		}

		public virtual ulong GetSteamId(ulong clientId)
		{
			return 0uL;
		}

		public virtual LobbySettingsData WithDefaults(LobbySettingsData settings)
		{
			settings.MaxPlayers = LobbyCapacity.Clamp(settings.MaxPlayers);
			settings.SelfDamagePercent = ResolveSelfDamagePercent(settings.SelfDamagePercent);
			settings.HunterSharePercent = ResolveHunterSharePercent(settings.HunterSharePercent);
			return settings;
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void StartVoteKickServerRpc(ulong targetClientId, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(1800394406u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, targetClientId);
				__endSendRpc(ref bufferWriter, 1800394406u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ulong senderClientId = rpcParams.Receive.SenderClientId;
				if (!VoteKick.Value.Active && VoteKickAllowedHere && !(base.NetworkManager.ServerTime.Time < voteAllowedAfterServerTime) && targetClientId != senderClientId && base.NetworkManager.ConnectedClients.ContainsKey(targetClientId) && targetClientId != 0L && base.NetworkManager.ConnectedClientsIds.Count > 2)
				{
					voteAnswers.Clear();
					voteAnswers[senderClientId] = true;
					VoteKick.Value = new VoteKickState
					{
						Active = true,
						TargetClientId = targetClientId,
						StartedByClientId = senderClientId,
						EndsAtServerTime = base.NetworkManager.ServerTime.Time + 60.0,
						Yes = 1,
						No = 0,
						Eligible = EligibleVoters(targetClientId)
					};
					SystemChat("Chat.VoteKickStarted", GetPlayerName(senderClientId), GetPlayerName(targetClientId));
				}
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void CastVoteKickServerRpc(bool yes, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(1397992363u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in yes, default(FastBufferWriter.ForPrimitives));
				__endSendRpc(ref bufferWriter, 1397992363u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				ulong senderClientId = rpcParams.Receive.SenderClientId;
				if (VoteKick.Value.Active && senderClientId != VoteKick.Value.TargetClientId && !voteAnswers.ContainsKey(senderClientId))
				{
					voteAnswers[senderClientId] = yes;
					RecountVote();
				}
			}
		}

		private void RecountVote()
		{
			int num = 0;
			int num2 = 0;
			foreach (bool value2 in voteAnswers.Values)
			{
				if (value2)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
			VoteKickState value = VoteKick.Value;
			value.Yes = num;
			value.No = num2;
			value.Eligible = EligibleVoters(value.TargetClientId);
			VoteKick.Value = value;
			if (num > value.Eligible / 2 || num2 >= value.Eligible - value.Eligible / 2 || num + num2 >= value.Eligible)
			{
				FinishVote();
			}
		}

		private int EligibleVoters(ulong targetClientId)
		{
			int num = 0;
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				if (connectedClientsId != targetClientId)
				{
					num++;
				}
			}
			return num;
		}

		private void TickVoteKick()
		{
			if (VoteKick.Value.Active)
			{
				if (!base.NetworkManager.ConnectedClients.ContainsKey(VoteKick.Value.TargetClientId))
				{
					CloseVote();
				}
				else if (base.NetworkManager.ServerTime.Time >= VoteKick.Value.EndsAtServerTime)
				{
					FinishVote();
				}
			}
		}

		private void FinishVote()
		{
			VoteKickState value = VoteKick.Value;
			bool num = value.Yes > value.Eligible / 2;
			ulong targetClientId = value.TargetClientId;
			string playerName = GetPlayerName(targetClientId);
			CloseVote();
			if (!num)
			{
				SystemChat("Chat.VoteKickFailed", playerName);
				return;
			}
			bool num2 = LobbyBans.Ban(targetClientId);
			SystemChat("Chat.VoteKickPassed", playerName);
			if (base.NetworkManager.ConnectedClients.ContainsKey(targetClientId))
			{
				base.NetworkManager.DisconnectClient(targetClientId, "Lobby.Banned");
			}
			if (!num2)
			{
				Debug.LogWarning($"[VoteKick] {targetClientId} numarali istemcinin Steam hesabi bulunamadi - " + "atildi ama tekrar girebilir.");
			}
		}

		private void CloseVote()
		{
			voteAnswers.Clear();
			VoteKick.Value = default(VoteKickState);
			voteAllowedAfterServerTime = base.NetworkManager.ServerTime.Time + 60.0;
		}

		private void ForgetVoter(ulong clientId)
		{
			if (base.IsServer)
			{
				voteAnswers.Remove(clientId);
				if (VoteKick.Value.Active)
				{
					RecountVote();
				}
			}
		}

		protected PlayerChatBubbles BubblesOf(ulong clientId)
		{
			if (!base.IsServer || base.NetworkManager == null || !base.NetworkManager.ConnectedClients.TryGetValue(clientId, out var value) || value.PlayerObject == null)
			{
				return null;
			}
			return value.PlayerObject.GetComponent<PlayerChatBubbles>();
		}

		public virtual bool TryGetScoreHighlight(out string text)
		{
			text = "";
			return false;
		}

		public void AddScore(ulong clientId, int amount)
		{
			if (!base.IsServer)
			{
				return;
			}
			for (int i = 0; i < Scores.Count; i++)
			{
				if (Scores[i].ClientId == clientId)
				{
					Scores[i] = new PlayerScoreEntry
					{
						ClientId = clientId,
						Score = Scores[i].Score + amount
					};
					return;
				}
			}
			Scores.Add(new PlayerScoreEntry
			{
				ClientId = clientId,
				Score = amount
			});
		}

		protected void RemoveName(ulong clientId)
		{
			for (int i = 0; i < Names.Count; i++)
			{
				if (Names[i].ClientId == clientId)
				{
					Names.RemoveAt(i);
					break;
				}
			}
		}

		protected void RemoveScore(ulong clientId)
		{
			for (int i = 0; i < Scores.Count; i++)
			{
				if (Scores[i].ClientId == clientId)
				{
					Scores.RemoveAt(i);
					break;
				}
			}
		}

		public string GetPlayerName(ulong clientId)
		{
			foreach (PlayerNameEntry name in Names)
			{
				if (name.ClientId == clientId)
				{
					FixedString64Bytes fixedString64Bytes = name.Name;
					return fixedString64Bytes.ToString();
				}
			}
			return $"Oyuncu {clientId}";
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void SubmitPlayerNameServerRpc(FixedString64Bytes name, ulong steamId, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(3529714440u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in name, default(FastBufferWriter.ForFixedStrings));
				BytePacker.WriteValueBitPacked(bufferWriter, steamId);
				__endSendRpc(ref bufferWriter, 3529714440u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			ulong senderClientId = rpcParams.Receive.SenderClientId;
			PlayerNameEntry playerNameEntry = new PlayerNameEntry
			{
				ClientId = senderClientId,
				Name = new FixedString64Bytes(MakeUniqueName(PlayerNameStore.Sanitize(name.ToString()), senderClientId)),
				SteamId = steamId
			};
			for (int i = 0; i < Names.Count; i++)
			{
				if (Names[i].ClientId == senderClientId)
				{
					Names[i] = playerNameEntry;
					return;
				}
			}
			Names.Add(playerNameEntry);
			SystemChat("Chat.PlayerConnected", playerNameEntry.Name.ToString());
		}

		private string MakeUniqueName(string desired, ulong senderId)
		{
			if (!IsNameTaken(desired, senderId))
			{
				return desired;
			}
			for (int i = 1; i <= 99; i++)
			{
				string text = WithSuffix(desired, i);
				if (!IsNameTaken(text, senderId))
				{
					return text;
				}
			}
			return desired;
		}

		private bool IsNameTaken(string candidate, ulong exceptClientId)
		{
			for (int i = 0; i < Names.Count; i++)
			{
				if (Names[i].ClientId != exceptClientId && Names[i].Name.ToString() == candidate)
				{
					return true;
				}
			}
			return false;
		}

		private static string WithSuffix(string baseName, int suffix)
		{
			string text = $" ({suffix})";
			int num = Mathf.Max(1, 16 - text.Length);
			return PlayerNameStore.Sanitize(((baseName.Length > num) ? baseName.Substring(0, num) : baseName) + text);
		}

		[ClientRpc]
		protected void SystemChatClientRpc(string message)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(3057608402u, clientRpcParams, RpcDelivery.Reliable);
				bool value = message != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(message);
				}
				__endSendClientRpc(ref bufferWriter, 3057608402u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (ChatView.Instance != null)
				{
					ChatView.Instance.AddSystemMessage(message);
				}
			}
		}

		protected void SystemChat(string key, string first = "", string second = "")
		{
			SystemChatKeyClientRpc(key, first, second);
		}

		[ClientRpc]
		private void SystemChatKeyClientRpc(string key, string first, string second)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(2118260047u, clientRpcParams, RpcDelivery.Reliable);
				bool value = key != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(key);
				}
				bool value2 = first != null;
				bufferWriter.WriteValueSafe(in value2, default(FastBufferWriter.ForPrimitives));
				if (value2)
				{
					bufferWriter.WriteValueSafe(first);
				}
				bool value3 = second != null;
				bufferWriter.WriteValueSafe(in value3, default(FastBufferWriter.ForPrimitives));
				if (value3)
				{
					bufferWriter.WriteValueSafe(second);
				}
				__endSendClientRpc(ref bufferWriter, 2118260047u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (!(ChatView.Instance == null))
				{
					string message = (string.IsNullOrEmpty(first) ? Loc.Get(key) : Loc.Format(key, first, second));
					ChatView.Instance.AddSystemMessage(message);
				}
			}
		}

		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();
			if (base.IsServer && base.NetworkManager != null)
			{
				base.NetworkManager.NetworkTickSystem.Tick += TickVoteKick;
				base.NetworkManager.OnClientDisconnectCallback += ForgetVoter;
			}
			if (base.IsClient)
			{
				SubmitPlayerNameServerRpc(new FixedString64Bytes(PlayerNameStore.Get()), SteamManager.IsInitialized ? SteamManager.LocalSteamId.Value : 0);
			}
			if (Current != null && Current != this)
			{
				Debug.LogError("[GameModeController] Ikinci bir mod denetleyicisi spawn oldu (" + Current.name + " zaten vardi) - onceki mod sahnesi bosaltilmamis olmali.", this);
			}
			Current = this;
			if (base.IsClient)
			{
				LobbySettingsData lobbySettingsData = ((LobbySettingsSync.Instance != null) ? LobbySettingsSync.Instance.CurrentSettings : default(LobbySettingsData));
				Telemetry.Send("lobby_joined", ("game_mode", lobbySettingsData.ModeId.ToString()), ("map", lobbySettingsData.MapId.ToString()), ("transport", SteamManager.IsInitialized ? "steam" : "utp"), ("is_host", base.IsHost), ("players", base.NetworkManager.ConnectedClientsIds.Count), ("lobbies_this_session", Telemetry.Bump("lobbies")), ("session_seconds", Telemetry.SessionSeconds));
			}
			BeginPublishingPhase();
			ApplyModeObjectsEverywhere();
			SceneManager.sceneLoaded += OnSceneLoadedForModeObjects;
		}

		public override void OnNetworkDespawn()
		{
			if (base.NetworkManager != null)
			{
				base.NetworkManager.NetworkTickSystem.Tick -= TickVoteKick;
				base.NetworkManager.OnClientDisconnectCallback -= ForgetVoter;
			}
			SceneManager.sceneLoaded -= OnSceneLoadedForModeObjects;
			NetworkVariable<RoundPhase> currentPhase = CurrentPhase;
			currentPhase.OnValueChanged = (NetworkVariable<RoundPhase>.OnValueChangedDelegate)Delegate.Remove(currentPhase.OnValueChanged, new NetworkVariable<RoundPhase>.OnValueChangedDelegate(PublishPhase));
			if (Current == this)
			{
				Current = null;
			}
			base.OnNetworkDespawn();
		}

		private void OnSceneLoadedForModeObjects(Scene scene, LoadSceneMode _)
		{
			MapModeObject.ApplyAll(scene, MarkerMode);
		}

		private void ApplyModeObjectsEverywhere()
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				MapModeObject.ApplyAll(SceneManager.GetSceneAt(i), MarkerMode);
			}
		}

		protected override void __initializeVariables()
		{
			if (CurrentPhase == null)
			{
				throw new Exception("GameModeController.CurrentPhase cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			CurrentPhase.Initialize(this);
			__nameNetworkVariable(CurrentPhase, "CurrentPhase");
			NetworkVariableFields.Add(CurrentPhase);
			if (PhaseEndServerTime == null)
			{
				throw new Exception("GameModeController.PhaseEndServerTime cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			PhaseEndServerTime.Initialize(this);
			__nameNetworkVariable(PhaseEndServerTime, "PhaseEndServerTime");
			NetworkVariableFields.Add(PhaseEndServerTime);
			if (Scores == null)
			{
				throw new Exception("GameModeController.Scores cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			Scores.Initialize(this);
			__nameNetworkVariable(Scores, "Scores");
			NetworkVariableFields.Add(Scores);
			if (Names == null)
			{
				throw new Exception("GameModeController.Names cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			Names.Initialize(this);
			__nameNetworkVariable(Names, "Names");
			NetworkVariableFields.Add(Names);
			if (Reveals == null)
			{
				throw new Exception("GameModeController.Reveals cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			Reveals.Initialize(this);
			__nameNetworkVariable(Reveals, "Reveals");
			NetworkVariableFields.Add(Reveals);
			if (Latencies == null)
			{
				throw new Exception("GameModeController.Latencies cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			Latencies.Initialize(this);
			__nameNetworkVariable(Latencies, "Latencies");
			NetworkVariableFields.Add(Latencies);
			if (VoteKick == null)
			{
				throw new Exception("GameModeController.VoteKick cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			VoteKick.Initialize(this);
			__nameNetworkVariable(VoteKick, "VoteKick");
			NetworkVariableFields.Add(VoteKick);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(2342366214u, __rpc_handler_2342366214, "PingClientRpc", RpcInvokePermission.Everyone);
			__registerRpc(395819220u, __rpc_handler_395819220, "PongServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(1800394406u, __rpc_handler_1800394406, "StartVoteKickServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(1397992363u, __rpc_handler_1397992363, "CastVoteKickServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(3529714440u, __rpc_handler_3529714440, "SubmitPlayerNameServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(3057608402u, __rpc_handler_3057608402, "SystemChatClientRpc", RpcInvokePermission.Server);
			__registerRpc(2118260047u, __rpc_handler_2118260047, "SystemChatKeyClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_2342366214(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out uint value);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GameModeController)target).PingClientRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_395819220(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out uint value);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GameModeController)target).PongServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1800394406(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GameModeController)target).StartVoteKickServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1397992363(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out bool value, default(FastBufferWriter.ForPrimitives));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GameModeController)target).CastVoteKickServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3529714440(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out FixedString64Bytes value, default(FastBufferWriter.ForFixedStrings));
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value2);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GameModeController)target).SubmitPlayerNameServerRpc(value, value2, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3057608402(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
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
				((GameModeController)target).SystemChatClientRpc(s);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2118260047(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
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
				reader.ReadValueSafe(out bool value2, default(FastBufferWriter.ForPrimitives));
				string s2 = null;
				if (value2)
				{
					reader.ReadValueSafe(out s2, false);
				}
				reader.ReadValueSafe(out bool value3, default(FastBufferWriter.ForPrimitives));
				string s3 = null;
				if (value3)
				{
					reader.ReadValueSafe(out s3, false);
				}
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GameModeController)target).SystemChatKeyClientRpc(s, s2, s3);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "GameModeController";
		}
	}
}
