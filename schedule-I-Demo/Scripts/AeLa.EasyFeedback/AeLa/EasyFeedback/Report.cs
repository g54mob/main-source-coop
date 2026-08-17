using System.Collections.Generic;
using System.Linq;
using System.Text;
using AeLa.EasyFeedback.APIs;
using AeLa.EasyFeedback.Utility;
using UnityEngine;

namespace AeLa.EasyFeedback
{
	public class Report
	{
		private const int MAX_ATTACHMENTS = 99;

		public readonly List<Label> Labels = new List<Label>();

		private readonly Dictionary<string, ReportSection> info;

		public List List;

		public string Title;

		public List<FileAttachment> Attachments { get; }

		public ReportSection this[string sectionTitle]
		{
			get
			{
				if (info.ContainsKey(sectionTitle))
				{
					return info[sectionTitle];
				}
				Debug.LogError("Report does not contain a section with title \"" + sectionTitle + "\"");
				return null;
			}
			set
			{
				if (info.ContainsKey(sectionTitle))
				{
					info[sectionTitle] = value;
				}
				else
				{
					Debug.LogError("Report does not contain a section with title \"" + sectionTitle + "\"");
				}
			}
		}

		public Report()
		{
			info = new Dictionary<string, ReportSection>();
			Attachments = new List<FileAttachment>();
		}

		public void AddSection(string title, int sortOrder = 0)
		{
			AddSection(new ReportSection(title, sortOrder));
		}

		public void AddSection(ReportSection section)
		{
			if (info.ContainsKey(section.Title))
			{
				Debug.LogError("Report already contains a section with title \"" + section.Title + "\"");
			}
			else
			{
				info.Add(section.Title, section);
			}
		}

		public void RemoveSection(string title)
		{
			if (!info.ContainsKey(title))
			{
				Debug.LogWarning("Can not remove section \"" + title + "\" because report does not contain a section with that name");
			}
			else
			{
				info.Remove(title);
			}
		}

		public bool HasSection(string title)
		{
			return info.ContainsKey(title);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			ReportSection[] array = (from r in info
				select r.Value into v
				orderby v.SortOrder
				select v).ToArray();
			foreach (ReportSection reportSection in array)
			{
				stringBuilder.AppendLine(reportSection.ToString());
			}
			return stringBuilder.ToString();
		}

		public string GetLocalFileText()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(Markdown.H3("Category"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(List.name);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(Markdown.H3("Labels"));
			stringBuilder.AppendLine();
			foreach (Label label in Labels)
			{
				stringBuilder.AppendLine("- " + label.name);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(ToString());
			return stringBuilder.ToString();
		}

		public void AttachFile(FileAttachment file)
		{
			if (Attachments.Count + 1 > 99)
			{
				Debug.LogError("Error attaching file: maximum attachment limit (" + 99 + ") reached!");
			}
			else
			{
				Attachments.Add(file);
			}
		}

		public void AttachFile(string name, string filePath)
		{
			AttachFile(new FileAttachment(name, filePath, null));
		}

		public void AttachFile(string name, byte[] data)
		{
			AttachFile(new FileAttachment(name, data));
		}

		public void AddLabel(Label label)
		{
			if (HasLabel(label))
			{
				Debug.LogWarning("The report already has the label \"" + label.name + "\"");
			}
			else
			{
				Labels.Add(label);
			}
		}

		public bool HasLabel(Label label)
		{
			return Labels.Contains(label);
		}
	}
}
