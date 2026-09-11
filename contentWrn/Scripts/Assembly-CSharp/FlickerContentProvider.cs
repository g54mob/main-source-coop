using System.Collections.Generic;
using UnityEngine;

public class FlickerContentProvider : MonsterContentProvider
{
	public Bot_Skinny bot;

	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		if (bot.isInDimention || bot.fullyInDimention)
		{
			contentEvents.Add(new ContentEventFrame(GetContentEvent<FlickerContentEvent>(), seenAmount, time));
		}
	}
}
