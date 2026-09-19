using Cysharp.Threading.Tasks;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ILevelContentSpawnRegistry
	{
		void Register(ILevelContentSpawnProvider provider);

		void Unregister(ILevelContentSpawnProvider provider);

		void BeginNewLevel();

		UniTask SpawnAllAsync();
	}
}
