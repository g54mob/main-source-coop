using BasicModules.DatabaseModule.Core.Scripts;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Zenject;

namespace Global.Modules.Localization_Module.Scripts
{
	public class OfflineLocalizationBatchSystem : IInitializable
	{
		private readonly LocalizationBatchModel _localizationBatchModel;

		private readonly ICsvDatabaseConverter _csvDatabaseConverter;

		public OfflineLocalizationBatchSystem(LocalizationBatchModel localizationBatchModel, ICsvDatabaseConverter csvDatabaseConverter)
		{
			_localizationBatchModel = localizationBatchModel;
			_csvDatabaseConverter = csvDatabaseConverter;
		}

		public void Initialize()
		{
			_localizationBatchModel.LocalizationDataHolders = new InternalLocalizationTablesMapper(_csvDatabaseConverter).GetLocalizationTablesData();
		}
	}
}
