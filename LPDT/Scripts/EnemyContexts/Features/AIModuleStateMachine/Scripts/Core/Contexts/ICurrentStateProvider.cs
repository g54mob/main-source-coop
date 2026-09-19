namespace Features.AIModuleStateMachine.Scripts.Core.Contexts
{
	public interface ICurrentStateProvider<out TStateId>
	{
		TStateId CurrentStateId { get; }
	}
}
