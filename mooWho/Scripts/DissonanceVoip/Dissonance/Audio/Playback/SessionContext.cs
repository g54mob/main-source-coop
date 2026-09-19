using System;
using Dissonance.Extensions;
using JetBrains.Annotations;

namespace Dissonance.Audio.Playback
{
	public readonly struct SessionContext : IEquatable<SessionContext>
	{
		public readonly string PlayerName;

		public readonly uint Id;

		public SessionContext([NotNull] string playerName, uint id)
		{
			PlayerName = playerName ?? throw new ArgumentNullException("playerName", "Cannot create a session context with a null player name");
			Id = id;
		}

		public bool Equals(SessionContext other)
		{
			if (string.Equals(PlayerName, other.PlayerName))
			{
				return Id == other.Id;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is SessionContext other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (PlayerName.GetFnvHashCode() * 397) ^ (int)Id;
		}
	}
}
