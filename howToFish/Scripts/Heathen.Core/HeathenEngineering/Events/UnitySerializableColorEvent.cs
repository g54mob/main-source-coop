using System;
using Unity.Mathematics;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySerializableColorEvent : UnityEvent<float4>
	{
	}
}
