using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class ScopeView : MonoBehaviour
	{
		[Tooltip("Dürbün görseli. Tam ekran kaplaması beklenir - ortası şeffaf, kenarları kapalı. Boş bırakılabilir; o zaman sadece kamera yakınlaşır ve ekranda bir şey belirmez.")]
		[SerializeField]
		private GameObject overlay;

		[Tooltip("Dürbün açıkken gizlenecek diğer HUD parçaları - nişangah gibi. Boş bırakılabilir.")]
		[SerializeField]
		private GameObject[] hideWhileScoped;

		public static ScopeView Instance { get; private set; }

		public bool IsScoped { get; private set; }

		public static ScopeView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "Scope");
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0f, 0f, 0f, 0f);
			image.raycastTarget = false;
			image.preserveAspect = true;
			ScopeView scopeView = rectTransform.gameObject.AddComponent<ScopeView>();
			scopeView.overlay = rectTransform.GetChild(0).gameObject;
			return scopeView;
		}

		private void Awake()
		{
			Instance = this;
			base.gameObject.SetActive(value: false);
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void Show()
		{
			IsScoped = true;
			Apply(scoped: true);
		}

		public void Hide()
		{
			IsScoped = false;
			Apply(scoped: false);
		}

		private void Apply(bool scoped)
		{
			if (base.gameObject.activeSelf != scoped)
			{
				base.gameObject.SetActive(scoped);
			}
			if (CrosshairView.Instance != null)
			{
				CrosshairView.Instance.SetSuppressed(scoped);
			}
			if (hideWhileScoped == null)
			{
				return;
			}
			bool flag = !scoped;
			GameObject[] array = hideWhileScoped;
			foreach (GameObject gameObject in array)
			{
				if (gameObject != null && gameObject.activeSelf != flag)
				{
					gameObject.SetActive(flag);
				}
			}
		}
	}
}
