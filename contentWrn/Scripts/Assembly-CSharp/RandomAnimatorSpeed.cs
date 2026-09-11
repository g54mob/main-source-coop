using UnityEngine;

public class RandomAnimatorSpeed : MonoBehaviour
{
	public Animator anim;

	public Vector2 speedRange;

	public bool worldPosAsRandomSeed;

	private void Start()
	{
		Random.State state = default(Random.State);
		if (worldPosAsRandomSeed)
		{
			state = HelperFunctions.SetRandomSeedFromWorldPos(base.transform.position, GameAPI.seed);
		}
		anim.speed = Random.Range(speedRange.x, speedRange.y);
		if (worldPosAsRandomSeed)
		{
			Random.state = state;
		}
	}
}
