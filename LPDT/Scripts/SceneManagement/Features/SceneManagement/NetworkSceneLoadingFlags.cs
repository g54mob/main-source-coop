using System;

namespace Features.SceneManagement
{
	[Flags]
	public enum NetworkSceneLoadingFlags
	{
		UnloadRedundant = 1,
		SyncLoading = 4,
		CleanUpObjectPool = 8
	}
}
