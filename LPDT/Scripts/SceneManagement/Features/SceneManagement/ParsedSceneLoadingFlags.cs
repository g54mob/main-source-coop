namespace Features.SceneManagement
{
	internal struct ParsedSceneLoadingFlags
	{
		internal bool SyncLoading { get; set; }

		internal bool UnloadRedundant { get; set; }

		internal bool CleanUpObjectPool { get; set; }
	}
}
