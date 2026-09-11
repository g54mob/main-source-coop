using Photon.Pun;
using UnityEngine;

public class Attacks_Arms : MonoBehaviour
{
	private Player player;

	private Bot bot;

	private PhotonView view;

	private MonsterAnimationHandler animator;

	private static readonly int LookYProp = Animator.StringToHash("Look Y");

	private static readonly int GrabAmountProp = Animator.StringToHash("Grab Amount");

	public float handForce = 10f;

	public float dragForce = 10f;

	private float strength = 3f;

	private float counter;

	private Rigidbody rig1;

	private Rigidbody rig2;

	private Transform handPoint1;

	private Transform handPoint2;

	private float grabAmount;

	private bool toggle;

	public SFX_Instance[] grabSFX;

	private bool isAggro;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		view = base.transform.GetComponent<PhotonView>();
		animator = GetComponentInParent<MonsterAnimationHandler>();
		rig1 = player.refs.ragdoll.GetBodypart(BodypartType.Finger_1_3_L).rig;
		rig2 = player.refs.ragdoll.GetBodypart(BodypartType.Finger_1_3_R).rig;
		handPoint1 = rig1.transform.GetChild(0);
		handPoint2 = rig2.transform.GetChild(0);
	}

	private void Update()
	{
		if (view.IsMine)
		{
			SyncAggro();
		}
		if (!player.NoControl())
		{
			Arms();
			if ((bool)bot.targetPlayer && bot.aggro)
			{
				TryAttack();
				animator.SetFloat(LookYProp, (bot.targetPlayer.Center() - bot.Center()).normalized.y * 0.5f + 0.05f);
			}
		}
	}

	private void SyncAggro()
	{
		if (bot.aggro != isAggro)
		{
			view.RPC("RPCA_SetAggro", RpcTarget.All, bot.aggro);
		}
	}

	[PunRPC]
	private void RPCA_SetAggro(bool setAggro)
	{
		isAggro = setAggro;
		bot.aggro = setAggro;
	}

	private void Arms()
	{
		if (bot.aggro && bot.distanceToTarget < 10f && bot.CanSeeTarget(bot.Center(), 30f))
		{
			float target = Mathf.InverseLerp(2f, 8f, bot.distanceToTarget);
			grabAmount = Mathf.MoveTowards(grabAmount, target, Time.deltaTime);
		}
		else
		{
			grabAmount = Mathf.MoveTowards(grabAmount, 0f, Time.deltaTime);
		}
		animator.SetFloat(GrabAmountProp, grabAmount);
		if (grabAmount == 0f)
		{
			toggle = false;
		}
	}

	private void TryAttack()
	{
		bool flag = false;
		Vector3 vector = (handPoint1.position + handPoint2.position) * 0.5f;
		if (Vector3.Distance(vector, bot.targetPlayer.Center()) < 1f)
		{
			float num = Mathf.Clamp(strength, 0f, 2f);
			flag = true;
			Rigidbody rig = bot.targetPlayer.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
			Vector3 normalized = (vector - rig.position).normalized;
			rig.AddForce(normalized * num * dragForce, ForceMode.Acceleration);
			Vector3 vector2 = Vector3.ClampMagnitude((rig.position - handPoint1.position) * 3f, 1f);
			Vector3 vector3 = Vector3.ClampMagnitude((rig.position - handPoint2.position) * 3f, 1f);
			rig1.AddForceAtPosition(vector2 * num * handForce, handPoint1.position, ForceMode.Acceleration);
			rig2.AddForceAtPosition(vector3 * num * handForce, handPoint2.position, ForceMode.Acceleration);
			counter += Time.fixedDeltaTime;
			if (counter > 0.75f)
			{
				if (bot.targetPlayer.IsLocal)
				{
					bot.targetPlayer.CallTakeDamage(5f);
				}
				counter = 0f;
			}
			if (!toggle)
			{
				toggle = true;
				for (int i = 0; i < grabSFX.Length; i++)
				{
					grabSFX[i].Play(bot.targetPlayer.Center());
				}
			}
			strength = Mathf.MoveTowards(strength, 0.5f, Time.fixedDeltaTime * 0.4f);
		}
		if (!flag)
		{
			counter = Mathf.MoveTowards(counter, 0f, Time.fixedDeltaTime);
			strength = Mathf.MoveTowards(strength, 3f, Time.fixedDeltaTime);
		}
	}
}
