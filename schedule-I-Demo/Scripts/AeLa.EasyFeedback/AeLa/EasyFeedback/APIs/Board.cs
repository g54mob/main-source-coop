using System;

namespace AeLa.EasyFeedback.APIs
{
	[Serializable]
	public struct Board
	{
		public string id;

		public string name;

		public string desc;

		public object descData;

		public bool closed;

		public string idOrganization;

		public bool pinned;

		public string url;

		public string shortUrl;

		public Prefs prefs;

		public LabelNames labelNames;
	}
}
