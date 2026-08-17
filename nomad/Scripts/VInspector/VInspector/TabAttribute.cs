using System;

namespace VInspector
{
	public class TabAttribute : Attribute
	{
		public string name;

		public TabAttribute(string name)
		{
			this.name = name;
		}
	}
}
