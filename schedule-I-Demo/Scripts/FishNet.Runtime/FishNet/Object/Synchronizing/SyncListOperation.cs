using FishNet.Documenting;

namespace FishNet.Object.Synchronizing
{
	[APIExclude]
	public enum SyncListOperation : byte
	{
		Add = 0,
		Insert = 1,
		Set = 2,
		RemoveAt = 3,
		Clear = 4,
		Complete = 5
	}
}
