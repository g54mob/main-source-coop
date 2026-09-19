using System;
using Features.NetworkRandomModule.Scripts;

namespace Features.StoreModule.Scripts
{
	public class StoreCustomizationModel : INetworkRandomConsumer
	{
		private static readonly DeterministicHash NameHash = new DeterministicHash(typeof(StoreCustomizationModel).FullName);

		public Random Random { get; private set; }

		public int GetConsumerIdentifier()
		{
			return NameHash.GetRaw();
		}

		public void InjectNetworkRandom(Random random)
		{
			Random = random;
		}
	}
}
