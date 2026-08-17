namespace VInspector
{
	public class ShowIfAttribute : IfAttribute
	{
		public ShowIfAttribute(string boolName)
			: base(boolName)
		{
		}

		public ShowIfAttribute(string variableName, object variableValue)
			: base(variableName, variableValue)
		{
		}
	}
}
