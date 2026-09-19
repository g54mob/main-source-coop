namespace Edgegap
{
	public enum ConnectionState : byte
	{
		Disconnected = 0,
		Checking = 1,
		Valid = 2,
		Invalid = 3,
		SessionTimeout = 4,
		Error = 5
	}
}
