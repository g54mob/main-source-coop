using System;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnityDataEvent : UnityEvent<EventData>
	{
	}
	[Serializable]
	public class UnityDataEvent<T> : UnityEvent<EventData<T>>
	{
	}
}
