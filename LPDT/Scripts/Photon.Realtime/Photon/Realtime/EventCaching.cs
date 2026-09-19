namespace Photon.Realtime
{
	public enum EventCaching : byte
	{
		DoNotCache = 0,
		AddToRoomCache = 4,
		AddToRoomCacheGlobal = 5,
		RemoveFromRoomCache = 6,
		RemoveFromRoomCacheForActorsLeft = 7,
		SliceIncreaseIndex = 10,
		SliceSetIndex = 11,
		SlicePurgeIndex = 12,
		SlicePurgeUpToIndex = 13
	}
}
