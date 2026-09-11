using UnityEngine;

public class JumpScareSound : MonoBehaviour
{
	public int level;

	public SFX_Instance sfx;

	private int monsterID;

	public Bot currentBot;

	private float currentDistance;

	private float sinceSeeCurrent = 10f;

	private void Update()
	{
		sinceSeeCurrent += Time.deltaTime;
		GetCurrent();
		if (!(currentBot == null) && HelperFunctions.CanSee(MainCamera.instance.transform, currentBot.Center(), currentBot.canJumpScareFromBehind ? 500f : 70f))
		{
			if (sinceSeeCurrent > 10f && currentDistance < 225f)
			{
				Scare();
			}
			sinceSeeCurrent = 0f;
		}
	}

	private void Scare()
	{
		if ((bool)sfx)
		{
			sfx.Play(currentBot.Center());
		}
		GamefeelHandler.instance.perlin.AddShake(15f);
	}

	private void GetCurrent()
	{
		Bot nextMonster = BotHandler.instance.GetNextMonster(ref monsterID);
		if ((bool)nextMonster && nextMonster.jumpScareLevel == level)
		{
			float num = Vector3.SqrMagnitude(MainCamera.instance.transform.position - nextMonster.Center());
			if (!currentBot || num < currentDistance)
			{
				currentBot = nextMonster;
				currentDistance = num;
			}
		}
		if ((bool)currentBot && currentBot.jumpScareLevel != level)
		{
			currentBot = null;
		}
		if ((bool)currentBot)
		{
			currentDistance = Vector3.SqrMagnitude(MainCamera.instance.transform.position - currentBot.Center());
		}
	}
}
