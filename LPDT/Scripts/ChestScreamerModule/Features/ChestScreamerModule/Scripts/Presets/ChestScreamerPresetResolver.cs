namespace Features.ChestScreamerModule.Scripts.Presets
{
	public class ChestScreamerPresetResolver : IChestScreamerPresetResolver
	{
		private readonly ChestScreamerConfiguration _configuration;

		public ChestScreamerPresetResolver(ChestScreamerConfiguration configuration)
		{
			_configuration = configuration;
		}

		public ChestScreamerPreset GetPreset(ChestScreamerType type)
		{
			ChestScreamerPreset[] presets = _configuration.Presets;
			foreach (ChestScreamerPreset chestScreamerPreset in presets)
			{
				if (chestScreamerPreset != null && chestScreamerPreset.Type == type)
				{
					return chestScreamerPreset;
				}
			}
			return null;
		}
	}
}
