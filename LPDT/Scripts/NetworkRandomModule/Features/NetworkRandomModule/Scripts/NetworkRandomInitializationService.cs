using System.Collections.Generic;
using UnityEngine;

namespace Features.NetworkRandomModule.Scripts
{
	public class NetworkRandomInitializationService : INetworkRandomInitializationService
	{
		private readonly List<INetworkRandomConsumer> _networkRandomConsumers;

		private readonly NetworkRandomRequestModel _networkRandomRequestModel;

		public NetworkRandomInitializationService(List<INetworkRandomConsumer> networkRandomConsumers, NetworkRandomRequestModel networkRandomRequestModel)
		{
			_networkRandomConsumers = networkRandomConsumers;
			_networkRandomRequestModel = networkRandomRequestModel;
		}

		public void InjectNetworkRandoms()
		{
			foreach (INetworkRandomConsumer networkRandomConsumer in _networkRandomConsumers)
			{
				_networkRandomRequestModel.AddRequest(networkRandomConsumer.GetConsumerIdentifier(), Random.Range(0, int.MaxValue));
			}
			_networkRandomRequestModel.Synchronize();
		}
	}
}
