using Cysharp.Threading.Tasks;

namespace EvilCore
{
	public interface IReloadLastSaveHandler
	{
		UniTask ReloadLastSaveAsync();
	}
}
