namespace Features.ChestScreamerModule.Scripts.Presets
{
	public interface IChestScreamerPresetResolver
	{
		ChestScreamerPreset GetPreset(ChestScreamerType type);
	}
}
