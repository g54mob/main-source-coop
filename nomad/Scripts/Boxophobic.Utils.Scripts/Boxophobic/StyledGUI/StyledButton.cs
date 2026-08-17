using UnityEngine;

namespace Boxophobic.StyledGUI
{
	public class StyledButton : PropertyAttribute
	{
		public string text = "";

		public float top;

		public float down;

		public StyledButton(string text)
		{
			this.text = text;
			top = 0f;
			down = 0f;
		}

		public StyledButton(string text, float top, float down)
		{
			this.text = text;
			this.top = top;
			this.down = down;
		}
	}
}
