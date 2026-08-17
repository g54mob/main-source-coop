using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Lobby
{
	public struct JoinLobbyAcceptedCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		[CompilerGenerated]
		private ProductUserId _003CLocalUserId_003Ek__BackingField;

		[CompilerGenerated]
		private ulong _003CUiEventId_003Ek__BackingField;

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

		public ulong UiEventId
		{
			[CompilerGenerated]
			set
			{
				_003CUiEventId_003Ek__BackingField = value;
			}
		}

		public Result? GetResultCode()
		{
			return null;
		}
	}
}
