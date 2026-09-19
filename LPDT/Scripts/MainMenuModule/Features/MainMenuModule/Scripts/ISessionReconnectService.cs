using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;

namespace Features.MainMenuModule.Scripts
{
	public interface ISessionReconnectService
	{
		UniTask<bool> IsSessionValidAsync(string sessionName);

		UniTask<bool> IsJoinBlockedAsync(string sessionName, SessionInProgressJoinPolicy policy = SessionInProgressJoinPolicy.FastFail);

		UniTask<bool> TryReconnectAsync(string sessionName);
	}
}
