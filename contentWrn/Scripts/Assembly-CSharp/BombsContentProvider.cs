using System.Collections.Generic;
using UnityEngine;

public class BombsContentProvider : MonsterContentProvider
{
	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		contentEvents.Add(new ContentEventFrame(GetContentEvent<BombsContentEvent>(), seenAmount, time));
	}
}
