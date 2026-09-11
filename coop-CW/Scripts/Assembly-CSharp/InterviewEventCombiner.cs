using System.Collections.Generic;
using UnityEngine;

public class InterviewEventCombiner : ContentCombiner
{
	public override void Combine(List<ContentEventFrame> events)
	{
		List<ContentEventFrame> list = new List<ContentEventFrame>();
		List<ContentEventFrame> list2 = new List<ContentEventFrame>();
		foreach (ContentEventFrame @event in events)
		{
			ContentEvent contentEvent = @event.contentEvent;
			if (contentEvent is MonsterContentEvent)
			{
				list.Add(@event);
			}
			if (contentEvent is PlayerHoldingMicContentEvent)
			{
				list2.Add(@event);
			}
		}
		foreach (ContentEventFrame item in list2)
		{
			foreach (ContentEventFrame item2 in list)
			{
				MonsterContentEvent monsterContentEvent = item2.contentEvent as MonsterContentEvent;
				if (Vector3.Distance((item.contentEvent as PlayerHoldingMicContentEvent).worldPosition, monsterContentEvent.worldPosition) < 10f)
				{
					InterviewEvent contentEvent2 = new InterviewEvent
					{
						contentValue = monsterContentEvent.GetContentValue() * 0.2f,
						monsterID = monsterContentEvent.GetID()
					};
					events.Add(new ContentEventFrame(contentEvent2, item.seenAmount, item.time));
				}
			}
		}
	}
}
