using System;
using System.Collections.Generic;

namespace UnityHFSM
{
	public class ParallelStates<TOwnId, TStateId, TEvent> : StateBase<TOwnId>, IActionable<TEvent>, ITriggerable<TEvent>, IStateTimingManager
	{
		private readonly List<StateBase<TStateId>> states = new List<StateBase<TStateId>>();

		private bool areStatesNameless;

		private bool isActive;

		private Func<ParallelStates<TOwnId, TStateId, TEvent>, bool> canExit;

		public bool HasPendingTransition => fsm.HasPendingTransition;

		public IStateTimingManager ParentFsm => fsm;

		public ParallelStates(Func<ParallelStates<TOwnId, TStateId, TEvent>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false)
			: base(needsExitTime, isGhostState)
		{
			this.canExit = canExit;
		}

		public ParallelStates(params StateBase<TStateId>[] states)
			: this((Func<ParallelStates<TOwnId, TStateId, TEvent>, bool>)null, false, false, states)
		{
		}

		public ParallelStates(bool needsExitTime, params StateBase<TStateId>[] states)
			: this((Func<ParallelStates<TOwnId, TStateId, TEvent>, bool>)null, needsExitTime, false, states)
		{
		}

		public ParallelStates(Func<ParallelStates<TOwnId, TStateId, TEvent>, bool> canExit, bool needsExitTime, params StateBase<TStateId>[] states)
			: this(canExit, needsExitTime, false, states)
		{
		}

		public ParallelStates(Func<ParallelStates<TOwnId, TStateId, TEvent>, bool> canExit, bool needsExitTime, bool isGhostState, params StateBase<TStateId>[] states)
			: base(needsExitTime, isGhostState)
		{
			this.canExit = canExit;
			areStatesNameless = true;
			foreach (StateBase<TStateId> state in states)
			{
				AddState(default(TStateId), state);
			}
		}

		public ParallelStates<TOwnId, TStateId, TEvent> AddState(TStateId id, StateBase<TStateId> state)
		{
			state.fsm = this;
			state.name = id;
			state.Init();
			states.Add(state);
			return this;
		}

		public override void Init()
		{
			foreach (StateBase<TStateId> state in states)
			{
				state.fsm = this;
			}
		}

		public override void OnEnter()
		{
			isActive = true;
			foreach (StateBase<TStateId> state in states)
			{
				state.OnEnter();
			}
		}

		public override void OnLogic()
		{
			foreach (StateBase<TStateId> state in states)
			{
				state.OnLogic();
				if (!isActive)
				{
					return;
				}
			}
			if (needsExitTime && canExit != null && fsm.HasPendingTransition && canExit(this))
			{
				fsm.StateCanExit();
			}
		}

		public override void OnExit()
		{
			isActive = false;
			foreach (StateBase<TStateId> state in states)
			{
				state.OnExit();
			}
		}

		public override void OnExitRequest()
		{
			if (canExit == null)
			{
				foreach (StateBase<TStateId> state in states)
				{
					state.OnExitRequest();
					if (!isActive)
					{
						break;
					}
				}
				return;
			}
			if (fsm.HasPendingTransition && canExit(this))
			{
				fsm.StateCanExit();
			}
		}

		public void OnAction(TEvent trigger)
		{
			foreach (StateBase<TStateId> state in states)
			{
				(state as IActionable<TEvent>)?.OnAction(trigger);
			}
		}

		public void OnAction<TData>(TEvent trigger, TData data)
		{
			foreach (StateBase<TStateId> state in states)
			{
				(state as IActionable<TEvent>)?.OnAction(trigger, data);
			}
		}

		public bool HasAction(TEvent trigger)
		{
			foreach (StateBase<TStateId> state in states)
			{
				IActionable<TEvent> obj = state as IActionable<TEvent>;
				if (obj != null && obj.HasAction(trigger))
				{
					return true;
				}
			}
			return false;
		}

		public void StateCanExit()
		{
			if (isActive && canExit == null)
			{
				fsm.StateCanExit();
			}
		}

		public void Trigger(TEvent trigger)
		{
			foreach (StateBase<TStateId> state in states)
			{
				(state as ITriggerable<TEvent>)?.Trigger(trigger);
				if (!isActive)
				{
					break;
				}
			}
		}

