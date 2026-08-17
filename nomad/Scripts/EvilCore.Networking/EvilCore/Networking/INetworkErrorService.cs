using System;

namespace EvilCore.Networking
{
	public interface INetworkErrorService
	{
		event Action<NetworkErrorInfo> OnNetworkError;

		void Report(NetworkErrorInfo info);

		void Report(NetworkErrorType type, string technicalDetail = null);

		NetworkErrorInfo? ConsumePending();
	}
}
