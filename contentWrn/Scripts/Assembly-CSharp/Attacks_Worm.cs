using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Attacks_Worm : MonoBehaviour
{
	public float force;

	public float headForce;

	public AnimationCurve forceCurve;

	public float dragForce;

	private Bot bot;

	private Player player;

	private PhotonView view;

	private Player otherTarget;

	private Player jointPlayer1;

	private Player jointPlayer2;

	private Joint joint1;

	private Joint joint2;

	private float findOtherTargetTimer;

	private Rigidbody hip;

	private float loungeCounter;

	private float sinceAttach;

	private MonsterSyncer syncer;

	public SFX_Instance attachSFX;

	public SFX_Instance detachSFX;

	public SFX_Instance pullSFX;

	public AudioSource holdLoop;

	private Coroutine jumpCor;

	private void Start()
	{
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		player = GetComponentInParent<Player>();
		hip = player.GetRig(BodypartType.Hip);
		syncer = GetComponentInParent<MonsterSyncer>();
	}

	private void FixedUpdate()
	{
		holdLoop.transform.position = hip.position;
		syncer.applyData = true;
		sinceAttach += Time.deltaTime;
		if ((bool)joint2)
		{
			syncer.applyData = false;
			if (view.IsMine && (sinceAttach > 200f || !jointPlayer1 || !jointPlayer2 || player.data.tazeTime > 0f))
			{
				UnAttachAll();
				detachSFX.Play(hip.position);
				holdLoop.enabled = false;
			}
			else
			{
				bot.attacking = true;
				player.refs.ragdoll.Fall(1f);
			}
		}
		else if ((bool)joint1)
		{
			syncer.applyData = false;
			if (view.IsMine && (sinceAttach > 200f || !jointPlayer1 || player.data.tazeTime > 0f))
			{
				UnAttachAll();
				detachSFX.Play(hip.position);
				holdLoop.enabled = false;
				return;
			}
			findOtherTargetTimer += Time.fixedDeltaTime;
			if (findOtherTargetTimer > 1f)
			{
				findOtherTargetTimer = 0f;
				if ((bool)jointPlayer1)
				{
					otherTarget = PlayerHandler.instance.FindClosestPlayerToPlayer(jointPlayer1);
				}
			}
			if ((bool)otherTarget && (bool)jointPlayer1)
			{
				float num = Vector3.Distance(jointPlayer1.Center(), otherTarget.Center());
				holdLoop.enabled = true;
				holdLoop.pitch = 0.5f + num * 0.1f;
				if (num < 10f)
				{
					Vector3 normalized = (otherTarget.Center() - hip.position).normalized;
					hip.AddForce(dragForce * normalized, ForceMode.Acceleration);
				}
				if (num < 9f)
				{
					loungeCounter += Time.fixedDeltaTime;
					if (loungeCounter > 2f)
					{
						loungeCounter = 0f;
						if (view.IsMine)
						{
							view.RPC("RPCA_WormJump", RpcTarget.All, otherTarget.refs.view.ViewID, false);
						}
					}
				}
			}
			else
			{
				loungeCounter = 0f;
			}
			bot.attacking = true;
			player.refs.ragdoll.Fall(1f);
		}
		else if (bot.AbleToAttack(3f, 2.5f, player) && view.IsMine)
		{
			view.RPC("RPCA_WormJump", RpcTarget.All, bot.targetPlayer.refs.view.ViewID, true);
		}
	}

	private void UnAttachAll()
	{
		view.RPC("RPCA_UnAttach", RpcTarget.All);
	}

	[PunRPC]
	private void RPCA_UnAttach()
	{
		if ((bool)joint1)
		{
			Object.Destroy(joint1);
		}
		jointPlayer1 = null;
		if ((bool)joint2)
		{
			Object.Destroy(joint2);
		}
		jointPlayer2 = null;
		bot.attacking = false;
		detachSFX.Play(hip.position);
	}

	[PunRPC]
	private void RPCA_WormJump(int targetID, bool firstJump)
	{
		Player target = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		jumpCor = StartCoroutine(IWormAttack());
		IEnumerator IWormAttack()
		{
			float c = 0f;
			float t = forceCurve.keys[forceCurve.keys.Length - 1].time;
			bot.attacking = true;
			Rigidbody rig = (firstJump ? player.refs.ragdoll.GetBodypart(BodypartType.Head).rig : player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig);
			Rigidbody targetRig = target.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
			while (c < t && !player.data.dead && !(player.data.tazeTime > 0f) && !(target == null))
			{
				c += Time.fixedDeltaTime;
				Vector3 normalized = (targetRig.position - rig.position).normalized;
				float num = forceCurve.Evaluate(c);
				rig.AddForce(normalized * num * headForce, ForceMode.Acceleration);
				player.refs.ragdoll.AddForce(normalized * num * force, ForceMode.Acceleration);
				if (Vector3.Distance(targetRig.position, rig.position) < 0.75f && view.IsMine)
				{
					view.RPC("RPCA_WormAttach", RpcTarget.All, targetID, firstJump);
					break;
				}
				yield return new WaitForFixedUpdate();
			}
			pullSFX.Play(hip.position);
			bot.attacking = false;
			jumpCor = null;
		}
	}

	[PunRPC]
	private void RPCA_WormAttach(int targetID, bool isFirst)
	{
		sinceAttach = 0f;
		if (jumpCor != null)
		{
			StopCoroutine(jumpCor);
		}
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		if (isFirst)
		{
			Rigidbody rig = this.player.refs.ragdoll.GetBodypart(BodypartType.Head).rig;
			Rigidbody rig2 = player.GetRig(BodypartType.Torso);
			Vector3 vector = rig.transform.TransformPoint(Vector3.up * 10f);
			Vector3 vector2 = rig2.position - vector;
			rig.transform.position += vector2;
			attachSFX.Play(hip.position);
			joint1 = HelperFunctions.AttachPositionJoint(rig, player.GetRig(BodypartType.Torso), useCustomConnection: true, rig2.position);
			jointPlayer1 = player;
		}
		else
		{
			Rigidbody rig3 = this.player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig;
			Rigidbody rig4 = player.GetRig(BodypartType.Torso);
			Vector3 position = rig3.transform.position;
			Vector3 vector3 = rig4.position - position;
			rig3.transform.position += vector3;
			attachSFX.Play(hip.position);
			joint2 = HelperFunctions.AttachPositionJoint(rig3, player.GetRig(BodypartType.Torso), useCustomConnection: true, rig4.position);
			jointPlayer2 = player;
		}
	}
}
