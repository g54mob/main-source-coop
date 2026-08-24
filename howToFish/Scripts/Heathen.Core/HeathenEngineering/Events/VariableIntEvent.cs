using System;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class VariableIntEvent : UnityEvent<IntVariable, int>
	{
	}
}
