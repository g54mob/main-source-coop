using System.Collections.Generic;
using Photon.Client;

namespace Photon.Chat
{
	public interface IChatClientListener
	{
		void DebugReturn(LogLevel level, string message);

		void OnDisconnected();

		void OnConnected();

		void OnCustomAuthenticationResponse(Dictionary<string, object> data);

		void OnCustomAuthenticationFailed(string debugMessage);

		void OnChatStateChange(ChatState state);

		void OnGetMessages(string channelName, string[] senders, object[] messages);

		void OnPrivateMessage(string sender, object message, string channelName);

		void OnSubscribed(string[] channels, bool[] results);

		void OnUnsubscribed(string[] channels);

		void OnStatusUpdate(string user, int status, bool gotMessage, object message);

		void OnUserSubscribed(string channel, string user);

		void OnUserUnsubscribed(string channel, string user);
	}
}
