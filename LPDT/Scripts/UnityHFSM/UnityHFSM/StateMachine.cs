using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityHFSM.Exceptions;
using UnityHFSM.Inspection;

namespace UnityHFSM
{
	public class StateMachine<TOwnId, TStateId, TEvent> : StateBase<TOwnId>, ITriggerable<TEvent>, IStateMachine<TStateId>, IStateTimingManager, IActionable<TEvent>
	{
		private class StateBundle
		{
			public StateBase<TStateId> state;

			public List<TransitionBase<TStateId>> transitions;

			public Dictionary<TEvent, List<TransitionBase<TStateId>>> triggerToTransitions;

			public void AddTransition(TransitionBase<TStateId> t)
			{
				transitions = transitions ?? new List<TransitionBase<TStateId>>();
				transitions.Add(t);
			}

			public void AddTriggerTransition(TEvent trigger, TransitionBase<TStateId> transition)
			{
				triggerToTransitions = triggerToTransitions ?? new Dictionary<TEvent, List<TransitionBase<TStateId>>>();
				if (!triggerToTransitions.TryGetValue(trigger, out var value))
				{
					value = new List<TransitionBase<TStateId>>();
					triggerToTransitions.Add(trigger, value);
				}
				value.Add(transition);
			}
		}

		private struct PendingTransition
		{
			public ITransitionListener listener;

			public TStateId targetState;

			public bool isPending;

			public bool isExitTransition;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Clear()
			{
				isPending = false;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetToExit(ITransitionListener listener = null)
			{
				this.listener = listener;
				isExitTransition = true;
				isPending = true;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetToState(TStateId target, ITransitionListener listener = null)
			{
				this.listener = listener;
				targetState = target;
				isExitTransition = false;
				isPending = true;
			}
		}

		private static readonly List<TransitionBase<TStateId>> noTransitions = new List<TransitionBase<TStateId>>(0);

		private static readonly Dictionary<TEvent, List<TransitionBase<TStateId>>> noTriggerTransitions = new Dictionary<TEvent, List<TransitionBase<TStateId>>>(0);

		private (TStateId state, bool hasState) startState = (state: default(TStateId), hasState: false);

		private PendingTransition pendingTransition;

		private bool rememberLastState;

		private readonly Dictionary<TStateId, StateBundle> stateBundlesByName = new Dictionary<TStateId, StateBundle>();

		private StateBase<TStateId> activeState;

		private List<TransitionBase<TStateId>> activeTransitions = noTransitions;

		private Dictionary<TEvent, List<TransitionBase<TStateId>>> activeTriggerTransitions = noTriggerTransitions;

		private readonly List<TransitionBase<TStateId>> transitionsFromAny = new List<TransitionBase<TStateId>>();

		private readonly Dictionary<TEvent, List<TransitionBase<TStateId>>> triggerTransitionsFromAny = new Dictionary<TEvent, List<TransitionBase<TStateId>>>();

		public StateBase<TStateId> ActiveState
		{
			get
			{
				EnsureIsInitializedFor("Trying to get the active state");
				return activeState;
			}
		}

		public TStateId ActiveStateName => ActiveState.name;

		public TStateId PendingStateName => pendingTransition.targetState;

		public StateBase<TStateId> PendingState => GetState(PendingStateName);

		public bool HasPendingTransition => pendingTransition.isPending;

		public bool IsInitialized
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return activeState != null;
			}
		}

		public IStateTimingManager ParentFsm => fsm;

		public bool IsRootFsm => fsm == null;

		public StateMachine<string, string, string> this[TStateId name] => (GetState(name) as StateMachine<string, string, string>) ?? throw Common.QuickIndexerMisusedForGettingState(this, name.ToString());

		public event Action<StateBase<TStateId>> StateChanged;

