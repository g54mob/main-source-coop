using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.UI
{
	public struct OnDisplaySettingsUpdatedCallbackInfo : ICallbackInfo
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

		public bool IsVisible { get; set; }

		public bool IsExclusiveInput { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}
	}
}
