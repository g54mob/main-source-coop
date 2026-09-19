using System.Collections.Generic;
using System.Linq;
using BasicModules.DatabaseModule.Core.Scripts;

namespace Global.Modules.LocalizationModule.Scripts.Generated
{
	public class InternalLocalizationTablesMapper
	{
		private readonly ICsvDatabaseConverter _csvDatabaseConverter;

		public InternalLocalizationTablesMapper(ICsvDatabaseConverter csvDatabaseConverter)
		{
			_csvDatabaseConverter = csvDatabaseConverter;
		}

		public List<ILocalizationDataHolder> GetLocalizationTablesData()
		{
			List<ILocalizationDataHolder> list = new List<ILocalizationDataHolder>();
			list.AddRange(_csvDatabaseConverter.GetTableData<DefaultLocalizationDataHolder>().OfType<ILocalizationDataHolder>().ToList());
			list.AddRange(_csvDatabaseConverter.GetTableData<UiLocalizationDataHolder>().OfType<ILocalizationDataHolder>().ToList());
			list.AddRange(_csvDatabaseConverter.GetTableData<UserStatusesDataHolder>().OfType<ILocalizationDataHolder>().ToList());
			return list;
		}
	}
}
