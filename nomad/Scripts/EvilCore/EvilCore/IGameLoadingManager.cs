using UnityEngine.Events;

namespace EvilCore
{
	public interface IGameLoadingManager
	{
		UnityEvent<int> OnStateChanged { get; }

		UnityEvent OnLoadingComplete { get; }

		UnityEvent OnTimeout { get; }

		void RegisterStateDescription(int stateId, string description);

		string GetStateDescription(int state);

		void SetState(int newState);

		int GetCurrentState();

		bool IsLoadingComplete();

		bool IsTimedOut();

		void SuspendTimeout();

		void Reset();
	}
}
