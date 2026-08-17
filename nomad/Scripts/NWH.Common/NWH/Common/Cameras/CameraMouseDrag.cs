using NWH.Common.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace NWH.Common.Cameras
{
	public class CameraMouseDrag : VehicleCamera
	{
		public enum POVType
		{
			FirstPerson = 0,
			ThirdPerson = 1
		}

		[Tooltip("Camera POV type. First person camera will invert controls.\r\nZoom is not available in 1st person.")]
		public POVType povType = POVType.ThirdPerson;

		[Tooltip("    Can the camera be rotated by the user?")]
		public bool allowRotation = true;

		[Tooltip("    Can the camera be panned by the user?")]
		public bool allowPanning = true;

		[Range(0f, 100f)]
		[Tooltip("    Distance from target at which camera will be positioned. Might vary depending on smoothing.")]
		public float distance = 5f;

		[FormerlySerializedAs("followTargetsRotation")]
		[Tooltip("    If true the camera will rotate with the vehicle along the X and Y axis.")]
		public bool followTargetPitchAndYaw = true;

		[Tooltip("    If true the camera will rotate with the vehicle along the Z axis.")]
		public bool followTargetRoll;

		[Range(0f, 100f)]
		[Tooltip("    Maximum distance that will be reached when zooming out.")]
		public float maxDistance = 13f;

		[Range(0f, 100f)]
		[Tooltip("    Minimum distance that will be reached when zooming in.")]
		public float minDistance = 3f;

		[Range(0f, 15f)]
		[Tooltip("    Sensitivity of the middle mouse button / wheel.")]
		public float zoomSensitivity = 1f;

		[Range(0f, 1f)]
		[Tooltip("    Smoothing of the camera rotation.")]
		public float rotationSmoothing = 0.02f;

		[Range(-90f, 90f)]
		[Tooltip("    Maximum vertical angle the camera can achieve.")]
		public float verticalMaxAngle = 80f;

		[Range(-90f, 90f)]
		[Tooltip("    Minimum vertical angle the camera can achieve.")]
		public float verticalMinAngle = -40f;

		[Tooltip("    Sensitivity of rotation input.")]
		public Vector2 rotationSensitivity = new Vector2(3f, 3f);

		[Tooltip("    Sensitivity of panning input.")]
		public Vector2 panningSensitivity = new Vector2(0.06f, 0.06f);

		[Tooltip("    Initial rotation around the X axis (up/down)")]
		public float initXRotation;

		[Tooltip("    Initial rotation around the Y axis (left/right)")]
		public float initYRotation;

		[Tooltip("    Look position offset from the target center.")]
		public Vector3 targetPositionOffset = Vector3.zero;

		[Tooltip("Should camera movement on acceleration be used?")]
		public bool useShake = true;

		[Range(0f, 1f)]
		[Tooltip("    Maximum head movement from the initial position.")]
		public float shakeMaxOffset = 0.2f;

		[Range(0f, 1f)]
		[Tooltip("    How much will the head move around for the given g-force.")]
		public float shakeIntensity = 0.125f;

		[Range(0f, 1f)]
		[Tooltip("    Smoothing of the head movement.")]
		public float shakeSmoothing = 0.3f;

		[Tooltip("    Movement intensity per axis. Set to 0 to disable movement on that axis or negative to reverse it.")]
		public Vector3 shakeAxisIntensity = new Vector3(1f, 0.5f, 1f);

		private Vector3 _lookDir;

		private Vector3 _newLookDir;

		private Vector3 _lookDirVel;

		private Vector3 _lookAtPosition;

		private Vector2 _rot;

		private Vector3 _pan;

		private bool _isFirstFrame;

		private bool _rotationModifier;

		private bool _panningModifier;

		private Vector2 _rotationInput;

		private Vector2 _panningInput;

		private float _zoomInput;

		private Vector3 _acceleration;

		private Vector3 _prevAcceleration;

		private Vector3 _accelerationChangeVelocity;

		private Vector3 _initialPosition;

		private Vector3 _localAcceleration;

		private Vector3 _newPositionOffset;

		private Vector3 _offsetChangeVelocity;

		private Vector3 _positionOffset;

		private Rigidbody _rigidbody;

		private float _rbSpeed;

		private Vector3 _rbLocalAcceleration;

		private Vector3 _rbLocalVelocity;

		private Vector3 _rbPrevLocalVelocity;

		private bool PointerOverUI
		{
			get
			{
				if (EventSystem.current != null)
				{
					return EventSystem.current.IsPointerOverGameObject();
				}
				return false;
			}
		}

		private void Start()
		{
			_initialPosition = base.transform.localPosition;
			_rigidbody = target?.GetComponent<Rigidbody>();
			distance = Mathf.Clamp(distance, minDistance, maxDistance);
			_rot.x = initXRotation;
			_rot.y = initYRotation;
			_isFirstFrame = true;
		}

		private void FixedUpdate()
		{
			if (!(_rigidbody == null))
			{
				_rbPrevLocalVelocity = _rbLocalVelocity;
				_rbLocalVelocity = base.transform.InverseTransformDirection(_rigidbody.linearVelocity);
				_rbLocalAcceleration = (_rbLocalVelocity - _rbPrevLocalVelocity) / Time.fixedDeltaTime;
				_rbSpeed = ((_rbLocalVelocity.z < 0f) ? (0f - _rbLocalVelocity.z) : _rbLocalVelocity.z);
			}
		}

		private void LateUpdate()
		{
			if (target == null)
			{
				return;
			}
			if (!PointerOverUI)
			{
				_rotationInput = InputProvider.CombinedInput((SceneInputProviderBase i) => i.CameraRotation());
				_panningInput = InputProvider.CombinedInput((SceneInputProviderBase i) => i.CameraPanning());
				_zoomInput = InputProvider.CombinedInput((SceneInputProviderBase i) => i.CameraZoom());
				_rotationModifier = InputProvider.CombinedInput((SceneInputProviderBase i) => i.CameraRotationModifier());
				_panningModifier = InputProvider.CombinedInput((SceneInputProviderBase i) => i.CameraPanningModifier());
				if (allowRotation && _rotationModifier)
				{
					_rot.y += _rotationInput.x * rotationSensitivity.x;
					_rot.x -= _rotationInput.y * rotationSensitivity.y;
				}
				if (allowPanning && _panningModifier)
				{
					float num = _panningInput.x * panningSensitivity.x;
					float num2 = _panningInput.y * panningSensitivity.y;
					_pan -= target.InverseTransformDirection(base.transform.right * num);
					_pan -= target.InverseTransformDirection(base.transform.up * num2);
				}
				_rot.x = ClampAngle(_rot.x, verticalMinAngle, verticalMaxAngle);
				if (povType == POVType.ThirdPerson && (_zoomInput > 0.0001f || _zoomInput < -0.0001f))
				{
					distance -= _zoomInput * zoomSensitivity;
				}
			}
			Vector3 vector = (followTargetPitchAndYaw ? target.forward : Vector3.forward);
			Vector3 axis = (followTargetPitchAndYaw ? target.right : Vector3.right);
			Vector3 axis2 = (followTargetPitchAndYaw ? target.up : Vector3.up);
			_lookAtPosition = target.position + target.TransformDirection(targetPositionOffset + _pan);
			_newLookDir = Quaternion.AngleAxis(_rot.x, axis) * vector;
			_newLookDir = Quaternion.AngleAxis(_rot.y, axis2) * _newLookDir;
			_lookDir = (_isFirstFrame ? _newLookDir : Vector3.SmoothDamp(_lookDir, _newLookDir, ref _lookDirVel, rotationSmoothing));
			_lookDir = Vector3.Normalize(_lookDir);
			if (povType == POVType.ThirdPerson)
			{
				distance = ((povType == POVType.FirstPerson) ? 0f : Mathf.Clamp(distance, minDistance, maxDistance));
				Vector3 position = _lookAtPosition - _lookDir * distance;
				base.transform.position = position;
				base.transform.forward = _lookDir;
				if (Physics.Raycast(base.transform.position, -Vector3.up, out var hitInfo, 0.5f))
				{
					base.transform.position = hitInfo.point + Vector3.up * 0.5f;
				}
				base.transform.rotation = Quaternion.LookRotation(_lookDir, followTargetRoll ? target.up : Vector3.up);
			}
			else
			{
				base.transform.localPosition = _initialPosition + _pan;
				base.transform.rotation = Quaternion.LookRotation(_lookDir, followTargetRoll ? target.up : Vector3.up);
			}
			_prevAcceleration = _acceleration;
			_acceleration = _rbLocalAcceleration;
			_localAcceleration = Vector3.zero;
			if (target != null)
			{
				_localAcceleration = target.TransformDirection(_acceleration);
			}
			if (!_isFirstFrame)
			{
				_newPositionOffset = Vector3.SmoothDamp(_prevAcceleration, _localAcceleration, ref _accelerationChangeVelocity, shakeSmoothing) / 100f * shakeIntensity;
				_newPositionOffset = Vector3.Scale(_newPositionOffset, shakeAxisIntensity);
				_positionOffset = Vector3.SmoothDamp(_positionOffset, _newPositionOffset, ref _offsetChangeVelocity, shakeSmoothing);
				_positionOffset = Vector3.ClampMagnitude(_positionOffset, shakeMaxOffset);
				base.transform.position -= target.TransformDirection(_positionOffset) * Mathf.Clamp01(_rbSpeed * 0.5f);
			}
			_isFirstFrame = false;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(_lookAtPosition, 0.1f);
			Gizmos.DrawRay(_lookAtPosition, _lookDir);
		}

		private void OnEnable()
		{
			_isFirstFrame = true;
		}

		public float ClampAngle(float angle, float min, float max)
		{
			while (angle < -360f || angle > 360f)
			{
				if (angle < -360f)
				{
					angle += 360f;
				}
				if (angle > 360f)
				{
					angle -= 360f;
				}
			}
			return Mathf.Clamp(angle, min, max);
		}
	}
}
