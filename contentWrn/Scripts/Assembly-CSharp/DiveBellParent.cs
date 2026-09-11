using System.Linq;
using UnityEngine;
using pworld.Scripts.Extensions;

public class DiveBellParent : MonoBehaviour
{
	public int testDay = 1;

	public static DiveBellParent instance;

	public DivingBell TargetDivingBell { get; private set; }

	internal SpawnPoint GetSpawn()
	{
		Random.State state = Random.state;
		Random.InitState(GameAPI.seed);
		int currentDay = testDay;
		if (SurfaceNetworkHandler.RoomStats != null)
		{
			currentDay = GameAPI.CurrentDay;
		}
		else
		{
			Debug.LogError("SurfaceNetworkHandler.RoomStats is null");
		}
		int diveBellDiff = BigNumbers.GetTempDiveBellDifficulty(currentDay);
		VerboseDebug.LogError("Difficulty: " + diveBellDiff + " CurrentDay " + currentDay);
		Transform transform = (from bell in GetComponentsInChildren<DivingBell>()
			where bell.spawnDifficulty <= (float)diveBellDiff
			select bell).ToList().GetRnd().transform;
		for (int num = 0; num < base.transform.childCount; num++)
		{
			if (base.transform.GetChild(num) == transform)
			{
				base.transform.GetChild(num).gameObject.SetActive(value: true);
			}
			else
			{
				base.transform.GetChild(num).gameObject.SetActive(value: false);
			}
		}
		Random.state = state;
		TargetDivingBell = transform.GetComponent<DivingBell>();
		VerboseDebug.LogError("PICKED Difficulty: " + TargetDivingBell.spawnDifficulty + " CurrentDay " + currentDay);
		return transform.GetComponentInChildren<SpawnPoint>();
	}

	private void DebugSpawnPoints()
	{
		Random.State state = Random.state;
		Random.InitState(GameAPI.seed);
		int currentDay = testDay;
		if (SurfaceNetworkHandler.Instance != null)
		{
			currentDay = GameAPI.CurrentDay;
		}
		int diveBellDiff = BigNumbers.GetTempDiveBellDifficulty(currentDay);
		Debug.Log("Difficulty: " + diveBellDiff);
		foreach (DivingBell item in from bell in GetComponentsInChildren<DivingBell>()
			where bell.spawnDifficulty <= (float)diveBellDiff
			select bell)
		{
			Debug.DrawLine(item.transform.position, item.transform.position + Vector3.up * 100f, Color.red, 2f);
		}
		Random.state = state;
	}

	private void Awake()
	{
		instance = this;
	}
}
