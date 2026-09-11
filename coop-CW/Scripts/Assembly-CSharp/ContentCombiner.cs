using System.Collections.Generic;

public abstract class ContentCombiner
{
	public abstract void Combine(List<ContentEventFrame> events);

	protected bool HasAllEvents(List<ContentEventFrame> events, ushort[] needed)
	{
		List<ushort> foundEvents = new List<ushort>();
		foreach (ContentEventFrame @event in events)
		{
			foundEvents.Add(@event.contentEvent.GetID());
		}
		for (int i = 0; i < needed.Length; i++)
		{
			if (!Contains(needed[i]))
			{
				return false;
			}
		}
		return true;
		bool Contains(ushort ev)
		{
			foreach (ushort item in foundEvents)
			{
				if (item == ev)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected bool ReplaceEvents(List<ContentEventFrame> events, ushort[] toReplace, ContentEvent replacementEvent, out ContentEventFrame replacementEventFrame)
	{
		if (HasAllEvents(events, toReplace))
		{
			List<ContentEventFrame> list = new List<ContentEventFrame>();
			float num = 0f;
			float time = 0f;
			foreach (ushort num2 in toReplace)
			{
				foreach (ContentEventFrame @event in events)
				{
					time = @event.time;
					if (@event.contentEvent.GetID() == num2)
					{
						list.Add(@event);
						num += @event.seenAmount;
					}
				}
			}
			foreach (ContentEventFrame item in list)
			{
				events.Remove(item);
			}
			replacementEventFrame = new ContentEventFrame(replacementEvent, num, time);
			events.Add(replacementEventFrame);
			return true;
		}
		replacementEventFrame = default(ContentEventFrame);
		return false;
	}
}
