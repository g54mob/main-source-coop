using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderVector4Event : UnityEvent<GameObject, float4>
	{
	}
}
