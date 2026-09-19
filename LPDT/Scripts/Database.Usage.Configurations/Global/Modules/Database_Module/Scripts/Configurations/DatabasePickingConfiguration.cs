using BasicModules.CsvConverterModule.Core;
using Global.SerializableDictionary;
using UnityEngine;

namespace Global.Modules.Database_Module.Scripts.Configurations
{
	[CreateAssetMenu(fileName = "DatabasePickingConfiguration_Default", menuName = "Configurations/DatabaseModule/DatabasePickingConfiguration")]
	public class DatabasePickingConfiguration : ScriptableObject
	{
		[field: SerializeField]
		[field: TextArea]
		public string ScriptsGenerationPath { get; private set; }

		[field: SerializeField]
		public CsvConfiguration CsvConfiguration { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<string, DatabasePickingDataConfiguration> DatabasePickingDataConfigurations { get; private set; }

		[field: SerializeField]
		public DatabasePickingDataConfiguration DefaultDatabasePickingDataConfigurations { get; private set; }
	}
}
