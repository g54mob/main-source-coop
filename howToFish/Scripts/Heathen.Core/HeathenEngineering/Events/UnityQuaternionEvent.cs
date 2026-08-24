using System;
using Unity.Mathematics;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnityQuaternionEvent : UnityEvent<quaternion>
	{
	}
}
