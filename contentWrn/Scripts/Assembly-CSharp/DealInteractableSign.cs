public class DealInteractableSign : Interactable
{
	private DealProposalUI proposalUI;

	protected override void Awake()
	{
		proposalUI = GetComponentInParent<DealProposalUI>();
		LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
		OnLanguageChanged();
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
	}

	private void OnLanguageChanged()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Deal_SignDeal);
	}

	public override void Interact(Player player)
	{
		proposalUI?.AcceptDeal();
	}

	public override void OnStartHover(Player player)
	{
	}

	public override void OnEndHover(Player player)
	{
	}
}
