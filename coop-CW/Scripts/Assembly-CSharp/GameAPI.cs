using System;
using UnityEngine;

public class GameAPI : MonoBehaviour
{
	public static int seed = -1;

	public static GameAPI instance;

	public Action<GameObject> objectSpawnedAction;

	private static readonly int BASE_BUDGET = 5;

	private static readonly float BUDGET_FACTOR = 2f;

	public static int MonsterBudget { get; private set; }

	public static int MaxSingleMonsterCost { get; private set; }

	public static int ArtifactBudget { get; private set; }

	public static int matchSeed => SurfaceNetworkHandler.RoomStats.MatchSeed;

	public static int CurrentDay
	{
		get
		{
			if (SurfaceNetworkHandler.RoomStats == null)
			{
				return 0;
			}
			return SurfaceNetworkHandler.RoomStats.CurrentDay;
		}
	}

	private void Awake()
	{
		instance = this;
		if (seed == -1)
		{
			seed = UnityEngine.Random.Range(0, 100000);
		}
	}

	internal static bool GetSeededPositionProbability(Vector3 position, double probability)
	{
		position.x = Mathf.RoundToInt(position.x);
		position.y = Mathf.RoundToInt(position.y);
		position.z = Mathf.RoundToInt(position.z);
		UnityEngine.Random.State state = UnityEngine.Random.state;
		UnityEngine.Random.InitState(Mathf.RoundToInt((float)seed + position.x + position.y * 100f + position.z * 10000f));
		float value = UnityEngine.Random.value;
		UnityEngine.Random.state = state;
		return (double)value < probability;
	}

	public static void GenerateNewBudget(int currentDay, int quotaDay, bool supressLogs = false)
	{
		if (!supressLogs)
		{
			VerboseDebug.Log("Generating New Budget For day: " + currentDay + " : " + quotaDay + "/3");
		}
		int num = currentDay / 3;
		if (!supressLogs)
		{
			VerboseDebug.Log("Number Of Runs: " + num);
		}
		float num2 = (float)BASE_BUDGET + (float)num * BUDGET_FACTOR;
		if (!supressLogs)
		{
			VerboseDebug.Log("Run Base Budget: " + num2);
		}
		MonsterBudget = (int)Mathf.Pow(num2 + (float)(quotaDay * quotaDay), 1.2f);
		if (!supressLogs)
		{
			VerboseDebug.Log("Run TotalBudget: " + MonsterBudget);
		}
	}
}
