using UnityEngine;

namespace EvilCore.UI.Scripts
{
	[CreateAssetMenu(fileName = "FeedbackStyleConfig", menuName = "EvilCore/UI/Feedback Style Config")]
	public class FeedbackStyleConfig : ScriptableObject
	{
		[SerializeField]
		private FeedbackStyle info;

		[SerializeField]
		private FeedbackStyle warning;

		[SerializeField]
		private FeedbackStyle error;

		[SerializeField]
		private FeedbackStyle success;

		public FeedbackStyle Get(FeedbackType type)
		{
			return type switch
			{
				FeedbackType.Warning => warning, 
				FeedbackType.Error => error, 
				FeedbackType.Success => success, 
				_ => info, 
			};
		}
	}
}
