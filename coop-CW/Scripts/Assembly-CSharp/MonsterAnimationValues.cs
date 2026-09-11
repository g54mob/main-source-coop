using UnityEngine;

public class MonsterAnimationValues : MonoBehaviour
{
	public bool rightPunch;

	public bool leftPunch;

	public float movementMultiplier = 1f;

	private Bot bot;

	private void Start()
	{
		bot = base.transform.root.GetComponentInChildren<Bot>();
		if ((bool)GetComponentInChildren<Rigidbody>())
		{
			Object.Destroy(this);
		}
	}

	private void LateUpdate()
	{
		if ((bool)bot)
		{
			bot.animMoveSpeedFactor = movementMultiplier;
		}
	}
}
