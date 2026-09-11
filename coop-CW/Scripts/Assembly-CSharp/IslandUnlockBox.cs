using TMPro;

public class IslandUnlockBox : Interactable
{
	public TextMeshPro priceText;

	private IslandUnlock unlock;

	private void Start()
	{
		unlock = GetComponentInParent<IslandUnlock>();
		priceText.text = unlock.price + " MC";
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Unlock);
		hoverText = localizedString + " " + unlock.price + " MC";
	}

	public override void Interact(Player player)
	{
		IslandUnlock componentInParent = GetComponentInParent<IslandUnlock>();
		GetComponentInParent<IslandUnlocks>().Interact(componentInParent.GetID(), componentInParent);
	}
}
