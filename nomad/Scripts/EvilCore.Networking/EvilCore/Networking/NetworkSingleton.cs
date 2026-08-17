using System;
using Mirror;

namespace EvilCore.Networking
{
	[Obsolete("Use VContainer dependency injection instead.")]
	public class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
	{
		private static volatile T _instance;

		public static T Instance => _instance;

		protected virtual void Awake()
		{
			_instance = this as T;
		}

		protected virtual void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
