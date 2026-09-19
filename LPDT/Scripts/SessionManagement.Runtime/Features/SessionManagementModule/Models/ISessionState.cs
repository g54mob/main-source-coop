using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface ISessionState
	{
		SessionState State { get; }

		SessionAuthorityLane ActivateLane { get; }

		SessionAuthorityLane DeactivateLane { get; }

		void Bind(ISessionStateContext context);

		UniTask EnterAsync();

		void UpdateActive();

		void UpdateActiveLocal();

		UniTask ExitAsync(SessionState next);
	}
}
