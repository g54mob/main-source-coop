namespace UnityHFSM.Inspection
{
	public interface IStateVisitor
	{
		void VisitStateMachine<TOwnId, TStateId, TEvent>(StateMachine<TOwnId, TStateId, TEvent> fsm);

		void VisitRegularState<TStateId>(StateBase<TStateId> state);
	}
}
