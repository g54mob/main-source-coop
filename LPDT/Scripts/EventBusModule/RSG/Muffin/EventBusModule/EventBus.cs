using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Muffin.EventBusModule.Interfaces;

namespace RSG.Muffin.EventBusModule
{
	public class EventBus<TEvent> : IEventBus<TEvent>
	{
		private readonly Dictionary<TEvent, Action> _events = new Dictionary<TEvent, Action>();

		protected bool _stopPublish;

		public void Subscribe(TEvent @event, Action listener)
		{
			if (_events.ContainsKey(@event))
			{
				Dictionary<TEvent, Action> events = _events;
				events[@event] = (Action)Delegate.Combine(events[@event], listener);
			}
			else
			{
				_events.Add(@event, listener);
			}
		}

		public void Unsubscribe(TEvent @event, Action listener)
		{
			if (_events.ContainsKey(@event))
			{
				Dictionary<TEvent, Action> events = _events;
				events[@event] = (Action)Delegate.Remove(events[@event], listener);
			}
		}

		public void Publish(TEvent @event)
		{
			if (!_stopPublish && _events.ContainsKey(@event))
			{
				_events[@event]?.Invoke();
			}
		}

		public void StopPublish()
		{
			_stopPublish = true;
		}

		public void RestartPublish()
		{
			_stopPublish = false;
		}

		public void Cleanup()
		{
			List<Action> list = _events.Select((KeyValuePair<TEvent, Action> d) => d.Value).ToList();
			for (int num = 0; num < list.Count; num++)
			{
				list[num] = null;
			}
			_events.Clear();
		}
	}
}
