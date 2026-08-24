using System.Collections.Generic;

namespace HeathenEngineering.Events
{
	public interface ICollectionChangeEventListener<T> : IChangeEventListener<List<T>>, IGameEventListener<List<T>>, IGameEventListener
	{
		void OnEventRaised(CollectionChangeEventData<T> data);
	}
}
