namespace Fusion
{
	public ref struct NetworkRunnerDebugEvent
	{
		public readonly NetworkRunner Runner;

		public readonly NetworkRunnerDebugEventType Type;

		public readonly NetworkObjectChange Change;

		public NetworkRunnerDebugEvent(NetworkRunner runner, NetworkRunnerDebugEventType eventType, in NetworkObjectChange change)
		{
			Runner = runner;
			Type = eventType;
			Change = change;
		}

		public override readonly string ToString()
		{
			return $"[NetworkRunnerDebugEvent {Type}, {Change.ToString()}]";
		}
	}
}
