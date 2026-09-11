using System;
using System.Linq;
using DefaultNamespace.Artifacts;
using Photon.Pun;
using UnityEngine;

public class ArtifactGravityFlipperOn : ItemInstanceBehaviour, ISpawnedByArtifactSpawner
{
	private StashAbleEntry stashAbleEntry;

	private float upAmount;

	public GameObject flipCurse;

	public IntRangeEntry chargesEntry;

	public Item itemGravityOff;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (!data.TryGetEntry<IntRangeEntry>(out chargesEntry))
		{
			chargesEntry = new IntRangeEntry
			{
				selectedValue = 1,
				maxValue = 1
			};
			data.AddDataEntry(chargesEntry);
		}
		if (!data.TryGetEntry<StashAbleEntry>(out stashAbleEntry))
		{
			stashAbleEntry = new StashAbleEntry
			{
				isStashAble = false
			};
			data.AddDataEntry(stashAbleEntry);
		}
	}

	private void Update()
	{
		if (isHeldByMe && isHeld)
		{
			CurseOfGravityFlip[] componentsInChildren = Player.localPlayer.refs.curses.GetComponentsInChildren<CurseOfGravityFlip>();
			if (componentsInChildren.Length == 0 || componentsInChildren.All((CurseOfGravityFlip curse) => curse.itemSource != itemInstance.item))
			{
				Player.localPlayer.refs.curses.SpawnCurse(this, flipCurse);
			}
		}
	}

	public void OnFinishSpawning()
	{
		if (PhotonNetwork.IsMasterClient && chargesEntry.selectedValue > 0)
		{
			chargesEntry.selectedValue = 0;
			chargesEntry.SetDirty();
			Debug.LogError("Charges " + chargesEntry.selectedValue);
			PickupHandler.CreatePickup(itemGravityOff.id, new ItemInstanceData(Guid.NewGuid()), base.transform.position + Vector3.up, Quaternion.identity);
		}
	}
}
