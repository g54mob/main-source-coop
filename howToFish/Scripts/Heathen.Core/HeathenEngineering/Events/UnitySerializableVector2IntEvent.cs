using System;
using Unity.Mathematics;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySerializableVector2IntEvent : UnityEvent<int2>
	{
	}
}
