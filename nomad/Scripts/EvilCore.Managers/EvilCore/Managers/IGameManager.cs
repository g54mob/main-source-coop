namespace EvilCore.Managers
{
	public interface IGameManager
	{
		void QuitGame();

		void SetFpsLimit(FpsLimit limit);

		void SetDisplayMode(ResolutionType type, ResolutionQuality quality);

		void SetResolution(ResolutionType type, int width, int height);
	}
}
