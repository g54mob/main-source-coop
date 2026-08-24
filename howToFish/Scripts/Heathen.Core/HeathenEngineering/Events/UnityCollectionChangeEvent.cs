using System;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnityCollectionChangeEvent<T> : UnityEvent<CollectionChangeEventData<T>>
	{
	}
}
