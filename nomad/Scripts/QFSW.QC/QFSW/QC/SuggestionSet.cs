using System.Collections.Generic;

namespace QFSW.QC
{
	public class SuggestionSet
	{
		public SuggestionContext Context;

		public int SelectionIndex;

		public readonly List<IQcSuggestion> Suggestions = new List<IQcSuggestion>();

		public IQcSuggestion CurrentSelection
		{
			get
			{
				if (SelectionIndex < 0 || SelectionIndex >= Suggestions.Count)
				{
					return null;
				}
				return Suggestions[SelectionIndex];
			}
		}
	}
}
