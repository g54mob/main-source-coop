namespace QFSW.QC.Suggestors.Tags
{
	public sealed class CommandNameAttribute : SuggestorTagAttribute
	{
		private readonly IQcSuggestorTag[] _tags = new IQcSuggestorTag[1] { default(CommandNameTag) };

		public override IQcSuggestorTag[] GetSuggestorTags()
		{
			return _tags;
		}
	}
}
