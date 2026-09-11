using Photon.Pun;
using UnityEngine;

public class WallExtraGrabArm : MonoBehaviour, IPunInstantiateMagicCallback
{
	public float maxRange = 10f;

	public float distanceFactor = 1f;

	public Transform handTransform;

	private Player target;

	private Rigidbody targetRig;

	private WallWarpArm arm;

	private int targetRealm;

	private float checkCounter;

	private float c;

	private bool steal;

	private PhotonView view;

	private int checks;

	private bool done;

	private float doneFor;

	private void Awake()
	{
		view = GetComponent<PhotonView>();
		base.transform.localScale = Vector3.zero;
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		object[] instantiationData = info.photonView.InstantiationData;
		targetRealm = (int)instantiationData[0];
		arm = GetComponent<WallWarpArm>();
		arm.distanceFactor = distanceFactor;
	}

	private void FixedUpdate()
	{
		checkCounter += Time.fixedDeltaTime;
		if (!target && !done)
		{
			if (checkCounter > 0.5f)
			{
				Player closestAlivePlayerToPoint = PlayerHandler.instance.GetClosestAlivePlayerToPoint(base.transform.position + base.transform.forward * 0.25f, requireVision: true, 10f);
				checkCounter = 0f;
				if ((bool)closestAlivePlayerToPoint)
				{
					target = closestAlivePlayerToPoint;
					targetRig = target.GetRig(BodypartType.Torso);
				}
				else
				{
					checks++;
				}
			}
			if (checks > 5)
			{
				done = true;
			}
		}
		else if (done)
		{
			doneFor += Time.deltaTime;
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.zero, Time.fixedDeltaTime * 10f);
			if (doneFor > 1f && view.IsMine)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
		}
		else
		{
			if (!target)
			{
				return;
			}
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Vector3.one, Time.fixedDeltaTime * 10f);
			c += Time.fixedDeltaTime;
			if (!(c < 2f))
			{
				if (steal)
				{
					Steal();
				}
				else
				{
					Reach();
				}
			}
		}
	}

	private void Steal()
	{
		arm.target = targetRig;
		arm.reachForPlayer = true;
		arm.closeHands = true;
		targetRig.linearVelocity *= 0.9f;
		targetRig.angularVelocity *= 0.9f;
		targetRig.AddForce(200f * (base.transform.position - targetRig.position).normalized, ForceMode.Acceleration);
		target.refs.ragdoll.AddForce(100f * (base.transform.position - targetRig.position).normalized, ForceMode.Acceleration);
		target.ClampGravity(0.5f);
		if (Vector3.Distance(base.transform.position, target.Center()) < 1f && view.IsMine)
		{
			ShadowRealmHandler.instance.AddPlayerToExistingRealm(target, targetRealm);
			view.RPC("RPCA_ArmDone", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_ArmDone()
	{
		arm.reachForPlayer = false;
		arm.target = null;
		arm.closeHands = false;
		done = true;
	}

	[PunRPC]
	private void RPCA_Steal(int targetPlayerID)
	{
		Player player = (target = PlayerHandler.instance.TryGetPlayerFromViewID(targetPlayerID));
		targetRig = player.GetRig(BodypartType.Torso);
		steal = true;
		player.ToggleCollisionForSeconds(1f);
	}

	private void Reach()
	{
		if (Vector3.Distance(base.transform.position, target.Center()) < maxRange && !HelperFunctions.LineCheck(base.transform.position + base.transform.forward * 0.25f, target.Center(), HelperFunctions.LayerType.TerrainProp).transform)
		{
			arm.reachForPlayer = true;
			if (Vector3.Distance(handTransform.position, target.Center()) < 1f)
			{
				if (view.IsMine)
				{
					view.RPC("RPCA_Steal", RpcTarget.All, target.refs.view.ViewID);
				}
			}
			else
			{
				arm.closeHands = false;
			}
			arm.target = targetRig;
		}
		else
		{
			arm.reachForPlayer = false;
		}
	}
}
