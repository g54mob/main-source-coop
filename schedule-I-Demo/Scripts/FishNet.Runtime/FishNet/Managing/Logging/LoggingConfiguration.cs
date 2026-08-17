using UnityEngine;

namespace FishNet.Managing.Logging
{
	public abstract class LoggingConfiguration : ScriptableObject
	{
		[Tooltip("True to use logging features. False to disable all logging.")]
		public bool LoggingEnabled = true;

		public virtual void InitializeOnce()
		{
		}

		public abstract bool CanLog(LoggingType loggingType);

		public abstract void Log(string value);

		public abstract void LogWarning(string value);

		public abstract void LogError(string value);

		public abstract LoggingConfiguration Clone();
	}
}
