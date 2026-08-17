namespace Epic.OnlineServices.Lobby
{
	public struct CreateLobbyOptions
	{
		public ProductUserId LocalUserId { get; set; }

		public uint MaxLobbyMembers { get; set; }

		public LobbyPermissionLevel PermissionLevel { get; set; }

		public bool PresenceEnabled { get; set; }

		public bool AllowInvites { get; set; }

		public Utf8String BucketId { get; set; }

		public bool DisableHostMigration { get; }

		public bool EnableRTCRoom { get; set; }

		public LocalRTCOptions? LocalRTCOptions { get; }

		public Utf8String LobbyId { get; }

		public bool EnableJoinById { get; }

		public bool RejoinAfterKickRequiresInvite { get; }

		public uint[] AllowedPlatformIds { get; }

		public bool CrossplayOptOut { get; }

		public LobbyRTCRoomJoinActionType RTCRoomJoinActionType { get; }
	}
}
