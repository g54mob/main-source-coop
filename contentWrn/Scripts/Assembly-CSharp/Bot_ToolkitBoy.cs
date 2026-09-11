using System;
using Photon.Pun;
using UnityEngine;

public class Bot_ToolkitBoy : MonoBehaviour
{
	private Player player;

	public Bot bot;

	private PhotonView view;

	public float targetDistance = 0.8f;

	private float chargeCounter = 5f;

	public bool isCharging;

	private float exhaustion;

	private float fleeFor;

	private float hasFledFor;

	private Vector3 fleeFromPoint;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Combine(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
	}

	private void Collide(Collision collision, Bodypart bodypart)
	{
		if (!collision.rigidbody && (collision.collider.gameObject.layer == 9 || collision.collider.gameObject.layer == 10) && view.IsMine && bodypart.bodypartType == BodypartType.Head && isCharging)
		{
			view.RPC("RPCA_BonkTool", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_BonkTool()
	{
		player.refs.ragdoll.Fall(2f);
		isCharging = false;
		chargeCounter = 0f;
		GamefeelHandler.instance.perlin.AddShake(bot.Center(), 5f, 0.4f, 15f, 20f);
	}

	private void Update()
	{
		if (player.data.fallTime > 0f)
		{
			bot.hurt = true;
		}
		else
		{
			bot.hurt = false;
		}
		if (!view.IsMine)
		{
			return;
		}
		bot.slowDownWhenNavigating = true;
		if (player.data.fallTime < 0.01f && bot.aggro)
		{
			chargeCounter = Mathf.MoveTowards(chargeCounter, 5.5f, Time.deltaTime);
		}
		else
		{
			chargeCounter = Mathf.MoveTowards(chargeCounter, 0f, Time.deltaTime);
		}
		if (fleeFor > 0f)
		{
			fleeFor -= Time.deltaTime;
			if (bot.Patrol(look: true, walk: true, 10f, listenToNoise: false, (bot.Center() - fleeFromPoint).normalized, alertable: false) && (bool)bot.lastPatrolPoint)
			{
				fleeFromPoint = bot.lastPatrolPoint.transform.position;
			}
			bot.syncData.sprint = true;
			if (player.data.fallTime < 0.1f)
			{
				hasFledFor += Time.deltaTime;
			}
			return;
		}
		hasFledFor = 0f;
		if ((bool)bot.targetPlayer)
		{
			exhaustion += Time.deltaTime;
			if (exhaustion > 20f && isCharging && player.data.fallTime < 0.1f)
			{
				exhaustion = 0f;
				fleeFromPoint = bot.targetPlayer.Center();
				fleeFor = 15f;
				bot.LoseTarget();
				return;
			}
		}
		if ((bool)bot.targetPlayer)
		{
			if (bot.aggro)
			{
				Combat();
			}
			else
			{
				Investigate();
			}
		}
		else
		{
			DefaultState();
		}
	}

	private void SetCharging(bool setCh)
	{
		if (setCh != isCharging)
		{
			view.RPC("RPCA_SetCharging", RpcTarget.All, setCh);
		}
	}

	[PunRPC]
	private void RPCA_SetCharging(bool setCh)
	{
		isCharging = setCh;
	}

	private void Combat()
	{
		if (isCharging)
		{
			bot.syncData.movementInput = new Vector2(0f, 1f);
			bot.syncData.sprint = true;
			bot.slowDownWhenNavigating = false;
			if (chargeCounter > 3f)
			{
				SetCharging(setCh: false);
				chargeCounter = 0f;
			}
		}
		else
		{
			if (chargeCounter > 2f && bot.CanSeeTarget(bot.Center()))
			{
				SetCharging(setCh: true);
				chargeCounter = 0f;
			}
			bot.StandStill();
			bot.LookAt(bot.targetPlayer.Center(), 10f);
			bot.ValidateChase(bot.Center(), 2.5f);
		}
	}

	private void Investigate()
	{
		bot.InvestigateCurrentTarget(bot.Center());
		bot.StandStill();
	}

	private void DefaultState()
	{
		bot.syncData.sprint = false;
		bot.Patrol();
		bot.LookForTarget(bot.Center());
	}
}
