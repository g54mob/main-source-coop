namespace UnityHFSM
{
	public class ReverseTransition<TStateId> : TransitionBase<TStateId>
	{
		public readonly TransitionBase<TStateId> wrappedTransition;

		private readonly bool shouldInitWrappedTransition;

		public ReverseTransition(TransitionBase<TStateId> wrappedTransition, bool shouldInitWrappedTransition = true)
			: base(wrappedTransition.to, wrappedTransition.from, wrappedTransition.forceInstantly)
		{
			this.wrappedTransition = wrappedTransition;
			this.shouldInitWrappedTransition = shouldInitWrappedTransition;
		}

		public override void Init()
		{
			if (shouldInitWrappedTransition)
			{
				wrappedTransition.fsm = fsm;
				wrappedTransition.Init();
			}
		}

		public override void OnEnter()
		{
			wrappedTransition.OnEnter();
		}

		public override bool ShouldTransition()
		{
			return !wrappedTransition.ShouldTransition();
		}

		public override void BeforeTransition()
		{
			wrappedTransition.AfterTransition();
		}

		public override void AfterTransition()
		{
			wrappedTransition.BeforeTransition();
		}
	}
	public class ReverseTransition : ReverseTransition<string>
	{
		public ReverseTransition(TransitionBase<string> wrappedTransition, bool shouldInitWrappedTransition = true)
			: base(wrappedTransition, shouldInitWrappedTransition)
		{
		}
	}
}
