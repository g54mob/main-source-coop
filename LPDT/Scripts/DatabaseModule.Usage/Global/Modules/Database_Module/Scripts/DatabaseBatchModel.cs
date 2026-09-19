using System;
using System.Collections.Generic;

namespace Global.Modules.Database_Module.Scripts
{
	public class DatabaseBatchModel
	{
		public Dictionary<Type, List<object>> BatchedDatabaseDataByType = new Dictionary<Type, List<object>>();
	}
}
