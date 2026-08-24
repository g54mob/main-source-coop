using System.Collections.Generic;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	public interface ICollectionChangeEvent<T> : IChangeEvent<List<T>>, IGameEvent<List<T>>, IGameEvent
	{
		void AddListener(ICollectionChangeEventListener<T> listener);

		void RemoveListener(ICollectionChangeEventListener<T> listener);

		void AddListener(UnityAction<CollectionChangeEventData<T>> listener);

		void RemoveListener(UnityAction<CollectionChangeEventData<T>> listener);
	}
}
