using Mimicraft.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class CrosshairView : MonoBehaviour
	{
		private const float BarLength = 8f;

		private const float BarThickness = 2f;

		private const float BarGap = 5f;

		private const float HitMarkerLength = 11f;

		private const float HitMarkerThickness = 3f;

		private const float HitMarkerDuration = 0.25f;

		private static readonly Color HitMarkerColor = new Color(0.95f, 0.2f, 0.15f, 0.95f);

		[SerializeField]
		private RoundManager roundManager;

		[SerializeField]
		private GameObject reticleRoot;

		[SerializeField]
		private GameObject hitMarkerRoot;

		private PlayerRole lastRole = (PlayerRole)(-1);

		private bool appliedOnce;

		private float hitMarkerTimer;

		private bool suppressed;

		private bool lastSuppressed;

		public static CrosshairView Instance { get; private set; }

		public static CrosshairView Create(Transform parent, RoundManager roundManager)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "Crosshair");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			RectTransform rectTransform2 = UIFactory.CreateRect(rectTransform, "Reticle");
			rectTransform2.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform2.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform2.pivot = new Vector2(0.5f, 0.5f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(13f, 13f) * 2f;
			float num = 9f;
			CreateBar(rectTransform2, "Top", new Vector2(0f, num), new Vector2(2f, 8f), Color.white);
			CreateBar(rectTransform2, "Bottom", new Vector2(0f, 0f - num), new Vector2(2f, 8f), Color.white);
			CreateBar(rectTransform2, "Left", new Vector2(0f - num, 0f), new Vector2(8f, 2f), Color.white);
			CreateBar(rectTransform2, "Right", new Vector2(num, 0f), new Vector2(8f, 2f), Color.white);
			RectTransform rectTransform3 = UIFactory.CreateRect(rectTransform, "HitMarker");
			rectTransform3.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform3.pivot = new Vector2(0.5f, 0.5f);
			rectTransform3.anchoredPosition = Vector2.zero;
			rectTransform3.sizeDelta = new Vector2(11f, 11f) * 2f;
			CreateBar(rectTransform3, "X1", Vector2.zero, new Vector2(3f, 11f), HitMarkerColor, 45f);
			CreateBar(rectTransform3, "X2", Vector2.zero, new Vector2(3f, 11f), HitMarkerColor, -45f);
			CrosshairView crosshairView = rectTransform.gameObject.AddComponent<CrosshairView>();
			crosshairView.roundManager = roundManager;
			crosshairView.reticleRoot = rectTransform2.gameObject;
			crosshairView.hitMarkerRoot = rectTransform3.gameObject;
			rectTransform2.gameObject.SetActive(value: false);
			rectTransform3.gameObject.SetActive(value: false);
			return crosshairView;
		}

		private static void CreateBar(Transform parent, string name, Vector2 anchoredPosition, Vector2 size, Color color, float rotationDegrees = 0f)
		{
			Image image = UIFactory.CreatePanel(parent, name, color);
			image.raycastTarget = false;
			RectTransform obj = (RectTransform)image.transform;
			obj.anchorMin = new Vector2(0.5f, 0.5f);
			obj.anchorMax = new Vector2(0.5f, 0.5f);
			obj.pivot = new Vector2(0.5f, 0.5f);
			obj.anchoredPosition = anchoredPosition;
			obj.sizeDelta = size;
			obj.localRotation = Quaternion.Euler(0f, 0f, rotationDegrees);
		}

		public void SetRoundManager(RoundManager roundManager)
		{
			this.roundManager = roundManager;
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

		public void ShowHitMarker()
		{
			hitMarkerRoot.SetActive(value: true);
			hitMarkerTimer = 0.25f;
		}

		private void Update()
		{
			if (hitMarkerTimer > 0f)
			{
				hitMarkerTimer -= Time.deltaTime;
				if (hitMarkerTimer <= 0f)
				{
					hitMarkerRoot.SetActive(value: false);
				}
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current as RoundManager;
			}
			GameModeController current = GameModeController.Current;
			bool flag = suppressed || (current != null && !current.IsLocalPlayerParticipating);
			if (roundManager == null)
			{
				if (flag != lastSuppressed)
				{
					reticleRoot.SetActive(!flag);
					lastSuppressed = flag;
				}
				return;
			}
			PlayerRole localRole = roundManager.LocalRole;
			if (!appliedOnce || localRole != lastRole || flag != lastSuppressed)
			{
				reticleRoot.SetActive(localRole == PlayerRole.Hunter && !flag);
				lastRole = localRole;
				lastSuppressed = flag;
				appliedOnce = true;
			}
		}

		public void SetSuppressed(bool value)
		{
			suppressed = value;
		}
	}
}
