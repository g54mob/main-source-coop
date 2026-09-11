using UnityEngine;

public class RadomChanceEvent : MonoBehaviour
{
	public ChanceEvent[] events;

	public void CallRandomEvent()
	{
		float num = 0f;
		for (int i = 0; i < events.Length; i++)
		{
			num += events[i].weight;
		}
		float num2 = Random.Range(0f, num);
		ChanceEvent chanceEvent = null;
		for (int j = 0; j < events.Length; j++)
		{
			num2 -= events[j].weight;
			chanceEvent = events[j];
			if (num2 <= 0f)
			{
				break;
			}
		}
		chanceEvent.eventToCall.Invoke();
	}
}
