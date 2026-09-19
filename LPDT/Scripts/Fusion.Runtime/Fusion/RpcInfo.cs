using System;

namespace Fusion
{
	public readonly struct RpcInfo
	{
		public readonly Tick Tick;

		public readonly PlayerRef Source;

		public readonly RpcChannel Channel;

		private readonly IntPtr _validationResult;

		public readonly bool IsInvokeLocal;

		public bool IsValidating => _validationResult != (IntPtr)0;

		private unsafe RpcInfo(Tick tick, PlayerRef source, bool isInvokeLocal, RpcChannel channel, bool* validationResult)
		{
			this = default(RpcInfo);
			Tick = tick;
			Source = source;
			IsInvokeLocal = isInvokeLocal;
			Channel = channel;
			_validationResult = new IntPtr(validationResult);
		}

		public static RpcInfo FromLocal(NetworkRunner runner, RpcChannel channel, RpcHostMode hostMode)
		{
			PlayerRef source = ResolveLocalSourcePlayer(runner, runner.Simulation.LocalPlayer, hostMode);
			return new RpcInfo(runner.Simulation.Tick, source, isInvokeLocal: true, channel, null);
		}

		public unsafe static RpcInfo FromRemote(in RpcInvokeContext context, RpcHostMode hostMode, RpcChannel channel)
		{
			PlayerRef source = ResolveRemoteSourcePlayer(context.Runner, context.Source, hostMode);
			return new RpcInfo(context.Tick, source, isInvokeLocal: false, channel, context.ValidationResult);
		}

		internal static PlayerRef ResolveLocalSourcePlayer(NetworkRunner runner, PlayerRef source, RpcHostMode hostMode)
		{
			if (hostMode == RpcHostMode.SourceIsServer && runner.Simulation.IsHostPlayer(source) && !runner.IsSinglePlayer)
			{
				return default(PlayerRef);
			}
			return source;
		}

		internal static PlayerRef ResolveRemoteSourcePlayer(NetworkRunner runner, PlayerRef source, RpcHostMode hostMode)
		{
			if (source.IsNone && hostMode == RpcHostMode.SourceIsHostPlayer && runner.Simulation.TryGetHostPlayer(out var player))
			{
				return player;
			}
			return source;
		}

		public override string ToString()
		{
			return string.Format("[RpcInfo: {0}={1}, {2}={3}, {4}={5}, {6}={7}]", "Tick", Tick, "Source", Source, "IsInvokeLocal", IsInvokeLocal, "Channel", Channel);
		}

		public unsafe void Cancel()
		{
			if (_validationResult != (IntPtr)0)
			{
				*(sbyte*)(void*)_validationResult = 0;
			}
		}
	}
}
