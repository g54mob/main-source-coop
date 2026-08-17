using UnityEngine;

namespace RootMotion
{
	public abstract class LazySingleton<T> : MonoBehaviour where T : LazySingleton<T>
	{
		private static T sInstance;

		public static bool hasInstance => sInstance != null;

		public static T instance
		{
			get
			{
				if (sInstance == null)
				{
					sInstance = new GameObject(typeof(T).ToString()).AddComponent<T>();
				}
				return sInstance;
			}
		}

		protected virtual void Awake()
		{
			sInstance = (T)this;
		}
	}
}
