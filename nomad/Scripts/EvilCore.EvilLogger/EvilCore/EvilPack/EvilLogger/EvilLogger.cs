using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace EvilCore.EvilPack.EvilLogger
{
	public static class EvilLogger
	{
		private static Dictionary<string, bool> _logStates;

		private const string LOG_STATES_KEY = "EvilLogger_States";

		private const string CONFIG_FILE_NAME = "EvilLoggerConfig.asset";

		private static string _configPath;

		private static EvilLoggerConfig _config;

		private static bool _isInitialized;

		private static readonly StringBuilder _sb;

		static EvilLogger()
		{
			_logStates = new Dictionary<string, bool>();
			_sb = new StringBuilder(256);
			Initialize();
		}

		private static void Initialize()
		{
			if (!_isInitialized)
			{
				_configPath = ResolveConfigPath("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilLogger\\EvilLogger.cs");
				LoadLogStates();
				LoadConfig();
				_isInitialized = true;
			}
		}

		private static string ResolveConfigPath([CallerFilePath] string sourceFilePath = "")
		{
			string text = Path.GetDirectoryName(sourceFilePath)?.Replace('\\', '/');
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			int num = text.IndexOf("Assets/", StringComparison.OrdinalIgnoreCase);
			if (num >= 0)
			{
				text = text.Substring(num);
			}
			return text + "/EvilLoggerConfig.asset";
		}

		private static void LoadConfig()
		{
		}

		public static EvilLoggerConfig GetConfig()
		{
			if (_config == null)
			{
				LoadConfig();
			}
			return _config;
		}

		private static void SaveLogStates()
		{
			string value = JsonUtility.ToJson(new LogStateWrapper(_logStates));
			PlayerPrefs.SetString("EvilLogger_States", value);
			PlayerPrefs.Save();
		}

		private static void LoadLogStates()
		{
			if (!PlayerPrefs.HasKey("EvilLogger_States"))
			{
				return;
			}
			string json = PlayerPrefs.GetString("EvilLogger_States");
			try
			{
				LogStateWrapper logStateWrapper = JsonUtility.FromJson<LogStateWrapper>(json);
				if (logStateWrapper != null)
				{
					_logStates = logStateWrapper.ToDictionary();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[EvilLogger] Error loading log states: " + ex.Message);
				_logStates = new Dictionary<string, bool>();
			}
		}

		public static bool IsLoggingEnabled(string scriptName)
		{
			if (!_logStates.ContainsKey(scriptName))
			{
				_logStates[scriptName] = true;
				SaveLogStates();
			}
			return _logStates[scriptName];
		}

		public static void SetLoggingEnabled(string scriptName, bool enabled)
		{
			_logStates[scriptName] = enabled;
			SaveLogStates();
		}

		public static LogLevel GetLogLevel(string scriptName)
		{
			if (_config != null)
			{
				return _config.GetLogLevelForScript(scriptName);
			}
			return LogLevel.Info;
		}

		public static void SetLogLevel(string scriptName, LogLevel level)
		{
			if (!(_config == null))
			{
				ScriptLogConfig scriptLogConfig = _config.ScriptConfigs.Find((ScriptLogConfig c) => c.ScriptName == scriptName);
				if (scriptLogConfig != null)
				{
					scriptLogConfig.OverrideGlobalLevel = true;
					scriptLogConfig.LogLevel = level;
					return;
				}
				_config.ScriptConfigs.Add(new ScriptLogConfig
				{
					ScriptName = scriptName,
					Enabled = true,
					OverrideGlobalLevel = true,
					LogLevel = level
				});
			}
		}

		[HideInCallstack]
		private static bool ShouldLog(string scriptName, LogLevel level, LogCategory category)
		{
			if (!IsLoggingEnabled(scriptName))
			{
				return false;
			}
			LogLevel logLevel = GetLogLevel(scriptName);
			if (level > logLevel)
			{
				return false;
			}
			if (_config != null && !_config.IsCategoryEnabled(category))
			{
				return false;
			}
			return true;
		}

		[HideInCallstack]
		private static string FormatMessage(string scriptName, string memberName, int lineNumber, object message, LogCategory category, string sourcePath = null)
		{
			_sb.Clear();
			if (_config != null && _config.ShowTimestamp)
			{
				_sb.Append('[');
				_sb.Append(DateTime.Now.ToString(_config.TimestampFormat));
				_sb.Append("] ");
			}
			if (category != LogCategory.General && category != LogCategory.None)
			{
				_sb.Append("<color=#888888>[");
				_sb.Append(category);
				_sb.Append("]</color> ");
			}
			_sb.Append('[');
			_sb.Append(scriptName);
			if (_config != null && _config.ShowMethodName && !string.IsNullOrEmpty(memberName))
			{
				_sb.Append('.');
				_sb.Append(memberName);
			}
			if (_config != null && _config.ShowLineNumber && lineNumber > 0)
			{
				_sb.Append(':');
				_sb.Append(lineNumber);
			}
			_sb.Append("] ");
			_sb.Append(message);
			if (!string.IsNullOrEmpty(sourcePath) && lineNumber > 0)
			{
				string value = ConvertToUnityPath(sourcePath);
				if (!string.IsNullOrEmpty(value))
				{
					_sb.Append('\n');
					_sb.Append("(at ");
					_sb.Append(value);
					_sb.Append(':');
					_sb.Append(lineNumber);
					_sb.Append(')');
				}
			}
			return _sb.ToString();
		}

		private static string ConvertToUnityPath(string absolutePath)
		{
			if (string.IsNullOrEmpty(absolutePath))
			{
				return null;
			}
			string text = absolutePath.Replace('\\', '/');
			int num = text.IndexOf("Assets/", StringComparison.OrdinalIgnoreCase);
			if (num >= 0)
			{
				return text.Substring(num);
			}
			return null;
		}

		[HideInCallstack]
		private static void WriteToFile(LogLevel level, string scriptName, string memberName, string message, LogCategory category)
		{
			if (_config != null && _config.WriteToFile && EvilLoggerFileWriter.IsInitialized)
			{
				EvilLoggerFileWriter.WriteLog(level, scriptName, memberName, message, category);
				EvilLoggerFileWriter.CheckRotation(_config.MaxFileSizeMB, _config.MaxRotatedFiles, Path.Combine(Application.dataPath, "..", _config.LogFilePath));
			}
		}

		[HideInCallstack]
		private static void LogInternal(LogLevel level, string scriptName, string memberName, int lineNumber, object message, LogCategory category, string sourcePath = null)
		{
			if (ShouldLog(scriptName, level, category))
			{
				string message2 = FormatMessage(scriptName, memberName, lineNumber, message, category, sourcePath);
				switch (level)
				{
				case LogLevel.Error:
					Debug.LogError(message2);
					break;
				case LogLevel.Warning:
					Debug.LogWarning(message2);
					break;
				default:
					Debug.Log(message2);
					break;
				}
				WriteToFile(level, scriptName, memberName, message?.ToString(), category);
			}
		}

		[HideInCallstack]
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("ENABLE_EVIL_LOGS")]
		public static void Log(object message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
			LogInternal(LogLevel.Info, fileNameWithoutExtension, memberName, lineNumber, message, LogCategory.General, sourceFilePath);
		}

		[HideInCallstack]
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("ENABLE_EVIL_LOGS")]
		public static void Log(object message, LogCategory category, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
			LogInternal(LogLevel.Info, fileNameWithoutExtension, memberName, lineNumber, message, category, sourceFilePath);
		}

		[HideInCallstack]
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("ENABLE_EVIL_LOGS")]
		public static void LogVerbose(object message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
			LogInternal(LogLevel.Verbose, fileNameWithoutExtension, memberName, lineNumber, message, LogCategory.General, sourceFilePath);
		}

		[HideInCallstack]
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("ENABLE_EVIL_LOGS")]
		public static void LogWarning(object message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
			LogInternal(LogLevel.Warning, fileNameWithoutExtension, memberName, lineNumber, message, LogCategory.General, sourceFilePath);
		}

		[HideInCallstack]
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("ENABLE_EVIL_LOGS")]
		public static void LogWarning(object message, LogCategory category, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
			LogInternal(LogLevel.Warning, fileNameWithoutExtension, memberName, lineNumber, message, category, sourceFilePath);
		}

		[HideInCallstack]
		public static void LogError(object message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
			LogInternal(LogLevel.Error, fileNameWithoutExtension, memberName, lineNumber, message, LogCategory.General, sourceFilePath);
		}

		[HideInCallstack]
		public static void LogError(object message, LogCategory category, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
			LogInternal(LogLevel.Error, fileNameWithoutExtension, memberName, lineNumber, message, category, sourceFilePath);
		}

		[HideInCallstack]
		public static void LogException(Exception exception, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
			if (ShouldLog(fileNameWithoutExtension, LogLevel.Error, LogCategory.General))
			{
				string message = "Exception: " + exception.Message + "\n" + exception.StackTrace;
				Debug.LogError(FormatMessage(fileNameWithoutExtension, memberName, lineNumber, message, LogCategory.General, sourceFilePath));
				WriteToFile(LogLevel.Error, fileNameWithoutExtension, memberName, message, LogCategory.General);
			}
		}

		public static void ClearLogStates()
		{
			_logStates.Clear();
			PlayerPrefs.DeleteKey("EvilLogger_States");
			PlayerPrefs.Save();
		}

		public static void EnableAll()
		{
			foreach (string item in new List<string>(_logStates.Keys))
			{
				_logStates[item] = true;
			}
			SaveLogStates();
		}

		public static void DisableAll()
		{
			foreach (string item in new List<string>(_logStates.Keys))
			{
				_logStates[item] = false;
			}
			SaveLogStates();
		}

		public static void FlushFileBuffer()
		{
			EvilLoggerFileWriter.Flush();
		}
	}
}
