public class CaptchaTerminal : Interactable
{
	private Bot_Weeping weeping;

	private string m_BusyText;

	private string m_HelpText;

	protected override void Awake()
	{
		base.Awake();
		weeping = base.transform.root.GetComponentInChildren<Bot_Weeping>();
	}

	private void Start()
	{
		m_BusyText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.TerminalBusy);
		m_HelpText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Help);
	}

	public override void Interact(Player player)
	{
		if (CanInteract())
		{
			weeping.PlayerInteracted(player.refs.view);
		}
	}

	public override bool IsValid(Player player)
	{
		return CanInteract();
	}

	public bool CanInteract()
	{
		if (!weeping.HasCapturedPlayer)
		{
			return false;
		}
		if (!weeping.debugCapturePlayerOverride && weeping.capturedPlayer.refs.view.IsMine)
		{
			return false;
		}
		if (weeping.HasPlayerInCaptchaGame)
		{
			return false;
		}
		if (weeping.timeSinceCapture < 5f)
		{
			return false;
		}
		return true;
	}

	private void Update()
	{
		if (!CanInteract())
		{
			hoverText = m_BusyText;
		}
		else
		{
			hoverText = m_HelpText;
		}
	}
}
