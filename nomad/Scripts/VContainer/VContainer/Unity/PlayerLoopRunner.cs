using System;
using VContainer.Internal;

namespace VContainer.Unity
{
	internal sealed class PlayerLoopRunner
	{
		private readonly FreeList<IPlayerLoopItem> runners = new FreeList<IPlayerLoopItem>(16);

		private int running;

		public void Dispatch(IPlayerLoopItem item)
		{
			runners.Add(item);
		}

		public void Run()
		{
			ReadOnlySpan<IPlayerLoopItem> readOnlySpan = runners.AsSpan();
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				IPlayerLoopItem playerLoopItem = readOnlySpan[i];
				if (playerLoopItem != null && !playerLoopItem.MoveNext())
				{
					runners.RemoveAt(i);
				}
			}
		}
	}
}
