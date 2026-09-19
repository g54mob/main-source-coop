using System;

namespace UnityHFSM
{
	public class TransitionAfterDynamic<TStateId> : TransitionBase<TStateId>
	{
		public ITimer timer;

		private float delay;

		private readonly bool onlyEvaluateDelayOnEnter;

		private readonly Func<TransitionAfterDynamic<TStateId>, float> delayCalculator;

		private readonly Func<TransitionAfterDynamic<TStateId>, bool> condition;

		private readonly Action<TransitionAfterDynamic<TStateId>> beforeTransition;

		private readonly Action<TransitionAfterDynamic<TStateId>> afterTransition;

		public TransitionAfterDynamic(TStateId from, TStateId to, Func<TransitionAfterDynamic<TStateId>, float> delay, Func<TransitionAfterDynamic<TStateId>, bool> condition = null, bool onlyEvaluateDelayOnEnter = false, Action<TransitionAfterDynamic<TStateId>> onTransition = null, Action<TransitionAfterDynamic<TStateId>> afterTransition = null, bool forceInstantly = false)
			: base(from, to, forceInstantly)
		{
			delayCalculator = delay;
			this.condition = condition;
			this.onlyEvaluateDelayOnEnter = onlyEvaluateDelayOnEnter;
			beforeTransition = onTransition;
			this.afterTransition = afterTransition;
			timer = new Timer();
		}

		public override void OnEnter()
		{
			timer.Reset();
			if (onlyEvaluateDelayOnEnter)
			{
				delay = delayCalculator(this);
			}
		}

		public override bool ShouldTransition()
		{
			if (!onlyEvaluateDelayOnEnter)
			{
				delay = delayCalculator(this);
			}
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
	public class TransitionAfterDynamic : TransitionAfterDynamic<string>
	{
		public TransitionAfterDynamic(string from, string to, Func<TransitionAfterDynamic<string>, float> delay, Func<TransitionAfterDynamic<string>, bool> condition = null, bool onlyEvaluateDelayOnEnter = false, Action<TransitionAfterDynamic<string>> onTransition = null, Action<TransitionAfterDynamic<string>> afterTransition = null, bool forceInstantly = false)
			: base(from, to, delay, condition, onlyEvaluateDelayOnEnter, onTransition, afterTransition, forceInstantly)
		{
		}
	}
}
