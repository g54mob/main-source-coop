using System.Collections.Generic;

namespace Features.CartUpgradesModule.Scripts.Core
{
	public interface ICartTierRegistry
	{
		IReadOnlyList<ICartTierCarrier> Carts { get; }

		void Register(ICartTierCarrier cart);

		void Unregister(ICartTierCarrier cart);
	}
}
