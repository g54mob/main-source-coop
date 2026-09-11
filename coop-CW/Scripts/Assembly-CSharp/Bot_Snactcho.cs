using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Bot_Snactcho : MonoBehaviour
{
	private static readonly int Fade = Shader.PropertyToID("_Fade");

	private Player player;

	private Bot bot;

	private PhotonView view;

	internal float sinceCapture;

	public Vector3 moveAwayPos;

	public Player snatchedPlayer;

	private float litLevel;

	private float litLevelInMat;

	private SkinnedMeshRenderer mr;

	private Material mat;

	private Transform headTrans;

	private float takeDamageCounter;

	private float tpCounter;

	private Joint joint;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		bot.slowDownWhenNavigating = false;
		mr = player.refs.bodyMeshRenderer;
		mat = mr.material;
		headTrans = player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.transform;
	}

	private void Update()
	{
		bot.moveSpeedMultiplier = 1f;
		sinceCapture += Time.deltaTime;
		tpCounter += Time.deltaTime;
		if ((bool)snatchedPlayer)
		{
			Snatching();
		}
		if (CheckForLight() || !view.IsMine)
		{
			return;
		}
		if (bot.Center().y < -100f)
		{
			ReturnToMap();
		}
		if (!snatchedPlayer)
		{
			if (bot.aggro)
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
	}

	private bool CheckForLight()
	{
		if (bot.sinceFlashLit < 0.2f)
		{
			litLevel = Mathf.MoveTowards(litLevel, 1f, Time.deltaTime);
			if (litLevel > 0.3f)
			{
				TeleportAway();
				return true;
			}
		}
		else
		{
			litLevel = Mathf.MoveTowards(litLevel, 0f, Time.deltaTime);
		}
		if (litLevel != litLevelInMat)
		{
			litLevelInMat = litLevel;
			float value = 1f - litLevel * 5f;
			mat.SetFloat(Fade, value);
		}
		return false;
	}

	private void ReturnToMap()
	{
		if (!(tpCounter < 1f))
		{
			tpCounter = 0f;
			PatrolPoint freePointWithDistance = Level.currentLevel.GetFreePointWithDistance(new List<PatrolPoint.PatrolGroup> { PatrolPoint.PatrolGroup.Bear }, bot.Center(), 30, 100000000f);
			if ((bool)freePointWithDistance)
			{
				view.RPC("RPCA_TeleportAway", RpcTarget.All, freePointWithDistance.transform.position);
			}
		}
	}

	private void TeleportAway()
	{
		if (!(tpCounter < 1f))
		{
			tpCounter = 0f;
			PatrolPoint freePointWithDistance = Level.currentLevel.GetFreePointWithDistance(new List<PatrolPoint.PatrolGroup> { PatrolPoint.PatrolGroup.Bear }, bot.Center(), 30, 20f);
			if ((bool)freePointWithDistance)
			{
				view.RPC("RPCA_TeleportAway", RpcTarget.All, freePointWithDistance.transform.position);
			}
		}
	}

	[PunRPC]
	private void RPCA_TeleportAway(Vector3 targetPos)
	{
		if ((bool)joint)
		{
			Object.Destroy(joint);
		}
		snatchedPlayer = null;
		bot.LoseTarget();
		bot.syncData.sprint = false;
		StartCoroutine(ITeleportAway());
		IEnumerator ITeleportAway()
		{
			yield return null;
			yield return new WaitForFixedUpdate();
			yield return null;
			player.refs.ragdoll.ExtraDrag(0f);
			player.MoveAllRigsInDirection(targetPos - bot.Center() + Vector3.up);
			litLevel = -1f;
		}
	}

	private void Snatching()
	{
		if (sinceCapture > 20f || snatchedPlayer == null || snatchedPlayer.data.dead || snatchedPlayer.data.sinceRescueDragged < 0.5f)
		{
			if (view.IsMine)
			{
				TeleportAway();
			}
			return;
		}
		takeDamageCounter += Time.deltaTime;
		if (takeDamageCounter > 1f)
		{
			if (snatchedPlayer.IsLocal)
			{
				snatchedPlayer.CallTakeDamage(6f);
			}
			takeDamageCounter = 0f;
		}
		bot.moveSpeedMultiplier = 0.6f;
		snatchedPlayer.data.dropItemsFor = 0.5f;
		snatchedPlayer.refs.ragdoll.AddForce((headTrans.position - snatchedPlayer.Center()) * 30f, ForceMode.Acceleration);
		bot.Patrol(look: false, walk: true, 10f, listenToNoise: false, bot.Center() - moveAwayPos, alertable: false);
		bot.LookAt(snatchedPlayer.Center());
		bot.SetMovementWorld(bot.navDirection_Read);
		bot.syncData.sprint = false;
	}

	[PunRPC]
	public void RPCA_SetSnatchTarget(int setTarget)
	{
		snatchedPlayer = PlayerHandler.instance.TryGetPlayerFromViewID(setTarget);
		if ((bool)snatchedPlayer)
		{
			snatchedPlayer.data.isSnatched = true;
			bot.LoseTarget();
			sinceCapture = 0f;
			moveAwayPos = bot.Center() - bot.syncData.lookDireciton;
			Rigidbody rig = snatchedPlayer.refs.ragdoll.GetBodypart(BodypartType.Hip).rig;
			Rigidbody rig2 = player.refs.ragdoll.GetBodypart(BodypartType.Head).rig;
			headTrans.transform.position += rig.worldCenterOfMass - rig2.worldCenterOfMass;
			joint = rig2.gameObject.AddComponent<FixedJoint>();
			joint.connectedBody = rig;
		}
	}

	private void DoKilling()
	{
		if (bot.sinceFlashLit < 0.3f)
		{
			litLevel = Mathf.MoveTowards(litLevel, 1f, Time.deltaTime);
			_ = litLevel;
			_ = 0.3f;
		}
		else
		{
			litLevel = Mathf.MoveTowards(litLevel, 0f, Time.deltaTime);
		}
	}

	private void Combat()
	{
		bot.ChaseTarget(bot.Center(), 0.8f, 1f, lookForBetterTarget: true, 10f, loseInterestIfUnreachable: true);
		bot.ValidateChase(bot.Center(), 3.5f);
		if (bot.distanceToTarget < 2f)
		{
			view.RPC("RPCA_SetSnatchTarget", RpcTarget.All, bot.targetPlayer.refs.view.ViewID);
		}
	}

	private void Investigate()
	{
		bot.InvestigateCurrentTarget(bot.Center());
	}

	private void DefaultState()
	{
		bot.Patrol(look: true, walk: true, 15f);
		bot.LookForTarget(bot.Center());
	}
}
