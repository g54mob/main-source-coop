using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;
using pworld.Scripts.Extensions;

public class DealRewardItem_1 : DealRewardBase
{
	public override bool UseInGame => false;

	public override DealRewardBase CreateNew()
	{
		return new DealRewardItem_1();
	}

	public override string GetRewardDescription()
	{
		return $"In return you will get <b><u>items worth ${GetAmount()}</b></u>";
	}

	private int GetAmount()
	{
		int num = 0;
		num = difficulty switch
		{
			DIFFICULTY.veryEasy => 0, 
			DIFFICULTY.easy => 1, 
			DIFFICULTY.medium => 2, 
			DIFFICULTY.hard => 3, 
			DIFFICULTY.veryHard => 4, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		return (int)((float)BigNumbers.GetMoneyFromScoreByRun(BigNumbers.GetQuota(num), num) * 0.85f);
	}

	public override bool ClaimReward()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return false;
		}
		ShoppingCart itemsNew = GetItemsNew();
		ShopHandler.Instance.BuyItem(itemsNew);
		return true;
	}

	public override string GetRewardClaimDescription()
	{
		return "";
	}

	public override void MakeLocalized()
	{
		Debug.LogError("Localization string not implemented!");
	}

	public override void DebugReward()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		base.DebugReward();
		int num = 100;
		Debug.Log("item reward debug start");
		for (int i = 0; i < num; i++)
		{
			foreach (ShopItem item in GetItemsNew().Cart)
			{
				if (!dictionary.ContainsKey(item.Item.id))
				{
					dictionary.Add(item.Item.id, 0);
				}
				dictionary[item.Item.id]++;
			}
		}
		foreach (Item item2 in (from item in new List<Item>(SingletonAsset<ItemDatabase>.Instance.lastLoadedItems).Where((Item o) => o.purchasable).ToList()
			orderby item.price descending
			select item).ToList())
		{
			int num2 = 0;
			if (dictionary.ContainsKey(item2.id))
			{
				num2 = dictionary[item2.id];
			}
			Debug.Log($"item {item2.name} bought {(float)num2 / (float)num} times");
		}
		Debug.Log("item reward debug end");
	}

	private ShoppingCart GetItemsNew()
	{
		ShoppingCart shoppingCart = new ShoppingCart();
		int budget = GetAmount();
		List<Item> itemsSorted = new List<Item>(SingletonAsset<ItemDatabase>.Instance.lastLoadedItems);
		itemsSorted = itemsSorted.Where((Item o) => o.purchasable).ToList();
		itemsSorted = itemsSorted.OrderByDescending((Item item2) => item2.price).ToList();
		while (budget > 0)
		{
			Item item = GetRandomItem();
			if (item == null)
			{
				break;
			}
			budget -= item.price;
			shoppingCart.Cart.Add(new ShopItem
			{
				Item = item
			});
		}
		return shoppingCart;
		Item GetRandomItem()
		{
			itemsSorted = itemsSorted.Where((Item o) => o.price <= budget).ToList();
			List<(Item, float)> list = new List<(Item, float)>();
			float num = 0f;
			foreach (Item item2 in itemsSorted)
			{
				list.Add((item2, GetWeight(item2)));
				num += GetWeight(item2);
			}
			for (int num2 = 0; num2 < 10; num2++)
			{
				float num3 = UnityEngine.Random.Range(0f, num);
				foreach (var item3 in list)
				{
					num3 -= item3.Item2;
					if (num3 <= 0f)
					{
						return item3.Item1;
					}
				}
			}
			return null;
		}
		float GetWeight(Item item2)
		{
			return (float)budget / (float)item2.price + 5f;
		}
	}

	private ShoppingCart GetItems()
	{
		ShoppingCart shoppingCart = new ShoppingCart();
		List<Item> source = new List<Item>(SingletonAsset<ItemDatabase>.Instance.lastLoadedItems);
		source = source.Where((Item o) => o.purchasable).ToList();
		source = source.OrderByDescending((Item item2) => item2.price).ToList();
		int num = GetAmount();
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int num2 = source.PGetRandomIndex() - 1; num2 >= 0; num2--)
			{
				if (source[num2].price <= num)
				{
					num -= source[num2].price;
					ShopItem item = new ShopItem
					{
						Item = source[num2]
					};
					shoppingCart.Cart.Add(item);
					flag = true;
					break;
				}
			}
		}
		return shoppingCart;
	}

	public override byte GetIndex()
	{
		return GetIndex<DealRewardItem_1>();
	}
}
