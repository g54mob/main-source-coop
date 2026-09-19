using System.Threading.Tasks;

namespace Google.Apis.Util.Store
{
	public interface IDataStore
	{
		Task StoreAsync<T>(string key, T value);

		Task DeleteAsync<T>(string key);

		Task<T> GetAsync<T>(string key);

		Task ClearAsync();
	}
}
