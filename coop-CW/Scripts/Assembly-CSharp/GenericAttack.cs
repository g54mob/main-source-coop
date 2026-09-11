using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GenericAttack : MonoBehaviour
{
	private Player player;

	private PhotonView view;

	private bool punching;

	public float damage = 35f;

	public float knockback = 5f;

	public float fallTime = 1f;

	public AnimationCurve attackCurve;

	public float force;

	public float collisionThreshold = 3f;

	public BodypartType attackPart;

	private List<Player> ignoredPlayers = new List<Player>();

	public SFX_Instance swingSFX;

	public SFX_Instance punchSFX;

	public List<BodypartType> additionalAttackPart = new List<BodypartType>();

	public float additionalAttackForceMultiplier = 0.5f;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Combine(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
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

	public void CallAttack(Player target)
	{
		view.RPC("RPCA_Attack", RpcTarget.All, target.refs.view.ViewID);
	}

	[PunRPC]
	private void RPCA_Attack(int targetID)
	{
		Player target = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		if ((bool)target)
		{
			StartCoroutine(IBite());
		}
		IEnumerator IBite()
		{
			punching = true;
			Rigidbody rig = player.refs.ragdoll.GetBodypart(attackPart).rig;
			Rigidbody targetRig = target.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
			List<Rigidbody> additionalRigs = new List<Rigidbody>();
			for (int i = 0; i < additionalAttackPart.Count; i++)
			{
				additionalRigs.Add(player.refs.ragdoll.GetBodypart(additionalAttackPart[i]).rig);
			}
			if ((bool)swingSFX)
			{
				swingSFX.Play(player.Center());
			}
			float c = 0f;
			while (c < 1f && (bool)target && !player.NoControl())
			{
				Vector3 normalized = (targetRig.position - rig.position).normalized;
				rig.AddForce(normalized * force * attackCurve.Evaluate(c), ForceMode.Acceleration);
				for (int j = 0; j < additionalRigs.Count; j++)
				{
					normalized = (targetRig.position - additionalRigs[j].position).normalized;
					rig.AddForce(normalized * force * additionalAttackForceMultiplier * attackCurve.Evaluate(c), ForceMode.Acceleration);
				}
				c += Time.deltaTime;
				yield return null;
			}
			punching = false;
		}
	}
}
