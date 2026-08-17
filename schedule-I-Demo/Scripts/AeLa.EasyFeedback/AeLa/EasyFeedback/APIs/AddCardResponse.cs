using System;

namespace AeLa.EasyFeedback.APIs
{
	[Serializable]
	public class AddCardResponse
	{
		public string id;

		public Badges badges;

		public bool[] checkItemStates;

		public bool closed;

		public DateTime dateLastActivity;

		public string desc;

		public Descdata descData;

		public string due;

		public bool dueComplete;

		public string email;

		public string idBoard;

		public string[] idChecklists;

		public string[] idLabels;

		public string idList;

		public string[] idMembers;

		public int idShort;

		public string idAttachmentCover;

		public bool manualCoverAttachment;

		public CardLabel[] labels;

		public string name;

		public int pos;

		public string shortUrl;

		public string url;

		public string[] stickers;
	}
}
