using System.Collections.Generic;

public class ShoppingCart
{
	public bool IsEmpty => Cart.Count == 0;

	public List<ShopItem> Cart { get; private set; }

	public int CartValue { get; private set; }

	public ShoppingCart()
	{
		Cart = new List<ShopItem>();
		CartValue = 0;
	}

	public void AddItemToCart(ShopItem itemToAdd)
	{
		Cart.Add(itemToAdd);
		CartValue += itemToAdd.Price;
	}

	public void ClearCart()
	{
		Cart = new List<ShopItem>();
		CartValue = 0;
	}
}
