using System;

namespace RSG.Muffin.EventBusModule.Interfaces
{
	public interface IGenericEventBus<in TBaseEvent>
	{
		void Subscribe<TEvent>(Action<TEvent> listener) where TEvent : TBaseEvent;

		void Unsubscribe<TEvent>(Action<TEvent> listener) where TEvent : TBaseEvent;

		void Publish<TEvent>(TEvent eventHandler) where TEvent : TBaseEvent;

		void StopPublish();

		void RestartPublish();

		void CleanUp();
	}
}
