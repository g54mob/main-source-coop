using System;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class VariableFloatEvent : UnityEvent<FloatVariable, float>
	{
	}
}
