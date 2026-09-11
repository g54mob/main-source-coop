using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;

public class ItemLostDisc : ItemInstanceBehaviour
{
	public float timeToTeleport = 30f;

	private ExtractVideoMachine machine;

	private float timeSinceHeld;

	public IntEntry lostFootageIndex;

	public BoolEntry boolEntry;

	private void Update()
	{
		if (PhotonNetwork.IsMasterClient && !isHeld)
		{
			timeSinceHeld += Time.deltaTime;
		}
	}

	public bool GetFootage(out LostFootageDatabase.FootageRarityPair footage)
	{
		footage = null;
		if (lostFootageIndex != null && lostFootageIndex.i >= 1 && SingletonAsset<LostFootageDatabase>.Instance.GetFootageByIndex(lostFootageIndex.i, out footage))
		{
			return true;
		}
		Debug.LogError("Failed to get footage by index: " + lostFootageIndex.i);
		return false;
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		machine = Object.FindObjectOfType<ExtractVideoMachine>();
		if (data.TryGetEntry<IntEntry>(out lostFootageIndex))
		{
			Debug.Log($"intEntry found, state: {lostFootageIndex.i}");
			return;
		}
		int randomLostFootageIndex = SingletonAsset<LostFootageDatabase>.Instance.GetRandomLostFootageIndex();
		lostFootageIndex = new IntEntry
		{
			i = randomLostFootageIndex
		};
		data.AddDataEntry(lostFootageIndex);
		lostFootageIndex.SetDirty();
		Debug.Log("Setting random index: " + randomLostFootageIndex);
	}
}
