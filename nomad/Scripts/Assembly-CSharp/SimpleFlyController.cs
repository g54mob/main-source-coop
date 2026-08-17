using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SimpleFlyController : MonoBehaviour
{
	[Header("Mouse Look Settings")]
	public float lookSensitivity = 2f;

	public float lookSmoothing = 5f;

	public float maxPitch = 89f;

	[Header("Movement Settings")]
	public float moveSpeed = 10f;

	public float fastMultiplier = 3f;

	private float yaw;

	private float pitch;

	private Vector2 smoothLook;

	private bool cursorLocked = true;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		yaw = eulerAngles.y;
		pitch = eulerAngles.x;
		LockCursor(locked: true);
	}

	private void Update()
	{
		HandleCursorToggle();
		if (cursorLocked)
		{
			HandleMouseLook();
			HandleMovement();
		}
	}

	private void HandleCursorToggle()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			LockCursor(!cursorLocked);
		}
	}

	private void LockCursor(bool locked)
	{
		cursorLocked = locked;
		Cursor.lockState = (locked ? CursorLockMode.Locked : CursorLockMode.None);
		Cursor.visible = !locked;
	}

	private void HandleMouseLook()
	{
		smoothLook = Vector2.Lerp(b: new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), a: smoothLook, t: Time.deltaTime * lookSmoothing);
		yaw += smoothLook.x * lookSensitivity;
		pitch -= smoothLook.y * lookSensitivity;
		pitch = Mathf.Clamp(pitch, 0f - maxPitch, maxPitch);
		base.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
	}

	private void HandleMovement()
	{
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float num = 0f;
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		if (Input.GetKey(KeyCode.E))
		{
			num += 1f;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			num -= 1f;
		}
		Vector3 normalized = new Vector3(axisRaw, num, axisRaw2).normalized;
		float num2 = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? fastMultiplier : 1f);
		base.transform.position += base.transform.TransformDirection(normalized) * num2 * Time.deltaTime;
	}
}
