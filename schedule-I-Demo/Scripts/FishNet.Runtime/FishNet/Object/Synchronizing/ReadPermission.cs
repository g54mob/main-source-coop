namespace FishNet.Object.Synchronizing
{
	public enum ReadPermission : byte
	{
		Observers = 0,
		OwnerOnly = 1,
		ExcludeOwner = 2
	}
}
