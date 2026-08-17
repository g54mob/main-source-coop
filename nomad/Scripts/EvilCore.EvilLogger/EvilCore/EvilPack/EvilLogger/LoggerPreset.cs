using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilPack.EvilLogger
{
	[Serializable]
	public class LoggerPreset
	{
		[Tooltip("Preset name")]
		public string Name;

		[Tooltip("Global log level")]
		public LogLevel Level;

		[Tooltip("Enabled categories")]
		public LogCategory Categories;

		[Tooltip("Show timestamps")]
		public bool ShowTimestamp;

		[Tooltip("Show method name in prefix")]
		public bool ShowMethodName;

		[Tooltip("Show line number in prefix")]
		public bool ShowLineNumber;

		[Tooltip("Write to file")]
		public bool WriteToFile;

		[Tooltip("List of disabled script names")]
		public List<string> DisabledScripts = new List<string>();
	}
}
