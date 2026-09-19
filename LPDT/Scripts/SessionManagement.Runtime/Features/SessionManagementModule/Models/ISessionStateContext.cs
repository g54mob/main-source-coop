using System;
using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface ISessionStateContext
	{
		bool IsAuthority { get; }

		int CurrentEpoch { get; }

		bool IsTargetStateActive { get; set; }

		SessionState PreviousState { get; }

		void Transition(SessionState target);

		UniTask AuthorityGate(SessionAuthorityLane lane, Func<UniTask> work);

		UniTask AuthorityGate(SessionAuthorityLane lane, Action work);

		UniTask AuthorityGate(SessionAuthorityLane lane, Action<int> work);

		UniTask AuthorityGate(SessionAuthorityLane lane, Func<int, UniTask> work);

		UniTask SynchronizationGate(SynchronizationGateKey key);
	}
}
