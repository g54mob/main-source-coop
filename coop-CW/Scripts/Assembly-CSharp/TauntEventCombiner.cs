using System.Collections.Generic;
using UnityEngine;

public class TauntEventCombiner : ContentCombiner
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
			if (contentEvent is PlayerEmoteContentEvent)
			{
				list2.Add(@event);
			}
		}
		foreach (ContentEventFrame item in list2)
		{
			foreach (ContentEventFrame item2 in list)
			{
				MonsterContentEvent monsterContentEvent = item2.contentEvent as MonsterContentEvent;
				PlayerEmoteContentEvent playerEmoteContentEvent = item.contentEvent as PlayerEmoteContentEvent;
				float num = Vector3.Distance(playerEmoteContentEvent.worldPosition, monsterContentEvent.worldPosition);
				if (num < 15f)
				{
					TauntEvent contentEvent2 = new TauntEvent(playerEmoteContentEvent.playerName, monsterContentEvent.GetID(), num, monsterContentEvent.GetContentValue() * 0.2f);
					events.Add(new ContentEventFrame(contentEvent2, item.seenAmount, item.time));
				}
			}
		}
	}
}
