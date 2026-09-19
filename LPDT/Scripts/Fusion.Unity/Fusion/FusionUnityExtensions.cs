using System;
using UnityEngine;

namespace Fusion
{
	public static class FusionUnityExtensions
	{
		public static T[] FindObjectsByType<T>() where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None);
		}

		public static T[] FindObjectsByType<T>(FindObjectsInactive findObjectsInactive) where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsByType<T>(findObjectsInactive, FindObjectsSortMode.None);
		}

		public static UnityEngine.Object[] FindObjectsByType(Type type)
		{
			return UnityEngine.Object.FindObjectsByType(type, FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		}

		public static UnityEngine.Object[] FindObjectsByType(Type type, FindObjectsInactive findObjectsInactive)
		{
			return UnityEngine.Object.FindObjectsByType(type, findObjectsInactive, FindObjectsSortMode.None);
		}
	}
}
