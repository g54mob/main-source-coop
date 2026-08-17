using System;

namespace PlayEveryWare.EpicOnlineServices
{
	[AttributeUsage(AttributeTargets.Field)]
	public class ButtonFieldAttribute : ConfigFieldAttribute
	{
		public ButtonFieldAttribute(string label, string tooltip = null, int group = -1)
			: base(label, ConfigFieldType.Button, tooltip, group)
		{
		}
	}
}
