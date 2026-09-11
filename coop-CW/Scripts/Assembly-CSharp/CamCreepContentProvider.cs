using System.Collections.Generic;
using UnityEngine;

public class CamCreepContentProvider : MonsterContentProvider
{
	private Bot_CameraCreep creep;

	private void Start()
	{
		creep = GetComponentInChildren<Bot_CameraCreep>();
	}

	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		creep.IsFilmed(camera, seenAmount, time);
		contentEvents.Add(new ContentEventFrame(GetContentEvent<CamCreepContentEvent>(), seenAmount, time));
	}
}
