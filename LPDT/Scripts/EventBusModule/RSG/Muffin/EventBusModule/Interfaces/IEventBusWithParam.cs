using System;
using JetBrains.Annotations;

namespace RSG.Muffin.EventBusModule.Interfaces
{
	[PublicAPI]
	public interface IEventBusWithParam<in TEvent, TParam>
	{
		void Publish(TEvent @event, TParam param);

		void Subscribe(TEvent @event, Action<TParam> listener);

		void Unsubscribe(TEvent @event, Action<TParam> listener);

		void StopPublish();

		void RestartPublish();

		void Cleanup();
	}
}
