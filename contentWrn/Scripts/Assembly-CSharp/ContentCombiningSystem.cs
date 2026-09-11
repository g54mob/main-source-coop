using System.Collections.Generic;

public static class ContentCombiningSystem
{
	public static List<ContentCombiner> Combiners = new List<ContentCombiner>();

	public static void Init()
	{
		Combiners = new List<ContentCombiner>();
		Combiners.Add(new MonsterEventCombiner());
		Combiners.Add(new InterviewEventCombiner());
		Combiners.Add(new TauntEventCombiner());
	}

	public static List<ContentEventFrame> CombineEvents(List<ContentEventFrame> events)
	{
		foreach (ContentCombiner combiner in Combiners)
		{
			combiner.Combine(events);
		}
		return events;
	}
}
