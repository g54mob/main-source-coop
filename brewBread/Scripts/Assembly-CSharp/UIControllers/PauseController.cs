using UnityEngine;

namespace UIControllers
{
	public class PauseController : UIController
	{
		[SerializeField]
		private Animator _pauseAnimator;

		public override void Start()
		{
			base.Start();
			StaticInstance<Pause>.Instance.SetPause(value: true);
		}

		public void UnloadScene()
		{
			StaticInstance<TransitionSystem>.Instance.UnloadScene(Scenes.Pause);
		}

		public override void Exit()
		{
			base.Exit();
			if (!SkipExit())
			{
				_pauseAnimator.Play("FadeOut_Pause");
				StaticInstance<Pause>.Instance.SetPause(value: false);
			}
		}
	}
}
