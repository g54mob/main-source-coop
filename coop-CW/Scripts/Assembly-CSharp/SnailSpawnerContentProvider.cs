using System.Collections.Generic;
using UnityEngine;

public class SnailSpawnerContentProvider : MonsterContentProvider
{
	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		contentEvents.Add(new ContentEventFrame(GetContentEvent<SnailSpawnerContentEvent>(), seenAmount, time));
	}
}
