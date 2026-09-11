using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CurvedUI;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using Zorro.Core;
using pworld.Scripts.Extensions;

public class RoundSpawner : MonoBehaviour
{
	public GameObject[] possibleSpawns;

	public int testBudget = 30;

	public int testBiggestPurchase = 20;

	public Vector2 testFirstWaveSize = new Vector2(0.5f, 1f);

	public Vector2 testMinMaxSecondWaveWaitTime = new Vector2(0.2f, 0.8f);

	[SerializeField]
	private bool m_hasWeeping;

	private IBudgetCost[] spawnBudgetCosts;

	private IEnumerator Start()
	{
		while (!PhotonNetwork.InRoom || !Level.currentLevel.levelIsReady)
		{
			yield return null;
		}
		if (PlayerHandler.instance.players.Count <= 1 && m_hasWeeping)
		{
			Debug.Log("Removed weeping");
			Array.Resize(ref possibleSpawns, possibleSpawns.Length - 2);
		}
		spawnBudgetCosts = possibleSpawns.Select((GameObject o) => o.GetComponent<IBudgetCost>()).ToArray();
		if (PhotonNetwork.IsMasterClient)
		{
			yield return new WaitForEndOfFrame();
			float num = Mathf.Lerp(testFirstWaveSize.x, testFirstWaveSize.y, UnityEngine.Random.value);
			int num2 = ((float)testBudget * num).ToInt();
			int num3 = ((float)testBudget * (1f - num)).ToInt();
			int biggestMonsterPurchaseForDay = testBiggestPurchase;
			float secondWaveWaitTime = BigNumbers.GetSecondWaveWaitTime();
			if (SurfaceNetworkHandler.RoomStats != null && SingletonAsset<BigNumbers>.Instance != null)
			{
				num2 = BigNumbers.GetMonsterBudgetForDayFirstWave(SurfaceNetworkHandler.RoomStats.CurrentDay);
				biggestMonsterPurchaseForDay = BigNumbers.GetBiggestMonsterPurchaseForDay(SurfaceNetworkHandler.RoomStats.CurrentDay);
				num3 = BigNumbers.GetMonsterBudgetForSecondWave(SurfaceNetworkHandler.RoomStats.CurrentDay);
				secondWaveWaitTime = BigNumbers.GetSecondWaveWaitTime();
				Debug.Log($"MB FOR DAY {SurfaceNetworkHandler.RoomStats.CurrentDay}: Budget: {num2}, 2nd: {num3}, BiggestPurchase: {biggestMonsterPurchaseForDay} WaitTime {secondWaveWaitTime}");
			}
			else
			{
				Debug.Log($"MB FOR TEST Budget: {num2} 2nd: {num3}, BiggestPurchase: {biggestMonsterPurchaseForDay} WaitTime {secondWaveWaitTime}");
			}
			if (num2 > 0)
			{
				SpawnRound(num2, biggestMonsterPurchaseForDay, isFirstRound: true);
			}
			if (num3 > 0)
			{
				SpawnSecondRun(secondWaveWaitTime, num3, biggestMonsterPurchaseForDay);
			}
		}
	}

	private void SpawnSecondRun(float waitTime, int budgetForSpawn, int biggestPurchase)
	{
		List<(IBudgetCost, float)> list = (from m in GetMonstersToSpawn(budgetForSpawn, biggestPurchase)
			select (m: m, UnityEngine.Random.Range(0f, waitTime))).ToList();
		Debug.Log($"Spawning {list.Count} late monsters, all before {waitTime}");
		foreach (var item in list)
		{
			StartCoroutine(SpawnMonsterAfterTime(item.Item1, item.Item2));
		}
		IEnumerator SpawnMonsterAfterTime(IBudgetCost monster, float time)
		{
			Debug.Log($"will spawn {monster.gameObject.name} after {time} seconds");
			yield return new WaitForSeconds(time);
			Debug.Log($"Spawning {monster.gameObject.name} after {time} seconds");
			StartCoroutine(SpawnMonstersOutOfSight(new List<IBudgetCost> { monster }));
		}
	}

	private void SpawnRound(int budget, int biggestPurchase, bool isFirstRound)
	{
		List<IBudgetCost> monstersToSpawn = GetMonstersToSpawn(budget, biggestPurchase);
		if (isFirstRound)
		{
			SpawnMonsters(monstersToSpawn);
		}
		else
		{
			StartCoroutine(SpawnMonstersOutOfSight(monstersToSpawn));
		}
	}

