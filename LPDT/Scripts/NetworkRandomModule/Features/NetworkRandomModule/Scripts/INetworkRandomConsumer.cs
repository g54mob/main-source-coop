using System;

namespace Features.NetworkRandomModule.Scripts
{
	public interface INetworkRandomConsumer
	{
		int GetConsumerIdentifier();

		void InjectNetworkRandom(Random random);
	}
}
