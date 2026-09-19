using Cysharp.Threading.Tasks;

namespace Features.DisconnectHandlerModule.Scripts.Systems
{
	public interface ISessionTeardownService
	{
		UniTask TearDownAsync();
	}
}
