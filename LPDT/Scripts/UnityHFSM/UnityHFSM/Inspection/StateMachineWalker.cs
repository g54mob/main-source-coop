namespace UnityHFSM.Inspection
{
	public class StateMachineWalker
	{
		private class HierarchyWalker : IStateVisitor
		{
			private StateMachinePath path;

			private readonly IStateMachineHierarchyVisitor hierarchyVisitor;

			public HierarchyWalker(IStateMachineHierarchyVisitor hierarchyVisitor)
			{
				this.hierarchyVisitor = hierarchyVisitor;
			}

			public void VisitStateMachine<TOwnId, TStateId, TEvent>(StateMachine<TOwnId, TStateId, TEvent> fsm)
			{
				path = (((object)path == null) ? StateMachinePath.Root : new StateMachinePath<TOwnId>(path, fsm.name));
				hierarchyVisitor.VisitStateMachine(path, fsm);
				foreach (StateBase<TStateId> allState in fsm.GetAllStates())
				{
					allState.AcceptVisitor(this);
				}
				hierarchyVisitor.ExitStateMachine(path, fsm);
				path = path.parentPath;
			}

			public void VisitRegularState<TStateId>(StateBase<TStateId> state)
			{
				StateMachinePath<TStateId> statePath = new StateMachinePath<TStateId>(path, state.name);
				hierarchyVisitor.VisitRegularState(statePath, state);
			}
		}

		private class ActiveStateVisitor : IStateVisitor
		{
			public StateMachinePath activePath;

			public void VisitStateMachine<TOwnId, TStateId, TEvent>(StateMachine<TOwnId, TStateId, TEvent> fsm)
			{
				activePath = (((object)activePath == null) ? StateMachinePath.Root : new StateMachinePath<TOwnId>(activePath, fsm.name));
				fsm.ActiveState.AcceptVisitor(this);
			}

			public void VisitRegularState<TStateId>(StateBase<TStateId> state)
			{
				activePath = new StateMachinePath<TStateId>(activePath, state.name);
			}
		}

		private class StatePathExtractor<TStartStateId> : IStateVisitor
		{
			public StateMachinePath path;

			public StatePathExtractor(StateBase<TStartStateId> state)
			{
				VisitParent(state.fsm);
				state.AcceptVisitor(this);
			}

			private void VisitParent(IStateTimingManager parent)
			{
				if (parent != null)
				{
					VisitParent(parent.ParentFsm);
					(parent as IVisitableState)?.AcceptVisitor(this);
				}
			}

			public void VisitStateMachine<TOwnId, TStateId, TEvent>(StateMachine<TOwnId, TStateId, TEvent> fsm)
			{
				if (fsm.IsRootFsm)
				{
					path = StateMachinePath.Root;
				}
				else
				{
					AddToPath(fsm.name);
				}
			}

			public void VisitRegularState<TStateId>(StateBase<TStateId> state)
			{
				AddToPath(state.name);
			}

			private void AddToPath<TStateId>(TStateId name)
			{
				path = ((path == null) ? new StateMachinePath<TStateId>(name) : new StateMachinePath<TStateId>(path, name));
			}
		}

		private class StringStatePathExtractor<TStartStateId> : IStateVisitor
		{
			public string path;

			public StringStatePathExtractor(StateBase<TStartStateId> state)
			{
				VisitParent(state.fsm);
				state.AcceptVisitor(this);
			}

			private void VisitParent(IStateTimingManager parent)
			{
				if (parent != null)
				{
					VisitParent(parent.ParentFsm);
					(parent as IVisitableState)?.AcceptVisitor(this);
				}
			}

			public void VisitStateMachine<TOwnId, TStateId, TEvent>(StateMachine<TOwnId, TStateId, TEvent> fsm)
			{
				if (fsm.IsRootFsm)
				{
					path = "Root";
				}
				else
				{
					AddToPath(fsm.name);
				}
			}

			public void VisitRegularState<TStateId>(StateBase<TStateId> state)
			{
				AddToPath(state.name);
			}

			private void AddToPath<TStateId>(TStateId name)
			{
				string text2;
				if (path != null)
				{
					string text = path;
					TStateId val = name;
					text2 = text + "/" + val;
				}
				else
				{
					text2 = name.ToString();
				}
				path = text2;
			}
		}

		public static void Walk<TOwnId, TStateId, TEvent>(StateMachine<TOwnId, TStateId, TEvent> fsm, IStateMachineHierarchyVisitor visitor)
		{
			new HierarchyWalker(visitor).VisitStateMachine(fsm);
		}

		public static StateMachinePath GetActiveStatePath<TOwnId, TStateId, TEvent>(StateMachine<TOwnId, TStateId, TEvent> fsm)
		{
			ActiveStateVisitor activeStateVisitor = new ActiveStateVisitor();
			activeStateVisitor.VisitStateMachine(fsm);
			return activeStateVisitor.activePath;
		}

		public static StateMachinePath GetPathOfState<TStateId>(StateBase<TStateId> state)
		{
			return new StatePathExtractor<TStateId>(state).path;
		}

		public static string GetStringPathOfState<TStateId>(StateBase<TStateId> state)
		{
			return new StringStatePathExtractor<TStateId>(state).path;
		}
	}
}
