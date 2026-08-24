using System;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderStringEvent : UnityEvent<GameObject, string>
	{
	}
}
