using System;

[Serializable]
public class SerializedSave
{
	public int SaveIndex;

	public string SaveID;

	public int CurrentDay;

	public int CurrentQuota;

	public int CurrentQuotaDay;

	public int Money;

	public int MatchSeed;

	public int LastPlayedLevel;

	public TimeOfDay TimeOfDay;

	public SavedInventoryItem[] InventoryItems;

	public SavedSurfaceItem[] SurfaceItems;

	public SerializedPickableNetworkDeal[] PickableNetworkDeals;
}
