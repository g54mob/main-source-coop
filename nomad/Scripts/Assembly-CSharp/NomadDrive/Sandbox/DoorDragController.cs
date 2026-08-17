using UnityEngine;

namespace NomadDrive.Sandbox
{
	public class DoorDragController : MonoBehaviour
	{
		private Rigidbody doorRigidbody;

		private HingeJoint hinge;

		private bool isDragging;

		[SerializeField]
		private Camera mainCamera;

		private float dragSensitivity = 500f;

		private Vector3 previousMousePosition;

		private void Start()
		{
			doorRigidbody = GetComponent<Rigidbody>();
			hinge = GetComponent<HingeJoint>();
			mainCamera = Camera.main;
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo) && hitInfo.transform == base.transform)
			{
				StartDragging();
			}
			if (Input.GetMouseButtonUp(0) && isDragging)
			{
				StopDragging();
			}
			if (isDragging)
			{
				DragDoor();
			}
		}

		private void StartDragging()
		{
			isDragging = true;
			previousMousePosition = Input.mousePosition;
			doorRigidbody.angularDamping = 10f;
			JointSpring spring = hinge.spring;
			spring.spring = 0f;
			hinge.spring = spring;
		}

		private void StopDragging()
		{
			isDragging = false;
			doorRigidbody.angularDamping = 0.05f;
			JointSpring spring = hinge.spring;
			spring.spring = 5f;
			hinge.spring = spring;
		}

		private void DragDoor()
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 vector = mousePosition - previousMousePosition;
			previousMousePosition = mousePosition;
			Vector3 up = Vector3.up;
			float num = vector.x * dragSensitivity * Time.deltaTime;
			doorRigidbody.AddTorque(up * num, ForceMode.Force);
		}
	}
}
