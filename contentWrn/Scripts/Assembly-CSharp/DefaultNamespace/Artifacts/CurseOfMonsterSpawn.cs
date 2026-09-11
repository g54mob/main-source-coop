using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using pworld.Scripts.Extensions;

namespace DefaultNamespace.Artifacts
{
	public class CurseOfMonsterSpawn : MonoBehaviour, IArtifactCurse
	{
		[SerializeField]
		private float rarity;

		[SerializeField]
		private float budgetCost;

		public Player cursedPlayer;

		public ItemInstanceBehaviour itemSource;

		private PhotonView view_g;

		[FormerlySerializedAs("numberOfZombies")]
		public int nrOfMonstersToSpawn = 5;

		public GameObject zombiePrefab;

		public float Rarity => rarity;

		public float BudgetCost => budgetCost;

		public void CastCurse(ItemInstanceBehaviour cursedItem, Player playerHoldingItem)
		{
			itemSource = cursedItem;
			cursedPlayer = playerHoldingItem;
			Debug.Log($"CastCurse {base.gameObject.name} from {cursedItem} on {playerHoldingItem.name}", cursedPlayer);
			if (playerHoldingItem.refs.view.IsMine)
			{
				Debug.Log("Im curse owner and doing shit");
				view_g.RPC("RPCA_AttachToPlayer", RpcTarget.All, playerHoldingItem.refs.view.OwnerActorNr);
				view_g.RPC("RPCS_SpawnMonsters", RpcTarget.MasterClient, cursedPlayer.Center());
			}
		}

		[PunRPC]
		private void RPCS_SpawnMonsters(Vector3 position)
		{
			if (nrOfMonstersToSpawn <= 0)
			{
				return;
			}
			Debug.Log("RPCS_SpawnMonsters");
			List<PatrolPoint> pointsOutsideMinDistanceSortedOnClosest = Level.currentLevel.GetPointsOutsideMinDistanceSortedOnClosest(PatrolPoint.PatrolGroup.Bear.PToList(), position, 20f, 4f);
			Debug.Log("points " + pointsOutsideMinDistanceSortedOnClosest.Count);
			foreach (PatrolPoint item in pointsOutsideMinDistanceSortedOnClosest)
			{
				if (!PlayerHandler.instance.CanAnAlivePlayerSeePoint(item.transform.position, out var _))
				{
					MonsterSpawner.SpawnMonster(zombiePrefab.name, pointsOutsideMinDistanceSortedOnClosest[0].transform.position);
					nrOfMonstersToSpawn--;
					if (nrOfMonstersToSpawn <= 0)
					{
						break;
					}
				}
			}
		}

		public void Awake()
		{
			view_g = GetComponent<PhotonView>();
		}

		[PunRPC]
		private void RPCA_AttachToPlayer(int playerid)
		{
			if (PlayerHandler.instance.TryGetPlayerFromOwnerID(playerid, out var o))
			{
				Debug.Log("Attaching to player " + o.name, o);
				base.transform.parent = o.refs.curses.transform;
			}
		}
	}
}
