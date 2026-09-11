using System;
using TMPro;
using UnityEngine;

public class ShopInteractibleCategory : Interactable
{
	[SerializeField]
	private TextMeshProUGUI m_CategoryText;

	private ShopItemCategory m_Category;

	private static ShopHandler m_ShopHandler;

	protected override void Awake()
	{
		base.Awake();
		LocalizationKeys.OnLanguageChanged += UpdateCategoryText;
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= UpdateCategoryText;
	}

	private void UpdateCategoryText()
	{
		string localizedString = LocalizationKeys.GetLocalizedString((LocalizationKeys.Keys)Enum.Parse(typeof(LocalizationKeys.Keys), m_Category.ToString()));
		m_CategoryText.text = localizedString;
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.SwitchCategory);
		localizedString2 = localizedString2.Replace("{category}", localizedString);
		hoverText = localizedString2;
	}

	public override void Interact(Player player)
	{
		Debug.Log(player.refs.view.Owner.NickName + " Pressed On: " + m_CategoryText.text);
		m_ShopHandler.OnChangeCategoryClicked((byte)m_Category);
	}

	public void Setup(ShopHandler handler, ShopItemCategory category)
	{
		m_ShopHandler = handler;
		m_Category = category;
		UpdateCategoryText();
	}
}
