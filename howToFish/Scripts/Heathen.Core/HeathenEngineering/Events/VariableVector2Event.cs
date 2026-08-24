using System;
using Unity.Mathematics;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class VariableVector2Event : UnityEvent<Vector2Variable, float2>
	{
	}
}
