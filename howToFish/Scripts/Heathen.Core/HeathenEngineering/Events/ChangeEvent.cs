using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.Events
{
	public abstract class ChangeEvent<T> : GameEvent<T>, IChangeEvent<T>, IGameEvent<T>, IGameEvent
	{
		[HideInInspector]
		public List<IChangeEventListener<T>> typeChangeListeners = new List<IChangeEventListener<T>>();

		[HideInInspector]
		public List<UnityAction<ChangeEventData<T>>> typeChangeSenderActions = new List<UnityAction<ChangeEventData<T>>>();

		public void AddListener(IChangeEventListener<T> listener)
		{
			typeChangeListeners.Add(listener);
		}

		public void AddListener(UnityAction<ChangeEventData<T>> listener)
		{
			typeChangeSenderActions.Add(listener);
		}

		public override void Raise(object sender)
		{
			Raise(sender, default(T), default(T));
		}

		public override void Raise(object sender, T value)
		{
			Raise(sender, default(T), value);
		}

		public void Raise(object sender, T oldValue, T newValue)
		{
			EventData eventData = new EventData
			{
				sender = sender
			};
			EventData<T> eventData2 = new EventData<T>(sender, newValue);
			ChangeEventData<T> changeEventData = new ChangeEventData<T>(sender, oldValue, newValue);
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
			for (int num5 = typeChangeListeners.Count - 1; num5 >= 0; num5--)
			{
				if (typeChangeListeners[num5] != null)
				{
					typeChangeListeners[num5].OnEventRaised(changeEventData);
				}
			}
			for (int num6 = typeChangeSenderActions.Count - 1; num6 >= 0; num6--)
			{
				if (typeChangeSenderActions[num6] != null)
				{
					typeChangeSenderActions[num6](changeEventData);
				}
			}
		}

		public void RemoveListener(IChangeEventListener<T> listener)
		{
			typeChangeListeners.Remove(listener);
		}

		public void RemoveListener(UnityAction<ChangeEventData<T>> listener)
		{
			typeChangeSenderActions.Remove(listener);
		}
	}
}
