using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Attacks_Bombs : MonoBehaviour
{
	public GameObject explosion;

	public GameObject fuzeObj;

	public Item itemToSpawn;

	private Bot bot;

	private Player player;

	private PhotonView view;

	private MonsterAnimationHandler anim;

	private MonsterAnimationValues values;

	public float throwForce;

	private bool hasLit;

	public float secondsToExplode = 4f;

	private void Start()
	{
		anim = GetComponentInParent<MonsterAnimationHandler>();
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		player = GetComponentInParent<Player>();
		values = player.refs.animatorTransform.GetComponent<MonsterAnimationValues>();
	}

	private void Update()
	{
		if (hasLit)
		{
			secondsToExplode -= Time.deltaTime;
			if (secondsToExplode < 0f)
			{
				UnityEngine.Object.Instantiate(explosion, bot.Center(), UnityEngine.Random.rotation);
				base.enabled = false;
				player.data.dead = true;
				fuzeObj.SetActive(value: false);
				base.transform.root.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (!hasLit && bot.AbleToAttack(4f, 1f, player))
			{
				view.RPC("RPCA_BombFuzeAttack", RpcTarget.All);
			}
			if (bot.AbleToAttack(15f, 2f, player))
			{
				view.RPC("RPCA_BombThrowAttack", RpcTarget.All);
			}
		}
	}

	[PunRPC]
	private void RPCA_BombFuzeAttack()
	{
		hasLit = true;
		fuzeObj.SetActive(value: true);
	}

	[PunRPC]
	private void RPCA_BombThrowAttack()
	{
		anim.PlayAnimation("Bombs_Throw");
		StartCoroutine(IDoThrow());
		IEnumerator IDoThrow()
		{
			bot.attacking = true;
			bool hasThrown = false;
			Vector3 targetPos = Vector3.zero;
			if ((bool)bot.targetPlayer)
			{
				targetPos = bot.targetPlayer.Center();
			}
			float c = 0f;
			while (c < 5f && !player.NoControl())
			{
				if (view.IsMine)
				{
					if (values.rightPunch && !hasThrown)
					{
						Vector3 vel = ((targetPos - bot.Center()).normalized + Vector3.up * 0.2f) * throwForce;
						PickupHandler.CreatePickup(itemToSpawn.id, new ItemInstanceData(Guid.NewGuid()), player.refs.ragdoll.GetBodypart(BodypartType.Elbow_R).rig.transform.GetChild(0).position, UnityEngine.Random.rotation, vel, UnityEngine.Random.onUnitSphere * 5f);
						hasThrown = true;
					}
					if ((bool)bot.targetPlayer && bot.CanSee(bot.Center(), bot.targetPlayer.Center(), 30f, 120f))
					{
						targetPos = bot.targetPlayer.Center();
					}
					bot.StandStill();
					bot.LookAt(targetPos, 5f);
				}
				c += Time.deltaTime;
				yield return null;
			}
			bot.attacking = false;
		}
	}
}
