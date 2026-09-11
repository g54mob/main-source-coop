using Photon.Pun;
using UnityEngine;

public class Bot_Ghost : MonoBehaviour
{
	public Material defaultMat;

	public Material frenzyMat;

	private SkinnedMeshRenderer skinnedMeshRenderer;

	public Player frenzyTarget;

	public Hazard targetHazard;

	private MonsterAnimationHandler monsterAnimationHandler;

	private PhotonView view;

	private Player player;

	private Bot bot;

	private float sinceDrop;

	private float exhaustion;

	private float sinceConnect;

	private float sinceFrenzyStart = 10f;

	private float hoverScale = 1f;

	private float sideMove;

	private float blindedValue;

	private bool hurt;

	private bool fleeing;

	private float sinceStartFlee;

	private bool displayFrensy;

	private Joint rJoint;

	private Joint lJoint;

	private void Start()
	{
		skinnedMeshRenderer = base.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
		monsterAnimationHandler = base.transform.root.GetComponent<MonsterAnimationHandler>();
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		view = base.transform.GetComponent<PhotonView>();
		hoverScale = Random.Range(0.8f, 1.2f);
		sideMove = Random.Range(-1f, 1f);
	}

	private void Update()
	{
		if (!view.IsMine)
		{
			return;
		}
		bot.hurt = false;
		ResetVariables();
		UpdateVariables();
		if (fleeing)
		{
			if (sinceStartFlee > 15f)
			{
				bot.LoseTarget();
				fleeing = false;
			}
			bot.hurt = true;
			if ((bool)bot.targetPlayer)
			{
				Vector3 normalized = (base.transform.position - bot.targetPlayer.Center()).normalized;
				bot.Look(normalized);
				bot.SetMovementWorld(normalized);
				bot.syncData.sprint = true;
			}
		}
		else if ((bool)frenzyTarget)
		{
			FrenzyAI();
		}
		else
		{
			HandleBlinding();
			if ((bool)rJoint || (bool)lJoint)
			{
				view.RPC("RPCA_Drop", RpcTarget.All);
			}
			if ((bool)bot.targetPlayer)
			{
				HauntPlayer();
				return;
			}
			bot.Patrol();
			bot.LookForTarget(base.transform.position, 20f);
		}
	}

	private void HandleBlinding()
	{
		if (bot.sinceFlashLit < 0.3f)
		{
			blindedValue = Mathf.MoveTowards(blindedValue, 1f, Time.deltaTime);
			bot.StandStill();
			if (blindedValue > 0.99f)
			{
				StartFlee();
			}
		}
		else
		{
			blindedValue = Mathf.MoveTowards(blindedValue, 0f, Time.deltaTime);
		}
	}

	private void HauntPlayer()
	{
		bot.syncData.sprint = false;
		bot.KeepDistanceHover(bot.targetPlayer.Center(), 10f + Mathf.Cos(Time.time * hoverScale) * 2f, sideMove);
		if (blindedValue > 0.1f)
		{
			return;
		}
		Hazard nearbyHazard = HazardHandler.instance.GetNearbyHazard(30f, base.transform.position);
		if ((bool)nearbyHazard)
		{
			bot.ChaseTarget(base.transform.position, 0f, 1f, lookForBetterTarget: false, 10f);
			if (bot.distanceToTarget < 1f)
			{
				CallOutFrenzy(bot.targetPlayer, nearbyHazard);
			}
		}
	}

	private void StartFlee()
	{
		fleeing = true;
		sinceStartFlee = 0f;
	}

	public void CallOutFrenzy(Player targetPlayer, Hazard setHazard)
	{
		for (int i = 0; i < BotHandler.instance.bots.Count; i++)
		{
			Bot_Ghost componentInChildren = BotHandler.instance.bots[i].GetComponentInChildren<Bot_Ghost>();
			if ((bool)componentInChildren && HelperFunctions.FlatDistance(base.transform.position, componentInChildren.transform.position) < 60f)
			{
				componentInChildren.StartFrenzy(targetPlayer, setHazard);
			}
		}
	}

	private void StartFrenzy(Player targetPlayer, Hazard setHazard)
	{
		frenzyTarget = targetPlayer;
		targetHazard = setHazard;
		sinceFrenzyStart = 0f;
	}

	private void FrenzyAI()
	{
		bot.targetPlayer = frenzyTarget;
		bot.syncData.targetPlayerId = frenzyTarget.refs.view.ViewID;
		if ((HelperFunctions.FlatDistance(base.transform.position, targetHazard.transform.position) > 40f || bot.distanceToTarget > 15f || bot.targetPlayer.data.dead) && sinceFrenzyStart > 3f)
		{
			frenzyTarget = null;
			targetHazard = null;
			bot.LoseTarget();
			return;
		}
		bot.syncData.sprint = true;
		if ((bool)rJoint && (bool)lJoint)
		{
			Pull(targetHazard.transform.position);
			return;
		}
		bot.ChaseTarget(base.transform.position, 1f, 1f, lookForBetterTarget: false, 10f);
		if (bot.distanceToTarget < 3f && sinceDrop > 3f)
		{
			TryToGrabPlayer();
		}
		if (sinceConnect > 2f && ((bool)lJoint || (bool)rJoint))
		{
			view.RPC("RPCA_Drop", RpcTarget.All);
		}
	}

