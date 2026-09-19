namespace Google.Apis.Requests
{
	public class SingleError
	{
		public string Domain { get; set; }

		public string Reason { get; set; }

		public string Message { get; set; }

		public string LocationType { get; set; }

		public string Location { get; set; }

		public override string ToString()
		{
			return $"Message[{Message}] Location[{Location} - {LocationType}] Reason[{Reason}] Domain[{Domain}]";
		}
	}
}
