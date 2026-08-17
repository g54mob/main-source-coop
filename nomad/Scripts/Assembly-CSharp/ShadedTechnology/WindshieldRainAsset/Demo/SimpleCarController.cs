using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadedTechnology.WindshieldRainAsset.Demo
{
	public class SimpleCarController : MonoBehaviour
	{
		private float horizontalInput;

		private float verticalInput;

		private float currentSteerAngle;

		private float currentBreakForce;

		private bool isBreaking;

		[SerializeField]
		private float motorForce;

		[SerializeField]
		private float breakForce;

		[SerializeField]
		private float maxSteerAngle;

		[SerializeField]
		private float idleBreakingForce = 100f;

		[SerializeField]
		private float maxRPM = 100f;

		[SerializeField]
		private WheelCollider frontLeftWheelCollider;

		[SerializeField]
		private WheelCollider frontRightWheelCollider;

		[SerializeField]
		private WheelCollider rearLeftWheelCollider;

		[SerializeField]
		private WheelCollider rearRightWheelCollider;

		[SerializeField]
		private Transform frontLeftWheelTransform;

		[SerializeField]
		private Transform frontRightWheelTransform;

		[SerializeField]
		private Transform rearLeftWheelTransform;

		[SerializeField]
		private Transform rearRightWheelTransform;

		[SerializeField]
		private float steeringWheelRotation = 270f;

		[SerializeField]
		private Transform steeringWheelTransform;

		[SerializeField]
		private Rigidbody carRigidbody;

		[SerializeField]
		private Transform centerOfMass;

		[SerializeField]
		private AnimationCurve torqueCurve;

		[SerializeField]
		private float maxSteeringStep = 0.1f;

		private Vector2 _movementVec;

		private void Start()
		{
			if ((bool)centerOfMass && (bool)carRigidbody)
			{
				carRigidbody.centerOfMass = centerOfMass.position - carRigidbody.transform.position;
			}
		}

		private void FixedUpdate()
		{
			UpdateInput();
			HandleMotor();
			HandleSteering();
			UpdateWheels();
		}

		private void UpdateInput()
		{
			float num = Mathf.Min(maxSteeringStep, Mathf.Abs(horizontalInput - _movementVec.x));
			horizontalInput += ((horizontalInput > _movementVec.x) ? (0f - num) : num);
			horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
			verticalInput = 0f - _movementVec.y;
		}

		public void OnMoveInput(InputAction.CallbackContext context)
		{
			_movementVec = context.ReadValue<Vector2>();
		}

		public void OnBreakInput(InputAction.CallbackContext context)
		{
			isBreaking = context.ReadValueAsButton();
		}

		private void HandleMotor()
		{
			float num = Mathf.Max(0f, (verticalInput > 0f) ? frontRightWheelCollider.rpm : (0f - frontRightWheelCollider.rpm));
			float num2 = Mathf.Max(0f, (verticalInput > 0f) ? frontLeftWheelCollider.rpm : (0f - frontLeftWheelCollider.rpm));
			frontRightWheelCollider.motorTorque = verticalInput * motorForce * torqueCurve.Evaluate(Mathf.Clamp01(num / maxRPM));
			frontLeftWheelCollider.motorTorque = verticalInput * motorForce * torqueCurve.Evaluate(Mathf.Clamp01(num2 / maxRPM));
			if (isBreaking)
			{
				currentBreakForce = breakForce;
			}
			else
			{
				currentBreakForce = ((Mathf.Abs(verticalInput) < 0.1f) ? idleBreakingForce : 0f);
			}
			ApplyBreaking();
		}

		private void ApplyBreaking()
		{
			frontRightWheelCollider.brakeTorque = currentBreakForce;
			frontLeftWheelCollider.brakeTorque = currentBreakForce;
			rearLeftWheelCollider.brakeTorque = currentBreakForce;
			rearRightWheelCollider.brakeTorque = currentBreakForce;
		}

		private void HandleSteering()
		{
			currentSteerAngle = maxSteerAngle * horizontalInput;
			frontLeftWheelCollider.steerAngle = currentSteerAngle;
			frontRightWheelCollider.steerAngle = currentSteerAngle;
			if ((bool)steeringWheelTransform)
			{
				steeringWheelTransform.localRotation = Quaternion.AngleAxis(horizontalInput * steeringWheelRotation, Vector3.forward);
			}
		}

		private void UpdateWheels()
		{
			UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
			UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
			UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
			UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
		}

		private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
		{
			wheelCollider.GetWorldPose(out var pos, out var quat);
			wheelTransform.rotation = quat;
			wheelTransform.position = pos;
		}
	}
}
