using System;
using UnityEngine;

namespace Global.Modules.Database_Module.Scripts.Configurations
{
	[Serializable]
	public class DatabasePickingDataConfiguration
	{
		[field: SerializeField]
		[field: TextArea]
		public string ConnectionString { get; private set; }

		[field: SerializeField]
		[field: TextArea]
		public string JsonKeyFullPath { get; private set; }

		[field: SerializeField]
		public string FolderName { get; private set; }
	}
}
