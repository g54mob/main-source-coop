using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Attacks_SnailSpawner : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	private MonsterAnimationHandler animator;

	public GameObject snailShot;

	public Transform[] spawns;

	public float cd = 0.3f;

	public float spread = 0.1f;

	private float counter;

	public int poolSize;

	private Queue<GameObject> pool;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		view = base.transform.GetComponent<PhotonView>();
		animator = GetComponentInParent<MonsterAnimationHandler>();
		if (poolSize > 0)
		{
			pool = new Queue<GameObject>(poolSize);
			for (int i = 0; i < poolSize; i++)
			{
				GameObject gameObject = Object.Instantiate(snailShot);
				gameObject.SetActive(value: false);
				pool.Enqueue(gameObject);
			}
		}
	}

	private void OnDestroy()
	{
		if (poolSize <= 0)
		{
			return;
		}
		foreach (GameObject item in pool)
		{
			Object.Destroy(item);
		}
	}

	private void Update()
	{
		if (view.IsMine && bot.aggro)
		{
			counter += Time.deltaTime;
			if (!(counter < cd))
			{
				counter = 0f;
				Spawn();
			}
		}
	}

	private void Spawn()
	{
		Transform obj = spawns[Random.Range(0, spawns.Length)];
		Vector3 position = obj.position;
		Vector3 forward = obj.forward;
		forward += Random.onUnitSphere * spread;
		view.RPC("RPCA_SnailSpawn", RpcTarget.All, position, forward);
	}

	[PunRPC]
	public void RPCA_SnailSpawn(Vector3 pos, Vector3 forward)
	{
		if (poolSize > 0)
		{
			GameObject gameObject = pool.Dequeue();
			if (!gameObject.activeSelf)
			{
				gameObject.transform.position = pos;
				gameObject.transform.rotation = Quaternion.LookRotation(forward);
				gameObject.SetActive(value: true);
			}
			pool.Enqueue(gameObject);
		}
		else
		{
			Object.Instantiate(snailShot, pos, Quaternion.LookRotation(forward));
		}
	}
}
