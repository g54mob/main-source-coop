using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Lobby
{
	public struct CreateLobbyCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		public Result ResultCode { get; set; }

		public object ClientData
		{
			[CompilerGenerated]
			set
			{
				_003CClientData_003Ek__BackingField = value;
			}
		}

		public Utf8String LobbyId { get; set; }

		public Result? GetResultCode()
		{
			return ResultCode;
		}
	}
}
