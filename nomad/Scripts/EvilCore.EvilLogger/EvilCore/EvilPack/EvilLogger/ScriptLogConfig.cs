using System;
using UnityEngine;

namespace EvilCore.EvilPack.EvilLogger
{
	[Serializable]
	public class ScriptLogConfig
	{
		[Tooltip("Name of the script (without .cs extension)")]
		public string ScriptName;

		[Tooltip("Is logging enabled for this script")]
		public bool Enabled = true;

		[Tooltip("Override the global log level for this script")]
		public bool OverrideGlobalLevel;

		[Tooltip("Log level for this script (if override is enabled)")]
		public LogLevel LogLevel = LogLevel.Info;

		[Tooltip("Number of EvilLogger calls in this script (auto-detected)")]
		[HideInInspector]
		public int LogCount;
	}
}
