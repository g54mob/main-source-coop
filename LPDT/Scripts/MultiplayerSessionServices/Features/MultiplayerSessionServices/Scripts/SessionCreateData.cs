namespace Features.MultiplayerSessionServices.Scripts
{
	public class SessionCreateData
	{
		public string RoomCode { get; set; }

		public string HostName { get; set; }

		public JoinSource JoinSource { get; set; }
	}
}
