using System;
using Epic.OnlineServices;

namespace PlayEveryWare.EpicOnlineServices
{
	public interface IFileTransferRequest : IDisposable
	{
		Result CancelRequest();

		void Release();
	}
}
