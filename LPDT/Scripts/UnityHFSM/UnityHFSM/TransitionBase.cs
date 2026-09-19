namespace UnityHFSM
{
	public class TransitionBase<TStateId> : ITransitionListener
	{
		public readonly TStateId from;

		public readonly TStateId to;

		public bool forceInstantly;

		public bool isExitTransition;

		public IStateMachine<TStateId> fsm;

		public TransitionBase(TStateId from, TStateId to, bool forceInstantly = false)
		{
			this.from = from;
			this.to = to;
			this.forceInstantly = forceInstantly;
			isExitTransition = false;
		}

		public virtual void Init()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual bool ShouldTransition()
		{
			return true;
		}

		public virtual void BeforeTransition()
		{
		}

		public virtual void AfterTransition()
		{
		}
	}
	public class TransitionBase : TransitionBase<string>
	{
		public TransitionBase(string from, string to, bool forceInstantly = false)
			: base(from, to, forceInstantly)
		{
		}
	}
}
