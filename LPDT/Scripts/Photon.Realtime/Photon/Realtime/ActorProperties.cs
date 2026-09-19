using System;

namespace Photon.Realtime
{
	public class ActorProperties
	{
		[Obsolete("Renamed. Use ActorProperties.NickName.")]
		public const byte PlayerName = byte.MaxValue;

		public const byte NickName = byte.MaxValue;

		public const byte IsInactive = 254;

		public const byte UserId = 253;
	}
}