	private List<IBudgetCost> GetMonstersToSpawn(int budget, int biggestPurchase)
	{
		List<IBudgetCost> list = new List<IBudgetCost>();
		int num = 1000;
		while (budget > 0 && num > 0)
		{
			num--;
			list.Add(GetMonster(ref budget, biggestPurchase));
		}
		return list;
	}

	private void PrintMonstersPerDay(int day, int times)
	{
		spawnBudgetCosts = possibleSpawns.Select((GameObject o) => o.GetComponent<IBudgetCost>()).ToArray();
		float num = BigNumbers.GetMonsterBudgetForDayFirstWave(day);
		float num2 = BigNumbers.GetMonsterBudgetForSecondWave(day);
		float num3 = BigNumbers.GetBiggestMonsterPurchaseForDay(day);
		float num4 = SingletonAsset<BigNumbers>.Instance.minMaxSecoundRoundSpawnTime.PRndRange() * 500f;
		Dictionary<IBudgetCost, int> dictionary = new Dictionary<IBudgetCost, int>();
		int num5 = 0;
		for (int num6 = 0; num6 < times; num6++)
		{
			List<IBudgetCost> list = new List<IBudgetCost>();
			int budget = (int)(num + num2);
			int num7 = 1000;
			while (budget > 0 && num7 > 0)
			{
				num7--;
				list.Add(GetMonster(ref budget, (int)num3));
			}
			num5 += list.Count;
			foreach (IBudgetCost item in list)
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
		List<KeyValuePair<IBudgetCost, int>> list2 = dictionary.Select((KeyValuePair<IBudgetCost, int> pair) => pair).ToList();
		list2.Sort((KeyValuePair<IBudgetCost, int> pair, KeyValuePair<IBudgetCost, int> valuePair) => valuePair.Value.CompareTo(pair.Value));
		foreach (KeyValuePair<IBudgetCost, int> item2 in list2)
		{
			Debug.Log(item2.Key.gameObject.name + ": " + (float)item2.Value / (float)times);
		}
		Debug.Log(" Average Monsters Per Day: " + (float)num5 / (float)times);
		Debug.Log($"Wait time for second wave: {num4}");
	}

	public void PrintRounds(int days = 51)
	{
		spawnBudgetCosts = possibleSpawns.Select((GameObject o) => o.GetComponent<IBudgetCost>()).ToArray();
		for (int num = 1; num < days; num++)
		{
			Debug.Log("Day: " + (num + 1));
			List<IBudgetCost> list = new List<IBudgetCost>();
			int num2 = 1000;
			int budget = BigNumbers.GetMonsterBudgetForDayFirstWave(num);
			int biggestMonsterPurchaseForDay = BigNumbers.GetBiggestMonsterPurchaseForDay(num);
			while (budget > 0 && num2 > 0)
			{
				num2--;
				list.Add(GetMonster(ref budget, biggestMonsterPurchaseForDay));
			}
			string text = "";
			for (int num3 = 0; num3 < list.Count; num3++)
			{
				text += list[num3].gameObject.name;
				text += ", ";
			}
			Debug.Log(text);
		}
	}

	private void SpawnMonsters(List<IBudgetCost> monsters)
	{
		Transform transform = DiveBellParent.instance.GetComponentInChildren<SpawnPoint>().transform;
		for (int i = 0; i < monsters.Count; i++)
		{
			List<PatrolPoint> pointsInGroups = Level.currentLevel.GetPointsInGroups(monsters[i].gameObject.GetComponentInChildren<IHasPatrolGroup>().GetGroup());
			float num = 0f;
			for (int j = 0; j < pointsInGroups.Count; j++)
			{
				num += pointsInGroups[j].spawnWeight * GetPointDistanceWeigth(pointsInGroups[i].transform.position, transform.position);
			}
			PatrolPoint p = null;
			float num2 = UnityEngine.Random.Range(0f, num);
			for (int k = 0; k < pointsInGroups.Count; k++)
			{
				num2 -= pointsInGroups[k].spawnWeight * GetPointDistanceWeigth(pointsInGroups[i].transform.position, transform.position);
				p = pointsInGroups[k];
				if (num2 <= 0f)
				{
					break;
				}
			}
			SpawnMonster(monsters[i], p);
		}
		Debug.Log($"spawned {monsters.Count} monsters");
	}

	private IEnumerator SpawnMonstersOutOfSight(List<IBudgetCost> monsters, float waitTime = 0.1f)
	{
		Transform spawn = DiveBellParent.instance.GetComponentInChildren<SpawnPoint>().transform;
		int i;
		for (i = 0; i < monsters.Count; i++)
		{
			IBudgetCost monsterToSpawn = monsters[i];
			List<PatrolPoint> pointsInGroups = Level.currentLevel.GetPointsInGroups(monsterToSpawn.gameObject.GetComponentInChildren<IHasPatrolGroup>().GetGroup());
			List<PatrolPoint> points = new List<PatrolPoint>(pointsInGroups);
			int num = 100;
			PatrolPoint p = null;
			for (int j = 0; j < num; j++)
			{
				if (FindPoint(pointsInGroups, out p, checkSight: true))
				{
					break;
				}
				pointsInGroups.Remove(p);
			}
			if (p == null)
			{
				FindPoint(points, out p, checkSight: false);
			}
			yield return new WaitForEndOfFrame();
			SpawnMonster(monsterToSpawn, p);
			Debug.Log("spawned " + monsterToSpawn.gameObject.name + " in Delayed Wave");
			yield return new WaitForSeconds(waitTime);
		}
		Debug.Log($"spawned {monsters.Count} monsters");
		bool FindPoint(List<PatrolPoint> list, out PatrolPoint reference, bool checkSight)
		{
			float num2 = 0f;
			for (int k = 0; k < list.Count; k++)
			{
				num2 += list[k].spawnWeight * GetPointDistanceWeigth(list[i].transform.position, spawn.position);
			}
			reference = null;
			float num3 = UnityEngine.Random.Range(0f, num2);
			for (int l = 0; l < list.Count; l++)
			{
				num3 -= list[l].spawnWeight * GetPointDistanceWeigth(list[i].transform.position, spawn.position);
				reference = list[l];
				if (num3 <= 0f)
				{
					break;
				}
			}
			Player firstPlayerThatCanSeeIt;
			if (checkSight)
			{
				return !PlayerHandler.instance.CanAnAlivePlayerSeePoint(reference.transform.position, out firstPlayerThatCanSeeIt);
			}
			return true;
		}
	}

	private float GetPointDistanceWeigth(Vector3 point, Vector3 spawn)
	{
		Vector3 vector = spawn - point;
		vector.y *= 3f;
		float magnitude = vector.magnitude;
		float t = Mathf.InverseLerp(50f, 100f, magnitude);
		return Mathf.Lerp(0.5f, 1f, t);
	}

	private void SpawnMonster(IBudgetCost monster, PatrolPoint p)
	{
		PhotonNetwork.Instantiate(monster.gameObject.name, p.transform.position, Quaternion.identity, 0);
	}

	public void CheckIfAllHaveBudget()
	{
		GameObject[] array = possibleSpawns;
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<IBudgetCost>() == null)
			{
				Debug.LogError(gameObject.name + " does not have a budget cost");
			}
		}
	}

	private IBudgetCost GetMonster(ref int budget, int biggestPurchase)
	{
		List<MonsterWithWeight> list = new List<MonsterWithWeight>();
		float num = 0f;
		for (int i = 0; i < spawnBudgetCosts.Length; i++)
		{
			if (spawnBudgetCosts[i].Cost <= biggestPurchase && spawnBudgetCosts[i].Cost <= budget)
			{
				float num2 = (float)spawnBudgetCosts[i].Cost / (float)budget;
				MonsterWithWeight monsterWithWeight = new MonsterWithWeight();
				monsterWithWeight.monster = spawnBudgetCosts[i];
				monsterWithWeight.weight = num2 * spawnBudgetCosts[i].Rarity;
				num += monsterWithWeight.weight;
				list.Add(monsterWithWeight);
			}
		}
		float num3 = UnityEngine.Random.Range(0f, num);
		IBudgetCost budgetCost = null;
		for (int j = 0; j < list.Count; j++)
		{
			budgetCost = list[j].monster;
			num3 -= list[j].weight;
			if (num3 <= 0f)
			{
				break;
			}
		}
		budget -= budgetCost.Cost;
		return budgetCost;
	}
}
