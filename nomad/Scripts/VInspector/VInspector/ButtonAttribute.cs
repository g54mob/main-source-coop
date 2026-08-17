using System;

namespace VInspector
{
	public class ButtonAttribute : Attribute
	{
		public string name = "";

		public int size = 30;

		public int space;

		public string color = "Grey";

		public ButtonAttribute()
		{
			name = "";
		}

		public ButtonAttribute(string name)
		{
			this.name = name;
		}
	}
}
