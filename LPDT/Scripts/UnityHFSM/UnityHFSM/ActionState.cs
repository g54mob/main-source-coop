using System;

namespace UnityHFSM
{
	public class ActionState<TStateId, TEvent> : StateBase<TStateId>, IActionable<TEvent>
	{
		private ActionStorage<TEvent> actionStorage;

		public ActionState(bool needsExitTime, bool isGhostState = false)
			: base(needsExitTime, isGhostState)
		{
		}

		public ActionState<TStateId, TEvent> AddAction(TEvent trigger, Action action)
		{
			actionStorage = actionStorage ?? new ActionStorage<TEvent>();
			actionStorage.AddAction(trigger, action);
			return this;
		}

		public ActionState<TStateId, TEvent> AddAction<TData>(TEvent trigger, Action<TData> action)
		{
			actionStorage = actionStorage ?? new ActionStorage<TEvent>();
			actionStorage.AddAction(trigger, action);
			return this;
		}

		public void OnAction(TEvent trigger)
		{
			actionStorage?.RunAction(trigger);
		}

		public void OnAction<TData>(TEvent trigger, TData data)
		{
			actionStorage?.RunAction(trigger, data);
		}

		public bool HasAction(TEvent trigger)
		{
			return actionStorage?.HasAction(trigger) ?? false;
		}
	}
	public class ActionState<TStateId> : ActionState<TStateId, string>
	{
		public ActionState(bool needsExitTime, bool isGhostState = false)
			: base(needsExitTime, isGhostState)
		{
		}
	}
	public class ActionState : ActionState<string, string>
	{
		public ActionState(bool needsExitTime, bool isGhostState = false)
			: base(needsExitTime, isGhostState)
		{
		}
	}
}
