using System;

namespace UnityHFSM
{
	public class HybridStateMachine<TOwnId, TStateId, TEvent> : StateMachine<TOwnId, TStateId, TEvent>
	{
		private Action<HybridStateMachine<TOwnId, TStateId, TEvent>> beforeOnEnter;

		private Action<HybridStateMachine<TOwnId, TStateId, TEvent>> afterOnEnter;

		private Action<HybridStateMachine<TOwnId, TStateId, TEvent>> beforeOnLogic;

		private Action<HybridStateMachine<TOwnId, TStateId, TEvent>> afterOnLogic;

		private Action<HybridStateMachine<TOwnId, TStateId, TEvent>> beforeOnExit;

		private Action<HybridStateMachine<TOwnId, TStateId, TEvent>> afterOnExit;

		private ActionStorage<TEvent> actionStorage;

		public Timer timer;

		public HybridStateMachine(Action<HybridStateMachine<TOwnId, TStateId, TEvent>> beforeOnEnter = null, Action<HybridStateMachine<TOwnId, TStateId, TEvent>> afterOnEnter = null, Action<HybridStateMachine<TOwnId, TStateId, TEvent>> beforeOnLogic = null, Action<HybridStateMachine<TOwnId, TStateId, TEvent>> afterOnLogic = null, Action<HybridStateMachine<TOwnId, TStateId, TEvent>> beforeOnExit = null, Action<HybridStateMachine<TOwnId, TStateId, TEvent>> afterOnExit = null, bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
			: base(needsExitTime, isGhostState, rememberLastState)
		{
			this.beforeOnEnter = beforeOnEnter;
			this.afterOnEnter = afterOnEnter;
			this.beforeOnLogic = beforeOnLogic;
			this.afterOnLogic = afterOnLogic;
			this.beforeOnExit = beforeOnExit;
			this.afterOnExit = afterOnExit;
			timer = new Timer();
		}

		public override void OnEnter()
		{
			beforeOnEnter?.Invoke(this);
			base.OnEnter();
			timer.Reset();
			afterOnEnter?.Invoke(this);
		}

		public override void OnLogic()
		{
			beforeOnLogic?.Invoke(this);
			base.OnLogic();
			afterOnLogic?.Invoke(this);
		}

		public override void OnExit()
		{
			beforeOnExit?.Invoke(this);
			base.OnExit();
			afterOnExit?.Invoke(this);
		}

		public override void OnAction(TEvent trigger)
		{
			actionStorage?.RunAction(trigger);
			base.OnAction(trigger);
		}

		public override void OnAction<TData>(TEvent trigger, TData data)
		{
			actionStorage?.RunAction(trigger, data);
			base.OnAction(trigger, data);
		}

		public override bool HasAction(TEvent trigger)
		{
			ActionStorage<TEvent> obj = actionStorage;
			if (obj == null || !obj.HasAction(trigger))
			{
				return base.HasAction(trigger);
			}
			return true;
		}

		public HybridStateMachine<TOwnId, TStateId, TEvent> AddAction(TEvent trigger, Action action)
		{
			actionStorage = actionStorage ?? new ActionStorage<TEvent>();
			actionStorage.AddAction(trigger, action);
			return this;
		}

		public HybridStateMachine<TOwnId, TStateId, TEvent> AddAction<TData>(TEvent trigger, Action<TData> action)
		{
			actionStorage = actionStorage ?? new ActionStorage<TEvent>();
			actionStorage.AddAction(trigger, action);
			return this;
		}
	}
	public class HybridStateMachine<TStateId, TEvent> : HybridStateMachine<TStateId, TStateId, TEvent>
	{
		public HybridStateMachine(Action<HybridStateMachine<TStateId, TStateId, TEvent>> beforeOnEnter = null, Action<HybridStateMachine<TStateId, TStateId, TEvent>> afterOnEnter = null, Action<HybridStateMachine<TStateId, TStateId, TEvent>> beforeOnLogic = null, Action<HybridStateMachine<TStateId, TStateId, TEvent>> afterOnLogic = null, Action<HybridStateMachine<TStateId, TStateId, TEvent>> beforeOnExit = null, Action<HybridStateMachine<TStateId, TStateId, TEvent>> afterOnExit = null, bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
			: base(beforeOnEnter, afterOnEnter, beforeOnLogic, afterOnLogic, beforeOnExit, afterOnExit, needsExitTime, isGhostState, rememberLastState)
		{
		}
	}
	public class HybridStateMachine<TStateId> : HybridStateMachine<TStateId, TStateId, string>
	{
		public HybridStateMachine(Action<HybridStateMachine<TStateId, TStateId, string>> beforeOnEnter = null, Action<HybridStateMachine<TStateId, TStateId, string>> afterOnEnter = null, Action<HybridStateMachine<TStateId, TStateId, string>> beforeOnLogic = null, Action<HybridStateMachine<TStateId, TStateId, string>> afterOnLogic = null, Action<HybridStateMachine<TStateId, TStateId, string>> beforeOnExit = null, Action<HybridStateMachine<TStateId, TStateId, string>> afterOnExit = null, bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
			: base(beforeOnEnter, afterOnEnter, beforeOnLogic, afterOnLogic, beforeOnExit, afterOnExit, needsExitTime, isGhostState, rememberLastState)
		{
		}
	}
	public class HybridStateMachine : HybridStateMachine<string, string, string>
	{
		public HybridStateMachine(Action<HybridStateMachine<string, string, string>> beforeOnEnter = null, Action<HybridStateMachine<string, string, string>> afterOnEnter = null, Action<HybridStateMachine<string, string, string>> beforeOnLogic = null, Action<HybridStateMachine<string, string, string>> afterOnLogic = null, Action<HybridStateMachine<string, string, string>> beforeOnExit = null, Action<HybridStateMachine<string, string, string>> afterOnExit = null, bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
			: base(beforeOnEnter, afterOnEnter, beforeOnLogic, afterOnLogic, beforeOnExit, afterOnExit, needsExitTime, isGhostState, rememberLastState)
		{
		}
	}
}
