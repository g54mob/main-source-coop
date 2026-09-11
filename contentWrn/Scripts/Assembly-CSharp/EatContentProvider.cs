using System.Collections.Generic;
using UnityEngine;

public class EatContentProvider : MonsterContentProvider
{
	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		contentEvents.Add(new ContentEventFrame(GetContentEvent<EarContentEvent>(), seenAmount, time));
	}
}
