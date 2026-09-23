using UnityEngine;

namespace Mimicraft.UI
{
	public static class HudVisibility
	{
		public static void Apply(Component view, bool shown)
		{
			if (view == null)
			{
				return;
			}
			if (!view.TryGetComponent<CanvasGroup>(out var component))
			{
				if (shown)
				{
					return;
				}
				component = view.gameObject.AddComponent<CanvasGroup>();
			}
			component.alpha = (shown ? 1f : 0f);
			component.blocksRaycasts = shown;
			component.interactable = shown;
		}
	}
}
