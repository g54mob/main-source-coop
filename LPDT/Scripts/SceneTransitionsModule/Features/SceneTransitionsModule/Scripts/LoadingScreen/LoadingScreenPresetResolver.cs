namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenPresetResolver : ILoadingScreenPresetResolver
	{
		private readonly LoadingScreenSettings _settings;

		public LoadingScreenPresetResolver(LoadingScreenSettings settings)
		{
			_settings = settings;
		}

		public LoadingScreenPreset GetShowPreset(LoadingScreenShowType type)
		{
			LoadingScreenPreset[] presets = _settings.Presets;
			foreach (LoadingScreenPreset loadingScreenPreset in presets)
			{
				if (loadingScreenPreset != null && loadingScreenPreset.ShowType == type)
				{
					return loadingScreenPreset;
				}
			}
			return null;
		}

		public LoadingScreenScreenPreset GetScreenPreset(LoadingScreenScreenType type)
		{
			LoadingScreenScreenPreset[] screenPresets = _settings.ScreenSetup.ScreenPresets;
			foreach (LoadingScreenScreenPreset loadingScreenScreenPreset in screenPresets)
			{
				if (loadingScreenScreenPreset != null && loadingScreenScreenPreset.ScreenType == type)
				{
					return loadingScreenScreenPreset;
				}
			}
			return null;
		}

		public bool CanOverrideActiveShowType(LoadingScreenShowType incoming, LoadingScreenShowType active)
		{
			LoadingScreenPreset showPreset = GetShowPreset(incoming);
			if (showPreset == null)
			{
				return false;
			}
			LoadingScreenShowType[] overrideActiveShowTypes = showPreset.OverrideActiveShowTypes;
			if (overrideActiveShowTypes == null || overrideActiveShowTypes.Length == 0)
			{
				return false;
			}
			LoadingScreenShowType[] array = overrideActiveShowTypes;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == active)
				{
					return true;
				}
			}
			return false;
		}
	}
}
