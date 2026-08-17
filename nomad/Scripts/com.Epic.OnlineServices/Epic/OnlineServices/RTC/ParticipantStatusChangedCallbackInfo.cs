using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.RTC
{
	public struct ParticipantStatusChangedCallbackInfo : ICallbackInfo
	{
		[CompilerGenerated]
		private object _003CClientData_003Ek__BackingField;

		[CompilerGenerated]
		private ProductUserId _003CLocalUserId_003Ek__BackingField;

		[CompilerGenerated]
		private Utf8String _003CRoomName_003Ek__BackingField;

		[CompilerGenerated]
		private ParticipantMetadata[] _003CParticipantMetadata_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CParticipantInBlocklist_003Ek__BackingField;

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

		public ProductUserId ParticipantId { get; set; }

		public RTCParticipantStatus ParticipantStatus { get; set; }

		public ParticipantMetadata[] ParticipantMetadata
		{
			[CompilerGenerated]
			set
			{
				_003CParticipantMetadata_003Ek__BackingField = value;
			}
		}

		public bool ParticipantInBlocklist
		{
			[CompilerGenerated]
			set
			{
				_003CParticipantInBlocklist_003Ek__BackingField = value;
			}
		}

		public Result? GetResultCode()
		{
			return null;
		}
	}
}
