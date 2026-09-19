namespace Photon.Chat
{
	public enum ChatDisconnectCause
	{
		None = 0,
		ExceptionOnConnect = 1,
		DnsExceptionOnConnect = 2,
		ServerAddressInvalid = 3,
		Exception = 4,
		ServerTimeout = 5,
		ClientTimeout = 6,
		DisconnectByServerLogic = 7,
		DisconnectByServerReasonUnknown = 8,
		InvalidAuthentication = 9,
		CustomAuthenticationFailed = 10,
		AuthenticationTicketExpired = 11,
		MaxCcuReached = 12,
		InvalidRegion = 13,
		OperationNotAllowedInCurrentState = 14,
		DisconnectByClientLogic = 15,
		DisconnectByOperationLimit = 16,
		DisconnectByDisconnectMessage = 17
	}
}
