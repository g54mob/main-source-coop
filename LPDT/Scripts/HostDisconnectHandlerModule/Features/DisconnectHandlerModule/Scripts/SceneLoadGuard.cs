using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.DisconnectHandlerModule.Scripts
{
	public static class SceneLoadGuard
	{
		public static bool CanPerformSceneOperations
		{
			get
			{
				if (!Application.isPlaying)
				{
					return false;
				}
				return true;
			}
		}

		public static async UniTask AwaitAsyncOperation(AsyncOperation operation)
		{
			if (CanPerformSceneOperations && operation != null)
			{
				await operation;
			}
		}
	}
}
