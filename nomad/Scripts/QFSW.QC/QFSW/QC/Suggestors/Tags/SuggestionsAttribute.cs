using System.Linq;

namespace QFSW.QC.Suggestors.Tags
{
	public sealed class SuggestionsAttribute : SuggestorTagAttribute
	{
		private readonly IQcSuggestorTag[] _tags;

		public SuggestionsAttribute(params object[] suggestions)
		{
			InlineSuggestionsTag inlineSuggestionsTag = new InlineSuggestionsTag(suggestions.Select((object o) => o.ToString()));
			_tags = new IQcSuggestorTag[1] { inlineSuggestionsTag };
		}

		public override IQcSuggestorTag[] GetSuggestorTags()
		{
			return _tags;
		}
	}
}
