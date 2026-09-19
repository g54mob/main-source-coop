using System;

namespace Features.PlayerPresenceModule
{
	public readonly struct SessionPlayerProfileData : IEquatable<SessionPlayerProfileData>
	{
		public string Nickname { get; }

		public int ColorId { get; }

		public int CosmeticsMask { get; }

		public int StatusMask { get; }

		public int LifeState { get; }

		public SessionPlayerProfileData(string nickname, int colorId, int cosmeticsMask, int statusMask, int lifeState)
		{
			Nickname = nickname ?? string.Empty;
			ColorId = colorId;
			CosmeticsMask = cosmeticsMask;
			StatusMask = statusMask;
			LifeState = lifeState;
		}

		public bool Equals(SessionPlayerProfileData other)
		{
			if (string.Equals(Nickname, other.Nickname, StringComparison.Ordinal) && ColorId == other.ColorId && CosmeticsMask == other.CosmeticsMask && StatusMask == other.StatusMask)
			{
				return LifeState == other.LifeState;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is SessionPlayerProfileData other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine((Nickname != null) ? StringComparer.Ordinal.GetHashCode(Nickname) : 0, ColorId, CosmeticsMask, StatusMask, LifeState);
		}
	}
}
