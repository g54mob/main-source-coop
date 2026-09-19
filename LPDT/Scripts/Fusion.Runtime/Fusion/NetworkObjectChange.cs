using System;

namespace Fusion
{
	public readonly ref struct NetworkObjectChange
	{
		public readonly NetworkId Id;

		public readonly PlayerRef Player;

		public readonly NetworkObjectChangeType ChangeType;

		public readonly ReadOnlySpan<(int WordOffset, int WordValue)> WordsAffected;

		public NetworkObjectChange(NetworkId id, NetworkObjectChangeType changeType, PlayerRef player, ReadOnlySpan<(int WordOffset, int WordValue)> wordsAffected)
		{
			Id = id;
			Player = player;
			ChangeType = changeType;
			WordsAffected = wordsAffected;
		}

		public override string ToString()
		{
			return $"[NetworkObjectChange {Id}, Player: {Player}, ChangeType: {ChangeType}]";
		}
	}
}
