using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.RTCAudio
{
	public struct UpdateReceivingCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		[CompilerGenerated]
		private ProductUserId _003CLocalUserId_003Ek__BackingField;

		[CompilerGenerated]
		private Utf8String _003CRoomName_003Ek__BackingField;

		[CompilerGenerated]
		private ProductUserId _003CParticipantId_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CAudioEnabled_003Ek__BackingField;

		public Result ResultCode { get; set; }

		public object ClientData
		{
			[CompilerGenerated]
			set
			{
				_003CClientData_003Ek__BackingField = value;
			}
		}

		public ProductUserId LocalUserId
		{
			[CompilerGenerated]
			set
			{
				_003CLocalUserId_003Ek__BackingField = value;
			}
		}

		public Utf8String RoomName
		{
			[CompilerGenerated]
			set
			{
				_003CRoomName_003Ek__BackingField = value;
			}
		}

		public ProductUserId ParticipantId
		{
			[CompilerGenerated]
			set
			{
				_003CParticipantId_003Ek__BackingField = value;
			}
		}

		public bool AudioEnabled
		{
			[CompilerGenerated]
			set
			{
				_003CAudioEnabled_003Ek__BackingField = value;
			}
		}

		public Result? GetResultCode()
		{
			return ResultCode;
		}
	}
}
