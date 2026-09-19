using System;
using UnityEngine;

namespace BasicModules.CsvConverterModule.Core
{
	[Serializable]
	public class CsvConfiguration
	{
		[field: SerializeField]
		public string Delimiter { get; private set; }

		[field: SerializeField]
		[field: TextArea]
		public string TablesExportingPath { get; private set; }
	}
}
