using System.Threading.Tasks;

namespace Google.Apis.Util.Store
{
	public class NullDataStore : IDataStore
	{
		private static readonly Task s_completedTask = CompletedTask<int>();

		private static Task<T> CompletedTask<T>()
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetResult(default(T));
			return taskCompletionSource.Task;
		}

		public Task ClearAsync()
		{
			return s_completedTask;
		}

		public Task DeleteAsync<T>(string key)
		{
			return s_completedTask;
		}

		public Task<T> GetAsync<T>(string key)
		{
			return CompletedTask<T>();
		}

		public Task StoreAsync<T>(string key, T value)
		{
			return s_completedTask;
		}
	}
}
