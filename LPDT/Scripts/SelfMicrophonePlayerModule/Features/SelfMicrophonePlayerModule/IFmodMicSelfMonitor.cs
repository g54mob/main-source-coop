namespace Features.SelfMicrophonePlayerModule
{
	public interface IFmodMicSelfMonitor
	{
		bool IsMonitoringEnabled { get; }

		void SetMonitoring(bool enabled);
	}
}
