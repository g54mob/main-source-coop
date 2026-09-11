using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Attack_Throw : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	private MonsterAnimationHandler animator;

	public AnimationCurve grabForceCurve;

	public AnimationCurve throwCurve;

	public float force;

	public float throwForce;

	public float windupForce;

	internal MonsterAnimationValues val;

	public float damage = 75f;

	public float hitPlayerForce = 3f;

	private Joint jl;

	private Joint jr;

	private Coroutine grabCor;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		view = base.transform.GetComponent<PhotonView>();
		animator = GetComponentInParent<MonsterAnimationHandler>();
		val = player.refs.animatorTransform.GetComponentInChildren<MonsterAnimationValues>();
	}

	private void Update()
	{
		if (bot.AbleToAttack(3f, 2.5f, player))
		{
			ReachFor();
		}
	}

	public void CallGrab(Player targetPlayer)
	{
		view.RPC("RPCA_LarvaGrab", RpcTarget.All, targetPlayer.refs.view.ViewID);
	}

	[PunRPC]
	public void RPCA_LarvaGrab(int targetPlayerID)
	{
		Player targetPlayer = PlayerHandler.instance.TryGetPlayerFromViewID(targetPlayerID);
		if (grabCor != null)
		{
			StopCoroutine(grabCor);
		}
		animator.PlayAnimation("Larva_Throw");
		bot.busy = true;
		StartCoroutine(IThrow(targetPlayer));
		IEnumerator IThrow(Player player)
		{
			Rigidbody left = this.player.refs.ragdoll.GetBodypart(BodypartType.Elbow_L).rig;
			Transform leftHand = left.transform.GetChild(0);
			Rigidbody right = this.player.refs.ragdoll.GetBodypart(BodypartType.Elbow_R).rig;
			Transform rightHand = right.transform.GetChild(0);
			Rigidbody rig = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig;
			left.transform.position += rig.transform.position - leftHand.position;
			right.transform.position += rig.transform.position - rightHand.position;
			Physics.SyncTransforms();
			jl = HelperFunctions.AttachPositionJoint(left, rig);
			jr = HelperFunctions.AttachPositionJoint(right, rig);
			bot.ignoredPlayers.Add(player);
			float c = 0f;
			float t = 5f;
			bool forceHasStarted = false;
			float throwForceC = 0f;
			while (c < t)
			{
				bot.busy = true;
				bot.attacking = true;
				if (view.IsMine)
				{
					bot.StandStill();
					Player nearbyPlayerInSight = bot.GetNearbyPlayerInSight(30f, 500f);
					if ((bool)nearbyPlayerInSight)
					{
						bot.LookAt(nearbyPlayerInSight.Center(), 5f);
					}
				}
				if (this.player.NoControl() || player.data.sinceRescueDragged < 0.5f)
				{
					LetGo();
					break;
				}
				player.data.sinceGrounded = 0f;
				if (val.rightPunch)
				{
					forceHasStarted = true;
				}
				if (forceHasStarted)
				{
					if (!val.rightPunch)
					{
						LetGo();
						player.refs.ragdoll.Fall(1f);
						player.refs.ragdoll.AddForce(Vector3.Lerp(bot.syncData.lookDireciton, Vector3.up, 0.1f) * throwForce, ForceMode.VelocityChange);
						if (player.IsLocal)
						{
							ThrownBodyDamage thrownBodyDamage = player.gameObject.AddComponent<ThrownBodyDamage>();
							thrownBodyDamage.damage = damage;
							thrownBodyDamage.force = hitPlayerForce;
							thrownBodyDamage.direction = bot.syncData.lookDireciton;
						}
						break;
					}
					throwForceC += Time.fixedDeltaTime;
					float num = windupForce * throwCurve.Evaluate(throwForceC);
					player.refs.ragdoll.AddForce(bot.syncData.lookDireciton * num, ForceMode.Acceleration);
					left.AddForceAtPosition(bot.syncData.lookDireciton * num, leftHand.position, ForceMode.Acceleration);
					right.AddForceAtPosition(bot.syncData.lookDireciton * num, rightHand.position, ForceMode.Acceleration);
				}
				bot.StandStill();
				c += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			bot.busy = false;
			bot.attacking = false;
			if ((bool)player && bot.ignoredPlayers.Contains(player))
			{
				bot.ignoredPlayers.Remove(player);
			}
		}
	}

	private void LetGo()
	{
		if ((bool)jr)
		{
			Object.Destroy(jr);
		}
		if ((bool)jl)
		{
			Object.Destroy(jl);
		}
	}

	private void ReachFor()
	{
		view.RPC("RPCA_LarvaReachForTarget", RpcTarget.All, bot.targetPlayer.refs.view.ViewID);
	}

	[PunRPC]
	public void RPCA_LarvaReachForTarget(int targetPlayerID)
	{
		Player targetPlayer = PlayerHandler.instance.TryGetPlayerFromViewID(targetPlayerID);
		grabCor = StartCoroutine(TryReach(targetPlayer));
		IEnumerator TryReach(Player player)
		{
			bot.attacking = true;
			float c = 0f;
			float t = 1f;
			Rigidbody torso = this.player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
			Rigidbody left = this.player.refs.ragdoll.GetBodypart(BodypartType.Elbow_L).rig;
			Transform leftHand = left.transform.GetChild(0);
			Rigidbody right = this.player.refs.ragdoll.GetBodypart(BodypartType.Elbow_R).rig;
			Transform rightHand = right.transform.GetChild(0);
			while (c < t && !this.player.NoControl())
			{
				if (view.IsMine)
				{
					bot.ChaseTarget(this.player.HeadPosition(), 0.5f, 0f, lookForBetterTarget: false, 10f);
				}
				c += Time.fixedDeltaTime;
				Vector3 normalized = (player.Center() - leftHand.position).normalized;
				Vector3 normalized2 = (player.Center() - rightHand.position).normalized;
				Vector3 normalized3 = (player.Center() - torso.position).normalized;
				float num = force * grabForceCurve.Evaluate(c);
				left.AddForceAtPosition(normalized * num, leftHand.position, ForceMode.Acceleration);
				right.AddForceAtPosition(normalized2 * num, rightHand.position, ForceMode.Acceleration);
				torso.AddForce(normalized3 * num * 0.2f, ForceMode.Acceleration);
				if (view.IsMine)
				{
					float num2 = Vector3.Distance(leftHand.position, player.Center());
					float num3 = Vector3.Distance(leftHand.position, player.Center());
					if (num2 < 0.5f || num3 < 0.5f)
					{
						CallGrab(player);
					}
				}
				yield return new WaitForFixedUpdate();
			}
			bot.attacking = false;
			grabCor = null;
		}
	}
}
