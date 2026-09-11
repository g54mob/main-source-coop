using ExitGames.Client.Photon;
using UnityEngine;

namespace Photon.Voice.Unity
{
	public class VoiceComponentImpl
	{
		private class LoggerImpl : ILogger
		{
			private VoiceLogger voiceLogger;

			private Object obj;

			private string objName;

			private string tag = "INIT";

			public void SetVoiceLogger(VoiceLogger voiceLogger, Object obj, string tag)
			{
				this.voiceLogger = voiceLogger;
				this.obj = obj;
				this.tag = tag;
			}

			public void SetObjName(string n)
			{
				objName = n;
			}

			private void Log(DebugLevel level, string fmt, params object[] args)
			{
				if (voiceLogger != null)
				{
					if ((int)voiceLogger.LogLevel >= (int)level)
					{
						UnityLogger.Log(level, obj, tag, objName, fmt, args);
					}
				}
				else
				{
					UnityLogger.Log(level, obj, tag, objName, fmt, args);
				}
			}

			public void LogError(string fmt, params object[] args)
			{
				Log(DebugLevel.ERROR, fmt, args);
			}

			public void LogWarning(string fmt, params object[] args)
			{
				Log(DebugLevel.WARNING, fmt, args);
			}

			public void LogInfo(string fmt, params object[] args)
			{
				Log(DebugLevel.INFO, fmt, args);
			}

			public void LogDebug(string fmt, params object[] args)
			{
				Log(DebugLevel.ALL, fmt, args);
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
				logger.LogWarning("VoiceLogger object is not found in the scene. Creating one.");
				voiceLogger = VoiceLogger.CreateRootLogger();
			}
			logger.SetVoiceLogger(voiceLogger, mb, mb.GetType().Name);
			logger.SetObjName(mb.name);
		}
	}
}
