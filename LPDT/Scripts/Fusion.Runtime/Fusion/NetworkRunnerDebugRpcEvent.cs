using System.Reflection;

namespace Fusion
{
	public readonly ref struct NetworkRunnerDebugRpcEvent
	{
		public readonly NetworkRunnerDebugRpcEventType EventType;

		public readonly Tick Tick;

		public readonly NetworkRunner Runner;

		public readonly NetworkBehaviour Behaviour;

		public readonly MethodBase MethodBase;

		public readonly int SentBytes;

		public readonly PlayerRef SourcePlayer;

		public readonly PlayerRef? TargetPlayer;

		public readonly RpcInvokeInfo? LocalInvokeInfo;

		internal NetworkRunnerDebugRpcEvent(NetworkRunner runner, NetworkBehaviour behaviour, Tick tick, MethodBase methodBase, int sentBytes, PlayerRef sourcePlayer, PlayerRef? targetPlayer, RpcInvokeInfo? localInvokeInfo = null)
		{
			EventType = ((!localInvokeInfo.HasValue) ? NetworkRunnerDebugRpcEventType.RemoteCall : NetworkRunnerDebugRpcEventType.LocalCall);
			Runner = runner;
			Behaviour = behaviour;
			Tick = tick;
			LocalInvokeInfo = localInvokeInfo;
			MethodBase = methodBase;
			SentBytes = sentBytes;
			SourcePlayer = sourcePlayer;
			TargetPlayer = targetPlayer;
		}

		public override string ToString()
		{
			return $"[NetworkRunnerDebugRpcEvent {EventType}" + string.Format(": {0}={1}", "Tick", Tick) + string.Format(", {0}={1}", "Runner", Runner) + (((object)Behaviour == null) ? "" : string.Format(", {0}={1}", "Behaviour", Behaviour)) + string.Format(", {0}={1}", "MethodBase", MethodBase) + string.Format(", {0}={1}", "SentBytes", SentBytes) + string.Format(", {0}={1}", "SourcePlayer", SourcePlayer) + ((!TargetPlayer.HasValue) ? "" : string.Format(", {0}={1}", "TargetPlayer", TargetPlayer)) + ((!LocalInvokeInfo.HasValue) ? "" : string.Format(", {0}={1}", "LocalInvokeInfo", LocalInvokeInfo)) + "]";
		}

		internal static NetworkRunnerDebugRpcEvent FromLocalCall(object runnerOrBehaviour, MethodBase method, RpcInvokeInfo invokeInfo, RpcHostMode hostMode, PlayerRef target)
		{
			(NetworkRunner, NetworkBehaviour) tuple = ResolveRunner(runnerOrBehaviour);
			NetworkRunner item = tuple.Item1;
			NetworkBehaviour item2 = tuple.Item2;
			PlayerRef sourcePlayer = RpcInfo.ResolveLocalSourcePlayer(item, item.LocalPlayer, hostMode);
			return new NetworkRunnerDebugRpcEvent(item, item2, item.Tick, method, invokeInfo.PayloadSize, sourcePlayer, target.IsRealPlayer ? new PlayerRef?(target) : ((PlayerRef?)null), invokeInfo);
		}

		internal static NetworkRunnerDebugRpcEvent FromRemoteCall(in RpcInvokeContext context, MethodBase method, RpcHostMode hostMode)
		{
			NetworkRunner runner = context.Runner;
			PlayerRef sourcePlayer = RpcInfo.ResolveRemoteSourcePlayer(runner, context.Source, hostMode);
			return new NetworkRunnerDebugRpcEvent(runner, context.TargetBehaviour, context.Tick, method, context.Payload.Length, sourcePlayer, context.TargetPlayer.IsRealPlayer ? new PlayerRef?(context.TargetPlayer) : ((PlayerRef?)null));
		}

		private static (NetworkRunner, NetworkBehaviour) ResolveRunner(object runnerOrBehaviour)
		{
			if (runnerOrBehaviour is NetworkBehaviour networkBehaviour)
			{
				return (networkBehaviour.Runner, networkBehaviour);
			}
			return ((NetworkRunner)runnerOrBehaviour, null);
		}
	}
}