		public StateMachine(bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
			: base(needsExitTime, isGhostState)
		{
			this.rememberLastState = rememberLastState;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void EnsureIsInitializedFor(string context)
		{
			if (!IsInitialized)
			{
				throw Common.NotInitialized(this, context);
			}
		}

		public void StateCanExit()
		{
			if (pendingTransition.isPending)
			{
				ITransitionListener listener = pendingTransition.listener;
				if (pendingTransition.isExitTransition)
				{
					pendingTransition = default(PendingTransition);
					listener?.BeforeTransition();
					PerformVerticalTransition();
					listener?.AfterTransition();
				}
				else
				{
					TStateId targetState = pendingTransition.targetState;
					pendingTransition = default(PendingTransition);
					ChangeState(targetState, listener);
				}
			}
		}

		private void ChangeState(TStateId name, ITransitionListener listener = null)
		{
			listener?.BeforeTransition();
			activeState?.OnExit();
			if (!stateBundlesByName.TryGetValue(name, out var value) || value.state == null)
			{
				throw Common.StateNotFound(this, name.ToString(), "Switching states");
			}
			activeTransitions = value.transitions ?? noTransitions;
			activeTriggerTransitions = value.triggerToTransitions ?? noTriggerTransitions;
			activeState = value.state;
			activeState.OnEnter();
			int i = 0;
			for (int count = activeTransitions.Count; i < count; i++)
			{
				activeTransitions[i].OnEnter();
			}
			foreach (List<TransitionBase<TStateId>> value2 in activeTriggerTransitions.Values)
			{
				int j = 0;
				for (int count2 = value2.Count; j < count2; j++)
				{
					value2[j].OnEnter();
				}
			}
			listener?.AfterTransition();
			this.StateChanged?.Invoke(activeState);
			if (activeState.isGhostState)
			{
				TryAllDirectTransitions();
			}
		}

		private void PerformVerticalTransition()
		{
			fsm?.StateCanExit();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RequestStateChange(TStateId name, bool forceInstantly = false, ITransitionListener listener = null)
		{
			if (!activeState.needsExitTime || forceInstantly)
			{
				pendingTransition = default(PendingTransition);
				ChangeState(name, listener);
			}
			else
			{
				pendingTransition.SetToState(name, listener);
				activeState.OnExitRequest();
			}
		}

		public void RequestExit(bool forceInstantly = false, ITransitionListener listener = null)
		{
			if (!activeState.needsExitTime || forceInstantly)
			{
				pendingTransition.Clear();
				listener?.BeforeTransition();
				PerformVerticalTransition();
				listener?.AfterTransition();
			}
			else
			{
				pendingTransition.SetToExit(listener);
				activeState.OnExitRequest();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryTransition(TransitionBase<TStateId> transition)
		{
			if (transition.isExitTransition)
			{
				if (fsm == null || !fsm.HasPendingTransition || !transition.ShouldTransition())
				{
					return false;
				}
				RequestExit(transition.forceInstantly, transition);
				return true;
			}
			if (!transition.ShouldTransition())
			{
				return false;
			}
			RequestStateChange(transition.to, transition.forceInstantly, transition);
			return true;
		}

		private bool TryAllGlobalTransitions()
		{
			int i = 0;
			for (int count = transitionsFromAny.Count; i < count; i++)
			{
				TransitionBase<TStateId> transitionBase = transitionsFromAny[i];
				if (!EqualityComparer<TStateId>.Default.Equals(transitionBase.to, activeState.name) && TryTransition(transitionBase))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryAllDirectTransitions()
		{
			int i = 0;
			for (int count = activeTransitions.Count; i < count; i++)
			{
				TransitionBase<TStateId> transition = activeTransitions[i];
				if (TryTransition(transition))
				{
					return true;
				}
			}
			return false;
		}

		public override void Init()
		{
			if (IsRootFsm)
			{
				OnEnter();
			}
		}

		public override void OnEnter()
		{
			if (!startState.hasState)
			{
				throw Common.MissingStartState(this, "Running OnEnter of the state machine.");
			}
			pendingTransition.Clear();
			ChangeState(startState.state);
			int i = 0;
			for (int count = transitionsFromAny.Count; i < count; i++)
			{
				transitionsFromAny[i].OnEnter();
			}
			foreach (List<TransitionBase<TStateId>> value in triggerTransitionsFromAny.Values)
			{
				int j = 0;
				for (int count2 = value.Count; j < count2; j++)
				{
					value[j].OnEnter();
				}
			}
		}

		public override void OnLogic()
		{
			EnsureIsInitializedFor("Running OnLogic");
			if (!TryAllGlobalTransitions())
			{
				TryAllDirectTransitions();
			}
			activeState?.OnLogic();
		}

		public override void OnExit()
		{
			if (activeState != null)
			{
				if (rememberLastState)
				{
					startState = (state: activeState.name, hasState: true);
				}
				activeState.OnExit();
				activeState = null;
			}
		}

		public override void OnExitRequest()
		{
			if (activeState.needsExitTime)
			{
				activeState.OnExitRequest();
			}
		}

		public void SetStartState(TStateId name)
		{
			startState = (state: name, hasState: true);
		}

		private StateBundle GetOrCreateStateBundle(TStateId name)
		{
			if (!stateBundlesByName.TryGetValue(name, out var value))
			{
				value = new StateBundle();
				stateBundlesByName.Add(name, value);
			}
			return value;
		}

		public void AddState(TStateId name, StateBase<TStateId> state)
		{
			state.fsm = this;
			state.name = name;
			state.Init();
			GetOrCreateStateBundle(name).state = state;
			if (stateBundlesByName.Count == 1 && !startState.hasState)
			{
				SetStartState(name);
			}
		}

		private void InitTransition(TransitionBase<TStateId> transition)
		{
			transition.fsm = this;
			transition.Init();
		}

		public void AddTransition(TransitionBase<TStateId> transition)
		{
			InitTransition(transition);
			GetOrCreateStateBundle(transition.from).AddTransition(transition);
		}

		public void AddTransitionFromAny(TransitionBase<TStateId> transition)
		{
			InitTransition(transition);
			transitionsFromAny.Add(transition);
		}

		public void AddTriggerTransition(TEvent trigger, TransitionBase<TStateId> transition)
		{
			InitTransition(transition);
			GetOrCreateStateBundle(transition.from).AddTriggerTransition(trigger, transition);
		}

		public void AddTriggerTransitionFromAny(TEvent trigger, TransitionBase<TStateId> transition)
		{
			InitTransition(transition);
			if (!triggerTransitionsFromAny.TryGetValue(trigger, out var value))
			{
				value = new List<TransitionBase<TStateId>>();
				triggerTransitionsFromAny.Add(trigger, value);
			}
			value.Add(transition);
		}

		public void AddTwoWayTransition(TransitionBase<TStateId> transition)
		{
			InitTransition(transition);
			AddTransition(transition);
			ReverseTransition<TStateId> transition2 = new ReverseTransition<TStateId>(transition, shouldInitWrappedTransition: false);
			InitTransition(transition2);
			AddTransition(transition2);
		}

		public void AddTwoWayTriggerTransition(TEvent trigger, TransitionBase<TStateId> transition)
		{
			InitTransition(transition);
			AddTriggerTransition(trigger, transition);
			ReverseTransition<TStateId> transition2 = new ReverseTransition<TStateId>(transition, shouldInitWrappedTransition: false);
			InitTransition(transition2);
			AddTriggerTransition(trigger, transition2);
		}

		public void AddExitTransition(TransitionBase<TStateId> transition)
		{
			transition.isExitTransition = true;
			AddTransition(transition);
		}

		public void AddExitTransitionFromAny(TransitionBase<TStateId> transition)
		{
			transition.isExitTransition = true;
			AddTransitionFromAny(transition);
		}

		public void AddExitTriggerTransition(TEvent trigger, TransitionBase<TStateId> transition)
		{
			transition.isExitTransition = true;
			AddTriggerTransition(trigger, transition);
		}

		public void AddExitTriggerTransitionFromAny(TEvent trigger, TransitionBase<TStateId> transition)
		{
			transition.isExitTransition = true;
			AddTriggerTransitionFromAny(trigger, transition);
		}

		private bool TryTrigger(TEvent trigger)
		{
			EnsureIsInitializedFor("Checking all trigger transitions of the active state");
			if (triggerTransitionsFromAny.TryGetValue(trigger, out var value))
			{
				int i = 0;
				for (int count = value.Count; i < count; i++)
				{
					TransitionBase<TStateId> transitionBase = value[i];
					if (!EqualityComparer<TStateId>.Default.Equals(transitionBase.to, activeState.name) && TryTransition(transitionBase))
					{
						return true;
					}
				}
			}
			if (activeTriggerTransitions.TryGetValue(trigger, out value))
			{
				int j = 0;
				for (int count2 = value.Count; j < count2; j++)
				{
					TransitionBase<TStateId> transition = value[j];
					if (TryTransition(transition))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void Trigger(TEvent trigger)
		{
			if (!TryTrigger(trigger))
			{
				(activeState as ITriggerable<TEvent>)?.Trigger(trigger);
			}
		}

		public void TriggerLocally(TEvent trigger)
		{
			TryTrigger(trigger);
		}

		public virtual void OnAction(TEvent trigger)
		{
			EnsureIsInitializedFor("Running OnAction of the active state");
			(activeState as IActionable<TEvent>)?.OnAction(trigger);
		}

		public virtual void OnAction<TData>(TEvent trigger, TData data)
		{
			EnsureIsInitializedFor("Running OnAction of the active state");
			(activeState as IActionable<TEvent>)?.OnAction(trigger, data);
		}

		public virtual bool HasAction(TEvent trigger)
		{
			EnsureIsInitializedFor("Running HasAction of the active state");
			return (activeState as IActionable<TEvent>)?.HasAction(trigger) ?? false;
		}

		public StateBase<TStateId> GetState(TStateId name)
		{
			if (!stateBundlesByName.TryGetValue(name, out var value) || value.state == null)
			{
				throw Common.StateNotFound(this, name.ToString(), "Getting a state");
			}
			return value.state;
		}

		public override string GetActiveHierarchyPath()
		{
			if (activeState == null)
			{
				return "";
			}
			return $"{name}/{activeState.GetActiveHierarchyPath()}";
		}

		public IReadOnlyList<TStateId> GetAllStateNames()
		{
			return (from bundle in stateBundlesByName.Values
				where bundle.state != null
				select bundle.state.name).ToArray();
		}

		public IReadOnlyList<StateBase<TStateId>> GetAllStates()
		{
			return (from bundle in stateBundlesByName.Values
				where bundle.state != null
				select bundle.state).ToArray();
		}

		public TStateId GetStartStateName()
		{
			if (!startState.hasState)
			{
				throw Common.MissingStartState(this, "Getting the start state", null, "Make sure that there is at least one state in the state machine before running GetStartStateName() by calling fsm.AddState(...).");
			}
			return startState.state;
		}

		public IReadOnlyList<TransitionBase<TStateId>> GetAllTransitions()
		{
			return stateBundlesByName.Values.Where((StateBundle bundle) => bundle.transitions != null).SelectMany((StateBundle bundle) => bundle.transitions).ToArray();
		}

		public IReadOnlyList<TransitionBase<TStateId>> GetAllTransitionsFromAny()
		{
			return transitionsFromAny.ToArray();
		}

		public IReadOnlyDictionary<TEvent, IReadOnlyList<TransitionBase<TStateId>>> GetAllTriggerTransitions()
		{
			Dictionary<TEvent, List<TransitionBase<TStateId>>> dictionary = new Dictionary<TEvent, List<TransitionBase<TStateId>>>();
			TEvent key;
			List<TransitionBase<TStateId>> value;
			foreach (StateBundle value4 in stateBundlesByName.Values)
			{
				if (value4.triggerToTransitions == null)
				{
					continue;
				}
				foreach (KeyValuePair<TEvent, List<TransitionBase<TStateId>>> triggerToTransition in value4.triggerToTransitions)
				{
					triggerToTransition.Deconstruct(out key, out value);
					TEvent key2 = key;
					List<TransitionBase<TStateId>> collection = value;
					if (!dictionary.TryGetValue(key2, out var value2))
					{
						value2 = new List<TransitionBase<TStateId>>();
						dictionary.Add(key2, value2);
					}
					value2.AddRange(collection);
				}
			}
			Dictionary<TEvent, IReadOnlyList<TransitionBase<TStateId>>> dictionary2 = new Dictionary<TEvent, IReadOnlyList<TransitionBase<TStateId>>>();
			foreach (KeyValuePair<TEvent, List<TransitionBase<TStateId>>> item in dictionary)
			{
				item.Deconstruct(out key, out value);
				TEvent key3 = key;
				List<TransitionBase<TStateId>> value3 = value;
				dictionary2.Add(key3, value3);
			}
			return dictionary2;
		}

		public IReadOnlyDictionary<TEvent, IReadOnlyList<TransitionBase<TStateId>>> GetAllTriggerTransitionsFromAny()
		{
			Dictionary<TEvent, IReadOnlyList<TransitionBase<TStateId>>> dictionary = new Dictionary<TEvent, IReadOnlyList<TransitionBase<TStateId>>>();
			foreach (var (key, value) in triggerTransitionsFromAny)
			{
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		public override void AcceptVisitor(IStateVisitor visitor)
		{
			visitor.VisitStateMachine(this);
		}
	}
	public class StateMachine<TStateId, TEvent> : StateMachine<TStateId, TStateId, TEvent>
	{
		public StateMachine(bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
			: base(needsExitTime, isGhostState, rememberLastState)
		{
		}
	}
	public class StateMachine<TStateId> : StateMachine<TStateId, TStateId, string>
	{
		public StateMachine(bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
			: base(needsExitTime, isGhostState, rememberLastState)
		{
		}
	}
	public class StateMachine : StateMachine<string, string, string>
	{
		public StateMachine(bool needsExitTime = false, bool isGhostState = false, bool rememberLastState = false)
			: base(needsExitTime, isGhostState, rememberLastState)
		{
		}
	}
}
