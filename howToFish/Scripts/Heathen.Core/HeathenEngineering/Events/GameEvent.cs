using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	[CreateAssetMenu(menuName = "System Core/Events/Simple Events/Void")]
	public class GameEvent : ScriptableObject, IGameEvent
	{
		internal const string dd01 = "Empty UnityActions are being depricated in favor of UnityAction<object> where the sender of the event will be passed in as well. \nPlease use AddListener(UnityAction<object>) instead.";

		[HideInInspector]
		public List<IGameEventListener> listeners = new List<IGameEventListener>();

		[HideInInspector]
		public List<UnityAction<EventData>> senderActions = new List<UnityAction<EventData>>();

		public virtual void Invoke(object sender)
		{
			Raise(sender);
		}

		public virtual void Raise(object sender)
		{
			EventData eventData = new EventData
			{
				sender = sender
			};
			for (int num = listeners.Count - 1; num >= 0; num--)
			{
				if (listeners[num] != null)
				{
					listeners[num].OnEventRaised(eventData);
				}
			}
			for (int num2 = senderActions.Count - 1; num2 >= 0; num2--)
			{
				if (senderActions[num2] != null)
				{
					senderActions[num2](eventData);
				}
			}
		}

		public void AddListener(IGameEventListener listener)
		{
			listeners.Add(listener);
		}

		public void AddListener(UnityAction<EventData> listener)
		{
			senderActions.Add(listener);
		}

		public void RemoveListener(IGameEventListener listener)
		{
			listeners.Remove(listener);
		}

		public void RemoveListener(UnityAction<EventData> listener)
		{
			senderActions.Remove(listener);
		}

		public void Invoke()
		{
			Raise(this);
		}

		public void Raise()
		{
			Raise(this);
		}
	}
	public abstract class GameEvent<T> : GameEvent, IGameEvent<T>, IGameEvent
	{
		[HideInInspector]
		public List<IGameEventListener<T>> typeListeners = new List<IGameEventListener<T>>();

		[HideInInspector]
		public List<UnityAction<EventData<T>>> typeSenderActions = new List<UnityAction<EventData<T>>>();

		public void AddListener(IGameEventListener<T> listener)
		{
			typeListeners.Add(listener);
		}

		public void AddListener(UnityAction<EventData<T>> listener)
		{
			typeSenderActions.Add(listener);
		}

		public virtual void RaiseSimple(T value)
		{
			Raise(this, value);
		}

		public virtual void InvokeSimple(T value)
		{
			Raise(this, value);
		}

		public override void Raise(object sender)
		{
			Raise(sender, default(T));
		}

		public virtual void Invoke(object sender, T value)
		{
			Raise(sender, value);
		}

		public virtual void Raise(object sender, T value)
		{
			EventData eventData = new EventData
			{
				sender = sender
			};
			EventData<T> eventData2 = new EventData<T>(sender, value);
			for (int num = listeners.Count - 1; num >= 0; num--)
			{
				if (listeners[num] != null)
				{
					listeners[num].OnEventRaised(eventData);
				}
			}
			for (int num2 = senderActions.Count - 1; num2 >= 0; num2--)
			{
				if (senderActions[num2] != null)
				{
					senderActions[num2](eventData);
				}
			}
			for (int num3 = typeListeners.Count - 1; num3 >= 0; num3--)
			{
				if (typeListeners[num3] != null)
				{
					typeListeners[num3].OnEventRaised(eventData2);
				}
			}
			for (int num4 = typeSenderActions.Count - 1; num4 >= 0; num4--)
			{
				if (typeSenderActions[num4] != null)
				{
					typeSenderActions[num4](eventData2);
				}
			}
		}

		public void RemoveListener(IGameEventListener<T> listener)
		{
			typeListeners.Remove(listener);
		}

		public void RemoveListener(UnityAction<EventData<T>> listener)
		{
			typeSenderActions.Remove(listener);
		}

		public virtual void Invoke(T value)
		{
			Raise(this, value);
		}

		public virtual void Raise(T value)
		{
			Raise(this, value);
		}
	}
}
