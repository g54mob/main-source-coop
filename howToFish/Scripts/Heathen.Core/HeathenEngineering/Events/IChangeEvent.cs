using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	public interface IChangeEvent<T> : IGameEvent<T>, IGameEvent
	{
		void Raise(object sender, T oldValue, T newValue);

		void AddListener(IChangeEventListener<T> listener);

		void RemoveListener(IChangeEventListener<T> listener);

		void AddListener(UnityAction<ChangeEventData<T>> listener);

		void RemoveListener(UnityAction<ChangeEventData<T>> listener);
	}
}
