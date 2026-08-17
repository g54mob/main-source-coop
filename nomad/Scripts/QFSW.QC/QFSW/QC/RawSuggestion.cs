namespace QFSW.QC
{
	public class RawSuggestion : IQcSuggestion
	{
		private readonly string _value;

		private readonly bool _singleLiteral;

		private readonly string _completion;

		public string FullSignature => _value;

		public string PrimarySignature => _value;

		public string SecondarySignature => string.Empty;

		public RawSuggestion(string value, bool singleLiteral = false)
		{
			_value = value;
			_singleLiteral = singleLiteral;
			_completion = _value;
			if (_completion.CanSplitScoped(' ', '"', '"'))
			{
				_completion = "\"" + _completion + "\"";
			}
		}

		public bool MatchesPrompt(string prompt)
		{
			if (_singleLiteral)
			{
				prompt = prompt.Trim('"');
			}
			return prompt == _value;
		}

		public string GetCompletion(string prompt)
		{
			return _completion;
		}

		public string GetCompletionTail(string prompt)
		{
			return string.Empty;
		}

		public SuggestionContext? GetInnerSuggestionContext(SuggestionContext context)
		{
			return null;
		}
	}
}
