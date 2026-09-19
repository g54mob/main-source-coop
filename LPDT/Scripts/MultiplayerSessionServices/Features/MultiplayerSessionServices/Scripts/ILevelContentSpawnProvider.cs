using Cysharp.Threading.Tasks;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ILevelContentSpawnProvider
	{
		UniTask SpawnLevelContentAsync();
	}
}
