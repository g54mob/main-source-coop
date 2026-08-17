using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Lobby
{
	public struct LobbyUpdateReceivedCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

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
