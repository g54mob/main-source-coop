using System;
using UnityEngine;

namespace Zorro.Core
{
	public static class MonoBehaviourExtensions
	{
		public static void HasComponent<T>(this MonoBehaviour monoBehaviour, Action<T> onHas) where T : Component
		{
			T component = monoBehaviour.GetComponent<T>();
			if (component != null)
			{
				onHas?.Invoke(component);
			}
		}
	}
}
