public class ShopInteractibleClear : Interactable
{
	protected override void Awake()
	{
		base.Awake();
		LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
		OnLanguageChanged();
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
	}

	private void OnLanguageChanged()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Clear);
	}

	public override void Interact(Player player)
	{
		ShopHandler.Instance.OnClearCartClicked();
	}
}
