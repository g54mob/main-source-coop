namespace Mimicraft.Networking
{
	public static class DisconnectNotice
	{
		public const string HostClosedReason = "HostClosed";

		public static string PendingKey { get; private set; }

		public static void Post(string key)
		{
			PendingKey = key;
		}

		public static string Take()
		{
			string pendingKey = PendingKey;
			PendingKey = null;
			return pendingKey;
		}
	}
}
