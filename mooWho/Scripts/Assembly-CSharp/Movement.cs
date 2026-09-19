using Mirror;
using UnityEngine;

public class Movement : NetworkBehaviour
{
	[Header("Movement Settings")]
	[SerializeField]
	private float walkingSpeed = 7.5f;

	[SerializeField]
	private float runningSpeed = 11.5f;

	[SerializeField]
	private float jumpHeight = 8f;

	[SerializeField]
	private float gravity = 20f;

	public Camera playerCamera;

	[Header("Grounding")]
	[SerializeField]
	private LayerMask groundMask;

	[SerializeField]
	private Transform groundedCheck;

	[SerializeField]
	private float groundedDistance = 2f;

	[Header("Camera")]
	[SerializeField]
	private Transform camHolder;

	[SerializeField]
	private float lookSpeed = 2f;

	[SerializeField]
	private float lookXLimit = 45f;

	private float rotationX;

	private Vector2 inputDir;

	private Vector3 velocity;

	public CharacterController characterController { get; private set; }

	public bool isGrounded { get; private set; }

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		characterController = GetComponent<CharacterController>();
	}

	private void FixedUpdate()
	{
		if (base.isLocalPlayer)
		{
			CheckIfGrounded();
			bool key = Input.GetKey(KeyCode.LeftShift);
			MovementHandler(key);
		}
	}

	private void MovementHandler(bool isRunning)
	{
		if (isGrounded && velocity.y < 0f)
		{
			velocity.y = 0f;
		}
		Vector3 normalized = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
		normalized *= (isRunning ? runningSpeed : walkingSpeed);
		normalized = base.transform.TransformDirection(normalized);
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y += Mathf.Sqrt(jumpHeight * -3f * (0f - gravity));
		}
		velocity.y -= gravity * Time.deltaTime;
		Vector3 vector = normalized;
		vector.y = velocity.y;
		characterController.Move(vector * Time.deltaTime);
	}

	public void CheckIfGrounded()
	{
		isGrounded = Physics.Raycast(groundedCheck.position, Vector3.down, groundedDistance, groundMask);
	}

	private void LateUpdate()
	{
		if (base.isLocalPlayer)
		{
			inputDir = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
			rotationX += (0f - inputDir.y) * lookSpeed;
			rotationX = Mathf.Clamp(rotationX, 0f - lookXLimit, lookXLimit);
			camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
			base.transform.rotation *= Quaternion.Euler(0f, inputDir.x * lookSpeed, 0f);
		}
	}

	private void OnDrawGizmos()
	{
		if ((bool)groundedCheck)
		{
			Gizmos.DrawRay(groundedCheck.position, Vector3.down * groundedDistance);
		}
	}

	public override bool Weaved()
	{
		return true;
	}
}
