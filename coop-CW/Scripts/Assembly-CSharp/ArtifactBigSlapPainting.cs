using System.Collections.Generic;
using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;
using Zorro.Core.Serizalization;
using pworld.Scripts.Extensions;

public class ArtifactBigSlapPainting : ItemInstanceBehaviour, IArtifactContent
{
	public GameObject bigSlapPrefab;

	private IntRangeEntry chargesEntry;

	private StashAbleEntry stashAbleEntry;

	private float timeInHand;

	public bool IsHeld => isHeld;

	public bool IsActive => chargesEntry.selectedValue <= 0;

	private void Update()
	{
		if (!isHeldByMe || !isSimulatedByMe || !isHeld)
		{
			return;
		}
		timeInHand += Time.deltaTime;
		if (timeInHand > 3f)
		{
			timeInHand = 0f;
			if (chargesEntry.selectedValue > 0)
			{
				chargesEntry.selectedValue = 0;
				chargesEntry.SetDirty();
				Debug.Log("calling bigslap spawn");
				itemInstance.CallRPC(ItemRPC.RPC0, new BinarySerializer());
				PlatformManager.UnlockAchievement(Achievements.ACH_SUMMON_BIGSLAP);
			}
		}
	}

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
			Debug.Log("stashAbleEntry entry not found, adding new entry with false.");
		}
		if (data.TryGetEntry<IntRangeEntry>(out chargesEntry))
		{
			Debug.Log($"chargesEntry entry found, charges: {chargesEntry.selectedValue}");
		}
		else
		{
			chargesEntry = new IntRangeEntry
			{
				selectedValue = 1,
				maxValue = 1
			};
			data.AddDataEntry(chargesEntry);
			Debug.Log($"chargesEntry entry not found, adding new entry with {1}");
		}
		itemInstance.RegisterRPC(ItemRPC.RPC0, RPCA_SpawnSlap);
	}

	private void RPCA_SpawnSlap(BinaryDeserializer deserializer)
	{
		Debug.Log("RPCA_SpawnSlap All");
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		Debug.Log("RPCA_SpawnSlap server");
		List<PatrolPoint> pointsOutsideMinDistanceSortedOnClosest = Level.currentLevel.GetPointsOutsideMinDistanceSortedOnClosest(PatrolPoint.PatrolGroup.Bear.PToList(), Player.localPlayer.Center(), 40f, 10f);
		foreach (PatrolPoint item in pointsOutsideMinDistanceSortedOnClosest)
		{
			if (!PlayerHandler.instance.CanAnAlivePlayerSeePoint(item.transform.position, out var _))
			{
				Debug.Log("Spawn big slap");
				MonsterSpawner.SpawnMonster(bigSlapPrefab.name, pointsOutsideMinDistanceSortedOnClosest[0].transform.position);
				break;
			}
		}
	}
}
