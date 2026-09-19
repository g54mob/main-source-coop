namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public interface ILoadingScreenPresetResolver
	{
		LoadingScreenPreset GetShowPreset(LoadingScreenShowType type);

		LoadingScreenScreenPreset GetScreenPreset(LoadingScreenScreenType type);

		bool CanOverrideActiveShowType(LoadingScreenShowType incoming, LoadingScreenShowType active);
	}
}
