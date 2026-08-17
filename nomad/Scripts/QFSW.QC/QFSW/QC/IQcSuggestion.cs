namespace QFSW.QC
{
	public interface IQcSuggestion
	{
		string FullSignature { get; }

		string PrimarySignature { get; }

		string SecondarySignature { get; }

		bool MatchesPrompt(string prompt);

		string GetCompletion(string prompt);

		string GetCompletionTail(string prompt);

		SuggestionContext? GetInnerSuggestionContext(SuggestionContext context);
	}
}
