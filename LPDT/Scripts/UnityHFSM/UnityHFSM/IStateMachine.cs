namespace UnityHFSM
{
	public interface IStateMachine<TStateId> : IStateTimingManager
	{
		StateBase<TStateId> PendingState { get; }

		TStateId PendingStateName { get; }

		StateBase<TStateId> ActiveState { get; }

		TStateId ActiveStateName { get; }

		StateBase<TStateId> GetState(TStateId name);
	}
}
