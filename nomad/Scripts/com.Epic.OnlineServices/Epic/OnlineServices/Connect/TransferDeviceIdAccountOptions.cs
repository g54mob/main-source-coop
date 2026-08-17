namespace Epic.OnlineServices.Connect
{
	public struct TransferDeviceIdAccountOptions
	{
		public ProductUserId PrimaryLocalUserId { get; }

		public ProductUserId LocalDeviceUserId { get; }

		public ProductUserId ProductUserIdToPreserve { get; }
	}
}
