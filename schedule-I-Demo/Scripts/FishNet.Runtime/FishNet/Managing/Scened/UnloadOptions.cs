namespace FishNet.Managing.Scened
{
	public class UnloadOptions
	{
		public enum ServerUnloadMode
		{
			UnloadUnused = 0,
			KeepUnused = 1
		}

		public ServerUnloadMode Mode;

		public bool Addressables;
	}
}
