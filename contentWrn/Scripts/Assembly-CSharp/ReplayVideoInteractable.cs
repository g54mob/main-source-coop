public class ReplayVideoInteractable : Interactable
{
	public UploadCompleteUI m_uploadCompleteUI;

	private void Start()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.ReplayVideo);
	}

	public override void Interact(Player player)
	{
		m_uploadCompleteUI.Replay();
	}
}
