using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.RTCAudio
{
	public struct AudioDevicesChangedCallbackInfo : ICallbackInfo
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

		public Result? GetResultCode()
		{
			return null;
		}
	}
}
