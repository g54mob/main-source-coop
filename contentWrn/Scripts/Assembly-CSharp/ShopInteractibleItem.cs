using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopInteractibleItem : Interactable
{
	private string m_Name;

	private int m_Price;

	[SerializeField]
	private TMP_Text m_ItemNameText;

	[SerializeField]
	private TMP_Text m_PriceText;

	[SerializeField]
	private TMP_Text m_NumberInBagText;

	[SerializeField]
	private GameObject m_DiscountObject;

	[SerializeField]
	private GameObject m_NumberInBagObject;

	public Image m_icon;

	public Sprite defaultIcon;

	private int m_NumberInCart;

	private ShopItem m_Item;

	private static ShopHandler m_Handler;

	private bool m_Valid = true;

	public byte ItemID { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		LocalizationKeys.OnLanguageChanged += UpdateItemTexts;
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= UpdateItemTexts;
	}

	private void UpdateItemTexts()
	{
		m_Item.UpdateLocalizedName();
		m_Name = m_Item.DisplayName;
		m_ItemNameText.text = m_Name;
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.SoldOut);
		m_PriceText.text = (m_Valid ? (m_Price + "$") : localizedString);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.AddToCart);
		localizedString2 = localizedString2.Replace("{item}", m_Name);
		hoverText = localizedString2;
	}

	public override bool IsValid(Player player)
	{
		return m_Valid;
	}

	public override void Interact(Player player)
	{
		Debug.Log("Pressed On: " + m_Name + " In Shop");
		m_Handler.OnAddToCartItemClicked(ItemID);
	}

	public void Setup(ShopHandler handler, ShopItem item)
	{
		m_Handler = handler;
		m_Item = item;
		ItemID = item.ItemID;
		m_Price = item.Price;
		m_Name = item.DisplayName;
		m_NumberInCart = 0;
		m_Valid = item.Quantity > 0;
		if (item.UpgradeID != 0)
		{
			m_Valid = !SurfaceNetworkHandler.RoomStats.HasUpgrade(item.UpgradeID);
		}
		m_ItemNameText.text = m_Name;
		m_DiscountObject.SetActive(item.HasSale);
		m_icon.sprite = ((item.Item.icon != null) ? item.Item.icon : defaultIcon);
		UpdateItemTexts();
	}

	public void AddOneToCartVisual()
	{
		m_NumberInCart++;
		CartTextUpdated();
	}

	public void ClearCartVisual()
	{
		m_NumberInCart = 0;
		CartTextUpdated();
	}

	private void CartTextUpdated()
	{
		m_NumberInBagText.text = m_NumberInCart.ToString();
		m_NumberInBagObject.SetActive(m_NumberInCart > 0);
	}
}
