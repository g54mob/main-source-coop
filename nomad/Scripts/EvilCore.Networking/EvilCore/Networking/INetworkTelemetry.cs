namespace EvilCore.Networking
{
	public interface INetworkTelemetry
	{
		void RecordAttempt(string op);

		void RecordSuccess(string op, int attempts);

		void RecordFailure(string op, NetworkErrorType type, string resultCode, int attempts, bool rateLimited);

		void RecordReportedError(NetworkErrorType type, string detail);
	}
}
