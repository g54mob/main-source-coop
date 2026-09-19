using System;
using UnityEngine;

namespace Features.NetworkRandomModule.Scripts
{
	[Serializable]
	public class NetworkRandomInjectRequest
	{
		[field: SerializeField]
		public int ConsumerId { get; private set; }

		[field: SerializeField]
		public int Seed { get; private set; }

		public NetworkRandomInjectRequest()
		{
		}

		public NetworkRandomInjectRequest(int consumerId, int seed)
		{
			ConsumerId = consumerId;
			Seed = seed;
		}
	}
}
