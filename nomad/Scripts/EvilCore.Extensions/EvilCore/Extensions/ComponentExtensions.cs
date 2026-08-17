using UnityEngine;

namespace EvilCore.Extensions
{
	public static class ComponentExtensions
	{
		public static T GetOrAddComponent<T>(this Component component) where T : Component
		{
			return component.gameObject.GetOrAddComponent<T>();
		}

		public static bool TryGetComponentInChildren<T>(this Component sourceComponent, out T component, bool includeInactive = false) where T : Component
		{
			return component = sourceComponent.gameObject.GetComponentInChildren<T>(includeInactive);
		}

		public static bool TryGetComponentInParent<T>(this Component sourceComponent, out T component, bool includeInactive = false) where T : Component
		{
			return component = sourceComponent.gameObject.GetComponentInParent<T>(includeInactive);
		}
	}
}
