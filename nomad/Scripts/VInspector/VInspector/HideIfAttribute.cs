namespace VInspector
{
	public class HideIfAttribute : IfAttribute
	{
		public HideIfAttribute(string boolName)
			: base(boolName)
		{
		}

		public HideIfAttribute(string variableName, object variableValue)
			: base(variableName, variableValue)
		{
		}
	}
}
