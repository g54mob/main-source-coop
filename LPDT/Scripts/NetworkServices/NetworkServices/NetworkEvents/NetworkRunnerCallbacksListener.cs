using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using Zenject;

namespace NetworkServices.NetworkEvents
{
	public class NetworkRunnerCallbacksListener : MonoBehaviour, INetworkRunnerCallbacks, IPublicFacingInterface
	{
		private NetworkRunnerEventBus _eventBus;

		[Inject]
		public void InjectDependencies(NetworkRunnerEventBus eventBus)
		{
			_eventBus = eventBus;
		}

		public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
			_eventBus.Publish(new OnObjectExitAOIEvent(runner, obj, player));
		}

		public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
			_eventBus.Publish(new OnObjectEnterAOIEvent(runner, obj, player));
		}

		public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
		{
			_eventBus.Publish(new OnPlayerJoinedEvent(runner, player));
		}

		public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
		{
			_eventBus.Publish(new OnPlayerLeftEvent(runner, player));
		}

		public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
		{
			_eventBus.Publish(new OnShutdownEvent(runner, shutdownReason));
		}

		public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
		{
			_eventBus.Publish(new OnDisconnectedFromServerEvent(runner, reason));
		}

		public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
		{
			_eventBus.Publish(new OnConnectRequestEvent(runner, request, token));
		}

		public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
		{
			_eventBus.Publish(new OnConnectFailedEvent(runner, remoteAddress, reason));
		}

		public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
		{
			_eventBus.Publish(new OnUserSimulationMessageEvent(runner, message));
		}

		public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
		{
			_eventBus.Publish(new OnReliableDataReceivedEvent(runner, player, key, data));
		}

		public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
		{
			_eventBus.Publish(new OnReliableDataProgressEvent(runner, player, key, progress));
		}

		public void OnInput(NetworkRunner runner, NetworkInput input)
		{
			_eventBus.Publish(new OnInputEvent(runner, input));
		}

		public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
		{
			_eventBus.Publish(new OnInputMissingEvent(runner, player, input));
		}

		public void OnConnectedToServer(NetworkRunner runner)
		{
			_eventBus.Publish(new OnConnectedToServerEvent(runner));
		}

		public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
		{
			_eventBus.Publish(new OnSessionListUpdatedEvent(runner, sessionList));
		}

		public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
		{
			_eventBus.Publish(new OnCustomAuthenticationResponseEvent(runner, data));
		}

		public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
		{
			_eventBus.Publish(new OnHostMigrationEvent(runner, hostMigrationToken));
		}

		public void OnSceneLoadDone(NetworkRunner runner)
		{
			_eventBus.Publish(new OnSceneLoadDoneEvent(runner));
		}

		public void OnSceneLoadStart(NetworkRunner runner)
		{
			_eventBus.Publish(new OnSceneLoadStartEvent(runner));
		}
	}
}
