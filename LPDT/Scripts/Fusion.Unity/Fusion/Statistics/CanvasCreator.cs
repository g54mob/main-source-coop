using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	internal static class CanvasCreator
	{
		private static readonly Vector2 ReferenceResolution = new Vector2(1920f, 1080f);

		private static readonly float WorldSpaceWidthInMeters = 5f;

		internal static Canvas CreateRootCanvas(string name)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.layer = LayerMask.NameToLayer("UI");
			Canvas canvas = gameObject.AddComponent<Canvas>();
			CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
			SetCanvasToScreenSpace(canvas, scaler);
			gameObject.AddComponent<GraphicRaycaster>();
			if (!EventSystem.current)
			{
				new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule)).transform.SetParent(gameObject.transform);
			}
			return canvas;
		}

		internal static void SetCanvasToScreenSpace(Canvas canvas, CanvasScaler scaler)
		{
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.pixelPerfect = true;
			canvas.vertexColorAlwaysGammaSpace = true;
			canvas.sortingOrder = 20;
			canvas.transform.localScale = Vector3.one;
			canvas.transform.localPosition = Vector3.zero;
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scaler.referenceResolution = ReferenceResolution;
			scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			scaler.matchWidthOrHeight = 0.5f;
			if (canvas.gameObject.TryGetComponent<FusionBasicBillboard>(out var component))
			{
				Object.Destroy(component);
			}
			Object.DontDestroyOnLoad(canvas.gameObject);
		}

		internal static void SetCanvasToWorldSpace(Canvas canvas, CanvasScaler scaler)
		{
			canvas.renderMode = RenderMode.WorldSpace;
			RectTransform component = canvas.GetComponent<RectTransform>();
			component.localScale = Vector3.one * (WorldSpaceWidthInMeters / component.rect.width);
			component.localPosition = Vector3.zero;
			canvas.worldCamera = Camera.main;
			canvas.gameObject.AddComponent<FusionBasicBillboard>();
		}
	}
}
