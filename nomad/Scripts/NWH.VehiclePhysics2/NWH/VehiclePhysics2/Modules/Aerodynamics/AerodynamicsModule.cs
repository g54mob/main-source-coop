using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWH.VehiclePhysics2.Modules.Aerodynamics
{
	[Serializable]
	public class AerodynamicsModule : VehicleComponent
	{
		public const float RHO = 1.225f;

		public Vector3 dimensions = new Vector3(2f, 1.5f, 4.5f);

		[Range(0f, 5f)]
		[Tooltip("The amount of drag that will be added when the vehicle is fully damaged.\r\n0.5 equals +50% on top of the original, undamaged, drag value.")]
		public float damageDragEffect = 0.5f;

		[Tooltip("Points at which downforce will be applied.\r\nAvoid applying force at too high positions as that will negatively influence suspension and steering.")]
		public List<DownforcePoint> downforcePoints = new List<DownforcePoint>();

		[Range(0f, 1f)]
		[Tooltip("    Coefficient of drag of the vehicle's frontal profile.\r\n    Also used for reverse.")]
		public float frontalCd = 0.35f;

		[Tooltip("Speed in [m/s] at which the downforce will reach it's maximum value\r\nassigned under downforce points settings.")]
		public float maxDownforceSpeed = 80f;

		[Range(0f, 2f)]
		[Tooltip("    Coefficient of drag of the vehicle's side profile.")]
		public float sideCd = 1.05f;

		[Tooltip("    Should downforce be calculated?")]
		public bool simulateDownforce;

		[Tooltip("    Should drag be calculated?")]
		public bool simulateDrag = true;

		private float _forwardSpeed;

		private float _frontalArea;

		private float _sideArea;

		private float _sideSpeed;

		[SerializeField]
		private float lateralDragForce;

		[SerializeField]
		private float longitudinalDragForce;

		public override void VC_FixedUpdate()
		{
			base.VC_FixedUpdate();
			if (vehicleController.Speed < 1f)
			{
				longitudinalDragForce = 0f;
				lateralDragForce = 0f;
				return;
			}
			if (simulateDrag)
			{
				_frontalArea = dimensions.x * dimensions.y * 0.85f;
				_sideArea = dimensions.y * dimensions.z * 0.8f;
				_forwardSpeed = vehicleController.LocalVelocity.z;
				_sideSpeed = vehicleController.LocalVelocity.x;
				longitudinalDragForce = 0.6125f * _frontalArea * frontalCd * (_forwardSpeed * _forwardSpeed) * ((_forwardSpeed > 0f) ? (-1f) : 1f);
				lateralDragForce = 0.6125f * _sideArea * sideCd * (_sideSpeed * _sideSpeed) * ((_sideSpeed > 0f) ? (-1f) : 1f);
				vehicleController.vehicleRigidbody.AddRelativeForce(new Vector3(lateralDragForce, 0f, longitudinalDragForce));
			}
			if (!simulateDownforce)
			{
				return;
			}
			float f = vehicleController.Speed / maxDownforceSpeed;
			float num = 1f - (1f - Mathf.Pow(f, 2f));
			foreach (DownforcePoint downforcePoint in downforcePoints)
			{
				vehicleController.vehicleRigidbody.AddForceAtPosition(num * downforcePoint.maxForce * -vehicleController.transform.up, vehicleController.transform.TransformPoint(downforcePoint.position));
			}
		}

		public override void VC_DrawGizmos()
		{
			foreach (DownforcePoint downforcePoint in downforcePoints)
			{
				Gizmos.color = Color.red;
				vehicleController.transform.TransformPoint(downforcePoint.position);
				Gizmos.DrawSphere(vehicleController.transform.TransformPoint(downforcePoint.position), 0.1f);
			}
		}
	}
}
