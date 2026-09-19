namespace Features.MainMenuModule.Scripts
{
	public class MainMenuViewModel
	{
		public bool IsAnyPopupOpen { get; private set; }

		public void SetMainMenuPopupOpenStatus(bool anyPopupOpen)
		{
			IsAnyPopupOpen = anyPopupOpen;
		}
	}
}
