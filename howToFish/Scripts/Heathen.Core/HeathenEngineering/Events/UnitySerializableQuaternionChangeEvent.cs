using System;
using Unity.Mathematics;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySerializableQuaternionChangeEvent : UnityChangeEvent<quaternion>
	{
	}
}
