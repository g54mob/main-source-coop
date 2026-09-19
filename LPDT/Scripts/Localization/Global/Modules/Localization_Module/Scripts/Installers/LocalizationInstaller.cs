using BasicModules.CsvConverterModule.Core;
using BasicModules.DatabaseModule.Core.Scripts;
using Global.Modules.Database_Module.Scripts.Configurations;
using Global.Modules.LocalizationModule.Scripts.Generated;
using RSG.Muffin.AssetLoaderModule.Core;
using UnityEngine;
using Zenject;

namespace Global.Modules.Localization_Module.Scripts.Installers
{
	public class LocalizationInstaller : Installer<LocalizationInstaller>
	{
		private const string DATABASE_PICKING_CONFIGURATION = "Configurations/LocalizationDatabasePickingConfiguration_Default";

		private const string LOCALIZATION_CONFIGURATION_SWITCHER = "Configurations/LocalizationConfigurationSwitcher";

		public override void InstallBindings()
		{
			base.Container.Bind<LocalizationBatchModel>().AsSingle();
			base.Container.BindInterfacesTo<LanguageService>().AsSingle();
			base.Container.Bind<LanguageMapper>().AsSingle();
			IAssetLoaderFacadeService assetLoaderFacadeService = base.Container.Resolve<IAssetLoaderFacadeService>();
			DatabasePickingConfiguration databasePickingConfiguration = assetLoaderFacadeService.LoadAsset<DatabasePickingConfiguration>("Configurations/LocalizationDatabasePickingConfiguration_Default", AssetLoadSource.Resources);
			DatabaseConfigurationSwitcher databaseConfigurationSwitcher = assetLoaderFacadeService.LoadAsset<DatabaseConfigurationSwitcher>("Configurations/LocalizationConfigurationSwitcher", AssetLoadSource.Resources);
			DatabasePickingDataConfiguration defaultDatabasePickingDataConfigurations = databasePickingConfiguration.DefaultDatabasePickingDataConfigurations;
			if (Application.isEditor && databaseConfigurationSwitcher.IsOnlineDatabase)
			{
				BindOnlineLocalization(databasePickingConfiguration.CsvConfiguration, defaultDatabasePickingDataConfigurations, defaultDatabasePickingDataConfigurations.FolderName);
			}
			else
			{
				BindOfflineLocalization(databasePickingConfiguration.CsvConfiguration, defaultDatabasePickingDataConfigurations.FolderName);
			}
		}

		private void BindOnlineLocalization(CsvConfiguration csvConfiguration, DatabasePickingDataConfiguration databasePickingDataConfiguration, string folderToPickFrom)
		{
			base.Container.Bind<ILocalizationService>().To<OnlineLocalizationService>().AsSingle();
			ICsvDatabaseConverter csvDatabaseConverter = CreateCsvDatabaseConverter(csvConfiguration, folderToPickFrom);
			IDatabaseDataReader param = base.Container.Resolve<IDatabaseDataManipulatorsFactory>().CreateGoogleSheetsDatabaseDataReader(csvDatabaseConverter, databasePickingDataConfiguration);
			base.Container.BindInterfacesTo<OnlineLocalizationBatchSystem>().AsSingle().WithArguments(param);
		}

		private void BindOfflineLocalization(CsvConfiguration csvConfiguration, string folderToPickFrom)
		{
			base.Container.Bind<ILocalizationService>().To<OfflineLocalizationService>().AsSingle();
			ICsvDatabaseConverter param = CreateCsvDatabaseConverter(csvConfiguration, folderToPickFrom);
			base.Container.BindInterfacesTo<OfflineLocalizationBatchSystem>().AsSingle().WithArguments(param);
		}

		private ICsvDatabaseConverter CreateCsvDatabaseConverter(CsvConfiguration csvConfiguration, string folderToPickFrom)
		{
			return base.Container.Resolve<ICsvDatabaseConverterFactory>().CreateCsvDatabaseConverter(csvConfiguration, folderToPickFrom);
		}
	}
}
