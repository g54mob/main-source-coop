using System;
using UnityEngine;

public struct ShopItem
{
	public Item Item;

	public bool HasSale { get; private set; }

	public string DisplayName { get; private set; }

	public int Price { get; private set; }

	public byte ItemID => Item.id;

	public int Quantity { get; private set; }

	public byte UpgradeID { get; private set; }

	public ShopItem(Item dbItem)
	{
		Item = dbItem;
		Price = dbItem.price;
		UpgradeID = (byte)((dbItem is CameraUpgradeItem) ? (dbItem as CameraUpgradeItem).upgradeId : 0);
		Quantity = dbItem.quantity;
		if (Quantity == 0)
		{
			Quantity = int.MaxValue;
		}
		HasSale = false;
		DisplayName = string.Empty;
		UpdateLocalizedName();
	}

	public void UpdateLocalizedName()
	{
		if (Enum.TryParse<LocalizationKeys.Keys>(Item.name.Trim().Replace(" ", ""), out var result))
		{
			DisplayName = LocalizationKeys.GetLocalizedString(result);
			return;
		}
		DisplayName = Item.displayName;
		Debug.LogError("Failed to get Localized displayName for: " + DisplayName);
	}
}
