using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	public interface IGameEvent
	{
		void Invoke();

		void Raise();

		void Invoke(object sender);

		void Raise(object sender);

		void AddListener(IGameEventListener listener);

		void RemoveListener(IGameEventListener listener);

		void AddListener(UnityAction<EventData> listener);

		void RemoveListener(UnityAction<EventData> listener);
	}
	public interface IGameEvent<T> : IGameEvent
	{
		void Invoke(T value);

		void Raise(T value);

		void Invoke(object sender, T value);

		void Raise(object sender, T value);

		void AddListener(IGameEventListener<T> listener);

		void RemoveListener(IGameEventListener<T> listener);

		void AddListener(UnityAction<EventData<T>> listener);

		void RemoveListener(UnityAction<EventData<T>> listener);
	}
}
