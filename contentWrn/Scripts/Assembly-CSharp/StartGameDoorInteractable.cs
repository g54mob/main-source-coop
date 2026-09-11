using Photon.Pun;

public class StartGameDoorInteractable : Interactable
{
	private SurfaceNetworkHandler m_NetworkHandler;

	private string m_StartGameTitleText;

	private string m_StartGameBody;

	private string m_StartGameConfirmText;

	private string m_CancelText;

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

	private void Start()
	{
		m_NetworkHandler = SurfaceNetworkHandler.Instance;
	}

	private void OnLanguageChanged()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGame);
		m_StartGameTitleText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGameTitle);
		m_StartGameBody = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGameBody);
		m_StartGameConfirmText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGameConfirm);
		m_CancelText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel);
	}

	public override void Interact(Player player)
	{
		if (PhotonNetwork.PlayerList.Length == 4)
		{
			m_NetworkHandler.RequestStartGame();
			return;
		}
		Modal.Show(m_StartGameTitleText, m_StartGameBody, new ModalOption[2]
		{
			new ModalOption(m_StartGameConfirmText, delegate
			{
				m_NetworkHandler.RequestStartGame();
			}),
			new ModalOption(m_CancelText)
		});
	}
}
