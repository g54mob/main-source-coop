using System;

namespace AeLa.EasyFeedback.APIs
{
	[Serializable]
	public struct Label
	{
		public string id;

		public string idBoard;

		public string name;

		public string color;

		public int uses;

		public int order;

		public Label(string id = null, string idBoard = null, string name = null, string color = null, int uses = 0, int order = 0)
		{
			this.id = id;
			this.idBoard = idBoard;
			this.name = name;
			this.color = color;
			this.uses = uses;
			this.order = order;
		}
	}
}
