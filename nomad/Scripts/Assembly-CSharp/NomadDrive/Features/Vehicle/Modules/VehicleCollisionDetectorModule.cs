using Mirror;
using NomadDrive.Features.Vehicle.Collision;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class VehicleCollisionDetectorModule : VehicleModule
	{
		[Header("Thresholds")]
		[SerializeField]
		private float minimumImpulseThreshold = 500f;

		[SerializeField]
		private float minimumSpeedThreshold = 2f;

		[Header("Cooldown")]
		[SerializeField]
		private float collisionCooldown = 0.3f;

		[Header("Severity")]
		[SerializeField]
		private float mediumForceThreshold = 3000f;

		[SerializeField]
		private float heavyForceThreshold = 8000f;

		private float _lastCollisionTime;

		private Vector3 _preCollisionVelocity;

		protected override void SubscribeEvents()
		{
			base.VehicleManager.VehicleController.onCollision.AddListener(OnVehicleCollision);
		}

		protected override void UnsubscribeEvents()
		{
			base.VehicleManager.VehicleController.onCollision.RemoveListener(OnVehicleCollision);
		}

		private void FixedUpdate()
		{
			Rigidbody vehicleRigidbody = base.VehicleManager.VehicleController.vehicleRigidbody;
			if (!(vehicleRigidbody == null))
			{
				_preCollisionVelocity = vehicleRigidbody.linearVelocity;
			}
		}

		private void OnVehicleCollision(UnityEngine.Collision collision)
		{
			if (Time.time - _lastCollisionTime < collisionCooldown || _preCollisionVelocity.magnitude < minimumSpeedThreshold || collision.collider is TerrainCollider || collision.collider.GetComponentInParent<NonDetachingCollisionMarker>() != null || collision.contactCount == 0)
			{
				return;
			}
			Rigidbody vehicleRigidbody = base.VehicleManager.VehicleController.vehicleRigidbody;
			float num = collision.relativeVelocity.magnitude * ((vehicleRigidbody != null) ? vehicleRigidbody.mass : 1500f);
			if (!(num < minimumImpulseThreshold))
			{
				_lastCollisionTime = Time.time;
				ContactPoint contact = collision.GetContact(0);
				Vector3 relativeVelocity = collision.relativeVelocity;
				float magnitude = relativeVelocity.magnitude;
				CollisionSeverity severity = ((num >= heavyForceThreshold) ? CollisionSeverity.Heavy : ((num >= mediumForceThreshold) ? CollisionSeverity.Medium : CollisionSeverity.Light));
				uint collidedObjectNetId = 0u;
				if (collision.collider.TryGetComponent<NetworkIdentity>(out var component))
				{
					collidedObjectNetId = component.netId;
				}
				VehicleCollisionData data = new VehicleCollisionData
				{
					ImpactPoint = contact.point,
					ImpactNormal = contact.normal,
					Force = num,
					RelativeVelocity = relativeVelocity,
					RelativeSpeed = magnitude,
					Severity = severity,
					CollidedObjectNetId = collidedObjectNetId
				};
				if (collision.collider.TryGetComponent<IVehicleCollisionReactable>(out var component2))
				{
					component2.OnHitByVehicle(data);
				}
				base.EventBus.FireCollisionAllClients(data);
				base.VehicleManager.NetworkSync.CmdReportCollision(data.ImpactPoint, data.ImpactNormal, data.Force, data.RelativeVelocity, data.RelativeSpeed, (byte)data.Severity, data.CollidedObjectNetId);
			}
		}
	}
}
