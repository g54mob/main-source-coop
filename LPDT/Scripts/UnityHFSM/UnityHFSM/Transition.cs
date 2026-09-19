using System;

namespace UnityHFSM
{
	public class Transition<TStateId> : TransitionBase<TStateId>
	{
		private readonly Func<Transition<TStateId>, bool> condition;

		private readonly Action<Transition<TStateId>> beforeTransition;

		private readonly Action<Transition<TStateId>> afterTransition;

		public Transition(TStateId from, TStateId to, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
			: base(from, to, forceInstantly)
		{
			this.condition = condition;
			beforeTransition = onTransition;
			this.afterTransition = afterTransition;
		}

		public override bool ShouldTransition()
		{
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
	public class Transition : Transition<string>
	{
		public Transition(string from, string to, Func<Transition<string>, bool> condition = null, Action<Transition<string>> onTransition = null, Action<Transition<string>> afterTransition = null, bool forceInstantly = false)
			: base(from, to, condition, onTransition, afterTransition, forceInstantly)
		{
		}
	}
}
