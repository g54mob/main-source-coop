using System.Collections;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.AI;

namespace Mirror.Examples.TopDownShooter
{
	public class EnemyTopDown : NetworkBehaviour
	{
		private CanvasTopDown canvasTopDown;

		public float followDistance = 8f;

		public float findPlayersTime = 1f;

		public float distanceToKillAt = 0.5f;

		private NavMeshAgent agent;

		private Transform closestTarget;

		public Vector3 previousPosition;

		public GameObject enemyArt;

		public GameObject idleSprite;

		public GameObject aggroSprite;

		public AudioSource soundDeath;

		public AudioSource soundAggro;

		private void Awake()
		{
			canvasTopDown = Object.FindAnyObjectByType<CanvasTopDown>();
		}

		private void Start()
		{
			previousPosition = base.transform.position;
			if (base.isServer)
			{
				agent = GetComponent<NavMeshAgent>();
				InvokeRepeating("FindClosestTarget", findPlayersTime, findPlayersTime);
			}
			if (base.isClient)
			{
				InvokeRepeating("SetSprite", 0.1f, 0.1f);
			}
		}

		[ServerCallback]
		private void Update()
		{
			if (NetworkServer.active)
			{
				FollowTarget();
			}
		}

		[ServerCallback]
		private void FindClosestTarget()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			float num = float.PositiveInfinity;
			closestTarget = null;
			foreach (PlayerTopDown player in PlayerTopDown.playerList)
			{
				float num2 = Vector3.Distance(base.transform.position, player.transform.position);
				if (player.flashLightStatus)
				{
					num2 /= 2f;
				}
				if (player.playerStatus == 0 && num2 < num && num2 <= followDistance)
				{
					num = num2;
					closestTarget = player.transform;
					if (Vector3.Distance(base.transform.position, player.transform.position) < distanceToKillAt)
					{
						player.Kill();
					}
				}
			}
			if (closestTarget == null)
			{
				agent.isStopped = true;
			}
			else
			{
				agent.isStopped = false;
			}
		}

		[ServerCallback]
		private void FollowTarget()
		{
			if (NetworkServer.active && closestTarget != null)
			{
				agent.SetDestination(closestTarget.position);
			}
		}

		[ServerCallback]
		public void Kill()
		{
			if (NetworkServer.active)
			{
				RpcKill();
				if (base.isServerOnly)
				{
					StartCoroutine(KillCoroutine());
				}
			}
		}

		[ClientRpc]
		private void RpcKill()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void Mirror.Examples.TopDownShooter.EnemyTopDown::RpcKill()", 2017001100, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private IEnumerator KillCoroutine()
		{
			soundDeath.Play();
			enemyArt.SetActive(value: false);
			if (base.isClient)
			{
				Object.Destroy(Object.Instantiate(canvasTopDown.deathSplatter, base.transform.position, base.transform.rotation), 5f);
			}
			yield return new WaitForSeconds(0.1f);
			if (base.isServer)
			{
				closestTarget = null;
				base.transform.position = new Vector3(Random.Range(canvasTopDown.networkTopDown.enemySpawnRangeX.x, canvasTopDown.networkTopDown.enemySpawnRangeX.y), 0f, Random.Range(canvasTopDown.networkTopDown.enemySpawnRangeZ.x, canvasTopDown.networkTopDown.enemySpawnRangeZ.y));
			}
			yield return new WaitForSeconds(0.1f);
			enemyArt.SetActive(value: true);
			if (base.isServer)
			{
				canvasTopDown.networkTopDown.SpawnEnemy();
			}
		}

		[ClientCallback]
		private void SetSprite()
		{
			if (!NetworkClient.active)
			{
				return;
			}
			if (base.transform.position == previousPosition)
			{
				if (!idleSprite.activeInHierarchy)
				{
					idleSprite.SetActive(value: true);
					aggroSprite.SetActive(value: false);
				}
				return;
			}
			if (!aggroSprite.activeInHierarchy)
			{
				idleSprite.SetActive(value: false);
				aggroSprite.SetActive(value: true);
				soundAggro.Play();
			}
			previousPosition = base.transform.position;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_RpcKill()
		{
			StartCoroutine(KillCoroutine());
		}

		protected static void InvokeUserCode_RpcKill(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcKill called on server.");
			}
			else
			{
				((EnemyTopDown)obj).UserCode_RpcKill();
			}
		}

		static EnemyTopDown()
		{
			RemoteProcedureCalls.RegisterRpc(typeof(EnemyTopDown), "System.Void Mirror.Examples.TopDownShooter.EnemyTopDown::RpcKill()", InvokeUserCode_RpcKill);
		}
	}
}
