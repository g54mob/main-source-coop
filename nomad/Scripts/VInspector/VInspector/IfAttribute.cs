using System;

namespace VInspector
{
	public abstract class IfAttribute : Attribute
	{
		public string variableName;

		public object variableValue;

		public IfAttribute(string boolName)
		{
			variableName = boolName;
			variableValue = true;
		}

		public IfAttribute(string variableName, object variableValue)
		{
			this.variableName = variableName;
			this.variableValue = variableValue;
		}
	}
}
