using System;

namespace AeLa.EasyFeedback.APIs
{
	[Serializable]
	public class Badges
	{
		public int votes;

		public bool viewingMemberVoted;

		public bool subscribed;

		public string fogbugz;

		public int checkItems;

		public int checkItemsChecked;

		public int comments;

		public int attachments;

		public bool description;

		public string due;

		public bool dueComplete;
	}
}
