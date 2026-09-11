using Photon.Pun;

public class ArtifactGeneric : ItemInstanceBehaviour, IArtifactContent
{
	private StashAbleEntry stashAbleEntry;

	public bool IsHeld => isHeld;

	public bool IsActive => true;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
		if (data.TryGetEntry<StashAbleEntry>(out stashAbleEntry))
		{
			VerboseDebug.Log($"stashAbleEntry entry found, isStashAble: {stashAbleEntry.isStashAble}");
			return;
		}
		stashAbleEntry = new StashAbleEntry
		{
			isStashAble = false
		};
		data.AddDataEntry(stashAbleEntry);
		VerboseDebug.Log("stashAbleEntry entry not found, adding new entry with false.");
	}
}
