using System.Collections.Generic;
using UnityEngine;

public class PropContentProvider : ContentProvider
{
	public PropContent content;

	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		PropContentEvent contentEvent = content.GetContentEvent();
		contentEvents.Add(new ContentEventFrame(contentEvent, seenAmount, time));
	}

	protected virtual bool ShouldShow()
	{
		return true;
	}
}
