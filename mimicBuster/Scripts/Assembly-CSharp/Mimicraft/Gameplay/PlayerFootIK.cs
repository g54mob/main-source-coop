using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(Animator))]
	public class PlayerFootIK : MonoBehaviour
	{
		[Header("Zemin")]
		[Tooltip("Ayağın basabileceği katmanlar. Oyuncuların kendi katmanı BURADA OLMAMALI - yoksa karakter kendi kapsülüne basmaya çalışır.")]
		[SerializeField]
		private LayerMask groundLayers = -1;

		[Tooltip("Işının ayak bileğinin ne kadar üstünden başlayacağı, metre. Basamak yüksekliğinden büyük olmalı; yoksa ışın basamağın içinden başlar ve onu görmez.")]
		[SerializeField]
		[Min(0f)]
		private float rayAboveFoot = 0.5f;

		[Tooltip("Işının ayak bileğinin ne kadar altına ineceği, metre. Bu mesafede zemin yoksa o ayak animasyonun bıraktığı yerde kalır.")]
		[SerializeField]
		[Min(0.01f)]
		private float rayBelowFoot = 0.6f;

		[Tooltip("Ayak bileği kemiğinden tabana olan mesafe, metre. Ayaklar zemine gömülüyorsa artır, havada duruyorsa azalt - kalibre edeceğin ilk değer bu.")]
		[SerializeField]
		private float soleOffset = 0.06f;

		[Tooltip("Bu açıdan dik bir yüzeye ayak yerleştirilmez; o ayak animasyondaki yerinde kalır. Duvara yapışan ayakları engelliyor.")]
		[SerializeField]
		[Range(0f, 89f)]
		private float maxGroundAngle = 55f;

		[Tooltip("Ayağın eğimle hizalanırken animasyondaki hâlinden en fazla kaç derece döndürülebileceği. Bir yüzey basmaya değecek kadar dik olabilir ama ayak bileği o kadar bükülmez; sınırı düşürmek bacakların yana açılmasını azaltır. 89 = sınırsız.")]
		[SerializeField]
		[Range(0f, 89f)]
		private float maxFootTilt = 25f;

		[Tooltip("Ayağın animasyondaki yerinden en fazla ne kadar aşağı çekilebileceği, metre. Zeminin çok altına uzanan bir ayak bacağı sonuna kadar açar ve çözücü onu yana savurur. Işının ulaştığı her yere basmak yerine, ulaşamayacağı yere hiç basmıyor.")]
		[SerializeField]
		[Min(0.01f)]
		private float maxFootDrop = 0.3f;

		[Header("Gövde")]
		[Tooltip("Açıkken leğen kemiği, alçakta kalan ayağa doğru indirilir. Kapatırsan basamaklarda bacak menzili yetmez ve ayak yine yere gömülür.")]
		[SerializeField]
		private bool adjustBody = true;

		[Tooltip("Gövdenin inebileceği en fazla mesafe, metre. Büyük değerler karakteri çömelmiş gösterir.")]
		[SerializeField]
		[Min(0f)]
		private float maxBodyDrop = 0.35f;

		[Header("Yumuşatma")]
		[Tooltip("Ayağın hedefine oturma hızı. Büyük = daha yapışkan ama daha titrek.")]
		[SerializeField]
		[Min(0.1f)]
		private float footSmoothing = 14f;

		[Tooltip("Gövdenin inip kalkma hızı.")]
		[SerializeField]
		[Min(0.1f)]
		private float bodySmoothing = 8f;

		[Tooltip("Havadayken IK'nın sönme hızı. Zıplarken ayakların yere yapışmasını engelleyen şey bu.")]
		[SerializeField]
		[Min(0.1f)]
		private float weightFadeSpeed = 10f;

		private Animator animator;

		private CharacterController controller;

		private PlayerRagdoll ragdoll;

		private Vector3 leftPosition;

		private Vector3 rightPosition;

		private Quaternion leftRotation = Quaternion.identity;

		private Quaternion rightRotation = Quaternion.identity;

		private bool leftSeeded;

		private bool rightSeeded;

		private float leftWeight;

		private float rightWeight;

		private float bodyOffset;

		private bool ikPassRan;

		private float aliveSince;

		private bool warnedAboutIkPass;

		private bool ShouldSolve
		{
			get
			{
				if (ragdoll != null && ragdoll.State != RagdollState.None)
				{
					return false;
				}
				if (!(controller == null))
				{
					return controller.isGrounded;
				}
				return true;
			}
		}

		private void Awake()
		{
			animator = GetComponent<Animator>();
			controller = GetComponentInParent<CharacterController>();
			ragdoll = GetComponentInParent<PlayerRagdoll>();
			aliveSince = Time.time;
		}

		private void Update()
		{
			if (!ikPassRan && !warnedAboutIkPass && !(Time.time - aliveSince < 2f))
			{
				warnedAboutIkPass = true;
				if (animator != null && !animator.isHuman)
				{
					Debug.LogWarning("[PlayerFootIK] '" + animator.name + "' humanoid degil - ayak IK'si calismayacak. Bu bilesen sadece Humanoid avatarlarda ise yarar.", this);
				}
				else
				{
					Debug.LogWarning("[PlayerFootIK] OnAnimatorIK hic calismadi - Animator Controller'daki katmanin 'IK Pass' kutusu isaretli mi? O kapaliyken ayak IK'si sessizce hicbir sey yapmaz.", this);
				}
			}
		}

		private void OnAnimatorIK(int layerIndex)
		{
			ikPassRan = true;
			if (!(animator == null) && animator.isHuman)
			{
				bool shouldSolve = ShouldSolve;
				SolveFoot(AvatarIKGoal.LeftFoot, shouldSolve, ref leftPosition, ref leftRotation, ref leftWeight, ref leftSeeded, out var drop);
				SolveFoot(AvatarIKGoal.RightFoot, shouldSolve, ref rightPosition, ref rightRotation, ref rightWeight, ref rightSeeded, out var drop2);
				ApplyBody(shouldSolve, Mathf.Min(drop, drop2));
				Apply(AvatarIKGoal.LeftFoot, leftPosition, leftRotation, leftWeight);
				Apply(AvatarIKGoal.RightFoot, rightPosition, rightRotation, rightWeight);
			}
		}

		private void SolveFoot(AvatarIKGoal goal, bool active, ref Vector3 position, ref Quaternion rotation, ref float weight, ref bool seeded, out float drop)
		{
			drop = 0f;
			Vector3 iKPosition = animator.GetIKPosition(goal);
			Quaternion iKRotation = animator.GetIKRotation(goal);
			if (!seeded)
			{
				position = iKPosition;
				rotation = iKRotation;
				seeded = true;
			}
			Vector3 b = iKPosition;
			Quaternion b2 = iKRotation;
			float target = 0f;
			if (active && Physics.Raycast(new Ray(iKPosition + Vector3.up * rayAboveFoot, Vector3.down), out var hitInfo, rayAboveFoot + rayBelowFoot, groundLayers, QueryTriggerInteraction.Ignore) && Vector3.Angle(hitInfo.normal, Vector3.up) <= maxGroundAngle && hitInfo.point.y >= iKPosition.y - maxFootDrop)
			{
				b = hitInfo.point + Vector3.up * soleOffset;
				b2 = Planted(iKRotation, hitInfo.normal);
				target = 1f;
				drop = Mathf.Min(0f, b.y - iKPosition.y);
			}
			float t = 1f - Mathf.Exp((0f - footSmoothing) * Time.deltaTime);
			position = Vector3.Lerp(position, b, t);
			rotation = Quaternion.Slerp(rotation, b2, t);
			weight = Mathf.MoveTowards(weight, target, weightFadeSpeed * Time.deltaTime);
		}

		private Quaternion Planted(Quaternion animatedRotation, Vector3 normal)
		{
			Vector3 vector = Vector3.ProjectOnPlane(animatedRotation * Vector3.forward, normal);
			if (vector.sqrMagnitude < 0.0001f)
			{
				return animatedRotation;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector.normalized, normal);
			if (!(maxFootTilt >= 89f))
			{
				return Quaternion.RotateTowards(animatedRotation, quaternion, maxFootTilt);
			}
			return quaternion;
		}

		private void ApplyBody(bool active, float lowestDrop)
		{
			float b = ((active && adjustBody) ? Mathf.Max(lowestDrop, 0f - maxBodyDrop) : 0f);
			bodyOffset = Mathf.Lerp(bodyOffset, b, 1f - Mathf.Exp((0f - bodySmoothing) * Time.deltaTime));
			if (!Mathf.Approximately(bodyOffset, 0f))
			{
				animator.bodyPosition += Vector3.up * bodyOffset;
			}
		}

		private void Apply(AvatarIKGoal goal, Vector3 position, Quaternion rotation, float weight)
		{
			animator.SetIKPositionWeight(goal, weight);
			animator.SetIKRotationWeight(goal, weight);
			if (!(weight <= 0f))
			{
				animator.SetIKPosition(goal, position);
				animator.SetIKRotation(goal, rotation);
			}
		}
	}
}
