namespace VisualDesignCafe.Nature
{
	internal interface ILog
	{
		long ElapsedMilliseconds { get; }

		void StartTimer();

		long StopTimer();

		void Indent();

		void Unindent();

		void Log(string message);

		void LogFormat(string message, params object[] args);

		void LogWarning(string message);

		void LogWarningFormat(string message, params object[] args);

		void LogError(string message);

		void LogErrorFormat(string message, params object[] args);
	}
}
