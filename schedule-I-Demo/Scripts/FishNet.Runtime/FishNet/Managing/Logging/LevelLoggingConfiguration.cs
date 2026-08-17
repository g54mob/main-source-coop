using System;
using System.Runtime.CompilerServices;
using FishNet.Documenting;
using UnityEngine;

namespace FishNet.Managing.Logging
{
	[CreateAssetMenu(fileName = "New LevelLoggingConfiguration", menuName = "FishNet/Logging/Level Logging Configuration")]
	public class LevelLoggingConfiguration : LoggingConfiguration
	{
		[Tooltip("Type of logging to use for development builds and editor.")]
		[SerializeField]
		private LoggingType _developmentLogging = LoggingType.Common;

		[Tooltip("Type of logging to use for GUI builds.")]
		[SerializeField]
		private LoggingType _guiLogging = LoggingType.Warning;

		[Tooltip("Type of logging to use for headless builds.")]
		[SerializeField]
		private LoggingType _headlessLogging = LoggingType.Error;

		private bool _initialized;

		private LoggingType _highestLoggingType;

		[APIExclude]
		public void LoggingConstructor(bool loggingEnabled, LoggingType development, LoggingType gui, LoggingType headless)
		{
			LoggingEnabled = loggingEnabled;
			_developmentLogging = development;
			_guiLogging = gui;
			_headlessLogging = headless;
		}

		public override void InitializeOnce()
		{
			byte val = 0;
			val = Math.Max(val, (byte)_guiLogging);
			_highestLoggingType = (LoggingType)val;
			_initialized = true;
		}

		public override bool CanLog(LoggingType loggingType)
		{
			if (!LoggingEnabled)
			{
				return false;
			}
			if (!_initialized)
			{
				return false;
			}
			return (int)loggingType <= (int)_highestLoggingType;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void Log(string value)
		{
			if (CanLog(LoggingType.Common))
			{
				Debug.Log(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void LogWarning(string value)
		{
			if (CanLog(LoggingType.Warning))
			{
				Debug.LogWarning(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void LogError(string value)
		{
			if (CanLog(LoggingType.Error))
			{
				Debug.LogError(value);
			}
		}

		public override LoggingConfiguration Clone()
		{
			LevelLoggingConfiguration levelLoggingConfiguration = ScriptableObject.CreateInstance<LevelLoggingConfiguration>();
			levelLoggingConfiguration.LoggingConstructor(LoggingEnabled, _developmentLogging, _guiLogging, _headlessLogging);
			return levelLoggingConfiguration;
		}
	}
}
