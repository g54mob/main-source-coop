using Photon.Pun;

public class ComputerRoomDoorInteractable : Interactable
{
	private string m_StartGameTitleText;

	private string m_StartGameBody;

	private string m_StartGameConfirmText;

	private string m_CancelText;

	public override bool IsValid(Player player)
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.StartGame);
		bool flag = !SurfaceNetworkHandler.HasStarted;
		if (flag)
		{
			flag = SurfaceNetworkHandler.RoomStats.ComputerRoomDoorUnlocked;
		}
		return flag;
	}

	private void Start()
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
			SurfaceNetworkHandler.Instance.RequestStartGame();
			return;
		}
		Modal.Show(m_StartGameTitleText, m_StartGameBody, new ModalOption[2]
		{
			new ModalOption(m_StartGameConfirmText, delegate
			{
				SurfaceNetworkHandler.Instance.RequestStartGame();
			}),
			new ModalOption(m_CancelText)
		});
	}
}
