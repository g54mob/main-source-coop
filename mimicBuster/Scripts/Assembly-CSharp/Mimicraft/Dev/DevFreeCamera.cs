using System.Collections.Generic;
using Mimicraft.Cameras;
using Mimicraft.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace Mimicraft.Dev
{
	public class DevFreeCamera : MonoBehaviour
	{
		private static DevFreeCamera instance;

		private Camera own;

		private readonly List<Camera> suspended = new List<Camera>();

		private FirstPersonLook parkedLook;

		private PlayerMovement parkedMovement;

		private bool restoreMovementInput;

		private float speed = 8f;

		private float pitch;

		private float yaw;

		private Transform followTarget;

		private Vector3 followPosition;

		public static bool Active => instance != null;

		public static Camera Camera
		{
			get
			{
				if (!(instance != null))
				{
					return null;
				}
				return instance.own;
			}
		}

		public static bool IsAttached
		{
			get
			{
				if (instance != null)
				{
					return instance.followTarget != null;
				}
				return false;
			}
		}

		public static Transform AttachedTo
		{
			get
			{
				if (!(instance != null))
				{
					return null;
				}
				return instance.followTarget;
			}
		}

		public static float Speed
		{
			get
			{
				if (!(instance != null))
				{
					return 0f;
				}
				return instance.speed;
			}
			set
			{
				if (instance != null)
				{
					instance.speed = Mathf.Clamp(value, 0.1f, 200f);
				}
			}
		}

		public static void Attach(Transform target)
		{
			if (!(instance == null) && !(target == null))
			{
				instance.followTarget = target;
				instance.followPosition = target.InverseTransformPoint(instance.transform.position);
				Vector3 eulerAngles = (Quaternion.Inverse(target.rotation) * instance.transform.rotation).eulerAngles;
				instance.pitch = ((eulerAngles.x > 180f) ? (eulerAngles.x - 360f) : eulerAngles.x);
				instance.yaw = eulerAngles.y;
			}
		}

		public static void Detach()
		{
			if (!(instance == null) && !(instance.followTarget == null))
			{
				instance.followTarget = null;
				Vector3 eulerAngles = instance.transform.eulerAngles;
				instance.pitch = ((eulerAngles.x > 180f) ? (eulerAngles.x - 360f) : eulerAngles.x);
				instance.yaw = eulerAngles.y;
			}
		}

		public static void FollowNow()
		{
			if (!(instance == null) && !(instance.followTarget == null))
			{
				instance.transform.SetPositionAndRotation(instance.followTarget.TransformPoint(instance.followPosition), instance.followTarget.rotation * Quaternion.Euler(instance.pitch, instance.yaw, 0f));
			}
		}

		public static void SetActive(bool active)
		{
			if (active == Active)
			{
				return;
			}
			if (!active)
			{
				if (instance != null)
				{
					Object.Destroy(instance.gameObject);
				}
			}
			else
			{
				GameObject obj = new GameObject("DevFreeCamera");
				Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<DevFreeCamera>();
			}
		}

		public static string Describe()
		{
			if (instance == null)
			{
				return "";
			}
			Vector3 position = instance.transform.position;
			Vector3 eulerAngles = instance.transform.eulerAngles;
			return $"cam {position.x:0.##} {position.y:0.##} {position.z:0.##} {eulerAngles.x:0.#} {eulerAngles.y:0.#}";
		}

		public static void Place(Vector3 position, float pitchDegrees, float yawDegrees)
		{
			if (!(instance == null))
			{
				instance.transform.position = position;
				instance.pitch = pitchDegrees;
				instance.yaw = yawDegrees;
				instance.transform.rotation = Quaternion.Euler(pitchDegrees, yawDegrees, 0f);
			}
		}

		private void Awake()
		{
			instance = this;
			Camera main = Camera.main;
			if (main != null)
			{
				base.transform.SetPositionAndRotation(main.transform.position, main.transform.rotation);
				Vector3 eulerAngles = main.transform.eulerAngles;
				pitch = ((eulerAngles.x > 180f) ? (eulerAngles.x - 360f) : eulerAngles.x);
				yaw = eulerAngles.y;
			}
			own = base.gameObject.AddComponent<Camera>();
			if (main != null)
			{
				own.fieldOfView = main.fieldOfView;
				own.nearClipPlane = main.nearClipPlane;
				own.farClipPlane = main.farClipPlane;
				own.clearFlags = main.clearFlags;
				own.backgroundColor = main.backgroundColor;
				own.cullingMask = main.cullingMask;
				CopyUniversalSettings(main);
			}
			own.depth = 1000f;
			SuspendOtherCameras();
			ParkLocalPlayer();
		}

		private void CopyUniversalSettings(Camera source)
		{
			UniversalAdditionalCameraData component = source.GetComponent<UniversalAdditionalCameraData>();
			UniversalAdditionalCameraData universalAdditionalCameraData = own.GetUniversalAdditionalCameraData();
			if (!(component == null) && !(universalAdditionalCameraData == null))
			{
				universalAdditionalCameraData.renderPostProcessing = component.renderPostProcessing;
				universalAdditionalCameraData.antialiasing = component.antialiasing;
				universalAdditionalCameraData.antialiasingQuality = component.antialiasingQuality;
				universalAdditionalCameraData.renderShadows = component.renderShadows;
				universalAdditionalCameraData.volumeLayerMask = component.volumeLayerMask;
				universalAdditionalCameraData.requiresDepthOption = component.requiresDepthOption;
				universalAdditionalCameraData.requiresColorOption = component.requiresColorOption;
				universalAdditionalCameraData.stopNaN = component.stopNaN;
				universalAdditionalCameraData.dithering = component.dithering;
				universalAdditionalCameraData.allowXRRendering = component.allowXRRendering;
			}
		}

		private void OnDestroy()
		{
			foreach (Camera item in suspended)
			{
				if (item != null)
				{
					item.enabled = true;
				}
			}
			if (parkedLook != null)
			{
				parkedLook.enabled = true;
			}
			if (parkedMovement != null)
			{
				parkedMovement.InputEnabled = restoreMovementInput;
			}
			if (instance == this)
			{
				instance = null;
			}
		}

		private void SuspendOtherCameras()
		{
			Camera[] array = Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (Camera camera in array)
			{
				if (!(camera == own) && camera.enabled)
				{
					suspended.Add(camera);
					camera.enabled = false;
				}
			}
		}

		private void ParkLocalPlayer()
		{
			PlayerMovement[] array = Object.FindObjectsByType<PlayerMovement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			foreach (PlayerMovement playerMovement in array)
			{
				if (playerMovement.IsOwner)
				{
					parkedMovement = playerMovement;
					restoreMovementInput = playerMovement.InputEnabled;
					playerMovement.InputEnabled = false;
					parkedLook = playerMovement.GetComponentInChildren<FirstPersonLook>(includeInactive: true);
					if (parkedLook != null)
					{
						parkedLook.enabled = false;
					}
					break;
				}
			}
		}

		private void Update()
		{
			if (!GameMenuState.IsDevConsoleOpen && !GameMenuState.IsDevPointerOpen && Keyboard.current != null)
			{
				if (followTarget != null)
				{
					FlyAttached();
				}
				else if (DevCameraPathPlayer.Tick(base.transform, own, Time.unscaledDeltaTime))
				{
					Vector3 eulerAngles = base.transform.eulerAngles;
					pitch = ((eulerAngles.x > 180f) ? (eulerAngles.x - 360f) : eulerAngles.x);
					yaw = eulerAngles.y;
				}
				else
				{
					Look();
					Move();
				}
			}
		}

		private void FlyAttached()
		{
			if (Mouse.current != null)
			{
				Vector2 vector = Mouse.current.delta.ReadValue() * 0.08f;
				yaw += vector.x;
				pitch = Mathf.Clamp(pitch - vector.y, -89f, 89f);
			}
			Keyboard current = Keyboard.current;
			Vector3 vector2 = new Vector3((current.dKey.isPressed ? 1f : 0f) - (current.aKey.isPressed ? 1f : 0f), (current.eKey.isPressed ? 1f : 0f) - (current.qKey.isPressed ? 1f : 0f), (current.wKey.isPressed ? 1f : 0f) - (current.sKey.isPressed ? 1f : 0f));
			if (Mouse.current != null)
			{
				float y = Mouse.current.scroll.ReadValue().y;
				if (Mathf.Abs(y) > 0.01f)
				{
					speed = Mathf.Clamp(speed * ((y > 0f) ? 1.15f : 0.86956525f), 0.1f, 200f);
				}
			}
			if (!(vector2.sqrMagnitude < 0.0001f))
			{
				float num = (current.leftShiftKey.isPressed ? 4f : (current.leftCtrlKey.isPressed ? 0.2f : 1f));
				Vector3 vector3 = Quaternion.Euler(pitch, yaw, 0f) * vector2.normalized * (speed * num * Time.unscaledDeltaTime);
				Vector3 lossyScale = followTarget.lossyScale;
				followPosition += new Vector3(vector3.x / Mathf.Max(0.0001f, lossyScale.x), vector3.y / Mathf.Max(0.0001f, lossyScale.y), vector3.z / Mathf.Max(0.0001f, lossyScale.z));
			}
		}

		private void Look()
		{
			if (Mouse.current != null)
			{
				Vector2 vector = Mouse.current.delta.ReadValue() * 0.08f;
				yaw += vector.x;
				pitch = Mathf.Clamp(pitch - vector.y, -89f, 89f);
				base.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
			}
		}

		private void Move()
		{
			Keyboard current = Keyboard.current;
			Vector3 vector = new Vector3((current.dKey.isPressed ? 1f : 0f) - (current.aKey.isPressed ? 1f : 0f), (current.eKey.isPressed ? 1f : 0f) - (current.qKey.isPressed ? 1f : 0f), (current.wKey.isPressed ? 1f : 0f) - (current.sKey.isPressed ? 1f : 0f));
			if (Mouse.current != null)
			{
				float y = Mouse.current.scroll.ReadValue().y;
				if (Mathf.Abs(y) > 0.01f)
				{
					speed = Mathf.Clamp(speed * ((y > 0f) ? 1.15f : 0.86956525f), 0.1f, 200f);
				}
			}
			float num = (current.leftShiftKey.isPressed ? 4f : (current.leftCtrlKey.isPressed ? 0.2f : 1f));
			base.transform.position += base.transform.TransformDirection(vector.normalized) * (speed * num * Time.unscaledDeltaTime);
		}
	}
}
