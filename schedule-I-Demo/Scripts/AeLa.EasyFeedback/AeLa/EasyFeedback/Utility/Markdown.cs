using System;

namespace AeLa.EasyFeedback.Utility
{
	public static class Markdown
	{
		public enum HeaderLevel
		{
			H1 = 0,
			H2 = 1,
			H3 = 2,
			H4 = 3,
			H5 = 4,
			H6 = 5
		}

		private const string _H1 = "# ";

		private const string _H2 = "## ";

		private const string _H3 = "### ";

		private const string _H4 = "#### ";

		private const string _H5 = "##### ";

		private const string _H6 = "###### ";

		private const string _EM = "*";

		private const string _STRONG = "**";

		private const string _STRIKE = "~~";

		private const string _UL = "- ";

		private const string _OL = ". ";

		private const string _LINK_INLINE_PRE = "[";

		private const string _LINK_INLINE_MID = "](";

		private const string _LINK_INLINE_END = ")";

		private const string _IMG_INLINE_PRE = "![";

		private const string _IMG_INLINE_MID = "](";

		private const string _IMG_INLINE_END = ")";

		private const string _CODE_INLINE = "`";

		private const string _CODE_BLOCK = "```";

		private const string _QUOTE = "> ";

		private const string _HR = "---";

		private const string _ENDL = "\n";

		private const string _LB = "\n\n";

		public const string HR = "---";

		public const string LINE_BREAK = "\n\n";

		public static string Header(string text, HeaderLevel level = HeaderLevel.H1)
		{
			return level switch
			{
				HeaderLevel.H1 => "# " + text, 
				HeaderLevel.H2 => "## " + text, 
				HeaderLevel.H3 => "### " + text, 
				HeaderLevel.H4 => "#### " + text, 
				HeaderLevel.H5 => "##### " + text, 
				HeaderLevel.H6 => "###### " + text, 
				_ => throw new ArgumentException("The header level value '" + level.ToString() + "' is invalid."), 
			};
		}

		public static string H1(string text)
		{
			return Header(text);
		}

		public static string H2(string text)
		{
			return Header(text, HeaderLevel.H2);
		}

		public static string H3(string text)
		{
			return Header(text, HeaderLevel.H3);
		}

		public static string H4(string text)
		{
			return Header(text, HeaderLevel.H4);
		}

		public static string H5(string text)
		{
			return Header(text, HeaderLevel.H5);
		}

		public static string H6(string text)
		{
			return Header(text, HeaderLevel.H6);
		}

		public static string Em(string text)
		{
			return "*" + text + "*";
		}

		public static string Strong(string text)
		{
			return "**" + text + "**";
		}

		public static string Strike(string text)
		{
			return "~~" + text + "~~";
		}

		public static string UnorderedList(string[] items)
		{
			string text = string.Empty;
			for (int i = 0; i < items.Length; i++)
			{
				text = text + "- " + items[i] + "\n";
			}
			return text;
		}

		public static string OrderedList(string[] items)
		{
			string text = string.Empty;
			for (int i = 0; i < items.Length; i++)
			{
				text = text + (i + 1) + ". " + items[i] + "\n";
			}
			return text;
		}

		public static string Hyperlink(string text, string url)
		{
			return "[" + text + "](" + url + ")";
		}

		public static string Image(string url, string alt = "")
		{
			return "![" + alt + "](" + url + ")";
		}

		public static string Code(string text)
		{
			return "`" + text + "`";
		}

		public static string CodeBlock(string text, string language = "")
		{
			return "```" + language + "\n" + text + "\n```";
		}

		public static string Blockquote(string text)
		{
			return "> " + text;
		}
	}
}
