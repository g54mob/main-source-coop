using System;
using Unity.Mathematics;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class VariableVector4Event : UnityEvent<Vector4Variable, float4>
	{
	}
}
