using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;

public class MonsterGroupClose : MonoBehaviour, IHasPatrolGroup, IBudgetCost
{
	public bool allOnSamePatrolPoint;

	public bool firstMonsterIsLeader;

	public List<GameObject> monsters = new List<GameObject>();

	public List<Bot> spanwedMonsters = new List<Bot>();

	private IHasPatrolGroup monsterPG;

	private List<IBudgetCost> budgetCosts = new List<IBudgetCost>();

	public int extraCost;

	public float rarity;

	private List<IBudgetCost> BudgetCosts
	{
		get
		{
			if (budgetCosts == null || budgetCosts.Count != monsters.Count)
			{
				budgetCosts = new List<IBudgetCost>();
				budgetCosts = monsters.Select((GameObject m) => m.transform.root.GetComponentInChildren<IBudgetCost>()).ToList();
			}
			return budgetCosts;
		}
	}

	public int Cost
	{
		get
		{
			int num = 0;
			foreach (IBudgetCost budgetCost in BudgetCosts)
			{
				num += budgetCost.Cost;
			}
			return num + extraCost;
		}
	}

	public float Rarity => rarity;

	GameObject IBudgetCost.gameObject => base.gameObject;

	private void Awake()
	{
		monsterPG = monsters[0].GetComponentInChildren<IHasPatrolGroup>();
		if (monsterPG == null)
		{
			throw new Exception("monster does not have a patrol group");
		}
	}

	private IEnumerator Start()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			yield break;
		}
		yield return new WaitForEndOfFrame();
		List<PatrolPoint> list = null;
		if (!allOnSamePatrolPoint)
		{
			list = Level.currentLevel.GetPointsOutsideMinDistanceSortedOnClosest(monsterPG.GetGroup(), base.transform.position, 0f, 4f);
		}
		Bot bot = null;
		for (int i = 0; i < monsters.Count; i++)
		{
			GameObject gameObject = monsters[i];
			Bot bot2 = ((!allOnSamePatrolPoint) ? MonsterSpawner.SpawnMonster(gameObject.name, list[i].transform.position).GetComponentInChildren<Bot>() : MonsterSpawner.SpawnMonster(gameObject.name, base.transform.position).GetComponentInChildren<Bot>());
			if (firstMonsterIsLeader)
			{
				if (bot == null)
				{
					bot = bot2;
				}
				else
				{
					bot2.SetLeader(bot);
				}
			}
			spanwedMonsters.Add(bot2);
		}
	}

	public List<PatrolPoint.PatrolGroup> GetGroup()
	{
		if (monsterPG == null)
		{
			monsterPG = monsters[0].GetComponentInChildren<IHasPatrolGroup>();
		}
		return monsterPG.GetGroup();
	}

	private void PrintCost()
	{
		Debug.Log("Cost: " + Cost);
	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw new NotImplementedException();
	}
}
