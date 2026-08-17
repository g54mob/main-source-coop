using UnityEngine;

namespace Ami.Extension
{
	public abstract class InstanceWrapper<T> : IRecyclable<InstanceWrapper<T>> where T : Object
	{
		private T _instance;

		protected T Instance
		{
			get
			{
				if (!IsAvailable())
				{
					return null;
				}
				return _instance;
			}
		}

		protected InstanceWrapper(T instance)
		{
			_instance = instance;
		}

		protected bool IsAvailable(bool logWarning = true)
		{
			if (_instance != null)
			{
				return true;
			}
			if (logWarning)
			{
				LogInstanceIsNull();
			}
			return false;
		}

		public virtual void UpdateInstance(T newInstance)
		{
			_instance = newInstance;
		}

		public virtual void Recycle()
		{
			_instance = null;
		}

		protected virtual void LogInstanceIsNull()
		{
			Debug.LogError("<b><color=#F3E9D7>[BroAudio] </color></b>The object that you are refering to is null.");
		}
	}
}
