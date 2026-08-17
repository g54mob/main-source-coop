using FishNet.Documenting;

namespace FishNet.Object.Synchronizing
{
	[APIExclude]
	public enum SyncHashSetOperation : byte
	{
		Add = 0,
		Remove = 1,
		Clear = 2,
		Complete = 3,
		Update = 4
	}
}
