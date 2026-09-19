using System;

namespace UnityHFSM
{
	public class TransitionAfter<TStateId> : TransitionBase<TStateId>
	{
		public float delay;

		public ITimer timer;

		private readonly Func<TransitionAfter<TStateId>, bool> condition;

		private readonly Action<TransitionAfter<TStateId>> beforeTransition;

		private readonly Action<TransitionAfter<TStateId>> afterTransition;

		public TransitionAfter(TStateId from, TStateId to, float delay, Func<TransitionAfter<TStateId>, bool> condition = null, Action<TransitionAfter<TStateId>> onTransition = null, Action<TransitionAfter<TStateId>> afterTransition = null, bool forceInstantly = false)
			: base(from, to, forceInstantly)
		{
			this.delay = delay;
			this.condition = condition;
			beforeTransition = onTransition;
			this.afterTransition = afterTransition;
			timer = new Timer();
		}

		public override void OnEnter()
		{
			timer.Reset();
		}

		public override bool ShouldTransition()
		{
			if (timer.Elapsed < delay)
			{
				return false;
			}
			if (condition == null)
			{
				return true;
			}
			return condition(this);
		}

		public override void BeforeTransition()
		{
			beforeTransition?.Invoke(this);
		}

		public override void AfterTransition()
		{
			afterTransition?.Invoke(this);
		}
	}
	public class TransitionAfter : TransitionAfter<string>
	{
		public TransitionAfter(string from, string to, float delay, Func<TransitionAfter<string>, bool> condition = null, Action<TransitionAfter<string>> onTransition = null, Action<TransitionAfter<string>> afterTransition = null, bool forceInstantly = false)
			: base(from, to, delay, condition, onTransition, afterTransition, forceInstantly)
		{
		}
	}
}
