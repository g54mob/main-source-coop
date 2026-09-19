using System;

namespace Fusion
{
	public readonly ref struct RpcInvokeContext
	{
		public readonly NetworkRunner Runner;

		public readonly NetworkBehaviour TargetBehaviour;

		public readonly int Tick;

		public readonly PlayerRef Source;

		public readonly PlayerRef TargetPlayer;

		public readonly ReadOnlySpan<byte> Payload;

		public readonly Type TargetType;

		internal unsafe readonly bool* ValidationResult;

		public unsafe bool IsValidating => ValidationResult != null;

		public RpcDataReader PayloadReader => new RpcDataReader(Payload);

		public unsafe RpcInvokeContext(NetworkRunner runner, NetworkBehaviour target, int tick, PlayerRef source, PlayerRef targetPlayer, ReadOnlySpan<byte> payload, bool* validationResult, Type targetType = null)
		{
			Runner = runner;
			TargetBehaviour = target;
			Tick = tick;
			Source = source;
			TargetPlayer = targetPlayer;
			Payload = payload;
			TargetType = targetType;
			ValidationResult = validationResult;
		}
	}
}
