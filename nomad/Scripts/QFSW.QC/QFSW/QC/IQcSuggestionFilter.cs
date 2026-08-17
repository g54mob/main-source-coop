namespace QFSW.QC
{
	public interface IQcSuggestionFilter
	{
		bool IsSuggestionPermitted(IQcSuggestion suggestion, SuggestionContext context);
	}
}
