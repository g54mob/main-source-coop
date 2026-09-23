using Mimicraft.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Cameras
{
	public class FirstPersonLook : MonoBehaviour
	{
		[SerializeField]
		private float sensitivity = 0.2f;

		[SerializeField]
		private float minPitch = -80f;

		[SerializeField]
		private float maxPitch = 80f;

		[SerializeField]
		private Transform bodyRoot;

		private float pitch;

		private Vector3 restLocalPosition;

		private readonly CameraShaker shaker = new CameraShaker();

		private readonly AimPunchSpring punchSpring = new AimPunchSpring();

		private const float PunchRestThreshold = 0.001f;

		private const float PunchImpulseScale = 20f;

		private bool hasRestPose;

		public float Pitch => pitch;

		public float MinPitch => minPitch;

		public float MaxPitch => maxPitch;

		private Vector2 punch => punchSpring.Value;

		public Vector2 AimPunch => punchSpring.Value;

		public float SensitivityMultiplier { get; set; } = 1f;

		public float CrouchOffset { get; set; }

		public void SetPitch(float degrees)
		{
			pitch = Mathf.Clamp(degrees, minPitch, maxPitch);
		}

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

		private void Awake()
		{
			if (bodyRoot == null)
			{
				bodyRoot = base.transform.parent;
			}
			restLocalPosition = base.transform.localPosition;
			hasRestPose = true;
		}

		public void ApplyRestPoseWhileInactive(float verticalOffset)
		{
			if (hasRestPose)
			{
				base.transform.localPosition = restLocalPosition + Vector3.up * verticalOffset;
			}
		}

		private void LateUpdate()
		{
			if (Mouse.current != null && !(bodyRoot == null) && !GameMenuState.LookCaptured)
			{
				float num = sensitivity * GameSettings.FpsSensitivity * SensitivityMultiplier;
				Vector2 vector = Mouse.current.delta.ReadValue();
				float num2 = (GameSettings.InvertLookY ? (0f - vector.y) : vector.y);
				bodyRoot.Rotate(Vector3.up, vector.x * num, Space.World);
				pitch = Mathf.Clamp(pitch - num2 * num, minPitch, maxPitch);
				punchSpring.Tick(Time.deltaTime);
				shaker.Tick(Time.deltaTime, out var positionOffset, out var rotationEulerOffset);
				base.transform.localPosition = restLocalPosition + positionOffset + Vector3.up * CrouchOffset;
				base.transform.localRotation = Quaternion.Euler(pitch + punch.x, punch.y, 0f) * Quaternion.Euler(rotationEulerOffset);
			}
		}
	}
}
