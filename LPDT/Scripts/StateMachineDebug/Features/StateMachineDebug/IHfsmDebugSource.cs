using UnityHFSM;

namespace Features.StateMachineDebug
{
	public interface IHfsmDebugSource
	{
	}
	public interface IHfsmDebugSource<TOwnId, TStateId, TEvent> : IHfsmDebugSource
	{
		StateMachine<TOwnId, TStateId, TEvent> StateMachineForDebug { get; }
	}
}
