using UnityEngine;

public class Bot_LookY : MonoBehaviour
{
	private MonsterAnimationHandler anim;

	private Bot bot;

	private void Start()
	{
		bot = base.transform.root.GetComponentInChildren<Bot>();
		anim = GetComponentInParent<MonsterAnimationHandler>();
	}

	private void Update()
	{
		anim.SetFloat("Look Y", bot.TargetLookY());
	}
}
