using UnityEngine;

namespace Features.MainMenuModule.Scripts.Views.Chapters.SupView
{
	public class ChapterProgressItemSupView : MonoBehaviour
	{
		private static readonly int StateHash = Animator.StringToHash("State");

		[SerializeField]
		private Animator _stateAnimator;

		public void SetState(ChapterProgressItemState state)
		{
			_stateAnimator.SetInteger(StateHash, (int)state);
		}
	}
}
