using System.Collections.Generic;
using UnityEngine;

public class EyeGuyContentProvider : MonsterContentProvider
{
	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		contentEvents.Add(new ContentEventFrame(GetContentEvent<EyeGuyContentEvent>(), seenAmount, time));
	}
}
