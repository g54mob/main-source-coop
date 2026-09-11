using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Attack_Melee : MonoBehaviour
{
	private Bot bot;

	private Player player;

	private PhotonView view;

	private MonsterAnimationHandler anim;

	private MonsterAnimationValues values;

	private bool punching;

	public float damage = 35f;

	public float knockback = 5f;

	public float fallTime = 1f;

	public float cooldown = 1f;

	public float range = 2f;

	public AnimationCurve attackCurve;

	public float force;

	public float collisionThreshold = 3f;

	public BodypartType attackPart;

	public List<BodypartType> additionalAttackPart = new List<BodypartType>();

	public float additionalAttackForceMultiplier = 0.5f;

	private List<Player> ignoredPlayers = new List<Player>();

	public SFX_Instance swingSFX;

	public SFX_Instance punchSFX;

	private void Start()
	{
		anim = GetComponentInParent<MonsterAnimationHandler>();
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		player = GetComponentInParent<Player>();
		values = player.refs.animatorTransform.GetComponent<MonsterAnimationValues>();
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Combine(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
	}

	private void Update()
	{
		if (bot.AbleToAttack(range, cooldown, player))
		{
			view.RPC("RPCA_Attack", RpcTarget.All);
		}
	}

	private void Collide(Collision col, Bodypart part)
	{
		if (punching)
		{
			Player componentInParent = col.transform.GetComponentInParent<Player>();
			if ((bool)componentInParent && !ignoredPlayers.Contains(componentInParent) && componentInParent.refs.view.IsMine && !player.Ragdoll() && part.bodypartType == attackPart && !(part.rig.linearVelocity.magnitude < collisionThreshold))
			{
				view.RPC("RPCA_Hit", RpcTarget.All, componentInParent.refs.view.ViewID, componentInParent.refs.ragdoll.GetBodypartIDFromCollider(col.collider), part.rig.linearVelocity.normalized);
				StartCoroutine(IIgnorePlayer(componentInParent));
			}
		}
	}

	[PunRPC]
	public void RPCA_Hit(int viewID, int bodyPartID, Vector3 addForce)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(viewID);
		if ((bool)player)
		{
			Bodypart bodypartFromID = player.refs.ragdoll.GetBodypartFromID(bodyPartID);
			if ((bool)bodypartFromID)
			{
				bodypartFromID.rig.AddForce(addForce * knockback, ForceMode.VelocityChange);
				player.refs.ragdoll.AddForce(addForce * knockback, ForceMode.VelocityChange);
			}
			player.refs.ragdoll.Fall(fallTime);
			player.TakeDamageLocalIKnowWhatImDoing(damage);
			if ((bool)punchSFX)
			{
				punchSFX.Play(player.Center());
			}
		}
	}

	private IEnumerator IIgnorePlayer(Player p)
	{
		ignoredPlayers.Add(p);
		yield return new WaitForSeconds(0.5f);
		if ((bool)p && ignoredPlayers.Contains(p))
		{
			ignoredPlayers.Remove(p);
		}
	}

	[PunRPC]
	private void RPCA_Attack()
	{
		StartCoroutine(IBite());
		IEnumerator IBite()
		{
			bot.attacking = true;
			punching = true;
			Rigidbody rig = player.refs.ragdoll.GetBodypart(attackPart).rig;
			List<Rigidbody> additionalRigs = new List<Rigidbody>();
			for (int i = 0; i < additionalAttackPart.Count; i++)
			{
				additionalRigs.Add(player.refs.ragdoll.GetBodypart(additionalAttackPart[i]).rig);
			}
			if ((bool)swingSFX)
			{
				swingSFX.Play(bot.Center());
			}
			float c = 0f;
			while (c < 1f && (bool)bot.targetPlayer && !player.NoControl())
			{
				if (view.IsMine)
				{
					bot.ChaseTarget(bot.Center());
				}
				if ((bool)bot.targetPlayer)
				{
					Vector3 normalized = (bot.targetPlayer.Center() - rig.position).normalized;
					rig.AddForce(normalized * force * attackCurve.Evaluate(c), ForceMode.Acceleration);
					for (int j = 0; j < additionalRigs.Count; j++)
					{
						normalized = (bot.targetPlayer.Center() - additionalRigs[j].position).normalized;
						additionalRigs[j].AddForce(normalized * force * additionalAttackForceMultiplier * attackCurve.Evaluate(c), ForceMode.Acceleration);
					}
				}
				c += Time.deltaTime;
				yield return null;
			}
			bot.attacking = false;
			punching = false;
		}
	}
}
