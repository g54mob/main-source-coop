using System;

namespace Zorro.Core
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TagAttribute : Attribute
	{
		public string Tag { get; set; }

		public TagAttribute(string tag)
		{
			Tag = tag;
		}
	}
}
