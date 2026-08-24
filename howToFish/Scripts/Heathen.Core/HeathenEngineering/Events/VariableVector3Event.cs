using System;
using Unity.Mathematics;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class VariableVector3Event : UnityEvent<Vector3Variable, float3>
	{
	}
}
