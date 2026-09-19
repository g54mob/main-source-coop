using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface INetworkedSceneLoader
	{
		UniTask LoadSceneAsync(string levelName, int transitionEpoch);

		UniTask UnloadAsync();

		void RequestAdvanceOnNextLoad();

		bool HasNextLevel();
	}
}
