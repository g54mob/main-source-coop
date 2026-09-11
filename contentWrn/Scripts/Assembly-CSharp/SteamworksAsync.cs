using System;
using System.Threading.Tasks;
using Steamworks;

public static class SteamworksAsync
{
	public static Task<T> ToAsync<T>(this SteamAPICall_t result)
	{
		if (result == SteamAPICall_t.Invalid)
		{
			return Task.FromException<T>(new Exception("Invalid result returned from Steamworks"));
		}
		TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
		CallResult<T> callResult = null;
		callResult = CallResult<T>.Create(OnComplete);
		callResult.Set(result);
		return tcs.Task;
		void OnComplete(T param, bool biofailure)
		{
			callResult?.Dispose();
			tcs.SetResult(param);
		}
	}
}
