using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;

namespace Features.PlayerSpawner.Scripts
{
	public static class PlayerAvatarSpawnReadyRegistry
	{
		private static readonly Dictionary<NetworkId, UniTaskCompletionSource> Waiters = new Dictionary<NetworkId, UniTaskCompletionSource>();

		private static readonly HashSet<NetworkId> ReadyIds = new HashSet<NetworkId>();

		public static UniTask WaitUntilReadyAsync(NetworkObject avatar)
		{
			if (avatar == null || !avatar.IsValid)
			{
				return UniTask.CompletedTask;
			}
			NetworkId id = avatar.Id;
			if (ReadyIds.Contains(id))
			{
				return UniTask.CompletedTask;
			}
			if (Waiters.TryGetValue(id, out var value))
			{
				return value.Task;
			}
			UniTaskCompletionSource uniTaskCompletionSource = new UniTaskCompletionSource();
			Waiters[id] = uniTaskCompletionSource;
			return uniTaskCompletionSource.Task;
		}

		public static void MarkReady(NetworkId id)
		{
			ReadyIds.Add(id);
			if (Waiters.TryGetValue(id, out var value))
			{
				Waiters.Remove(id);
				value.TrySetResult();
			}
		}

		public static void Clear(NetworkId id)
		{
			ReadyIds.Remove(id);
			Waiters.Remove(id);
		}
	}
}
