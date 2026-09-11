using Photon.Pun;
using UnityEngine;

public class BombItem : ItemInstanceBehaviour, IArtifactContent
{
	public GameObject explosion;

	public float fuze = 5f;

	private TimeEntry fuzeTimeEntry;

	private StashAbleEntry stashAbleEntry;

	public bool IsHeld => isHeld;

	public bool IsActive => true;

	private void Update()
	{
		fuzeTimeEntry.currentTime -= Time.deltaTime;
		if (!(fuzeTimeEntry.currentTime < 0f))
		{
			return;
		}
		base.gameObject.SetActive(value: false);
		Object.Instantiate(explosion, base.transform.position, base.transform.rotation);
		Player componentInParent = GetComponentInParent<Player>();
		if (componentInParent != null)
		{
			InventorySlot slot;
			if (!componentInParent.TryGetInventory(out var o))
			{
				Debug.LogError("No inventory found");
			}
			else if (!o.TryGetSlot(componentInParent.data.selectedItemSlot, out slot))
			{
				Debug.LogError($"Slot not found with index: {componentInParent.data.selectedItemSlot}");
			}
			else
			{
				slot.ClearLocal();
			}
		}
		else
		{
			MonoFunctions.instance.PhotonDestroy(base.transform.root.gameObject, 2f);
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<StashAbleEntry>(out stashAbleEntry))
		{
			VerboseDebug.Log($"stashAbleEntry entry found, isStashAble: {stashAbleEntry.isStashAble}");
		}
		else
		{
			stashAbleEntry = new StashAbleEntry
			{
				isStashAble = false
			};
			data.AddDataEntry(stashAbleEntry);
			VerboseDebug.Log("stashAbleEntry entry not found, adding new entry with false.");
		}
		fuzeTimeEntry = new TimeEntry
		{
			currentTime = fuze
		};
		data.TryGetElseAdd(ref fuzeTimeEntry);
	}
}
