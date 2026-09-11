using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;

public class RoundSpawnerTools : MonoBehaviour
{
	public class ToolWithWeight
	{
		public Item tool;

		public float weight;

		public int BudgetCost => tool.toolBudgetCost;

		public float Rarity => tool.ToolRarity;
	}

	public static RoundSpawnerTools me;

	public int testDay = 1;

	public GameObject artifactSpawnerPrefab;

	public List<Item> possibleSpawns;

	private void Awake()
	{
		me = this;
	}

	private void Populate()
	{
		possibleSpawns = new List<Item>();
		foreach (Item lastLoadedItem in SingletonAsset<ItemDatabase>.Instance.lastLoadedItems)
		{
			if (lastLoadedItem.itemType == Item.ItemType.Tool && lastLoadedItem.spawnable)
			{
				possibleSpawns.Add(lastLoadedItem);
			}
		}
	}

	private IEnumerator Start()
	{
		Populate();
		Debug.Log("Start Artifact round spawner");
		while (!PhotonNetwork.InRoom || !Level.currentLevel.levelIsReady)
		{
			yield return null;
		}
		if (PhotonNetwork.IsMasterClient)
		{
			yield return new WaitForEndOfFrame();
			int currentDay = testDay;
			if (SurfaceNetworkHandler.RoomStats != null && SingletonAsset<BigNumbers>.Instance != null)
			{
				currentDay = SurfaceNetworkHandler.RoomStats.CurrentDay;
			}
			int toolBudgetForDay = BigNumbers.GetToolBudgetForDay(currentDay);
			float rarestToolForDay = BigNumbers.GetRarestToolForDay(currentDay);
			Debug.Log($"ToolSpawner Day{currentDay} Budget: {toolBudgetForDay}, BiggestPurchase: {rarestToolForDay}");
			SpawnRound(toolBudgetForDay, rarestToolForDay);
		}
	}

	private void SpawnRound(int budget, float rarestPurchase)
	{
		CreateArtifactSpawners(GetToolsToSpawn(budget, rarestPurchase));
	}

	private void PrintToolsPerDay(int day, int times)
	{
		float num = BigNumbers.GetToolBudgetForDay(day);
		float rarestToolForDay = BigNumbers.GetRarestToolForDay(day);
		Dictionary<Item, int> dictionary = new Dictionary<Item, int>();
		int num2 = 0;
		for (int i = 0; i < times; i++)
		{
			List<Item> toolsToSpawn = GetToolsToSpawn((int)num, rarestToolForDay);
			num2 += toolsToSpawn.Count;
			foreach (Item item in toolsToSpawn)
			{
				if (dictionary.ContainsKey(item))
				{
					dictionary[item]++;
				}
				else
				{
					dictionary.Add(item, 1);
				}
			}
		}
		Debug.Log("Day " + day + " avg tools: " + num2 / times + " budget: " + num + " rarest: " + rarestToolForDay + " times: " + times);
		foreach (KeyValuePair<Item, int> item2 in dictionary)
		{
			Debug.Log(item2.Key.name + ": " + (float)item2.Value / (float)times);
		}
	}

	private List<Item> GetToolsToSpawn(int budget, float rarestPurchase)
	{
		List<Item> list = new List<Item>();
		int num = 1000;
		while (budget > 0 && num > 0)
		{
			num--;
			list.Add(GetArtifact(ref budget, possibleSpawns, rarestPurchase));
		}
		return list;
	}

	private void CreateArtifactSpawners(List<Item> artifacts)
	{
		for (int i = 0; i < artifacts.Count; i++)
		{
			ArtifactSpawner component = Object.Instantiate(artifactSpawnerPrefab, GetRandPointWithWeight(), Quaternion.identity).GetComponent<ArtifactSpawner>();
			component.gameObject.SetActive(value: true);
			component.transform.parent = base.transform;
			component.artifactToSpawn = artifacts[i];
		}
	}

	public static Vector3 GetRandPointWithWeight()
	{
		float num = 0f;
		PatrolPoint patrolPoint = null;
		List<PatrolPoint> pointsInGroups = Level.currentLevel.GetPointsInGroups(new List<PatrolPoint.PatrolGroup>
		{
			PatrolPoint.PatrolGroup.Bear,
			PatrolPoint.PatrolGroup.Dog
		});
		for (int i = 0; i < pointsInGroups.Count; i++)
		{
			num += pointsInGroups[i].spawnWeight;
		}
		float num2 = Random.Range(0f, num);
		for (int j = 0; j < pointsInGroups.Count; j++)
		{
			num2 -= pointsInGroups[j].spawnWeight;
			patrolPoint = pointsInGroups[j];
			if (num2 <= 0f)
			{
				break;
			}
		}
		return patrolPoint.transform.position;
	}

	private static Item GetArtifact(ref int budget, List<Item> possibleSpawns, float rarestPurchase)
	{
		List<ToolWithWeight> list = new List<ToolWithWeight>();
		float num = 0f;
		for (int i = 0; i < possibleSpawns.Count; i++)
		{
			if (!(possibleSpawns[i].ToolRarity <= rarestPurchase) && possibleSpawns[i].toolBudgetCost <= budget)
			{
				float num2 = (float)possibleSpawns[i].toolBudgetCost / (float)budget;
				ToolWithWeight toolWithWeight = new ToolWithWeight();
				toolWithWeight.tool = possibleSpawns[i];
				toolWithWeight.weight = num2 * possibleSpawns[i].ToolRarity;
				num += toolWithWeight.weight;
				list.Add(toolWithWeight);
			}
		}
		float num3 = Random.Range(0f, num);
		ToolWithWeight toolWithWeight2 = null;
		for (int j = 0; j < list.Count; j++)
		{
			toolWithWeight2 = list[j];
			num3 -= list[j].weight;
			if (num3 <= 0f)
			{
				break;
			}
		}
		budget -= toolWithWeight2.BudgetCost;
		return toolWithWeight2.tool;
	}

	public static Item GetRandomArtifactByRarity(Item[] possibleSpawns)
	{
		List<ToolWithWeight> list = new List<ToolWithWeight>();
		float num = 0f;
		for (int i = 0; i < possibleSpawns.Length; i++)
		{
			ToolWithWeight toolWithWeight = new ToolWithWeight();
			toolWithWeight.tool = possibleSpawns[i];
			toolWithWeight.weight = possibleSpawns[i].ToolRarity;
			num += toolWithWeight.weight;
			list.Add(toolWithWeight);
		}
		float num2 = Random.Range(0f, num);
		ToolWithWeight toolWithWeight2 = null;
		for (int j = 0; j < list.Count; j++)
		{
			toolWithWeight2 = list[j];
			num2 -= list[j].weight;
			if (num2 <= 0f)
			{
				break;
			}
		}
		return toolWithWeight2.tool;
	}
}
