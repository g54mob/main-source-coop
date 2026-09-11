using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using pworld.Scripts.Extensions;

public class InfiltratorSpawner : MonoBehaviour, IBudgetCost, IHasPatrolGroup
{
	public GameObject infiltratorPrefab;

	public Bot_Infiltrator spawnedInfiltrator;

	private Bot_Infiltrator infiltratorComponent;

	public bool forceSpawn;

	private float timeAlive;

	public bool Infiltrating => spawnedInfiltrator != null;

	public int Cost => infiltratorPrefab.GetComponent<IBudgetCost>().Cost;

	public float Rarity => infiltratorPrefab.GetComponent<IBudgetCost>().Rarity;

	GameObject IBudgetCost.gameObject => base.gameObject;

	private void Awake()
	{
		infiltratorComponent = infiltratorPrefab.GetComponentInChildren<Bot_Infiltrator>();
		Debug.Log("Spanwed InfiltratorSpawner");
	}

	private bool FindMimicTarget(out Player hitTarget, out Player mimicTarget)
	{
		if (PlayerHandler.instance.GetLargestClosestDistanceBetweenPlayers(out var maxMinDistanceBetweenPlayers, out var mostAlonePlayer) && maxMinDistanceBetweenPlayers > infiltratorComponent.distanceToBeConsideredAlone)
		{
			mimicTarget = mostAlonePlayer;
			hitTarget = PlayerHandler.instance.GetFurthestPlayerFromPlayer(mimicTarget);
			return true;
		}
		hitTarget = null;
		mimicTarget = null;
		return false;
	}

	private void Update()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		timeAlive += Time.deltaTime;
		if (timeAlive < 5f)
		{
			return;
		}
		if (!Infiltrating)
		{
			if (!FindMimicTarget(out var hitTarget, out var mimicTarget))
			{
				return;
			}
			{
				foreach (PatrolPoint item in Level.currentLevel.GetPointsOutsideMinDistanceSortedOnClosest(PatrolPoint.PatrolGroup.Bear.PToList(), hitTarget.Center(), 10f, 4f))
				{
					Vector3 spawnPosition;
					if (!PlayerHandler.instance.CanAnAlivePlayerSeePoint(item.transform.position, out var _))
					{
						spawnPosition = item.transform.position + Vector3.up;
						StartCoroutine(SpawnMonster());
						break;
					}
					IEnumerator SpawnMonster()
					{
						yield return new WaitForEndOfFrame();
						spawnedInfiltrator = MonsterSpawner.SpawnMonster(infiltratorPrefab.name, spawnPosition).GetComponentInChildren<Bot_Infiltrator>();
						spawnedInfiltrator.Init(hitTarget, mimicTarget);
					}
				}
				return;
			}
		}
		PhotonNetwork.Destroy(base.transform.root.gameObject);
	}

	public List<PatrolPoint.PatrolGroup> GetGroup()
	{
		return infiltratorPrefab.GetComponentInChildren<IHasPatrolGroup>().GetGroup();
	}
}
