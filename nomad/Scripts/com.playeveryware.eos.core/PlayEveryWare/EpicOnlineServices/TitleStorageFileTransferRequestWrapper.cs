using Epic.OnlineServices;
using Epic.OnlineServices.TitleStorage;

namespace PlayEveryWare.EpicOnlineServices
{
	public class TitleStorageFileTransferRequestWrapper : FileRequestTransferWrapper<TitleStorageFileTransferRequest>
	{
		public TitleStorageFileTransferRequestWrapper(TitleStorageFileTransferRequest instance)
			: base(instance)
		{
		}

		public static implicit operator TitleStorageFileTransferRequestWrapper(TitleStorageFileTransferRequest instance)
		{
			return new TitleStorageFileTransferRequestWrapper(instance);
		}

		public override Result CancelRequest()
		{
			if (!(null == _instance))
			{
				return _instance.CancelRequest();
			}
			return Result.Success;
		}

		public override void Release()
		{
			_instance?.Release();
			_instance = null;
		}
	}
}
