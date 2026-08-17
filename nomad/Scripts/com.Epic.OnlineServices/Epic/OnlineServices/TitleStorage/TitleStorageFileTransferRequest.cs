namespace Epic.OnlineServices.TitleStorage
{
	public sealed class TitleStorageFileTransferRequest : Handle
	{
		public Result CancelRequest()
		{
			return Bindings.EOS_TitleStorageFileTransferRequest_CancelRequest(base.InnerHandle);
		}

		public void Release()
		{
			Bindings.EOS_TitleStorageFileTransferRequest_Release(base.InnerHandle);
		}
	}
}
