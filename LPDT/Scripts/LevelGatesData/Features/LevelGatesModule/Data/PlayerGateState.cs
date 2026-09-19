using System;

namespace Features.LevelGatesModule.Data
{
	[Serializable]
	public struct PlayerGateState
	{
		public int OwnerId;

		public bool PlayerInsideGate;
	}
}
