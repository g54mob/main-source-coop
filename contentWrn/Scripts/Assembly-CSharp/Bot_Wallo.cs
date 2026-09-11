using Photon.Pun;
using UnityEngine;

public class Bot_Wallo : MonoBehaviour
{
	public enum WalloState
	{
		Hiding = 0,
		Reaching = 1,
		Dragging = 2,
		Stealing = 3
	}

	private WallWarpArm[] arms;

	private Bot bot;

	private Vector3 savePoint;

	private Vector3 saveNormal;

	public Animator animator;

	private float sinceSeenPlayer = 10f;

	private float hasPlayerFor;

	private PhotonView view;

	private bool intendToTPAway;

	private float intentToTPAwayFor;

	private Player targetPlayer;

	public WalloState walloState;

	private float stealFor;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		arms = GetComponentsInChildren<WallWarpArm>();
		bot = GetComponentInChildren<Bot>();
		if (view.IsMine)
		{
			GoToRandomPoint();
		}
	}

	private void SetState(WalloState setState)
	{
		if (walloState != setState)
		{
			view.RPC("RPCA_SetWalloState", RpcTarget.All, (int)setState, targetPlayer ? targetPlayer.refs.view.ViewID : (-1));
		}
	}

	[PunRPC]
	private void RPCA_SetWalloState(int setState, int targetID)
	{
		if (targetID == -1)
		{
			targetPlayer = null;
		}
		else
		{
			targetPlayer = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		}
		walloState = (WalloState)setState;
		StateChanged(walloState);
	}

	private void StateChanged(WalloState newSate)
	{
		switch (newSate)
		{
		case WalloState.Stealing:
			OnStartStealing();
			break;
		case WalloState.Reaching:
		{
			for (int i = 0; i < arms.Length; i++)
			{
				arms[i].InitTarget(targetPlayer);
			}
			animator.SetBool("Spawn", value: true);
			break;
		}
		case WalloState.Hiding:
			ResetToIdle();
			intendToTPAway = true;
			break;
		}
		walloState = newSate;
	}

	private void FixedUpdate()
	{
		if (view.IsMine)
		{
			if (intendToTPAway)
			{
				if ((bool)targetPlayer)
				{
					LoseTarget();
				}
				intentToTPAwayFor += Time.deltaTime;
				if (intentToTPAwayFor > 2f)
				{
					intendToTPAway = false;
					GoToRandomPoint();
				}
			}
			else
			{
				intentToTPAwayFor = 0f;
			}
		}
		if (view.IsMine)
		{
			if (sinceSeenPlayer > 0.2f && walloState != WalloState.Stealing)
			{
				bot.LoseTarget();
			}
			sinceSeenPlayer += Time.fixedDeltaTime;
			if (sinceSeenPlayer > 20f)
			{
				GoToRandomPoint();
				sinceSeenPlayer = 1f;
			}
		}
		if (walloState != WalloState.Hiding)
		{
			hasPlayerFor += Time.fixedDeltaTime;
		}
		if (walloState == WalloState.Stealing)
		{
			DoSteal();
		}
		else if (walloState == WalloState.Dragging)
		{
			DragPlayerIn();
		}
		else if (walloState == WalloState.Reaching)
		{
			Reaching();
		}
		else
		{
			Idle();
		}
	}

	private void LoseTarget()
	{
		view.RPC("RPCA_LoseTarget", RpcTarget.All);
	}

	[PunRPC]
	public void RPCA_LoseTarget()
	{
		targetPlayer = null;
	}

	private void DoSteal()
	{
		if (view.IsMine && !targetPlayer)
		{
			SetState(WalloState.Hiding);
			return;
		}
		for (int i = 0; i < arms.Length; i++)
		{
			arms[i].Pull(200f, base.transform.position);
		}
		stealFor += Time.fixedDeltaTime;
		if (targetPlayer.refs.view.IsMine)
		{
			BlackScreen.instance.SetBlackScreen(0.5f);
		}
		if (stealFor > 0.5f && view.IsMine)
		{
			FinishSteal();
		}
	}

	private void FinishSteal()
	{
		int num = ShadowRealmHandler.instance.TeleportPlayerToRandomRealm(targetPlayer);
		SetState(WalloState.Hiding);
		intendToTPAway = true;
		PhotonNetwork.Instantiate(data: new object[1] { num }, prefabName: "WalloArm", position: base.transform.position, rotation: base.transform.rotation, group: 0);
	}

	private void OnStartStealing()
	{
		animator.SetBool("Steal", value: true);
		targetPlayer.ToggleCollisionForSeconds(3f);
	}

	private void DragPlayerIn()
	{
		if (view.IsMine && !targetPlayer)
		{
			SetState(WalloState.Hiding);
			return;
		}
		if (view.IsMine && Vector3.Distance(base.transform.position, targetPlayer.Center()) < 3f && walloState != WalloState.Stealing)
		{
			SetState(WalloState.Stealing);
			return;
		}
		for (int i = 0; i < arms.Length; i++)
		{
			arms[i].Pull(200f, base.transform.position);
		}
	}

	private void Reaching()
	{
		for (int i = 0; i < arms.Length; i++)
		{
			arms[i].reachForPlayer = hasPlayerFor > 2.5f;
		}
		if (arms[0].reachAmount > 0.99f && (bool)targetPlayer)
		{
			if (view.IsMine)
			{
				SetState(WalloState.Dragging);
			}
			for (int j = 0; j < arms.Length; j++)
			{
				arms[j].closeHands = true;
			}
		}
		if (!targetPlayer && view.IsMine)
		{
			SetState(WalloState.Hiding);
		}
	}

	private void ResetToIdle()
	{
		walloState = WalloState.Hiding;
		for (int i = 0; i < arms.Length; i++)
		{
			arms[i].ClearTarget();
		}
		animator.SetBool("Steal", value: false);
		animator.SetBool("Spawn", value: false);
		for (int j = 0; j < arms.Length; j++)
		{
			arms[j].ClearTarget();
		}
		bot.LoseTarget();
		hasPlayerFor = 0f;
		stealFor = 0f;
		walloState = WalloState.Hiding;
	}

	private void Idle()
	{
		if (view.IsMine && (bool)targetPlayer)
		{
			SetState(WalloState.Reaching);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if ((bool)view && !other.isTrigger && !intendToTPAway)
		{
			Player componentInParent = other.GetComponentInParent<Player>();
			if ((bool)componentInParent && !componentInParent.ai && view.IsMine)
			{
				targetPlayer = componentInParent;
				sinceSeenPlayer = 0f;
			}
		}
	}

	private void GoToRandomPoint()
	{
	}

	[PunRPC]
	private void RPCA_TeleportToPoint(Vector3 targetPoint, Vector3 targetNormal)
	{
		base.transform.position = targetPoint;
		base.transform.rotation = Quaternion.LookRotation(targetNormal);
	}

	private bool EvalPoint(Vector3 point)
	{
		if (EvalPointInDirection(point, Vector3.forward))
		{
			return true;
		}
		if (EvalPointInDirection(point, -Vector3.forward))
		{
			return true;
		}
		if (EvalPointInDirection(point, Vector3.right))
		{
			return true;
		}
		if (EvalPointInDirection(point, -Vector3.right))
		{
			return true;
		}
		return false;
	}

	private bool EvalPointInDirection(Vector3 point, Vector3 dir)
	{
		float num = 1f;
		RaycastHit raycastHit = HelperFunctions.LineCheck(point, point + dir * 10f, HelperFunctions.LayerType.TerrainProp);
		if (!raycastHit.transform)
		{
			return false;
		}
		if ((bool)raycastHit.transform.GetComponentInParent<SlidingDoor>())
		{
			return false;
		}
		float num2 = 0f;
		if (Vector3.Angle(dir, -raycastHit.normal) > 30f)
		{
			return false;
		}
		num2 = raycastHit.distance;
		Vector3 vector = Vector3.Cross(raycastHit.normal, Vector3.up);
		if (!AdditionalEval(point + num * vector, dir, num2))
		{
			return false;
		}
		if (!AdditionalEval(point + num * -vector, dir, num2))
		{
			return false;
		}
		Vector3 vector2 = Vector3.Cross(raycastHit.normal, vector);
		if (!AdditionalEval(point + num * vector2, dir, num2))
		{
			return false;
		}
		if (!AdditionalEval(point + num * -vector2, dir, num2))
		{
			return false;
		}
		savePoint = raycastHit.point;
		saveNormal = raycastHit.normal;
		return true;
	}

	private bool AdditionalEval(Vector3 point, Vector3 dir, float ogDistance)
	{
		RaycastHit raycastHit = HelperFunctions.LineCheck(point, point + dir * 20f, HelperFunctions.LayerType.TerrainProp);
		if (!raycastHit.transform)
		{
			return false;
		}
		if (Vector3.Angle(dir, -raycastHit.normal) > 30f)
		{
			return false;
		}
		if (Mathf.Abs(raycastHit.distance - ogDistance) > 0.25f)
		{
			return false;
		}
		return true;
	}

	private void GoToFailedPoint()
	{
		view.RPC("RPCA_TeleportToPoint", RpcTarget.All, new Vector3(0f, 2000f, 0f), Vector3.forward);
	}
}
