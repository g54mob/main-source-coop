using System.Collections.Generic;
using UnityEngine;

public class BigSlapContentProvider : MonsterContentProvider
{
	public Bot bot;

	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		if (bot.aggro)
		{
			contentEvents.Add(new ContentEventFrame(GetContentEvent<BigSlapAgroContentEvent>(), seenAmount, time));
		}
		else
		{
			contentEvents.Add(new ContentEventFrame(GetContentEvent<BigSlapPeacefulContentEvent>(), seenAmount, time));
		}
	}
}
