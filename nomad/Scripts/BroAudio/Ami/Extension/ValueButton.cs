using System;
using UnityEngine;

namespace Ami.Extension
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ValueButton : PropertyAttribute
	{
		public string Label;

		public object Value;

		public float ButtonWidth;

		public ValueButton(string label, object value, float buttonWidth = -1f)
		{
			Label = label;
			Value = value;
			ButtonWidth = buttonWidth;
		}
	}
}
