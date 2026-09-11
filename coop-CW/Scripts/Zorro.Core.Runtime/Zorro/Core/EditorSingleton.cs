using Sirenix.OdinInspector;
using UnityEngine;

namespace Zorro.Core
{
	public class EditorSingleton<T> : SerializedMonoBehaviour where T : EditorSingleton<T>
	{
		private static T _instance;

		private static bool m_shuttingDown;

		public static T Instance
		{
			get
			{
				if (m_shuttingDown)
				{
					return null;
				}
				if (_instance == null)
				{
					_instance = new GameObject(typeof(T).Name).AddComponent<T>();
					_instance.gameObject.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
					_instance.OnCreated();
				}
				return _instance;
			}
		}

		public virtual void OnCreated()
		{
		}
	}
}
