using UnityEngine;

namespace AeLa.EasyFeedback
{
	public class EFConfig : ScriptableObject
	{
		[Tooltip("Save feedback locally, instead of sending it to Trello")]
		public bool StoreLocal;

		public string Token;

		public FeedbackBoard Board;

		public EFConfig()
		{
			StoreLocal = true;
			Board = new FeedbackBoard();
		}
	}
}
