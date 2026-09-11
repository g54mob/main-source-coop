using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Zorro.Core;

public class ShopHandler : MonoBehaviour
{
	public GameObject droneObject;

	[SerializeField]
	private Transform m_BoughtItemPosition;

	[SerializeField]
	private TextMeshProUGUI m_ItemAddedToCartText;

	public SFX_Instance clickSFX;

	public SFX_Instance addSFX;

	public SFX_Instance noMoneySFX;

	public SFX_Instance emptykSFX;

	public SFX_Instance purchaseSFX;

	private string m_AddedToCartText;

	private RoomStatsHolder m_RoomStats;

	private Dictionary<byte, ShopItem> m_ItemsForSaleDictionary;

	private ShoppingCart m_ShoppingCart;

	private Dictionary<ShopItemCategory, List<ShopItem>> m_CategoryItemDic;

	private Action m_OnBuyAction;

	private ShopViewScreen m_ShopView;

	private PhotonView m_PhotonView;

	public static ShopHandler Instance { get; private set; }

	public int NumberOfItemsInShop => m_ItemsForSaleDictionary.Count();

	private void Awake()
	{
		Instance = this;
		LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
		OnLanguageChanged();
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
	}

	private void OnLanguageChanged()
	{
		m_AddedToCartText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.AddedToCart);
	}

	public void InitShopHandler()
	{
		Debug.Log("Shop Started, Initializing...");
		m_PhotonView = GetComponent<PhotonView>();
		m_RoomStats = SurfaceNetworkHandler.RoomStats;
		InitShop();
		InitShopScreen();
		if (!PhotonNetwork.IsMasterClient)
		{
			m_PhotonView.RPC("RPCM_RequestShop", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
		}
	}

	public void InitShopScreen()
	{
		m_ShopView = GetComponentInChildren<ShopViewScreen>();
		m_ShopView.Init(this);
	}

	private void InitShop()
	{
		m_ShoppingCart = new ShoppingCart();
		ItemDatabase instance = SingletonAsset<ItemDatabase>.Instance;
		m_CategoryItemDic = new Dictionary<ShopItemCategory, List<ShopItem>>();
		Item[] array = instance.Objects.Where((Item item) => item.purchasable).ToArray();
		m_ItemsForSaleDictionary = new Dictionary<byte, ShopItem>();
		VerboseDebug.Log("Found: " + array.Length + " Items For Sale!");
		for (int num = 0; num < array.Length; num++)
		{
			ShopItem shopItem = new ShopItem(array[num]);
			m_ItemsForSaleDictionary.Add(shopItem.ItemID, shopItem);
			ShopItemCategory category = array[num].Category;
			if (category == ShopItemCategory.Invalid)
			{
				Debug.LogError("Item: " + shopItem.DisplayName + " Has Category: " + category.ToString() + " Skipping adding it to the shop");
			}
			else
			{
				if (!m_CategoryItemDic.ContainsKey(category))
				{
					m_CategoryItemDic.Add(array[num].Category, new List<ShopItem>());
				}
				m_CategoryItemDic[category].Add(shopItem);
			}
		}
		SortShopItemsByPrice();
	}

	private void SortShopItemsByPrice()
	{
		foreach (KeyValuePair<ShopItemCategory, List<ShopItem>> item in m_CategoryItemDic)
		{
			item.Value.Sort((ShopItem a, ShopItem b) => a.Price.CompareTo(b.Price));
		}
	}

	public void OnAddToCartItemClicked(byte itemID)
	{
		Debug.Log("Item Clicked!");
		ShopItem item = default(ShopItem);
		if (TryGetShopItem(itemID, ref item))
		{
			int cost = m_ShoppingCart.CartValue + item.Price;
			if (!m_RoomStats.CanAfford(cost))
			{
				noMoneySFX.Play(base.transform.position);
				Debug.Log("Cant afford, dont add to cart");
				return;
			}
		}
		if (PhotonNetwork.IsMasterClient)
		{
			m_PhotonView.RPC("RPCA_AddItemToCart", RpcTarget.All, itemID);
			addSFX.Play(base.transform.position);
		}
		else
		{
			addSFX.Play(base.transform.position);
			m_PhotonView.RPC("RPCM_RequestShopAction", RpcTarget.MasterClient, (byte)2, itemID);
		}
	}

	public void OnChangeCategoryClicked(byte category)
	{
		ShopItemCategory shopItemCategory = (ShopItemCategory)category;
		Debug.Log("Change Category Clicked: " + shopItemCategory);
		clickSFX.Play(base.transform.position);
		if (PhotonNetwork.IsMasterClient)
		{
			m_PhotonView.RPC("RPCA_ChangeCategory", RpcTarget.All, category);
		}
		else
		{
			m_PhotonView.RPC("RPCM_RequestShopAction", RpcTarget.MasterClient, (byte)1, category);
		}
	}

	public void OnClearCartClicked()
	{
		Debug.Log("Clear Cart Clicked!");
		if (m_ShoppingCart.IsEmpty)
		{
			clickSFX.Play(base.transform.position);
			Debug.Log("Cart is Empty, nothing to clear!");
			return;
		}
		emptykSFX.Play(base.transform.position);
		if (PhotonNetwork.IsMasterClient)
		{
			m_PhotonView.RPC("RPCA_ClearCart", RpcTarget.All);
			return;
		}
		m_PhotonView.RPC("RPCM_RequestShopAction", RpcTarget.MasterClient, (byte)3, null);
	}

	public void ForceUpdate()
	{
		VerboseDebug.Log("Force Updating ShopView");
		if (m_ShopView != null)
		{
			m_ShopView.UpdateView();
		}
	}

	public void OnOrderCartClicked()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			BuyItem(m_ShoppingCart);
			return;
		}
		if (m_ShoppingCart.IsEmpty)
		{
			clickSFX.Play(base.transform.position);
			Debug.Log("Cart is Empty, cant buy!");
			return;
		}
		purchaseSFX.Play(base.transform.position);
		m_PhotonView.RPC("RPCM_RequestShopAction", RpcTarget.MasterClient, (byte)4, null);
	}

	[PunRPC]
	private void RPCM_RequestShop(int playerRequesting)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		byte[] array = new byte[0];
		if (m_ShoppingCart != null)
		{
			array = m_ShoppingCart.Cart.Select((ShopItem item) => item.ItemID).ToArray();
		}
		Photon.Realtime.Player player = PhotonNetwork.CurrentRoom.GetPlayer(playerRequesting);
		if (player == null)
		{
			Debug.LogError("Cant find player " + playerRequesting);
			m_PhotonView.RPC("RPCO_UpdateShop", RpcTarget.Others, (byte)m_ShopView.CurrentCategoryIndex, array);
		}
		else
		{
			m_PhotonView.RPC("RPCO_UpdateShop", player, (byte)m_ShopView.CurrentCategoryIndex, array);
		}
	}

	[PunRPC]
	private void RPCO_UpdateShop(byte category, byte[] itemsInCart)
	{
		Debug.Log("Got New Shop Item Category From Master");
		ShopItemCategory shopItemCategory = (ShopItemCategory)category;
		if (shopItemCategory != ShopItemCategory.Invalid)
		{
			m_ShopView.SetIndex(shopItemCategory);
		}
		ResetCart(itemsInCart);
	}

	private void ResetCart(byte[] itemsInCart)
	{
		m_ShoppingCart = new ShoppingCart();
		foreach (byte itemID in itemsInCart)
		{
			RPCA_AddItemToCart(itemID);
		}
	}

	[PunRPC]
	private void RPCM_RequestShopAction(byte shopAction, byte argument)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("Client got this call, not supported!");
			return;
		}
		switch ((ShopAction)shopAction)
		{
		case ShopAction.ChangeCategory:
			OnChangeCategoryClicked(argument);
			break;
		case ShopAction.AddItemToCart:
			OnAddToCartItemClicked(argument);
			break;
		case ShopAction.ClearCart:
			OnClearCartClicked();
			break;
		case ShopAction.BuyCart:
			OnOrderCartClicked();
			break;
		}
	}

	[PunRPC]
	private void RPCA_ClearCart()
	{
		Debug.Log("Clear Cart!");
		m_ShoppingCart.ClearCart();
		m_ShopView.OnCartUpdated();
		m_ShopView.UpdateView();
	}

	[PunRPC]
	private void RPCA_AddItemToCart(byte itemID)
	{
		ShopItem item = default(ShopItem);
		if (TryGetShopItem(itemID, ref item))
		{
			m_ShoppingCart.AddItemToCart(item);
			m_ShopView.OnCartUpdated();
			StartCoroutine(ShowItemAddedToCart(2f, item));
		}
		else
		{
			Debug.LogError("Cant Find Item: " + itemID);
		}
	}

	private IEnumerator ShowItemAddedToCart(float f, ShopItem item)
	{
		m_ItemAddedToCartText.text = item.DisplayName + m_AddedToCartText;
		m_ItemAddedToCartText.gameObject.SetActive(value: true);
		yield return new WaitForSecondsRealtime(f);
		m_ItemAddedToCartText.gameObject.SetActive(value: false);
		m_ItemAddedToCartText.text = string.Empty;
	}

	[PunRPC]
	private void RPCA_ChangeCategory(byte categoryID)
	{
		ShopItemCategory argument = (ShopItemCategory)categoryID;
		Debug.Log("CHanging Category! " + argument);
		m_ShopView.ChangeCategory(argument);
	}

	public bool TryGetShopItem(byte index, ref ShopItem item)
	{
		if (!m_ItemsForSaleDictionary.ContainsKey(index))
		{
			return false;
		}
		item = m_ItemsForSaleDictionary[index];
		return true;
	}

	public bool TryGetShopCategoryItems(ShopItemCategory categoryIndex, ref ShopItem[] items)
	{
		if (!m_CategoryItemDic.ContainsKey(categoryIndex))
		{
			return false;
		}
		items = m_CategoryItemDic[categoryIndex].ToArray();
		return true;
	}

	public void BuyItem(ShoppingCart cart)
	{
		if (cart.IsEmpty)
		{
			clickSFX.Play(base.transform.position);
			Debug.Log("Cart is Empty, cant buy!");
			return;
		}
		purchaseSFX.Play(base.transform.position);
		int cartValue = cart.CartValue;
		byte[] itemIDs = cart.Cart.Select((ShopItem item) => item.ItemID).ToArray();
		Vector3 position = m_BoughtItemPosition.position;
		BuyItem(cartValue, itemIDs, position.x, position.y, position.z);
	}

	private void BuyItem(int cost, byte[] itemIDs, float x, float y, float z)
	{
		m_RoomStats = SurfaceNetworkHandler.RoomStats;
		Debug.Log("Trying to buy!");
		if (m_RoomStats.CanAfford(cost))
		{
			Debug.Log("Should Buy Here!");
			for (int i = 0; i < itemIDs.Length; i++)
			{
				if (ItemDatabase.TryGetItemFromID(itemIDs[i], out var item))
				{
					Debug.Log("Buying " + item.name);
					CheckIfBoughtCameraUpgrade(item);
					if (PhotonNetwork.IsMasterClient && PhotonGameLobbyHandler.CurrentObjective is BuyEquipmentObjective)
					{
						PhotonGameLobbyHandler.Instance.SetCurrentObjective(new EnterDiveBellDay2Objective());
					}
					m_OnBuyAction?.Invoke();
				}
			}
			m_PhotonView.RPC("RPCA_SpawnDrone", RpcTarget.All, itemIDs);
			m_RoomStats.RemoveMoney(cost);
			OnClearCartClicked();
		}
		else
		{
			Debug.Log("Cant afford to buy! Cost: " + cost);
		}
	}

	[PunRPC]
	private void RPCA_SpawnDrone(byte[] itemIDs)
	{
		List<Item> list = new List<Item>();
		for (int i = 0; i < itemIDs.Length; i++)
		{
			if (ItemDatabase.TryGetItemFromID(itemIDs[i], out var item))
			{
				list.Add(item);
				Debug.Log("Drone is carrying: " + item.name);
			}
		}
		UnityEngine.Object.Instantiate(droneObject, Vector3.zero, Quaternion.Euler(new Vector3(0f, -30f, 0f))).GetComponent<Drone>().items = list.ToArray();
	}

	private void CheckIfBoughtCameraUpgrade(Item item)
	{
		if (item.Category == ShopItemCategory.Upgrades && item is CameraUpgradeItem)
		{
			CameraUpgradeItem cameraUpgradeItem = (CameraUpgradeItem)item;
			if (cameraUpgradeItem.upgradeId != 0)
			{
				Debug.Log("Bought Camera Upgrade: " + item.name);
				m_RoomStats.AddCameraUpgrade(cameraUpgradeItem.upgradeId);
				item.quantity = -1;
			}
		}
	}

	public void AddOnBuyListeners(Action a)
	{
		m_OnBuyAction = (Action)Delegate.Combine(m_OnBuyAction, a);
	}

	public int GetCurrentCartValue()
	{
		return m_ShoppingCart.CartValue;
	}

	public int GetNumberOfItemsInCart()
	{
		return m_ShoppingCart.Cart.Count;
	}

	public List<ShopItem> GetItemsInCart()
	{
		return m_ShoppingCart.Cart;
	}
}
