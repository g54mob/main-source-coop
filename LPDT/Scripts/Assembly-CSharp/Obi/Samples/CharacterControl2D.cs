using UnityEngine;

namespace Obi.Samples
{
	public class CharacterControl2D : MonoBehaviour
	{
		public float floorRaycastDistance = 1.2f;

		[Header("Grounded")]
		public float acceleration = 80f;

		public float maxSpeed = 6f;

		public float damping = 0.005f;

		public float jumpPower = 10f;

		[Header("Airborne")]
		public float airAcceleration = 16f;

		public float airMaxSpeed = 12f;

		public float extraGravity = -12f;

		[Header("Auto upright")]
		public Vector3 centerOfMass = new Vector3(0f, -0.25f, 0f);

		public float P = 2f;

		public float D = 0.1f;

		private Rigidbody unityRigidbody;

		private float axis;

		private bool grounded;

		private float error;

		private float prevError;

		public void Awake()
		{
			unityRigidbody = GetComponent<Rigidbody>();
			unityRigidbody.centerOfMass = centerOfMass;
		}

		private void Update()
		{
			axis = Input.GetAxisRaw("Horizontal");
			grounded = Physics.Raycast(new Ray(base.transform.position, -Vector3.up), floorRaycastDistance);
			if (Input.GetButtonDown("Jump") && grounded)
			{
				unityRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
			}
		}

		private void FixedUpdate()
		{
			Vector3 linearVelocity = unityRigidbody.linearVelocity;
			prevError = error;
			error = Vector3.SignedAngle(unityRigidbody.transform.up, Vector3.up, Vector3.forward);
			if (grounded)
			{
				float num = axis * acceleration * Time.deltaTime;
				if ((linearVelocity.x < maxSpeed && num > 0f) || (linearVelocity.x > 0f - maxSpeed && num < 0f))
				{
					linearVelocity.x += num;
				}
				if (Mathf.Approximately(axis, 0f))
				{
					linearVelocity.x *= Mathf.Pow(damping, Time.deltaTime);
				}
				unityRigidbody.AddTorque(new Vector3(0f, 0f, error * P + (error - prevError) / Time.deltaTime * D));
			}
			else
			{
				float num2 = axis * airAcceleration * Time.deltaTime;
				if ((linearVelocity.x < airMaxSpeed && num2 > 0f) || (linearVelocity.x > 0f - airMaxSpeed && num2 < 0f))
				{
					linearVelocity.x += num2;
				}
				linearVelocity.y += extraGravity * Time.deltaTime;
			}
			unityRigidbody.linearVelocity = linearVelocity;
		}
	}
}
