using System.Collections.Generic;
using Mimicraft.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Cameras
{
	public class OrbitCamera : MonoBehaviour
	{
		[SerializeField]
		private Vector3 focusPoint = Vector3.zero;

		[SerializeField]
		private float distance = 6f;

		[SerializeField]
		private float minDistance = 2f;

		[SerializeField]
		private float maxDistance = 20f;

		[SerializeField]
		private float rotationSpeed = 0.2f;

		[SerializeField]
		private float zoomSpeed = 0.01f;

		[SerializeField]
		private float minPitch = -80f;

		[SerializeField]
		private float maxPitch = 80f;

		[SerializeField]
		private float startYaw = 45f;

		[SerializeField]
		private float startPitch = 30f;

		[SerializeField]
		private float panSpeed = 0.0015f;

		private float yaw;

		private float pitch;

		private Camera lens;

		private static readonly float[] FallbackPitches = new float[4] { 45f, 60f, 15f, 75f };

		private const float SearchStepDegrees = 30f;

		private const int SearchSteps = 6;

		private const float AcceptableRoomFraction = 0.35f;

		private const float FramingMargin = 2.5f;

		public float Yaw => yaw;

		public float Distance => distance;

		private CameraCollision.Probe Probe => CameraCollision.Probe.For((lens != null) ? lens : (lens = GetComponent<Camera>()));

		public Transform IgnoreRoot { get; set; }

		public bool CollisionEnabled { get; set; } = true;

		private void Awake()
		{
			yaw = startYaw;
			pitch = startPitch;
		}

		private void LateUpdate()
		{
			if (Mouse.current != null && !GameMenuState.LookCaptured)
			{
				if (Mouse.current.rightButton.isPressed)
				{
					Vector2 vector = Mouse.current.delta.ReadValue();
					yaw += vector.x * rotationSpeed;
					pitch -= vector.y * rotationSpeed;
					pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
				}
				if (Mouse.current.middleButton.isPressed)
				{
					Vector2 vector2 = Mouse.current.delta.ReadValue();
					focusPoint -= (base.transform.right * vector2.x + base.transform.up * vector2.y) * panSpeed * distance;
				}
				float num = ((Keyboard.current != null && (Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed)) ? 0f : Mouse.current.scroll.ReadValue().y);
				if (!Mathf.Approximately(num, 0f))
				{
					float num2 = zoomSpeed * GameSettings.EditorZoomSensitivity;
					distance = Mathf.Clamp(distance - num * num2, minDistance, maxDistance);
				}
				Vector3 vector3 = Quaternion.Euler(pitch, yaw, 0f) * Vector3.back;
				float num3 = (CollisionEnabled ? CameraCollision.ResolveDistance(Probe, focusPoint, vector3, distance, IgnoreRoot) : distance);
				base.transform.position = focusPoint + vector3 * num3;
				base.transform.LookAt(focusPoint);
			}
		}

		public void SetFocusPoint(Vector3 point)
		{
			focusPoint = point;
		}

		public void SettleIntoClearSpace()
		{
			float room = RoomAt(yaw, pitch);
			if (Enough(room))
			{
				ClampDistanceTo(room);
				return;
			}
			foreach (float item in PitchCandidates(pitch))
			{
				for (int i = 0; i <= 6; i++)
				{
					float num = (float)i * 30f;
					float room2 = RoomAt(yaw + num, item);
					if (Enough(room2))
					{
						yaw += num;
						pitch = item;
						ClampDistanceTo(room2);
						return;
					}
					if (i != 0)
					{
						room2 = RoomAt(yaw - num, item);
						if (Enough(room2))
						{
							yaw -= num;
							pitch = item;
							ClampDistanceTo(room2);
							return;
						}
					}
				}
			}
			float num2 = yaw;
			float num3 = pitch;
			float num4 = -1f;
			foreach (float item2 in PitchCandidates(pitch))
			{
				for (int j = 0; j < 12; j++)
				{
					float num5 = yaw + (float)j * 30f;
					float num6 = RoomAt(num5, item2);
					if (!(num6 <= num4))
					{
						num4 = num6;
						num2 = num5;
						num3 = item2;
					}
				}
			}
			yaw = num2;
			pitch = num3;
			ClampDistanceTo(num4);
		}

		public void FindClearAngle()
		{
			SettleIntoClearSpace();
		}

		private void ClampDistanceTo(float room)
		{
			distance = Mathf.Clamp(Mathf.Min(distance, room), minDistance, maxDistance);
		}

		private IEnumerable<float> PitchCandidates(float current)
		{
			yield return current;
			float[] fallbackPitches = FallbackPitches;
			for (int i = 0; i < fallbackPitches.Length; i++)
			{
				float num = Mathf.Clamp(fallbackPitches[i], minPitch, maxPitch);
				if (!Mathf.Approximately(num, current))
				{
					yield return num;
				}
			}
		}

		private bool Enough(float room)
		{
			return room >= distance * 0.35f;
		}

		private float RoomAt(float candidateYaw, float candidatePitch)
		{
			Vector3 direction = Quaternion.Euler(candidatePitch, candidateYaw, 0f) * Vector3.back;
			return CameraCollision.ClearDistance(Probe, focusPoint, direction, distance, IgnoreRoot);
		}

		public void SetMinDistance(float min)
		{
			minDistance = Mathf.Max(0.01f, min);
			distance = Mathf.Max(distance, minDistance);
		}

		public void Frame(Vector3 point, float radius)
		{
			focusPoint = point;
			distance = Mathf.Clamp(Mathf.Max(radius, 0.01f) * 2.5f, minDistance, maxDistance);
		}

		public void SetDistanceLimits(float min, float max)
		{
			minDistance = Mathf.Max(0.01f, min);
			maxDistance = Mathf.Max(minDistance + 0.01f, max);
			distance = Mathf.Clamp(distance, minDistance, maxDistance);
		}
	}
}
