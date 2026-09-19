namespace Photon.Voice
{
	public interface ILogger
	{
		LogLevel Level { get; }

		void Log(LogLevel level, string fmt, params object[] args);
	}
}
