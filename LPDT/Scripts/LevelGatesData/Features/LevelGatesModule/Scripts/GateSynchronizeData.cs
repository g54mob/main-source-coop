using System;

namespace Features.LevelGatesModule.Scripts
{
	[Serializable]
	public struct GateSynchronizeData
	{
		public int OwnerId;

		public bool IsInsideGate;

		public GateSynchronizeOperation Operation;
	}
}
