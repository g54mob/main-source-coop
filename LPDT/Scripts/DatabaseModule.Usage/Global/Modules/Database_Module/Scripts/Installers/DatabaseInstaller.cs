using BasicModules.CsvConverterModule.Core;
using BasicModules.DatabaseModule.Core.Scripts;
using Global.Modules.DatabaseModule.Scripts.Generated;
using Global.Modules.Database_Module.Scripts.Configurations;
using RSG.Muffin.AssetLoaderModule.Core;
using UnityEngine;
using Zenject;

namespace Global.Modules.Database_Module.Scripts.Installers
{
	public class DatabaseInstaller : Installer<DatabaseInstaller>
	{
		private const string DATABASE_PICKING_CONFIGURATION = "Configurations/DatabasePickingConfiguration_Default";

		private const string DATABASE_CONFIGURATION_SWITCHER = "Configurations/DatabaseConfigurationSwitcher";

		public override void InstallBindings()
		{
			base.Container.Bind<DatabaseBatchModel>().AsSingle();
			base.Container.BindInterfacesTo<DatabaseTypeChooseService>().AsSingle();
			base.Container.BindInterfacesTo<CsvDatabaseReaderFactory>().AsSingle();
			base.Container.BindInterfacesTo<CsvDatabaseWriterFactory>().AsSingle();
			base.Container.BindInterfacesTo<CsvDatabaseConverterFactory>().AsSingle();
			base.Container.BindInterfacesTo<DatabaseDataManipulatorsFactory>().AsSingle();
			base.Container.BindInterfacesTo<DataHoldersNamer>().AsSingle();
			IAssetLoaderFacadeService assetLoaderFacadeService = base.Container.Resolve<IAssetLoaderFacadeService>();
			DatabasePickingConfiguration databasePickingConfiguration = assetLoaderFacadeService.LoadAsset<DatabasePickingConfiguration>("Configurations/DatabasePickingConfiguration_Default", AssetLoadSource.Resources);
			DatabaseConfigurationSwitcher databaseConfigurationSwitcher = assetLoaderFacadeService.LoadAsset<DatabaseConfigurationSwitcher>("Configurations/DatabaseConfigurationSwitcher", AssetLoadSource.Resources);
			string databaseType = base.Container.Resolve<IDatabaseTypeChooseService>().GetDatabaseType();
			DatabasePickingDataConfiguration databasePickingDataConfiguration = (databasePickingConfiguration.DatabasePickingDataConfigurations.ContainsKey(databaseType) ? databasePickingConfiguration.DatabasePickingDataConfigurations[databaseType] : databasePickingConfiguration.DefaultDatabasePickingDataConfigurations);
			if (Application.isEditor && databaseConfigurationSwitcher.IsOnlineDatabase)
			{
				BindOnlineDatabase(databasePickingConfiguration.CsvConfiguration, databasePickingDataConfiguration.FolderName, databasePickingDataConfiguration);
			}
			else
			{
				BindOfflineDatabase(databasePickingConfiguration.CsvConfiguration, databasePickingDataConfiguration.FolderName);
			}
		}

		private void BindOnlineDatabase(CsvConfiguration csvConfiguration, string folderToPickFrom, DatabasePickingDataConfiguration databasePickingDataConfiguration)
		{
			ICsvDatabaseConverter csvDatabaseConverter = CreateCsvDatabaseConverter(csvConfiguration, folderToPickFrom);
			IDatabaseDataManipulatorsFactory databaseDataManipulatorsFactory = base.Container.Resolve<IDatabaseDataManipulatorsFactory>();
			IDatabaseDataReader databaseDataReader = databaseDataManipulatorsFactory.CreateGoogleSheetsDatabaseDataReader(csvDatabaseConverter, databasePickingDataConfiguration);
			IDatabaseDataWriter param = databaseDataManipulatorsFactory.CreateGoogleSheetsDatabaseDataWriter(databasePickingDataConfiguration);
			base.Container.Bind<IDatabaseService>().To<OnlineDatabaseService>().AsSingle()
				.WithArguments(databaseDataReader, param);
			base.Container.BindInterfacesTo<OnlineDatabaseBatchSystem>().AsSingle().WithArguments(databaseDataReader);
		}

		private void BindOfflineDatabase(CsvConfiguration csvConfiguration, string folderToPickFrom)
		{
			base.Container.Bind<DataHolderTypesMapper>().AsSingle();
			base.Container.Bind<IDatabaseService>().To<OfflineDatabaseService>().AsSingle();
			ICsvDatabaseConverter param = CreateCsvDatabaseConverter(csvConfiguration, folderToPickFrom);
			base.Container.BindInterfacesTo<OfflineDatabaseBatchSystem>().AsSingle().WithArguments(param);
		}

		private ICsvDatabaseConverter CreateCsvDatabaseConverter(CsvConfiguration csvConfiguration, string folderToPickFrom)
		{
			return base.Container.Resolve<ICsvDatabaseConverterFactory>().CreateCsvDatabaseConverter(csvConfiguration, folderToPickFrom);
		}
	}
}
