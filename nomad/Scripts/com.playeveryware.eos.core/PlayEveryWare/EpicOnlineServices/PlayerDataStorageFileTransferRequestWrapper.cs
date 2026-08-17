using Epic.OnlineServices;
using Epic.OnlineServices.PlayerDataStorage;

namespace PlayEveryWare.EpicOnlineServices
{
	public class PlayerDataStorageFileTransferRequestWrapper : FileRequestTransferWrapper<PlayerDataStorageFileTransferRequest>
	{
		public PlayerDataStorageFileTransferRequestWrapper(PlayerDataStorageFileTransferRequest instance)
			: base(instance)
		{
		}

		public static implicit operator PlayerDataStorageFileTransferRequestWrapper(PlayerDataStorageFileTransferRequest instance)
		{
			return new PlayerDataStorageFileTransferRequestWrapper(instance);
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
