using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Connect
{
	public struct LoginStatusChangedCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		public object ClientData
		{
			[CompilerGenerated]
			set
			{
				_003CClientData_003Ek__BackingField = value;
			}
		}

		public ProductUserId LocalUserId { get; set; }

		public LoginStatus PreviousStatus { get; set; }

		public LoginStatus CurrentStatus { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}
	}
}
