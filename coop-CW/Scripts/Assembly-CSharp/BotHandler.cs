using System.Collections.Generic;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;

public class BotHandler : MonoBehaviour
{
	public static BotHandler instance;

	public List<Bot> bots = new List<Bot>();

	private void Awake()
	{
		instance = this;
	}

	public void DestroyAll()
	{
		bots.ForEach(delegate(Bot bot)
		{
			PhotonNetwork.Destroy(bot.transform.root.gameObject);
		});
	}

	internal Bot GetFurthestFromAnyPlayer()
	{
		List<Player> playersAlive = PlayerHandler.instance.playersAlive;
		if (playersAlive.Count == 0)
		{
			return null;
		}
		float num = 0f;
		Bot result = null;
		for (int i = 0; i < bots.Count; i++)
		{
			Bot bot = bots[i];
			if (PlayerHandler.instance.CanAnAlivePlayerSeePoint(bot.Center(), out var _) || (bool)bot.GetComponent<Bot_Slurper>() || (bool)bot.GetComponent<Bot_Infiltrator>())
			{
				continue;
			}
			float num2 = float.MaxValue;
			for (int j = 0; j < playersAlive.Count; j++)
			{
				Player player = playersAlive[j];
				float num3 = Vector3.SqrMagnitude(bot.Center() - player.Center());
				if (num3 < num2)
				{
					num2 = num3;
				}
			}
			if (num2 > num)
			{
				result = bot;
				num = num2;
			}
		}
		return result;
	}

	internal Bot GetNextMonster(ref int botID)
	{
		botID++;
		if (botID >= bots.Count)
		{
			botID = 0;
		}
		if (bots.Count == 0)
		{
			return null;
		}
		return bots[botID];
	}
}
