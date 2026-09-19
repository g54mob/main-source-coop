using System;

namespace UnityHFSM
{
	public class State<TStateId, TEvent> : ActionState<TStateId, TEvent>
	{
		private readonly Action<State<TStateId, TEvent>> onEnter;

		private readonly Action<State<TStateId, TEvent>> onLogic;

		private readonly Action<State<TStateId, TEvent>> onExit;

		private readonly Func<State<TStateId, TEvent>, bool> canExit;

		public ITimer timer;

		public State(Action<State<TStateId, TEvent>> onEnter = null, Action<State<TStateId, TEvent>> onLogic = null, Action<State<TStateId, TEvent>> onExit = null, Func<State<TStateId, TEvent>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false)
			: base(needsExitTime, isGhostState)
		{
			this.onEnter = onEnter;
			this.onLogic = onLogic;
			this.onExit = onExit;
			this.canExit = canExit;
			timer = new Timer();
		}

		public override void OnEnter()
		{
			timer.Reset();
			onEnter?.Invoke(this);
		}

		public override void OnLogic()
		{
			onLogic?.Invoke(this);
			if (needsExitTime && canExit != null && fsm.HasPendingTransition && canExit(this))
			{
				fsm.StateCanExit();
			}
		}

		public override void OnExit()
		{
			onExit?.Invoke(this);
		}

		public override void OnExitRequest()
		{
			if (canExit != null && canExit(this))
			{
				fsm.StateCanExit();
			}
		}
	}
	public class State<TStateId> : State<TStateId, string>
	{
		public State(Action<State<TStateId, string>> onEnter = null, Action<State<TStateId, string>> onLogic = null, Action<State<TStateId, string>> onExit = null, Func<State<TStateId, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false)
			: base(onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
		{
		}
	}
	public class State : State<string, string>
	{
		public State(Action<State<string, string>> onEnter = null, Action<State<string, string>> onLogic = null, Action<State<string, string>> onExit = null, Func<State<string, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false)
			: base(onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
		{
		}
	}
}
