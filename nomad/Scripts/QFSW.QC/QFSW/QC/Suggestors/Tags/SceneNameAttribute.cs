namespace QFSW.QC.Suggestors.Tags
{
	public sealed class SceneNameAttribute : SuggestorTagAttribute
	{
		private SceneNameTag _tag;

		public bool LoadedOnly
		{
			get
			{
				return _tag.LoadedOnly;
			}
			set
			{
				_tag.LoadedOnly = value;
			}
		}

		public override IQcSuggestorTag[] GetSuggestorTags()
		{
			return new IQcSuggestorTag[1] { _tag };
		}
	}
}
