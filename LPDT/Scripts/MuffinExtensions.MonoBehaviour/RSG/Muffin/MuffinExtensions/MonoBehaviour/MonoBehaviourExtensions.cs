using UnityEngine;

namespace RSG.Muffin.MuffinExtensions.MonoBehaviour
{
	public static class MonoBehaviourExtensions
	{
		public static TComponent InstantiateComponent<TComponent>(this Object monoBehaviour, GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent) where TComponent : Object
		{
			return Object.Instantiate(gameObject, position, rotation, parent).GetComponent<TComponent>();
		}
	}
}
