using System;
using Unity.Mathematics;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySerializableQuaternionEvent : UnityEvent<quaternion>
	{
	}
}
