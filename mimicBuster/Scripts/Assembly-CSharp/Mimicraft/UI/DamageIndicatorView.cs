using System;
using System.Collections.Generic;
using Mimicraft.Gameplay;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class DamageIndicatorView : MonoBehaviour
	{
		private class Indicator
		{
			public RectTransform Rect;

			public CanvasGroup Group;

			public Vector3 Source;

			public float Age;
		}

		[Tooltip("Tek bir yön göstergesi - ekranın ORTASINA bağlı (anchor ve pivot orta), YUKARI bakan bir ok ya da yay. Sahnede KAPALI bırak; her isabette bundan kopya çıkarılır ve hasarın geldiği yöne döndürülür.")]
		[SerializeField]
		private RectTransform indicatorTemplate;

		[Tooltip("Göstergelerin ekran merkezinden uzaklığı, piksel.")]
		[SerializeField]
		[Min(0f)]
		private float radius = 160f;

		[Tooltip("Bir göstergenin ekranda kalma süresi, saniye. İkinci yarısında söner.")]
		[SerializeField]
		[Min(0.1f)]
		private float lifetime = 1.5f;

		[Tooltip("Aynı anda en fazla kaç gösterge.")]
		[SerializeField]
		[Min(1f)]
		private int maxIndicators = 6;

		[Tooltip("Bu açıdan yakın gelen yeni bir isabet mevcut göstergeyi tazeler, yenisini açmaz.")]
		[SerializeField]
		[Range(0f, 90f)]
		private float mergeAngle = 25f;

		[Header("Kırmızılaşma")]
		[Tooltip("Ekran kenarlarını kızartan tam ekran görsel - bir vignette sprite'ı. İsteğe bağlı. Rengini sen ver; alfası buradan sürülür, sahnede 0 bırakabilirsin.")]
		[SerializeField]
		private Graphic vignette;

		[Tooltip("İsabet anında vignette'in alfası.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float hitAlpha = 0.45f;

		[Tooltip("İsabet kızarmasının sönme süresi, saniye.")]
		[SerializeField]
		[Min(0.05f)]
		private float hitFadeSeconds = 0.6f;

		[Tooltip("Can bu değerin altındayken vignette sürekli kırmızı kalır ve can azaldıkça koyulaşır. 0 = kapalı.")]
		[SerializeField]
		[Range(0f, 100f)]
		private int lowHealthThreshold = 40;

		[Tooltip("Düşük canda vignette'in çıkabileceği en yüksek alfa.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float lowHealthMaxAlpha = 0.35f;

		private readonly List<Indicator> active = new List<Indicator>();

		private readonly Stack<Indicator> pool = new Stack<Indicator>();

		private float hitPulse;

		private PlayerHealth localHealth;

		private PlayerCameraRig localRig;

		private void OnEnable()
		{
			PlayerHealth.LocalDamageFrom += OnDamageFrom;
			if (indicatorTemplate != null && indicatorTemplate.gameObject.activeSelf)
			{
				indicatorTemplate.gameObject.SetActive(value: false);
			}
			SetVignetteAlpha(0f);
		}

		private void OnDisable()
		{
			PlayerHealth.LocalDamageFrom -= OnDamageFrom;
			foreach (Indicator item in active)
			{
				Release(item);
			}
			active.Clear();
			hitPulse = 0f;
			SetVignetteAlpha(0f);
		}

		private void OnDamageFrom(Vector3 source)
		{
			hitPulse = 1f;
			if (indicatorTemplate == null || !ResolveLocal())
			{
				return;
			}
			float target = AngleTo(source);
			foreach (Indicator item in active)
			{
				if (!(Mathf.Abs(Mathf.DeltaAngle(AngleTo(item.Source), target)) > mergeAngle))
				{
					item.Source = source;
					item.Age = 0f;
					return;
				}
			}
			if (active.Count >= maxIndicators)
			{
				Release(active[0]);
				active.RemoveAt(0);
			}
			Indicator indicator = ((pool.Count > 0) ? pool.Pop() : Create());
			indicator.Source = source;
			indicator.Age = 0f;
			indicator.Rect.gameObject.SetActive(value: true);
			active.Add(indicator);
			Place(indicator);
		}

		private void Update()
		{
			ResolveLocal();
			float deltaTime = Time.deltaTime;
			for (int num = active.Count - 1; num >= 0; num--)
			{
				Indicator indicator = active[num];
				indicator.Age += deltaTime;
				if (indicator.Age >= lifetime)
				{
					Release(indicator);
					active.RemoveAt(num);
				}
				else
				{
					Place(indicator);
				}
			}
			hitPulse = Mathf.MoveTowards(hitPulse, 0f, deltaTime / hitFadeSeconds);
			float b = 0f;
			if (localHealth != null && lowHealthThreshold > 0)
			{
				int value = localHealth.Health.Value;
				if (value > 0 && value < lowHealthThreshold)
				{
					b = (1f - (float)value / (float)lowHealthThreshold) * lowHealthMaxAlpha;
				}
			}
			SetVignetteAlpha(Mathf.Max(hitPulse * hitAlpha, b));
		}

		private void Place(Indicator indicator)
		{
			float num = AngleTo(indicator.Source);
			float f = num * (MathF.PI / 180f);
			indicator.Rect.anchoredPosition = new Vector2(Mathf.Sin(f), Mathf.Cos(f)) * radius;
			indicator.Rect.localRotation = Quaternion.Euler(0f, 0f, 0f - num);
			float num2 = lifetime * 0.5f;
			indicator.Group.alpha = ((indicator.Age <= num2) ? 1f : Mathf.Clamp01(1f - (indicator.Age - num2) / num2));
		}

		private float AngleTo(Vector3 source)
		{
			Transform transform = ((localRig != null) ? localRig.AimCameraTransform : null);
			if (transform == null || localHealth == null)
			{
				return 0f;
			}
			Vector3 vector = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
			Vector3 to = Vector3.ProjectOnPlane(source - localHealth.transform.position, Vector3.up);
			if (vector.sqrMagnitude < 0.0001f || to.sqrMagnitude < 0.0001f)
			{
				return 0f;
			}
			return Vector3.SignedAngle(vector, to, Vector3.up);
		}

		private Indicator Create()
		{
			RectTransform rectTransform = UnityEngine.Object.Instantiate(indicatorTemplate, indicatorTemplate.parent);
			rectTransform.name = "DamageIndicator";
			CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
			if (canvasGroup == null)
			{
				canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>();
			}
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
			return new Indicator
			{
				Rect = rectTransform,
				Group = canvasGroup
			};
		}

		private void Release(Indicator indicator)
		{
			if (indicator.Rect != null)
			{
				indicator.Rect.gameObject.SetActive(value: false);
			}
			pool.Push(indicator);
		}

		private bool ResolveLocal()
		{
			if (localHealth != null && localRig != null)
			{
				return true;
			}
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || singleton.LocalClient == null || singleton.LocalClient.PlayerObject == null)
			{
				return false;
			}
			localHealth = singleton.LocalClient.PlayerObject.GetComponent<PlayerHealth>();
			localRig = singleton.LocalClient.PlayerObject.GetComponent<PlayerCameraRig>();
			if (localHealth != null)
			{
				return localRig != null;
			}
			return false;
		}

		private void SetVignetteAlpha(float alpha)
		{
			if (!(vignette == null))
			{
				Color color = vignette.color;
				if (!Mathf.Approximately(color.a, alpha))
				{
					color.a = alpha;
					vignette.color = color;
				}
			}
		}
	}
}
