using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.P2P
{
	public struct OnRemoteConnectionClosedInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		[CompilerGenerated]
		private ProductUserId _003CLocalUserId_003Ek__BackingField;

		public object ClientData
		{
			[CompilerGenerated]
			set
			{
				_003CClientData_003Ek__BackingField = value;
			}
		}

		public ProductUserId LocalUserId
		{
			[CompilerGenerated]
			set
			{
				_003CLocalUserId_003Ek__BackingField = value;
			}
		}

		public ProductUserId RemoteUserId { get; set; }

		public SocketId? SocketId { get; set; }

		public ConnectionClosedReason Reason { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}
	}
}
