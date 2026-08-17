using System.Collections.Generic;
using QFSW.QC.Suggestors.Tags;

namespace QFSW.QC.Suggestors
{
	public class InlineSuggestor : IQcSuggestor
	{
		public IEnumerable<IQcSuggestion> GetSuggestions(SuggestionContext context, SuggestorOptions options)
		{
			foreach (InlineSuggestionsTag tag in context.GetTags<InlineSuggestionsTag>())
			{
				foreach (string suggestion in tag.Suggestions)
				{
					yield return new RawSuggestion(suggestion, singleLiteral: true);
				}
			}
		}
	}
}
