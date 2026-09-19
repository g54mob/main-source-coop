using System;

namespace Fusion
{
	public readonly struct FusionPlayerLoopSystemHandle : IDisposable
	{
		public readonly Type Type;

		public FusionPlayerLoopSystemHandle(Type type)
		{
			this = default(FusionPlayerLoopSystemHandle);
			Type = type;
		}

		public void Dispose()
		{
			if (!(Type == null))
			{
				FusionPlayerLoopSystemUtils.RemoveFromPlayerLoop(Type);
			}
		}
	}
}
