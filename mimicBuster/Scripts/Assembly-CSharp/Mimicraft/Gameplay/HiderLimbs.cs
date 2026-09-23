using System;
using System.Collections.Generic;
using Mimicraft.VoxelEditor;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class HiderLimbs : MonoBehaviour
	{
		[Serializable]
		public class LimbTuning
		{
			[Tooltip("Bacak köklerinin gövdenin altından ne kadar dışarıda durduğu (metre).")]
			[Min(0f)]
			public float legReach = 0.05f;

			[Tooltip("El köklerinin gövdenin yanından ne kadar dışarıda durduğu (metre).")]
			[Min(0f)]
			public float handReach = 0.05f;

			[Tooltip("Bacakların gövde genişliğinin ne kadar içinde durduğu. 0.5 = tam kenarda.")]
			[Range(0f, 0.5f)]
			public float legSpread = 0.28f;

			[Tooltip("Koşarken modelin ne kadar yukarı kalktığı (metre).")]
			[Min(0f)]
			public float runLift = 1f;
		}

		[Header("Uzuvlar (prefab içinde)")]
		[Tooltip("Gövdenin altından çıkan sol bacak.")]
		[SerializeField]
		private Transform legLeft;

		[SerializeField]
		private Transform legRight;

		[Tooltip("Gövdenin en solundan çıkan el.")]
		[SerializeField]
		private Transform handLeft;

		[SerializeField]
		private Transform handRight;

		[Header("Uzanma")]
		[Tooltip("Bacakların gövdenin altına ne kadar sarktığı (metre). Oyundaki gövde 0.05: rig'in kendi uzuv boyu zaten var, bu yalnızca kökün gövdeden ne kadar dışarıda durduğu.")]
		[SerializeField]
		[Min(0f)]
		private float legReach = 0.05f;

		[Tooltip("Ellerin yanlara ne kadar uzandığı (metre). Oyundaki gövde 0.05.")]
		[SerializeField]
		[Min(0f)]
		private float handReach = 0.05f;

		[Tooltip("Bacakların gövde genişliğinin ne kadar içinde durduğu. 0.5 = tam kenarda, 0 = ortada.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float legSpread = 0.28f;

		[Tooltip("Koşarken modelin ne kadar yukarı kalktığı (metre). Leg Reach'ten BAĞIMSIZ - ikisini eşit tutmak ayakları tam zemine oturtur; küçük tutmak ayakları biraz gömer, büyük tutmak modeli havalandırır. Hangisi daha komik duruyorsa.")]
		[SerializeField]
		[Min(0f)]
		private float runLift = 1f;

		[Tooltip("Çıkma/geri çekilme süresi (saniye).")]
		[SerializeField]
		[Min(0.01f)]
		private float extendSeconds = 0.15f;

		[Header("Yüzeye oturtma")]
		[Tooltip("Açıkken uzuvlar gövdenin sınır kutusuna değil VOXEL YÜZEYİNE takılır: bacaklar kendi hizalarındaki en alçak voxel sütununun altından, eller gövde ortası hizasındaki en dıştaki voxellerin yanından çıkar. Oyulmuş, kemerli, bacaklı modellerde uzuvlar boşluğa asılı kalmaz. Kapalıyken eski kutu yerleşimi.")]
		[SerializeField]
		private bool snapToSurface = true;

		[Tooltip("Bir bacağın kendi hizasında sütun ararken bakacağı yarıçap - gövde genişliğinin oranı. Bu yarıçapta hiç voxel yoksa en yakın sütun alınır.")]
		[SerializeField]
		[Range(0.05f, 0.5f)]
		private float legSearchRadius = 0.2f;

		[Tooltip("İki bacağın kökleri arasındaki yükseklik farkı gövde yüksekliğinin bu oranını aşarsa biri havada kalmış demektir: iki bacak da ALÇAK olanın altında yan yana toplanır, model de o tarafa yaslanır. Altından ikiye bölünüp bir yarısı yukarı çekilmiş bir gövde için.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float legHeightTolerance = 0.25f;

		[Tooltip("Bacaklar tek tarafa toplandığında aralarındaki mesafe, metre.")]
		[SerializeField]
		[Min(0f)]
		private float legPairGap = 0.12f;

		[Tooltip("Ellerin gövdenin dışında hangi yükseklik/derinlik bandında yer arayacağı - gövde yüksekliğinin ve derinliğinin oranı, ortadan iki yana.")]
		[SerializeField]
		[Range(0.05f, 0.5f)]
		private float handBand = 0.25f;

		[Tooltip("Gövdenin kütle merkezi bacakların üstünde değilse modelin bacaklara doğru YASLANMA sınırı, derece. Bacaklar gövdenin arkasından çıkıyorsa model geriye yaslanır, öndense öne. 0 = yaslanma yok.")]
		[SerializeField]
		[Range(0f, 30f)]
		private float leanDegreesMax = 10f;

		[Tooltip("Kütle merkezinin bacaklardan sapmasının ne kadarı açıya çevrilir. 1 = merkez tam bacakların üstüne gelene kadar yaslanır; daha azı yalnızca ima eder.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float leanFactor = 0.6f;

		[Header("Koşu animasyonu")]
		[Tooltip("Rig'i taşıyan obje (ModelerLimbs). Koşarken açılır, dururken kapanır.")]
		[SerializeField]
		private GameObject limbRig;

		[Tooltip("Left empty, limbRig üzerinde aranır.")]
		[SerializeField]
		private Animator limbAnimator;

		[Tooltip("Duvara tutunurken açılan Animator bool parametresinin adı. Parametreyi ve ona bağlı state'i Animator'de sen kuruyorsun; bu bileşen sadece doğru anda açıp kapatıyor.")]
		[SerializeField]
		private string wallClimbParameter = "WallClimb";

		[Tooltip("Modeler.controller içindeki durumun adı. Her koşu başlangıcında baştan oynatılır.")]
		[SerializeField]
		private string runStateName = "silly running";

		[Header("IK")]
		[Tooltip("Rig'i gövde boyutuyla birlikte ölçekler. TwoBoneIK yalnızca uzuv boyu kadar uzanabilir, bu yüzden büyük bir modelde sabit boyutlu bir karakterin kolları gövdenin kenarına yetişemez ve sonuna kadar gerilmiş dururlar. Rig'i modelle birlikte büyütmek bunu kaynağında çözer. Kapalıyken rig prefabdaki ölçeğinde kalır.")]
		[SerializeField]
		private bool scaleRigToBody;

		private PlayerMovement movement;

		private ILimbBody voxelBody;

		private Bounds restBounds;

		private bool hasRestBounds;

		private int appliedBodyVersion = -1;

		private float extend;

		private bool rigPlaying;

		private int runStateHash;

		private int wallClimbHash;

		private const float UprightForLift = 0.5f;

		private const float ClimbingSpeedThreshold = 0.3f;

		private const float ClimbSpeedBlendSeconds = 0.1f;

		private Vector3 lastPosition;

		private bool hasLastPosition;

		private float appliedAnimatorSpeed = 1f;

		private Vector3 authoredRigScale = Vector3.one;

		private readonly List<Vector3> voxels = new List<Vector3>();

		private float halfVoxel;

		private Vector3 massCentre;

		public GameObject LimbRig => limbRig;

		public Vector3 VisibleRigScale
		{
			get
			{
				if (!scaleRigToBody || voxelBody == null)
				{
					return authoredRigScale;
				}
				return authoredRigScale * voxelBody.BodySizeRatio;
			}
		}

		public void ApplyTuning(LimbTuning tuning)
		{
			if (tuning != null)
			{
				legReach = tuning.legReach;
				handReach = tuning.handReach;
				legSpread = tuning.legSpread;
				runLift = tuning.runLift;
			}
		}

		private void Awake()
		{
			movement = GetComponent<PlayerMovement>();
			voxelBody = GetComponent<ILimbBody>();
			if (limbRig != null)
			{
				Vector3 localScale = limbRig.transform.localScale;
				if (localScale.sqrMagnitude > 0.0001f)
				{
					authoredRigScale = localScale;
				}
				if (limbAnimator == null)
				{
					limbAnimator = limbRig.GetComponentInChildren<Animator>(includeInactive: true);
				}
				if (limbAnimator != null && !limbAnimator.transform.IsChildOf(limbRig.transform))
				{
					Debug.LogError("[HiderLimbs] Limb Animator (" + limbAnimator.name + ") Limb Rig (" + limbRig.name + ") altinda degil - yanlis Animator bagli. Yok sayiliyor.", this);
					limbAnimator = null;
				}
			}
			runStateHash = Animator.StringToHash(runStateName);
			wallClimbHash = Animator.StringToHash(wallClimbParameter);
			ApplyRigScale(visible: false);
		}

		private void Update()
		{
			Tick(movement != null && movement.IsRunning, Time.deltaTime, movement != null && movement.IsWallClingingNetworked);
		}

		public void DevTick(bool running)
		{
			Tick(running, Time.unscaledDeltaTime);
		}

		public void DevCollapse()
		{
			extend = 0f;
			SetRigPlaying(playing: false, climbing: false);
			ApplyRigScale(visible: false);
			if (voxelBody != null)
			{
				voxelBody.SetRunLean(Quaternion.identity, Vector3.zero);
				voxelBody.SetRunLift(0f);
			}
		}

		private void Tick(bool running, float deltaTime, bool climbing = false)
		{
			if (voxelBody == null)
			{
				return;
			}
			bool flag = running && voxelBody.HasVoxelBody;
			extend = Mathf.MoveTowards(extend, flag ? 1f : 0f, deltaTime / extendSeconds);
			bool flag2 = extend > 0.001f;
			RefreshRestBounds();
			if (!hasRestBounds)
			{
				SetRigPlaying(playing: false, climbing);
				voxelBody.SetRunLean(Quaternion.identity, Vector3.zero);
				return;
			}
			float num = ((Vector3.Dot(base.transform.up, Vector3.up) > 0.5f) ? (extend * runLift) : 0f);
			float num2 = ((movement != null) ? movement.RunLiftHeadroom(num, voxelBody.CurrentRunLift) : num);
			voxelBody.SetRunLift(num2);
			SetRigPlaying(flag2, climbing);
			if (limbAnimator != null)
			{
				limbAnimator.SetBool(wallClimbHash, climbing);
			}
			ApplyClimbSpeed(climbing, deltaTime);
			if (!flag2)
			{
				voxelBody.SetRunLean(Quaternion.identity, Vector3.zero);
				return;
			}
			ApplyRigScale(visible: true);
			float num3 = restBounds.size.x * legSpread;
			float z = restBounds.center.z;
			float y = restBounds.min.y;
			float y2 = restBounds.center.y;
			Vector3 vector = Vector3.up * num2;
			Vector3 retractPoint = restBounds.center + vector;
			Vector3 left = new Vector3(restBounds.center.x - num3, y, z);
			Vector3 right = new Vector3(restBounds.center.x + num3, y, z);
			Vector3 vector2 = new Vector3(restBounds.min.x, y2, z);
			Vector3 vector3 = new Vector3(restBounds.max.x, y2, z);
			if (snapToSurface && voxels.Count > 0)
			{
				left = FindLegRoot(left);
				right = FindLegRoot(right);
				PairLegsIfUneven(ref left, ref right);
				vector2 = FindHandRoot(vector2, -1);
				vector3 = FindHandRoot(vector3, 1);
			}
			Quaternion lean = ComputeLean(left, right);
			Vector3 vector4 = (left + right) * 0.5f + vector;
			voxelBody.SetRunLean(lean, vector4);
			PlaceLimb(legLeft, left + vector + Vector3.down * legReach, retractPoint);
			PlaceLimb(legRight, right + vector + Vector3.down * legReach, retractPoint);
			PlaceLimb(handLeft, Leaned(vector2 + vector, lean, vector4) + Vector3.left * handReach, retractPoint);
			PlaceLimb(handRight, Leaned(vector3 + vector, lean, vector4) + Vector3.right * handReach, retractPoint);
		}

		private static Vector3 Leaned(Vector3 point, Quaternion lean, Vector3 pivot)
		{
			return pivot + lean * (point - pivot);
		}

		private void RebuildVoxels()
		{
			voxels.Clear();
			halfVoxel = 0f;
			Vector3 zero = Vector3.zero;
			Quaternion quaternion = Quaternion.Inverse(voxelBody.CurrentRunLean);
			Vector3 currentRunLeanPivot = voxelBody.CurrentRunLeanPivot;
			Vector3 vector = Vector3.up * voxelBody.CurrentRunLift;
			foreach (VoxelModel item in voxelBody.BodyPieces())
			{
				if (item == null || item.Grid == null)
				{
					continue;
				}
				halfVoxel = Mathf.Max(halfVoxel, item.VoxelSize * 0.5f);
				foreach (Vector3Int position in item.Grid.Positions)
				{
					Vector3 vector2 = base.transform.InverseTransformPoint(item.transform.TransformPoint(position + Vector3.one * 0.5f));
					Vector3 vector3 = currentRunLeanPivot + quaternion * (vector2 - currentRunLeanPivot) - vector;
					voxels.Add(vector3);
					zero += vector3;
				}
			}
			massCentre = ((voxels.Count > 0) ? (zero / voxels.Count) : Vector3.zero);
		}

		private Vector3 FindLegRoot(Vector3 wanted)
		{
			float num = Mathf.Max(restBounds.size.x, restBounds.size.z) * legSearchRadius + halfVoxel;
			float num2 = num * num;
			Vector3 vector = wanted;
			bool flag = false;
			float num3 = float.MaxValue;
			float num4 = float.MaxValue;
			foreach (Vector3 voxel in voxels)
			{
				float num5 = voxel.x - wanted.x;
				float num6 = voxel.z - wanted.z;
				float num7 = num5 * num5 + num6 * num6;
				if (num7 <= num2)
				{
					if (!flag || num4 > num2 || voxel.y < num3 - 0.0001f || (Mathf.Abs(voxel.y - num3) <= 0.0001f && num7 < num4))
					{
						vector = voxel;
						num3 = voxel.y;
						num4 = num7;
						flag = true;
					}
				}
				else if (!flag || (num4 > num2 && num7 < num4))
				{
					vector = voxel;
					num3 = voxel.y;
					num4 = num7;
					flag = true;
				}
			}
			return new Vector3(vector.x, vector.y - halfVoxel, vector.z);
		}

		private void PairLegsIfUneven(ref Vector3 left, ref Vector3 right)
		{
			float num = restBounds.size.y * legHeightTolerance;
			if (!(Mathf.Abs(left.y - right.y) <= num))
			{
				Vector3 vector = ((left.y <= right.y) ? left : right);
				float num2 = legPairGap * 0.5f;
				left = new Vector3(vector.x - num2, vector.y, vector.z);
				right = new Vector3(vector.x + num2, vector.y, vector.z);
			}
		}

		private Vector3 FindHandRoot(Vector3 wanted, int side)
		{
			float num = restBounds.size.y * handBand + halfVoxel;
			float num2 = restBounds.size.z * handBand + halfVoxel;
			Vector3 vector = wanted;
			bool flag = false;
			bool flag2 = false;
			float num3 = float.MinValue;
			foreach (Vector3 voxel in voxels)
			{
				bool flag3 = Mathf.Abs(voxel.y - wanted.y) <= num && Mathf.Abs(voxel.z - wanted.z) <= num2;
				if (!flag2 || flag3)
				{
					float num4 = voxel.x * (float)side;
					if (!flag || (flag3 && !flag2) || num4 > num3)
					{
						vector = voxel;
						num3 = num4;
						flag = true;
						flag2 = flag2 || flag3;
					}
				}
			}
			return new Vector3(vector.x + (float)side * halfVoxel, vector.y, vector.z);
		}

		private Quaternion ComputeLean(Vector3 legLeftRoot, Vector3 legRightRoot)
		{
			if (leanDegreesMax <= 0f || voxels.Count == 0)
			{
				return Quaternion.identity;
			}
			Vector3 vector = (legLeftRoot + legRightRoot) * 0.5f;
			float x = Mathf.Max(massCentre.y - vector.y, halfVoxel * 2f);
			Vector3 vector2 = massCentre - vector;
			float value = (0f - Mathf.Atan2(vector2.z, x)) * 57.29578f * leanFactor;
			float value2 = Mathf.Atan2(vector2.x, x) * 57.29578f * leanFactor;
			value = Mathf.Clamp(value, 0f - leanDegreesMax, leanDegreesMax) * extend;
			value2 = Mathf.Clamp(value2, 0f - leanDegreesMax, leanDegreesMax) * extend;
			return Quaternion.Euler(value, 0f, value2);
		}

		private void ApplyClimbSpeed(bool climbing, float deltaTime)
		{
			Vector3 position = base.transform.position;
			float num = ((hasLastPosition && deltaTime > 0f) ? ((position - lastPosition).magnitude / deltaTime) : 0f);
			lastPosition = position;
			hasLastPosition = true;
			bool flag = limbAnimator != null && limbAnimator.IsInTransition(0);
			float target = ((climbing && !flag && num < 0.3f) ? 0f : 1f);
			float num2 = Mathf.MoveTowards(appliedAnimatorSpeed, target, deltaTime / 0.1f);
			if (num2 != appliedAnimatorSpeed)
			{
				appliedAnimatorSpeed = num2;
				if (limbAnimator != null)
				{
					limbAnimator.speed = num2;
				}
			}
		}

		private void SetRigPlaying(bool playing, bool climbing)
		{
			if (rigPlaying != playing)
			{
				rigPlaying = playing;
				if (playing && limbAnimator != null && !climbing)
				{
					limbAnimator.Play(runStateHash, 0, 0f);
				}
				if (!playing)
				{
					ApplyRigScale(visible: false);
				}
			}
		}

		private void ApplyRigScale(bool visible)
		{
			if (!(limbRig == null))
			{
				Vector3 vector = ((!visible) ? Vector3.zero : ((!scaleRigToBody) ? authoredRigScale : (authoredRigScale * voxelBody.BodySizeRatio)));
				if (limbRig.transform.localScale != vector)
				{
					limbRig.transform.localScale = vector;
				}
			}
		}

		private void PlaceLimb(Transform limb, Vector3 anchor, Vector3 retractPoint)
		{
			if (!(limb == null))
			{
				Vector3 vector = Vector3.Lerp(retractPoint, anchor, extend);
				Transform parent = limb.parent;
				limb.localPosition = ((parent == null || parent == base.transform) ? vector : parent.InverseTransformPoint(base.transform.TransformPoint(vector)));
			}
		}

		private void RefreshRestBounds()
		{
			if (hasRestBounds && appliedBodyVersion == voxelBody.BodyVersion)
			{
				return;
			}
			if (!voxelBody.TryGetBodyBoundsLocal(out var bounds))
			{
				hasRestBounds = false;
				voxels.Clear();
				return;
			}
			bounds.center -= Vector3.up * voxelBody.CurrentRunLift;
			if (snapToSurface)
			{
				RebuildVoxels();
				if (voxels.Count > 0)
				{
					bounds = new Bounds(voxels[0], Vector3.zero);
					foreach (Vector3 voxel in voxels)
					{
						bounds.Encapsulate(voxel);
					}
					bounds.Expand(halfVoxel * 2f);
				}
			}
			else
			{
				voxels.Clear();
			}
			restBounds = bounds;
			hasRestBounds = true;
			appliedBodyVersion = voxelBody.BodyVersion;
		}
	}
}
