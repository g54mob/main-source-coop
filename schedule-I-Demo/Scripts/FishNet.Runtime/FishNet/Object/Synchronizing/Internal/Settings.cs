using FishNet.Transporting;

namespace FishNet.Object.Synchronizing.Internal
{
	public class Settings
	{
		public WritePermission WritePermission;

		public ReadPermission ReadPermission;

		public float SendRate;

		public Channel Channel;

		public Settings()
		{
		}

		public Settings(WritePermission writePermission, ReadPermission readPermission, float sendRate, Channel channel)
		{
			WritePermission = writePermission;
			ReadPermission = readPermission;
			SendRate = sendRate;
			Channel = channel;
		}

		public Settings(float sendTickrate)
		{
			SendRate = sendTickrate;
		}

		public Settings(ReadPermission readPermission, float sendRate, Channel channel)
		{
			ReadPermission = readPermission;
			SendRate = sendRate;
			Channel = channel;
		}
	}
}
