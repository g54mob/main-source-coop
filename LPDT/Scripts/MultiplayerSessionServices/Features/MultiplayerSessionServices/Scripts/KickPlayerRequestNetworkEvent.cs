using System;
using Features.CustomNetworkEventsModule.Scripts;
using UnityEngine;

namespace Features.MultiplayerSessionServices.Scripts
{
	[Serializable]
	public class KickPlayerRequestNetworkEvent : NetworkEventBase<KickPlayerRequestNetworkEvent>
	{
		[field: SerializeField]
		public int PlayerToKickId { get; private set; }

		public void SendKickRequest(int playerId)
		{
			PlayerToKickId = playerId;
			Send();
		}
	}
}
