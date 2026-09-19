using System;
using Features.CustomNetworkEventsModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts
{
	[Serializable]
	public class ItemCostLossNetworkEvent : NetworkEventBase<ItemCostLossNetworkEvent>
	{
		[field: SerializeField]
		public int CostLoss { get; private set; }

		[field: SerializeField]
		public Vector3 CostPosition { get; private set; }

		[field: SerializeField]
		public Vector3 HitPosition { get; private set; }

		[field: SerializeField]
		public NetworkId NetworkObjectId { get; private set; }

		public void SendEvent(int costLoss, Vector3 costPosition, Vector3 hitPosition, NetworkId networkObjectId)
		{
			CostLoss = costLoss;
			CostPosition = costPosition;
			HitPosition = hitPosition;
			NetworkObjectId = networkObjectId;
			Send();
		}
	}
}
