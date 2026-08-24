using System;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderIntEvent : UnityEvent<GameObject, int>
	{
	}
}
