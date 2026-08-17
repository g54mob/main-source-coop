using System;

namespace VInspector
{
	public class FoldoutAttribute : Attribute
	{
		public string name;

		public FoldoutAttribute(string name)
		{
			this.name = name;
		}
	}
}
