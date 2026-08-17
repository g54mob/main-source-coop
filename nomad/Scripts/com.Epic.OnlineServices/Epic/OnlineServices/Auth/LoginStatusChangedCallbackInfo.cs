using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Auth
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

		public EpicAccountId LocalUserId { get; set; }

		public LoginStatus PrevStatus { get; set; }

		public LoginStatus CurrentStatus { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}
	}
}
