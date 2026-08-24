using System;
using HeathenEngineering.Serializable;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySerializableTransformEvent : UnityEvent<SerializableTransform>
	{
	}
}
