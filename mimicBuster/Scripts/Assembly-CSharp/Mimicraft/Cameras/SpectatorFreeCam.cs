using Mimicraft.Gameplay;
using Mimicraft.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.Cameras
{
	public class SpectatorFreeCam : MonoBehaviour
	{
		private const float MoveSpeed = 12f;

		private const float RunMultiplier = 3f;

		private const float LookSensitivity = 0.15f;

		private const float MinPitch = -85f;

		private const float MaxPitch = 85f;

		private float yaw;

		private float pitch;

		private const float Radius = 0.3f;

		private const float Skin = 0.02f;

		public bool Solid { get; set; }

		public void AdoptCurrentPose()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			yaw = eulerAngles.y;
			pitch = ((eulerAngles.x > 180f) ? (eulerAngles.x - 360f) : eulerAngles.x);
			pitch = Mathf.Clamp(pitch, -85f, 85f);
		}

		private void LateUpdate()
		{
			if (GameMenuState.LookCaptured)
			{
				return;
			}
			if (Mouse.current != null)
			{
				Vector2 vector = Mouse.current.delta.ReadValue();
				yaw += vector.x * 0.15f;
				pitch = Mathf.Clamp(pitch - vector.y * 0.15f, -85f, 85f);
				base.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
			}
			Vector2 vector2 = GameInput.Move.ReadValue<Vector2>();
			Vector3 vector3 = base.transform.forward * vector2.y + base.transform.right * vector2.x;
			if (GameInput.Jump.IsPressed())
			{
				vector3 += Vector3.up;
			}
			if (GameInput.Crouch.IsPressed())
			{
				vector3 -= Vector3.up;
			}
			Keyboard current = Keyboard.current;
			if (current != null)
			{
				if (current.eKey.isPressed)
				{
					vector3 += Vector3.up;
				}
				if (current.qKey.isPressed)
				{
					vector3 -= Vector3.up;
				}
			}
			if (vector3.sqrMagnitude > 1f)
			{
				vector3.Normalize();
			}
			bool flag = GameInput.Run.IsPressed();
			Vector3 delta = vector3 * (12f * (flag ? 3f : 1f)) * Time.deltaTime;
			base.transform.position = (Solid ? Confine(base.transform.position, SlideAlongLevel(base.transform.position, delta) - base.transform.position) : Confine(base.transform.position, delta));
		}

		private static Vector3 Confine(Vector3 from, Vector3 delta)
		{
			if (!MapBounds.AnyBaked || !MapBounds.IsInsidePlayArea(from))
			{
				return from + delta;
			}
			Vector3 position = from;
			TryAxis(ref position, new Vector3(delta.x, 0f, 0f));
			TryAxis(ref position, new Vector3(0f, delta.y, 0f));
			TryAxis(ref position, new Vector3(0f, 0f, delta.z));
			return position;
		}

		private static Vector3 SlideAlongLevel(Vector3 from, Vector3 delta)
		{
			int num = -1957;
			if (Physics.CheckSphere(from, 0.3f, num, QueryTriggerInteraction.Ignore))
			{
				return from + delta;
			}
			return StepAxis(StepAxis(StepAxis(from, new Vector3(delta.x, 0f, 0f), num), new Vector3(0f, delta.y, 0f), num), new Vector3(0f, 0f, delta.z), num);
		}

		private static Vector3 StepAxis(Vector3 position, Vector3 step, int mask)
		{
			float magnitude = step.magnitude;
			if (magnitude < 1E-05f)
			{
				return position;
			}
			Vector3 vector = step / magnitude;
			if (!Physics.SphereCast(position, 0.3f, vector, out var hitInfo, magnitude + 0.02f, mask, QueryTriggerInteraction.Ignore))
			{
				return position + step;
			}
			return position + vector * Mathf.Max(0f, hitInfo.distance - 0.02f);
		}

		private static void TryAxis(ref Vector3 position, Vector3 step)
		{
			if (!(step.sqrMagnitude < 1E-12f))
			{
				Vector3 vector = position + step;
				if (MapBounds.IsInsidePlayArea(vector))
				{
					position = vector;
				}
			}
		}
	}
}
