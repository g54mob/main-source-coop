using BasicModules.DatabaseModule.Core.Scripts;
using Global.Modules.LocalizationModule.Scripts.Generated;
using Zenject;

namespace Global.Modules.Localization_Module.Scripts
{
	public class OnlineLocalizationBatchSystem : IInitializable
	{
		private readonly LocalizationBatchModel _localizationBatchModel;

		private readonly IDatabaseDataReader _databaseDataReader;

		public OnlineLocalizationBatchSystem(LocalizationBatchModel localizationBatchModel, IDatabaseDataReader databaseDataReader)
		{
			_localizationBatchModel = localizationBatchModel;
			_databaseDataReader = databaseDataReader;
		}

		public void Initialize()
		{
			_localizationBatchModel.LocalizationDataHolders = new ExternalLocalizationTablesMapper(_databaseDataReader).GetLocalizationTablesData();
		}
	}
}
