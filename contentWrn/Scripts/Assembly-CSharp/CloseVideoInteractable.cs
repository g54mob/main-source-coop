public class CloseVideoInteractable : Interactable
{
	public UploadVideoStation UploadVideoStation;

	private void Start()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CloseVideo);
	}

	public override void Interact(Player player)
	{
		UploadVideoStation.CloseVideo();
	}
}
