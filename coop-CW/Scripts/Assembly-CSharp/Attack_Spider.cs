using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Attack_Spider : MonoBehaviour
{
	public GameObject projectile;

	private Bot bot;

	private Player player;

	private PhotonView view;

	private MonsterAnimationHandler anim;

	private MonsterAnimationValues values;

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
		if (bot.AbleToAttack(15f, 5f, player))
		{
			view.RPC("RPCA_ThrowNet", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_ThrowNet()
	{
		StartCoroutine(IThrowNet());
		IEnumerator IThrowNet()
		{
			bot.attacking = true;
			bool hasThrown = false;
			Vector3 targetPos = Vector3.zero;
			if ((bool)bot.targetPlayer)
			{
				targetPos = bot.targetPlayer.Center();
			}
			float c = 0f;
			while (c < 1f && !player.NoControl())
			{
				if (view.IsMine)
				{
					bot.StandStill();
					bot.LookAt(targetPos, 5f);
				}
				if (!hasThrown && c > 0.5f)
				{
					Vector3 normalized = (targetPos - bot.Center()).normalized;
					Object.Instantiate(projectile, player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.transform.position, Quaternion.LookRotation(normalized));
					hasThrown = true;
				}
				if ((bool)bot.targetPlayer && bot.CanSee(bot.Center(), bot.targetPlayer.Center(), 30f, 120f))
				{
					targetPos = bot.targetPlayer.Center();
				}
				c += Time.deltaTime;
				yield return null;
			}
			bot.attacking = false;
		}
	}
}
