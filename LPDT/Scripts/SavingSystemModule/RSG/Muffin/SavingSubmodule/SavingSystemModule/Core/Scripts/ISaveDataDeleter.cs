using System.Collections.Generic;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts
{
	public interface ISaveDataDeleter
	{
		void DeleteDataWithID(string id);

		void DeleteDataRangeForIDs(List<string> ids);
	}
}
