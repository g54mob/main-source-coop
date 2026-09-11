using Zorro.Core;

public abstract class MatchmakingState : StateMachineState
{
	protected MatchmakingHandler MatchmakingHandler => Singleton<MatchmakingHandler>.Instance;
}
