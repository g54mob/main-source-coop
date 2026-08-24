using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderVector2Event : UnityEvent<GameObject, float2>
	{
	}
}
