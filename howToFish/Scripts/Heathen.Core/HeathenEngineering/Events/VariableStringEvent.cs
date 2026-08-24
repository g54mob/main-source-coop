using System;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[Serializable]
	public class VariableStringEvent : UnityEvent<StringVariable, string>
	{
	}
}
