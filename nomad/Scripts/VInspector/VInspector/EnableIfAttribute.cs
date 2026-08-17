namespace VInspector
{
	public class EnableIfAttribute : IfAttribute
	{
		public EnableIfAttribute(string boolName)
			: base(boolName)
		{
		}

		public EnableIfAttribute(string variableName, object variableValue)
			: base(variableName, variableValue)
		{
		}
	}
}
