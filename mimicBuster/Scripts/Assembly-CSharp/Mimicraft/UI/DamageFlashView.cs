using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class DamageFlashView : MonoBehaviour
	{
		private const float FlashAlpha = 0.35f;

		private const float FadeSpeed = 1.2f;

		[SerializeField]
		private Image overlay;

		public static DamageFlashView Instance { get; private set; }

		public static DamageFlashView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "DamageFlash");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.6f, 0f, 0f, 0f);
			image.raycastTarget = false;
			DamageFlashView damageFlashView = rectTransform.gameObject.AddComponent<DamageFlashView>();
			damageFlashView.overlay = image;
			return damageFlashView;
		}

		private void Awake()
		{
			Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Flash()
		{
			Flash(1f);
		}

		public void Flash(float intensity)
		{
			float b = 0.35f * Mathf.Clamp01(intensity);
			Color color = overlay.color;
			color.a = Mathf.Max(color.a, b);
			overlay.color = color;
		}

		private void Update()
		{
			if (!(overlay.color.a <= 0f))
			{
				Color color = overlay.color;
				color.a = Mathf.Max(0f, color.a - 1.2f * Time.deltaTime);
				overlay.color = color;
			}
		}
	}
}
