using System;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public abstract class VariableEvent<T> : UnityEvent<DataVariable<T>, T>
	{
	}
}
