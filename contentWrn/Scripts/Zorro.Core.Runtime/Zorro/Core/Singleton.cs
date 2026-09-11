using Sirenix.Utilities;
using UnityEngine;

namespace Zorro.Core
{
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		internal bool m_hasBeenCreated;

		private static bool m_shuttingDown;

		private static T _instance;

		public static T Instance
		{
			get
			{
				if (m_shuttingDown)
				{
					return null;
				}
				if (_instance == null && typeof(T).ImplementsOrInherits(typeof(IFindIfNull)))
				{
					_instance = FindInstance();
					if (_instance != null)
					{
						_instance.m_hasBeenCreated = true;
						_instance.OnCreated();
					}
				}
				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this as T;
				if (!m_hasBeenCreated)
				{
					m_hasBeenCreated = true;
					OnCreated();
				}
			}
			else if (_instance != this)
			{
				Object.Destroy(base.gameObject);
			}
		}

		protected virtual void OnCreated()
		{
		}

		private void OnApplicationQuit()
		{
			m_shuttingDown = true;
		}

		public static T FindInstance()
		{
			if (_instance != null)
			{
				return _instance;
			}
			_instance = Object.FindObjectOfType<T>();
			return _instance;
		}

		public static void ClearInstance()
		{
			if (_instance != null)
			{
				GameObject obj = _instance.gameObject;
				_instance = null;
				Object.Destroy(obj);
			}
		}
	}
}
