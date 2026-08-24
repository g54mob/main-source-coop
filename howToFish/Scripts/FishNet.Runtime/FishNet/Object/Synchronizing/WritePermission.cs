namespace FishNet.Object.Synchronizing
{
	public enum WritePermission : byte
	{
		ServerOnly = 0,
		ClientUnsynchronized = 1
	}
}
