using NWH.Common.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NWH.Common.Demo
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class RigidbodyFPSController : MonoBehaviour
	{
		public float gravity = 10f;

		public float jumpHeight = 2f;

		public float maximumY = 60f;

		public float maxVelocityChange = 10f;

		public float minimumY = -60f;

		public float sensitivityX = 15f;

		public float sensitivityY = 15f;

		public float speed = 10f;

		private bool _grounded;

		private Rigidbody _rb;

		private float _rotationY;

		private Vector2 _movement;

		private Vector2 _cameraRotationInput;

		private bool PointerOverUI => EventSystem.current.IsPointerOverGameObject();

		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
			_rb.freezeRotation = true;
			_rb.useGravity = false;
		}

		private void LateUpdate()
		{
			_movement = InputProvider.CombinedInput((SceneInputProviderBase i) => i.CharacterMovement());
			_cameraRotationInput = InputProvider.CombinedInput((SceneInputProviderBase i) => i.CameraRotation());
			if (_grounded)
			{
				Vector3 direction = new Vector3(_movement.x, 0f, _movement.y);
				direction = base.transform.TransformDirection(direction);
				direction *= speed;
				Vector3 linearVelocity = _rb.linearVelocity;
				Vector3 force = direction - linearVelocity;
				force.x = Mathf.Clamp(force.x, 0f - maxVelocityChange, maxVelocityChange);
				force.z = Mathf.Clamp(force.z, 0f - maxVelocityChange, maxVelocityChange);
				force.y = 0f;
				_rb.AddForce(force, ForceMode.VelocityChange);
			}
			float num = Time.deltaTime * 20f;
			float y = base.transform.localEulerAngles.y + _cameraRotationInput.x * sensitivityX * num;
			_rotationY += _cameraRotationInput.y * sensitivityY * num;
			_rotationY = Mathf.Clamp(_rotationY, minimumY, maximumY);
			base.transform.localEulerAngles = new Vector3(0f - _rotationY, y, 0f);
			_rb.AddForce(new Vector3(0f, (0f - gravity) * _rb.mass, 0f));
			_grounded = false;
		}

		private float CalculateJumpVerticalSpeed()
		{
			return Mathf.Sqrt(2f * jumpHeight * gravity);
		}

		private void OnCollisionStay()
		{
			_grounded = true;
		}
	}
}
