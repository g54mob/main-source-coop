using System.Linq;
using DefaultNamespace.Artifacts;
using Photon.Pun;

public class ArtifactGravityFlipperOff : ItemInstanceBehaviour
{
	private StashAbleEntry stashAbleEntry;

	public Item itemGravityOn;

	public override void ConfigItem(ItemInstanceData data, PhotonView playerView)
	{
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
		if (!isHeldByMe || !isHeld)
		{
			return;
		}
		CurseOfGravityFlip[] componentsInChildren = Player.localPlayer.refs.curses.GetComponentsInChildren<CurseOfGravityFlip>();
		if (componentsInChildren.Length == 0)
		{
			return;
		}
		foreach (CurseOfGravityFlip item in componentsInChildren.Where((CurseOfGravityFlip curse) => curse.itemSource == itemGravityOn))
		{
			PhotonNetwork.Destroy(item.gameObject);
		}
	}
}
