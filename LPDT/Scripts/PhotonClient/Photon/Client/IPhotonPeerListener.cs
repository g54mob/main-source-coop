namespace Photon.Client
{
	public interface IPhotonPeerListener
	{
		void DebugReturn(LogLevel level, string message);

		void OnOperationResponse(OperationResponse operationResponse);

		void OnStatusChanged(StatusCode statusCode);

		void OnEvent(EventData eventData);

		void OnMessage(bool isRawMessage, object message);

		void OnDisconnectMessage(DisconnectMessage dm);
	}
}
