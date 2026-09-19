namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ISessionRecoverySource
	{
		bool HasError { get; }

		bool TryGetReconnectSession(out string sessionName);
	}
}
