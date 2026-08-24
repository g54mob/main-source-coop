using System;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderRectTransformEvent : UnityEvent<GameObject, RectTransform>
	{
	}
}
