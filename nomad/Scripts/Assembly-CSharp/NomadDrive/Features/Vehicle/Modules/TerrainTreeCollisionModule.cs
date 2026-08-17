using NomadDrive.Features.Vehicle.Collision;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle.Modules
{
	public class TerrainTreeCollisionModule : VehicleModule
	{
		[Header("Configuration")]
		[SerializeField]
		private float collisionCooldown = 0.2f;

		[SerializeField]
		private float minimumSpeedThreshold = 3f;

		[Inject]
		private ITerrainTreeDestructionManager _treeDestructionManager;

		private float _lastCollisionTime;

		private Vector3 _preCollisionVelocity;

		private Vector3 _preCollisionAngularVelocity;

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
				_preCollisionAngularVelocity = vehicleRigidbody.angularVelocity;
			}
		}

		private void OnVehicleCollision(UnityEngine.Collision collision)
		{
			if (_treeDestructionManager == null)
			{
				return;
			}
			TerrainTreeDestructionConfig config = _treeDestructionManager.Config;
			if (config == null || Time.time - _lastCollisionTime < collisionCooldown || !(collision.collider is TerrainCollider terrainCollider) || _preCollisionVelocity.magnitude < minimumSpeedThreshold)
			{
				return;
			}
			Terrain component = terrainCollider.GetComponent<Terrain>();
			if (component == null || collision.contactCount == 0)
			{
				return;
			}
			Vector3 point = collision.GetContact(0).point;
			Vector3 relativeVelocity = collision.relativeVelocity;
			Rigidbody vehicleRigidbody = base.VehicleManager.VehicleController.vehicleRigidbody;
			float num = relativeVelocity.magnitude * ((vehicleRigidbody != null) ? vehicleRigidbody.mass : 1500f);
			if (TerrainTreeCollisionHelper.FindNearestTreeInstance(component, point, _treeDestructionManager.TreeSearchRadius, out var _, out var treeWorldPos, out var prototypeIndex))
			{
				GameObject treePrototypePrefab = TerrainTreeCollisionHelper.GetTreePrototypePrefab(component, prototypeIndex);
				if (!(treePrototypePrefab == null) && config.TryGetEntry(treePrototypePrefab, out var entry) && entry.isDestructible && !(num < entry.minimumForceToDestroy))
				{
					_lastCollisionTime = Time.time;
					_treeDestructionManager.ReportDestruction(treeWorldPos, relativeVelocity);
					Rigidbody vehicleRigidbody2 = base.VehicleManager.VehicleController.vehicleRigidbody;
					vehicleRigidbody2.linearVelocity = _preCollisionVelocity;
					vehicleRigidbody2.angularVelocity = _preCollisionAngularVelocity;
				}
			}
		}
	}
}
