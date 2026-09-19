using UnityEngine;

namespace solidocean
{
	public abstract class Singleton<T> : MonoBehaviour where T : Component
	{
		private static T instance;

		private static bool m_applicationIsQuitting;

		public static T GetInstance()
		{
			if (m_applicationIsQuitting)
			{
				return null;
			}
			if (instance == null)
			{
				instance = Object.FindFirstObjectByType<T>();
				if (instance == null)
				{
					instance = new GameObject
					{
						name = typeof(T).Name
					}.AddComponent<T>();
				}
			}
			return instance;
		}

		protected virtual void Awake()
		{
			if (instance == null)
			{
				instance = this as T;
			}
			else if (instance != this as T)
			{
				Object.Destroy(base.gameObject);
			}
		}

		private void OnApplicationQuit()
		{
			m_applicationIsQuitting = true;
		}
	}
}
