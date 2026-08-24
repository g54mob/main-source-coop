using System;
using System.Collections.Generic;
using HeathenEngineering.Events;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering
{
	[Serializable]
	public abstract class DataVariable<T> : DataVariable, IChangeEvent<T>, IGameEvent<T>, IGameEvent, IDataVariable<T>, IDataVariable
	{
		[SerializeField]
		internal T m_value;

		[HideInInspector]
		public List<IGameEventListener<T>> typeListeners = new List<IGameEventListener<T>>();

		[HideInInspector]
		public List<UnityAction<EventData<T>>> typeSenderActions = new List<UnityAction<EventData<T>>>();

		[HideInInspector]
		public List<IChangeEventListener<T>> typeChangeListeners = new List<IChangeEventListener<T>>();

		[HideInInspector]
		public List<UnityAction<ChangeEventData<T>>> typeChangeSenderActions = new List<UnityAction<ChangeEventData<T>>>();

		public T Value
		{
			get
			{
				return GetValue();
			}
			set
			{
				SetValue(value);
			}
		}

		public override object ObjectValue
		{
			get
			{
				return GetValue();
			}
			set
			{
				SetValue((T)value);
			}
		}

		public void AddListener(IGameEventListener<T> listener)
		{
			typeListeners.Add(listener);
		}

		public void AddListener(UnityAction<EventData<T>> listener)
		{
			typeSenderActions.Add(listener);
		}

		public void AddListener(IChangeEventListener<T> listener)
		{
			typeChangeListeners.Add(listener);
		}

		public void AddListener(UnityAction<ChangeEventData<T>> listener)
		{
			typeChangeSenderActions.Add(listener);
		}

		public T GetValue()
		{
			return m_value;
		}

		public override void Raise(object sender)
		{
			Raise(sender, default(T), default(T));
		}

		public virtual void Raise(object sender, T value)
		{
			Raise(sender, default(T), value);
		}

		public virtual void Raise(object sender, T oldValue, T newValue)
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

		public void RemoveListener(IGameEventListener<T> listener)
		{
			typeListeners.Remove(listener);
		}

		public void RemoveListener(UnityAction<EventData<T>> listener)
		{
			typeSenderActions.Remove(listener);
		}

		public void RemoveListener(IChangeEventListener<T> listener)
		{
			typeChangeListeners.Remove(listener);
		}

		public void RemoveListener(UnityAction<ChangeEventData<T>> listener)
		{
			typeChangeSenderActions.Remove(listener);
		}

		public void SetValue(T value)
		{
			if (!EqualityComparer<T>.Default.Equals(m_value, value))
			{
				T value2 = m_value;
				m_value = value;
				Raise(this, value2, m_value);
			}
		}

		public void SetValue(IDataVariable<T> value)
		{
			SetValue(value.Value);
		}

		public void Invoke(T value)
		{
			Raise(this, value);
		}

		public void Raise(T value)
		{
			Raise(this, value);
		}

		public void Invoke(object sender, T value)
		{
			Raise(sender, value);
		}
	}
	public abstract class DataVariable : GameEvent, IDataVariable, IGameEvent
	{
		public abstract object ObjectValue { get; set; }
	}
}
