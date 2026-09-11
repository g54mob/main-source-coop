using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Attack_Animation : MonoBehaviour
{
	public UnityEvent attackEvent;

	public float secondsEventDelay;

	public string animationName;

	public float attackLength = 1f;

	public float range = 2f;

	public float cooldown = 4f;

	private Bot bot;

	private Player player;

	private PhotonView view;

	private MonsterAnimationHandler anim;

	private void Start()
	{
		anim = GetComponentInParent<MonsterAnimationHandler>();
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		player = GetComponentInParent<Player>();
	}

	private void Update()
	{
		if (view.IsMine && bot.AbleToAttack(range, cooldown, player))
		{
			view.RPC("RPCA_AnimationAttack", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_AnimationAttack()
	{
		if (animationName != "")
		{
			anim.PlayAnimation(animationName);
		}
		StartCoroutine(IAttackValue());
		StartCoroutine(IDelayEvent());
		IEnumerator IAttackValue()
		{
			bot.StandStill();
			bot.attacking = true;
			yield return new WaitForSeconds(attackLength);
			bot.attacking = false;
		}
		IEnumerator IDelayEvent()
		{
			yield return new WaitForSeconds(secondsEventDelay);
			attackEvent.Invoke();
		}
	}
}
