namespace UnityHFSM
{
	public interface IActionable<TEvent>
	{
		void OnAction(TEvent trigger);

		void OnAction<TData>(TEvent trigger, TData data);

		bool HasAction(TEvent trigger);
	}
	public interface IActionable : IActionable<string>
	{
	}
}
