using UnityEngine;
using UnityEngine.UI;

namespace UIControllers
{
	public class AssistMenuController : UIController
	{
		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private Toggle _infiniteJumps;

		[SerializeField]
		private Toggle _pullRope;

		[SerializeField]
		private Toggle _checkpoints;

		public override void Start()
		{
			base.Start();
			_animator.SetTrigger("Start");
			_infiniteJumps.SetIsOnWithoutNotify(StaticInstance<AssistModeManager>.Instance.InfiniteJumps);
			_checkpoints.SetIsOnWithoutNotify(StaticInstance<AssistModeManager>.Instance.CheckPoints);
			_pullRope.SetIsOnWithoutNotify(StaticInstance<AssistModeManager>.Instance.PullingRope);
		}

		public override void Exit()
		{
			base.Exit();
			_animator.Play("QuitAssistMode");
		}

		public void UnloadScene()
		{
			StaticInstance<TransitionSystem>.Instance.UnloadScene(Scenes.AssistModeMenu);
		}

		public void ToggleCheckPoints()
		{
			StaticInstance<AssistModeManager>.Instance.CheckPoints = !StaticInstance<AssistModeManager>.Instance.CheckPoints;
		}

		public void TogglePullingRope()
		{
			StaticInstance<AssistModeManager>.Instance.PullingRope = !StaticInstance<AssistModeManager>.Instance.PullingRope;
		}

		public void ToggleInfiniteJumps()
		{
			StaticInstance<AssistModeManager>.Instance.InfiniteJumps = !StaticInstance<AssistModeManager>.Instance.InfiniteJumps;
		}
	}
}
