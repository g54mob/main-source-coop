using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace NomadDrive.Sandbox.Test
{
	[RequireComponent(typeof(Camera))]
	public class FreeRoamCameraController : MonoBehaviour
	{
		[Header("Movement Settings")]
		[SerializeField]
		private float moveSpeed = 10f;

		[SerializeField]
		private float sprintMultiplier = 2f;

		[SerializeField]
		private float smoothTime = 0.05f;

		[Header("Look Settings")]
		[SerializeField]
		private float mouseSensitivity = 2f;

		[SerializeField]
		private float maxLookAngle = 80f;

		[SerializeField]
		private float mouseSmoothing = 0.02f;

		[Header("References")]
		[SerializeField]
		private Camera freeRoamCamera;

		private bool isActive;

		private Vector3 currentVelocity;

		private Vector3 targetPosition;

		private float verticalRotation;

		private Vector2 currentMouseDelta;

		private Vector2 mouseVelocity;

		private Vector3 inputDirection;

		private Vector2 mouseDelta;

		private Vector2 targetMouseDelta;

		public bool IsActive => isActive;

		private void Awake()
		{
			if (freeRoamCamera == null)
			{
				freeRoamCamera = GetComponent<Camera>();
			}
			if (freeRoamCamera == null)
			{
				EvilLogger.LogError("FreeRoamCameraController: No camera component found!", "Awake", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Sandbox\\Test\\Scripts\\FreeRoamCameraController.cs", 44);
				return;
			}
			targetPosition = base.transform.position;
			SetCameraState(active: false);
		}

		private void Update()
		{
			HandleToggleInput();
			if (isActive)
			{
				HandleMovementInput();
				HandleMouseInput();
			}
		}

		private void LateUpdate()
		{
			if (isActive)
			{
				UpdateMovement();
				UpdateRotation();
			}
		}

		private void HandleToggleInput()
		{
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
			{
				ToggleCamera();
			}
		}

		private void HandleMovementInput()
		{
			inputDirection = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				inputDirection += base.transform.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				inputDirection -= base.transform.forward;
			}
			if (Input.GetKey(KeyCode.A))
			{
				inputDirection -= base.transform.right;
			}
			if (Input.GetKey(KeyCode.D))
			{
				inputDirection += base.transform.right;
			}
			if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
			{
				inputDirection += Vector3.up;
			}
			if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.C))
			{
				inputDirection -= Vector3.up;
			}
			inputDirection = inputDirection.normalized;
			float num = moveSpeed;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				num *= sprintMultiplier;
			}
			targetPosition += inputDirection * num * Time.deltaTime;
		}

		private void HandleMouseInput()
		{
			if (isActive)
			{
				targetMouseDelta.x = Input.GetAxis("Mouse X") * mouseSensitivity;
				targetMouseDelta.y = Input.GetAxis("Mouse Y") * mouseSensitivity;
			}
		}

		private void UpdateMovement()
		{
			base.transform.position = Vector3.SmoothDamp(base.transform.position, targetPosition, ref currentVelocity, smoothTime);
		}

		private void UpdateRotation()
		{
			currentMouseDelta = Vector2.Lerp(currentMouseDelta, targetMouseDelta, 1f - mouseSmoothing);
			base.transform.Rotate(Vector3.up, currentMouseDelta.x, Space.World);
			verticalRotation -= currentMouseDelta.y;
			verticalRotation = Mathf.Clamp(verticalRotation, 0f - maxLookAngle, maxLookAngle);
			base.transform.localEulerAngles = new Vector3(verticalRotation, base.transform.localEulerAngles.y, 0f);
		}

		private void ToggleCamera()
		{
			SetCameraState(!isActive);
		}

		private void SetCameraState(bool active)
		{
			isActive = active;
			if (freeRoamCamera != null)
			{
				freeRoamCamera.enabled = active;
			}
			if (active)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				targetPosition = base.transform.position;
				currentMouseDelta = Vector2.zero;
				targetMouseDelta = Vector2.zero;
				mouseVelocity = Vector2.zero;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		private void OnDisable()
		{
			if (isActive)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		public void SetMoveSpeed(float speed)
		{
			moveSpeed = speed;
		}

		public void SetMouseSensitivity(float sensitivity)
		{
			mouseSensitivity = sensitivity;
		}

		public void SetMouseSmoothing(float smoothing)
		{
			mouseSmoothing = Mathf.Clamp01(smoothing);
		}

		public void TeleportTo(Vector3 position)
		{
			base.transform.position = position;
			targetPosition = position;
		}

		public void TeleportTo(Transform target)
		{
			TeleportTo(target.position);
			base.transform.rotation = target.rotation;
			verticalRotation = base.transform.eulerAngles.x;
		}
	}
}