		public override string GetActiveHierarchyPath()
		{
			object obj = name?.ToString();
			if (obj == null)
			{
				obj = "";
			}
			string text = (string)obj;
			if (areStatesNameless || states.Count == 0)
			{
				return text;
			}
			if (states.Count == 1)
			{
				return text + "/" + states[0].GetActiveHierarchyPath();
			}
			string text2 = text + "/(";
			for (int i = 0; i < states.Count; i++)
			{
				text2 += states[i].GetActiveHierarchyPath();
				if (i < states.Count - 1)
				{
					text2 += " & ";
				}
			}
			return text2 + ")";
		}
	}
	public class ParallelStates<TStateId, TEvent> : ParallelStates<TStateId, TStateId, TEvent>
	{
		public ParallelStates(Func<ParallelStates<TStateId, TStateId, TEvent>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false)
			: base(canExit, needsExitTime, isGhostState)
		{
		}

		public ParallelStates(params StateBase<TStateId>[] states)
			: base((Func<ParallelStates<TStateId, TStateId, TEvent>, bool>)null, false, false, states)
		{
		}

		public ParallelStates(bool needsExitTime, params StateBase<TStateId>[] states)
			: base((Func<ParallelStates<TStateId, TStateId, TEvent>, bool>)null, needsExitTime, false, states)
		{
		}

		public ParallelStates(Func<ParallelStates<TStateId, TStateId, TEvent>, bool> canExit, bool needsExitTime, params StateBase<TStateId>[] states)
			: base(canExit, needsExitTime, false, states)
		{
		}

		public ParallelStates(Func<ParallelStates<TStateId, TStateId, TEvent>, bool> canExit, bool needsExitTime, bool isGhostState, params StateBase<TStateId>[] states)
			: base(canExit, needsExitTime, isGhostState, states)
		{
		}
	}
	public class ParallelStates<TStateId> : ParallelStates<TStateId, TStateId, string>
	{
		public ParallelStates(Func<ParallelStates<TStateId, TStateId, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false)
			: base(canExit, needsExitTime, isGhostState)
		{
		}

		public ParallelStates(params StateBase<TStateId>[] states)
			: base((Func<ParallelStates<TStateId, TStateId, string>, bool>)null, false, false, states)
		{
		}

		public ParallelStates(bool needsExitTime, params StateBase<TStateId>[] states)
			: base((Func<ParallelStates<TStateId, TStateId, string>, bool>)null, needsExitTime, false, states)
		{
		}

		public ParallelStates(Func<ParallelStates<TStateId, TStateId, string>, bool> canExit, bool needsExitTime, params StateBase<TStateId>[] states)
			: base(canExit, needsExitTime, false, states)
		{
		}

		public ParallelStates(Func<ParallelStates<TStateId, TStateId, string>, bool> canExit, bool needsExitTime, bool isGhostState, params StateBase<TStateId>[] states)
			: base(canExit, needsExitTime, isGhostState, states)
		{
		}
	}
	public class ParallelStates : ParallelStates<string, string, string>
	{
		public ParallelStates(Func<ParallelStates<string, string, string>, bool> canExit = null, bool needsExitTime = false, bool isGhostState = false)
			: base(canExit, needsExitTime, isGhostState)
		{
		}

		public ParallelStates(params StateBase<string>[] states)
			: this(null, needsExitTime: false, isGhostState: false, states)
		{
		}

		public ParallelStates(bool needsExitTime, params StateBase<string>[] states)
			: this(null, needsExitTime, isGhostState: false, states)
		{
		}

		public ParallelStates(Func<ParallelStates<string, string, string>, bool> canExit, bool needsExitTime, params StateBase<string>[] states)
			: this(canExit, needsExitTime, isGhostState: false, states)
		{
		}

		public ParallelStates(Func<ParallelStates<string, string, string>, bool> canExit, bool needsExitTime, bool isGhostState, params StateBase<string>[] states)
			: base(canExit, needsExitTime, isGhostState)
		{
			for (int i = 0; i < states.Length; i++)
			{
				AddState(i.ToString(), states[i]);
			}
		}
	}
}
