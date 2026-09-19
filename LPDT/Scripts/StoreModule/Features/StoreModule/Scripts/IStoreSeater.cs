using Cysharp.Threading.Tasks;

namespace Features.StoreModule.Scripts
{
	public interface IStoreSeater
	{
		void ReleaseGrabs();

		UniTask EnsureBodyAuthorityAsync();

		void ResetRagdoll();

		UniTask SeatAtStoreSeatAsync();

		void CancelSeating();

		void RestorePreStoreState();
	}
}
