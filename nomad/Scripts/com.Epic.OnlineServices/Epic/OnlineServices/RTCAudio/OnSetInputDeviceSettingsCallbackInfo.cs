using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.RTCAudio
{
	public struct OnSetInputDeviceSettingsCallbackInfo : ICallbackInfo
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

		public Utf8String RealDeviceId { get; set; }

		public Result? GetResultCode()
		{
			return ResultCode;
		}
	}
}
