using System;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnityChangeEvent<T> : UnityEvent<ChangeEventData<T>>
	{
	}
}
