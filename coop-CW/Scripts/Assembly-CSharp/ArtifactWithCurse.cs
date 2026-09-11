using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class ArtifactWithCurse : ItemInstanceBehaviour, IArtifactContent
{
	[FormerlySerializedAs("curse")]
	[SerializeReference]
	public GameObject cursePrefab;

	public GameObject spawnedCurse;

	private StashAbleEntry stashAbleEntry;

	public bool IsHeld => isHeld;

	public bool IsActive => isHeld;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<StashAbleEntry>(out stashAbleEntry))
		{
			Debug.Log($"stashAbleEntry entry found, isStashAble: {stashAbleEntry.isStashAble}");
		}
		else
		{
			stashAbleEntry = new StashAbleEntry
			{
				isStashAble = false
			};
			data.AddDataEntry(stashAbleEntry);
			Debug.Log("stashAbleEntry entry not found, adding new entry with true.");
		}
		itemInstance.onItemEquipped.AddListener(OnItemEquipped);
	}

	private void OnItemEquipped(Player player)
	{
		Debug.Log("OnItemEquipped");
		player.refs.curses.SpawnCurse(this, cursePrefab);
	}

	private void OnDestroy()
	{
		itemInstance.onItemEquipped.RemoveListener(OnItemEquipped);
	}
}
