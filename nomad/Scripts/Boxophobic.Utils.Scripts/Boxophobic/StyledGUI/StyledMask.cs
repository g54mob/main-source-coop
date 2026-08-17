using UnityEngine;

namespace Boxophobic.StyledGUI
{
	public class StyledMask : PropertyAttribute
	{
		public string display = "";

		public string file = "";

		public string options = "";

		public int top;

		public int down;

		public StyledMask(string file, string options, int top, int down)
		{
			this.file = file;
			this.options = options;
			this.top = top;
			this.down = down;
		}

		public StyledMask(string display, string file, string options, int top, int down)
		{
			this.display = display;
			this.file = file;
			this.options = options;
			this.top = top;
			this.down = down;
		}
	}
}
