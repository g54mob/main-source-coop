using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;

public class RoundArtifactSpawner : MonoBehaviour
{
	public class ArtifactWithWeight
	{
		public Item artifact;

		public float weight;

		public int BudgetCost => artifact.budgetCost;

		public float Rarity => artifact.rarity;
	}

	public static RoundArtifactSpawner me;

	public Item[] possibleSpawns;

	public int testDay = 1;

	public GameObject artifactSpawnerPrefab;

	private void Awake()
	{
		me = this;
	}

	private IEnumerator Start()
	{
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
			int artifactBudgetForDay = BigNumbers.GetArtifactBudgetForDay(currentDay);
			float rarestArtifactForDay = BigNumbers.GetRarestArtifactForDay(currentDay);
			Debug.Log($"MB FOR DAY {currentDay}: Budget: {artifactBudgetForDay}, BiggestPurchase: {rarestArtifactForDay}");
			SpawnRound(artifactBudgetForDay, rarestArtifactForDay);
		}
	}

	private void SpawnRound(int budget, float rarestPurchase)
	{
		CreateArtifactSpawners(GetArtifactsToSpawn(budget, rarestPurchase));
	}

	private void PrintArtifactsPerDay(int day, int times)
	{
		float num = BigNumbers.GetArtifactBudgetForDay(day);
		float rarestArtifactForDay = BigNumbers.GetRarestArtifactForDay(day);
		Dictionary<Item, int> dictionary = new Dictionary<Item, int>();
		for (int i = 0; i < times; i++)
		{
			foreach (Item item in GetArtifactsToSpawn((int)num, rarestArtifactForDay))
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
		Debug.Log("Day " + day + " times: " + times + " budget: " + num + " rarest: " + rarestArtifactForDay);
		List<KeyValuePair<Item, int>> list = dictionary.ToList();
		list.Sort((KeyValuePair<Item, int> pair1, KeyValuePair<Item, int> pair2) => pair1.Value.CompareTo(pair2.Value));
		foreach (KeyValuePair<Item, int> item2 in list)
		{
			Debug.Log(item2.Key.name + ": " + (float)item2.Value / (float)times);
		}
	}

	private List<Item> GetArtifactsToSpawn(int budget, float rarestPurchase)
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
			component.transform.parent = base.transform;
			component.gameObject.SetActive(value: true);
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

	private static Item GetArtifact(ref int budget, Item[] possibleSpawns, float rarestPurchase)
	{
		List<ArtifactWithWeight> list = new List<ArtifactWithWeight>();
		float num = 0f;
		for (int i = 0; i < possibleSpawns.Length; i++)
		{
			if (!(possibleSpawns[i].rarity <= rarestPurchase) && possibleSpawns[i].budgetCost <= budget)
			{
				float num2 = (float)possibleSpawns[i].budgetCost / (float)budget;
				ArtifactWithWeight artifactWithWeight = new ArtifactWithWeight();
				artifactWithWeight.artifact = possibleSpawns[i];
				artifactWithWeight.weight = num2 * possibleSpawns[i].rarity;
				num += artifactWithWeight.weight;
				list.Add(artifactWithWeight);
			}
		}
		float num3 = Random.Range(0f, num);
		ArtifactWithWeight artifactWithWeight2 = null;
		for (int j = 0; j < list.Count; j++)
		{
			artifactWithWeight2 = list[j];
			num3 -= list[j].weight;
			if (num3 <= 0f)
			{
				break;
			}
		}
		budget -= artifactWithWeight2.BudgetCost;
		return artifactWithWeight2.artifact;
	}

	public static Item GetRandomArtifactByRarity(Item[] possibleSpawns)
	{
		List<ArtifactWithWeight> list = new List<ArtifactWithWeight>();
		float num = 0f;
		for (int i = 0; i < possibleSpawns.Length; i++)
		{
			ArtifactWithWeight artifactWithWeight = new ArtifactWithWeight();
			artifactWithWeight.artifact = possibleSpawns[i];
			artifactWithWeight.weight = possibleSpawns[i].rarity;
			num += artifactWithWeight.weight;
			list.Add(artifactWithWeight);
		}
		float num2 = Random.Range(0f, num);
		ArtifactWithWeight artifactWithWeight2 = null;
		for (int j = 0; j < list.Count; j++)
		{
			artifactWithWeight2 = list[j];
			num2 -= list[j].weight;
			if (num2 <= 0f)
			{
				break;
			}
		}
		return artifactWithWeight2.artifact;
	}
}
