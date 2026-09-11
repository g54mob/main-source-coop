using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;
using UnityEngine.Serialization;

public class PartyPopper : ItemInstanceBehaviour
{
	[FormerlySerializedAs("explodedGoopPref")]
	public GameObject confettiPref;

	private StashAbleEntry stashAbleEntry;

	private OnOffEntry usedEntry;

	public GameObject chargesLeftGO;

	private bool wasUsedOnConfig;

	private bool wasConfig;

	public Transform dir;

	private void Update()
	{
		if (!wasConfig)
		{
			return;
		}
		if (isHeldByMe && !Player.localPlayer.HasLockedInput() && !usedEntry.on && Player.localPlayer.input.clickWasPressed)
		{
			usedEntry.on = true;
			stashAbleEntry.isStashAble = false;
			stashAbleEntry.SetDirty();
			usedEntry.SetDirty();
		}
		if (usedEntry.on && !wasUsedOnConfig)
		{
			Object.Instantiate(confettiPref, dir.position, dir.rotation);
			wasUsedOnConfig = true;
			if (isHeldByMe)
			{
				PlatformManager.UnlockAchievement(Achievements.ACH_PARTY_POPPER);
			}
		}
		chargesLeftGO.SetActive(!usedEntry.on);
	}

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		wasConfig = true;
		if (data.TryGetEntry<StashAbleEntry>(out stashAbleEntry))
		{
			Debug.Log($"stashAbleEntry entry found, isStashAble: {stashAbleEntry.isStashAble}");
		}
		else
		{
			stashAbleEntry = new StashAbleEntry
			{
				isStashAble = true
			};
			data.AddDataEntry(stashAbleEntry);
			Debug.Log("stashAbleEntry entry not found, adding new entry with true.");
		}
		if (data.TryGetEntry<OnOffEntry>(out usedEntry))
		{
			Debug.Log($"OnOff entry found, state: {usedEntry.on}");
		}
		else
		{
			usedEntry = new OnOffEntry
			{
				on = false
			};
			data.AddDataEntry(usedEntry);
			Debug.Log("OnOff entry not found, adding new entry with false.");
		}
		wasUsedOnConfig = usedEntry.on;
		chargesLeftGO.SetActive(!usedEntry.on);
	}
}
