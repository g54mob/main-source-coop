using System;

namespace Features.PlayerIdentityModule
{
	public readonly struct PersistentPlayerId : IEquatable<PersistentPlayerId>
	{
		public static readonly PersistentPlayerId None = new PersistentPlayerId(string.Empty);

		public string Value { get; }

		public bool IsNone => string.IsNullOrEmpty(Value);

		public PersistentPlayerId(string value)
		{
			Value = value ?? string.Empty;
		}

		public bool Equals(PersistentPlayerId other)
		{
			return string.Equals(Value, other.Value, StringComparison.Ordinal);
		}

		public override bool Equals(object obj)
		{
			if (obj is PersistentPlayerId other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (Value != null)
			{
				return StringComparer.Ordinal.GetHashCode(Value);
			}
			return 0;
		}

		public override string ToString()
		{
			if (!IsNone)
			{
				return Value;
			}
			return "<none>";
		}
	}
}
