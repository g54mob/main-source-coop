using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;

public class Bot_Jelly : MonoBehaviour
{
	private Player player;

	internal Bot bot;

	public Player jellyPlayer;

	public bool fleeing;

	public PhotonView view;

	public Vector3 moveAwayPos;

	internal float sinceCapture;

	private float sinceStartFlee;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
	}

	private void Update()
	{
		sinceCapture += Time.deltaTime;
		if (!view.IsMine)
		{
			return;
		}
		bot.moveSpeedMultiplier = 1f;
		sinceStartFlee += Time.deltaTime;
		if (fleeing)
		{
			if (sinceStartFlee > 15f)
			{
				fleeing = false;
				return;
			}
			if (bot.Patrol(look: true, walk: true, 10f, listenToNoise: false, bot.Center() - moveAwayPos, alertable: false) && (bool)bot.lastPatrolPoint)
			{
				moveAwayPos = bot.lastPatrolPoint.transform.position;
			}
			bot.syncData.sprint = true;
		}
		else if ((bool)jellyPlayer)
		{
			if (sinceCapture > 10f || jellyPlayer.data.sinceRescueDragged < 0.5f)
			{
				view.RPC("RPCA_DropAndFlee", RpcTarget.All);
				return;
			}
			if (bot.Patrol(look: true, walk: true, 10f, listenToNoise: false, bot.Center() - moveAwayPos, alertable: false) && (bool)bot.lastPatrolPoint)
			{
				moveAwayPos = bot.lastPatrolPoint.transform.position;
			}
			bot.syncData.sprint = true;
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
	private void RPCA_DropAndFlee()
	{
		fleeing = true;
		jellyPlayer = null;
		sinceStartFlee = 0f;
		bot.attacking = false;
	}

	[PunRPC]
	public void RPCA_SetJelloTarget(int setTarget)
	{
		jellyPlayer = PlayerHandler.instance.TryGetPlayerFromViewID(setTarget);
		if ((bool)jellyPlayer)
		{
			bot.LoseTarget();
			sinceCapture = 0f;
			moveAwayPos = bot.Center() - bot.syncData.lookDireciton;
			bot.attacking = true;
			if (jellyPlayer.IsLocal)
			{
				PlatformManager.UnlockAchievement(Achievements.ACH_JELLO);
			}
		}
	}

	private void Combat()
	{
		bot.ChaseTarget(bot.Center(), 0.8f, 1f, lookForBetterTarget: true, 6f, loseInterestIfUnreachable: true);
		bot.ValidateChase(bot.Center());
		bot.moveSpeedMultiplier = 0.5f;
	}

	private void Investigate()
	{
		bot.InvestigateCurrentTarget(bot.Center());
	}

	private void DefaultState()
	{
		bot.Patrol();
		bot.LookForTarget(bot.Center());
		bot.syncData.sprint = false;
	}
}
