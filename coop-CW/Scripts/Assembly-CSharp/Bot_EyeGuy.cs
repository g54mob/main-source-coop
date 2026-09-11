using Photon.Pun;
using UnityEngine;

public class Bot_EyeGuy : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	private Transform head;

	private float exhaustion;

	private float fleeFor;

	private float hasFledFor;

	private Vector3 fleeFromPoint;

	public float exhastionTime = 20f;

	public float fleeForSeconds = 20f;

	private float eyeAggro;

	public SFX_Instance aggroSound;

	public SFX_Instance aggroSound2;

	private Material eyeMat;

	public SkinnedMeshRenderer renderer;

	private float sfxCounter = 10f;

	private float sfxCounter2 = 10f;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		eyeMat = renderer.materials[1];
		head = player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.transform;
	}

	private void Update()
	{
		sfxCounter += Time.deltaTime;
		sfxCounter2 += Time.deltaTime;
		if (eyeAggro > 0.1f)
		{
			if (eyeAggro > 0.9f)
			{
				if (sfxCounter2 > 3f)
				{
					sfxCounter2 = 0f;
					aggroSound2.Play(bot.Center());
				}
			}
			else if (sfxCounter > 2f)
			{
				sfxCounter = 0f;
				aggroSound.Play(bot.Center());
			}
		}
		if (bot.sinceFlashLit_PlayerLight < 0.1f)
		{
			eyeAggro = Mathf.MoveTowards(eyeAggro, 1f, Time.deltaTime * 2f);
		}
		else
		{
			eyeAggro = Mathf.MoveTowards(eyeAggro, 0f, Time.deltaTime * 0.3f);
		}
		eyeMat.SetFloat("_Eye", eyeAggro);
		if (!view.IsMine || bot.BusyOrAttacking())
		{
			return;
		}
		bot.slowDownWhenNavigating = true;
		if (fleeFor > 0f)
		{
			player.data.currentStamina = player.refs.controller.maxStamina;
			fleeFor -= Time.deltaTime;
			if (bot.Patrol(look: true, walk: true, 6f, listenToNoise: false, (bot.Center() - fleeFromPoint).normalized, alertable: false) && (bool)bot.lastPatrolPoint)
			{
				fleeFromPoint = bot.lastPatrolPoint.transform.position;
			}
			bot.syncData.sprint = true;
			bot.moveSpeedMultiplier = 2f;
			hasFledFor += Time.deltaTime;
			if (hasFledFor > 3f)
			{
				bot.LookForTarget(bot.Center(), 5f, 120f);
			}
			return;
		}
		bot.moveSpeedMultiplier = 1f;
		hasFledFor = 0f;
		if (!bot.aggro)
		{
			bot.LookForTarget(bot.Center(), 30f, 500f, 0f);
		}
		if (eyeAggro > 0.99f && (bool)bot.targetPlayer && !bot.aggro)
		{
			bot.aggro = true;
		}
		if (bot.aggro)
		{
			if ((bool)bot.targetPlayer)
			{
				exhaustion += Time.deltaTime;
				if (exhaustion > exhastionTime)
				{
					fleeFromPoint = bot.targetPlayer.Center();
					fleeFor = fleeForSeconds;
					bot.LoseTarget();
					exhaustion = 0f;
					return;
				}
			}
			Combat();
		}
		else if (Seen() && !LitByLevelLight())
		{
			bot.StandStill();
		}
		else
		{
			DefaultState();
		}
	}

	private bool LitByLevelLight()
	{
		return bot.sinceFlashLit_LevelLight < 1.5f;
	}

	private bool Seen()
	{
		Player firstPlayerThatCanSeeIt;
		return PlayerHandler.instance.CanAnAlivePlayerSeePoint(bot.Center(), out firstPlayerThatCanSeeIt);
	}

	private void Combat()
	{
		if (bot.targetIsHiding || bot.targetUnReachable)
		{
			TargetIsHidingBehaviour();
			return;
		}
		bot.ChaseTarget(bot.Center(), 1.5f, 1f, lookForBetterTarget: true, 30f);
		bot.ValidateChase(bot.Center(), 6f);
	}

	private void TargetIsHidingBehaviour()
	{
		exhaustion += Time.deltaTime * 2f;
		bot.LookForBetterTarget(head.position);
		float num = 12f;
		if (bot.CanSee(head.position, bot.targetPlayer.HeadPosition(), 20f, 400f) && bot.distanceToTarget < num)
		{
			bot.StandStill();
		}
		else if (bot.distanceToTarget > num * 0.8f)
		{
			bot.LookAt(bot.targetPlayer.Center());
			bot.navTargetPos_Set = bot.lastGodNavPos;
			bot.SetMovementWorld(bot.navDirection_Read);
			bot.syncData.sprint = true;
			bot.slowDownWhenNavigating = false;
		}
		else
		{
			bot.navTargetPos_Set = bot.Center() + (bot.Center() - bot.targetPlayer.Center()).Flat().normalized;
			bot.LookAt(bot.targetPlayer.Center(), 6f);
			bot.SetMovementWorld(bot.navDirection_Read);
			bot.syncData.sprint = true;
			bot.slowDownWhenNavigating = false;
		}
	}

	private void DefaultState()
	{
		bot.Patrol(look: true, walk: true, 6f, listenToNoise: false, Vector3.zero, alertable: false);
	}
}
