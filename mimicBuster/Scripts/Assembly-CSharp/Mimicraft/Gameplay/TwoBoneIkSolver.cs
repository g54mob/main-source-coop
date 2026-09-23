using UnityEngine;

namespace Mimicraft.Gameplay
{
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public class TwoBoneIkSolver : MonoBehaviour
	{
		[Tooltip("Zincirin ilk kemiği - omuz/kalça. Mid'in atası olmalı.")]
		[SerializeField]
		private Transform root;

		[Tooltip("Ortadaki eklem - dirsek/diz. Root'un çocuğu, Tip'in atası olmalı.")]
		[SerializeField]
		private Transform mid;

		[Tooltip("Zincirin ucu - el/ayak. Target'a oturtulacak olan bu.")]
		[SerializeField]
		private Transform tip;

		[Tooltip("Tip'in gitmesi istenen yer. Silah prefablerinde kavrama noktası.")]
		[SerializeField]
		private Transform target;

		[Tooltip("Dirseğin/dizin hangi yöne bakacağı. Boş bırakılırsa bükülme yönü kemiklerin şu anki duruşundan ne çıkıyorsa o kalır - kol düz durmuyorsa bu genelde makul bir sonuç verir, ama tamamen düz bir kolda bükülme yönü belirsizdir ve bir hint şart olur.")]
		[SerializeField]
		private Transform hint;

		[Tooltip("Tip, Target'ın rotasyonunu da alsın mı? Bir el için genelde istenen budur: kavrama noktası hem elin nerede olacağını hem de nasıl duracağını söyler. Kapatılırsa Tip sadece konuma gider, açısı kolun bükülmesinden ne geliyorsa o olur.")]
		[SerializeField]
		private bool matchTargetRotation = true;

		[Header("Ne zaman çözülsün")]
		[Tooltip("Editörde her karede çözer. Sahnede Target'ı sürüklerken kolun anında takip etmesini sağlayan şey bu - kapatılırsa çözüm sadece Inspector'da bir değer değişince ya da Calculate() elle çağrılınca yapılır.")]
		[SerializeField]
		private bool solveInEditMode = true;

		[Tooltip("Oyun çalışırken de her karede çözer. Varsayılan olarak KAPALI: bu bileşen poz kaydetmek için var, çalışma zamanında bir animasyonun üstüne yazmak için değil.")]
		[SerializeField]
		private bool solveInPlayMode;

		private const float Epsilon = 1E-06f;

		public Transform Root
		{
			get
			{
				return root;
			}
			set
			{
				root = value;
			}
		}

		public Transform Mid
		{
			get
			{
				return mid;
			}
			set
			{
				mid = value;
			}
		}

		public Transform Tip
		{
			get
			{
				return tip;
			}
			set
			{
				tip = value;
			}
		}

		public Transform Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public Transform Hint
		{
			get
			{
				return hint;
			}
			set
			{
				hint = value;
			}
		}

		public void Calculate()
		{
			if (root == null || mid == null || tip == null || target == null)
			{
				return;
			}
			Vector3 position = root.position;
			Vector3 position2 = mid.position;
			Vector3 position3 = tip.position;
			Vector3 position4 = target.position;
			Vector3 lhs = position2 - position;
			Vector3 rhs = position3 - position2;
			Vector3 vector = position3 - position;
			Vector3 vector2 = position4 - position;
			float magnitude = lhs.magnitude;
			float magnitude2 = rhs.magnitude;
			if (magnitude < 1E-06f || magnitude2 < 1E-06f)
			{
				return;
			}
			float magnitude3 = vector.magnitude;
			float magnitude4 = vector2.magnitude;
			float num = TriangleAngle(magnitude3, magnitude, magnitude2);
			float num2 = TriangleAngle(magnitude4, magnitude, magnitude2);
			Vector3 vector3 = Vector3.Cross(lhs, rhs);
			if (vector3.sqrMagnitude < 1E-06f)
			{
				vector3 = Vector3.Cross(lhs, Vector3.up);
				if (vector3.sqrMagnitude < 1E-06f)
				{
					vector3 = Vector3.Cross(lhs, Vector3.right);
				}
				if (vector3.sqrMagnitude < 1E-06f)
				{
					return;
				}
			}
			vector3 = vector3.normalized;
			mid.rotation = Quaternion.AngleAxis((num - num2) * 57.29578f, vector3) * mid.rotation;
			vector = tip.position - position;
			if (vector.sqrMagnitude > 1E-06f && vector2.sqrMagnitude > 1E-06f)
			{
				root.rotation = Quaternion.FromToRotation(vector, vector2) * root.rotation;
			}
			if (hint != null)
			{
				TwistTowardsHint(position, vector2, magnitude + magnitude2);
			}
			if (matchTargetRotation)
			{
				tip.rotation = target.rotation;
			}
		}

		private void TwistTowardsHint(Vector3 rootPosition, Vector3 rootToTarget, float reach)
		{
			if (!(rootToTarget.sqrMagnitude < 1E-06f))
			{
				Vector3 vector = mid.position - rootPosition;
				Vector3 vector2 = hint.position - rootPosition;
				Vector3 fromDirection = Vector3.ProjectOnPlane(vector, rootToTarget);
				Vector3 toDirection = Vector3.ProjectOnPlane(vector2, rootToTarget);
				float num = reach * reach * 0.001f;
				if (!(fromDirection.sqrMagnitude < num) && !(toDirection.sqrMagnitude < 1E-06f))
				{
					root.rotation = Quaternion.FromToRotation(fromDirection, toDirection) * root.rotation;
				}
			}
		}

		private static float TriangleAngle(float opposite, float sideA, float sideB)
		{
			return Mathf.Acos(Mathf.Clamp((sideA * sideA + sideB * sideB - opposite * opposite) / (2f * sideA * sideB), -1f, 1f));
		}

		private void LateUpdate()
		{
			if (Application.isPlaying ? solveInPlayMode : solveInEditMode)
			{
				Solve();
			}
		}

		private void OnValidate()
		{
		}

		private void Solve()
		{
			Calculate();
		}

		private void OnDrawGizmosSelected()
		{
			if (!(root == null) && !(mid == null) && !(tip == null))
			{
				Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.9f);
				Gizmos.DrawLine(root.position, mid.position);
				Gizmos.DrawLine(mid.position, tip.position);
				if (target != null)
				{
					Gizmos.color = new Color(0.2f, 1f, 0.4f, 0.9f);
					Gizmos.DrawLine(tip.position, target.position);
				}
				if (hint != null)
				{
					Gizmos.color = new Color(1f, 0.8f, 0.2f, 0.7f);
					Gizmos.DrawLine(mid.position, hint.position);
				}
			}
		}
	}
}
