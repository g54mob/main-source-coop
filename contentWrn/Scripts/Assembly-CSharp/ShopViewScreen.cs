using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopViewScreen : MonoBehaviour, ITimeOfDayListener
{
	[SerializeField]
	private GameObject m_ItemCell;

	[SerializeField]
	private GameObject m_CategoryCell;

	[SerializeField]
	private Transform m_CategoriesGrid;

	[SerializeField]
	private Transform m_ItemsGrid;

	[SerializeField]
	private TextMeshProUGUI m_CurrentCategoryNameText;

	[SerializeField]
	private TextMeshProUGUI m_CartValueText;

	[SerializeField]
	private TextMeshProUGUI m_CartNumberOfItemsText;

	private Dictionary<ShopItem, ShopInteractibleItem> m_ShopItemToInteractable;

	private ShopHandler m_ShopHandler;

	private ShopItemCategory m_CurrentCategoryScreenIndex;

	public List<ShopItemCategory> excludeCategories = new List<ShopItemCategory> { ShopItemCategory.Upgrades };

	private int m_ItemsInShop;

	private string m_ItemsLocalized;

	private bool m_Inited;

	public ShopItem CurrentSelectedShopItem { get; private set; }

	public ShopItem[] CurrentShopItems { get; private set; }

	public ShopItemCategory CurrentCategoryIndex => m_CurrentCategoryScreenIndex;

	private void Awake()
	{
		TimeOfDayToggler.AddListener(this);
	}

	public void Init(ShopHandler handler)
	{
		m_ShopHandler = handler;
		m_ItemsInShop = m_ShopHandler.NumberOfItemsInShop;
		m_ItemsLocalized = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Items);
		m_Inited = true;
		DrawCategories();
		InitListeners();
		UpdateViewScreen();
		OnUpdate();
	}

	private void CloseShop()
	{
		m_CartValueText.text = string.Empty;
		m_CartNumberOfItemsText.text = string.Empty;
		m_CurrentCategoryNameText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.ShopClosed);
		m_CategoriesGrid.gameObject.SetActive(value: false);
		m_ItemsGrid.gameObject.SetActive(value: false);
	}

	private void DrawItems()
	{
		DestroyItemGrid();
		m_ShopItemToInteractable = new Dictionary<ShopItem, ShopInteractibleItem>();
		ShopItem[] currentShopItems = CurrentShopItems;
		foreach (ShopItem item in currentShopItems)
		{
			SpawnItemCell(item);
		}
		UpdateShopInteractablesInCart();
	}

	private void DrawCategories()
	{
		ResetScreenIndex();
		DestroyCategoryGrid();
		ShopItemCategory[] array = (ShopItemCategory[])Enum.GetValues(typeof(ShopItemCategory));
		foreach (ShopItemCategory shopItemCategory in array)
		{
			if (shopItemCategory != ShopItemCategory.Invalid && !excludeCategories.Contains(shopItemCategory))
			{
				SpawnCategoryCell(shopItemCategory);
			}
		}
	}

	private void SpawnCategoryCell(ShopItemCategory category)
	{
		GameObject obj = UnityEngine.Object.Instantiate(m_CategoryCell, m_CategoriesGrid, worldPositionStays: true);
		obj.GetComponent<ShopInteractibleCategory>().Setup(m_ShopHandler, category);
		obj.SetActive(value: true);
	}

	private void SpawnItemCell(ShopItem item)
	{
		GameObject obj = UnityEngine.Object.Instantiate(m_ItemCell, m_ItemsGrid, worldPositionStays: true);
		ShopInteractibleItem component = obj.GetComponent<ShopInteractibleItem>();
		component.Setup(m_ShopHandler, item);
		if (!m_ShopItemToInteractable.ContainsKey(item))
		{
			m_ShopItemToInteractable.Add(item, component);
		}
		obj.SetActive(value: true);
	}

	private void DestroyItemGrid()
	{
		for (int num = m_ItemsGrid.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(m_ItemsGrid.GetChild(num).gameObject);
		}
		m_ShopItemToInteractable = new Dictionary<ShopItem, ShopInteractibleItem>();
	}

	private void DestroyCategoryGrid()
	{
		for (int num = m_CategoriesGrid.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(m_CategoriesGrid.GetChild(num).gameObject);
		}
	}

	public void SetIndex(ShopItemCategory newIndex)
	{
		m_CurrentCategoryScreenIndex = newIndex;
		UpdateViewScreen();
	}

	public void UpdateView()
	{
		VerboseDebug.Log("Force Update View");
		UpdateViewScreen();
	}

	private void UpdateViewScreen()
	{
		if (m_Inited)
		{
			VerboseDebug.Log("UpdateViewScreen");
			string localizedString = LocalizationKeys.GetLocalizedString((LocalizationKeys.Keys)Enum.Parse(typeof(LocalizationKeys.Keys), m_CurrentCategoryScreenIndex.ToString()));
			m_CurrentCategoryNameText.text = localizedString.ToUpper();
			ShopItem[] items = null;
			if (m_ShopHandler.TryGetShopCategoryItems(m_CurrentCategoryScreenIndex, ref items))
			{
				VerboseDebug.Log("UpdateViewScreen FOUND: Category" + m_CurrentCategoryScreenIndex);
				CurrentShopItems = items;
				DrawItems();
			}
			else
			{
				DestroyItemGrid();
				Debug.LogError("Cant find Items In Category: " + m_CurrentCategoryScreenIndex);
			}
		}
	}

	public void ChangeCategory(ShopItemCategory argument)
	{
		m_CurrentCategoryScreenIndex = argument;
		UpdateViewScreen();
	}

	private void ResetScreenIndex()
	{
		m_CurrentCategoryScreenIndex = ShopItemCategory.Lights;
	}

	private void InitListeners()
	{
		m_ShopHandler.AddOnBuyListeners(OnUpdate);
		SurfaceNetworkHandler.RoomStats.AddOnUpdateAction(OnUpdate);
	}

	private void OnUpdate()
	{
		RoomStatsHolder roomStats = SurfaceNetworkHandler.RoomStats;
		if (roomStats != null)
		{
			VerboseDebug.Log("ShopView Screen Update Money " + roomStats.Money);
			OnCartUpdated();
		}
	}

	public void OnCartUpdated()
	{
		VerboseDebug.Log("Shop View, Cart Updated!");
		UpdateCartTexts();
		UpdateShopInteractablesInCart();
	}

	private void UpdateCartTexts()
	{
		m_CartValueText.text = m_ShopHandler.GetCurrentCartValue() + " $";
		m_CartNumberOfItemsText.text = m_ShopHandler.GetNumberOfItemsInCart() + " " + m_ItemsLocalized;
	}

	private void UpdateShopInteractablesInCart()
	{
		foreach (ShopInteractibleItem value in m_ShopItemToInteractable.Values)
		{
			value.ClearCartVisual();
		}
		foreach (ShopItem item in m_ShopHandler.GetItemsInCart())
		{
			if (m_ShopItemToInteractable.ContainsKey(item))
			{
				m_ShopItemToInteractable[item].AddOneToCartVisual();
			}
		}
	}

	private void OnDestroy()
	{
		m_ShopItemToInteractable = new Dictionary<ShopItem, ShopInteractibleItem>();
	}

	public void DayTimeChanged(TimeOfDay timeOfDay)
	{
		if (timeOfDay == TimeOfDay.Evening)
		{
			CloseShop();
		}
		else
		{
			OpenShop();
		}
	}

	private void OpenShop()
	{
		m_CategoriesGrid.gameObject.SetActive(value: true);
		m_ItemsGrid.gameObject.SetActive(value: true);
		UpdateViewScreen();
	}
}
