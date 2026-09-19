namespace UnityHFSM.Inspection
{
	public interface IStateMachineHierarchyVisitor
	{
		void VisitStateMachine<TOwnId, TStateId, TEvent>(StateMachinePath fsmPath, StateMachine<TOwnId, TStateId, TEvent> fsm);

		void VisitRegularState<TStateId>(StateMachinePath statePath, StateBase<TStateId> state);

		void ExitStateMachine<TOwnId, TStateId, TEvent>(StateMachinePath fsmPath, StateMachine<TOwnId, TStateId, TEvent> fsm);
	}
}
