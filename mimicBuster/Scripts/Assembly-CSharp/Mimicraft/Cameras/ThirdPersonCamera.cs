using Mimicraft.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Cameras
{
	public class ThirdPersonCamera : MonoBehaviour
	{
		[SerializeField]
		private float distance = 5f;

		[SerializeField]
		private float minDistance = 1.5f;

		[SerializeField]
		private float maxDistance = 10f;

		[SerializeField]
		private float rotationSpeed = 0.2f;

		[SerializeField]
		private float zoomSpeed = 0.01f;

		[SerializeField]
		private float minPitch = -30f;

		[SerializeField]
		private float maxPitch = 70f;

		[Tooltip("Omuz kamerasındayken dikey bakış sınırları. Nişan alırken normal orbit'in sınırları dar kalıyor - yukarıdaki değerler bir kamerayı çerçevelemek için, bunlar bir silahı doğrultmak için.")]
		[SerializeField]
		private float aimMinPitch = -75f;

		[SerializeField]
		private float aimMaxPitch = 80f;

		[Tooltip("Extra world-space nudge on top of the true center (PivotOffsetLocal) - 0 frames the pivot exactly at the model's center.")]
		[SerializeField]
		private float heightOffset;

		[Tooltip("Bu mesafenin altına inince kamera artık üçüncü şahıs değildir - PlayerCameraRig burayı görüp birinci şahıs takımına geçer. minDistance'tan küçük olmalı, yoksa tekerlek oraya hiç inemez.")]
		[SerializeField]
		[Min(0f)]
		private float firstPersonDistance = 0.7f;

		[Tooltip("Omuz kamerasının mesafesi. Silah çekilince ya da dövüş duruşuna girilince kamera buraya kadar yaklaşır ve daha fazla açılamaz.")]
		[SerializeField]
		[Min(0.2f)]
		private float shoulderDistance = 2.2f;

		[Tooltip("Omuz kamerasının yana kaçıklığı - artı değer sağ omuz. Karakteri karenin bir yanına alır ki nişan aldığın yer boş kalsın.")]
		[SerializeField]
		private float shoulderSide = 0.6f;

		[Tooltip("Omuz kamerasının yükseklik farkı.")]
		[SerializeField]
		private float shoulderHeight = 0.15f;

		[Tooltip("Omuz kamerasına GİRİŞ/ÇIKIŞ hızı: kameranın saniyede kaç metre yaklaşıp uzaklaşacağı. Büyük değer daha sert, küçük değer daha yumuşak.")]
		[SerializeField]
		[Min(0.5f)]
		private float shoulderEaseSpeed = 6f;

		[Tooltip("Omuz kaçıklığının kaç saniyede tamamlanacağı - kameranın yana kayma süresi. Yukarıdaki hızla birlikte geçişin bütün hissini bu ikisi belirler.")]
		[SerializeField]
		[Min(0.01f)]
		private float shoulderBlendSeconds = 0.25f;

		private float yaw;

		private float pitch = 15f;

		private readonly CameraShaker shaker = new CameraShaker();

		private readonly AimPunchSpring punchSpring = new AimPunchSpring();

		private float shoulderSign = 1f;

		private bool allowFirstPersonZoom;

		private Camera lens;

		private const float PivotFollowSpeed = 14f;

		private const float PivotSnapDistance = 6f;

		private Vector3 smoothedPivot;

		private bool hasSmoothedPivot;

		private const float PullInRecoverSpeed = 5f;

		private float pullInApplied;

		private float easedDistance;

		private bool hasEasedDistance;

		private float shoulderBlend;

		private const float PitchSettleSpeed = 120f;

		private float pullOut = 1f;

		private const float PullOutSeconds = 0.7f;

		public Transform Target { get; set; }

		public Vector2 AimPunch => punchSpring.Value;

		public Vector3 PivotOffsetLocal { get; set; }

		public float Yaw => yaw;

		public float Pitch => pitch;

		public float ShoulderSign => shoulderSign;

		public float FirstPersonDistance => firstPersonDistance;

		public float Distance => distance;

		public bool WantsFirstPerson
		{
			get
			{
				if (allowFirstPersonZoom)
				{
					return distance <= firstPersonDistance;
				}
				return false;
			}
		}

		public bool AllowFirstPersonZoom
		{
			get
			{
				return allowFirstPersonZoom;
			}
			set
			{
				if (allowFirstPersonZoom != value)
				{
					allowFirstPersonZoom = value;
					if (!value)
					{
						distance = Mathf.Max(distance, minDistance);
					}
				}
			}
		}

		public bool OverShoulder { get; set; }

		private Camera Lens
		{
			get
			{
				if (!(lens != null))
				{
					return lens = GetComponent<Camera>();
				}
				return lens;
			}
		}

		public float ActualDistance { get; private set; } = float.MaxValue;

		public void AddTrauma(float amount)
		{
			shaker.AddTrauma(amount);
		}

		public void AddAimPunch(Vector2 degrees)
		{
			punchSpring.Add(degrees);
		}

		public void ClearAimPunch()
		{
			punchSpring.Clear();
		}

		public void SyncAngles(float fromYaw, float fromPitch)
		{
			yaw = fromYaw;
			pitch = Mathf.Clamp(fromPitch, minPitch, maxPitch);
		}

		public void SwapShoulder()
		{
			shoulderSign = 0f - shoulderSign;
		}

		public void SetZoom(float value)
		{
			float num = (allowFirstPersonZoom ? firstPersonDistance : minDistance);
			distance = Mathf.Clamp(value, num, Mathf.Max(num, maxDistance));
		}

		public void AddZoom(float scroll)
		{
			if (!Mathf.Approximately(scroll, 0f))
			{
				float num = (allowFirstPersonZoom ? firstPersonDistance : minDistance);
				if (OverShoulder && scroll > 0f && allowFirstPersonZoom)
				{
					distance = num;
				}
				else
				{
					distance = Mathf.Clamp(distance - scroll * zoomSpeed, num, Mathf.Max(num, maxDistance));
				}
			}
		}

		public void PullOutToShoulder()
		{
			distance = Mathf.Max(distance, shoulderDistance);
		}

		private void OnEnable()
		{
			if (Target != null)
			{
				yaw = Target.eulerAngles.y;
			}
			pullInApplied = 0f;
		}

		private void LateUpdate()
		{
			if (!(Target == null) && Mouse.current != null && !GameMenuState.LookCaptured)
			{
				float num = rotationSpeed * GameSettings.TpsSensitivity;
				Vector2 vector = Mouse.current.delta.ReadValue();
				float num2 = (GameSettings.InvertLookY ? (0f - vector.y) : vector.y);
				float min = (OverShoulder ? aimMinPitch : minPitch);
				float max = (OverShoulder ? aimMaxPitch : maxPitch);
				yaw += vector.x * num;
				pitch -= num2 * num;
				pitch = (OverShoulder ? Mathf.Clamp(pitch, min, max) : Mathf.Clamp(Mathf.MoveTowards(pitch, Mathf.Clamp(pitch, min, max), 120f * Time.deltaTime), aimMinPitch, aimMaxPitch));
				float num3 = (allowFirstPersonZoom ? firstPersonDistance : minDistance);
				float b = (OverShoulder ? shoulderDistance : maxDistance);
				AddZoom(Mouse.current.scroll.ReadValue().y);
				TickPullOut();
				float num4 = Mathf.Clamp(distance, num3, Mathf.Max(num3, b));
				easedDistance = (hasEasedDistance ? Mathf.MoveTowards(easedDistance, num4, shoulderEaseSpeed * Time.deltaTime) : num4);
				hasEasedDistance = true;
				shoulderBlend = Mathf.MoveTowards(shoulderBlend, OverShoulder ? shoulderSign : 0f, Time.deltaTime / shoulderBlendSeconds);
				Vector3 vector2 = SmoothPivot(Target.TransformPoint(PivotOffsetLocal) + Vector3.up * heightOffset);
				punchSpring.Tick(Time.deltaTime);
				Vector2 value = punchSpring.Value;
				shaker.Tick(Time.deltaTime, out var positionOffset, out var rotationEulerOffset);
				Quaternion quaternion = Quaternion.Euler(pitch + value.x, yaw + value.y, 0f) * Quaternion.Euler(rotationEulerOffset);
				Vector3 vector3 = quaternion * Vector3.back;
				Vector3 vector4 = vector2 + quaternion * new Vector3(shoulderSide * shoulderBlend, shoulderHeight * Mathf.Abs(shoulderBlend), 0f);
				base.transform.position = vector4 + vector3 * ResolveDistance(vector4, vector3) + positionOffset;
				base.transform.LookAt(vector4);
			}
		}

		private Vector3 SmoothPivot(Vector3 target)
		{
			if (!hasSmoothedPivot)
			{
				hasSmoothedPivot = true;
				smoothedPivot = target;
				return smoothedPivot;
			}
			if ((target - smoothedPivot).sqrMagnitude > 36f)
			{
				smoothedPivot = target;
				pullInApplied = 0f;
			}
			else
			{
				smoothedPivot = Vector3.Lerp(smoothedPivot, target, 1f - Mathf.Exp(-14f * Time.deltaTime));
			}
			return smoothedPivot;
		}

		private float ResolveDistance(Vector3 pivot, Vector3 back)
		{
			float num = easedDistance * pullOut;
			float num2 = CameraCollision.ResolveDistance(CameraCollision.Probe.For(Lens), pivot, back, num, Target);
			float num3 = Mathf.Max(num - num2, 0f);
			pullInApplied = ((num3 >= pullInApplied) ? num3 : Mathf.MoveTowards(pullInApplied, num3, 5f * Time.deltaTime));
			ActualDistance = Mathf.Max(num - pullInApplied, 0.15f);
			return ActualDistance;
		}

		public void BeginPullOut()
		{
			pullOut = 0f;
		}

		public void SkipPullOut()
		{
			pullOut = 1f;
		}

		private void TickPullOut()
		{
			if (!(pullOut >= 1f))
			{
				pullOut = Mathf.MoveTowards(pullOut, 1f, Time.deltaTime / 0.7f);
			}
		}
	}
}
