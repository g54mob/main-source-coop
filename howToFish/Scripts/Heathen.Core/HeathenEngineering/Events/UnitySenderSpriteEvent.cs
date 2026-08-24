using System;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class UnitySenderSpriteEvent : UnityEvent<GameObject, Sprite>
	{
	}
}
