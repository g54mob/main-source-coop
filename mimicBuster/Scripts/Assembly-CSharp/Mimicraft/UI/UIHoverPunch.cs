using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class UIHoverPunch : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		[Serializable]
		public class PunchSettings
		{
			[Tooltip("Bu vuruş oynasın mı.")]
			public bool enabled = true;

			[Tooltip("Ölçek vuruşunun büyüklüğü. 0.1 = yüzde on büyüyüp geri gelir. 0 = ölçek vuruşu yok.")]
			[Min(0f)]
			public float scale = 0.12f;

			[Tooltip("Konum vuruşu, piksel (anchored position). Sıfır = konum vuruşu yok.\n\nRectTransform gerektirir. Ölçekle birlikte de kullanılabilir.")]
			public Vector2 anchor = Vector2.zero;

			[Tooltip("Vuruşun süresi, saniye.")]
			[Min(0.01f)]
			public float duration = 0.25f;

			[Tooltip("Kaç kez gidip gelsin. Yüksek değer titreşim, düşük değer tek bir nefes gibi durur.")]
			[Min(1f)]
			public int vibrato = 8;

			[Tooltip("Vuruşun ana eksenin dışına ne kadar taşacağı (0-1).")]
			[Range(0f, 1f)]
			public float elasticity = 0.6f;
		}

		[Tooltip("Ölçeklenecek/oynatılacak obje. Boş bırakılırsa bu objenin kendisi - normal durum. Yazıyı değil de altındaki zemini oynatmak istersen burayı doldur.")]
		[SerializeField]
		private Transform target;

		[Tooltip("Fare üzerine gelince.")]
		[SerializeField]
		private PunchSettings hover = new PunchSettings();

		[Tooltip("Tıklanınca. Üzerine gelmekten biraz daha güçlü olması iyi durur - basma, geçmekten daha büyük bir olay.")]
		[SerializeField]
		private PunchSettings click = new PunchSettings
		{
			scale = 0.22f,
			duration = 0.28f
		};

		[Tooltip("Yalnızca basılabilir durumdaki butonlarda oynasın mı. Kapalı bir butonun canlanması basılabileceğini söyler, ki söyleyemez. Buton yoksa bu ayarın hükmü yok.")]
		[SerializeField]
		private bool onlyWhenInteractable = true;

		private Selectable selectable;

		private Tween scalePunch;

		private Tween anchorPunch;

		private Vector3 authoredScale;

		private Vector2 authoredAnchor;

		private bool hasAuthoredPose;

		private Transform Target
		{
			get
			{
				if (!(target != null))
				{
					return base.transform;
				}
				return target;
			}
		}

		private void Awake()
		{
			selectable = GetComponent<Selectable>();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			PlayHover();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Stop();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			PlayClick();
		}

		private void OnDisable()
		{
			Stop();
		}

		public void PlayHover()
		{
			Play(hover);
		}

		public void PlayClick()
		{
			Play(click);
		}

		private void Play(PunchSettings settings)
		{
			if (settings == null || !settings.enabled || (onlyWhenInteractable && selectable != null && !selectable.IsInteractable()))
			{
				return;
			}
			Transform transform = Target;
			if (!(transform == null))
			{
				RectTransform rectTransform = transform as RectTransform;
				if (!hasAuthoredPose)
				{
					hasAuthoredPose = true;
					authoredScale = transform.localScale;
					authoredAnchor = ((rectTransform != null) ? rectTransform.anchoredPosition : Vector2.zero);
				}
				Restore();
				if (settings.scale > 0f)
				{
					scalePunch = transform.DOPunchScale(Vector3.one * settings.scale, settings.duration, settings.vibrato, settings.elasticity).SetEase(Ease.OutCubic).SetUpdate(isIndependentUpdate: true)
						.SetLink(transform.gameObject);
				}
				if (!(rectTransform == null) && !(settings.anchor == Vector2.zero))
				{
					anchorPunch = rectTransform.DOPunchAnchorPos(settings.anchor, settings.duration, settings.vibrato, settings.elasticity).SetEase(Ease.OutCubic).SetUpdate(isIndependentUpdate: true)
						.SetLink(rectTransform.gameObject);
				}
			}
		}

		public void Stop()
		{
			Restore();
		}

		private void Restore()
		{
			scalePunch?.Kill();
			anchorPunch?.Kill();
			scalePunch = null;
			anchorPunch = null;
			if (!hasAuthoredPose)
			{
				return;
			}
			Transform transform = Target;
			if (!(transform == null))
			{
				transform.localScale = authoredScale;
				if (transform is RectTransform rectTransform)
				{
					rectTransform.anchoredPosition = authoredAnchor;
				}
			}
		}
	}
}
