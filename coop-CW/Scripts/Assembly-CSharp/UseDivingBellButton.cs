public class UseDivingBellButton : Interactable
{
	public bool onSurface;

	public DivingBell divingBell;

	private string m_SubmergeText;

	private string m_ReturnText;

	private void Start()
	{
		m_SubmergeText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Submerge);
		m_ReturnText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.ReturnToSurface);
	}

	public override void Interact(Player player)
	{
		if (onSurface)
		{
			if (TimeOfDayHandler.TimeOfDay == TimeOfDay.Morning)
			{
				divingBell.GoUnderground();
			}
		}
		else
		{
			divingBell.GoToSurface();
		}
	}

	public override bool IsValid(Player player)
	{
		if (onSurface)
		{
			hoverText = m_SubmergeText;
		}
		else
		{
			hoverText = m_ReturnText;
		}
		return !divingBell.opened;
	}
}
