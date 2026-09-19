using System;
using System.Collections.Generic;

namespace RSG.Muffin.SavingSubmodule.SavingSystemModule.Core.Scripts.Implementation
{
	public class SaveDataDeleter : ISaveDataDeleter
	{
		private readonly ISaveDataContainer _container;

		public SaveDataDeleter(ISaveDataContainer container)
		{
			_container = container ?? throw new ArgumentNullException("container");
		}

		public void DeleteDataWithID(string id)
		{
			_container.RemoveDataForID(id);
		}

		public void DeleteDataRangeForIDs(List<string> ids)
		{
			_container.RemoveDataRangeForIDs(ids);
		}
	}
}
