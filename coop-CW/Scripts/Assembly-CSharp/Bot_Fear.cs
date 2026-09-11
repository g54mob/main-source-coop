using UnityEngine;

public class Bot_Fear : MonoBehaviour
{
	private Bot bot;

	public Player fearTarget;

	private void Start()
	{
		bot = GetComponent<Bot>();
	}

	private void Update()
	{
		if (fearTarget != bot.targetPlayer)
		{
			if ((bool)fearTarget)
			{
				fearTarget.data.fearList.Remove(bot);
			}
			fearTarget = bot.targetPlayer;
			if ((bool)fearTarget)
			{
				fearTarget.data.fearList.Add(bot);
			}
		}
	}
}
