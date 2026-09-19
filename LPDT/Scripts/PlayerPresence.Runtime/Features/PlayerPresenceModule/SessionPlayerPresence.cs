namespace Features.PlayerPresenceModule
{
	public sealed class SessionPlayerPresence
	{
		public string Id { get; }

		public SessionPlayerPresence(string id)
		{
			Id = id;
		}
	}
}
