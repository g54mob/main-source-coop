using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace EvilCore.Networking
{
	public interface INetworkManager
	{
		TransportType ActiveTransportType { get; }

		bool IsLocalTestMode { get; }

		bool IsSingleplayerSession { get; }

		string NetworkAddress { get; set; }

		int MaxConnections { get; }

		IReadOnlyList<INetworkPlayer> ConnectedPlayers { get; }

		event Action OnConnectedPlayersChanged;

		void StartHost();

		void StartClient();

		void BeginSinglePlayerSession();

		void StartSinglePlayerHost(string worldName, int seed);

		void StartSinglePlayerContinue(string slotId);

		void ActivateMultiplayer();

		string ConsumePendingMultiplayerSlot();

		void Disconnect();

		void DisconnectByAddress(string address);

		bool TryGetNetworkObjectById(uint networkId, out GameObject networkObject);

		UniTask<(bool success, GameObject networkObject)> TryGetNetworkObjectByIdAsync(uint networkId, int maxAttempts = 50, int delayBetweenAttemptsMs = 100, CancellationToken cancellationToken = default(CancellationToken));

		UniTask<(bool success, GameObject networkObject)> TryGetNetworkObjectByIdWithBackoffAsync(uint networkId, int maxAttempts = 30, int initialDelayMs = 100, int maxDelayMs = 2000, float backoffMultiplier = 1.5f, CancellationToken cancellationToken = default(CancellationToken));

		void RegisterPlayer(INetworkPlayer player);

		void UnregisterPlayer(INetworkPlayer player);
	}
}
