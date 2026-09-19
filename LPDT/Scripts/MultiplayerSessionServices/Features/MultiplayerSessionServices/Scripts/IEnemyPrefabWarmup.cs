using Cysharp.Threading.Tasks;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface IEnemyPrefabWarmup
	{
		UniTask WarmupForCurrentLevelAsync();
	}
}
