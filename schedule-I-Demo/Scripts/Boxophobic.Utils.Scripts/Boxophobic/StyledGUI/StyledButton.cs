using UnityEngine;

namespace Boxophobic.StyledGUI
{
	public class StyledButton : PropertyAttribute
	{
		public string Text = "";

		public float Top;

		public float Down;

		public StyledButton(string Text)
		{
			this.Text = Text;
			Top = 0f;
			Down = 0f;
		}

		public StyledButton(string Text, float Top, float Down)
		{
			this.Text = Text;
			this.Top = Top;
			this.Down = Down;
		}
	}
}
