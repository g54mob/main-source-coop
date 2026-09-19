using System.Collections.Generic;

namespace Features.CartUpgradesModule.Scripts.Core
{
	public class CartTierRegistry : ICartTierRegistry
	{
		private readonly List<ICartTierCarrier> _carts = new List<ICartTierCarrier>();

		public IReadOnlyList<ICartTierCarrier> Carts => _carts;

		public void Register(ICartTierCarrier cart)
		{
			if (cart != null && !_carts.Contains(cart))
			{
				_carts.Add(cart);
			}
		}

		public void Unregister(ICartTierCarrier cart)
		{
			_carts.Remove(cart);
		}
	}
}
