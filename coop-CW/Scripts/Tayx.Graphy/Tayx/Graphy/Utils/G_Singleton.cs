using UnityEngine;

namespace Tayx.Graphy.Utils
{
	public class G_Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		private static object _lock = new object();

		public static T Instance
		{
			get
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						Debug.Log("[Singleton] An instance of " + typeof(T)?.ToString() + " is trying to be accessed, but it wasn't initialized first. Make sure to add an instance of " + typeof(T)?.ToString() + " in the scene before  trying to access it.");
					}
					return _instance;
				}
			}
		}

		private void Awake()
		{
			if (_instance != null)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				_instance = GetComponent<T>();
			}
		}

		private void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}
	}
}
