namespace Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer
{
	public class PlayerIdDataHolder
	{
		public const string SyncRequestMarker = "DataStreamSyncRequest";

		public string RequestType;

		public int SendedPlayerId;

		public PlayerIdDataHolder()
		{
		}

		public PlayerIdDataHolder(int sendedPlayerId)
		{
			RequestType = "DataStreamSyncRequest";
			SendedPlayerId = sendedPlayerId;
		}
	}
}
