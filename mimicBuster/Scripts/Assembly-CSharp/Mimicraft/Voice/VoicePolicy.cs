namespace Mimicraft.Voice
{
	public readonly struct VoicePolicy
	{
		public readonly bool Allowed;

		public readonly VoiceReach Reach;

		public static readonly VoicePolicy Silent = new VoicePolicy(allowed: false, VoiceReach.Lobby);

		public static readonly VoicePolicy Lobby = new VoicePolicy(allowed: true, VoiceReach.Lobby);

		public static readonly VoicePolicy Nearby = new VoicePolicy(allowed: true, VoiceReach.Proximity);

		public VoicePolicy(bool allowed, VoiceReach reach)
		{
			Allowed = allowed;
			Reach = reach;
		}
	}
}
