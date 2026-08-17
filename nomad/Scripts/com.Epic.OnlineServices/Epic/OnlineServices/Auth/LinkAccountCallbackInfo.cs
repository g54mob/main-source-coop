using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Auth
{
	public struct LinkAccountCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		[CompilerGenerated]
		private PinGrantInfo? _003CPinGrantInfo_003Ek__BackingField;

		[CompilerGenerated]
		private EpicAccountId _003CSelectedAccountId_003Ek__BackingField;

		public Result ResultCode { get; set; }

		public object ClientData
		{
			[CompilerGenerated]
			set
			{
				_003CClientData_003Ek__BackingField = value;
			}
		}

		public EpicAccountId LocalUserId { get; set; }

		public PinGrantInfo? PinGrantInfo
		{
			[CompilerGenerated]
			set
			{
				_003CPinGrantInfo_003Ek__BackingField = value;
			}
		}

		public EpicAccountId SelectedAccountId
		{
			[CompilerGenerated]
			set
			{
				_003CSelectedAccountId_003Ek__BackingField = value;
			}
		}

		public Result? GetResultCode()
		{
			return ResultCode;
		}
	}
}
