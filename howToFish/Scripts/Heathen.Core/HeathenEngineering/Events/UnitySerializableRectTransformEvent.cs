using System;
using HeathenEngineering.Serializable;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySerializableRectTransformEvent : UnityEvent<SerializableRectTransform>
	{
	}
}
