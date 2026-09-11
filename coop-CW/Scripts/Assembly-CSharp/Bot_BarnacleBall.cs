using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bot_BarnacleBall : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	private float attackCounter;

	private float suckAttackFor;

	private float suckAttackCooldown;

	private SuckTrigger trigger;

	private ParticleSystem part;

	private List<Player> ignoredPlayers = new List<Player>();

	private bool punch;

	public SFX_Instance punchSFX;

	public ParticleSystem barnacleGas;

	private bool releaseGas;

	private float gasSyncCD;

	public float releaseGasFor;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		trigger = base.transform.root.GetComponentInChildren<SuckTrigger>();
		part = base.transform.root.GetComponentInChildren<ParticleSystem>();
		suckAttackCooldown = UnityEngine.Random.Range(0f, 6f);
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Combine(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
	}

	private void Update()
	{
		if (suckAttackFor > 0f)
		{
			suckAttackFor -= Time.deltaTime;
			bot.attackType = 2;
			if (!trigger.enabled)
			{
				part.Play();
				trigger.enabled = true;
			}
			ParticleSystem.MainModule main = part.main;
			main.simulationSpeed = 4f * (trigger.suckTime / trigger.maxSuckTimeScale);
		}
		else if (trigger.enabled)
		{
			part.Stop();
			trigger.enabled = false;
			bot.attackType = 0;
		}
		if (releaseGas && (bool)bot.targetPlayer)
		{
			if (!barnacleGas.isPlaying)
			{
				bot.attackType = 3;
				barnacleGas.Play();
			}
			barnacleGas.transform.LookAt(bot.targetPlayer.Center());
		}
		else if (barnacleGas.isPlaying)
		{
			barnacleGas.Stop();
			bot.attackType = 0;
		}
		if (!view.IsMine)
		{
			return;
		}
		gasSyncCD -= Time.deltaTime;
		releaseGasFor = Mathf.MoveTowards(releaseGasFor, -1f, Time.deltaTime);
		if (releaseGasFor > 0f && (bool)bot.targetPlayer)
		{
			TrySyncGas(val: true);
		}
		else
		{
			TrySyncGas(val: false);
		}
		attackCounter += Time.deltaTime;
		if (suckAttackFor > 0f)
		{
			bot.StandStill();
			bot.Look(player.data.lookDirectionRight, 12f);
			return;
		}
		if (bot.aggro)
		{
			TryToAttack();
		}
		if (!punch)
		{
			if (bot.aggro)
			{
				ChaseTarget();
			}
			else if ((bool)bot.targetPlayer)
			{
				Investigate();
			}
			else
			{
				DefaultState();
			}
		}
	}

	private void TrySyncGas(bool val)
	{
		if (!(gasSyncCD > 0f))
		{
			gasSyncCD = 0.5f;
			view.RPC("RPCA_SyncGas", RpcTarget.All, val);
		}
	}

	[PunRPC]
	public void RPCA_SyncGas(bool val)
	{
		releaseGas = val;
	}

	private void TryToAttack()
	{
		if (!(bot.targetPlayer == null) && !(bot.distanceToTarget > 4f) && !(attackCounter < 2f))
		{
			view.RPC("RPCA_DoTentacleAttack", RpcTarget.All, bot.targetPlayer.refs.view.ViewID);
		}
	}

	[PunRPC]
	public void RPCA_DoTentacleAttack(int targetID)
	{
		Rigidbody attackFootL;
		if ((bool)PlayerHandler.instance.TryGetPlayerFromViewID(targetID))
		{
			punch = true;
			attackFootL = player.refs.ragdoll.GetBodypart(BodypartType.Foot_L).rig;
			_ = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig;
			StartCoroutine(DoPunch());
		}
		IEnumerator DoPunch()
		{
			float c = 0f;
			while (c < 0.5f && !bot.targetIsHiding)
			{
				if ((bool)bot.targetPlayer)
				{
					attackFootL.AddForce((bot.targetPlayer.Center() - attackFootL.position).normalized * Time.deltaTime * 8000f, ForceMode.Acceleration);
				}
				c += Time.deltaTime;
				if ((bool)bot.targetPlayer)
				{
					bot.ChaseTarget(player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.position, 0.5f);
				}
				bot.attackType = 1;
				yield return null;
			}
			punch = false;
			bot.attackType = 0;
		}
	}

	private void Collide(Collision col, Bodypart part)
	{
		if (!(base.transform.root == col.transform.root))
		{
			Player componentInParent = col.transform.GetComponentInParent<Player>();
			if ((bool)componentInParent && !ignoredPlayers.Contains(componentInParent) && !componentInParent.ai && !componentInParent.data.dead && componentInParent.refs.view.IsMine && (part.bodypartType == BodypartType.Hand_L || part.bodypartType == BodypartType.Hand_R || part.bodypartType == BodypartType.Foot_L || part.bodypartType == BodypartType.Foot_R) && !(part.rig.linearVelocity.magnitude < 5f))
			{
				view.RPC("RPCA_TentacleHit", RpcTarget.All, componentInParent.refs.view.ViewID, componentInParent.refs.ragdoll.GetBodypartIDFromCollider(col.collider), part.rig.linearVelocity.normalized);
				StartCoroutine(IIgnorePlayer(componentInParent));
			}
		}
	}

	[PunRPC]
	public void RPCA_TentacleHit(int viewID, int bodyPartID, Vector3 force)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(viewID);
		if ((bool)player)
		{
			Bodypart bodypartFromID = player.refs.ragdoll.GetBodypartFromID(bodyPartID);
			if ((bool)bodypartFromID)
			{
				bodypartFromID.rig.AddForce(force * 20f, ForceMode.VelocityChange);
				player.refs.ragdoll.AddForce(force * 20f, ForceMode.VelocityChange);
			}
			player.TakeDamageLocalIKnowWhatImDoing(51f);
			player.refs.ragdoll.Fall(2f);
			punchSFX.Play(player.Center(), player.data.isLocal);
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
	public void RPCA_DoSuckAttack()
	{
		suckAttackFor = 10f;
	}

	private void ChaseTarget()
	{
		if (bot.targetIsHiding || bot.targetUnReachable)
		{
			bot.LookForBetterTarget(bot.Center());
			float num = 14f;
			if (bot.distanceToTarget > num * 0.8f)
			{
				bot.LookAt(bot.targetPlayer.Center());
				bot.navTargetPos_Set = bot.lastGodNavPos;
				bot.SetMovementWorld(bot.navDirection_Read);
				bot.syncData.sprint = true;
				bot.slowDownWhenNavigating = false;
			}
			else
			{
				releaseGasFor = Mathf.MoveTowards(releaseGasFor, 1f, Time.deltaTime * 2f);
				bot.StandStill();
				bot.LookAt(bot.targetPlayer.Center());
			}
		}
		else
		{
			bot.ChaseTarget(bot.Center(), 0.8f, 1f, lookForBetterTarget: true, 6f, loseInterestIfUnreachable: true);
			bot.ValidateChase(bot.Center(), 7f);
			suckAttackCooldown -= Time.deltaTime;
			if (bot.distanceToTarget < 10f && suckAttackCooldown < 0f && bot.CanSeeTarget(bot.Center()))
			{
				suckAttackCooldown = 30f;
				view.RPC("RPCA_DoSuckAttack", RpcTarget.All);
			}
		}
	}

	private void Investigate()
	{
		bot.InvestigateCurrentTarget(bot.Center());
	}

	private void DefaultState()
	{
		bot.Patrol();
		bot.LookForTarget(bot.Center());
	}
}
