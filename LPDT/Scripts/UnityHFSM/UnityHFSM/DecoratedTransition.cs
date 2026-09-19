using System;

namespace UnityHFSM
{
	public class DecoratedTransition<TStateId> : TransitionBase<TStateId>
	{
		private readonly Action<TransitionBase<TStateId>> beforeOnEnter;

		private readonly Action<TransitionBase<TStateId>> afterOnEnter;

		private readonly Action<TransitionBase<TStateId>> beforeShouldTransition;

		private readonly Action<TransitionBase<TStateId>> afterShouldTransition;

		private readonly Action<TransitionBase<TStateId>> beforeOnTransition;

		private readonly Action<TransitionBase<TStateId>> afterOnTransition;

		private readonly Action<TransitionBase<TStateId>> beforeAfterTransition;

		private readonly Action<TransitionBase<TStateId>> afterAfterTransition;

		public readonly TransitionBase<TStateId> transition;

		public DecoratedTransition(TransitionBase<TStateId> transition, Action<TransitionBase<TStateId>> beforeOnEnter = null, Action<TransitionBase<TStateId>> afterOnEnter = null, Action<TransitionBase<TStateId>> beforeShouldTransition = null, Action<TransitionBase<TStateId>> afterShouldTransition = null, Action<TransitionBase<TStateId>> beforeOnTransition = null, Action<TransitionBase<TStateId>> afterOnTransition = null, Action<TransitionBase<TStateId>> beforeAfterTransition = null, Action<TransitionBase<TStateId>> afterAfterTransition = null)
			: base(transition.from, transition.to, transition.forceInstantly)
		{
			this.transition = transition;
			this.beforeOnEnter = beforeOnEnter;
			this.afterOnEnter = afterOnEnter;
			this.beforeShouldTransition = beforeShouldTransition;
			this.afterShouldTransition = afterShouldTransition;
			this.beforeOnTransition = beforeOnTransition;
			this.afterOnTransition = afterOnTransition;
			this.beforeAfterTransition = beforeAfterTransition;
			this.afterAfterTransition = afterAfterTransition;
		}

		public override void Init()
		{
			transition.fsm = fsm;
		}

		public override void OnEnter()
		{
			beforeOnEnter?.Invoke(transition);
			transition.OnEnter();
			afterOnEnter?.Invoke(transition);
		}

		public override bool ShouldTransition()
		{
			beforeShouldTransition?.Invoke(transition);
			bool result = transition.ShouldTransition();
			Action<TransitionBase<TStateId>> action = afterShouldTransition;
			if (action != null)
			{
				action(transition);
				return result;
			}
			return result;
		}

		public override void BeforeTransition()
		{
			beforeOnTransition?.Invoke(transition);
			transition.BeforeTransition();
			afterOnTransition?.Invoke(transition);
		}

		public override void AfterTransition()
		{
			beforeAfterTransition?.Invoke(transition);
			transition.AfterTransition();
			afterAfterTransition?.Invoke(transition);
		}
	}
	public class DecoratedTransition : DecoratedTransition<string>
	{
		public DecoratedTransition(TransitionBase<string> transition, Action<TransitionBase<string>> beforeOnEnter = null, Action<TransitionBase<string>> afterOnEnter = null, Action<TransitionBase<string>> beforeShouldTransition = null, Action<TransitionBase<string>> afterShouldTransition = null, Action<TransitionBase<string>> beforeOnTransition = null, Action<TransitionBase<string>> afterOnTransition = null, Action<TransitionBase<string>> beforeAfterTransition = null, Action<TransitionBase<string>> afterAfterTransition = null)
			: base(transition, beforeOnEnter, afterOnEnter, beforeShouldTransition, afterShouldTransition, beforeOnTransition, afterOnTransition, beforeAfterTransition, afterAfterTransition)
		{
		}
	}
}
