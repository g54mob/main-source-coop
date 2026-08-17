using UnityEngine;

namespace Boxophobic.StyledGUI
{
	public class StyledMessage : PropertyAttribute
	{
		public string type;

		public string message;

		public float top;

		public float down;

		public StyledMessage(string type, string message)
		{
			this.type = type;
			this.message = message;
			top = 0f;
			down = 0f;
		}

		public StyledMessage(string type, string message, float top, float down)
		{
			this.type = type;
			this.message = message;
			this.top = top;
			this.down = down;
		}
	}
}
