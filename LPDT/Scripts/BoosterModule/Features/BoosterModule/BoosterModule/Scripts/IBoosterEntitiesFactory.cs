using Features.BoosterModule.BoosterModule.Scripts.Entities;

namespace Features.BoosterModule.BoosterModule.Scripts
{
	public interface IBoosterEntitiesFactory
	{
		BoosterEntityBase<TBoosterSettings> CreateBoosterEntity<TBoosterSettings>(TBoosterSettings boosterSettings) where TBoosterSettings : BoosterSettingsBase;
	}
}
