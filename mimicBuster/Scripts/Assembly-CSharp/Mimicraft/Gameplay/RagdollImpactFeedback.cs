using System.Collections.Generic;
using Mimicraft.UI;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(PlayerRagdoll))]
	public class RagdollImpactFeedback : MonoBehaviour
	{
		[Header("Ne duyulur")]
		[Tooltip("Duyulmaya değer en yavaş çarpma (m/s). Bunun altındaki her temas yok sayılır - yatan bir vücut kıpırdadıkça zemine sürtünür, ve onların hepsi çarpma değildir.")]
		[SerializeField]
		[Min(0f)]
		private float minSpeed = 2.5f;

		[Tooltip("Sesin ve efektin sonuna vurduğu hız (m/s). Bunun üstündeki her çarpma aynı gelir, yani bu 'en sert' demek - iki katı iki kat gürültü değil.")]
		[SerializeField]
		[Min(0.1f)]
		private float loudSpeed = 9f;

		[Tooltip("İki çarpma arasında geçmesi gereken en kısa süre (saniye). Bu süre içinde gelen çarpmalar kaybolmuyor: en serti bekletilip süre dolunca çalıyor.")]
		[SerializeField]
		[Min(0f)]
		private float minInterval = 0.12f;

		[Tooltip("Hangi katmanlara çarpınca sayılacağı. Kendi kemiklerine çarpması zaten sayılmıyor.")]
		[SerializeField]
		private LayerMask surfaces = -1;

		[Header("Nasıl duyulur")]
		[Tooltip("Ses yüksekliği: en hafif çarpma -> en sert çarpma.")]
		[SerializeField]
		private Vector2 volumeRange = new Vector2(0.25f, 1f);

		[Tooltip("Perde: en hafif çarpma -> en sert çarpma. Sert olanı KALIN olsun diye ikinci değer birinciden küçük - ağırlık hissi çoğunlukla perdededir, yükseklikte değil.")]
		[SerializeField]
		private Vector2 pitchRange = new Vector2(1.15f, 0.85f);

		[Tooltip("Efektin ölçeği: en hafif çarpma -> en sert çarpma. Prefabin kendi ölçeğiyle çarpılır.")]
		[SerializeField]
		private Vector2 effectScaleRange = new Vector2(0.6f, 1.4f);

		[Tooltip("Kapatırsan sadece ses çıkar. Sesi kapatmak için AudioLibrary'deki klip listesini boş bırakman yeterli.")]
		[SerializeField]
		private bool spawnEffect = true;

		private PlayerRagdoll ragdoll;

		private bool wired;

		private float pendingSpeed;

		private Vector3 pendingPoint;

		private Vector3 pendingNormal;

		private float nextAllowedTime;

		private void Awake()
		{
			ragdoll = GetComponent<PlayerRagdoll>();
		}

		private void Update()
		{
			if (wired)
			{
				return;
			}
			IReadOnlyList<Rigidbody> boneBodies = ragdoll.BoneBodies;
			if (boneBodies.Count == 0)
			{
				return;
			}
			wired = true;
			foreach (Rigidbody item in boneBodies)
			{
				if (!(item == null))
				{
					RagdollBoneImpact ragdollBoneImpact = item.gameObject.GetComponent<RagdollBoneImpact>();
					if (ragdollBoneImpact == null)
					{
						ragdollBoneImpact = item.gameObject.AddComponent<RagdollBoneImpact>();
					}
					ragdollBoneImpact.Bind(this);
				}
			}
		}

		internal void ReportBoneImpact(Collision collision)
		{
			if (collision == null || collision.contactCount == 0 || !ragdoll.IsLimp)
			{
				return;
			}
			Collider collider = collision.collider;
			if (!(collider == null) && (surfaces.value & (1 << collider.gameObject.layer)) != 0 && !collider.transform.IsChildOf(base.transform))
			{
				float magnitude = collision.relativeVelocity.magnitude;
				if (!(magnitude < minSpeed) && !(magnitude <= pendingSpeed))
				{
					ContactPoint contact = collision.GetContact(0);
					pendingSpeed = magnitude;
					pendingPoint = contact.point;
					pendingNormal = contact.normal;
				}
			}
		}

		private void LateUpdate()
		{
			if (!(pendingSpeed <= 0f) && !(Time.time < nextAllowedTime))
			{
				float value = pendingSpeed;
				pendingSpeed = 0f;
				nextAllowedTime = Time.time + minInterval;
				float t = Mathf.InverseLerp(minSpeed, Mathf.Max(loudSpeed, minSpeed + 0.01f), value);
				ImpactEffects.PlayAtPoint(AudioLibrary.NextRagdollImpactClip(), pendingPoint, Mathf.Lerp(volumeRange.x, volumeRange.y, t), Mathf.Lerp(pitchRange.x, pitchRange.y, t));
				if (spawnEffect && EffectLibrary.Instance != null)
				{
					EffectLibrary.TrySpawn(EffectLibrary.Instance.RagdollImpactPrefab, pendingPoint, pendingNormal, Mathf.Lerp(effectScaleRange.x, effectScaleRange.y, t));
				}
			}
		}
	}
}
