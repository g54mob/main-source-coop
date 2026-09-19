using UnityEngine;

namespace Photon.Voice.Unity
{
	public class VoiceComponentImpl
	{
		private class LoggerImpl : ILogger
		{
			private VoiceLogger voiceLogger;

			private LogLevel voiceLoggerLastLevel;

			private Object obj;

			private string objName;

			private string tag = "INIT";

			public LogLevel Level
			{
				get
				{
					if (!voiceLogger)
					{
						return voiceLoggerLastLevel;
					}
					return voiceLogger.LogLevel;
				}
			}

			public void SetVoiceLogger(VoiceLogger voiceLogger, Object obj, string tag)
			{
				this.voiceLogger = voiceLogger;
				voiceLoggerLastLevel = voiceLogger.LogLevel;
				this.obj = obj;
				this.tag = tag;
			}

			public void SetObjName(string n)
			{
				objName = n;
			}

			public void Log(LogLevel level, string fmt, params object[] args)
			{
				if (voiceLogger != null)
				{
					if (voiceLogger.LogLevel >= level)
					{
						UnityLogger.Log(level, obj, tag, objName, fmt, args);
					}
					voiceLoggerLastLevel = voiceLogger.LogLevel;
				}
				else if (voiceLoggerLastLevel >= level)
				{
					UnityLogger.Log(level, obj, tag, objName, fmt, args);
				}
			}
		}

		private VoiceLogger voiceLogger;

		private LoggerImpl logger = new LoggerImpl();

		public ILogger Logger => logger;

		public VoiceLogger VoiceLogger => voiceLogger;

		public string Name
		{
			set
			{
				logger.SetObjName(value);
			}
		}

		public void Awake(MonoBehaviour mb)
		{
			voiceLogger = VoiceLogger.FindLogger(mb.gameObject);
			if (voiceLogger == null)
			{
				logger.Log(LogLevel.Warning, "VoiceLogger object is not found in the scene. Creating one.");
				voiceLogger = VoiceLogger.CreateRootLogger();
			}
			logger.SetVoiceLogger(voiceLogger, mb, mb.GetType().Name);
			logger.SetObjName(mb.name);
		}
	}
}
