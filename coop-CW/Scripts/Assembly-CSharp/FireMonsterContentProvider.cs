using System.Collections.Generic;
using UnityEngine;

public class FireMonsterContentProvider : MonsterContentProvider
{
	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		contentEvents.Add(new ContentEventFrame(GetContentEvent<FireMonsterContentEvent>(), seenAmount, time));
	}
}
