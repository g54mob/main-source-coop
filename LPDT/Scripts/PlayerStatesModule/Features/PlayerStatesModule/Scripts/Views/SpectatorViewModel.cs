namespace Features.PlayerStatesModule.Scripts.Views
{
	public class SpectatorViewModel
	{
		public bool ShowWithFade { get; set; }

		public void Clear()
		{
			ShowWithFade = false;
		}
	}
}
