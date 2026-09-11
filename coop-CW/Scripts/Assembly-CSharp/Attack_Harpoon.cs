using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Attack_Harpoon : MonoBehaviour
{
	public RopeRender ropeRender;

	private Player player;

	private Bot bot;

	private PhotonView view;

	private MonsterAnimationHandler animator;

	public Transform reelPos;

	public float initalDamage = 15f;

	public float impactForce = 5f;

	public float dragForce = 50f;

	public float currentDistance;

	public LineRenderer line;

	private Rigidbody hookedLimb;

	private Player hookedPlayer;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = base.transform.root.GetComponentInChildren<Bot>();
		view = base.transform.GetComponent<PhotonView>();
		animator = GetComponentInParent<MonsterAnimationHandler>();
		line.positionCount = 40;
	}

	private void Update()
	{
		if (view.IsMine && bot.AbleToAttack(15f, 1f, player) && bot.CanSee(bot.Center(), bot.targetPlayer.Center(), 30f, 360f))
		{
			Aim();
		}
	}

	private void Aim()
	{
		view.RPC("RPCA_Aim", RpcTarget.All, Random.Range(1f, 3f));
	}

	[PunRPC]
	private void RPCA_HarpoonFire(int targetID, int targetBodypart)
	{
		hookedPlayer = PlayerHandler.instance.TryGetPlayerFromViewID(targetID);
		hookedLimb = hookedPlayer.refs.ragdoll.GetBodypartFromID(targetBodypart).rig;
		hookedPlayer.TakeDamageLocalIKnowWhatImDoing(initalDamage);
		if (!hookedLimb)
		{
			RPCA_BreakReel();
		}
		else
		{
			StartCoroutine(ReelPlayerIn());
		}
	}

	private IEnumerator ReelPlayerIn()
	{
		float c = 0f;
		bot.attackType = 2;
		if (view.IsMine && !bot.CanSee(bot.Center(), bot.targetPlayer.Center(), 30f, 360f))
		{
			view.RPC("RPCA_BreakReel", RpcTarget.All);
			yield break;
		}
		while (c < 2f && !player.NoControl() && bot.attacking)
		{
			c += Time.deltaTime;
			ropeRender.DisplayRope(reelPos.position, hookedLimb.position, c, line);
			yield return null;
		}
		bot.attackType = 3;
		c = 0f;
		float outOfSightCounter = 0f;
		while (c < 13f && !player.NoControl() && bot.attacking)
		{
			c += Time.fixedDeltaTime;
			Vector3 normalized = (hookedPlayer.Center() - bot.Center()).normalized;
			Vector3 normalized2 = Vector3.Cross(Vector3.up, normalized).normalized;
			Vector3 zero = Vector3.zero;
			float num = 1f;
			if ((bool)HelperFunctions.LineCheck(bot.Center() + normalized2 * num, hookedPlayer.Center() + normalized2 * num, HelperFunctions.LayerType.TerrainProp).transform)
			{
				zero += -normalized2;
			}
			if ((bool)HelperFunctions.LineCheck(bot.Center() - normalized2 * num, hookedPlayer.Center() - normalized2 * num, HelperFunctions.LayerType.TerrainProp).transform)
			{
				zero += normalized2;
			}
			if ((bool)HelperFunctions.LineCheck(bot.Center(), hookedPlayer.Center(), HelperFunctions.LayerType.TerrainProp).transform && view.IsMine)
			{
				outOfSightCounter = Mathf.MoveTowards(outOfSightCounter, 1f, Time.fixedDeltaTime);
				if (outOfSightCounter > 0.99f)
				{
					break;
				}
			}
			else
			{
				outOfSightCounter = Mathf.MoveTowards(outOfSightCounter, 0f, Time.fixedDeltaTime);
			}
			if (!hookedLimb)
			{
				break;
			}
			Vector3 force = ((bot.Center() - hookedPlayer.Center()).normalized + zero) * dragForce;
			hookedLimb.AddForce(force, ForceMode.Acceleration);
			hookedPlayer.refs.ragdoll.AddForce(force, ForceMode.Acceleration);
			if (view.IsMine && hookedPlayer.data.sinceRescueDragged < 0.3f)
			{
				break;
			}
			hookedPlayer.data.movementSlowFactor = 0.5f;
			ropeRender.DisplayRope(reelPos.position, hookedLimb.position, c, line);
			currentDistance = Vector3.Distance(bot.Center(), hookedPlayer.Center());
			if (view.IsMine && currentDistance < 1.5f)
			{
				if ((bool)bot.targetPlayer)
				{
					bot.targetPlayer.CallSlowFor(0.1f, 2f);
				}
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		if (view.IsMine)
		{
			view.RPC("RPCA_BreakReel", RpcTarget.All);
		}
		else
		{
			RPCA_BreakReel();
		}
	}

	[PunRPC]
	private void RPCA_BreakReel()
	{
		ropeRender.StopRend(line);
		bot.attacking = false;
		bot.attackType = 0;
	}

	[PunRPC]
	private void RPCA_Aim(float aimTime)
	{
		bot.sinceAttack = 0f;
		bot.attacking = true;
		bot.attackType = 1;
		bot.StandStill();
		StartCoroutine(IAim());
		IEnumerator IAim()
		{
			float c = 0f;
			while (c < aimTime && !player.NoControl() && (bool)bot.targetPlayer)
			{
				c += Time.deltaTime;
				bot.LookAt(bot.targetPlayer.Center());
				yield return null;
			}
			if (view.IsMine)
			{
				if ((bool)bot.targetPlayer && bot.CanSeeTarget(bot.Center()))
				{
					view.RPC("RPCA_HarpoonFire", RpcTarget.All, bot.syncData.targetPlayerId, bot.targetPlayer.refs.ragdoll.GetRandomBodypartID());
				}
				else
				{
					view.RPC("RPCA_BreakReel", RpcTarget.All);
				}
			}
		}
	}
}
