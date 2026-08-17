using GameKit.Dependencies.Inspectors;
using UnityEngine;
using UnityEngine.UI;

namespace GameKit.Utilities.Types.CanvasContainers
{
	public class FloatingImage : FloatingContainer
	{
		[Tooltip("Renderer to apply sprite on.")]
		[SerializeField]
		[Group("Components", false)]
		protected Image Renderer;

		public virtual void SetSprite(Sprite sprite, Vector3? sizeOverride)
		{
			Renderer.sprite = sprite;
			Vector3 vector = ((!sizeOverride.HasValue) ? (sprite.bounds.size * sprite.pixelsPerUnit) : sizeOverride.Value);
			Renderer.rectTransform.sizeDelta = vector;
		}
	}
}
