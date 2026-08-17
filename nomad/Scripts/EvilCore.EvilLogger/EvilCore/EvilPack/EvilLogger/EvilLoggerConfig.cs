using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.EvilPack.EvilLogger
{
	[CreateAssetMenu(fileName = "EvilLoggerConfig", menuName = "EvilPack/Logger Config")]
	public class EvilLoggerConfig : ScriptableObject
	{
		[Header("Global Settings")]
		[Tooltip("Show timestamp in log messages")]
		public bool ShowTimestamp;

		[Tooltip("Timestamp format (C# DateTime format)")]
		public string TimestampFormat = "HH:mm:ss";

		[Tooltip("Show method name in log prefix: [Script.Method] vs [Script]")]
		public bool ShowMethodName;

		[Tooltip("Show line number in log prefix: [Script:42] vs [Script]")]
		public bool ShowLineNumber;

		[Tooltip("Global log level - logs below this level will be ignored")]
		public LogLevel GlobalLogLevel = LogLevel.Info;

		[Tooltip("Enabled log categories (bitwise flags)")]
		public LogCategory EnabledCategories = LogCategory.All;

		[Header("File Output")]
		[Tooltip("Write logs to file")]
		public bool WriteToFile;

		[Tooltip("Log file directory (relative to project root)")]
		public string LogFilePath = "Logs/";

		[Tooltip("Maximum log file size in MB before rotation")]
		public float MaxFileSizeMB = 10f;

		[Tooltip("Maximum number of rotated log files to keep")]
		public int MaxRotatedFiles = 5;

		[Header("Script-Specific Settings")]
		[Tooltip("Override settings for specific scripts")]
		public List<ScriptLogConfig> ScriptConfigs = new List<ScriptLogConfig>();

		[Header("Presets")]
		[Tooltip("Saved configuration presets for quick switching")]
		public List<LoggerPreset> Presets = new List<LoggerPreset>();

		public LogLevel GetLogLevelForScript(string scriptName)
		{
			ScriptLogConfig scriptLogConfig = ScriptConfigs.Find((ScriptLogConfig c) => c.ScriptName == scriptName);
			if (scriptLogConfig != null && scriptLogConfig.OverrideGlobalLevel)
			{
				return scriptLogConfig.LogLevel;
			}
			return GlobalLogLevel;
		}

		public bool IsScriptEnabled(string scriptName)
		{
			return ScriptConfigs.Find((ScriptLogConfig c) => c.ScriptName == scriptName)?.Enabled ?? true;
		}

		public bool IsCategoryEnabled(LogCategory category)
		{
			return (EnabledCategories & category) != 0;
		}

		public void ApplyPreset(string presetName)
		{
			LoggerPreset loggerPreset = Presets.Find((LoggerPreset p) => p.Name == presetName);
			if (loggerPreset == null)
			{
				return;
			}
			GlobalLogLevel = loggerPreset.Level;
			EnabledCategories = loggerPreset.Categories;
			ShowTimestamp = loggerPreset.ShowTimestamp;
			ShowMethodName = loggerPreset.ShowMethodName;
			ShowLineNumber = loggerPreset.ShowLineNumber;
			WriteToFile = loggerPreset.WriteToFile;
			foreach (ScriptLogConfig scriptConfig in ScriptConfigs)
			{
				scriptConfig.Enabled = !loggerPreset.DisabledScripts.Contains(scriptConfig.ScriptName);
			}
		}

		public void SaveAsPreset(string presetName)
		{
			LoggerPreset loggerPreset = Presets.Find((LoggerPreset p) => p.Name == presetName);
			if (loggerPreset != null)
			{
				Presets.Remove(loggerPreset);
			}
			LoggerPreset loggerPreset2 = new LoggerPreset
			{
				Name = presetName,
				Level = GlobalLogLevel,
				Categories = EnabledCategories,
				ShowTimestamp = ShowTimestamp,
				ShowMethodName = ShowMethodName,
				ShowLineNumber = ShowLineNumber,
				WriteToFile = WriteToFile,
				DisabledScripts = new List<string>()
			};
			foreach (ScriptLogConfig scriptConfig in ScriptConfigs)
			{
				if (!scriptConfig.Enabled)
				{
					loggerPreset2.DisabledScripts.Add(scriptConfig.ScriptName);
				}
			}
			Presets.Add(loggerPreset2);
		}
	}
}
