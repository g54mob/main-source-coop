namespace HeathenEngineering.Events
{
	public abstract class ChangeEventListener<T> : GameEventListener<T>, IChangeEventListener<T>, IGameEventListener<T>, IGameEventListener
	{
		public abstract IDataVariable<T> m_variable { get; }

		public abstract UnityChangeEvent<T> m_changeresponce { get; }

		public override void EnableListener()
		{
			base.EnableListener();
			if (m_variable != null)
			{
				m_variable.AddListener(this);
			}
		}

		public override void DisableListener()
		{
			base.DisableListener();
			if (m_variable != null)
			{
				m_variable.RemoveListener(this);
			}
		}

		public override void OnEventRaised(EventData<T> data)
		{
			base.OnEventRaised(data);
			ChangeEventData<T> arg = new ChangeEventData<T>
			{
				sender = data.sender,
				oldValue = default(T),
				newValue = data.value
			};
			m_changeresponce.Invoke(arg);
		}

		public override void OnEventRaised(EventData data)
		{
			base.OnEventRaised(data);
			ChangeEventData<T> arg = new ChangeEventData<T>
			{
				sender = data.sender,
				oldValue = default(T),
				newValue = default(T)
			};
			m_changeresponce.Invoke(arg);
		}

		public virtual void OnEventRaised(ChangeEventData<T> data)
		{
			m_response.Invoke(new EventData<T>(data.sender, data.newValue));
			m_changeresponce.Invoke(data);
		}
	}
}
