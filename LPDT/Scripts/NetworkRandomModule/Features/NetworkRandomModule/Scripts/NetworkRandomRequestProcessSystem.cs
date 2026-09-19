using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Features.NetworkRandomModule.Scripts
{
	public class NetworkRandomRequestProcessSystem : IInitializable, IDisposable
	{
		private readonly List<INetworkRandomConsumer> _networkRandomConsumers;

		private readonly NetworkRandomRequestModel _networkRandomRequestModel;

		public NetworkRandomRequestProcessSystem(List<INetworkRandomConsumer> networkRandomConsumers, NetworkRandomRequestModel networkRandomRequestModel)
		{
			_networkRandomConsumers = networkRandomConsumers;
			_networkRandomRequestModel = networkRandomRequestModel;
		}

		public void Initialize()
		{
			_networkRandomRequestModel.OnRequestAdded += InjectNetworkRandom;
		}

		public void Dispose()
		{
			_networkRandomRequestModel.OnRequestAdded -= InjectNetworkRandom;
		}

		private void InjectNetworkRandom(NetworkRandomInjectRequest networkRandomInjectRequest)
		{
			_networkRandomConsumers.FirstOrDefault((INetworkRandomConsumer x) => x.GetConsumerIdentifier() == networkRandomInjectRequest.ConsumerId)?.InjectNetworkRandom(new Random(networkRandomInjectRequest.Seed));
		}
	}
}
