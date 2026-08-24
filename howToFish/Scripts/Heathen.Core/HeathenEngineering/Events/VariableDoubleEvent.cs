using System;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class VariableDoubleEvent : UnityEvent<DoubleVariable, double>
	{
	}
}
