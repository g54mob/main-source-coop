using System.Collections.Generic;

namespace QFSW.QC.Suggestors.Tags
{
	public struct InlineSuggestionsTag : IQcSuggestorTag
	{
		public readonly IEnumerable<string> Suggestions;

		public InlineSuggestionsTag(IEnumerable<string> suggestions)
		{
			Suggestions = suggestions;
		}
	}
}
