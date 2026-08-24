using System;
using Unity.Mathematics;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySerializableVector3ChangeEvent : UnityChangeEvent<float3>
	{
	}
}
