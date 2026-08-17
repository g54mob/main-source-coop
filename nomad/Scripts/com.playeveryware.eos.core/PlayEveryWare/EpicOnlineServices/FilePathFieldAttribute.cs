using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Field)]
	public class FilePathFieldAttribute : ConfigFieldAttribute
	{
		public string Extension { get; }

		public FilePathFieldAttribute(string label, string extension, string tooltip = null, int group = -1)
			: base(label, ConfigFieldType.FilePath, tooltip, group)
		{
			Extension = extension;
		}
	}
}
