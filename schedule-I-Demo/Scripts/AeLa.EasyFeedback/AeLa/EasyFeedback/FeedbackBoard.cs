using System;
using AeLa.EasyFeedback.APIs;

namespace AeLa.EasyFeedback
{
	[Serializable]
	public class FeedbackBoard
	{
		public string Id;

		public string[] ListNames;

		public string[] ListIds;

		public string[] CategoryNames = new string[2] { "Feedback", "Bug" };

		public string[] CategoryIds = new string[2];

		public Label[] Labels = new Label[3]
		{
			new Label("1", null, "Low Priority"),
			new Label("2", null, "Medium Priority"),
			new Label("3", null, "High Priority")
		};
	}
}
