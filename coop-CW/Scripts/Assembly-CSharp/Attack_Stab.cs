using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Attack_Stab : MonoBehaviour
{
	public float forceM = 1f;

	public float range = 4f;

	public BodypartType mainRig = BodypartType.Torso;

	private Player player;

	private Bot bot;

	private PhotonView view;

	internal MonsterAnimationValues val;

	private List<Player> ignoredPlayers = new List<Player>();

	public SFX_Instance punchSFX;

	private bool punch;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		view = base.transform.GetComponent<PhotonView>();
		val = base.transform.root.Find("AnimationRig").GetComponentInChildren<MonsterAnimationValues>();
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Combine(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
	}

	private void Update()
	{
		if (view.IsMine && !bot.BusyOrAttacking() && bot.aggro && !(bot.distanceToTarget > range) && !(bot.sinceAttack < 2.5f) && !player.NoControl())
		{
			Attack();
		}
	}

	private void Collide(Collision col, Bodypart part)
	{
		if (punch)
		{
			Player componentInParent = col.transform.GetComponentInParent<Player>();
			if ((bool)componentInParent && !ignoredPlayers.Contains(componentInParent) && componentInParent.refs.view.IsMine && !componentInParent.Ragdoll() && part.bodypartType == BodypartType.Elbow_L && !(part.rig.linearVelocity.magnitude < 3f))
			{
				view.RPC("RPCA_Hit", RpcTarget.All, componentInParent.refs.view.ViewID, componentInParent.refs.ragdoll.GetBodypartIDFromCollider(col.collider), part.rig.linearVelocity.normalized);
				StartCoroutine(IIgnorePlayer(componentInParent));
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
	public void RPCA_Hit(int viewID, int bodyPartID, Vector3 force)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(viewID);
		if ((bool)player)
		{
			Bodypart bodypartFromID = player.refs.ragdoll.GetBodypartFromID(bodyPartID);
			if ((bool)bodypartFromID)
			{
				bodypartFromID.rig.AddForce(force * 5f, ForceMode.VelocityChange);
				player.refs.ragdoll.AddForce(force * 5f, ForceMode.VelocityChange);
			}
			player.refs.ragdoll.Fall(1f);
			player.TakeDamageLocalIKnowWhatImDoing(35f);
			if ((bool)punchSFX)
			{
				punchSFX.Play(player.Center());
			}
		}
	}

	private void Attack()
	{
		view.RPC("RPCA_Punch", RpcTarget.All);
	}

	[PunRPC]
	private void RPCA_Punch()
	{
		Attack_Grab componentInChildren = base.transform.root.GetComponentInChildren<Attack_Grab>();
		if ((bool)componentInChildren)
		{
			componentInChildren.LetGo();
		}
		punch = true;
		Rigidbody elbowL = player.refs.ragdoll.GetBodypart(BodypartType.Elbow_L).rig;
		Rigidbody torso = player.refs.ragdoll.GetBodypart(mainRig).rig;
		StartCoroutine(DoPunch());
		bot.attacking = true;
		IEnumerator DoPunch()
		{
			float c = 0f;
			while (c < 1f && !bot.hurt && !player.NoControl())
			{
				if ((bool)bot.targetPlayer)
				{
					if (val.leftPunch)
					{
						elbowL.AddForce((bot.targetPlayer.Center() - elbowL.position).normalized * Time.deltaTime * 50000f * forceM, ForceMode.Acceleration);
					}
					torso.AddForce((bot.targetPlayer.Center() - torso.position).normalized * Time.deltaTime * 10000f * forceM, ForceMode.Acceleration);
				}
				c += Time.deltaTime;
				if ((bool)bot.targetPlayer)
				{
					bot.ChaseTarget(player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.position, 2f);
				}
				yield return null;
			}
			bot.attacking = false;
			punch = false;
		}
	}
}
