namespace Epic.OnlineServices.PlayerDataStorage
{
	public sealed class PlayerDataStorageFileTransferRequest : Handle
	{
		public Result CancelRequest()
		{
			return Bindings.EOS_PlayerDataStorageFileTransferRequest_CancelRequest(base.InnerHandle);
		}

		public void Release()
		{
			Bindings.EOS_PlayerDataStorageFileTransferRequest_Release(base.InnerHandle);
		}
	}
}
