using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Connect
{
	public struct LoginCallbackInfo : ICallbackInfo
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

		public ProductUserId LocalUserId { get; set; }

		public ContinuanceToken ContinuanceToken { get; set; }

		public Result? GetResultCode()
		{
			return ResultCode;
		}
	}
}
