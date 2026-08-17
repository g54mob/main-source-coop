namespace VInspector
{
	public class DisableIfAttribute : IfAttribute
	{
		public DisableIfAttribute(string boolName)
			: base(boolName)
		{
		}

		public DisableIfAttribute(string variableName, object variableValue)
			: base(variableName, variableValue)
		{
		}
	}
}
