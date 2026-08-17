using System;
using UnityEngine;

namespace EvilCore
{
	[Obsolete("Use VContainer dependency injection instead.")]
	public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
	{
		private static volatile T _instance;

		protected virtual bool PersistAcrossScenes => false;

		public static T Instance => _instance;

		protected virtual void Awake()
		{
			if (_instance != null && _instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			_instance = this as T;
			if (PersistAcrossScenes)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		protected virtual void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}
	}
}
