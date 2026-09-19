using System;

namespace Features.VoiceSpeakersModule.Scripts.Data
{
	[Serializable]
	public struct PlayerVoiceActivitySyncData
	{
		public int PlayerId;

		public bool IsSpeaking;
	}
}
