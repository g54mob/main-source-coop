using System;
using Fusion;
using Fusion.Sockets;

namespace NetworkServices.NetworkEvents
{
	public class OnReliableDataReceivedEvent : NetworkRunnerEvent
	{
		public readonly NetworkRunner Runner;

		public readonly PlayerRef Player;

		public readonly ReliableKey Key;

		public readonly byte[] Data;

		public OnReliableDataReceivedEvent(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
		{
			Runner = runner;
			Player = player;
			Key = key;
			Data = data.ToArray();
		}
	}
}
