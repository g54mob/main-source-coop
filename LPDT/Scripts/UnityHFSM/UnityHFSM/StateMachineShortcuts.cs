using System;

namespace UnityHFSM
{
	public static class StateMachineShortcuts
	{
		public static void AddState<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TStateId name, Action<State<TStateId, TEvent>> onEnter = null, Action<State<TStateId, TEvent>> onLogic = null, Action<State<TStateId, TEvent>> onExit = null, Func<State<TStateId, TEvent>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false)
		{
			if (onEnter == null && onLogic == null && onExit == null && canExit == null)
			{
				fsm.AddState(name, new StateBase<TStateId>(needsExitTime, isGhostState));
			}
			else
			{
				fsm.AddState(name, new State<TStateId, TEvent>(onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState));
			}
		}

		private static TransitionBase<TStateId> CreateOptimizedTransition<TStateId>(TStateId from, TStateId to, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			if (condition == null && onTransition == null && afterTransition == null)
			{
				return new TransitionBase<TStateId>(from, to, forceInstantly);
			}
			return new Transition<TStateId>(from, to, condition, onTransition, afterTransition, forceInstantly);
		}

		public static void AddTransition<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TStateId from, TStateId to, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddTransition(CreateOptimizedTransition(from, to, condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddTransitionFromAny<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TStateId to, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddTransitionFromAny(CreateOptimizedTransition(default(TStateId), to, condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddTriggerTransition<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TEvent trigger, TStateId from, TStateId to, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddTriggerTransition(trigger, CreateOptimizedTransition(from, to, condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddTriggerTransitionFromAny<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TEvent trigger, TStateId to, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddTriggerTransitionFromAny(trigger, CreateOptimizedTransition(default(TStateId), to, condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddTwoWayTransition<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TStateId from, TStateId to, Func<Transition<TStateId>, bool> condition, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddTwoWayTransition(new Transition<TStateId>(from, to, condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddTwoWayTriggerTransition<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TEvent trigger, TStateId from, TStateId to, Func<Transition<TStateId>, bool> condition, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddTwoWayTriggerTransition(trigger, new Transition<TStateId>(from, to, condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddExitTransition<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TStateId from, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddExitTransition(CreateOptimizedTransition(from, default(TStateId), condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddExitTransitionFromAny<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddExitTransitionFromAny(CreateOptimizedTransition(default(TStateId), default(TStateId), condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddExitTriggerTransition<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TEvent trigger, TStateId from, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddExitTriggerTransition(trigger, CreateOptimizedTransition(from, default(TStateId), condition, onTransition, afterTransition, forceInstantly));
		}

		public static void AddExitTriggerTransitionFromAny<TOwnId, TStateId, TEvent>(this StateMachine<TOwnId, TStateId, TEvent> fsm, TEvent trigger, Func<Transition<TStateId>, bool> condition = null, Action<Transition<TStateId>> onTransition = null, Action<Transition<TStateId>> afterTransition = null, bool forceInstantly = false)
		{
			fsm.AddExitTriggerTransitionFromAny(trigger, CreateOptimizedTransition(default(TStateId), default(TStateId), condition, onTransition, afterTransition, forceInstantly));
		}
	}
}
