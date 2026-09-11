using UnityEngine;

public class CageAnimationHandler : MonoBehaviour
{
	private Bot_Weeping weep;

	private Animator animator;

	private Bot bot;

	private void Start()
	{
		weep = GetComponent<Bot_Weeping>();
		bot = GetComponent<Bot>();
		animator = base.transform.root.GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		animator.SetBool("Move", bot.syncData.movementInput.y > 0.1f);
		animator.SetBool("Found Player", weep.HasCapturedPlayer);
		animator.SetBool("Catch", weep.HasCapturedPlayer);
		animator.SetBool("Show Captcha", weep.HasCapturedPlayer);
		animator.SetBool("Attacking", weep.captchaGameFinished && weep.captchaGameFailed);
		animator.SetBool("Win", weep.captchaGameFinished && !weep.captchaGameFailed);
	}
}
