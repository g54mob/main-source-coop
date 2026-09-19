using System;

namespace Fusion
{
	[Flags]
	public enum RpcSendMessageResult
	{
		Sent = 0,
		[Obsolete]
		SentToServerForForwarding = 0x101,
		[Obsolete]
		SentToTargetClient = 0x102,
		[Obsolete]
		SentBroadcast = 0x103,
		TargetObjectNotInPlayerInterest = 0x205,
		TargetPlayerNotAvailable = 0x206,
		NotInvokableDuringResim = 0x209,
		PayloadSizeExceeded = 0x20A,
		InsufficientSourceAuthority = 0x20B,
		TargetPlayerIsLocalPlayer = 0x20C,
		NoActiveConnections = 0x20D,
		InsufficientTargetAuthority = 0x20E,
		NoConnectionsWithAuthorityOrInterest = 0x20F,
		MaskSent = 0x100,
		MaskNotSent = 0x200,
		[Obsolete("Not used anymore")]
		MaskBroadcast = 0x400,
		[Obsolete("Not used anymore.")]
		MaskCulled = 0x800,
		[Obsolete("Not used anymore.")]
		NotSentTargetObjectNotConfirmed = 0xA04,
		[Obsolete("Use TargetObjectNotInPlayerInterest instead")]
		NotSentTargetObjectNotInPlayerInterest = 0x205,
		[Obsolete("Use TargetPlayerNotAvailable instead")]
		NotSentTargetClientNotAvailable = 0x206,
		[Obsolete("Not used anymore. Use NotSentNoActiveConnections instead.")]
		NotSentBroadcastNoActiveConnections = 0x607,
		[Obsolete("Not used anymore. Use NotSentTargetObjectNotInPlayerInterest instead.")]
		NotSentBroadcastNoConfirmedNorInterestedClients = 0xE08
	}
}
