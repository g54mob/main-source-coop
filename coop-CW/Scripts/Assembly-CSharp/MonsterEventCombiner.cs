using System.Collections.Generic;

public class MonsterEventCombiner : ContentCombiner
{
	public override void Combine(List<ContentEventFrame> events)
	{
		byte b = 0;
		float num = 0f;
		float time = 0f;
		float num2 = 0f;
		List<ushort> list = new List<ushort>();
		foreach (ContentEventFrame @event in events)
		{
			if (@event.contentEvent is MonsterContentEvent monsterContentEvent)
			{
				b++;
				num += @event.seenAmount;
				time = @event.time;
				num2 += monsterContentEvent.GetContentValue();
				list.Add(monsterContentEvent.GetID());
			}
		}
		if (b > 1)
		{
			MultiMonsterEvent contentEvent = new MultiMonsterEvent
			{
				contentValue = num2 * 0.3f,
				monsterCount = b
			};
			events.Add(new ContentEventFrame(contentEvent, num, time));
		}
	}
}
