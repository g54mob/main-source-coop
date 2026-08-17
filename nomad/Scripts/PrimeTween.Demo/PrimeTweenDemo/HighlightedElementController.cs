using JetBrains.Annotations;
using PrimeTween;
using UnityEngine;

namespace PrimeTweenDemo
{
	public class HighlightedElementController : MonoBehaviour
	{
		[SerializeField]
		private Camera mainCamera;

		[SerializeField]
		private CameraProjectionMatrixAnimation cameraProjectionMatrixAnimation;

		private static readonly int emissionColorPropId = Shader.PropertyToID("_EmissionColor");

		[CanBeNull]
		public HighlightableElement current { get; private set; }

		private void Update()
		{
			if (cameraProjectionMatrixAnimation.IsAnimating)
			{
				return;
			}
			if (Application.isMobilePlatform && InputController.touchSupported && !InputController.Get())
			{
				SetCurrentHighlighted(null);
				return;
			}
			Vector2 screenPosition = InputController.screenPosition;
			if (new Rect(0f, 0f, Screen.width, Screen.height).Contains(screenPosition))
			{
				HighlightableElement currentHighlighted = RaycastHighlightableElement(mainCamera.ScreenPointToRay(screenPosition));
				SetCurrentHighlighted(currentHighlighted);
				if (current != null && InputController.GetDown())
				{
					current.GetComponent<Animatable>().OnClick();
				}
			}
		}

		[CanBeNull]
		private static HighlightableElement RaycastHighlightableElement(Ray ray)
		{
			if (!Physics.Raycast(ray, out var hitInfo))
			{
				return null;
			}
			return hitInfo.collider.GetComponentInParent<HighlightableElement>();
		}

		private void SetCurrentHighlighted([CanBeNull] HighlightableElement newHighlighted)
		{
			if (newHighlighted != current)
			{
				if (current != null)
				{
					AnimateHighlightedElement(current, isHighlighted: false);
				}
				current = newHighlighted;
				if (newHighlighted != null)
				{
					AnimateHighlightedElement(newHighlighted, isHighlighted: true);
				}
			}
		}

		private static void AnimateHighlightedElement([NotNull] HighlightableElement highlightable, bool isHighlighted)
		{
			Tween.LocalPositionZ(highlightable.highlightAnchor, isHighlighted ? 0.08f : 0f, 0.3f);
			MeshRenderer[] models = highlightable.models;
			for (int i = 0; i < models.Length; i++)
			{
				Tween.MaterialColor(models[i].sharedMaterial, emissionColorPropId, isHighlighted ? (Color.white * 0.25f) : Color.black, 0.2f, Ease.OutQuad);
			}
		}
	}
}
