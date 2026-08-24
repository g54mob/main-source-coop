using System;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderDoubleEvent : UnityEvent<GameObject, double>
	{
	}
}
