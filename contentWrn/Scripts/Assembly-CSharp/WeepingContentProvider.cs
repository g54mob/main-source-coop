using System;
using System.Collections.Generic;
using UnityEngine;

public class WeepingContentProvider : MonsterContentProvider
{
	public enum WEEPING_CONTENT_STATE : byte
	{
		idle = 0,
		success = 1,
		fail = 2,
		captured = 3
	}

	private Bot_Weeping weeping;

	private void Start()
	{
		weeping = base.transform.root.GetComponentInChildren<Bot_Weeping>();
	}

	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		switch (weeping.GetContentState())
		{
		case WEEPING_CONTENT_STATE.idle:
			contentEvents.Add(new ContentEventFrame(GetContentEvent<WeepingContentEvent>(), seenAmount, time));
			break;
		case WEEPING_CONTENT_STATE.success:
			contentEvents.Add(new ContentEventFrame(GetContentEvent<WeepingContentEventSuccess>(), seenAmount, time));
			break;
		case WEEPING_CONTENT_STATE.fail:
			contentEvents.Add(new ContentEventFrame(GetContentEvent<WeepingContentEventFail>(), seenAmount, time));
			break;
		case WEEPING_CONTENT_STATE.captured:
			contentEvents.Add(new ContentEventFrame(GetContentEvent<WeepingContentEventCaptured>(), seenAmount, time));
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
