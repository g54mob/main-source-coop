using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiPropEventCombiner : ContentCombiner
{
	public PropContent[] neededProps;

	public PropContent replacementProp;

	public override void Combine(List<ContentEventFrame> events)
	{
		if (ReplaceEvents(events, neededProps.Select((PropContent content) => content.id).ToArray(), replacementProp.GetContentEvent(), out var replacementEventFrame))
		{
			Debug.Log("Replaced events with " + replacementEventFrame.contentEvent.GetName() + " with seen amount " + replacementEventFrame.seenAmount);
		}
	}
}
