using System;

namespace Features.MultiplayerSessionServices.Scripts
{
	[Serializable]
	public class RumSessionEntry
	{
		public int RumType;

		public float Duration;

		public bool IsTemporal;
	}
}
