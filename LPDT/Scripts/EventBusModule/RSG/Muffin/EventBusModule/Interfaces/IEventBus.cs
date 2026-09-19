using System;
using JetBrains.Annotations;

namespace RSG.Muffin.EventBusModule.Interfaces
{
	[PublicAPI]
	public interface IEventBus<in TEvent>
	{
		void Publish(TEvent @event);

		void Subscribe(TEvent @event, Action listener);

		void Unsubscribe(TEvent @event, Action listener);

		void StopPublish();

		void RestartPublish();

		void Cleanup();
	}
}
