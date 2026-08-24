using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderVector3Event : UnityEvent<GameObject, float3>
	{
	}
}
