using System;
using System.Collections.Generic;
using RSG.Muffin.EventBusModule.Interfaces;

namespace RSG.Muffin.EventBusModule
{
	public class GenericEventBus<TBaseEventType> : IGenericEventBus<TBaseEventType>
	{
		private class EventListenersContainerLocator<TEventType> where TEventType : TBaseEventType
		{
			private static readonly Dictionary<IGenericEventBus<TBaseEventType>, EventListenersContainer<TEventType>> ContainerByBusDictionary = new Dictionary<IGenericEventBus<TBaseEventType>, EventListenersContainer<TEventType>>();

			public static EventListenersContainer<TEventType> GetContainer(IGenericEventBus<TBaseEventType> forBus)
			{
				if (ContainerByBusDictionary.TryGetValue(forBus, out var value))
				{
					return value;
				}
				value = new EventListenersContainer<TEventType>();
				value.OnDispose += OnContainerDisposed;
				ContainerByBusDictionary[forBus] = value;
				return value;
			}

			private static void OnContainerDisposed(EventListenersContainer<TEventType> disposedContainer)
			{
				foreach (KeyValuePair<IGenericEventBus<TBaseEventType>, EventListenersContainer<TEventType>> item in ContainerByBusDictionary)
				{
					if (item.Value == disposedContainer)
					{
						ContainerByBusDictionary[item.Key] = null;
					}
				}
				disposedContainer.OnDispose -= OnContainerDisposed;
			}
		}

		private class EventListenersContainer<TEventType> : IDisposable where TEventType : TBaseEventType
		{
			private Action<TEventType> _firstListener;

			public event Action<EventListenersContainer<TEventType>> OnDispose;

			public void InvokeListeners(TEventType eventHandler)
			{
				_firstListener?.Invoke(eventHandler);
			}

			public void AddListener(Action<TEventType> @event)
			{
				if (_firstListener != null)
				{
					_firstListener = (Action<TEventType>)Delegate.Combine(_firstListener, @event);
				}
				else
				{
					_firstListener = @event;
				}
			}

			public void RemoveListener(Action<TEventType> @event)
			{
				if (_firstListener != null)
				{
					_firstListener = (Action<TEventType>)Delegate.Remove(_firstListener, @event);
				}
			}

			public void Dispose()
			{
				_firstListener = null;
				this.OnDispose?.Invoke(this);
			}
		}

		private readonly List<IDisposable> _disposables = new List<IDisposable>();

		private bool _publishingStopped;

		public void Subscribe<TEventType>(Action<TEventType> listener) where TEventType : TBaseEventType
		{
			GetEventListenersContainer<TEventType>().AddListener(listener);
		}

		public void Unsubscribe<TEventType>(Action<TEventType> listener) where TEventType : TBaseEventType
		{
			GetEventListenersContainer<TEventType>().RemoveListener(listener);
		}

		public void Publish<TEventType>(TEventType eventHandler) where TEventType : TBaseEventType
		{
			if (!_publishingStopped)
			{
				GetEventListenersContainer<TEventType>().InvokeListeners(eventHandler);
			}
		}

		public void StopPublish()
		{
			_publishingStopped = true;
		}

		public void RestartPublish()
		{
			_publishingStopped = false;
		}

		public void CleanUp()
		{
			_disposables.ForEach(delegate(IDisposable cleanUpper)
			{
				cleanUpper.Dispose();
			});
			_disposables.Clear();
		}

		private EventListenersContainer<TEventType> GetEventListenersContainer<TEventType>() where TEventType : TBaseEventType
		{
			EventListenersContainer<TEventType> container = EventListenersContainerLocator<TEventType>.GetContainer(this);
			if (!_disposables.Contains(container))
			{
				_disposables.Add(container);
			}
			return container;
		}
	}
}
