using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Lobby
{
	public struct LobbyInviteAcceptedCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		[CompilerGenerated]
		private Utf8String _003CInviteId_003Ek__BackingField;

		[CompilerGenerated]
		private ProductUserId _003CLocalUserId_003Ek__BackingField;

		[CompilerGenerated]
		private ProductUserId _003CTargetUserId_003Ek__BackingField;

		[CompilerGenerated]
		private Utf8String _003CLobbyId_003Ek__BackingField;

		public object ClientData
		{
			[CompilerGenerated]
			set
			{
				_003CClientData_003Ek__BackingField = value;
			}
		}

		public Utf8String InviteId
		{
			[CompilerGenerated]
			set
			{
				_003CInviteId_003Ek__BackingField = value;
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

		public ProductUserId TargetUserId
		{
			[CompilerGenerated]
			set
			{
				_003CTargetUserId_003Ek__BackingField = value;
			}
		}

		public Utf8String LobbyId
		{
			[CompilerGenerated]
			set
			{
				_003CLobbyId_003Ek__BackingField = value;
			}
		}

		public Result? GetResultCode()
		{
			return null;
		}
	}
}
