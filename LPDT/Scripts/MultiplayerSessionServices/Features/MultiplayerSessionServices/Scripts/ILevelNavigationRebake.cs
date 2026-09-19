using Cysharp.Threading.Tasks;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ILevelNavigationRebake
	{
		UniTask RebakeForCurrentLevelAsync();
	}
}
