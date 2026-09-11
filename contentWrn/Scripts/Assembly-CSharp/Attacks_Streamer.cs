using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Attacks_Streamer : MonoBehaviour
{
	public float force;

	public float torsoForce;

	public AnimationCurve forceCurve;

	public AnimationCurve torsoCurve;

	public Transform forcePoint;

	public GameObject toggleObject;

	private Bot bot;

	private Player player;

	private PhotonView view;

	private MonsterAnimationHandler anim;

	private MonsterAnimationValues values;

	private float taseCounter;

	private void Start()
	{
		anim = GetComponentInParent<MonsterAnimationHandler>();
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		player = GetComponentInParent<Player>();
		values = player.refs.animatorTransform.GetComponent<MonsterAnimationValues>();
	}

	private void Update()
	{
		if (view.IsMine)
		{
			taseCounter += Time.deltaTime;
			if (bot.AbleToAttack(4f, 2f, player))
			{
				view.RPC("RPCA_TazeAttack", RpcTarget.All);
			}
		}
	}

	[PunRPC]
	private void RPCA_TazeAttack()
	{
		anim.PlayAnimation("Streamer Attack");
		StartCoroutine(IDoThrow());
		IEnumerator IDoThrow()
		{
			bot.attacking = true;
			Rigidbody rig = player.refs.ragdoll.GetBodypart(BodypartType.Hand_R).rig;
			Rigidbody torso = player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
			float c = 0f;
			while (c < 1.5f)
			{
				if (c > 0.3f && !toggleObject.activeSelf)
				{
					toggleObject.SetActive(value: true);
				}
				if (player.NoControl() || bot.targetPlayer == null)
				{
					break;
				}
				if (view.IsMine)
				{
					bot.StandStill();
					bot.LookAt(bot.targetPlayer.Center(), 5f);
				}
				rig.AddForceAtPosition((bot.targetPlayer.Center() - forcePoint.position).normalized * force * forceCurve.Evaluate(c), forcePoint.position, ForceMode.Acceleration);
				torso.AddForce((bot.targetPlayer.Center() - torso.position).normalized * torsoForce * torsoCurve.Evaluate(c), ForceMode.Acceleration);
				c += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			toggleObject.SetActive(value: false);
			bot.attacking = false;
		}
	}
}
