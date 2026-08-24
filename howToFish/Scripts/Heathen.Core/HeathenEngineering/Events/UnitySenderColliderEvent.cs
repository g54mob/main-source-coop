using System;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderColliderEvent : UnityEvent<GameObject, Collider>
	{
	}
}
