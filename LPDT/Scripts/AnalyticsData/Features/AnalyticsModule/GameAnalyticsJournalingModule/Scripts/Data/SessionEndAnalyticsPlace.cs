using System;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	public readonly struct SessionEndAnalyticsPlace : IEquatable<SessionEndAnalyticsPlace>
	{
		public static readonly SessionEndAnalyticsPlace Unknown = new SessionEndAnalyticsPlace(SessionEndAnalyticsPlaceKind.Unknown);

		public SessionEndAnalyticsPlaceKind Kind { get; }

		public int LevelNumber { get; }

		public SessionEndAnalyticsPlace(SessionEndAnalyticsPlaceKind kind, int levelNumber = 0)
		{
			Kind = kind;
			LevelNumber = (UsesLevelNumber(kind) ? levelNumber : 0);
		}

		public bool Equals(SessionEndAnalyticsPlace other)
		{
			if (Kind == other.Kind)
			{
				return LevelNumber == other.LevelNumber;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is SessionEndAnalyticsPlace other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((int)Kind * 397) ^ LevelNumber;
		}

		private static bool UsesLevelNumber(SessionEndAnalyticsPlaceKind kind)
		{
			if (kind != SessionEndAnalyticsPlaceKind.Level && kind != SessionEndAnalyticsPlaceKind.Shop)
			{
				return kind == SessionEndAnalyticsPlaceKind.Beach;
			}
			return true;
		}
	}
}
