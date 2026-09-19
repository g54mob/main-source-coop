using Features.BoosterModule.BoosterModule.Scripts.Entities;
using Fusion;

namespace Features.BoosterModule.BoosterModule.Scripts
{
	public interface IBoostersService
	{
		BoosterEntityBase<TBoosterSettings> ActivateBooster<TBoosterSettings>(TBoosterSettings boosterSettings, PlayerRef componentStateAuthority) where TBoosterSettings : BoosterSettingsBase;

		void ReActivateBooster<TBoosterSettings>(BoosterEntityBase<TBoosterSettings> boosterEntityBase, TBoosterSettings boosterSettings, PlayerRef componentStateAuthority) where TBoosterSettings : BoosterSettingsBase;

		void DeactivateBooster<TBoosterSettings>(BoosterEntityBase<TBoosterSettings> boosterEntityBase, PlayerRef playerRef) where TBoosterSettings : BoosterSettingsBase;

		void DeactivateBooster(string identifier, PlayerRef playerRef);
	}
}
