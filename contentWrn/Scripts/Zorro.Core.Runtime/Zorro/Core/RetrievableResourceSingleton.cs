using System;
using UnityEngine;

namespace Zorro.Core
{
	public class RetrievableResourceSingleton<T> : MonoBehaviour where T : RetrievableResourceSingleton<T>
	{
		private static bool m_shuttingDown;

		private static T _instance;

		public static T Instance
		{
			get
			{
				if (m_shuttingDown)
				{
					return _instance;
				}
				if (_instance == null)
				{
					_instance = CreateInstance();
				}
				return _instance;
			}
		}

		protected virtual void OnCreated()
		{
		}

		private static T CreateInstance()
		{
			if (!Application.isPlaying)
			{
				throw new Exception("Trying to access RetrievableResourceSingleton can only be used in play mode. Specified type: " + typeof(T).Name);
			}
			_instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(typeof(T).Name)).GetComponent<T>();
			_instance.OnCreated();
			return _instance;
		}

		private void OnApplicationQuit()
		{
			m_shuttingDown = true;
		}
	}
}
