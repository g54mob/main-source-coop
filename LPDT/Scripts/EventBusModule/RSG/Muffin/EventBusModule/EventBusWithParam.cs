using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Muffin.EventBusModule.Interfaces;

namespace RSG.Muffin.EventBusModule
{
	public class EventBusWithParam<TEvent, TParam> : IEventBusWithParam<TEvent, TParam>
	{
		private readonly IDictionary<TEvent, Action<TParam>> _events = new Dictionary<TEvent, Action<TParam>>();

		protected bool _stopPublish;

		public void Subscribe(TEvent @event, Action<TParam> listener)
		{
			if (_events.TryGetValue(@event, out var value))
			{
				value = (Action<TParam>)Delegate.Combine(value, listener);
				_events[@event] = value;
			}
			else
			{
				_events.Add(@event, listener);
			}
		}

		public void Unsubscribe(TEvent @event, Action<TParam> listener)
		{
			if (_events.TryGetValue(@event, out var value))
			{
				value = (Action<TParam>)Delegate.Remove(value, listener);
				_events[@event] = value;
			}
		}

		public void Publish(TEvent @event, TParam parameter)
		{
			if (!_stopPublish && _events.TryGetValue(@event, out var value))
			{
				value?.Invoke(parameter);
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
			List<Action<TParam>> list = _events.Select((KeyValuePair<TEvent, Action<TParam>> d) => d.Value).ToList();
			for (int num = 0; num < list.Count; num++)
			{
				list[num] = null;
			}
			_events.Clear();
		}
	}
}
