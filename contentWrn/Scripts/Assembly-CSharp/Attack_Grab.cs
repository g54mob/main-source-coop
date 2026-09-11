using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Attack_Grab : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private MonsterAnimationHandler anim;

	private PhotonView view;

	private int grabs;

	private Joint joint;

	private Player carriedPlayer;

	private float counter;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		anim = GetComponentInParent<MonsterAnimationHandler>();
		view = base.transform.GetComponent<PhotonView>();
	}

	private void FixedUpdate()
	{
		counter += Time.deltaTime;
		if ((bool)carriedPlayer && carriedPlayer.IsLocal && counter > 1f)
		{
			carriedPlayer.CallTakeDamage(11f);
			counter = 0f;
		}
		if (!bot.BusyOrAttacking() && !(bot.distanceToTarget > 4f) && bot.targetIsHiding && !(bot.sinceAttack < 2f) && view.IsMine && !player.NoControl())
		{
			Attack();
		}
	}

	private void Attack()
	{
		bot.attacking = true;
		view.RPC("RPCA_GrabAttack", RpcTarget.All);
	}

	[PunRPC]
	private void RPCA_GrabAttack()
	{
		LetGo();
		bot.attacking = true;
		grabs++;
		if (grabs > 1)
		{
			base.transform.root.GetComponentInChildren<NavMeshAgent>().agentTypeID = 0;
		}
		anim.Grab();
		StartCoroutine(DoGrab());
		IEnumerator DoGrab()
		{
			float c = 0f;
			bool grabbed = false;
			while (c < 3f)
			{
				c += Time.deltaTime;
				player.refs.IK_Right.weight = 0f;
				if (player.NoControl())
				{
					break;
				}
				bot.StandStill();
				if (!grabbed)
				{
					Vector3 normalized = (bot.targetPlayer.Center() - bot.Center()).normalized;
					bot.LookAt(bot.targetPlayer.Center());
					Vector3 position = bot.targetPlayer.Center();
					position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.transform.InverseTransformPoint(position);
					position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).animationTarget.transform.TransformPoint(position);
					player.refs.IK_Hand_R.transform.position = position;
					player.refs.IK_Hand_R.transform.rotation = Quaternion.LookRotation(normalized + Vector3.up, Vector3.down);
					if (c > 1f && c < 2f)
					{
						player.refs.IK_Right.weight = 1f;
						Rigidbody rig = player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
						rig.AddForce(normalized * 400f, ForceMode.Acceleration);
						if (view.IsMine && c > 1.3f && Vector3.Distance(rig.position, bot.targetPlayer.Center()) < 1f)
						{
							CallGrab();
							grabbed = true;
						}
					}
				}
				yield return null;
			}
			player.refs.IK_Right.weight = 0f;
			bot.attacking = false;
		}
	}

	public void LetGo()
	{
		if ((bool)carriedPlayer)
		{
			if (bot.ignoredPlayers.Contains(carriedPlayer))
			{
				bot.ignoredPlayers.Remove(carriedPlayer);
			}
			if (carriedPlayer.IsLocal)
			{
				carriedPlayer.CallTakeDamage(110f);
			}
		}
		if ((bool)joint)
		{
			Object.Destroy(joint);
			if ((bool)carriedPlayer)
			{
				carriedPlayer.data.carried = false;
			}
		}
	}

	private void CallGrab()
	{
		view.RPC("RPCA_Grab", RpcTarget.All, bot.targetPlayer.refs.view.ViewID);
	}

	[PunRPC]
	private void RPCA_Grab(int playerID)
	{
		Player player = PlayerHandler.instance.TryGetPlayerFromViewID(playerID);
		Rigidbody rig = this.player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
		rig.transform.position = player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.position;
		joint = rig.gameObject.AddComponent<FixedJoint>();
		joint.connectedBody = player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
		player.data.carried = true;
		bot.ignoredPlayers.Add(player);
		carriedPlayer = player;
		bot.LoseTarget();
	}
}
