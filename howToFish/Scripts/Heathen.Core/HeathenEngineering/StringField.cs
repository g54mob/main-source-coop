using HeathenEngineering.Events;
using UnityEngine;

namespace HeathenEngineering
{
	[CreateAssetMenu(menuName = "System Core/Application/String Field")]
	public class StringField : ScriptableObject
	{
		public uint Id;

		public string defaultValue;

		[HideInInspector]
		public string activeValue;

		[HideInInspector]
		public UnityStringEvent ValueChanged;

		public string Value
		{
			get
			{
				if (!string.IsNullOrEmpty(activeValue))
				{
					return activeValue;
				}
				return defaultValue;
			}
			set
			{
				if (activeValue != value)
				{
					activeValue = value;
					ValueChanged.Invoke(activeValue);
				}
			}
		}

		public void ApplyDefault()
		{
			if (activeValue != defaultValue)
			{
				activeValue = defaultValue;
				ValueChanged.Invoke(activeValue);
			}
		}
	}
}
