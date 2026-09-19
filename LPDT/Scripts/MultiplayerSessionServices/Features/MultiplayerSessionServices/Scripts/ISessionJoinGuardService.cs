using Cysharp.Threading.Tasks;

namespace Features.MultiplayerSessionServices.Scripts
{
	public interface ISessionJoinGuardService
	{
		UniTask<bool> IsJoinBlockedAsync(string sessionName, SessionInProgressJoinPolicy policy = SessionInProgressJoinPolicy.FastFail);

		UniTask<bool> IsSessionValidAsync(string sessionName);
	}
}
