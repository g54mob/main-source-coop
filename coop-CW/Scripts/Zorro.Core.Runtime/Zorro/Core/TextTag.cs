namespace Zorro.Core
{
	public abstract class TextTag
	{
		private TextTag m_otherTag;

		public string TextBeforeTag { get; protected set; }

		public string LastText { get; protected set; }

		public bool IsOpeningTag { get; protected set; }

		public void Setup(string textBeforeTag)
		{
			IsOpeningTag = true;
			TextBeforeTag = textBeforeTag;
			m_otherTag = null;
			LastText = "";
		}

		public void Setup(string textBeforeTag, TextTag otherTag)
		{
			IsOpeningTag = false;
			TextBeforeTag = textBeforeTag;
			m_otherTag = otherTag;
			LastText = "";
		}

		public virtual void ParseParameter(string param)
		{
		}

		public T GetOtherTag<T>() where T : TextTag
		{
			return (T)m_otherTag;
		}

		public void SetLastText(string lastText)
		{
			LastText = lastText;
		}
	}
}
