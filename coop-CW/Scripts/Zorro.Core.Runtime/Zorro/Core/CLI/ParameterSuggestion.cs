namespace Zorro.Core.CLI
{
	public class ParameterSuggestion : Suggestion
	{
		public string DisplayLine;

		public string TypeLine;

		public string ParameterInput;

		public ParameterSuggestion(string displayLine, string typeLine, string parameterInput)
		{
			DisplayLine = displayLine;
			TypeLine = typeLine;
			ParameterInput = parameterInput;
		}

		public override string ToString()
		{
			return "<alpha=#00>" + DisplayLine + "<alpha=#FF>" + ParameterInput;
		}

		public override string GetInputValue()
		{
			return TypeLine + ParameterInput;
		}

		public override bool CanBeSelected()
		{
			return true;
		}
	}
}
