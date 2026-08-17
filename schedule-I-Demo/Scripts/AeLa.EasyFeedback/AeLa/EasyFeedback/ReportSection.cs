using System.Text;
using AeLa.EasyFeedback.Utility;

namespace AeLa.EasyFeedback
{
	public class ReportSection
	{
		public string Title;

		public int SortOrder;

		private StringBuilder sectionText;

		public ReportSection(string title, int sortOrder = 0)
		{
			Title = title;
			SortOrder = sortOrder;
			sectionText = new StringBuilder();
		}

		public ReportSection(string title, string text)
		{
			Title = title;
			sectionText = new StringBuilder(text);
		}

		public void Append(string text)
		{
			sectionText.Append(text);
		}

		public void AppendLine(string line)
		{
			sectionText.AppendLine(line);
		}

		public void SetText(string text)
		{
			sectionText = new StringBuilder(text);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(Markdown.H3(Title));
			stringBuilder.AppendLine(sectionText.ToString());
			return stringBuilder.ToString();
		}
	}
}
