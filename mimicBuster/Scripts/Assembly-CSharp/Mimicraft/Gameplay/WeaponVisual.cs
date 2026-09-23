using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class WeaponVisual : MonoBehaviour
	{
		[Tooltip("Karakterin SOL elinin tutacağı nokta. Boşsa sol el serbest kalır.")]
		[SerializeField]
		private Transform leftGrip;

		[Tooltip("Karakterin SAĞ elinin tutacağı nokta. Boşsa sağ el serbest kalır.")]
		[SerializeField]
		private Transform rightGrip;

		[Tooltip("Namlu ucu - ateş efekti buradan çıkar. Boşsa modelin kendi orijini kullanılır.")]
		[SerializeField]
		private Transform muzzle;

		[Tooltip("Ateş edilince oynatılacak efekt objesi, modelin İÇİNDE. Boş bırakılabilir; o zaman WeaponSway'in prosedürel ışığı devreye girer.")]
		[SerializeField]
		private GameObject muzzleFlash;

		[Tooltip("Efektin ne kadar süre açık kalacağı. Particle System'ler bunu kendileri bitirir; bu süre asıl olarak ışık gibi kendiliğinden sönmeyen şeyler için.")]
		[SerializeField]
		[Min(0.01f)]
		private float muzzleFlashLifetime = 0.06f;

		[Header("Birinci şahıs kol IK'leri")]
		[Tooltip("SOL kolu Left Grip'e oturtan çözücü - birinci şahıs modelinin içindeki kolun omuz/dirsek/el kemiklerine bağlı. Silah kuşanılınca Target'ı Left Grip yapılıp bir kez hesaplanır. Üçüncü şahıs prefabinde boş bırakılır: oradaki eller rig'in kendi IK'siyle sürülüyor.")]
		[SerializeField]
		private TwoBoneIkSolver leftArmSolver;

		[Tooltip("Sağ kol için aynısı, Right Grip'e.")]
		[SerializeField]
		private TwoBoneIkSolver rightArmSolver;

		[Header("Nişan alma (ADS)")]
		[Tooltip("Nişan alırken taşınan transform - view model'in WeaponPivot'u. Boş bırakılırsa bu objenin hemen altında \"WeaponPivot\" adlı çocuk aranır. Üçüncü şahıs prefabinde boş kalır; orada nişan alma diye bir şey yok.")]
		[SerializeField]
		private Transform aimPivot;

		[Tooltip("YALNIZCA voxel modeli olmayan bir silahta: nişan alırken WeaponPivot'un gideceği yerel pozisyon (X, Y ve Z). Modeli olan silahlarda ADS Pos silahın üzerindeki bir noktadır ve o nokta ekranın tam ortasına getirilir - bu alan kullanılmaz. Silah özelleştirme ekranındaki \"ADS Pos\" tutamacı bunu skin başına değiştirir; burası hiç ayarlanmamış bir silahın varsayılanı. (0, 0) silahı tam kamera eksenine getirir - nişangâh namlunun üstündeyse Y'yi o kadar eksiye çek.")]
		[SerializeField]
		private Vector3 adsPivotPosition;

		[Tooltip("Nişan alırken hangi pozisyon çözülürse çözülsün (skin'inki ya da yukarıdaki varsayılan) üstüne eklenen sabit kayma. Bench'te doğru görünen bir ADS Pos'un bu silahta yüksek kalması gibi prefaba özgü farkları burada düzelt - skin'e dokunmadan.")]
		[SerializeField]
		private Vector3 adsOffset;

		private Vector3 aimPivotRest;

		private Quaternion aimPivotRestRotation = Quaternion.identity;

		private Vector3 aimPivotRestScale = Vector3.one;

		private Vector3 aimPointInPivot;

		private Vector3 aimReferenceInPivot;

		private bool hasAimPoint;

		private bool armsSolvedOnce;

		private float flashOffAt;

		public Transform LeftGrip => leftGrip;

		public Transform RightGrip => rightGrip;

		public Transform Muzzle
		{
			get
			{
				if (!(muzzle != null))
				{
					return base.transform;
				}
				return muzzle;
			}
		}

		public Transform AimPivot
		{
			get
			{
				if (aimPivot == null)
				{
					aimPivot = base.transform.Find("WeaponPivot");
				}
				return aimPivot;
			}
		}

		public Vector3 AdsPivotDelta => AdsPivotDeltaFrom(aimPivotRest, aimPivotRestRotation, aimPivotRestScale);

		public Vector3 AuthoredAdsPivotPosition => adsPivotPosition;

		public string AdsReport => "aimPoint(pivot)=" + (hasAimPoint ? aimPointInPivot.ToString("F3") : "YOK - Ads Pivot Position kullaniliyor") + " referans=" + aimReferenceInPivot.ToString("F3") + " pivotRest=" + aimPivotRest.ToString("F3") + " adsOffset=" + adsOffset.ToString("F3") + " delta=" + AdsPivotDelta.ToString("F3");

		public Vector3 AimPivotRestPosition => aimPivotRest;

		public bool HasMuzzleFlash => muzzleFlash != null;

		public Transform MuzzleFlashTransform
		{
			get
			{
				if (!(muzzleFlash != null))
				{
					return null;
				}
				return muzzleFlash.transform;
			}
		}

		public float MuzzleFlashLifetime => muzzleFlashLifetime;

		public Vector3 AdsPivotDeltaFrom(Vector3 basePosition, Quaternion rotation, Vector3 scale)
		{
			if (!hasAimPoint)
			{
				return adsPivotPosition + adsOffset - basePosition;
			}
			Vector3 vector = basePosition + rotation * Vector3.Scale(scale, aimPointInPivot);
			float num = (aimPointInPivot.z - aimReferenceInPivot.z) * scale.z;
			return new Vector3(0f - vector.x, 0f - vector.y, 0f - num) + adsOffset;
		}

		public bool TryGetAdsAimPointWorld(out Vector3 world)
		{
			Transform transform = AimPivot;
			bool flag = hasAimPoint && transform != null;
			world = (flag ? transform.TransformPoint(aimPointInPivot) : Vector3.zero);
			return flag;
		}

		public void SetAdsAimPoint(Vector3? pointInPivot, Vector3 referenceInPivot)
		{
			hasAimPoint = pointInPivot.HasValue;
			aimPointInPivot = pointInPivot ?? Vector3.zero;
			aimReferenceInPivot = (hasAimPoint ? referenceInPivot : Vector3.zero);
		}

		public void OverridePoints(Transform left, Transform right, Transform muzzlePoint)
		{
			if (left != null)
			{
				leftGrip = left;
			}
			if (right != null)
			{
				rightGrip = right;
			}
			if (muzzlePoint != null)
			{
				muzzle = muzzlePoint;
			}
			SolveArms();
		}

		public void SolveArms()
		{
			SolveArm(leftArmSolver, leftGrip);
			SolveArm(rightArmSolver, rightGrip);
			armsSolvedOnce = true;
		}

		private void LateUpdate()
		{
			if (armsSolvedOnce && (leftArmSolver != null || rightArmSolver != null))
			{
				SolveArms();
			}
		}

		private static void SolveArm(TwoBoneIkSolver solver, Transform grip)
		{
			if (!(solver == null) && !(grip == null))
			{
				solver.Target = grip;
				solver.Calculate();
			}
		}

		public void StopMuzzleFlash()
		{
			flashOffAt = 0f;
			if (muzzleFlash != null && muzzleFlash.activeSelf)
			{
				muzzleFlash.SetActive(value: false);
			}
		}

		public void SimulateFlashWithWeapon()
		{
			if (!(muzzleFlash == null))
			{
				ParticleSystem[] componentsInChildren = muzzleFlash.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					ParticleSystem.MainModule main = componentsInChildren[i].main;
					main.simulationSpace = ParticleSystemSimulationSpace.Local;
				}
			}
		}

		public void PlayMuzzleFlash()
		{
			if (!(muzzleFlash == null))
			{
				if (!muzzleFlash.activeSelf)
				{
					muzzleFlash.SetActive(value: true);
				}
				ParticleSystem[] componentsInChildren = muzzleFlash.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
				foreach (ParticleSystem obj in componentsInChildren)
				{
					obj.Clear(withChildren: true);
					obj.Play(withChildren: true);
				}
				flashOffAt = Time.time + muzzleFlashLifetime;
			}
		}

		private void Awake()
		{
			Transform transform = AimPivot;
			aimPivotRest = ((transform != null) ? transform.localPosition : Vector3.zero);
			aimPivotRestRotation = ((transform != null) ? transform.localRotation : Quaternion.identity);
			aimPivotRestScale = ((transform != null) ? transform.localScale : Vector3.one);
			if (muzzleFlash != null)
			{
				muzzleFlash.SetActive(value: false);
			}
		}

		private void Update()
		{
			if (!(muzzleFlash == null) && !(flashOffAt <= 0f) && !(Time.time < flashOffAt))
			{
				flashOffAt = 0f;
				muzzleFlash.SetActive(value: false);
			}
		}
	}
}
