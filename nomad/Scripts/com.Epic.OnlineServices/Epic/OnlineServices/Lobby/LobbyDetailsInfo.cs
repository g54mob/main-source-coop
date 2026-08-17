using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Lobby
{
	public struct LobbyDetailsInfo
	{
		[CompilerGenerated]
		private ProductUserId _003CLobbyOwnerUserId_003Ek__BackingField;

		[CompilerGenerated]
		private LobbyPermissionLevel _003CPermissionLevel_003Ek__BackingField;

		[CompilerGenerated]
		private uint _003CAvailableSlots_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CAllowInvites_003Ek__BackingField;

		[CompilerGenerated]
		private Utf8String _003CBucketId_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CAllowHostMigration_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CRTCRoomEnabled_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CAllowJoinById_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CRejoinAfterKickRequiresInvite_003Ek__BackingField;

		[CompilerGenerated]
		private bool _003CPresenceEnabled_003Ek__BackingField;

		[CompilerGenerated]
		private uint[] _003CAllowedPlatformIds_003Ek__BackingField;

		public Utf8String LobbyId { get; set; }

		public ProductUserId LobbyOwnerUserId
		{
			[CompilerGenerated]
			set
			{
				_003CLobbyOwnerUserId_003Ek__BackingField = value;
			}
		}

		public LobbyPermissionLevel PermissionLevel
		{
			[CompilerGenerated]
			set
			{
				_003CPermissionLevel_003Ek__BackingField = value;
			}
		}

		public uint AvailableSlots
		{
			[CompilerGenerated]
			set
			{
				_003CAvailableSlots_003Ek__BackingField = value;
			}
		}

		public uint MaxMembers { get; set; }

		public bool AllowInvites
		{
			[CompilerGenerated]
			set
			{
				_003CAllowInvites_003Ek__BackingField = value;
			}
		}

		public Utf8String BucketId
		{
			[CompilerGenerated]
			set
			{
				_003CBucketId_003Ek__BackingField = value;
			}
		}

		public bool AllowHostMigration
		{
			[CompilerGenerated]
			set
			{
				_003CAllowHostMigration_003Ek__BackingField = value;
			}
		}

		public bool RTCRoomEnabled
		{
			[CompilerGenerated]
			set
			{
				_003CRTCRoomEnabled_003Ek__BackingField = value;
			}
		}

		public bool AllowJoinById
		{
			[CompilerGenerated]
			set
			{
				_003CAllowJoinById_003Ek__BackingField = value;
			}
		}

		public bool RejoinAfterKickRequiresInvite
		{
			[CompilerGenerated]
			set
			{
				_003CRejoinAfterKickRequiresInvite_003Ek__BackingField = value;
			}
		}

		public bool PresenceEnabled
		{
			[CompilerGenerated]
			set
			{
				_003CPresenceEnabled_003Ek__BackingField = value;
			}
		}

		public uint[] AllowedPlatformIds
		{
			[CompilerGenerated]
			set
			{
				_003CAllowedPlatformIds_003Ek__BackingField = value;
			}
		}
	}
}
