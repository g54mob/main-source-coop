using System.Threading;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;

namespace Features.MainMenuModule.Scripts
{
	public interface IQuickJoinService
	{
		UniTask<QuickJoinResult> FindOpenSessionAsync(CancellationToken cancellationToken = default(CancellationToken));

		UniTask<JoinRoomResult> JoinSessionAsync(string sessionName, string region);
	}
}
