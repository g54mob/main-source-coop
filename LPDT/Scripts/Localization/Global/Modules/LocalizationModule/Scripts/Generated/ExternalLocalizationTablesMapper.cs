using System.Collections.Generic;
using System.Linq;
using BasicModules.DatabaseModule.Core.Scripts;

namespace Global.Modules.LocalizationModule.Scripts.Generated
{
	public class ExternalLocalizationTablesMapper
	{
		private readonly IDatabaseDataReader _databaseDataReader;

		public ExternalLocalizationTablesMapper(IDatabaseDataReader databaseDataReader)
		{
			_databaseDataReader = databaseDataReader;
		}

		public List<ILocalizationDataHolder> GetLocalizationTablesData()
		{
			List<ILocalizationDataHolder> list = new List<ILocalizationDataHolder>();
			list.AddRange(_databaseDataReader.GetTableData<DefaultLocalizationDataHolder>().OfType<ILocalizationDataHolder>().ToList());
			list.AddRange(_databaseDataReader.GetTableData<UiLocalizationDataHolder>().OfType<ILocalizationDataHolder>().ToList());
			list.AddRange(_databaseDataReader.GetTableData<UserStatusesDataHolder>().OfType<ILocalizationDataHolder>().ToList());
			return list;
		}
	}
}
