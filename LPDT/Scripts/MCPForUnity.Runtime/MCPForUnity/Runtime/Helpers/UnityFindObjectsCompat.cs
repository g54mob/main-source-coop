using System;
using UnityEngine;

namespace MCPForUnity.Runtime.Helpers
{
	public static class UnityFindObjectsCompat
	{
		public static T[] FindAll<T>() where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None);
		}

		public static UnityEngine.Object[] FindAll(Type type)
		{
			return UnityEngine.Object.FindObjectsByType(type, FindObjectsSortMode.None);
		}

		public static UnityEngine.Object[] FindAll(Type type, bool includeInactive)
		{
			return UnityEngine.Object.FindObjectsByType(type, includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		}

		public static UnityEngine.Object FindAny(Type type)
		{
			return UnityEngine.Object.FindAnyObjectByType(type);
		}
	}
}
