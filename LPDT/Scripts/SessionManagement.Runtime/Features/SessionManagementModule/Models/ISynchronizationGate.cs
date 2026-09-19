using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface ISynchronizationGate
	{
		void OpenOnEntry(SynchronizationGateKey key, int epoch, int windowTicks);

		bool IsPassed(SynchronizationGateKey key);

		void Reconcile(SynchronizationGateKey key, bool isReady);

		UniTask WaitUntilPassedAsync(SynchronizationGateKey key);
	}
}
