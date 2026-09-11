using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;

public class Bot_Ear : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	private bool fleeing;

	private float sinceStartFlee = 10f;

	private Vector3 fleeFromPos;

	private float hurtAmount;

	private bool otherClientsDisplayHurt;

	private float sinceHurtSync = 10f;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
	}

	private void Update()
	{
		if (!view.IsMine || player.NoControl())
		{
			return;
		}
		sinceHurtSync += Time.deltaTime;
		if (sinceHurtSync > 0.5f && otherClientsDisplayHurt != bot.hurt)
		{
			otherClientsDisplayHurt = bot.hurt;
			sinceHurtSync = 0f;
			view.RPC("RPCA_EarSetHurt", RpcTarget.All, bot.hurt);
		}
		if (fleeing)
		{
			if (bot.Patrol(look: true, walk: true, 10f, listenToNoise: false, bot.Center() - fleeFromPos, alertable: false) && (bool)bot.lastPatrolPoint)
			{
				fleeFromPos = bot.lastPatrolPoint.transform.position;
			}
			bot.syncData.sprint = true;
			sinceStartFlee += Time.deltaTime;
			if (sinceStartFlee > 3f)
			{
				bot.hurt = false;
			}
			if (sinceStartFlee > 30f)
			{
				view.RPC("RPCA_EarStopFlee", RpcTarget.All);
			}
			return;
		}
		if (PlayerHandler.instance.PlayerVoiceVolumeAtPosition(bot.Center(), 3f, 15f) > 0.7f)
		{
			hurtAmount = Mathf.MoveTowards(hurtAmount, 1f, Time.deltaTime * 0.8f);
			bot.hurt = true;
		}
		else
		{
			hurtAmount = Mathf.MoveTowards(hurtAmount, 0f, Time.deltaTime * 0.3f);
			bot.hurt = false;
		}
		if (hurtAmount > 0.99f)
		{
			view.RPC("RPCA_EarFlee", RpcTarget.All);
		}
		else if (bot.aggro)
		{
			Combat();
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

	[PunRPC]
	private void RPCA_EarSetHurt(bool setHurt)
	{
		bot.hurt = setHurt;
	}

	[PunRPC]
	private void RPCA_EarStopFlee()
	{
		fleeing = false;
	}

	[PunRPC]
	private void RPCA_EarFlee()
	{
		sinceStartFlee = 0f;
		fleeing = true;
		bot.LoseTarget();
		if ((bool)bot.targetPlayer)
		{
			fleeFromPos = bot.targetPlayer.Center();
		}
		else
		{
			fleeFromPos = bot.Center() + bot.syncData.lookDireciton * 2f;
		}
		hurtAmount = 0f;
		PlatformManager.UnlockAchievement(Achievements.ACH_HURT_EAR);
	}

	private void Combat()
	{
		bot.ChaseTarget(bot.Center(), 3f, 1f, lookForBetterTarget: true, 6f, loseInterestIfUnreachable: true);
		bot.ValidateChase(bot.Center());
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
