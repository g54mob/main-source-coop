using FishNet.Documenting;

namespace FishNet.Object.Synchronizing
{
	[APIExclude]
	public enum SyncDictionaryOperation : byte
	{
		Add = 0,
		Clear = 1,
		Remove = 2,
		Set = 3,
		Complete = 4
	}
}
