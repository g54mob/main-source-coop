using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Connect
{
	public struct CreateDeviceIdCallbackInfo : ICallbackInfo
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

		public Result? GetResultCode()
		{
			return ResultCode;
		}
	}
}
