using System;
using System.Collections.Generic;
using System.Text;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.UI;
using Mimicraft.Voice;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class GarticRoundManager : GameModeController
	{
		private const int MinPlayers = 1;

		private const int WordChoiceCount = 3;

		private const int DefaultScoreLimit = 60;

		private const int DefaultTurnSeconds = 180;

		private const int GameOverSeconds = 15;

		public const int WordSelectSeconds = 15;

		public const int TurnEndSeconds = 6;

		public const int MinimumTurnSeconds = 60;

		private const int GuessScoreMax = 20;

		private const int GuessScoreMin = 5;

		private const int ModellerScorePerGuess = 8;

		private const int PlacementPenalty = 3;

		private const float MaxRevealedFraction = 0.5f;

		public const float RevealCooldownSeconds = 8f;

		private const float AutoRevealStartFraction = 0.6f;

		public readonly NetworkVariable<GarticPhase> Phase = new NetworkVariable<GarticPhase>(GarticPhase.WaitingForPlayers);

		public readonly NetworkVariable<ulong> CurrentModeller = new NetworkVariable<ulong>(0uL);

		public readonly NetworkVariable<FixedString64Bytes> MaskedWord = new NetworkVariable<FixedString64Bytes>();

		public readonly NetworkVariable<FixedString32Bytes> CategoryId = new NetworkVariable<FixedString32Bytes>();

		public readonly NetworkVariable<int> ScoreLimit = new NetworkVariable<int>(60);

		public readonly NetworkVariable<int> TurnSeconds = new NetworkVariable<int>(180);

		public readonly NetworkVariable<FixedString32Bytes> WordLanguage = new NetworkVariable<FixedString32Bytes>();

		public readonly NetworkVariable<bool> AutoHints = new NetworkVariable<bool>(value: false);

		public readonly NetworkVariable<int> MaxHints = new NetworkVariable<int>(0);

		public readonly NetworkVariable<int> HintsGiven = new NetworkVariable<int>(0);

		public readonly NetworkVariable<double> NextHintServerTime = new NetworkVariable<double>(0.0);

		private string currentWord = "";

		private readonly List<int> revealedLetters = new List<int>();

		private readonly List<int> revealOrder = new List<int>();

		private string[] pendingChoices = Array.Empty<string>();

		private readonly List<ulong> turnOrder = new List<ulong>();

		private readonly HashSet<ulong> guessedThisTurn = new HashSet<ulong>();

		private int turnIndex = -1;

		private bool subscribedToChat;

		private readonly List<Transform> spawnPoints = new List<Transform>();

		private Transform modelerSpawn;

		private int spawnIndex;

		private readonly HashSet<string> offeredWords = new HashSet<string>();

		private readonly HashSet<string> playedWords = new HashSet<string>();

		private bool HasGuessers => turnOrder.Count > 1;

		public bool LocalPlayerOwnsHints
		{
			get
			{
				if (Phase.Value == GarticPhase.Drawing && LocalIsModeller)
				{
					return MaxHints.Value > 0;
				}
				return false;
			}
		}

		public bool CanLocalPlayerGiveHint
		{
			get
			{
				if (Phase.Value == GarticPhase.Drawing && LocalIsModeller && HintsGiven.Value < MaxHints.Value)
				{
					return base.NetworkManager.ServerTime.Time >= NextHintServerTime.Value;
				}
				return false;
			}
		}

		public float HintCooldownRemaining => Mathf.Max(0f, (float)(NextHintServerTime.Value - base.NetworkManager.ServerTime.Time));

		private bool LocalIsModeller
		{
			get
			{
				if (base.NetworkManager != null)
				{
					return CurrentModeller.Value == base.NetworkManager.LocalClientId;
				}
				return false;
			}
		}

		public override MapMarkerModes MarkerMode => MapMarkerModes.Gartic;

		public override bool CanLocalPlayerEditModel
		{
			get
			{
				if (LocalIsModeller)
				{
					return Phase.Value == GarticPhase.Drawing;
				}
				return false;
			}
		}

		public override bool IsModelSetupWindow => Phase.Value == GarticPhase.WordSelect;

		public override MusicCue Music => Phase.Value switch
		{
			GarticPhase.WaitingForPlayers => MusicCue.Lobby, 
			GarticPhase.GameOver => MusicCue.RoundEnd, 
			GarticPhase.TurnEnd => MusicCue.RoundEnd, 
			_ => MusicCue.Prep, 
		};

		public override bool LocalPlayerShouldHaveModel => LocalIsModeller;

		public override int MinBodyExtent => 1;

		public override int MinBodyIslandVoxels => 1;

		public override bool EnforcesBodyClearance => false;

		public override bool LocalPlayerUsesHunterMovement => false;

		public override bool IsLocalPlayerArmed => false;

		public override bool ModeUsesWeapons => false;

		public override bool LocalPlayerMayWallClimb => false;

		public override bool IsLocalPlayerFrozen
		{
			get
			{
				if (LocalIsModeller)
				{
					return Phase.Value == GarticPhase.WordSelect;
				}
				return false;
			}
		}

		public override bool IsLocalPlayerParticipating => true;

		public override bool LocalPlayerMayLeaveEditMode => false;

		public override ulong HighlightedClientId
		{
			get
			{
				if (Phase.Value != GarticPhase.TurnEnd && Phase.Value != GarticPhase.GameOver)
				{
					return ulong.MaxValue;
				}
				ulong result = ulong.MaxValue;
				int num = int.MinValue;
				foreach (PlayerScoreEntry score in Scores)
				{
					if (score.Score > num)
					{
						num = score.Score;
						result = score.ClientId;
					}
				}
				return result;
			}
		}

		public override bool LocalPlayerNeedsCursor
		{
			get
			{
				if (!LocalIsModeller || Phase.Value != GarticPhase.WordSelect)
				{
					if (base.IsServer)
					{
						if (Phase.Value != GarticPhase.WaitingForPlayers)
						{
							return Phase.Value == GarticPhase.GameOver;
						}
						return true;
					}
					return false;
				}
				return true;
			}
		}

		public bool CanStart
		{
			get
			{
				if (Phase.Value == GarticPhase.WaitingForPlayers)
				{
					return turnOrder.Count >= 1;
				}
				return false;
			}
		}

		public override bool SupportsHostRoundControls => true;

		public override bool ServerAllowsBodyEdit(ulong clientId)
		{
			if (clientId == CurrentModeller.Value)
			{
				if (Phase.Value != GarticPhase.Drawing)
				{
					return Phase.Value == GarticPhase.WordSelect;
				}
				return true;
			}
			return false;
		}

		public override bool ShouldShowNameTag(ulong clientId)
		{
			return clientId != CurrentModeller.Value;
		}

		public override VoicePolicy VoicePolicyFor(ulong speaker, ulong listener, VoiceChannel channel)
		{
			if (!base.IsServer)
			{
				return VoicePolicy.Silent;
			}
			if ((Phase.Value == GarticPhase.WordSelect || Phase.Value == GarticPhase.Drawing) && speaker == CurrentModeller.Value)
			{
				return VoicePolicy.Silent;
			}
			return VoicePolicy.Lobby;
		}

		public override void FillLobbyInfo(LobbySettingsData settings, List<LobbyInfoRow> rows)
		{
			base.FillLobbyInfo(settings, rows);
			rows.Add(new LobbyInfoRow(Loc.Get("Category"), WordCategoryCatalog.DisplayName(CategoryId.Value.ToString())));
			rows.Add(new LobbyInfoRow(Loc.Get("TurnLength"), Loc.Format("Common.Seconds", TurnSeconds.Value)));
			rows.Add(new LobbyInfoRow(Loc.Get("ScoreLimit"), ScoreLimit.Value.ToString()));
			rows.Add(new LobbyInfoRow(Loc.Get("WordLanguage"), Loc.LanguageName(WordLanguage.Value.ToString())));
			rows.Add(new LobbyInfoRow(Loc.Get("AutomaticHints"), Loc.Get(AutoHints.Value ? "Common.On" : "Common.Off")));
		}

		public override bool TryGetScoreHighlight(out string text)
		{
			text = "";
			if (Phase.Value != GarticPhase.TurnEnd && Phase.Value != GarticPhase.GameOver)
			{
				return false;
			}
			List<PlayerScoreEntry> list = new List<PlayerScoreEntry>();
			foreach (PlayerScoreEntry score in Scores)
			{
				list.Add(score);
			}
			if (list.Count == 0)
			{
				return false;
			}
			list.Sort((PlayerScoreEntry a, PlayerScoreEntry b) => b.Score.CompareTo(a.Score));
			string text2 = Loc.Get((Phase.Value == GarticPhase.GameOver) ? "GameOver" : "Standings");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<font=\"ari-w9500-bold SDF\">" + text2 + "</font>");
			string[] array = new string[3] { "<color=#FFD54A>1.</color>", "<color=#CFCFCF>2.</color>", "<color=#C98A4B>3.</color>" };
			int num = Mathf.Min(3, list.Count);
			for (int num2 = 0; num2 < num; num2++)
			{
				stringBuilder.Append($"\n{array[num2]} {GetPlayerName(list[num2].ClientId)}  ({list[num2].Score})");
			}
			text = stringBuilder.ToString();
			return true;
		}

		private void CollectSpawnPoints()
		{
			spawnPoints.Clear();
			List<Transform> list = new List<Transform>();
			List<Transform> list2 = new List<Transform>();
			modelerSpawn = null;
			MapMarker[] array = UnityEngine.Object.FindObjectsByType<MapMarker>(FindObjectsSortMode.None);
			foreach (MapMarker mapMarker in array)
			{
				if (mapMarker.AppliesTo(MarkerMode))
				{
					if (mapMarker.Is(MapMarkerKind.ModelerSpawn))
					{
						modelerSpawn = mapMarker.transform;
					}
					if (mapMarker.Is(MapMarkerKind.LobbySpawn))
					{
						list.Add(mapMarker.transform);
					}
					if (mapMarker.Is(MapMarkerKind.HiderSpawn))
					{
						list2.Add(mapMarker.transform);
					}
				}
			}
			spawnPoints.AddRange((list.Count > 0) ? list : list2);
			if (spawnPoints.Count == 0)
			{
				Debug.LogWarning("[GarticRoundManager] Sahnede spawn noktası yok - oyuncular X ekseninde sıralanacak. Odaya MapMarker (Lobby Spawn) ekle.", this);
			}
		}

		public override bool TryGetLobbySpawn(out Vector3 position, out Quaternion rotation)
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
			if (spawnPoints.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < spawnPoints.Count; i++)
			{
				Transform transform = spawnPoints[spawnIndex++ % spawnPoints.Count];
				if (!(transform == null))
				{
					position = transform.position;
					rotation = transform.rotation;
					return true;
				}
			}
			return false;
		}

		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();
			CollectSpawnPoints();
			if (base.IsServer)
			{
				base.NetworkManager.OnClientConnectedCallback += OnClientConnected;
				base.NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;
				RebuildTurnOrder();
				EnterPhase(GarticPhase.WaitingForPlayers);
			}
			TrySubscribeToChat();
		}

		public override void OnNetworkDespawn()
		{
			base.OnNetworkDespawn();
			offeredWords.Clear();
			playedWords.Clear();
			if (base.IsServer && base.NetworkManager != null)
			{
				base.NetworkManager.OnClientConnectedCallback -= OnClientConnected;
				base.NetworkManager.OnClientDisconnectCallback -= OnClientDisconnected;
			}
			if (subscribedToChat && ChatView.Instance != null)
			{
				ChatView.Instance.MessageSubmitted -= OnChatSubmitted;
			}
			subscribedToChat = false;
		}

		private void TrySubscribeToChat()
		{
			if (!subscribedToChat && base.IsClient && !(ChatView.Instance == null))
			{
				ChatView.Instance.MessageSubmitted += OnChatSubmitted;
				subscribedToChat = true;
			}
		}

		private void Update()
		{
			TrySubscribeToChat();
			if (base.IsServer)
			{
				SampleLatencies();
			}
			if (!base.IsServer || Phase.Value == GarticPhase.WaitingForPlayers)
			{
				return;
			}
			TickAutoHints();
			if (!(base.NetworkManager.ServerTime.Time < PhaseEndServerTime.Value))
			{
				switch (Phase.Value)
				{
				case GarticPhase.WordSelect:
					ChooseWord((pendingChoices.Length != 0) ? pendingChoices[UnityEngine.Random.Range(0, pendingChoices.Length)] : "");
					break;
				case GarticPhase.Drawing:
					EndTurn("TimeUp");
					break;
				case GarticPhase.TurnEnd:
					StartNextTurn();
					break;
				case GarticPhase.GameOver:
					EnterPhase(GarticPhase.WaitingForPlayers);
					break;
				}
			}
		}

		private void OnClientConnected(ulong clientId)
		{
			RebuildTurnOrder();
		}

		public override void RequestStartRound()
		{
			StartGameServerRpc();
		}

		public override void RequestRestartRound()
		{
			RestartGameServerRpc();
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void RestartGameServerRpc(RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(3166935561u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 3166935561u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (rpcParams.Receive.SenderClientId == 0L)
				{
					SystemChat("Chat.GameRestarted", GetPlayerName(0uL));
					AbandonTurn();
					EnterPhase(GarticPhase.WaitingForPlayers);
				}
			}
		}

		private void AbandonTurn()
		{
			currentWord = "";
			pendingChoices = Array.Empty<string>();
			guessedThisTurn.Clear();
			revealedLetters.Clear();
			revealOrder.Clear();
			MaskedWord.Value = default(FixedString64Bytes);
			HintsGiven.Value = 0;
			NextHintServerTime.Value = 0.0;
			YourWordClientRpc("");
			HideWordChoicesClientRpc();
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void StartGameServerRpc(RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(1189638458u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 1189638458u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (rpcParams.Receive.SenderClientId != 0L)
			{
				return;
			}
			RebuildTurnOrder();
			if (CanStart)
			{
				if (WordLanguage.Value.IsEmpty)
				{
					WordLanguage.Value = new FixedString32Bytes(WordCategoryCatalog.DefaultLanguage());
				}
				Scores.Clear();
				turnIndex = -1;
				SystemChat("GameStarting", WordCategoryCatalog.DisplayName(CategoryId.Value.ToString()), ScoreLimit.Value.ToString());
				StartNextTurn();
			}
		}

		private void OnClientDisconnected(ulong clientId)
		{
			bool flag = clientId == CurrentModeller.Value;
			RemoveName(clientId);
			RemoveScore(clientId);
			RebuildTurnOrder();
			if (turnOrder.Count < 1)
			{
				EnterPhase(GarticPhase.WaitingForPlayers);
			}
			else if (flag && (Phase.Value == GarticPhase.WordSelect || Phase.Value == GarticPhase.Drawing))
			{
				EndTurn("PlayerLeft", GetPlayerName(clientId));
			}
		}

		private void RebuildTurnOrder()
		{
			turnOrder.Clear();
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				turnOrder.Add(connectedClientsId);
			}
		}

		private void StartNextTurn()
		{
			RebuildTurnOrder();
			if (turnOrder.Count < 1)
			{
				EnterPhase(GarticPhase.WaitingForPlayers);
				return;
			}
			turnIndex = (turnIndex + 1) % turnOrder.Count;
			CurrentModeller.Value = turnOrder[turnIndex];
			PlaceEveryoneAtSpawns();
			guessedThisTurn.Clear();
			currentWord = "";
			revealedLetters.Clear();
			revealOrder.Clear();
			MaskedWord.Value = default(FixedString64Bytes);
			string language = WordLanguage.Value.ToString();
			WordCategory wordCategory = WordCategoryCatalog.Find(CategoryId.Value.ToString());
			if (wordCategory != null && !wordCategory.Supports(language))
			{
				wordCategory = null;
			}
			if (wordCategory == null)
			{
				List<WordCategory> list = WordCategoryCatalog.For(language);
				wordCategory = ((list.Count > 0) ? list[0] : WordCategoryCatalog.Default);
			}
			if (wordCategory == null)
			{
				Debug.LogError("[GarticRoundManager] Kullanılabilir kelime kategorisi yok - tur başlatılamadı.", this);
				EnterPhase(GarticPhase.WaitingForPlayers);
				return;
			}
			CategoryId.Value = new FixedString32Bytes(wordCategory.CategoryId);
			pendingChoices = PickChoices(wordCategory, WordLanguage.Value.ToString());
			EnterPhase(GarticPhase.WordSelect);
			SystemChat("TurnOf", GetPlayerName(CurrentModeller.Value));
			WordChoicesClientRpc(ChoiceAt(0), ChoiceAt(1), ChoiceAt(2), TargetClient(CurrentModeller.Value));
		}

		private string ChoiceAt(int index)
		{
			if (index >= pendingChoices.Length)
			{
				return "";
			}
			return pendingChoices[index];
		}

		private void PlaceEveryoneAtSpawns()
		{
			if (spawnPoints.Count == 0)
			{
				return;
			}
			foreach (ulong connectedClientsId in base.NetworkManager.ConnectedClientsIds)
			{
				if (base.NetworkManager.ConnectedClients.TryGetValue(connectedClientsId, out var value) && !(value.PlayerObject == null))
				{
					Vector3 position;
					Quaternion rotation;
					if (connectedClientsId == CurrentModeller.Value && modelerSpawn != null)
					{
						position = modelerSpawn.position;
						rotation = modelerSpawn.rotation;
					}
					else if (!TryGetLobbySpawn(out position, out rotation))
					{
						continue;
					}
					PlayerMovement component = value.PlayerObject.GetComponent<PlayerMovement>();
					if (component != null)
					{
						component.TeleportClientRpc(position, rotation);
					}
				}
			}
		}

		private string[] PickChoices(WordCategory category, string language)
		{
			string[] array = category.WordsFor(language);
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				if (playedWords.Contains(array[i]))
				{
					list3.Add(i);
				}
				else if (offeredWords.Contains(array[i]))
				{
					list2.Add(i);
				}
				else
				{
					list.Add(i);
				}
			}
			int num = Mathf.Min(3, array.Length);
			List<string> list4 = new List<string>(num);
			List<int>[] array2 = new List<int>[3] { list, list2, list3 };
			foreach (List<int> list5 in array2)
			{
				while (list4.Count < num && list5.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, list5.Count);
					list4.Add(array[list5[index]]);
					list5.RemoveAt(index);
				}
			}
			foreach (string item in list4)
			{
				offeredWords.Add(item);
			}
			return list4.ToArray();
		}

		private void PlanReveals(string word)
		{
			revealedLetters.Clear();
			revealOrder.Clear();
			List<int> list = GuessMatcher.LetterIndices(word);
			int num = ((list.Count >= 3) ? Mathf.FloorToInt((float)list.Count * 0.5f) : 0);
			MaxHints.Value = Mathf.Max(0, num);
			HintsGiven.Value = 0;
			NextHintServerTime.Value = 0.0;
			if (num > 0)
			{
				for (int num2 = list.Count - 1; num2 > 0; num2--)
				{
					int num3 = UnityEngine.Random.Range(0, num2 + 1);
					int index = num2;
					List<int> list2 = list;
					int index2 = num3;
					int num4 = list[num3];
					int num5 = list[num2];
					int num6 = (list[index] = num4);
					num6 = (list2[index2] = num5);
				}
				for (int i = 0; i < num; i++)
				{
					revealOrder.Add(list[i]);
				}
			}
		}

		private bool RevealNextLetter()
		{
			if (revealedLetters.Count >= revealOrder.Count)
			{
				return false;
			}
			revealedLetters.Add(revealOrder[revealedLetters.Count]);
			HintsGiven.Value = revealedLetters.Count;
			NextHintServerTime.Value = base.NetworkManager.ServerTime.Time + 8.0;
			MaskedWord.Value = new FixedString64Bytes(GuessMatcher.Mask(currentWord, revealedLetters));
			return true;
		}

		private void TickAutoHints()
		{
			if (AutoHints.Value && Phase.Value == GarticPhase.Drawing && HintsGiven.Value < MaxHints.Value && !(base.NetworkManager.ServerTime.Time < NextHintServerTime.Value))
			{
				double num = Mathf.Max(60, TurnSeconds.Value);
				if (!(num - (PhaseEndServerTime.Value - base.NetworkManager.ServerTime.Time) < num * 0.6000000238418579))
				{
					RevealNextLetter();
				}
			}
		}

		private void ChooseWord(string word)
		{
			if (string.IsNullOrWhiteSpace(word))
			{
				EndTurn("NoWordChosen");
				return;
			}
			currentWord = word;
			playedWords.Add(word);
			HideWordChoicesClientRpc();
			PlanReveals(word);
			MaskedWord.Value = new FixedString64Bytes(GuessMatcher.Mask(word, revealedLetters));
			EnterPhase(GarticPhase.Drawing);
			YourWordClientRpc(word, TargetClient(CurrentModeller.Value));
		}

		private void EndTurn(string reasonKey, string reasonArg = "")
		{
			TurnEndedClientRpc(reasonKey, reasonArg, currentWord ?? "");
			if (guessedThisTurn.Count == 0 && HasGuessers && !string.IsNullOrEmpty(currentWord))
			{
				TurnFailedClientRpc();
			}
			currentWord = "";
			pendingChoices = Array.Empty<string>();
			YourWordClientRpc("");
			if (TryFindWinner(out var winnerId))
			{
				EnterPhase(GarticPhase.GameOver);
				SystemChat("GameOverWinner", GetPlayerName(winnerId));
				GameOverClientRpc(winnerId);
			}
			else
			{
				EnterPhase(GarticPhase.TurnEnd);
			}
		}

		private bool TryFindWinner(out ulong winnerId)
		{
			winnerId = 0uL;
			int num = int.MinValue;
			foreach (PlayerScoreEntry score in Scores)
			{
				if (score.Score > num)
				{
					num = score.Score;
					winnerId = score.ClientId;
				}
			}
			return num >= ScoreLimit.Value;
		}

		private void EnterPhase(GarticPhase next)
		{
			Phase.Value = next;
			int num = next switch
			{
				GarticPhase.WordSelect => 15, 
				GarticPhase.Drawing => Mathf.Max(60, TurnSeconds.Value), 
				GarticPhase.TurnEnd => 6, 
				GarticPhase.GameOver => 15, 
				_ => 0, 
			};
			PhaseEndServerTime.Value = ((num > 0) ? (base.NetworkManager.ServerTime.Time + (double)num) : 0.0);
			NetworkVariable<RoundPhase> currentPhase = CurrentPhase;
			currentPhase.Value = next switch
			{
				GarticPhase.WaitingForPlayers => RoundPhase.WaitingForPlayers, 
				GarticPhase.GameOver => RoundPhase.RoundEnd, 
				GarticPhase.TurnEnd => RoundPhase.RoundEnd, 
				_ => RoundPhase.Hunt, 
			};
		}

		private void OnChatSubmitted(string message)
		{
			SubmitGuessServerRpc(new FixedString128Bytes(message));
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		private void SubmitGuessServerRpc(FixedString128Bytes message, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(1117947231u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in message, default(FastBufferWriter.ForFixedStrings));
				__endSendRpc(ref bufferWriter, 1117947231u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			ulong senderClientId = rpcParams.Receive.SenderClientId;
			string text = message.ToString();
			if (Phase.Value != GarticPhase.Drawing || string.IsNullOrWhiteSpace(text))
			{
				RelayChatClientRpc(GetPlayerName(senderClientId) + ": " + text);
				BubblesOf(senderClientId)?.ServerShow(text, ChatBubbleKind.Message);
			}
			else if (senderClientId == CurrentModeller.Value)
			{
				if (!HasGuessers && GuessMatcher.Matches(text, currentWord))
				{
					EndTurn("SelfGuessed");
				}
				else if (!GuessMatcher.Matches(text, currentWord))
				{
					RelayChatClientRpc(GetPlayerName(senderClientId) + ": " + text);
					BubblesOf(senderClientId)?.ServerShow(text, ChatBubbleKind.Message);
				}
			}
			else if (guessedThisTurn.Contains(senderClientId))
			{
				RelayChatClientRpc(GetPlayerName(senderClientId) + ": " + text);
				BubblesOf(senderClientId)?.ServerShow(text, ChatBubbleKind.Message);
			}
			else if (!GuessMatcher.Matches(text, currentWord))
			{
				RelayChatClientRpc(GetPlayerName(senderClientId) + ": " + text);
				if (GuessMatcher.IsClose(text, currentWord))
				{
					NearMissClientRpc(TargetClient(senderClientId));
					BubblesOf(senderClientId)?.ServerShowTo(senderClientId, text, ChatBubbleKind.GarticClose);
					BubblesOf(senderClientId)?.ServerShowExcept(senderClientId, text, ChatBubbleKind.GarticWrong);
				}
				else
				{
					BubblesOf(senderClientId)?.ServerShow(text, ChatBubbleKind.GarticWrong);
				}
			}
			else
			{
				AwardCorrectGuess(senderClientId);
			}
		}

		private void AwardCorrectGuess(ulong guesserId)
		{
			guessedThisTurn.Add(guesserId);
			BubblesOf(guesserId)?.ServerShow("", ChatBubbleKind.GarticCorrect);
			double num = Mathf.Max(60, TurnSeconds.Value);
			double num2 = Mathf.Max(0f, (float)(PhaseEndServerTime.Value - base.NetworkManager.ServerTime.Time));
			int num3 = 5 + Mathf.RoundToInt((float)(num2 / num) * 15f);
			int count = guessedThisTurn.Count;
			num3 = Mathf.Clamp(num3 - (count - 1) * 3, 5, 20);
			AddScore(guesserId, num3);
			AddScore(CurrentModeller.Value, 8);
			CorrectGuessClientRpc(guesserId, num3);
			if (HasGuessers && guessedThisTurn.Count >= turnOrder.Count - 1)
			{
				EndTurn("EveryoneGuessed");
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void SelectWordServerRpc(int choiceIndex, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(755950410u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, choiceIndex);
				__endSendRpc(ref bufferWriter, 755950410u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (Phase.Value == GarticPhase.WordSelect && rpcParams.Receive.SenderClientId == CurrentModeller.Value && choiceIndex >= 0 && choiceIndex < pendingChoices.Length)
				{
					ChooseWord(pendingChoices[choiceIndex]);
				}
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void RequestHintServerRpc(RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(4164642007u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				__endSendRpc(ref bufferWriter, 4164642007u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute)
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (Phase.Value == GarticPhase.Drawing && rpcParams.Receive.SenderClientId == CurrentModeller.Value && HintsGiven.Value < MaxHints.Value && !(base.NetworkManager.ServerTime.Time < NextHintServerTime.Value))
				{
					RevealNextLetter();
				}
			}
		}

		[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
		public void ApplySettingsServerRpc(FixedString32Bytes categoryId, int scoreLimit, int turnSeconds, bool autoHints, FixedString32Bytes wordLanguage, RpcParams rpcParams = default(RpcParams))
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
				FastBufferWriter bufferWriter = __beginSendRpc(720839062u, rpcParams2, attributeParams, SendTo.Server, RpcDelivery.Reliable);
				bufferWriter.WriteValueSafe(in categoryId, default(FastBufferWriter.ForFixedStrings));
				BytePacker.WriteValueBitPacked(bufferWriter, scoreLimit);
				BytePacker.WriteValueBitPacked(bufferWriter, turnSeconds);
				bufferWriter.WriteValueSafe(in autoHints, default(FastBufferWriter.ForPrimitives));
				bufferWriter.WriteValueSafe(in wordLanguage, default(FastBufferWriter.ForFixedStrings));
				__endSendRpc(ref bufferWriter, 720839062u, rpcParams, attributeParams, SendTo.Server, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute)
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (rpcParams.Receive.SenderClientId == 0L)
			{
				string item = wordLanguage.ToString();
				if (WordCategoryCatalog.Languages().Contains(item))
				{
					WordLanguage.Value = wordLanguage;
				}
				WordCategory wordCategory = WordCategoryCatalog.Find(categoryId.ToString());
				if (wordCategory != null && wordCategory.Supports(WordLanguage.Value.ToString()))
				{
					CategoryId.Value = categoryId;
				}
				ScoreLimit.Value = Mathf.Max(1, scoreLimit);
				TurnSeconds.Value = Mathf.Max(60, turnSeconds);
				AutoHints.Value = autoHints;
				SystemChat("SettingsUpdated");
			}
		}

		[ClientRpc]
		private void WordChoicesClientRpc(string first, string second, string third, ClientRpcParams rpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(3316279752u, rpcParams, RpcDelivery.Reliable);
				bool value = first != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(first);
				}
				bool value2 = second != null;
				bufferWriter.WriteValueSafe(in value2, default(FastBufferWriter.ForPrimitives));
				if (value2)
				{
					bufferWriter.WriteValueSafe(second);
				}
				bool value3 = third != null;
				bufferWriter.WriteValueSafe(in value3, default(FastBufferWriter.ForPrimitives));
				if (value3)
				{
					bufferWriter.WriteValueSafe(third);
				}
				__endSendClientRpc(ref bufferWriter, 3316279752u, rpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				GarticWordChoiceView.Instance?.Show(new string[3] { first, second, third });
				AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.wordChoiceClip : null);
			}
		}

		[ClientRpc]
		private void HideWordChoicesClientRpc()
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(1804990879u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 1804990879u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				GarticWordChoiceView.Instance?.Hide();
			}
		}

		[ClientRpc]
		private void YourWordClientRpc(string word, ClientRpcParams rpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(1475495617u, rpcParams, RpcDelivery.Reliable);
				bool value = word != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(word);
				}
				__endSendClientRpc(ref bufferWriter, 1475495617u, rpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				GarticWordDisplayView.Instance?.SetWord(word);
			}
		}

		[ClientRpc]
		private void CorrectGuessClientRpc(ulong guesserId, int award)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(2657206299u, clientRpcParams, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, guesserId);
				BytePacker.WriteValueBitPacked(bufferWriter, award);
				__endSendClientRpc(ref bufferWriter, 2657206299u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsClient && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (!(ChatView.Instance == null))
			{
				bool num = base.NetworkManager.LocalClientId == guesserId;
				string arg = (num ? "Bildin!" : (GetPlayerName(guesserId) + " bildi!"));
				if (num)
				{
					AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.guessCorrectClip : null);
				}
				ChatView.Instance.AddPlayerMessage($"<color=#5BD75B>✓  {arg}  +{award}</color>");
			}
		}

		[ClientRpc]
		private void TurnFailedClientRpc()
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(1898487654u, clientRpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 1898487654u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.guessFailedClip : null);
			}
		}

		[ClientRpc]
		private void GameOverClientRpc(ulong winnerId)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(810625487u, clientRpcParams, RpcDelivery.Reliable);
				BytePacker.WriteValueBitPacked(bufferWriter, winnerId);
				__endSendClientRpc(ref bufferWriter, 810625487u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				AudioLibrary instance = AudioLibrary.Instance;
				if (!(instance == null))
				{
					AudioLibrary.PlayOneShotClip((base.NetworkManager.LocalClientId == winnerId) ? instance.gameWonClip : instance.gameLostClip);
				}
			}
		}

		[ClientRpc]
		private void NearMissClientRpc(ClientRpcParams rpcParams = default(ClientRpcParams))
		{
			NetworkManager networkManager = base.NetworkManager;
			if ((object)networkManager == null || !networkManager.IsListening)
			{
				Debug.LogError("Rpc methods can only be invoked after starting the NetworkManager!");
				return;
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute && (networkManager.IsServer || networkManager.IsHost))
			{
				FastBufferWriter bufferWriter = __beginSendClientRpc(2460002965u, rpcParams, RpcDelivery.Reliable);
				__endSendClientRpc(ref bufferWriter, 2460002965u, rpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage == __RpcExecStage.Execute && (networkManager.IsClient || networkManager.IsHost))
			{
				__rpc_exec_stage = __RpcExecStage.Send;
				if (ChatView.Instance != null)
				{
					ChatView.Instance.AddPlayerMessage("<color=#E8C15A>Yaklaştın!</color>");
				}
				AudioLibrary.PlayOneShotClip((AudioLibrary.Instance != null) ? AudioLibrary.Instance.guessCloseClip : null);
			}
		}

		[ClientRpc]
		private void RelayChatClientRpc(string composed)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(477094131u, clientRpcParams, RpcDelivery.Reliable);
				bool value = composed != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(composed);
				}
				__endSendClientRpc(ref bufferWriter, 477094131u, clientRpcParams, RpcDelivery.Reliable);
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
		private void TurnEndedClientRpc(string reasonKey, string reasonArg, string word)
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
				FastBufferWriter bufferWriter = __beginSendClientRpc(1750399823u, clientRpcParams, RpcDelivery.Reliable);
				bool value = reasonKey != null;
				bufferWriter.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
				if (value)
				{
					bufferWriter.WriteValueSafe(reasonKey);
				}
				bool value2 = reasonArg != null;
				bufferWriter.WriteValueSafe(in value2, default(FastBufferWriter.ForPrimitives));
				if (value2)
				{
					bufferWriter.WriteValueSafe(reasonArg);
				}
				bool value3 = word != null;
				bufferWriter.WriteValueSafe(in value3, default(FastBufferWriter.ForPrimitives));
				if (value3)
				{
					bufferWriter.WriteValueSafe(word);
				}
				__endSendClientRpc(ref bufferWriter, 1750399823u, clientRpcParams, RpcDelivery.Reliable);
			}
			if (__rpc_exec_stage != __RpcExecStage.Execute || (!networkManager.IsClient && !networkManager.IsHost))
			{
				return;
			}
			__rpc_exec_stage = __RpcExecStage.Send;
			if (!(ChatView.Instance == null))
			{
				string text = (string.IsNullOrEmpty(reasonArg) ? Loc.Get(reasonKey) : Loc.Format(reasonKey, reasonArg));
				if (string.IsNullOrEmpty(word))
				{
					ChatView.Instance.AddSystemMessage(Loc.Format("TurnEnd", text));
				}
				else
				{
					ChatView.Instance.AddPlayerMessage(Loc.Format("TurnEndWord", text, word));
				}
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

		protected override void __initializeVariables()
		{
			if (Phase == null)
			{
				throw new Exception("GarticRoundManager.Phase cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			Phase.Initialize(this);
			__nameNetworkVariable(Phase, "Phase");
			NetworkVariableFields.Add(Phase);
			if (CurrentModeller == null)
			{
				throw new Exception("GarticRoundManager.CurrentModeller cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			CurrentModeller.Initialize(this);
			__nameNetworkVariable(CurrentModeller, "CurrentModeller");
			NetworkVariableFields.Add(CurrentModeller);
			if (MaskedWord == null)
			{
				throw new Exception("GarticRoundManager.MaskedWord cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			MaskedWord.Initialize(this);
			__nameNetworkVariable(MaskedWord, "MaskedWord");
			NetworkVariableFields.Add(MaskedWord);
			if (CategoryId == null)
			{
				throw new Exception("GarticRoundManager.CategoryId cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			CategoryId.Initialize(this);
			__nameNetworkVariable(CategoryId, "CategoryId");
			NetworkVariableFields.Add(CategoryId);
			if (ScoreLimit == null)
			{
				throw new Exception("GarticRoundManager.ScoreLimit cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			ScoreLimit.Initialize(this);
			__nameNetworkVariable(ScoreLimit, "ScoreLimit");
			NetworkVariableFields.Add(ScoreLimit);
			if (TurnSeconds == null)
			{
				throw new Exception("GarticRoundManager.TurnSeconds cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			TurnSeconds.Initialize(this);
			__nameNetworkVariable(TurnSeconds, "TurnSeconds");
			NetworkVariableFields.Add(TurnSeconds);
			if (WordLanguage == null)
			{
				throw new Exception("GarticRoundManager.WordLanguage cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			WordLanguage.Initialize(this);
			__nameNetworkVariable(WordLanguage, "WordLanguage");
			NetworkVariableFields.Add(WordLanguage);
			if (AutoHints == null)
			{
				throw new Exception("GarticRoundManager.AutoHints cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			AutoHints.Initialize(this);
			__nameNetworkVariable(AutoHints, "AutoHints");
			NetworkVariableFields.Add(AutoHints);
			if (MaxHints == null)
			{
				throw new Exception("GarticRoundManager.MaxHints cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			MaxHints.Initialize(this);
			__nameNetworkVariable(MaxHints, "MaxHints");
			NetworkVariableFields.Add(MaxHints);
			if (HintsGiven == null)
			{
				throw new Exception("GarticRoundManager.HintsGiven cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			HintsGiven.Initialize(this);
			__nameNetworkVariable(HintsGiven, "HintsGiven");
			NetworkVariableFields.Add(HintsGiven);
			if (NextHintServerTime == null)
			{
				throw new Exception("GarticRoundManager.NextHintServerTime cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			NextHintServerTime.Initialize(this);
			__nameNetworkVariable(NextHintServerTime, "NextHintServerTime");
			NetworkVariableFields.Add(NextHintServerTime);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			__registerRpc(3166935561u, __rpc_handler_3166935561, "RestartGameServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(1189638458u, __rpc_handler_1189638458, "StartGameServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(1117947231u, __rpc_handler_1117947231, "SubmitGuessServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(755950410u, __rpc_handler_755950410, "SelectWordServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(4164642007u, __rpc_handler_4164642007, "RequestHintServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(720839062u, __rpc_handler_720839062, "ApplySettingsServerRpc", RpcInvokePermission.Everyone);
			__registerRpc(3316279752u, __rpc_handler_3316279752, "WordChoicesClientRpc", RpcInvokePermission.Server);
			__registerRpc(1804990879u, __rpc_handler_1804990879, "HideWordChoicesClientRpc", RpcInvokePermission.Server);
			__registerRpc(1475495617u, __rpc_handler_1475495617, "YourWordClientRpc", RpcInvokePermission.Server);
			__registerRpc(2657206299u, __rpc_handler_2657206299, "CorrectGuessClientRpc", RpcInvokePermission.Server);
			__registerRpc(1898487654u, __rpc_handler_1898487654, "TurnFailedClientRpc", RpcInvokePermission.Server);
			__registerRpc(810625487u, __rpc_handler_810625487, "GameOverClientRpc", RpcInvokePermission.Server);
			__registerRpc(2460002965u, __rpc_handler_2460002965, "NearMissClientRpc", RpcInvokePermission.Server);
			__registerRpc(477094131u, __rpc_handler_477094131, "RelayChatClientRpc", RpcInvokePermission.Server);
			__registerRpc(1750399823u, __rpc_handler_1750399823, "TurnEndedClientRpc", RpcInvokePermission.Server);
			base.__initializeRpcs();
		}

		private static void __rpc_handler_3166935561(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).RestartGameServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1189638458(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).StartGameServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1117947231(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out FixedString128Bytes value, default(FastBufferWriter.ForFixedStrings));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).SubmitGuessServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_755950410(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out int value);
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).SelectWordServerRpc(value, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_4164642007(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).RequestHintServerRpc(ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_720839062(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				reader.ReadValueSafe(out FixedString32Bytes value, default(FastBufferWriter.ForFixedStrings));
				ByteUnpacker.ReadValueBitPacked(reader, out int value2);
				ByteUnpacker.ReadValueBitPacked(reader, out int value3);
				reader.ReadValueSafe(out bool value4, default(FastBufferWriter.ForPrimitives));
				reader.ReadValueSafe(out FixedString32Bytes value5, default(FastBufferWriter.ForFixedStrings));
				RpcParams ext = rpcParams.Ext;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).ApplySettingsServerRpc(value, value2, value3, value4, value5, ext);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_3316279752(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
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
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).WordChoicesClientRpc(s, s2, s3, client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1804990879(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).HideWordChoicesClientRpc();
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1475495617(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
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
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).YourWordClientRpc(s, client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2657206299(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value);
				ByteUnpacker.ReadValueBitPacked(reader, out int value2);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).CorrectGuessClientRpc(value, value2);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1898487654(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).TurnFailedClientRpc();
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_810625487(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ByteUnpacker.ReadValueBitPacked(reader, out ulong value);
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).GameOverClientRpc(value);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_2460002965(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
		{
			NetworkManager networkManager = target.NetworkManager;
			if ((object)networkManager != null && networkManager.IsListening)
			{
				ClientRpcParams client = rpcParams.Client;
				target.__rpc_exec_stage = __RpcExecStage.Execute;
				((GarticRoundManager)target).NearMissClientRpc(client);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_477094131(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
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
				((GarticRoundManager)target).RelayChatClientRpc(s);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		private static void __rpc_handler_1750399823(NetworkBehaviour target, FastBufferReader reader, __RpcParams rpcParams)
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
				((GarticRoundManager)target).TurnEndedClientRpc(s, s2, s3);
				target.__rpc_exec_stage = __RpcExecStage.Send;
			}
		}

		protected internal override string __getTypeName()
		{
			return "GarticRoundManager";
		}
	}
}
