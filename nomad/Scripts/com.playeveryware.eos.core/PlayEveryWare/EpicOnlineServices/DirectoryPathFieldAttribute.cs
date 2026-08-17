using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Field)]
	public class DirectoryPathFieldAttribute : ConfigFieldAttribute
	{
		public DirectoryPathFieldAttribute(string label, string tooltip = null, int group = -1)
			: base(label, ConfigFieldType.DirectoryPath, tooltip, group)
		{
		}
	}
}
