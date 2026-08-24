namespace HeathenEngineering.Events
{
	public interface IChangeEventListener<T> : IGameEventListener<T>, IGameEventListener
	{
		void OnEventRaised(ChangeEventData<T> data);
	}
}
