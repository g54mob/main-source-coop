using System;

namespace AeLa.EasyFeedback.APIs
{
	[Serializable]
	public struct List
	{
		public string id;

		public string name;

		public bool closed;

		public string idBoard;

		public float pos;

		public bool subscribed;
	}
}
