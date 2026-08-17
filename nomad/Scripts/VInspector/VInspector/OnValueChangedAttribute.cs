using System;

namespace VInspector
{
	[AttributeUsage(AttributeTargets.Method)]
	public class OnValueChangedAttribute : Attribute
	{
		public string[] variableOrGroupNames;

		public OnValueChangedAttribute(string name)
		{
			variableOrGroupNames = new string[1] { name };
		}

		public OnValueChangedAttribute(params string[] names)
		{
			variableOrGroupNames = names;
		}
	}
}
