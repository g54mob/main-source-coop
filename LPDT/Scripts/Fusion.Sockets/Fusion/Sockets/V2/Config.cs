namespace Fusion.Sockets.V2
{
	internal static class Config
	{
		public const int SEND_WINDOW_SIZE = 1024;

		public const int PACKET_MTU_BYTES = 1136;

		public const int MAX_UNRELIABLE_FRAGS = 3;

		public const int DELIVER_BUFFER = 1048576;
	}
}
