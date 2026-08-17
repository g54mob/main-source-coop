using System;

namespace EvilCore.Networking
{
	public interface IVoiceChatManager
	{
		bool IsConnected { get; }

		bool IsMuted { get; }

		event Action<bool> OnMuteStateChanged;

		void SetMuted(bool muted);

		void ToggleMute();
	}
}
