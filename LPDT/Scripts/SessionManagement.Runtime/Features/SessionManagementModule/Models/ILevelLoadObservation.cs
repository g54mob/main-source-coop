using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface ILevelLoadObservation
	{
		UniTask WaitUntilLoadedAsync(string levelName);

		bool TryVerifyLoaded(string levelName, out string problem);
	}
}
