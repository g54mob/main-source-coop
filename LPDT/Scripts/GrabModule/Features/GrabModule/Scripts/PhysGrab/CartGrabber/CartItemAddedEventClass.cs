using System;

namespace Features.GrabModule.Scripts.PhysGrab.CartGrabber
{
	public class CartItemAddedEventClass
	{
		public event Action<ICartItemsContainer, IPointGrabable, int> OnGrabableAddedInCart;

		internal void InvokeOnGrabableAddedInCart(ICartItemsContainer cartItemsContainer, IPointGrabable grabable, int adderPlayerId)
		{
			this.OnGrabableAddedInCart?.Invoke(cartItemsContainer, grabable, adderPlayerId);
		}
	}
}
