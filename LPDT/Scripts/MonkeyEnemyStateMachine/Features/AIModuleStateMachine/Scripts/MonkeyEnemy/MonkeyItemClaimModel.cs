using System.Collections.Generic;
using Features.GrabModule.Scripts;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy
{
	public class MonkeyItemClaimModel
	{
		public SimplePointGrabable ClaimedGrabable { get; set; }

		public bool HasClaim { get; set; }

		public HashSet<uint> ConsumedTargetItemIds { get; } = new HashSet<uint>();

		public void ClearClaimState()
		{
			ClaimedGrabable = null;
			HasClaim = false;
		}
	}
}
