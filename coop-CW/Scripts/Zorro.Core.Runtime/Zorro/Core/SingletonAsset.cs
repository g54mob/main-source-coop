using Sirenix.OdinInspector;
using UnityEngine;

namespace Zorro.Core
{
	public abstract class SingletonAsset<T> : SerializedScriptableObject where T : SingletonAsset<T>
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Resources.Load<T>(typeof(T).Name);
					if (_instance != null)
					{
						_instance.OnLoaded();
					}
				}
				return _instance;
			}
		}

		public virtual void OnLoaded()
		{
		}
	}
}
