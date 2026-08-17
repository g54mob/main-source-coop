using System.Collections.Generic;

namespace QFSW.QC
{
	public interface IQcSuggestor
	{
		IEnumerable<IQcSuggestion> GetSuggestions(SuggestionContext context, SuggestorOptions options);
	}
}