	private void ResetVariables()
	{
		ResetIK();
	}

	private void UpdateVariables()
	{
		sinceFrenzyStart += Time.deltaTime;
		sinceDrop += Time.deltaTime;
		sinceConnect += Time.deltaTime;
		sinceStartFlee += Time.deltaTime;
		if (hurt)
		{
			bot.hurt = true;
		}
		if (displayFrensy != (bool)frenzyTarget)
		{
			view.RPC("RPCA_DisplayFrenzy", RpcTarget.All, frenzyTarget != null);
		}
		bool flag = blindedValue > 0.01f || fleeing;
		if (hurt != flag)
		{
			view.RPC("RPCA_DisplayBlinded", RpcTarget.All, flag);
		}
	}

	[PunRPC]
	private void RPCA_DisplayBlinded(bool setTrue)
	{
		hurt = setTrue;
	}

	[PunRPC]
	private void RPCA_DisplayFrenzy(bool setTrue)
	{
		displayFrensy = setTrue;
		if (setTrue)
		{
			skinnedMeshRenderer.sharedMaterial = frenzyMat;
		}
		else
		{
			skinnedMeshRenderer.sharedMaterial = defaultMat;
		}
	}

	private void Pull(Vector3 pos)
	{
		bot.navTargetPos_Set = pos;
		bot.Look(-bot.navDirection_Read);
		bot.syncData.movementInput = new Vector2(0f, -1f);
		if (Vector3.Angle(player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.linearVelocity, bot.navDirection_Read) > 90f)
		{
			exhaustion = Mathf.MoveTowards(exhaustion, 1f, Time.deltaTime * 0.3f);
			if (exhaustion > 0.99f)
			{
				view.RPC("RPCA_Drop", RpcTarget.All);
			}
		}
		else
		{
			exhaustion = Mathf.MoveTowards(exhaustion, 0f, Time.deltaTime * 0.3f);
		}
	}

	[PunRPC]
	private void RPCA_Drop()
	{
		sinceDrop = 0f;
		if ((bool)lJoint)
		{
			Object.Destroy(lJoint);
		}
		if ((bool)rJoint)
		{
			Object.Destroy(rJoint);
		}
	}

	private void TryToGrabPlayer()
	{
		DoReachIK();
		if (!lJoint)
		{
			TryConnectLeft();
		}
		if (!rJoint)
		{
			TryConnectRight();
		}
	}

	private void TryConnectRight()
	{
		if (Vector3.Distance(player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig.position, bot.targetPlayer.Center()) < 1f)
		{
			view.RPC("RPCA_ConnectJoint", RpcTarget.All, bot.targetPlayer.refs.view.ViewID, true);
		}
	}

	private void TryConnectLeft()
	{
		if (Vector3.Distance(player.refs.ragdoll.GetBodypart(BodypartType.Hand_L).rig.position, bot.targetPlayer.Center()) < 1f)
		{
			view.RPC("RPCA_ConnectJoint", RpcTarget.All, bot.targetPlayer.refs.view.ViewID, false);
		}
	}

	[PunRPC]
	private void RPCA_ConnectJoint(int targetID, bool rightHand)
	{
		sinceConnect = 0f;
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		if ((bool)player)
		{
			Bodypart bodypart = (rightHand ? this.player.refs.ragdoll.GetBodypart(BodypartType.Hand_R) : this.player.refs.ragdoll.GetBodypart(BodypartType.Hand_L));
			bodypart.transform.position = player.Center();
			if (rightHand)
			{
				rJoint = HelperFunctions.AttachPositionJoint(bodypart.rig, player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig);
			}
			else
			{
				lJoint = HelperFunctions.AttachPositionJoint(bodypart.rig, player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig);
			}
		}
	}

	private void ResetIK()
	{
		player.refs.IK_Right.weight = 0f;
		player.refs.IK_Left.weight = 0f;
	}

	private void DoReachIK()
	{
		player.refs.IK_Right.weight = 1f;
		player.refs.IK_Left.weight = 1f;
		Vector3 upwards = bot.targetPlayer.Center() - player.Center();
		player.refs.ikHandler.SetRightHandPosition(bot.targetPlayer.Center(), Quaternion.LookRotation(player.data.lookDirectionRight, upwards));
		player.refs.ikHandler.SetLeftHandPosition(bot.targetPlayer.Center(), Quaternion.LookRotation(-player.data.lookDirectionRight, upwards));
	}
}
