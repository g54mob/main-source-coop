using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Auth
{
	public struct LogoutCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		[CompilerGenerated]
		private EpicAccountId _003CLocalUserId_003Ek__BackingField;

		public Result ResultCode { get; set; }

		public object ClientData
		{
			[CompilerGenerated]
			set
			{
				_003CClientData_003Ek__BackingField = value;
			}
		}

		public EpicAccountId LocalUserId
		{
			[CompilerGenerated]
			set
			{
				_003CLocalUserId_003Ek__BackingField = value;
			}
		}

		public Result? GetResultCode()
		{
			return ResultCode;
		}
	}
}
