using Photon.Pun;
using UnityEngine;

public class ArtifactZombieBook : ItemInstanceBehaviour, IArtifactContent
{
	public int maxCharges = 1;

	public GameObject cursePrefab;

	private IntRangeEntry chargesEntry;

	private OnOffEntry onOffEntry;

	private StashAbleEntry stashAbleEntry;

	public bool IsHeld => isHeld;

	public bool IsActive => onOffEntry.on;

	private void Update()
	{
		if (isHeldByMe && !Player.localPlayer.HasLockedInput() && !onOffEntry.on)
		{
			onOffEntry.on = true;
			onOffEntry.SetDirty();
			Player.localPlayer.refs.curses.SpawnCurse(this, cursePrefab);
		}
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<OnOffEntry>(out onOffEntry))
		{
			Debug.Log($"OnOff entry found, state: {onOffEntry.on}");
		}
		else
		{
			onOffEntry = new OnOffEntry
			{
				on = false
			};
			data.AddDataEntry(onOffEntry);
			Debug.Log("OnOff entry not found, adding new entry with false.");
		}
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
			Debug.Log("stashAbleEntry entry not found, adding new entry with false.");
		}
		if (data.TryGetEntry<IntRangeEntry>(out chargesEntry))
		{
			Debug.Log($"chargesEntry entry found, charges: {chargesEntry.selectedValue}");
			return;
		}
		chargesEntry = new IntRangeEntry
		{
			selectedValue = maxCharges,
			maxValue = maxCharges
		};
		data.AddDataEntry(chargesEntry);
		Debug.Log($"chargesEntry entry not found, adding new entry with {maxCharges}");
	}
}
